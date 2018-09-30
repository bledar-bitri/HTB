using System;
using System.Collections;
using HTB.Database;
using HTBExtras;
using HTBUtilities;
using System.Reflection;
using HTB.Database.Views;

namespace HTBAktLayer
{
    public class AktInterventionUtils
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly AktUtils _aktUtils;
        
        #region properties
        private tblAktenInt _intAkt;
        public tblAktenInt IntAkt
        {
            get { return _intAkt; }
            set { _intAkt = value; }
        }
        #endregion

        public AktInterventionUtils(AktUtils aktU)
        {
            _aktUtils = aktU;
            IntAkt = (tblAktenInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenInt WHERE AktIntCustInkAktID = " + aktU.InkAktId + " ORDER BY AktIntDatum DESC", typeof(tblAktenInt));
        }
        public tblAktenInt CreateNewInterventionAkt(qryCustInkAkt custInkAkt, string memo, int allowedWorkDays, int aktType = 1)
        {
            log.Info("Creating New Intervention for Akt " + custInkAkt.CustInkAktID);
            tblAktenInt aktInt = GetCreateNewInterventionAkt(custInkAkt, memo, allowedWorkDays, aktType);
            double balance = _aktUtils.InkAktUtils.GetAktOriginalInvoiceBalance();
            if (balance > 0)
                _aktUtils.InsertAktenIntPos(aktInt.AktIntID, balance, "Ursprüngl. Forderungen ");

            balance = _aktUtils.InkAktUtils.GetAktKlientCostsBalance();
            if (balance > 0)
                _aktUtils.InsertAktenIntPos(aktInt.AktIntID, balance, "Spesen Auftraggeber  ");

            /*
            balance = GlobalDataArea.GetAktTotalCollectionBalance(aktId);
            if (balance > 0)
                InsertAktenIntPos(interventionAkt.AktIntID, balance, "CollectionInvoice Kosten ");
            */
            
            HTBUtils.NotifySBAboutNewAkt(aktInt.AktIntID);

            return aktInt;
        }
        private tblAktenInt GetCreateNewInterventionAkt(qryCustInkAkt custInkAkt, string memo, int allowedWorkDays, int aktType = 1)
        {
            var set = new RecordSet();
            double balance = _aktUtils.InkAktUtils.GetAktBalance();
            double interest = _aktUtils.InkAktUtils.GetAktTotalInterest();
            double collection = _aktUtils.InkAktUtils.GetAktTotalCollectionBalance();
            int distance = (int)_aktUtils.InkAktUtils.GetDistance(custInkAkt.GegnerLastZip);
            double weggebuehr = _aktUtils.InkAktUtils.GetWeggebuehr(distance);
            //aktUtils.CreateWeggebuer(custInkAkt.CustInkAktID, weggebuehr);

            var intAkt = new tblAktenInt();

            intAkt.AktIntAZ = custInkAkt.CustInkAktAZ.Trim() != string.Empty ? custInkAkt.CustInkAktAZ : custInkAkt.CustInkAktID.ToString();

            intAkt.AktIntAuftraggeber = custInkAkt.CustInkAktAuftraggeber;
            intAkt.AktIntKlient = custInkAkt.KlientOldID;
            intAkt.AktIntGegner = custInkAkt.GegnerOldID;
            intAkt.AktIntDatum = DateTime.Now;
            intAkt.AktIntStatus = 1;
            intAkt.AktIntTermin = DateTime.Now.AddDays(allowedWorkDays);
            intAkt.AktIntTerminAD = DateTime.Now.AddDays(allowedWorkDays);
            //intAkt.AktIntWeggebuehr = weggebuehr;
            intAkt.AktIntIZ = "";
            intAkt.AktIntSB = HTBUtils.GetGegnerSB(custInkAkt);
            intAkt.AktIntOldID = custInkAkt.CustInkAktAZ;
            intAkt.AKTIntMemo = memo;
            intAkt.AKTIntDistance = distance;
            intAkt.AKTIntKosten = collection;
            intAkt.AKTIntZinsen = 0;
            intAkt.AKTIntZinsenBetrag = interest;
            intAkt.AKTIntRVStartDate = new DateTime(1900, 1, 1);
            intAkt.AKTIntAGSB = "Thomas Jaky";
            intAkt.AKTIntKSVEMail = "office@ecp.or.at";
            //intAkt.AKTIntRVAmmount;
            //intAkt.AKTIntRVNoMonth;
            //intAkt.AKTIntRVInkassoType;
            //intAkt.AKTIntRVIntervallDay;
            //intAkt.AKTIntAGSB;
            //intAkt.AKTIntDruckkennz;
            //intAkt.AKTIntKSVAuftragsart;
            //intAkt.AKTIntKSVEMail;
            //intAkt.AKTIntVerrechnet;
            intAkt.AKTIntDownloadDatum = new DateTime(1900, 1, 1);
            //intAkt.AKTIntDub;
            //intAkt.AKTIntKommerz;
            //intAkt.AKTIntGrosskunde;
            //intAkt.AKTIntVormerk;
            //intAkt.AKTIntRVInfo;
            intAkt.AktIntAktType = aktType;
            intAkt.AktIntCustInkAktID = custInkAkt.CustInkAktID;
            intAkt.AktIntSBAssignDate = DateTime.Now;
            intAkt.AktIntOverdueNotifiedDate = new DateTime(1900, 1, 1);

            log.Info("Inserting Intervention " + custInkAkt.CustInkAktID);
            set.InsertRecord(intAkt);
            //Thread.Sleep(10000);
 
            tblCustInkAkt tblCustInkAkt = new tblCustInkAkt();
            tblCustInkAkt.Assign(custInkAkt);
            tblCustInkAkt.CustInkAktStatus = 2; // Intervention 
            tblCustInkAkt.CustInkAktCurStatus = 6; // Intervention 
            log.Info("Updating CollectionInvoice Status: to [2] and [6]");
            set.UpdateRecord(tblCustInkAkt);
            //Thread.Sleep(10000);
 
            log.Info("Getting Intervention Number " + custInkAkt.CustInkAktID);
            tblAktenInt aktInt = (tblAktenInt)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblAktenInt ORDER BY AktIntID DESC", typeof(tblAktenInt));
            if (aktInt != null)
            {
                log.Info("Intervention Number: " + aktInt.AktIntID);
                tblInkIntAktRel inkIntRel = new tblInkIntAktRel();
                inkIntRel.InkAktID = tblCustInkAkt.CustInkAktID;
                inkIntRel.IntAktID = aktInt.AktIntID;
                log.Info("Inserting Relation Record " + custInkAkt.CustInkAktID);
                set.InsertRecord(inkIntRel);
                //Thread.Sleep(10000);
            }
            log.Info("Intervention DONE !!!!");
            //Thread.Sleep(60 * 1000);
            //log.Info("Resuming ");
            //Thread.Sleep(5000);
            return aktInt;
        }

        public void CreateAktion(int type, string memo = "")
        {
            

            var aktType = (tblAktenIntActionType)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntActionType WHERE AktIntActionTypeID = " + type, typeof(tblAktenIntActionType));

            if (aktType != null)
            {
                RecordSet.Insert(new tblAktenIntAction
                                     {
                                         AktIntActionDate = DateTime.Now,
                                         AktIntActionTime = DateTime.Now,
                                         AktIntActionMemo = memo,
                                         AktIntActionAkt = IntAkt.AktIntID,
                                         AktIntActionSB = _aktUtils.control.AutoUserId,
                                         AktIntActionType = aktType.AktIntActionTypeID,
                                         AktIntActionProvision = 0,
                                         AktIntActionHonorar = 0,
                                         AktIntActionPrice = 0
                                     });
            }
        }

        public static AktIntAmounts GetAktAmounts(qryAktenInt akt, bool includePosNr = true)
        {
            var amounts = new AktIntAmounts();
            if (akt.IsInkasso())
            {
                ArrayList invList = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceStatus <> -1 AND InvoiceCustInkAktId = " + akt.AktIntCustInkAktID + " ORDER BY InvoiceDate", typeof(tblCustInkAktInvoice));
                foreach (tblCustInkAktInvoice inv in invList)
                {
                    if (inv.IsPayment())
                    {
                        amounts.Zahlungen -= inv.InvoiceAmount;
                    }
                    else if (inv.IsInterest() || inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST)
                    {
                        amounts.Zinsen += inv.InvoiceAmount;
                    }
                    else
                    {
                        switch (inv.InvoiceType)
                        {
                            case tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL:
                                if (amounts.ForderungList.Count > 0)
                                {
                                    var pf = (AktIntForderungPrintLine)amounts.ForderungList[0];
                                    pf.Amount += inv.InvoiceAmount;
                                }
                                else
                                {
                                    var pf = new AktIntForderungPrintLine
                                    {
                                        Text = "Forderungen",
                                        Amount = inv.InvoiceAmount
                                    };
                                    amounts.ForderungList.Add(pf);
                                }
                                break;
                            case tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_PERSONAL_INTERVENTION:
                                amounts.Weggebuhr += inv.InvoiceAmountNetto;
                                amounts.MWS += inv.InvoiceTax;
                                break;
                            default:
                                amounts.InkassoKosten += inv.InvoiceAmountNetto;
                                amounts.MWS += inv.InvoiceTax;
                                break;
                        }
                    }
                }
            }
            else
            {
                ArrayList posList = HTBUtils.GetPosList(akt.AktIntID);
                foreach (tblAktenIntPos pos in posList)
                {
                    var pf = new AktIntForderungPrintLine
                    {
                        Text = includePosNr ? pos.AktIntPosNr + "&nbsp;<br/>" + pos.AktIntPosCaption : pos.AktIntPosCaption,
                        Amount = pos.AktIntPosBetrag
                    };
                    amounts.ForderungList.Add(pf);
                }
                amounts.Zinsen = akt.AKTIntZinsenBetrag;
                amounts.Weggebuhr = akt.AktIntWeggebuehr;
                amounts.InkassoKosten = akt.AKTIntKosten / (1 + (HTBUtils.GetControlRecord().TaxRate / 100)); // AKTIntKosten includes Tax [formula: Net = Gross / (1 + Tax Rate)]
                amounts.MWS = akt.AKTIntKosten - amounts.InkassoKosten;
                
            }
            return amounts;
        }
    }
}
