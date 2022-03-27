using System;
using System.Collections.Generic;
using System.Text;
using HTB.Database;
using HTBReports;
using HTBUtilities;
using HTBInvoiceManager;
using HTBAktLayer;

using System.Collections;
using System.Reflection;
using HTB.Database.Views;
using HTBServices.Mail;
using HTBServices;

namespace HTBDailyKosten
{
    public class NextStepManager
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly tblControl _control = HTBUtils.GetControlRecord();
        
        #region Intervention
        
        public void GenerateIntNextStep()
        {
            #region Completed
            ArrayList interventionCompletedList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenInt WHERE AktIntStatus = 4 and AktIntCustInkAktID > 0 and AktIntProcessCode <> " + _control.ProcessCodeDone, typeof(tblAktenInt));
            foreach (tblAktenInt aktInt in interventionCompletedList)
            {
                bool ok = true;
                tblCustInkAkt ink = null;
                if (aktInt.IsInkasso())
                {
                    ink = HTBUtils.GetInkassoAkt(aktInt.AktIntCustInkAktID);
                    if (ink == null ||
                        ink.CustInkAktIsWflStopped ||
                        ink.CustInkAktStatus == _control.InkassoAktStornoStatus)
                    {
                        ok = false;
                    }
                }
                if (!ok)
                    continue;
                var action = (qryAktenIntAktionen) HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenIntAktionen WHERE AktIntActionAkt = " + aktInt.AktIntID + " ORDER BY AktIntActionDate DESC", typeof (qryAktenIntAktionen));
                if (ink != null && ink.CustInkAktCurStatus == _control.InkassoAktInstallmentStatus)
                {
                    // there is an existing Installment Plan so just set the status to done and move on
                    SetIntAktProcessDone(aktInt);
                }
                else
                {
                    if (aktInt.AktIntProcessCode == _control.ProcessCodeInstallment)
                    {
                        ProcessIntInstallment(aktInt, action, ink);
                    }
                    else
                    {
                        if (action != null)
                        {
                            Log.Info("[Ink: " + action.AktIntCustInkAktID + "]  [Int: " + action.AktIntActionAkt + "]  [ActionType: " + action.AktIntActionType + "]  [ActionDate:" + action.AktIntActionDate + "]  [NextStepID: " + action.ActionTypeNextStepID + "]  [NextStep: " +
                                     action.ActionTypeNextStepCaption + "]   [NextStepCode: " + action.ActionTypeNextStepCode + "]");
                            switch (action.ActionTypeNextStepCode)
                            {
                                case 1:
                                    ProcessIntMelde(aktInt, action, ink);
                                    break;
                                case 2:
                                    ProcessIntKlage(action);
                                    break;
                                case 3:
                                    ProcessIntStrono(action);
                                    break;
                                case 4:
                                    ProcessIntInstallment(aktInt, action, ink);
                                    break;
                                case 5:
                                    ProcessIntNotice(action); //TODO: Talk to nadine about this (what happens when the field person collects money)
                                    ProcessIntInkassoNextStep(aktInt); //  continue with the next step in workflow
                                    break;
                                case 6:
                                    ProcessIntInkassoNextStep(aktInt); // Just continue with the next step in workflow
                                    break;
                                case 7:
                                    ProcessIntBuchhaltung(action);
                                    break;
                                case 8:
                                    // Extension Request
                                    break;
                                case 9:
                                    ProcessAppointment(aktInt);
                                    break;
                                case 10:
                                    ProcessTotalCollection(aktInt);
                                    break;
                                case 11:
                                    ProcessIntMeldeIfLawyer(aktInt, action, ink);
                                    break;
                                default:
                                    Log.Error("Invalid Next Step [AktIntActionAkt=" + action.AktIntActionAkt + "] [AktIntActionType=" + action.AktIntActionType + "] [ActionTypeNextStepCode=" + action.ActionTypeNextStepCode + "]");
                                    break;
                            }
                        }
                    }
                }
            }

            #endregion

            #region Overdue
            ArrayList interventionOverdueList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenInt WHERE AktIntCustInkAktID > 0 AND AktIntStatus = 1 AND AktIntTermin < '" + DateTime.Now.ToShortDateString() + "' and AktIntProcessCode <> " + _control.ProcessCodeDone + " Order by AktIntID desc", typeof(tblAktenInt));
            foreach (tblAktenInt aktInt in interventionOverdueList)
            {
                ProcessIntOverdue(aktInt);
            }
            #endregion
        }

