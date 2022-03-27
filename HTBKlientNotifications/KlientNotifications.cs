using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using HTB.Database;
using HTB.Database.Views;
using HTBAktLayer;
using HTBExcel;
using HTBExtras;
using HTBExtras.KingBill;
using HTBReports;
using HTBServices;
using HTBServices.Mail;
using HTBUtilities;

namespace HTBKlientNotifications
{
    public class KlientNotifications
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _testMode;
        private string _testKlientId;
        private string _testAktId;
        private string _outputFolderForTestPdf;
        
        private static tblControl _control = HTBUtils.GetControlRecord();

        private static readonly int StornoCodeS001 = Convert.ToInt32(HTBUtils.GetConfigValue("Storno_Code_S001"));
        private static readonly int StornoCodeS002 = Convert.ToInt32(HTBUtils.GetConfigValue("Storno_Code_S002"));
        private static readonly int StornoCodeS007 = Convert.ToInt32(HTBUtils.GetConfigValue("Storno_Code_S007"));
        private static readonly int StornoCodeErl = Convert.ToInt32(HTBUtils.GetConfigValue("Storno_Code_ERL"));
        private static readonly int StornoCodeVerjaerung = Convert.ToInt32(HTBUtils.GetConfigValue("Storno_Code_Verjaerung"));


        public KlientNotifications(string[] args)
        {
            try
            {
                _startDate = Convert.ToDateTime(ConfigurationManager.AppSettings["startDate"]);
                _endDate = Convert.ToDateTime(ConfigurationManager.AppSettings["endDate"]);
            }
            catch
            {
                _startDate = DateTime.Now.AddDays(-1).AddMonths(-1);
                _endDate = DateTime.Now.AddDays(-1);
                //                _endDate = DateTime.Now.AddDays(1);
            }
            
            _testMode = Convert.ToBoolean(ConfigurationManager.AppSettings["testMode"]);
            _testKlientId = ConfigurationManager.AppSettings["testKlientId"];
            _testAktId = ConfigurationManager.AppSettings["testAktId"];
            _outputFolderForTestPdf = ConfigurationManager.AppSettings["outputFolderForTestPdf"];

            if (_testMode)
                TestNotifications();
            else
            {
                switch (args[0].ToLower())
                {
                    case "transfers":
                        SendBankTransferNotifications();
                        break;
                    case "monthly_notification":
                        SendMonthlyNotifications();
                        break;
                    case "yearly_notification":
                        try
                        {
                            _startDate = Convert.ToDateTime(ConfigurationManager.AppSettings["startDate"]);
                            _endDate = Convert.ToDateTime(ConfigurationManager.AppSettings["endDate"]);
                        }
                        catch
                        {
                            _startDate = DateTime.Now.AddYears(-1);
                            _endDate = DateTime.Now.AddDays(-1);
                        }
                        SendYearlyNotification();
                        break;
                }
            }
        }

        private void SendBankTransferNotifications()
        {
            var klients = HTBUtils.GetSqlRecords("SELECT * FROM tblKlient WHERE KlientType = 15", typeof(tblKlient));
            foreach (tblKlient klient in klients)
            {
                ArrayList invoices = GetInvoicesForKlient(klient.KlientID)[0];
                if (invoices != null && invoices.Count > 0)
                {
                    SendBankTransferNotificationToKlient(klient, invoices);
                }
            }
        }

        private void TestNotifications()
        {
            Console.WriteLine("Testing Notification");
            var klients = HTBUtils.GetSqlRecords("SELECT * FROM tblKlient WHERE KlientId = " + _testKlientId, typeof(tblKlient));
            Console.WriteLine("Client Loaded");
            foreach (tblKlient klient in klients)
            {
                Console.WriteLine("Klient: " + klient.KlientID+"    Loading Akts");

                var where = " CustInkAktKlient = " + _testKlientId;
                if (!string.IsNullOrEmpty(_testAktId))
                    where += " AND CustInkAktID = " + _testAktId;

                var akts = HTBUtils.GetSqlRecords("SELECT CustInkAktID FROM tblCustInkAkt WHERE " + where, typeof(tblCustInkAkt));

                Console.WriteLine("Akts Loaded " + akts.Count);

                var aktNumbers = (from tblCustInkAkt akt in akts select akt.CustInkAktID.ToString()).ToList();
                
                if (aktNumbers.Count <= 0) continue;

                Console.WriteLine("Generating Report... ");

                var attachment = GetKlientBerichAttachment(klient, aktNumbers, false);

                Console.WriteLine("Report Generated! Showing Report");

                if (attachment == null) continue;
                ShowReport(attachment);
                Console.WriteLine("Notification Testing Finished");
            }
        }

