using System;
using System.Collections.Generic;
using System.Linq;
using HTB.Database;
using System.Collections;
using HTBReports;
using HTBUtilities;
using HTBAktLayer;
using HTBInvoiceManager;
using System.Reflection;
using HTB.Database.Views;
using HTBServices;
using HTBServices.Mail;

namespace HTBDailyKosten
{
    public class KostenCalculator
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly InvoiceManager _invMgr = new InvoiceManager();
        private readonly tblControl _control = HTBUtils.GetControlRecord();
        private bool _hasAktWorkflow;

        // Constructor used for testing
        public KostenCalculator(IEnumerable<int> aktId)
        {
            foreach (var id in aktId)
            {
                Log.Info("==============   NEW  ================");
                CalculateKosten(id);
                Log.Info("============== END NEW ===============");
            }
        }

        public KostenCalculator()
        {
        }

        public void CalculateKosten()
        {
            // Do NOT process King Bill Cases
            // string qry = "SELECT * FROM qryCustInkAkt WHERE CustInkAktIsPartial = 0 AND CustInkAktIsWflStopped = 0 AND CustInkAktSource <> 'King Bill' AND CustInkAktStatus NOT IN (" + _control.InkassoAktInterventionStatus + ", " + _control.InkassoAktMeldeStatus + ", " + _control.InkassoAktStornoStatus + ", " + _control.InkassoAktFertigStatus + ", " + _control.InkassoAktWflDoneStatus + ", " + _control.InkassoAktSentToPartnerStatus + " ) AND CustInkAktCurrentStep >= 0 AND CustInkAktNextWFLStep <= '" + DateTime.Now + "' ORDER BY CustInkAktCurrentStep ASC";
            //            Process King Bill Cases

            string qry = "SELECT * FROM qryCustInkAkt WHERE CustInkAktIsPartial = 0 AND CustInkAktIsWflStopped = 0 AND CustInkAktStatus NOT IN (" +
                _control.InkassoAktInterventionStatus + ", " + 
                _control.InkassoAktMeldeStatus + ", " + 
                _control.InkassoAktStornoStatus + ", " + 
                _control.InkassoAktFertigStatus + ", " + 
                _control.InkassoAktWflDoneStatus + ", " + 
                _control.InkassoAktSentToPartnerStatus + 
                " ) AND CustInkAktCurrentStep >= 0 AND CustInkAktNextWFLStep <= '" + DateTime.Now + "' ORDER BY CustInkAktCurrentStep ASC";

            ArrayList custInAktProcessList = HTBUtils.GetSqlRecords(qry, typeof(tblCustInkAkt));
            Log.Info(qry);
            foreach (tblCustInkAkt custInkAkt in custInAktProcessList)
            {
                // Just to make sure the database did not take a crap
                if (!custInkAkt.CustInkAktIsWflStopped &&
                    custInkAkt.CustInkAktStatus != _control.InkassoAktInterventionStatus &&
                    custInkAkt.CustInkAktStatus != _control.InkassoAktMeldeStatus &&
                    custInkAkt.CustInkAktStatus != _control.InkassoAktStornoStatus)
                {
                    if (custInkAkt.CustInkAktStatus == _control.InkassoAktInkassoStatus && (custInkAkt.CustInkAktCurStatus == _control.InkassoAktInstallmentStatus || custInkAkt.CustInkAktCurStatus == _control.InkassoAktOverchargeStatus))
                    {
                        // Skip Installments (RA) and Overcharges (TVL)
                    }
                    else
                    {
                        CalculateKosten(custInkAkt.CustInkAktID);
                    }
                }
            }
        }