        private void CloseOverdueAkts()
        {
            ArrayList interventionOverdueList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenInt WHERE AktIntTermin < '" + DateTime.Now.ToShortDateString() + " 23:59:59' AND AktIntStatus = 1 AND AKTIntRVInkassoType <> 1 AND AktIntCustInkAktID > 0 AND AktIntProcessCode <> " + _control.ProcessCodeDone, typeof(tblAktenInt));
            foreach (tblAktenInt aktInt in interventionOverdueList)
            {
                bool ok = true;
                tblCustInkAkt ink = null;
                if (aktInt.IsInkasso())
                {
                    ink = HTBUtils.GetInkassoAkt(aktInt.AktIntCustInkAktID);
                    if (ink == null || ink.CustInkAktIsWflStopped || ink.CustInkAktStatus == _control.InkassoAktStornoStatus)
                    {
                        ok = false;
                    }
                }
                if (!ok)
                    continue;
                var action = (qryAktenIntAktionen)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenIntAktionen WHERE AktIntActionAkt = " + aktInt.AktIntID + " ORDER BY AktIntActionDate DESC", typeof(qryAktenIntAktionen));
                if (aktInt.AktIntProcessCode == _control.ProcessCodeInstallment)
                {
                    ProcessIntInstallment(aktInt, action, ink);
                }
                else
                {
                    if (action != null)
                    {
                        Log.Info("[Ink: " + action.AktIntCustInkAktID + "]  [Int: " + action.AktIntActionAkt + "]  [ActionType: " + action.AktIntActionType + "]  [ActionDate:" + action.AktIntActionDate + "]  [NextStepID: " + action.ActionTypeNextStepID + "]  [NextStep: " + action.ActionTypeNextStepCaption + "]   [NextStepCode: " + action.ActionTypeNextStepCode + "]");
                        switch (action.ActionTypeNextStepCode)
                        {
                            case 1:
                                ProcessIntMelde(aktInt, action, ink);
                                break;
                            case 2:
                                ProcessIntKlage(action);
                                break;
                            case 3:
                                ProcessIntStrono(action);
                                break;
                            case 4:
                                ProcessIntInstallment(aktInt, action, ink);
                                break;
                            case 5:
                                ProcessIntNotice(action); //TODO: Talk to nadine about this (what happens when the field person collects money)
                                ProcessIntInkassoNextStep(aktInt); //  continue with the next step in workflow
                                break;
                            case 6:
                                ProcessIntInkassoNextStep(aktInt); // Just continue with the next step in workflow
                                break;
                            case 7:
                                ProcessIntBuchhaltung(action);
                                break;
                            case 9:
                                ProcessAppointment(aktInt);
                                break;
                            case 10:
                                ProcessTotalCollection(aktInt);
                                break;
                            default:
                                Log.Error("Invalid Next Step [AktIntActionAkt=" + action.AktIntActionAkt + "] [AktIntActionType=" + action.AktIntActionType + "] [ActionTypeNextStepCode=" + action.ActionTypeNextStepCode + "]");
                                break;
                        }
                    }
                }
            }
        }