        private void SendMonthlyNotifications()
        {
            var klients = HTBUtils.GetSqlRecords("SELECT * FROM tblKlient WHERE KlientType = 15", typeof(tblKlient));
            foreach (tblKlient klient in klients)
            {
                ArrayList akts = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAkt WHERE  CustInkAktSendBericht = 1 AND CustInkAktKlient = " + klient.KlientID, typeof(tblCustInkAkt));
                var aktNumbers = (from tblCustInkAkt akt in akts where HasActionsWithinDateRange(akt) select akt.CustInkAktID.ToString()).ToList();
                if (aktNumbers.Count > 0)
                {
                    HTBEmailAttachment attachment = GetKlientBerichAttachment(klient, aktNumbers);
                    if (attachment != null)
                    {
                        var attachments = new List<HTBEmailAttachment> { attachment };
                        CreateAndSendEmail(klient, attachments, "Klient_Monthly_Info_Text");
                    }
                }
            }
        }

        private void SendYearlyNotification()
        {
            var klients = HTBUtils.GetSqlRecords("SELECT * FROM tblKlient WHERE KlientType = 15", typeof(tblKlient));
            foreach (tblKlient klient in klients)
            {
                if (HasAktsOrActionsWithinDateRange(klient))
                {
                    ArrayList akts = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAkt WHERE CustInkAktSendBericht = 1 AND CustInkAktKlient = " + klient.KlientID, typeof(tblCustInkAkt));
                    var aktNumbers = (from tblCustInkAkt akt in akts where HasActionsWithinDateRange(akt) select akt.CustInkAktID.ToString()).ToList();
                    if (aktNumbers.Count > 0)
                    {
                        HTBEmailAttachment attachment = GetKlientBerichAttachment(klient, aktNumbers);
                        if (attachment != null)
                        {
                            var attachments = new List<HTBEmailAttachment> { attachment };
                            CreateAndSendEmail(klient, attachments, "Klient_Yearly_Info_Text", "Jährlicher Status");
                        }
                    }
                }
            }
        }

        private void SendBankTransferNotificationToKlient(tblKlient klient, ArrayList invoices)
        {
            var attachments = new List<HTBEmailAttachment>();

            #region Transfers Excel

            var excelMS = new MemoryStream();
            new TransferExcelGenerator().WriteExcelFile(excelMS, invoices, _startDate, _endDate.AddDays(-1));
            excelMS.Seek(0, SeekOrigin.Begin);
            attachments.Add(new HTBEmailAttachment(excelMS, "Überweisungsliste.xls", "application/ms-excel"));

            #endregion

            #region Transfers PDF

            var pdfMS = new MemoryStream();
            new TransferToClient().GenerateTransferList(pdfMS, klient, invoices, _startDate, _endDate.AddDays(-1));  // this operation closes the stream (pdfMS is closed)
            /* 
             * Create a new 'opened' MemoryStream to send as attachment.
             */
            var transferPdfNewStream = new MemoryStream();
            var transferBinaryWriter = new BinaryWriter(transferPdfNewStream);
            transferBinaryWriter.Write(pdfMS.ToArray());
            transferBinaryWriter.Flush();
            transferPdfNewStream.Seek(0, SeekOrigin.Begin);
            attachments.Add(new HTBEmailAttachment(transferPdfNewStream, "Überweisungsliste.pdf", "Application/pdf"));

            #endregion

            #region Bericht PDF

            var aktNumbers = (from KlientTransferRecord invoice in invoices select invoice.AktId.ToString()).ToList();

            HTBEmailAttachment berichAttachment = GetKlientBerichAttachment(klient, aktNumbers);

            if (berichAttachment != null)
                attachments.Add(berichAttachment);

            #endregion

            CreateAndSendEmail(klient, attachments, "Klient_Transfer_Info_Text", "Überweisungsliste");
        }

