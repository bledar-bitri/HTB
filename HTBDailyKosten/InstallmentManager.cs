using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using HTBAktLayer;
using HTBUtilities;
using HTB.Database;
using HTB.Database.Views;
using HTBServices.Mail;
using HTBServices;

namespace HTBDailyKosten
{
    public class InstallmentManager
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private ArrayList _installmentsList = new ArrayList();
        private readonly tblControl _control = HTBUtils.GetControlRecord();
        private readonly HTBInvoiceManager.InvoiceManager _invManager = new HTBInvoiceManager.InvoiceManager();

        public void GenerateOverdueInvoices()
        {
            var qryKostenSet = new qryKostenSet();
            qryKostenSet.LoadTerminverlust();
           
            LoadInstallmentsList();
            foreach (qryKosten kosten in qryKostenSet.qryKostenList)
            {
                foreach (tblCustInkAktRate installment in _installmentsList)
                {
                    if (installment.CustInkAktRateBalance > 0 && !IsOverdueInvoice(installment.CustInkAktRateID))
                    {
                        bool doOverdue = true;
                        if (installment.CustInkAktRatePostponeTillDate > DateTime.Now && installment.CustInkAktRatePostponeWithNoOverdue)
                        {
                            doOverdue = false;
                        }
                        if (doOverdue)
                        {
                            // Personal Collection send email to Field Person (and office) [every 7 days if nothing hapens]
                            if (installment.CustInkAktRatePaymentType == 1)
                            {
                                if (DateTime.Now.Subtract(installment.CustInkAktRateNotifiedAD).Days >= 7)
                                {
                                    #region Personal Collection Send Email to Field Person
                                    qryCustInkAkt akt = HTBUtils.GetInkassoAktQry(installment.CustInkAktRateAktID);
                                    if (akt == null)
                                    {
                                        ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail(HTBUtils.GetConfigValue("Default_EMail_Addr"), "Rate ohne Akt!", "Rate: " + installment.CustInkAktRateID + " hatt einen falschen Aktenzahl: " + installment.CustInkAktRateAktID, true, installment.CustInkAktRateAktID);
                                    }
                                    else
                                    {
                                        var intAkt = (qryAktenInt) HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenInt WHERE AktIntCustInkAktID = " + installment.CustInkAktRateAktID + " ORDER BY AktIntID DESC", typeof (qryAktenInt));
                                        if (intAkt == null)
                                        {
                                            ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail(HTBUtils.GetConfigValue("Default_EMail_Addr"), "Persönlichesinkasso ohne Intervenionsakt!",
                                                                            "Rate: " + installment.CustInkAktRateID + " Inkassoakt: " + installment.CustInkAktRateAktID + " könte nicht im Interventionsbereich gefunden werden!", true, installment.CustInkAktRateAktID);
                                        }
                                        else if(HasPersonalCollectionAction(intAkt.AktIntID))
                                        {
                                            
                                            installment.CustInkAktRateNotifiedAD = DateTime.Now;

                                            var user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserID = " + intAkt.AktIntSB, typeof(tblUser));
                                        
                                            String adTvlText = HTBUtils.GetFileText(HTBUtils.GetConfigValue("AD_TVL_Text"));
                                            string userSalutation = "";
                                            if (user.UserSex == 1)
                                                userSalutation = "geehrter Herr";
                                            else if (user.UserSex == 2)
                                                userSalutation = "geehrte Frau";

                                            adTvlText = adTvlText.Replace("[ad_anrede]", userSalutation);
                                            adTvlText = adTvlText.Replace("[ad_nachname]", intAkt.UserNachname);
                                            adTvlText = adTvlText.Replace("[s_anrede]", akt.GegnerAnrede);
                                            adTvlText = adTvlText.Replace("[s_name]", akt.GegnerLastName1 + " " + akt.GegnerLastName2);
                                            adTvlText = adTvlText.Replace("[akt]", intAkt.AktIntID + "  " + intAkt.AktIntAZ);
                                            var toList = HTBUtils.GetValidEmailAddressesFromStrings(new []
                                                {
                                                    HTBUtils.GetConfigValue("Default_EMail_Addr"),
                                                    user.UserEMailOffice,
                                                    user.UserEMailPrivate
                                                });


                                            ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail(toList.ToArray(), intAkt.GegnerLastName1 + " " + intAkt.GegnerLastName2 + " Terminverlust", adTvlText);

                                            RecordSet.Update(installment);
                                            new AktUtils(installment.CustInkAktRateAktID).CreateAktAktion(-1, _control.AutoUserId, "Email an AD über die Rate: " + installment.CustInkAktRateDueDate.ToShortDateString());
                                        }
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                #region Create Terminverlust Invoice
                                tblCustInkAktInvoice inv = _invManager.CreateInvoice(
                                    installment.CustInkAktRateAktID,
                                    tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_OVERDUE_CHARGE,
                                    Convert.ToDouble(HTBUtils.GetCalculatedKost(Convert.ToDecimal(installment.CustInkAktRateAmount), kosten, DateTime.Now)),
                                    "Terminverlust [Rate vom " + installment.CustInkAktRateDueDate.ToShortDateString() + "]",
                                    true
                                    );
                                inv.InvoiceCustInkAktRateID = installment.CustInkAktRateID;
                                _invManager.SaveInvoice(inv);
                                #endregion

                                #region Update CollectionInvoice Akt [to show missing payment]
                                var aktInk = HTBUtils.GetInkassoAkt(inv.InvoiceCustInkAktId);
                                if (aktInk  != null && aktInk.CustInkAktStatus != _control.InkassoAktInterventionStatus)
                                {
                                    aktInk.CustInkAktCurStatus = 11;
                                    aktInk.CustInkAktStatus = _control.InkassoAktInkassoStatus;
                                    aktInk.CustInkAktNextWFLStep = DateTime.Now.AddDays(_control.GracePeriod);
                                    RecordSet.Update(aktInk);
                                }
                                #endregion

                                #region Create Termintverlust Mahnung

                                RecordSet.Insert(new tblMahnung
                                                     {
                                                         MahnungAktID = inv.InvoiceCustInkAktId,
                                                         MahnungDate = DateTime.Now,
                                                         MahnungStatus = 0,
                                                         // Terminverlust
                                                         MahnungType = 1,
                                                         MahnungRateID = installment.CustInkAktRateID
                                                     });                                
                                #endregion

                                #region Create Terminverlust Action
                                var aktUtils = new AktUtils(installment.CustInkAktRateAktID);
                                aktUtils.CreateAktAktion(11, _control.AutoUserId, "Rate: " + installment.CustInkAktRateDueDate.ToShortDateString());
                                #endregion

                                #region Create Terminverlust Intervention
                                var count = (SingleValue)HTBUtils.GetSqlSingleRecord("SELECT COUNT(*) [IntValue] FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + installment.CustInkAktRateAktID + " and InvoiceType = " + tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_OVERDUE_CHARGE, typeof(SingleValue));
                                if(count.IntValue >= 2) // more than 2 TVL
                                {
                                    var intAkt = (tblAktenInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenInt WHERE AktIntCustInkAktID = " + installment.CustInkAktRateAktID + " and AktIntAktType = " + _control.InterventionTypeTerminverlust, typeof(tblAktenInt));
                                    if (intAkt == null) // no tvl intervenion
                                    {
                                        var akt = (qryCustInkAkt) HTBUtils.GetSqlSingleRecord("SELECT *  FROM qryCustInkAkt WHERE CustInkAktId = " + installment.CustInkAktRateAktID, typeof (qryCustInkAkt));
                                        CreateTerminverlustInterventionAkt(aktUtils, akt);
                                        new RecordSet().ExecuteNonQuery("UPDATE tblCustInkAktRate SET CustInkAktRatePaymentType = 1 WHERE CustInkAktRateAktID = " + akt.CustInkAktID);
                                    }
                                        /* 
                                         * Do nothing for now. This stepp will take place at the end of the intervention 
                                    else if (intAkt.AktIntStatus == 4) // intervention done... either send akt to lawyer or mark it as storno
                                    {

                                        if (aktUtils.IsLaywerInWorkflow())
                                            aktUtils.SendInkassoPackageToLawyer(new HTBEmailAttachment(ReportFactory.GetZwischenbericht(installment.CustInkAktRateAktID), "Zwischenbericht.pdf", "Application/pdf"));
                                        else
                                        {
                                            var akt = HTBUtils.GetInkassoAkt(installment.CustInkAktRateAktID);
                                            aktUtils.CreateAktAktion(27);
                                            akt.CustInkAktStatus = _control.InkassoAktStornoStatus;
                                            akt.CustInkAktCurStatus = 27; // Storno lt. Auftraggeber
                                            RecordSet.Update(akt);
                                        }
                                    }
                                         * */
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
        }

        private void LoadInstallmentsList()
        {
            var overdueInstallmentsQuery = new StringBuilder("SELECT * FROM tblCustInkAktRate R INNER JOIN tblCustInkAkt A on R.CustInkAktRateAktID = A.CustInkAktID ");
            overdueInstallmentsQuery.Append("WHERE (CustInkAktRateBalance > 0) AND (CustInkAktRateDueDate <= '");
            overdueInstallmentsQuery.Append(DateTime.Now.AddDays(-_control.TerminverlustGracePeriod).ToShortDateString());
            overdueInstallmentsQuery.Append("') AND CustInkAktStatus in (1, 2, 3)");
            overdueInstallmentsQuery.Append(" ORDER BY ");
            overdueInstallmentsQuery.Append("CustInkAktRateDueDate");

            _installmentsList = HTBUtils.GetSqlRecords(overdueInstallmentsQuery.ToString(), typeof(tblCustInkAktRate));
        }

        private bool IsOverdueInvoice(int installmentId)
        {
            return HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktRateID = " + installmentId, typeof(tblCustInkAktInvoice)) != null;
        }

        private void CreateTerminverlustInterventionAkt(AktUtils aktUtils, qryCustInkAkt akt)
        {
            aktUtils.CreateInterventionCosts(akt.CustInkAktID, akt.CustInkAktInvoiceDate);
            tblAktenInt aktInt = aktUtils.CreateNewInterventionAkt(akt, "Terminverlust: Ratenansuchen nicht möglich" + akt.CustInkAktMemo, 21, _control.InterventionTypeTerminverlust);
            aktUtils.SetInkassoStatusBasedOnWflAction(aktUtils.control.InterventionKostenArtId, _control.AutoUserId, aktInt, "Terminverlust: Ratenansuchen nicht möglich");
        }

        private bool HasPersonalCollectionAction(int aktIntId)
        {
            var action = (qryAktenIntAktionen)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenIntAktionen WHERE AktIntActionIsPersonalCollection = 1 AND AktIntActionAkt = " + aktIntId + " ORDER BY AktIntActionDate DESC", typeof(qryAktenIntAktionen));
            if(action != null && DateTime.Now.Subtract(action.AktIntActionDate).TotalDays < 7)
            {
                return true;
            }
            return false;
        }
    }
}