        private void ProcessIntMelde(tblAktenInt intAkt, qryAktenIntAktionen action, tblCustInkAkt ink)
        {
            
            var aktUtils = new AktUtils(action.AktIntCustInkAktID);
            if (intAkt.AktIntStatus == _control.InterventionTypeTerminverlust && !aktUtils.IsLaywerInWorkflow())
            {
                aktUtils.CancelInkassoAkt();
            }
            else
            {
                int meldeID = aktUtils.CreateMeldeAkt(HTBUtils.GetInkassoAktQry(action.AktIntCustInkAktID));

                var inkAkt = HTBUtils.GetInkassoAkt(action.AktIntCustInkAktID);

                if (inkAkt != null)
                {
                    qryCustInkAkt qryInkAkt = HTBUtils.GetInkassoAktQry(action.AktIntCustInkAktID);

                    int meldeKostenArtId = aktUtils.control.MeldeKostenArtId;
                    if (!qryInkAkt.GegnerZipPrefix.Trim().ToUpper().Equals("") && !qryInkAkt.GegnerZipPrefix.Trim().ToUpper().Equals("A"))
                        meldeKostenArtId = aktUtils.control.MeldeKostenAuslandArtId;

                    aktUtils.SetInkassoStatusBasedOnWflAction(meldeKostenArtId, _control.AutoUserId, null, "", aktUtils.control.MeldePeriod);

                    SendAktSentToMeldeNotification(inkAkt.CustInkAktID, meldeID);
                }
                SetIntAktProcessDone(action.AktIntActionAkt);
                Log.Info("\nMELDE CREATED\n");
            }
        }
        private void ProcessIntKlage(qryAktenIntAktionen action)
        {
            qryCustInkAkt qryInkAkt = HTBUtils.GetInkassoAktQry(action.AktIntCustInkAktID);
            var aktUtils = new AktUtils(action.AktIntCustInkAktID);

            if (aktUtils.IsLaywerInWorkflow())
            {
                aktUtils.CreateRechtsanwaldCosts(qryInkAkt); // currently there are no costs but you never know
                aktUtils.SendInkassoPackageToLawyer(new HTBEmailAttachment(ReportFactory.GetZwischenbericht(action.AktIntCustInkAktID), "Zwischenbericht.pdf", "Application/pdf"));
                aktUtils.SetInkassoStatusBasedOnWflAction(_control.RechtsanwaldKostenArtId, _control.AutoUserId);
            }
            else
            {
                aktUtils.CancelInkassoAkt();
            }
            SetIntAktProcessDone(action.AktIntActionAkt);
            Log.Info("Packet Sent to Lawyer");
        }
        private void ProcessIntInstallment(tblAktenInt intAkt, qryAktenIntAktionen action, tblCustInkAkt ink)
        {
            if (ink != null)
            {
                if (action.AktIntActionIsPersonalCollection)
                {
                    ink.CustInkAktStatus = _control.InkassoAktInterventionStatus;
                    ink.CustInkAktCurStatus = 41; // XXX CollectionInvoice vor Ort
                }
                else
                {
                    ink.CustInkAktStatus = _control.InkassoAktInkassoStatus;
                    ink.CustInkAktCurStatus = 10; // RAT Ratenvereinbarung
                }
                RecordSet.Update(ink);
            }
            SetIntAktProcessDone(intAkt);
            Log.Info("Ratenvereinbarung Processed");
        }
        private void ProcessIntStrono(qryAktenIntAktionen action)
        {
            tblCustInkAkt inkAkt = HTBUtils.GetInkassoAkt(action.AktIntCustInkAktID);
            inkAkt.CustInkAktStatus = _control.InkassoAktStornoStatus;
            RecordSet.Update(inkAkt);
            SetIntAktProcessDone(action.AktIntActionAkt);
            Log.Info("Storno Processed");
        }
        private void ProcessIntNotice(qryAktenIntAktionen action)
        {
            SetIntAktProcessDone(action.AktIntActionAkt);
            Log.Info("Send Notice to...  Not Sure... maybe email");
        }
        private void ProcessIntBuchhaltung(qryAktenIntAktionen action)
        {
            SetIntAktProcessDone(action.AktIntActionAkt);
            Log.Info("Make Sure it gets entered in Buchhaltung... maybe send email");
        }
        private void ProcessAppointment(tblAktenInt intAkt)
        {
            tblCustInkAkt inkAkt = HTBUtils.GetInkassoAkt(intAkt.AktIntCustInkAktID);
            if (inkAkt != null)
            {
                inkAkt.CustInkAktStatus = _control.InkassoAktInkassoStatus;
                inkAkt.CustInkAktCurStatus = 12; // Kalendirung
//                inkAkt.CustInkAktNextWFLStep = DateTime.Now.AddDays(10); // allow 10 days for the akt to be processed
//                inkAkt.CustInkAktIsWflStopped = true;
                RecordSet.Update(inkAkt);
            }
            SetIntAktProcessDone(intAkt);
        }
        private void ProcessTotalCollection(tblAktenInt intAkt)
        {
            tblCustInkAkt inkAkt = HTBUtils.GetInkassoAkt(intAkt.AktIntCustInkAktID);
            if (inkAkt != null)
            {
                inkAkt.CustInkAktStatus = _control.InkassoAktWflDoneStatus;
                inkAkt.CustInkAktCurStatus = 24; // ÜAK (Forderung bezahlt, Überweisung folgt)
                inkAkt.CustInkAktNextWFLStep = DateTime.Now.AddDays(21); // allow 21 days for the transfer to be processed
                RecordSet.Update(inkAkt);
                // Notify Client (copy office)
                new AktUtils(inkAkt.CustInkAktID).SendAbschlusBerichtToClient(inkAkt, ReportFactory.GetZwischenbericht(inkAkt.CustInkAktID), "Klient_Akt_Total_Collection_Text", string.Format("Forderung bezahlt [ {0} ], Überweisung folgt", string.IsNullOrEmpty(inkAkt.CustInkAktKunde) ? inkAkt.CustInkAktID.ToString() : inkAkt.CustInkAktKunde));
            }
            SetIntAktProcessDone(intAkt);
        }
        