        private void CreateAndSendEmail(tblKlient klient, IEnumerable<HTBEmailAttachment> attachments, string configValue, string subject = "Monatlicher Status")
        {
            var sb = new StringBuilder();
            var receipients = HTBUtils.GetKlientEmailAddresses(klient.KlientID, klient.KlientEMail);
            if (HTBUtils.IsTestEnvironment)
                receipients.Clear();

            subject += " " + HTBUtils.GetKlientNotifiationEmailSubject(klient, sb, receipients);

            receipients.Add(HTBUtils.GetConfigValue("Default_EMail_Addr")); // send copy to office [this step MUST be performed AFTER we get the subject]

            ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail(receipients, subject, sb + GetKlientReceiptBody(klient, configValue), true, attachments, 0, 0);
        }

        private void ShowReport(HTBEmailAttachment attachment)
        {
            var fname = Path.Combine(_outputFolderForTestPdf, "KlientReport");
            const string fextenssion = ".pdf";
            var name = string.Format("{0}{1}", fname, fextenssion);
            if(File.Exists(name))
                File.Delete(name);
            HTBUtils.SaveMemoryStream(attachment.AttachmentStream as MemoryStream, name);
            var proc = new Process {StartInfo = {FileName = name}};
            proc.Start();
            
            
        }

        private void TestSendBankTransferNotificationToKlient(tblKlient klient, ArrayList invoices)
        {
            const string fname = "C:\\temp\\BankTransfer.pdf";
            //new TransferExcelGenerator().WriteExcelFile(fname, invoices, _startDate, _endDate);
            new TransferToClient().GenerateTransferList(new FileStream(fname, FileMode.OpenOrCreate), klient, invoices, _startDate, _endDate.AddDays(-1));
            Process.Start(fname);

        }

        private HTBEmailAttachment GetKlientBerichAttachment(tblKlient klient, IEnumerable<string> aktNumbers, bool createNewAction = true)
        {
            var berichMS = GetKlientBericht(klient, aktNumbers, createNewAction);
            if (berichMS != null)
            {
                var berichtNewStream = new MemoryStream();
                var binaryBerichtWriter = new BinaryWriter(berichtNewStream);
                binaryBerichtWriter.Write(berichMS.ToArray());
                binaryBerichtWriter.Flush();
                berichtNewStream.Seek(0, SeekOrigin.Begin);
                return new HTBEmailAttachment(berichtNewStream, "Bericht.pdf", "Application/pdf");
            }
            return null;
        }
        private ArrayList[] GetInvoicesForKlient(int klientId)
        {

            var parameters = new ArrayList
                                 {
                                     new StoredProcedureParameter("startDate", SqlDbType.DateTime, _startDate),
                                     new StoredProcedureParameter("endDate", SqlDbType.DateTime, _endDate),
                                     new StoredProcedureParameter("klientId", SqlDbType.Int, klientId)
                                 };

            return HTBUtils.GetMultipleListsFromStoredProcedure("spGetTransferredToKlient", parameters, new[] { typeof(KlientTransferRecord), typeof(KlientTransferRecord) });
        }

