using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using HTB.Database;
using System.Collections;
using HTB.Database.HTB.StoredProcs;
using HTB.GeocodeService;
using HTBAktLayer;
using HTBExtras.KingBill;
using HTBUtilities;
using System.Net.Mail;
using HTBReports;
using System.IO;
using HTB.Database.Views;
using HTBExtras;
using System.Diagnostics;
using HTB.v2.intranetx.routeplanter;
using System.Collections.Generic;
using System.Net;
using HTB.v2.intranetx.util;
using Microsoft.VisualBasic;
using HTBServices;
using HTBServices.Mail;

namespace HTBDailyKosten
{
    internal class Program
    {
        private static tblControl control = HTBUtils.GetControlRecord();

        private static void Main(string[] args)
        {
            //            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            //TestSystem();
            //TestNextStepManager();
            //TestZinsen();
            //TestWorkflow();
            //            TestOverdue();
            //TestPayments();
            //TestEmail();
            //TestSystem();
            //            TestMahnung();
            //            TestMahnungPdf();
            //            TestAuftragReceipt();
            //TestExtensionRequest();
            //            TestZwischenbericht();
            //TestKlientbericht();
            //TestUebergabeAkten();
            //            TestExcel();
            //            TestMissingBeleg();
            //            TestRV();
            //            TestRoutePlanner();
            //            Console.WriteLine("OUTPUT: " + HTBUtils.ReplaceStringBetween(HTBUtils.ReplaceStringBetween("Rennbahnstrasse 4 Top 10, 5020, Salzburg, Österreich", " top ", ",", ","), "/", ",", ","));
            //            TestSerilization();
            //            ProcessDir("C:\\NintendoDS");
            //            ProcessDir2("C:\\NintendoDS\\Unzip");
            //                        TestFtp();
            //            FixAllAE();
            //            TestProtokol();
            //TestWebServiceNewAkt_Debug();
            //                        TestWebServiceNewAkt();
            //                        TestWebServiceNewAktProduction();
            //            TestWebServiceAktStatus();
            //                        TestWebServiceAktStatusProduction();
            //TestFinancialPmt();
            //TestFinanacial_NumberOfInstallments();
            //             ObjectiveCSql.GenerateSql();
            // TestLogFileConverssion();
            //            TestErlagschein();
            //            TestSmallLinesOfCode();
            //            TestInterverntionAction();
            //TestLawyerMahnung();


            Console.WriteLine(Uri.EscapeUriString("BE Pröll.txt"));
            
            Console.Read();
        }

        private static void TestSmallLinesOfCode()
        {
            var strEmails = "m.haendlhuber@geyrhofer.bmw.at xyz bledi1@yahoo.com bla test@tesrt.at";
            var strEmails2 = "bledi1@gmail.com  test@bla.test adf some thi things.@bla.  things@t12.com";
            string strEmails3 = null;
            Console.WriteLine(string.Format("testig: \t1.  {0}\n\t\t2.  {1}\n\t\t3.  {2}\n\n", strEmails, strEmails2, strEmails3));
            foreach (var valid in (HTBUtils.GetValidEmailAddressesFromStrings(new[] { strEmails, strEmails2, strEmails3 })))
            {
                Console.WriteLine(string.Format("\t[{0}]", valid));
            }
            Console.WriteLine("Done!");

        }
        private static void TestSystem()
        {
            while (true)
            {
                TestNextStepManager();
                TestZinsen();
                TestWorkflow();
                TestZinsen();
                TestOverdue();
                //TestPayments();
                //TestMahnungPdf();
                //TestEmail();
                TestMahnung();
                TestAuftragReceipt();
                TestExtensionRequest();
                Thread.Sleep(1000);
            }
        }

        private static void TestZinsen()
        {
            new HTBInvoiceManager.InvoiceManager().GenerateInterest();
        }

        private static void TestWorkflow()
        {
            new KostenCalculator().CalculateKosten();
        }

        private static void TestPayments()
        {
            for (int i = 0; i < 1; i++)
                TestPayment(19632, 500);

            //TestPayment(19625, 467.14);
            //TestPayment(19625, 500);
            return;
        }

        private static void TestPayment(int aktId, double amount)
        {
            //int aktId = 19625;
            //double amount = 7467.14;
            //double amount = 1000;

            var invMgr = new HTBInvoiceManager.InvoiceManager();
            invMgr.CreateAndSavePayment(aktId, amount);

            ArrayList list = invMgr.GetOpenedPayments(aktId);
            foreach (tblCustInkAktInvoice inv in list)
                invMgr.ApplyPayment(inv.InvoiceID);


        }

        private static void TestOverdue()
        {
            var installMgr = new InstallmentManager();
            installMgr.GenerateOverdueInvoices();
        }

        private static void TestMahnungPdf()
        {
            new MahnungPdfReportGenerator("C:\\temp\\mahnung.xml");
        }

        private static void TestAuftragReceipt()
        {
            string[] wtimeString = control.KlientReceiptTime.Split(':');
            int wrunHour = Convert.ToInt16(wtimeString[0]);
            int wrunMin = Convert.ToInt16(wtimeString[1]);
            var wrunTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, wrunHour, wrunMin, 0);
            DateTime now = DateTime.Now;