        private void ProcessIntInkassoNextStep(tblAktenInt intAkt)
        {
            tblCustInkAkt inkAkt = HTBUtils.GetInkassoAkt(intAkt.AktIntCustInkAktID);
            if (inkAkt != null)
            {
                inkAkt.CustInkAktStatus = _control.InkassoAktInkassoStatus;
                inkAkt.CustInkAktNextWFLStep = DateTime.Now;
                RecordSet.Update(inkAkt);
            }
            SetIntAktProcessDone(intAkt);
        }
        private void ProcessIntMeldeIfLawyer(tblAktenInt intAkt, qryAktenIntAktionen action, tblCustInkAkt ink)
        {
            var aktUtils = new AktUtils(action.AktIntCustInkAktID);

            if (aktUtils.IsLaywerInWorkflow())
                ProcessIntMelde(intAkt, action, ink);
            else
                aktUtils.CancelInkassoAkt();
            SetIntAktProcessDone(action.AktIntActionAkt);
        }

        private void ProcessIntOverdue(tblAktenInt intAkt)
        {
            if(intAkt.AktIntOverdueNotifiedDate == HTBUtils.DefaultDate)
            {
                string adName = HTBUtils.GetADSalutationAndName(intAkt.AktIntID);
                string text = HTBUtils.GetFileText(HTBUtils.GetConfigValue("AD_Overdue_Akt_Text"));
                text = text.Replace("[name]", HTBUtils.ReplaceHtmlUmlauten(adName));
                text = text.Replace("[date]", DateTime.Now.AddDays(_control.GracePeriod).ToShortDateString());
                text = text.Replace("[akt]", intAkt.AktIntID + " [" + intAkt.AktIntAZ + "]<br/><br/>"+HTBUtils.GetAktGegnerNameAndAddress(intAkt.AktIntID, true, 8)+"<br/><br/>");
                var subject = "Akt überfälig: " + intAkt.AktIntID;

                var toList = new List<string>
                                          {
                                              HTBUtils.GetConfigValue("Default_EMail_Addr")
                                          };
                string adEmail = HTBUtils.GetADEmailAddress(intAkt.AktIntID);
                if (adEmail != null)
                {
                    toList.Add(adEmail);
                }
                else
                {
                    subject += " [an AD NICHT Geschickt ... keine Emailadresse]";
                }

                ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail(toList, subject, text, true);
                intAkt.AktIntOverdueNotifiedDate = DateTime.Now;
                RecordSet.Update(intAkt);
            }
            else if(intAkt.IsInkasso() && DateTime.Now.Subtract(intAkt.AktIntOverdueNotifiedDate).TotalDays >= _control.GracePeriod)
            {
                new AktUtils(intAkt.AktIntCustInkAktID).CreateInterventionAktAktion(_control.InternalActionGegnerNotFoundPersonalVisit);
                intAkt.AktIntStatus = 2; // Abgegeben
                //intAkt.AktIntSB = _control.AutoUserId;
                SetIntAktProcessDone(intAkt);
            }
        }
        private void SetIntAktProcessDone(int aktId)
        {
            SetIntAktProcessDone(HTBUtils.GetInterventionAkt(aktId));
        }