        private MemoryStream GetKlientBericht(tblKlient klient, IEnumerable<string> aktNumbers, bool createNewAction)
        {
            var aktList = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkAkt WHERE CustInkAktID IN (" + GetInClause(aktNumbers) + ")  AND KlientID = " + klient.KlientID, typeof(qryCustInkAkt));
            if (aktList.Count > 0)
            {
                var rpt = new Zwischenbericht();
                var ms = new MemoryStream();
                rpt.Open(ms);
                var firstAkt = true;
                foreach (qryCustInkAkt akt in aktList)
                {
                    if (akt != null)
                    {
                        var aktionsList = new ArrayList();

                        #region Load Records

                        var interventionAktions = new ArrayList();
                        var inkassoAktions = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkAktAktionen WHERE CustInkAktAktionAktID = " + akt.CustInkAktID + " and KZIsVisibleForKlient = 1 ORDER BY CustInkAktAktionDate DESC", typeof(qryCustInkAktAktionen));
                        var intAktList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenInt WHERE AktIntCustInkAktID = " + akt.CustInkAktID + " ORDER BY AktIntID DESC", typeof(tblAktenInt));

                        // no more inkassomemo
//                        if (!string.IsNullOrEmpty(akt.CustInkAktMemo))
//                            aktionsList.Add(new InkassoActionRecord { ActionID = 9999, ActionDate = DateTime.Now, ActionCaption = "Inkassomemo:", ActionMemo = akt.CustInkAktMemo, IsOnlyMemo = true });

                        if (intAktList != null && intAktList.Count > 0)
                        {
                            foreach (tblAktenInt interventionsAkt in intAktList)
                            {
                                ArrayList intActionsList = HTBUtils.GetSqlRecords("SELECT * FROM qryInktAktAction WHERE AktIntActionAkt = " + interventionsAkt.AktIntID + " ORDER BY AktIntActionDate DESC", typeof(qryInktAktAction));
                                HTBUtils.AddListToList(intActionsList, interventionAktions);
                                if (interventionsAkt.AKTIntMemo.Trim() != string.Empty)
                                {
                                    DateTime memoDate = interventionsAkt.AktIntDatum;
                                    if (interventionAktions.Count > 0)
                                    {
                                        var action = (qryInktAktAction)interventionAktions[0];
                                        memoDate = action.AktIntActionTime.AddMinutes(10);
                                    }
                                    aktionsList.Add(new InkassoActionRecord { ActionID = 8888, ActionDate = memoDate, ActionCaption = "Interventionsmemo:", ActionMemo = interventionsAkt.AKTIntMemo, IsOnlyMemo = true });
                                }
                            }
                        }
                        ArrayList meldeResults = HTBUtils.GetSqlRecords("SELECT * FROM qryMeldeResult WHERE AMNr = '" + akt.CustInkAktID + "' ORDER BY AMID", typeof(qryMeldeResult));

                        #endregion

                        #region CombineActions

                        tblAktenInt intAkt = (intAktList == null || intAktList.Count == 0) ? null : (tblAktenInt)intAktList[0];
                        foreach (qryCustInkAktAktionen inkAction in inkassoAktions)
                            if (!inkAction.CustInkAktAktionCaption.Equals("Kosten Änderung"))
                                aktionsList.Add(new InkassoActionRecord(inkAction, intAkt));

                        foreach (qryInktAktAction intAction in interventionAktions)
                            aktionsList.Add(new InkassoActionRecord(intAction, intAkt));

                        foreach (qryMeldeResult melde in meldeResults)
                            aktionsList.Add(new InkassoActionRecord(melde, intAkt, false));

                        aktionsList.Sort(new InkassoActionRecordComparer(SortDirection.Desc));

                        #endregion

                        #region Generate KLientbericht

                        if (!firstAkt)
                        {
                            rpt.NewPage();
                        }
                        else
                        {
                            firstAkt = false;
                        }
                        var aktStatus = GetAktStatusText(akt, inkassoAktions, interventionAktions);
                        if (createNewAction && !string.IsNullOrEmpty(aktStatus))
                            new AktUtils(akt.CustInkAktID).CreateAktAktion(69, _control.AutoUserId, aktStatus);
                        rpt.GenerateZwischenbericht(akt, aktionsList, aktStatus, null, false);

                        #endregion
                    }
                }
                rpt.PrintFooter();
                rpt.CloseReport();
                return ms;
            }
            return null;
        }