            TimeSpan wrunSpan = now.Subtract(wrunTime);
            if (wrunSpan.TotalMinutes >= 1)
            {
                String klientQuery = "SELECT DISTINCT k.klientid AS [IntValue] FROM tblKlient k inner join tblCustInkAkt a " +
                                     " on k.KlientID = a.CustInkAktKlient " +
                                     " WHERE a.CustInkAktEnterDate BETWEEN '" + now.ToShortDateString() + " 00:00.000' AND '" + now.ToShortDateString() + " 23:59.999'";

                ArrayList klientsList = HTBUtils.GetSqlRecords(klientQuery, typeof (SingleValue));

                var email = ServiceFactory.Instance.GetService<IHTBEmail>();

                foreach (SingleValue klientId in klientsList)
                {

                    // make sure we have not already sent a receipt for this klient
                    klientQuery = "SELECT * FROM tblCommunicationLog " +
                                  " WHERE ComLogKlientID = " + klientId.IntValue +
                                  " AND ComLogType = " + tblCommunicationLog.COMMUNICATION_TYPE_RECEIPT +
                                  " AND ComLogDate BETWEEN '" + now.ToShortDateString() + " 00:00.000' AND '" + now.ToShortDateString() + " 23:59.999'";

                    var receiptLog = (tblCommunicationLog) HTBUtils.GetSqlSingleRecord(klientQuery, typeof (tblCommunicationLog));

                    if (receiptLog == null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            var paramaters = new ReportParameters {StartKlient = klientId.IntValue, EndKlient = klientId.IntValue, StartDate = now, EndDate = now};

                            var receipt = new AuftragReceipt();
                            receipt.GenerateClientReceipt(paramaters, stream);
                            var klient = (tblKlient) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKlient WHERE KlientId = " + klientId.IntValue, typeof (tblKlient));
                            if (klient != null)
                            {
                                var emailAddressList = new ArrayList();
                                String body = GetKlientReceiptBody(klient);
                                if (body != null)
                                {
                                    email.SendKlientReceipt(klient, null, body, HTBUtils.ReopenMemoryStream(stream));
                                    SaveKlientAuftragReceipt(receipt.RecordsList, HTBUtils.ReopenMemoryStream(stream), klientId.IntValue);
                                    // insert log record so that we know when the receipt was sent
                                    RecordSet.Insert(new tblCommunicationLog {ComLogKlientID = klientId.IntValue, ComLogType = tblCommunicationLog.COMMUNICATION_TYPE_RECEIPT, ComLogDate = DateTime.Now});
                                }
                            }
                        }
                    }
                }
            }
        }

        private static string GetKlientReceiptBody(tblKlient klient)
        {
            var sb = new StringBuilder();
            if (klient != null)
            {
                var contact = (tblAnsprechpartner) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAnsprechpartner WHERE AnsprechKlient = " + klient.KlientID, typeof (tblAnsprechpartner));
                StreamReader re = File.OpenText(HTBUtils.GetConfigValue("Klient_Receipt_Text"));
                string input = null;
                while ((input = re.ReadLine()) != null)
                {
                    sb.Append(input);
                }
                re.Close();
                re.Dispose();
                if (contact != null)
                {
                    if (contact.AnsprechTitel.Trim().ToUpper() == "HERR")
                    {
                        return sb.ToString().Replace("[name]", "r " + contact.AnsprechTitel + " " + contact.AnsprechNachname);
                    }
                    return sb.ToString().Replace("[name]", " " + contact.AnsprechTitel + " " + contact.AnsprechNachname);
                }
                return sb.ToString().Replace("[name]", " Damen und Herren");
            }
            return null;
        }

        private static void SaveKlientAuftragReceipt(ArrayList recordsList, MemoryStream stream, int klientId)
        {
            string documentsFolder = HTBUtils.GetConfigValue("DocumentsFolder");
            string filename = klientId + "_AB_" + HTBUtils.GetPathTimestamp() + ".pdf";
            HTBUtils.SaveMemoryStream(stream, documentsFolder + filename);
            foreach (spAGReceipt rec in recordsList)
            {
                var doc = new tblDokument
                              {
                                  // CollectionInvoice
                                  DokDokType = 25,
                                  DokCaption = "Auftragsbestätigung",
                                  DokInkAkt = rec.CustInkAktID,
                                  DokCreator = control.AutoUserId,
                                  DokAttachment = filename,
                                  DokCreationTimeStamp = DateTime.Now,
                                  DokChangeDate = DateTime.Now
                              };

                RecordSet.Insert(doc);

                doc = (tblDokument) HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblDokument ORDER BY DokID DESC", typeof (tblDokument));
                if (doc != null)
                {
                    RecordSet.Insert(new tblAktenDokumente {ADAkt = rec.CustInkAktID, ADDok = doc.DokID, ADAkttyp = 1});
                }
            }
        }

        private static void TestMahnung()
        {
            new MahnungManager().GenerateMahnungen(control.AutoUserId);
        }

        private static void TestEmail()
        {
            try
            {
                tblServerSettings serverSettings = (tblServerSettings) HTBUtils.GetSqlSingleRecord("Select * from tblServerSettings", typeof (tblServerSettings));
                // TODO: Add error handling for invalid arguments

                // To
                MailMessage mailMsg = new MailMessage();
                mailMsg.To.Add("bledi1@yahoo.com");

                // From
                MailAddress mailAddress = new MailAddress("E.C.P. Office " + serverSettings.ServerSystemMail);
                mailMsg.From = mailAddress;

                // Subject and Body
                mailMsg.Subject = "Ze Subject";
                mailMsg.Body = "haha";

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient(serverSettings.ServerSMTP);
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(serverSettings.ServerSMTPUser, serverSettings.ServerSMTPPW);
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }

        private static void TestFormPostFromEmail()
        {
            string htmlEmail = "<html>" +
                               "<head>" +
                               "<title>Test</title>" +
                               "</head>" +
                               "<body>" +
                               "    <a href=\"http://localhost/v2/intranetx/auftraggeber/AuftraggeberExtension.aspx?params=" +
                               HTBUtils.EncodeTo64("AG=41,1234") +
                               "\"> check params </a>" +
                               "</body>" +
                               "</html>";
            IHTBEmail email = ServiceFactory.Instance.GetService<IHTBEmail>();
            email.SendGenericEmail(HTBUtils.GetConfigValue("Default_EMail_Addr"), "test form data", htmlEmail);
        }

        private static void TestExtensionRequest()
        {

            string[] wtimeString = control.KlientNotificationTime.Split(':');
            int wrunHour = Convert.ToInt16(wtimeString[0]);
            int wrunMin = Convert.ToInt16(wtimeString[1]);
            DateTime wrunTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, wrunHour, wrunMin, 0);
            DateTime now = DateTime.Now;

            TimeSpan wrunSpan = now.Subtract(wrunTime);
            if (wrunSpan.TotalMinutes >= 1)
            {
                string extCriteria = "AktIntExtIsRequestSent = 0 and AktIntExtApprovedDate = '01.01.1900' AND AktIntExtDeniedDate = '01.01.1900'";
                String agQuery = "SELECT AuftraggeberId as[IntValue],  AKTIntAGSB AS [StringValue] FROM qryAktenIntExtension WHERE " + extCriteria + " GROUP BY AuftraggeberId, AKTIntAGSB";

                ArrayList agList = HTBUtils.GetSqlRecords(agQuery, typeof (SingleValue));

                IHTBEmail email = ServiceFactory.Instance.GetService<IHTBEmail>();
                RecordSet set = new RecordSet();

                foreach (SingleValue agSB in agList)
                {
                    if (agSB.StringValue.Trim() != string.Empty)
                    {

                        // make sure we have not already sent a request to this auftraggeber
                        agQuery = "SELECT * FROM tblCommunicationLog " +
                                  " WHERE ComLogAuftraggeberID = " + agSB.IntValue +
                                  " AND ComLogAuftraggeberSB = '" + agSB.StringValue + "'" +
                                  " AND ComLogType = " + tblCommunicationLog.COMMUNICATION_TYPE_EXTENSION_REQUEST +
                                  " AND ComLogDate BETWEEN '" + now.ToShortDateString() + " 00:00.000' AND '" + now.ToShortDateString() + " 23:59.999'";

                        tblCommunicationLog receiptLog = (tblCommunicationLog) HTBUtils.GetSqlSingleRecord(agQuery, typeof (tblCommunicationLog));

                        if (receiptLog == null)
                        {
                            qryAktenIntExtension ext = (qryAktenIntExtension) HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenIntExtension WHERE AuftraggeberID = " + agSB.IntValue + " AND AKTIntAGSB = '" + agSB.StringValue + "' AND " + extCriteria, typeof (qryAktenIntExtension));
                            if (ext != null)
                            {
                                ArrayList emailAddressList = new ArrayList();
                                String body = GetAGExtensionRequestBody(ext);
                                if (body != null)
                                {
                                    ArrayList toList = new ArrayList();
                                    toList.Add(HTBUtils.GetConfigValue("Default_EMail_Addr"));
                                    body = "TO: [" + ext.AKTIntKSVEMail + "]<BR>" + body;

                                    string[] toAddress = new string[toList.Count];
                                    for (int i = 0; i < toList.Count; i++)
                                        toAddress[i] = toList[i].ToString();

                                    email.SendGenericEmail(toAddress, "Verlängerungsanfrage", body, true);

                                    // insert log record so that we know when the receipt was sent
                                    receiptLog = new tblCommunicationLog();
                                    receiptLog.ComLogAuftraggeberID = ext.AuftraggeberID;
                                    receiptLog.ComLogAuftraggeberSB = ext.AKTIntAGSB;
                                    receiptLog.ComLogType = tblCommunicationLog.COMMUNICATION_TYPE_EXTENSION_REQUEST;
                                    receiptLog.ComLogDate = DateTime.Now;
                                    set.InsertRecord(receiptLog);
                                    string extReqUpdate = "UPDATE qryAktenIntExtension set AktIntExtIsRequestSent = 1 WHERE AuftraggeberID = " + agSB.IntValue + " AND AKTIntAGSB = '" + ext.AKTIntAGSB + "'";
                                    set.ExecuteNonQuery(extReqUpdate);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static string GetAGExtensionRequestBody(qryAktenIntExtension ext)
        {
            StringBuilder sb = new StringBuilder();
            if (ext != null)
            {
                StreamReader re = File.OpenText(HTBUtils.GetConfigValue("AG_Extension_Req_Text"));
                string input = null;
                while ((input = re.ReadLine()) != null)
                {
                    sb.Append(input);
                }
                re.Close();
                re.Dispose();
                return sb.ToString().Replace("[name]", ext.AKTIntAGSB).Replace("[AG_EXTENSION_HREF]",
                                                                               HTBUtils.GetConfigValue("URL_Extension_Req") + HTBUtils.EncodeTo64("AG=" + ext.AuftraggeberID + "&AGSB=" + ext.AKTIntAGSB));
            }
            return null;
        }

        private static void TestNextStepManager()
        {
            var mgr = new NextStepManager();
            mgr.GenerateIntNextStep();
            mgr.GenerateMeldeNextStep();
        }

        private static void TestZwischenbericht()
        {
            int aktId = 22127;
            qryCustInkAkt akt = (qryCustInkAkt) HTBUtils.GetSqlSingleRecord("SELECT * FROM qryCustInkAkt WHERE CustInkAktID = " + aktId, typeof (qryCustInkAkt));
            if (akt != null)
            {
                ArrayList aktionsList = new ArrayList();

                #region Load Records

                ArrayList interventionAktions = new ArrayList();
                ArrayList invoiceList = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + aktId, typeof (tblCustInkAktInvoice));
                ArrayList inkassoAktions = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkAktAktionen WHERE CustInkAktAktionAktID = " + aktId + " ORDER BY CustInkAktAktionDate", typeof (qryCustInkAktAktionen));
                tblAktenInt intAkt = (tblAktenInt) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenInt WHERE AktIntCustInkAktID = " + aktId + " ORDER BY AktIntID DESC", typeof (tblAktenInt));
                if (intAkt != null)
                    interventionAktions = HTBUtils.GetSqlRecords("SELECT * FROM qryInktAktAction WHERE AktIntActionAkt = " + intAkt.AktIntID + " ORDER BY AktIntActionDate DESC", typeof (qryInktAktAction));

                #endregion

                #region CombineActions

                foreach (qryCustInkAktAktionen inkAction in inkassoAktions)
                    aktionsList.Add(new InkassoActionRecord(inkAction, intAkt));

                foreach (qryInktAktAction intAction in interventionAktions)
                    aktionsList.Add(new InkassoActionRecord(intAction, intAkt));

                aktionsList.Sort(new InkassoActionRecordComparer());

                #endregion

                #region Send Zwischenbericht

                /* HTBReports.Zwischenbericht mahMgr = new HTBReports.Zwischenbericht();
                using (MemoryMappedFile mmf = MemoryMappedFile.CreateNew("Zwischenbericht" + akt.CustInkAktID + ".pdf", 5000000))
                {
                    using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                    {
                        mahMgr.GenerateZwischenbericht(akt, aktionsList, "TEST text here", stream);
                        HTBEmail email = new HTBEmail();
                        email.SendKlientReceipt(new string[]{"b.bitri@ecp.or.at"}, "here it is", mmf.CreateViewStream());
                    }
                }
                 */

                #endregion

                #region Save Zwischenbericht to file

                string fileName = "c:/temp/Zwischenbericht.pdf";
                var mahMgr = new Zwischenbericht();
                mahMgr.GenerateZwischenbericht(akt, aktionsList, "das ist ein test bericht... bla bla test hob i g'sogt!", new FileStream(fileName, FileMode.OpenOrCreate));
                var proc = new Process();
                proc.StartInfo.FileName = fileName;
                proc.Start();

                #endregion
            }
        }

        private static void TestKlientbericht()
        {
            int klientId = 13439;
            ArrayList aktList = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkAkt WHERE KlientId = " + klientId, typeof (qryCustInkAkt));
            string fileName = "c:/temp/Klientbericht.pdf";
            var rpt = new Zwischenbericht();
            rpt.Open(new FileStream(fileName, FileMode.OpenOrCreate));
            bool firstAkt = true;
            foreach (qryCustInkAkt akt in aktList)
            {
                if (akt != null)
                {
                    ArrayList aktionsList = new ArrayList();

                    #region Load Records

                    ArrayList interventionAktions = new ArrayList();
                    ArrayList invoiceList = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + akt.CustInkAktID, typeof (tblCustInkAktInvoice));
                    ArrayList inkassoAktions = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkAktAktionen WHERE CustInkAktAktionAktID = " + akt.CustInkAktID + " ORDER BY CustInkAktAktionDate", typeof (qryCustInkAktAktionen));
                    tblAktenInt intAkt = (tblAktenInt) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenInt WHERE AktIntCustInkAktID = " + akt.CustInkAktID + " ORDER BY AktIntID DESC", typeof (tblAktenInt));
                    if (intAkt != null)
                    {
                        interventionAktions = HTBUtils.GetSqlRecords("SELECT * FROM qryInktAktAction WHERE AktIntActionAkt = " + intAkt.AktIntID + " ORDER BY AktIntActionDate DESC", typeof (qryInktAktAction));
                        if (intAkt.AKTIntMemo.Trim() != string.Empty)
                        {
                            InkassoActionRecord action = new InkassoActionRecord();
                            action.ActionDate = DateTime.Now;
                            action.ActionCaption = "Interventionsmemo:";
                            action.ActionMemo = intAkt.AKTIntMemo;
                            action.IsOnlyMemo = true;
                            aktionsList.Add(action);
                        }
                    }

                    #endregion

                    #region CombineActions

                    foreach (qryCustInkAktAktionen inkAction in inkassoAktions)
                        aktionsList.Add(new InkassoActionRecord(inkAction, intAkt));

                    foreach (qryInktAktAction intAction in interventionAktions)
                        aktionsList.Add(new InkassoActionRecord(intAction, intAkt));

                    aktionsList.Sort(new InkassoActionRecordComparer());

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
                    rpt.GenerateZwischenbericht(akt, aktionsList, "", null, false);

                    #endregion
                }
            }
            rpt.PrintFooter();
            rpt.CloseReport();

            #region Show PDF File

            var proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.Start();

            #endregion
        }

        private static void TestUebergabeAkten()
        {

            ArrayList aktList = HTBUtils.GetSqlRecords("SELECT * FROM qryAkten ", typeof (qryAkten));

            #region Save Uebergabeakten to file

            string fileName = "c:/temp/UebergabenAkten.pdf";
            UebergabenAkten mahMgr = new UebergabenAkten();
            mahMgr.GenerateUebergabenAktenPDF(aktList, new FileStream(fileName, FileMode.OpenOrCreate));
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.Start();

            #endregion

        }

        private static void TestExcel()
        {
            new HTBAktLayer.AktUtils(22155).SendInkassoPackageToLawyer(new HTBEmailAttachment(ReportFactory.GetZwischenbericht(22155), "Zwischenbericht.pdf", "Application/pdf"));
        }

        private static void TestMissingBeleg()
        {
            var sqlWhere = "WHERE (KbMissReceivedDate IS NULL OR KbMissReceivedDate = '01.01.1900') ";
            var missingBelegUserList = HTBUtils.GetSqlRecords("SELECT distinct (KbMissUser) IntValue FROM tblKassaBlockMissingNr " + sqlWhere, typeof (SingleValue));
            foreach (SingleValue missingUserIdRec in missingBelegUserList)
            {
                var missingRec = (tblKassaBlockMissingNr) HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblKassaBlockMissingNr " + sqlWhere + " AND KbMissUser = " + missingUserIdRec.IntValue + " ORDER BY KbMissDate DESC", typeof (tblKassaBlockMissingNr));
                if ((DateTime.Now.Subtract(missingRec.KbMissDate)).Days >= 7)
                {
                    var user = (tblUser) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserID = " + missingRec.KbMissUser, typeof (tblUser));
                    user.UserStatus = 0;
                    RecordSet.Update(user);
                    ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail(new string[] {HTBUtils.GetConfigValue("Default_EMail_Addr"), "b.bitri@ecp.or.at"}, "Login gesperrt für Benutzer: " + user.UserVorname + " " + user.UserNachname, "Belege fehlen");

                    #region Notify when last beleg in block gets used

                    #endregion
                }
            }
        }

        private static void TestRV()
        {

            var akt = (qryCustInkAkt) HTBUtils.GetSqlSingleRecord("SELECT * FROM qryCustInkAkt WHERE CustInkAktID = 22127", typeof (qryCustInkAkt));
            var ms = new MemoryStream();
            var rpt = new Ratenansuchen();
            rpt.GenerateRatenansuchen(akt, ms, true);

            FileStream fs = File.OpenWrite("c:\\temp\\RV.pdf");
            //            ms.Seek(0, SeekOrigin.Begin);
            //            ms.WriteTo(fs);
            fs.Write(ms.ToArray(), 0, ms.ToArray().Length);
            fs.Close();
            fs.Dispose();
            Process.Start("c:\\temp\\RV.pdf");
        }

        #region RoutePlaner

        private static readonly RoutePlanerManager rpManager = new RoutePlanerManager(9999, true, "123");

        private static void TestRoutePlanner()
        {
            rpManager.Clear();
            string aktIdList = "146241,146240,146239,146238,146237,146236,146235,146234,146233,146232,146231,146230,146229,146228,146227,146226,146225,146221,146220,146219,146218,146217,146216,146215,146214,146213,146212,146205,146204,146203";

            //            string aktIdList = "146241,146240,146239,146238,146237,146236,146235,146234,146233,146232,146231,146230,146229";
            if (aktIdList.Trim().Length > 0)
            {
                string qryAktCommand = "SELECT * FROM dbo.qryAktenInt WHERE AktIntID in (" + aktIdList + ")";
                ArrayList aktenList = HTBUtils.GetSqlRecords(qryAktCommand, typeof (qryAktenInt));
                LoadAddresses(aktenList);
                rpManager.CalculateBestRoute();
                Console.WriteLine("LOCATIONS\n===========================\n");
                foreach (City city in rpManager.Cities)
                {
                    Console.WriteLine(city.Address.ID + "&nbsp;&nbsp;" + city.Address.Address + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;LAT:" + city.Location.Locations[0].Latitude + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;LGN:" + city.Location.Locations[0].Longitude + "<br>");
                }

                Console.WriteLine("\nROADS\n===========================\n");
                foreach (Road road in rpManager.Roads)
                {
                    Console.WriteLine(road.From.Address.ID + "&nbsp;&nbsp;" + road.From.Address.Address + " =====> " + road.To.Address.ID + "&nbsp;&nbsp;" + road.To.Address + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + road.Distance);
                }
            }
        }

        private static void LoadAddresses(ArrayList akten)
        {
            foreach (qryAktenInt akt in akten)
            {
                var address = new StringBuilder(HTBUtils.ReplaceStringAfter(HTBUtils.ReplaceStringAfter(akt.GegnerLastStrasse, " top ", ""), "/", ""));
                address.Append(",");
                address.Append(akt.GegnerLastZip);
                address.Append(",");
                address.Append(akt.GegnerLastOrt);
                address.Append(",österreich");

                rpManager.AddAddress(new AddressWithID(akt.AktIntID, address.ToString()));
            }

        }

        #endregion

        private static void TestLawyerEmail()
        {

        }

        private static void TestSerilization()
        {
            var addr = new AddressWithID(100, "some street");
            addr.SuggestedAddresses.Add(new AddressLocation(new AddressWithID(200, "bla bla"), new GeocodeLocation[]
                                                                                                   {
                                                                                                       new GeocodeLocation
                                                                                                           {
                                                                                                               Latitude = 0,
                                                                                                               Longitude = 1
                                                                                                           }
                                                                                                   }));
            string str = HTBUtils.SerializeObject(addr, typeof (AddressWithID));
        }


        public static void ProcessDir(string sourceDir)
        {

            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(sourceDir);
            foreach (string fileName in fileEntries)
            {
                if (sourceDir != "C:\\NintendoDS" && sourceDir != "C:\\NintendoDS\\AllFiles")
                {
                    if (fileName.ToLower().EndsWith(".zip"))
                    {
                        try
                        {
                            File.Copy(fileName, "C:\\NintendoDS\\AllFiles\\" + sourceDir.Substring(sourceDir.LastIndexOf("\\")) + ".zip");
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                        Console.WriteLine(fileName.Substring(fileName.LastIndexOf("\\")));
                    }

                    else if (fileName.ToLower().EndsWith(".rar"))
                    {
                        try
                        {
                            File.Copy(fileName, "C:\\NintendoDS\\AllFiles\\" + sourceDir.Substring(sourceDir.LastIndexOf("\\")) + ".rar");
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                        Console.WriteLine(fileName.Substring(fileName.LastIndexOf("\\")));
                    }
                    else
                    {
                        for (int i = 0; i < 99; i++)
                        {
                            string ext = GetRarFileExtension(i);
                            if (fileName.ToLower().EndsWith(ext))
                            {
                                try
                                {
                                    File.Copy(fileName, "C:\\NintendoDS\\AllFiles\\" + sourceDir.Substring(sourceDir.LastIndexOf("\\")) + "." + ext);
                                }
                                catch (Exception e)
                                {
                                    throw e;
                                }

                                Console.WriteLine(fileName.Substring(fileName.LastIndexOf("\\")));
                            }
                        }
                    }
                }
            }
            // Recurse into subdirectories of this directory.
            string[] subdirEntries = Directory.GetDirectories(sourceDir);
            foreach (string subdir in subdirEntries)
                // Do not iterate through reparse points
                if ((File.GetAttributes(subdir) &
                     FileAttributes.ReparsePoint) !=
                    FileAttributes.ReparsePoint)
                    ProcessDir(subdir);

        }

        public static void ProcessDir2(string sourceDir)
        {

            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(sourceDir);
            foreach (string fileName in fileEntries)
            {
                if (sourceDir != "C:\\NintendoDS\\Unzip" && sourceDir != "C:\\NintendoDS\\Unzip\\Games")
                {
                    if (fileName.ToLower().EndsWith(".nds"))
                    {
                        try
                        {
                            string fname = sourceDir.Substring(sourceDir.LastIndexOf("\\") + 1);
                            fname = fname.Substring(fname.IndexOf("_") + 1);
                            fname = fname.Substring(0, fname.LastIndexOf("_"));
                            File.Copy(fileName, "C:\\NintendoDS\\Unzip\\Games\\" + fname + ".nds");
                        }
                        catch (Exception)
                        {
                            //throw e;
                        }

                        Console.WriteLine(fileName.Substring(fileName.LastIndexOf("\\")));
                    }

                }
            }
            // Recurse into subdirectories of this directory.
            string[] subdirEntries = Directory.GetDirectories(sourceDir);
            foreach (string subdir in subdirEntries)
                // Do not iterate through reparse points
                if ((File.GetAttributes(subdir) &
                     FileAttributes.ReparsePoint) !=
                    FileAttributes.ReparsePoint)
                    ProcessDir2(subdir);


        }

        private static string GetRarFileExtension(int idx)
        {
            return (idx < 10) ? "r0" + idx : "r" + idx;
        }

        private static void TestFtp()
        {
            HTBSftp.SendFile("c:/temp/Mahnung.xml");
        }

        private static void FixAllAE()
        {
            var list = new List<AeCountRecord>();
            using (StreamWriter file = new System.IO.StreamWriter(@"C:\temp\AktivResults.txt"))
            {
                foreach (string f in Directory.GetFiles("C:\\temp\\bledi_aktiv\\test"))
                    //                foreach (string f in Directory.GetFiles("C:\\temp\\bledi_aktiv"))
                {
                    if (f.EndsWith(".txt"))
                    {
                        Console.WriteLine(f);
                        FixAE(file, list, f);
                    }
                }

                foreach (var rec in list)
                {
                    //                    file.Write(rec.dateList[0] + "\t" + rec.caption + "\t" + (rec.count - 1));
                    //                    for (int i = 0; i < rec.priceList.Count; i++ )
                    //                    {
                    //                        if(i > 0)
                    //                            file.Write("\t" + rec.dateList[i] + "    "+rec.priceList[i]);
                    //                        else
                    //                            file.Write("\t" + rec.priceList[i]);
                    //                    }
                    //                    file.WriteLine("");


                    for (int i = 0; i < rec.priceList.Count; i++)
                    {
                        double price = 0;
                        try
                        {
                            price = Convert.ToDouble(rec.priceList[i].Replace(",", "."));
                        }
                        catch (Exception)
                        {

                            price = 0;
                        }


                        if (price > 0)
                            file.WriteLine(rec.dateList[i] + "\t" + rec.caption + "\t" + (rec.count - 1) + "\t" + rec.priceList[i]);
                    }

                }
            }
        }

        private static void FixAE(StreamWriter file, List<AeCountRecord> list, string fileName = null)
        {
            if (fileName == null)
                fileName = @"c:\temp\aeshit\2\aeshit.txt";

            string text = HTBUtils.GetFileText(fileName);

            var sb = new StringBuilder();
            string dateText = "Salzburg, am ";
            bool lookForAktivNumber = true;

            string startToken = "Unser AZ: ";
            string endToken = "Ihr AZ: ";
            if (lookForAktivNumber)
            {
                startToken = "Ihr AZ:";
                endToken = "Anschrifterhebung:";
            }

            string priceTextStart = "Kosten der Erhebung € ";
            string priceTextEnd = ",";

            int dateIdx = text.IndexOf(dateText);
            string dateString = text.Substring(dateIdx + dateText.Length, 10);
            int startIdx = text.IndexOf(startToken);
            startIdx += startToken.Length;
            int endIdx = text.IndexOf(endToken, startIdx);

            int startPriceIdx = text.IndexOf(priceTextStart, startIdx);
            startPriceIdx += priceTextStart.Length;
            int endPriceIdx = text.IndexOf(priceTextEnd, startPriceIdx) + 3; // cent ,00

            //            Console.WriteLine("Fixing AE");
            Console.WriteLine("Text Length: " + text.Length);


            while (startIdx > 10 && endIdx > startIdx)
            {
                //Console.WriteLine("[ " + startIdx +" - " +(endIdx - startIdx)+" ]");
                string token = text.Substring(startIdx, endIdx - startIdx - 1).Trim();
                string price = text.Substring(startPriceIdx, endPriceIdx - startPriceIdx).Trim();
                bool found = false;
                AeCountRecord aeRec = null;
                foreach (var rec in list)
                {
                    if (rec.caption.Equals(token))
                    {
                        aeRec = rec;
                    }
                }
                if (aeRec == null)
                    list.Add(new AeCountRecord(token, price, dateString));
                else
                    aeRec.Increment(price, dateString);

                startIdx = text.IndexOf(startToken, endIdx);
                startIdx += startToken.Length;
                endIdx = text.IndexOf(endToken, startIdx);

                startPriceIdx = text.IndexOf(priceTextStart, endPriceIdx);
                startPriceIdx += priceTextStart.Length;
                endPriceIdx = text.IndexOf(priceTextEnd, startPriceIdx) + 3;
            }
            //Console.WriteLine(sb.ToString());
            Console.WriteLine("DONE!");
        }

        private static void TestProtokol()
        {
            // ProtocolId Reposession (mercedes): 2237 
            // ProtocolId Reposession: 2244
            // ProtocolId Collection: 2245 

            //var protocol = (tblProtokol) HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblProtokol where protokolid = 2244 order by ProtokolID DESC", typeof (tblProtokol));
            //var protocol = (tblProtokol) HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblProtokol where aktintid = 221508 order by ProtokolID DESC", typeof (tblProtokol));
            var protocol = (tblProtokol)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblProtokol order by ProtokolID DESC", typeof(tblProtokol));
            //protocol.ProtokolAkt = 221504; // test
            var akt = (qryAktenInt) HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenInt WHERE AktIntID = " + protocol.ProtokolAkt, typeof (qryAktenInt));
            var action = (qryAktenIntActionWithType) HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenIntActionWithType WHERE AktIntActionAkt = " + akt.AktIntID + " and AktIntActionIsInternal = 0 ORDER BY AktIntActionTime DESC", typeof (qryAktenIntActionWithType));
            var protokolUbername = (tblProtokolUbername)HTBUtils.GetSqlSingleRecord($"SELECT * FROM tblProtokolUbername WHERE UbernameAktIntID = { protocol.ProtokolAkt } ORDER BY UbernameDatum DESC", typeof(tblProtokolUbername));

            ArrayList docsList = HTBUtils.GetSqlRecords("SELECT * FROM qryDoksIntAkten WHERE AktIntID = " + akt.AktIntID, typeof(qryDoksIntAkten));

            var fileName = "Protocol_" + protocol.ProtokolAkt + ".pdf";
            string filepath = HTBUtils.GetConfigValue("DocumentsFolder") + fileName;
            FileStream ms = null;
            ms = File.Exists(filepath) ? new FileStream(filepath, FileMode.Truncate) : new FileStream(filepath, FileMode.Create);

            var rpt = new ProtokolTablet();
            var emailAddresses = new List<string>();
            emailAddresses.AddRange(HTBUtils.GetConfigValue("Office_Email").Split(' '));
            emailAddresses.AddRange(akt.AuftraggeberEMail.Split(' '));
            emailAddresses.AddRange(protocol.HandlerEMail.Split(' '));
            ///*
            rpt.GenerateProtokol(akt, 
                protocol, 
                protokolUbername,
                action, ms, 
                GlobalUtilArea.GetVisitedDates(akt.AktIntID), 
                GlobalUtilArea.GetPosList(akt.AktIntID), 
                docsList.Cast<Record>().ToList(), 
                emailAddresses);
                //*/

            //rpt.GenerateDealerProtokol(akt, protocol, ms, emailAddresses);


            ms.Close();
            ms.Dispose();
            Thread.Sleep(100);
            Process.Start(HTBUtils.GetConfigValue("DocumentsFolder").Replace("/", "\\") + fileName);
        }

        private static void TestWebServiceNewAkt_Debug()
        {
            var newAktService = new WsInkassoDebug.WsNewInkassoSoapClient();

            var akt = new InkassoAkt
                          {
                              RechnungDatum = new DateTime(2013, 10, 18),
                              RechnungNummer = "Rechnung Nr. 2013-224",
                              RechnungReferencenummer = "",
                              RechnungForderungOffen = 132,
                              RechnungMahnKosten = 0,
                              RechnungMemo = "",
                              KlientAnrede = "Herr",
                              KlientTitel = "",
                              KlientVorname = "Anton Kraushofer",
                              KlientNachname = "",
                              KlientGeschlecht = 2,
                              KlientStrasse = "Neugebäudeplatz 10",
                              KlientPLZ = "3100",
                              KlientOrt = "St. Pölten",
                              KlientLKZ = "A",
                              //                KlientTelefonVorwahlLand = "43",
                              //                KlientTelefonVorwahl = "660",
                              KlientTelefonNummer = "027422 7 65",
                              KlientEMail = "office@fire-control.at",
                              KlientBank = "Volksbank NÖ-Mitte",
                              KlientKontonummer = "VBOEATWWNOM",
                              KlientBLZ = "47150",
                              KlientFirmenbuchnummer = "",
                              //                KlientVersicherungnummer = "Versicherung: 121241343434",
                              //                KlientPolizzennummer = "12 12 -21012091290",
                              KlientAnschprechPartnerAnrede = "Frau",
                              KlientAnschprechPartnerVorname = "Maria",
                              KlientAnschprechPartnerNachname = "Mayr",
                              KlientAnschprechPartnerGeburtsdatum = new DateTime(1990, 5, 6),
                              KlientAnschprechPartnerTelefonNummer = "43 662 445566",
                              KlientAnschprechPartnerEMail = "m.mayr@klientssite.at",
                              //                KlientMemo = "Disney aint paying lately.\nGots to collect!",
                              SchuldnerAnrede = "",
                              SchuldnerGeschlecht = 2,
                              SchuldnerVorname = "",
                              SchuldnerNachname = "KFZ - Sandisic",
                              SchuldnerGeburtsdatum = DateTime.MinValue,
                              SchuldnerLKZ = "AT",
                              SchuldnerStrasse = "Gross Hain 31",
                              SchuldnerPLZ = "3170",
                              SchuldnerOrt = "Wien",
                              //                SchuldnerTelefonVorwahlLand = "43",
                              //                SchuldnerTelefonVorwahl = "1",
                              SchuldnerTelefonNummer = "",
                              SchuldnerEMail = "",
                              SchuldnerMemo = "",
                              WorkflowErsteMahnung = true,
                              WorkflowErsteMahnungsfrist = 21,
                              WorkflowIntervention = true,
                              WorkflowInterventionsfrist = 21,
                              WorkflowZweiteMahnung = true,
                              WorkflowZweiteMahnungsfrist = 14,
                              WorkflowDritteMahnung = true,
                              WorkflowDritteMahnungsfrist = 14,
                              WorkflowRechtsanwaltMahnung = true
                          };
            akt.Dokumente.Add(new InkassoAktDokument
                                  {
                                      DokumentBeschreibung = "Rechnung",
                                      DokumentURL = "http://localhost/v2/intranet/documents/files/22228_Kosten.xls"
                                  });
                        string newAktResponse = newAktService.CreateNewAkt(akt.ToXmlString());
            //string newAktResponse = newAktService.CreateNewAkt(HTBUtils.GetFileText("C:\\temp\\TestAktData.txt"));

            Console.WriteLine(newAktResponse);
        }

        private static void TestWebServiceNewAkt()
        {
            var newAktService = new WsInkasso.WsNewInkassoSoapClient();

            var akt = new InkassoAkt
                          {
                              RechnungDatum = DateTime.Now,
                              RechnungNummer = "123 123",
                              RechnungReferencenummer = "",
                              RechnungForderungOffen = 100,
                              RechnungMahnKosten = 10,
                              RechnungMemo = "some memo",
                              KlientAnrede = "Herr",
                              KlientTitel = "Dr.",
                              KlientVorname = "Mickey",
                              KlientNachname = "Mouse",
                              KlientGeschlecht = 1,
                              KlientStrasse = "Rennbahnstrasse 10",
                              KlientPLZ = "5020",
                              KlientOrt = "Salzburg",
                              KlientLKZ = "A",
                              //                              KlientTelefonVorwahlLand = "43",
                              //                              KlientTelefonVorwahl = "660",
                              KlientTelefonNummer = "43 660 521144",
                              KlientEMail = "b.bitri@ecp.or.at",
                              KlientFirmenbuchnummer = "Firma: 123123123",
                              //                              KlientVersicherungnummer = "Versicherung: 121241343434",
                              //                              KlientPolizzennummer = "12 12 -21012091290",
                              KlientAnschprechPartnerAnrede = "Frau",
                              KlientAnschprechPartnerVorname = "Maria",
                              KlientAnschprechPartnerNachname = "Mayr",
                              KlientAnschprechPartnerGeburtsdatum = new DateTime(1990, 5, 6),
                              //                              KlientAnschprechPartnerTelefonVorwahlLand = "43",
                              //                              KlientAnschprechPartnerTelefonVorwahl = "662",
                              KlientAnschprechPartnerTelefonNummer = "445566",
                              KlientAnschprechPartnerEMail = "m.mayr@klientssite.at",
                              //                              KlientMemo = "Disney aint paying lately.\nGots to collect!",
                              SchuldnerAnrede = "Herr",
                              SchuldnerGeschlecht = 1,
                              SchuldnerVorname = "Franz",
                              SchuldnerNachname = "Podolsky",
                              SchuldnerGeburtsdatum = new DateTime(1980, 10, 4),
                              SchuldnerLKZ = "A",
                              SchuldnerStrasse = "Aignerstrasse 11",
                              SchuldnerPLZ = "1010",
                              SchuldnerOrt = "Wien",
                              //                              SchuldnerTelefonVorwahlLand = "43",
                              //                              SchuldnerTelefonVorwahl = "1",
                              SchuldnerTelefonNummer = "20520",
                              SchuldnerEMail = "f.podolskly@schuldnerdomain.at",
                              SchuldnerMemo = "Sie vaehrt ein 100000 Euro Auto",
                              WorkflowErsteMahnung = true,
                              WorkflowErsteMahnungsfrist = 12,
                              WorkflowIntervention = true,
                              WorkflowInterventionsfrist = 21,
                              WorkflowZweiteMahnung = true,
                              WorkflowZweiteMahnungsfrist = 8,
                              WorkflowDritteMahnung = true,
                              WorkflowDritteMahnungsfrist = 15,
                              WorkflowRechtsanwaltMahnung = true
                          };
            akt.Dokumente.Add(new InkassoAktDokument
                                  {
                                      DokumentBeschreibung = "Rechnung",
                                      DokumentURL = "http://localhost/v2/intranet/documents/files/22228_Kosten.xls"
                                  });
            //            string newAktResponse = newAktService.CreateNewAkt(akt.ToXmlString());
            string newAktResponse = newAktService.CreateNewAkt(HTBUtils.GetFileText("C:\\temp\\TestAktData.txt"));

            Console.WriteLine(newAktResponse);
        }
       
        private static void TestWebServiceAktStatus()
        {
            var newAktService = new WsInkasso.WsNewInkassoSoapClient();

            string response = newAktService.GetAktStatus(22225);
            Console.WriteLine(response);

            var ds = new DataSet();
            ds.ReadXml(new StringReader(response));
            var status = new InkassoAktStatusResponse();

            foreach (DataTable tbl in ds.Tables)
            {
                if (tbl.TableName.ToUpper().Trim() == typeof (InkassoAktStatusResponse).Name.ToUpper())
                {
                    foreach (DataRow dr in tbl.Rows) // there should be only one row but one never knows
                        status.LoadFromDataRow(dr);

                }
                if (tbl.TableName.ToUpper().Trim() == typeof (Aktion).Name.ToUpper())
                {
                    foreach (DataRow dr in tbl.Rows)
                    {
                        var rec = new Aktion();
                        rec.LoadFromDataRow(dr);
                        status.Aktionen.Add(rec);
                    }
                }
            }
            Console.WriteLine("CollectionInvoice Memo: " + status.Inkassomemo);
            Console.WriteLine("Intervention Memo: " + status.Interventionsmemo);

            foreach (Aktion aktion in status.Aktionen)
            {

                Console.WriteLine(aktion.AktionDatum.ToShortDateString() + " " + aktion.AktionDatum.ToShortTimeString() + " " + aktion.AktionBeschreibung + " " + aktion.AktionMemo);
            }
        }

        private static void TestWebServiceNewAktProduction()
        {
            var newAktService = new WsInkassoProduction.WsNewInkassoSoapClient();

            var akt = new InkassoAkt
                          {
                              RechnungDatum = new DateTime(2013, 10, 18),
                              RechnungNummer = "Rechnung Nr. 2013-224",
                              RechnungReferencenummer = "",
                              RechnungForderungOffen = 132,
                              RechnungMahnKosten = 0,
                              RechnungMemo = "",
                              KlientAnrede = "Herr",
                              KlientTitel = "",
                              KlientVorname = "Anton Kraushofer",
                              KlientNachname = "",
                              KlientGeschlecht = 2,
                              KlientStrasse = "Neugebäudeplatz 10",
                              KlientPLZ = "3100",
                              KlientOrt = "St. Pölten",
                              KlientLKZ = "A",
                              //                KlientTelefonVorwahlLand = "43",
                              //                KlientTelefonVorwahl = "660",
                              KlientTelefonNummer = "027422 7 65",
                              KlientEMail = "office@fire-control.at",
                              KlientBank = "Volksbank NÖ-Mitte",
                              KlientKontonummer = "VBOEATWWNOM",
                              KlientBLZ = "47150",
                              KlientFirmenbuchnummer = "",
                              //                KlientVersicherungnummer = "Versicherung: 121241343434",
                              //                KlientPolizzennummer = "12 12 -21012091290",
                              KlientAnschprechPartnerAnrede = "Frau",
                              KlientAnschprechPartnerVorname = "Maria",
                              KlientAnschprechPartnerNachname = "Mayr",
                              KlientAnschprechPartnerGeburtsdatum = new DateTime(1990, 5, 6),
                              KlientAnschprechPartnerTelefonNummer = "43 662 445566",
                              KlientAnschprechPartnerEMail = "m.mayr@klientssite.at",
                              //                KlientMemo = "Disney aint paying lately.\nGots to collect!",
                              SchuldnerAnrede = "",
                              SchuldnerGeschlecht = 2,
                              SchuldnerVorname = "",
                              SchuldnerNachname = "KFZ - Sandisic",
                              SchuldnerGeburtsdatum = DateTime.MinValue,
                              SchuldnerLKZ = "AT",
                              SchuldnerStrasse = "Gross Hain 31",
                              SchuldnerPLZ = "3170",
                              SchuldnerOrt = "Wien",
                              //                SchuldnerTelefonVorwahlLand = "43",
                              //                SchuldnerTelefonVorwahl = "1",
                              SchuldnerTelefonNummer = "",
                              SchuldnerEMail = "",
                              SchuldnerMemo = "",
                              WorkflowErsteMahnung = true,
                              WorkflowErsteMahnungsfrist = 21,
                              WorkflowIntervention = true,
                              WorkflowInterventionsfrist = 21,
                              WorkflowZweiteMahnung = true,
                              WorkflowZweiteMahnungsfrist = 14,
                              WorkflowDritteMahnung = true,
                              WorkflowDritteMahnungsfrist = 14,
                              WorkflowRechtsanwaltMahnung = true
                          };
            akt.Dokumente.Add(new InkassoAktDokument
                                  {
                                      DokumentBeschreibung = "Rechnung",
                                      DokumentURL = "http://localhost/v2/intranet/documents/files/22228_Kosten.xls"
                                  });
            //string newAktResponse = newAktService.CreateNewAkt(akt.ToXmlString());
            string newAktResponse = newAktService.CreateNewAkt(HTBUtils.GetFileText("C:\\temp\\TestAktData.txt"));

            Console.WriteLine(akt.ToXmlString());

            Console.WriteLine(newAktResponse);
        }

        private static void TestWebServiceAktStatusProduction()
        {
            var newAktService = new WsInkassoProduction.WsNewInkassoSoapClient();

            string response = newAktService.GetAktStatus(32519);
            Console.WriteLine(response);

            var ds = new DataSet();
            ds.ReadXml(new StringReader(response));
            var status = new InkassoAktStatusResponse();

            foreach (DataTable tbl in ds.Tables)
            {
                if (tbl.TableName.ToUpper().Trim() == typeof (InkassoAktStatusResponse).Name.ToUpper())
                {
                    foreach (DataRow dr in tbl.Rows) // there should be only one row but one never knows
                        status.LoadFromDataRow(dr);

                }
                if (tbl.TableName.ToUpper().Trim() == typeof (Aktion).Name.ToUpper())
                {
                    foreach (DataRow dr in tbl.Rows)
                    {
                        var rec = new Aktion();
                        rec.LoadFromDataRow(dr);
                        status.Aktionen.Add(rec);
                    }
                }
                if (tbl.TableName.ToUpper().Trim() == typeof (InkassoAktDokument).Name.ToUpper())
                {
                    foreach (DataRow dr in tbl.Rows)
                    {
                        var rec = new InkassoAktDokument();
                        rec.LoadFromDataRow(dr);
                        status.Dokumente.Add(rec);
                    }
                }
            }
            TextWriter tw = new StreamWriter("c:/temp/InkassoAktStatusResponse.txt");
            tw.Write(status.ToXmlString());
            tw.Close();
            tw.Dispose();
            Console.WriteLine("CollectionInvoice Memo: " + status.Inkassomemo);
            Console.WriteLine("Intervention Memo: " + status.Interventionsmemo);

            foreach (Aktion aktion in status.Aktionen)
            {

                Console.WriteLine(aktion.AktionDatum.ToShortDateString() + " " + aktion.AktionDatum.ToShortTimeString() + " " + aktion.AktionBeschreibung + " " + aktion.AktionMemo);
            }

        }

        private static InkassoAkt GetAkt(string xmlData)
        {
            var ds = new DataSet();
            ds.ReadXml(new StringReader(xmlData));
            var rec = new InkassoAkt();
            foreach (DataTable tbl in ds.Tables)
            {
                if (tbl.TableName.ToUpper().Trim() == typeof(InkassoAkt).Name.ToUpper().Trim())
                {
                    foreach (DataRow dr in tbl.Rows)
                    {
                        rec.LoadFromDataRow(dr);
                    }
                }
                else if (tbl.TableName.ToUpper().Trim() == typeof(InkassoAktDokument).Name.ToUpper().Trim())
                {

                    foreach (DataRow dr in tbl.Rows)
                    {
                        var rate = new InkassoAktDokument();
                        rate.LoadFromDataRow(dr);
                        rec.Dokumente.Add(rate);
                    }
                }
            }
            return rec;
        }
        private static void TestFinancialPmt()
        {
            double interest = 12 / 12;
            double nper = 4;
            double pv = 6122.67;

            double fv = 0; // future value of the load

            double pmt = 20;
            
            Console.WriteLine("[I: "+interest+"] [nper: "+nper+"] [pv: "+pv+"]");

            Console.WriteLine("PMT: " + HTBUtils.FormatCurrencyNumber(Microsoft.VisualBasic.Financial.Pmt(interest, nper, -pv)));
            Console.WriteLine("PMT: " + Microsoft.VisualBasic.Financial.Pmt(interest, nper, -pv));

            Console.WriteLine("PMT2: " + HTBUtils.FormatCurrencyNumber(pv*((interest*Math.Pow(1 + interest, nper))/(Math.Pow(1 + interest, nper) - 1))));

            //Console.WriteLine("NPER: " + Financial.NPer(interest, pmt, -pv));


            nper = Math.Log(1 - interest*pv/pmt)/Math.Log(1 + interest)*-1;

            Console.WriteLine("NPER2: " + HTBUtils.FormatCurrencyNumber(nper));
        }

        private static void TestFinanacial_NumberOfInstallments()
        {
            const double balance = 6122.67;
            const double installmentAmount = 1950;
            const double annualInterestRate = .12;
            const double interestPeriod = 12;

            double numberOfInstallments = Microsoft.VisualBasic.Financial.NPer(annualInterestRate/interestPeriod, installmentAmount, -balance);
            double totalAmountToPay = numberOfInstallments * installmentAmount;
            double lastInstallment = totalAmountToPay - (installmentAmount*(int) numberOfInstallments);
            double totalInterest = totalAmountToPay - balance;

            Console.WriteLine("Total Amount To Pay: {0}", HTBUtils.FormatCurrency(totalAmountToPay));
            Console.WriteLine("Number of installments: {0}", ((int)numberOfInstallments + 1));
            Console.WriteLine("Last Installment: {0}", HTBUtils.FormatCurrency(lastInstallment));
            Console.WriteLine("Total Interest:   {0}", HTBUtils.FormatCurrency(totalInterest));
        }


        private static void TestLogFileConverssion()
        {

            var directory = "C:\\temp\\ECP_LOGS";
            var toName = directory+ "\\Good\\Access.log";

            //tw.Write(text.Replace("\n", Environment.NewLine));

            string[] fileEntries = Directory.GetFiles(directory);
            foreach (string fileName in fileEntries)
            {
                if (fileName.IndexOf(".log") > 0)
                {
                    string text = HTBUtils.GetFileText(fileName);
                    if (string.IsNullOrEmpty(text))
                        return;
            
                    TextWriter tw = new StreamWriter(new FileStream(toName, FileMode.Append));
            
                    int dteIdxStart = text.IndexOf("[");
                    while (dteIdxStart >= 0)
                    {
                        int dteIdxEnd = text.IndexOf("]", dteIdxStart);
                        if (dteIdxEnd < 0)
                            break;

                        int ipIdxStart = dteIdxEnd + 2;


                        int ipIdxEnd = text.IndexOf("-", ipIdxStart);
                        if (ipIdxStart < 0)
                            break;

                        int nextLineIdx = text.IndexOf("[", dteIdxEnd);
                        if (nextLineIdx < 0)
                            break;

                        try
                        {
                            string dateText = text.Substring(dteIdxStart + 1, dteIdxEnd - dteIdxStart - 6);
                            string ipText = text.Substring(ipIdxStart, ipIdxEnd - ipIdxStart);
                            string restText = text.Substring(ipIdxEnd + 1, nextLineIdx - ipIdxEnd - 1);


                            string[] parts = dateText.Split();
                            string pattern = "yyyy-MM-dd";
                            DateTime dt;

                            DateTime.TryParseExact(parts[0], pattern, null, DateTimeStyles.None, out dt);
                            tw.Write(ipText);
                            tw.Write("- anonymus [");
                            //tw.Write(dt.ToShortDateString() + ":" + parts[1]);
                            tw.Write(dt.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture));
                            tw.Write(":" + parts[1]);
                            tw.Write(" +0000]");
                            tw.Write(restText);
                            if (!restText.EndsWith(Environment.NewLine))
                                tw.WriteLine();
                            dteIdxStart = text.IndexOf("[", dteIdxEnd);
                        }
                        catch (Exception)
                        {
                            int greatestReadIdx = dteIdxStart;
                            if (greatestReadIdx > dteIdxEnd)
                                greatestReadIdx = dteIdxEnd;
                            if (greatestReadIdx > ipIdxStart)
                                greatestReadIdx = ipIdxStart;
                            if (greatestReadIdx > ipIdxEnd)
                                greatestReadIdx = ipIdxEnd;
                            if (greatestReadIdx > nextLineIdx)
                                greatestReadIdx = nextLineIdx;
                            //tw.Write(text.Substring(greatestReadIdx));
                            dteIdxStart = greatestReadIdx + 1;
                            //break;
                        }
                    }
                    tw.Close();
                    tw.Dispose();
                }
                //new Process {StartInfo = {FileName = toName}}.Start();

                //var lines = File.ReadAllLines(toName).Where(arg => !string.IsNullOrWhiteSpace(arg));
                //File.WriteAllLines(fileName, lines);
            }
        }

        private static void TestErlagschein()
        {
           
        }

        private static void TestInterverntionAction()
        {
            try
            {
                int intAktEmailSentActionTypeId = HTBUtils.GetIntConfigValue("AktInt_EMail_Sent_Action_Type");
                int intAktEmailNotSentActionTypeId = HTBUtils.GetIntConfigValue("AktInt_EMail_NOT_Sent_Action_Type");
                tblControl control = HTBUtils.GetControlRecord();
                HTBUtils.AddInterventionAction(147193, intAktEmailSentActionTypeId, "test EMail OK",  control.DefaultSB);
                HTBUtils.AddInterventionAction(147193, intAktEmailNotSentActionTypeId, "test EMail NOT OK",  control.DefaultSB);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void TestLawyerMahnung()
        {
            //new LawyerMahnung().GenerateLawyerMahnungPdf(null, null);
            //var aktId = 22302;
            var aktId = 22301;
            var attachment = new HTBEmailAttachment(ReportFactory.GetZwischenbericht(aktId), "Zwischenbericht.pdf", "Application/pdf");
            new AktUtils(aktId).SendInkassoPackageToLawyer(attachment, true, "c:/temp/lawyerTest/");
        }

    }

    class AeCountRecord
    {
        public string caption;
        public int count;
        public List<string> priceList = new List<string>();
        public List<string> dateList = new List<string>();
        public AeCountRecord(string c, string p, string d)
        {
            caption = c;
            count = 1;
            priceList.Add(p);
            dateList.Add(d);
        }
        public void Increment(string price, string date)
        {
            count++;
            priceList.Add(price);
            dateList.Add(date);
        }
    }
}