        private void CalculateKosten(int aktId)
        {
            tblCustInkAkt akt = HTBUtils.GetInkassoAkt(aktId);
            
            if (akt == null)return;
            if(new AktUtils(aktId).GetAktBalance() <= 0) return;

            if (akt.CustInkAktStatus == _control.InkassoAktWaitingForReMeldeStatus)
            {
                CreateMeldeAkt(aktId);
            }
            else
            {
                #region Normal Costs Generation
                akt.CustInkAktCurrentStep += 1;
                tblWFA wfa = GetWFA(akt.CustInkAktCurrentStep, aktId, akt.CustInkAktKlient);

                if (wfa == null) return;
                

                var aktActionKostenList = GetAktAktionKosten(akt.CustInkAktCurrentStep, aktId, akt.CustInkAktKlient);
                var mahnungSet = new tblMahnungSet();
                
                // Fälligkeitsdatum Schuldner Mahnungsdok (WFL Datum -5 Tage)
                DateTime dueDte = DateTime.Now.AddDays(wfa.WFPPreTime - 5);
                DateTime endZinsenDte = DateTime.Now.AddDays(wfa.WFPPreTime);

                var inv = _invMgr.GetInvoiceByAktAndType(aktId, tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL);
                decimal kapital = inv.InvoiceAmount > 0 ? Convert.ToDecimal(inv.InvoiceAmount) : 0;
                
                inv = _invMgr.GetInvoiceByAktAndType(aktId, tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST);
                decimal klientKosten = inv != null && inv.InvoiceAmount > 0 ? Convert.ToDecimal(inv.InvoiceAmount) : 0;
                
                Log.Info("Total Client Kosten " + klientKosten);
                decimal forderung = kapital + klientKosten;
                Log.Info("Total Forderung: " + forderung);

                // Load Kosten Art Ids for next step
                var kostenArtIdList = (from qryAktAktionKosten aktActionKosten in aktActionKostenList select aktActionKosten.KostenArtID).ToList();

                #region Calculate WKF (Current Step) Kosten
                var wset = new qryKostenSet();
                wset.LoadKostenBasedOnForderungAndArtId(forderung, kostenArtIdList);
                foreach (qryKosten record in wset.qryKostenList)
                {
                    decimal amount = HTBUtils.GetCalculatedKost(forderung, record, akt.CustInkAktInvoiceDate); // calc based on balance
                    Log.Info("[ID: " + record.KostenArtId + "]  [Cost: " + record.KostenArtText.Trim() + "]  [Send Mahnung: " + record.SendMahnung + "]  [Amount: " + amount + "] [IsKlage: " + record.IsKlage + "] [IsKlageErinnerung: " + record.IsKlageErinnerung + "]");
                    if (record.IsTaxable)
                    {
                    }
                    if (amount > 0)
                    {
                        CreateInvoiceRecord(aktId, record, amount);
                    }
                    if (record.SendMahnung)
                    {
                        tblMahnung mahnung = mahnungSet.GetNewMahnung();
                        mahnung.MahnungAktID = akt.CustInkAktID;
                        mahnungSet.UpdateMahnung(mahnung);
                    }
                    if (record.IsKlage || record.IsKlageErinnerung)
                    {
                        var mail = ServiceFactory.Instance.GetService<IHTBEmail>();
                        if (record.IsKlageErinnerung)
                        {
                            mail.SendLawyerReminder(HTBUtils.GetInkassoAktQry(aktId));
                        }
                        if (record.IsKlage)
                        {
                            new AktUtils(aktId).SendInkassoPackageToLawyer(new HTBEmailAttachment(ReportFactory.GetZwischenbericht(aktId), "Zwischenbericht.pdf", "Application/pdf"));
                        }
                    }
                    // Change inkasso akt to indicate current status
                    if (record.KostenCustInkAktStatus >= 0)
                    {
                        akt.CustInkAktStatus = record.KostenCustInkAktStatus;
                        if (akt.CustInkAktStatus == _control.InkassoAktInterventionStatus)
                        {
                            var aktUtils = new AktUtils(akt.CustInkAktID);
                            int workPeriod = 0;
                            tblWFA workflowNext = GetWFA(wfa.WFPPosition+1, aktId, akt.CustInkAktKlient);
                            if (workflowNext != null)
                            {
                                workPeriod = workflowNext.WFPPreTime;
                            }
                            aktUtils.CreateNewInterventionAkt((qryCustInkAkt)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryCustInkAkt WHERE CustInkAktID = " + akt.CustInkAktID, typeof(qryCustInkAkt)), akt.CustInkAktMemo, workPeriod);
                        }
                    }
                    if (record.KostenCustInkAktCurStatus >= 0)
                    {
                        akt.CustInkAktCurStatus = record.KostenCustInkAktCurStatus;
                    }
                    if (record.KostenKZID > 0)
                    {
                        var aktUtils = new AktUtils(akt.CustInkAktID);
                        aktUtils.CreateAktAktion(record.KostenKZID, _control.AutoUserId);
                    }
                    Log.Info(record.KostenArtText.Trim() + " " + amount);
                }
                #endregion

                #region Calculate General Costs (Skip them if indicated)
                if (!akt.CustInkAktSkipInitialInvoices && akt.CustInkAktCurrentStep == 1)
                {
                    // first step calculate Costs that take place once (evidenzgebuhren, ratenangebott...)
                    wset.LoadInitialKostenBasedOnForderung(forderung);
                    foreach (qryKosten record in wset.qryKostenList)
                    {
                        decimal amount = HTBUtils.GetCalculatedKost(forderung, record, akt.CustInkAktInvoiceDate); // calc based on balance
                        if (record.IsTaxable)
                        {
                        }
                        if (amount > 0)
                        {
                            CreateInvoiceRecord(aktId, record, amount);
                        }
                        Log.Info(record.KostenArtText.Trim() + "  " + amount);
                    }
                }
                #endregion

                #region Create and Insert CustInkAktDok
                RecordSet.Insert(new tblCustInkAktDok
                              {
                                  CustInkAktDokAkt = aktId, 
                                  CustInkAktDokDate = DateTime.Now, 
                                  CustInkAktDokDelFlag = 0, 
                                  CustInkAktDokDueDate = dueDte, 
                                  CustInkAktNextWorkflowStep = endZinsenDte, 
                                  CustInkAktDokCapital = Convert.ToDouble(forderung), 
                                  CustInkAktDokType = 1, 
                                  CustInkAktDokPrintFlag = 0,
                                  CustInkAktDokEnterUser = 0,
                                  CustInkAktDokCostClient = akt.CustInkAktKlient
                              });
                #endregion

                #region Update akt
                wfa = GetWFA(akt.CustInkAktCurrentStep + 1, aktId, akt.CustInkAktKlient);
                if (wfa == null)
                {
                    SetWFLStatusDone(akt);
                }
                else
                {
                    akt.CustInkAktNextWFLStep = DateTime.Now.AddDays(wfa.WFPPreTime);
                }
                UpdateAktStatus(akt);
                #endregion

                #endregion
            }
            return;
        }

        public void GenerateNewMahnung(int aktId)
        {
            tblCustInkAkt akt = HTBUtils.GetInkassoAkt(aktId);
            var aktUtl = new AktUtils(aktId);
            var mahnungSet = new tblMahnungSet();
            int mahnungNumber = MahnungManager.GetNextMahnungNumber(aktId);

            // Fälligkeitsdatum Schuldner Mahnungsdok (WFL Datum -5 Tage)
            decimal forderung = Convert.ToDecimal(aktUtl.GetAktOriginalInvoiceAmount());

            List<int> kostenArtIdList = GetKostenArtIdForMahnung(mahnungNumber);
            
            #region Create Mahnung
            var wset = new qryKostenSet();
            wset.LoadKostenBasedOnForderungAndArtId(forderung, kostenArtIdList);
            foreach (qryKosten record in wset.qryKostenList)
            {
                decimal amount = HTBUtils.GetCalculatedKost(forderung, record, akt.CustInkAktInvoiceDate);
                Log.Info("[Mahnung Cost: " + record.KostenArtText + "]  [Send Mahnung: " + record.SendMahnung + "]  [Amount: " + amount + "]");
                if (amount > 0)
                {
                    CreateInvoiceRecord(aktId, record, amount);
                }
                tblMahnung mahnung = mahnungSet.GetNewMahnung();
                mahnung.MahnungAktID = akt.CustInkAktID;
                mahnungSet.UpdateMahnung(mahnung);
                if (record.KostenKZID > 0)
                {
                    var aktUtils = new AktUtils(akt.CustInkAktID);
                    aktUtils.CreateAktAktion(record.KostenKZID, _control.AutoUserId);
                }
                Log.Info(record.KostenArtText.Trim() + " " + amount);
            }
            #endregion
        }

        private void CreateMeldeAkt(int aktId)
        {
            var aktUtils = new AktUtils(aktId);
            qryCustInkAkt qryInkAkt = HTBUtils.GetInkassoAktQry(aktId);

            int meldeAktId = aktUtils.CreateMeldeAkt(HTBUtils.GetInkassoAktQry(aktId));

            int meldeKostenArtId = _control.MeldeKostenArtId;
            if (!qryInkAkt.GegnerZipPrefix.Trim().ToUpper().Equals("") && !qryInkAkt.GegnerZipPrefix.Trim().ToUpper().Equals("A"))
                meldeKostenArtId = aktUtils.control.MeldeKostenAuslandArtId;

            aktUtils.SetInkassoStatusBasedOnWflAction(meldeKostenArtId, _control.AutoUserId, null, "", _control.MeldePeriod);

            Log.Info("MELDE CREATED");
            ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail(new string[] { HTBUtils.GetConfigValue("Melde_Email"), HTBUtils.GetConfigValue("Default_EMail_Addr") }, "Neu Meldeakt: " + meldeAktId + " Akt: " + aktId, "Melde: " + meldeAktId + " InkassoAkt: " + aktId);
        }

        private void SetWFLStatusDone(tblCustInkAkt akt)
        {
            var aktUtils = new AktUtils(akt.CustInkAktID);
//            akt.CustInkAktStatus = _control.InkassoAktWflDoneStatus;
//            akt.CustInkAktCurStatus = 68; // Workflow Fertig
            akt.CustInkAktCurrentStep = -1; // DONE !!!
            
            if(HTBUtils.IsZero(aktUtils.GetAktBalance()))
            {
                akt.CustInkAktStatus = _control.InkassoAktFertigStatus;
                akt.CustInkAktCurStatus = 25; // Erledigt: Fal abgeschlossen
            }
            else
            {
                int stornoCode = 27;    // tblKZ: Storno auf Ihren Wunsch (kostenfrei für Sie)
                if(HTBUtils.IsZero(aktUtils.GetAktKlientTotalBalance()))
                {
                    stornoCode = 31; // tblKZ: Forderung direkt an Sie bezahlt (Kein Kosteneingang)
                }
                else if (aktUtils.IsLaywerInWorkflow())
                {
                    stornoCode = 32; // tblKZ: Storno - Uneinbringlich (gerichtliche Betreibung)
                }
                aktUtils.CreateAktAktion(stornoCode, _control.AutoUserId);
                akt.CustInkAktCurStatus = stornoCode; 
                akt.CustInkAktStatus = _control.InkassoAktStornoStatus;
            }

            RecordSet.Update(akt);
            Log.Info("WFL DONE");
            
            // Notify Client (copy office)
//            new AktUtils(akt.CustInkAktID).SendAbschlusBerichtToClient(akt, ReportFactory.GetZwischenbericht(akt.CustInkAktID), "Klient_Akt_Done_Text", string.Format("Abschlussbericht  [ {0} ]", string.IsNullOrEmpty(akt.CustInkAktKunde) ? akt.CustInkAktID.ToString() : akt.CustInkAktKunde));
        }

        #region DB I/O Utility Methods

        private tblWFA GetWFA(int wfpPosition, int aktId, int klientId)
        {
            _hasAktWorkflow = false;
            tblWFA ret = null;
            ArrayList wfaList = HTBUtils.GetSqlRecords("SELECT * FROM tblWFA WHERE WFPAkt = " + aktId, typeof(tblWFA));
            if (wfaList.Count > 0)
            {
                foreach (tblWFA wfa in wfaList)
                {
                    if (wfa.WFPPosition == wfpPosition)
                    {
                        _hasAktWorkflow = true;
                        return wfa;
                    }
                }
            }
            else // Get Akt Work-Flow from Client
            {
                var wfk = (tblWFK)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblWFK WHERE WFPPosition = " + wfpPosition + " AND WFPKlient = " + klientId, typeof(tblWFK));
                if (wfk != null)
                {
                    ret = new tblWFA();
                    ret.Assign(wfk);
                }
            }
            return ret;
        }

        private ArrayList GetAktAktionKosten(int wfpPosition, int aktId, int klientId)
        {
            if (_hasAktWorkflow)
                return HTBUtils.GetSqlRecords("SELECT * FROM qryAktAktionKosten WHERE WFPPosition = " + wfpPosition + " AND WFPAkt = " + aktId, typeof(qryAktAktionKosten));
            return HTBUtils.GetSqlRecords("SELECT * FROM qryAktAktionKostenKlient WHERE WFPPosition = " + wfpPosition +" AND WFPKlient = "+klientId, typeof(qryAktAktionKosten));
        }

        private List<int> GetKostenArtIdForMahnung(int mahnungNumber)
        {
            return (from tblMahnungKostenArtId kostArtId in HTBUtils.GetSqlRecords("SELECT * FROM tblMahnungKostenArtId WHERE MahKostMahnungNummer = " + mahnungNumber, typeof (tblMahnungKostenArtId)) select kostArtId.MahKostArtId).ToList();
        }

        private void UpdateAktStatus(tblCustInkAkt akt)
        {
            akt.CustInkAktLastChange = DateTime.Now;
            RecordSet.Update(akt);
        }
        private void CreateInvoiceRecord(int aktID, qryKosten record, decimal amount)
        {
            int invType = tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE; // collection 'inkasso' cost
            if (record.KostenInvoiceType > 0)
            {
                invType = record.KostenInvoiceType;
            }
            _invMgr.CreateAndSaveInvoice(aktID, invType, Convert.ToDouble(amount), record.KostenArtText, true);
        }
        #endregion
        
    }
}