        private bool HasActionsWithinDateRange(tblCustInkAkt akt)
        {
            if (akt == null)
                return false;
            ArrayList inkassoAktions = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkAktAktionen WHERE CustInkAktAktionTyp <> 69 AND CustInkAktAktionAktID = " + akt.CustInkAktID + " AND CustInkAktAktionDate BETWEEN '" + _startDate.ToShortDateString() + "' AND '" + _endDate.ToShortDateString() + " 23:59:59.000'", typeof(qryCustInkAktAktionen));
            if (inkassoAktions.Count > 0)
                return true;

            ArrayList intAktList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenInt WHERE AktIntCustInkAktID = " + akt.CustInkAktID + " ORDER BY AktIntID DESC", typeof(tblAktenInt));

            if (intAktList == null || intAktList.Count == 0)
                return false;

            foreach (tblAktenInt intAkt in intAktList)
            {
                ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM qryInktAktAction WHERE AktIntActionAkt = " + intAkt.AktIntID + " AND  AktIntActionDate  BETWEEN '" + _startDate.ToShortDateString() + "' AND '" + _endDate.ToShortDateString() + " 23:59:59.000'", typeof(qryInktAktAction));
                if (list.Count > 0)
                    return true;
            }

            return false;
        }

        private bool HasAktsOrActionsWithinDateRange(tblKlient klient)
        {
            ArrayList akts = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAkt WHERE  CustInkAktSendBericht = 1 AND CustInkAktKlient = " + klient.KlientID + " AND CustInkAktEnterDate BETWEEN '" + _startDate.ToShortDateString() + "' AND '" + _endDate.ToShortDateString() + "'", typeof(tblCustInkAkt));
            if (akts.Count > 0)
                return true;

            akts = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAkt WHERE  CustInkAktSendBericht = 1 AND CustInkAktKlient = " + klient.KlientID, typeof(tblCustInkAkt));
            return akts.Cast<tblCustInkAkt>().Any(HasActionsWithinDateRange);
        }

        private string GetInClause(IEnumerable<string> list)
        {
            var sb = new StringBuilder();
            foreach (var str in list)
            {
                sb.Append(str);
                sb.Append(", ");
            }
            return sb.Remove(sb.Length - 2, 2).ToString();
        }