        private void SetIntAktProcessDone(tblAktenInt intAkt)
        {
            if (intAkt != null)
            {
                intAkt.AktIntProcessCode = _control.ProcessCodeDone;
                RecordSet.Update(intAkt);
            }
        }
        private void SendAktSentToMeldeNotification(int aktId, int meldeId)
        {
            var subject = new StringBuilder("Neue Melde: ");
            subject.Append(meldeId);
            subject.Append(" für Akt: ");
            subject.Append(aktId);
            subject.Append(" wurde generiert!");

            ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail(HTBUtils.GetConfigValue("Default_EMail_Addr"), subject.ToString(), "Akt: " + aktId + "  Melde: "+meldeId, true, aktId);
        }
        #endregion

        #region Melde
        public void GenerateMeldeNextStep()
        {
            ArrayList meldeCompletedList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktMelde WHERE AMStatus = 4 and AMNr > '0' and AMProcessCode <> " + _control.ProcessCodeDone, typeof(tblAktMelde));
            foreach (tblAktMelde aktMelde in meldeCompletedList)
            {
                int amNr = 0;
                try
                {
                    amNr = Convert.ToInt32(aktMelde.AMNr);
                }
                catch
                {
                    amNr = 0;
                }
                if (amNr > 0)
                {
                    tblCustInkAkt inkAkt = HTBUtils.GetInkassoAkt(amNr);
                    if (inkAkt != null && inkAkt.CustInkAktStatus != _control.InkassoAktFertigStatus && inkAkt.CustInkAktStatus != _control.InkassoAktStornoStatus)
                    {
                        var intAkt = (tblAktenInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenInt WHERE AktIntstatus = 4 AND AktIntCustInkAktID = " + amNr + " AND AktIntAktType = " + _control.InterventionTypeTerminverlust + " ORDER BY AktIntDatum DESC", typeof (tblAktenInt));
                        if (intAkt != null)
                        {
                            ProcessMeldeAfterTerminverlust(aktMelde, inkAkt);
                        }
                        else
                        {
                            switch (aktMelde.AMInfoAuswahl)
                            {
                                case 1: // Aufrecht Gemeldet
                                    ProcessMeldeCurrentAddress(inkAkt);
                                    break;
                                case 2: // Unbekannt verzogen
                                case 9: // Auskunftssperre
                                    ProcessMeldeThreeMonthWait(inkAkt);
                                    break;
                                case 3: // Name und/oder Geburtsdatum neu/Korrektur
                                case 11: // Neue Anschrift
                                    ProcessMeldeRestartInkasso(inkAkt);
                                    break;
                                case 8: // Verstorben
                                    ProcessMeldePersonIsDead(inkAkt);
                                    break;
                                case 4: // Amtlich unbekannt
                                case 5: // Überprüfen der Geburtsdaten
                                case 6: // Nicht gemeldet, daher kein Geb.Dat
                                case 7: // Geburtsdatum erforderlich
                                case 10: // Kein Handelsregistereintrag
                                    ProcessMeldeFurtherResearch(aktMelde);
                                    break;
                                default:
                                    ProcessMeldeFurtherResearch(aktMelde);
                                    break;
                            }
                            RecordSet.Update(inkAkt);
                        }
                    }
                    else
                    {
                        Log.Error("No CollectionInvoice Akt Found for: " + aktMelde.AMNr);
                    }
                }
                SetMeldeAktProcessDone(aktMelde);
            }
        }


        private void ProcessMeldeAfterTerminverlust(tblAktMelde aktMelde, tblCustInkAkt inkAkt)
        {
            if (aktMelde.AMInfoAuswahl == 8)
            {
                ProcessMeldePersonIsDead(inkAkt);
            }
            else
            {
                qryCustInkAkt qryInkAkt = HTBUtils.GetInkassoAktQry(inkAkt.CustInkAktID);
                var aktUtils = new AktUtils(inkAkt.CustInkAktID);

                if (aktUtils.IsLaywerInWorkflow())
                {
                    aktUtils.CreateRechtsanwaldCosts(qryInkAkt); // currently there are no costs but you never know
                    aktUtils.SendInkassoPackageToLawyer(new HTBEmailAttachment(ReportFactory.GetZwischenbericht(inkAkt.CustInkAktID), "Zwischenbericht.pdf", "Application/pdf"));
                    aktUtils.SetInkassoStatusBasedOnWflAction(_control.RechtsanwaldKostenArtId, _control.AutoUserId);
                }
                else
                {
                    aktUtils.CancelInkassoAkt();
                }
                Log.Info("Packet Sent to Lawyer");
            }
        }
        

        private void ProcessMeldeCurrentAddress(tblCustInkAkt inkAkt)
        {
            inkAkt.CustInkAktStatus = _control.InkassoAktInkassoStatus;
            inkAkt.CustInkAktNextWFLStep = DateTime.Now;
            Log.Info("CURRENT ADDRESS PROCESSED");
        }
        private void ProcessMeldeThreeMonthWait(tblCustInkAkt inkAkt)
        {
            inkAkt.CustInkAktNextWFLStep = DateTime.Today.AddDays(_control.InkassoAktWaitForReMeldePeriod);
            inkAkt.CustInkAktStatus = _control.InkassoAktWaitingForReMeldeStatus;
            inkAkt.CustInkAktCurStatus = 12; // Kalendirung
             
            SendAktWillWaitNotification(inkAkt.CustInkAktID);
            
            Log.Info("TODO: WAIT " + _control.InkassoAktWaitForReMeldePeriod + " DAYS");
        }

        private void ProcessMeldeRestartInkasso(tblCustInkAkt inkAkt)
        {
            var delMahnungActionsSql = "DELETE FROM tblMahnung WHERE MahnungAktID = " + inkAkt.CustInkAktID;
            var delMahnungDocsSql = "DELETE FROM tblDokument WHERE DokInkAkt = " + inkAkt.CustInkAktID + " AND DokCaption like 'Mahnung%' ";
            
            var set = new RecordSet();
            
            var invMgr = new InvoiceManager();
            
            invMgr.DeleteCollectionInvoicesForAct(inkAkt.CustInkAktID, false);
            inkAkt.CustInkAktStatus = _control.InkassoAktInkassoStatus;
            inkAkt.CustInkAktNextWFLStep = DateTime.Now;
            inkAkt.CustInkAktCurrentStep = 0;
            inkAkt.CustInkAktSkipInitialInvoices = false;

            set.ExecuteNonQuery(delMahnungActionsSql);
            set.ExecuteNonQuery(delMahnungDocsSql);

            Log.Info("RESTART INKASSO PROCESSED");
        }
        private void ProcessMeldeFurtherResearch(tblAktMelde aktMelde)
        {
            ServiceFactory.Instance.GetService<IHTBEmail>().SendMeldeResearchNotice(aktMelde);
            Log.Info("SENT MAIL");
        }
        private void ProcessMeldePersonIsDead(tblCustInkAkt inkAkt)
        {
            inkAkt.CustInkAktStatus = _control.InkassoAktStornoStatus;
            inkAkt.CustInkAktCurStatus = 30; // Verstorben
            Log.Info("Shuldner is DEAD :-(");
        }

        private void SetMeldeAktProcessDone(tblAktMelde meldeAkt)
        {
            if (meldeAkt != null)
            {
                var set = new RecordSet();
                set.ExecuteNonQuery("UPDATE tblAktMelde SET AMProcessCode = " + _control.ProcessCodeDone + " WHERE AMID = " + meldeAkt.AMID);
            }
        }
        private void SendAktWillWaitNotification(int aktId)
        {
            var subject = new StringBuilder("Akt: ");
            subject.Append(aktId);
            subject.Append(" wird ");
            subject.Append(_control.InkassoAktWaitForReMeldePeriod);
            subject.Append(" Tage warten auf eine neue Melde.");

            ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail(HTBUtils.GetConfigValue("Default_EMail_Addr"), subject.ToString(), "Akt: " + aktId + " wurde Kalendiert!", true,aktId);
        }
        #endregion

    }
}