        private string GetKlientReceiptBody(tblKlient klient, string configValue)
        {
            var sb = new StringBuilder();
            if (klient != null)
            {
                var contact = (tblAnsprechpartner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAnsprechpartner WHERE AnsprechKlient = " + klient.KlientID, typeof(tblAnsprechpartner));
                string text = HTBUtils.GetFileText(HTBUtils.GetConfigValue(configValue));
                text = text.Replace("[StartDate]", _startDate.ToShortDateString()).Replace("[EndDate]", _endDate.ToShortDateString());
                return contact != null ? text.Replace("[name]", contact.AnsprechTitel + " " + contact.AnsprechNachname) : text.Replace("[name]", "Damen und Herren");
            }
            return null;
        }

        private string GetAktStatusText(qryCustInkAkt akt, ArrayList inkassoAktions, ArrayList intAktions)
        {
            string text;
            if (akt.CustInkAktStatus == _control.InkassoAktWflDoneStatus || akt.CustInkAktStatus == _control.InkassoAktFertigStatus || akt.CustInkAktStatus == _control.InkassoAktStornoStatus)
            {

                string stringToReplace = "";
                string stringReplaceWith = "";

                string configValue = "Klient_Akt_Status_Negative_Text";

                if (akt.CustInkAktCurStatus == StornoCodeErl)
                    configValue = "Klient_Akt_Status_Success_ERL_Text";
                else if (akt.CustInkAktCurStatus == StornoCodeS001)
                    configValue = "Klient_Akt_Status_Success_S001_Text";
                else if (akt.CustInkAktCurStatus == StornoCodeS007)
                    configValue = "Klient_Akt_Status_Success_S007_Text";
                else if (akt.CustInkAktCurStatus == StornoCodeVerjaerung)
                    configValue = "Klient_Akt_Status_Verjaerung_Text";
                else if (IsTotalCollected(intAktions))
                    configValue = "Klient_Akt_Status_Success_Text";
                else if (HTBUtils.IsZero(new AktUtils(akt.CustInkAktID).GetAktKlientTotalBalance()))
                {
                    configValue = "Klient_Akt_Status_Direct_Payment_Text";
                    try
                    {
                        /* no longer send notification to klient */
                        new RecordSet().ExecuteNonQuery("UPDATE tblCustInkAkt SET CustInkAktSendBericht = 0 WHERE CustInkAktID = " + akt.CustInkAktID);
                    }
                    catch
                    {
                    }
                }
                else if (IsSentToLawyer(inkassoAktions))
                {
                    configValue = "Klient_Akt_Status_Lawyer_Text";
                    stringToReplace = "[Lawyer_Info]";

                    var lawyer = (tblLawyer)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblLawyer WHERE LawyerId = " + akt.CustInkAktLawyerId, typeof(tblLawyer));
                    if (lawyer != null)
                    {
                        var sb = new StringBuilder();
                        sb.Append(lawyer.LawyerAnrede);
                        sb.Append(" ");
                        sb.Append(lawyer.LawyerName2);
                        sb.Append(" ");
                        sb.Append(lawyer.LawyerName1);
                        sb.Append(Environment.NewLine);
                        sb.Append("    ");
                        sb.Append(lawyer.LawyerStrasse);
                        sb.Append(", ");
                        sb.Append(lawyer.LawyerZip);
                        sb.Append(" ");
                        sb.Append(lawyer.LawyerOrt);
                        sb.Append(Environment.NewLine);
                        sb.Append("    Tel:     ");
                        sb.Append(lawyer.LawyerPhoneCountry);
                        sb.Append("(0)");
                        sb.Append(lawyer.LawyerPhoneCity);
                        sb.Append("/");
                        sb.Append(lawyer.LawyerPhone);
                        sb.Append(Environment.NewLine);
                        sb.Append("    Fax:    ");
                        sb.Append(lawyer.LawyerFaxCountry);
                        sb.Append("(0)");
                        sb.Append(lawyer.LawyerFaxCity);
                        sb.Append("/");
                        sb.Append(lawyer.LawyerFax);
                        sb.Append(Environment.NewLine);
                        sb.Append("    Email: ");
                        sb.Append(lawyer.LawyerEmail);

                        stringReplaceWith = sb.ToString();
                    }
                }

                text = HTBUtils.GetFileText(HTBUtils.GetConfigValue(configValue));

                if (!string.IsNullOrEmpty(stringToReplace) && !string.IsNullOrEmpty(stringReplaceWith))
                    text = text.Replace(stringToReplace, stringReplaceWith);

            }
            else if (akt.CustInkAktStatus == _control.InkassoAktInstallmentStatus)
                text = HTBUtils.GetFileText(HTBUtils.GetConfigValue("Klient_Akt_Status_Installment_In_Progress_Text"));
            else if (HTBUtils.IsZero(new AktUtils(akt.CustInkAktID).GetAktKlientTotalBalance()))
            {
                text = HTBUtils.GetFileText(HTBUtils.GetConfigValue("Klient_Akt_Status_Direct_Payment_Text"));
                try
                {
                    /* no longer send notification to klient */
                    new RecordSet().ExecuteNonQuery("UPDATE tblCustInkAkt SET CustInkAktSendBericht = 0 WHERE CustInkAktID = " + akt.CustInkAktID);
                }
                catch
                {
                }
            }
            else
                text = HTBUtils.GetFileText(HTBUtils.GetConfigValue("Klient_Akt_Status_In_Progress_Text"));


            return text;
        }

        private bool IsSentToLawyer(ArrayList inkassoAktions)
        {
            return inkassoAktions.Cast<qryCustInkAktAktionen>().Any(inkassoAktion => inkassoAktion.CustInkAktAktionTyp == 45);
        }

        private bool IsTotalCollected(ArrayList intAktions)
        {
            return (from qryInktAktAction action in intAktions
                    select (tblAktenIntActionType)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntActionType WHERE AktIntActionTypeID = " + action.AktIntActionType, typeof(tblAktenIntActionType))).Any(type => type != null && type.AktIntActionIsTotalCollection);
        }

        private static bool IsStornoCodeFound(int stornoCode)
        {
            return stornoCode == StornoCodeErl ||
                   stornoCode == StornoCodeS001 ||
                   stornoCode == StornoCodeS002 ||
                   stornoCode == StornoCodeS007;
        }

        static void Main(string[] args)
        {
            new KlientNotifications(args);
        }
    }
}
