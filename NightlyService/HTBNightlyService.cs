using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.Reflection;
using HTB.Database.HTB.StoredProcs;
using HTBDailyKosten;
using System.Collections;
using System.Threading;
using System.IO;
using HTBUtilities;
using System.Globalization;
using log4net.Config;
using HTB.Database;
using HTBReports;
using HTB.Database.Views;
using HTBServices;
using HTBServices.Mail;

namespace NightlyService
{
    public partial class HTBNightlyService : ServiceBase
    {
        private static FileStream _ostrm;
        private static StreamWriter _writer;
        private static int _timeToSleep = 20;
        private bool _keepRunning = true;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private tblControl _control;
        
        public HTBNightlyService()
        {
            var sb = new StringBuilder();
            try
            {
                sb.Append($"setting current directory to {AppDomain.CurrentDomain.BaseDirectory}\n\r");
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                sb.Append("current directory SET\n\r");
                sb.Append($"Log4Net Path: {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4Net.config")} \n\r");
                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4Net.config")))
                    sb.Append("Log4Net File Exists\n\r");
                else
                    sb.Append("Log4Net File Does NOT Exist\n\r");

            }
            catch (Exception ex)
            {
                sb.Append($"Could not set current directory: {ex.Message}\n\r");
            }
            sb.Append("Initializing Component\n\r");
            InitializeComponent();
            sb.Append("Configuring XmlConfigurator\n\r");
            XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4Net.config")));
            try
            {
                sb.Append("Setting time to sleep\n\r");
                _timeToSleep = Convert.ToInt32(ConfigurationManager.AppSettings["TimeToSleep"]);
            }
            catch
            {
                sb.Append("Error While Setting Time To sleep (Defauld: 500)\n\r");
                _timeToSleep = 500;
            }
            HTBUtils.SaveTextFile("c:/temp/HTBNightlyService_Start.txt", sb.ToString());
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                RedirectConsoleOutput();
                Log.Info("Initializing Thread");
                var t = new Thread(Run);
                Log.Info("Starting Thread");
                t.Start();
                Log.Info("DONE FINISH");
            }
            catch (Exception e)
            {
                Log.Error(e.Message + "\n");
                Log.Error(e.StackTrace + "\n");
                Log.Error(e.Source + "\n");
            }
        }

        protected override void OnStop()
        {
            CloseConsoleOutput();
            _keepRunning = false;
        }

        public void Run()
        {
            Log.Info("Started The Run");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Log.Info("In Run: --------> CULTURE SET");
            _keepRunning = true;
            _control = HTBUtils.GetControlRecord();
            while (_keepRunning)
            {
                try
                {
                    Log.Info("Calling DoNextStep");
                    DoNextStep();
                    Log.Info("Calling DoInterest");
                    DoInterest();
                    Log.Info("Calling DoWorkflow");
                    DoWorkflow();
                    Log.Info("Calling DoOverdue");
                    DoOverdue();
                    Log.Info("Calling DoMahnung");
                    DoMahnung();
                    // AuftragReceipt gets sent from another process
//                    Log.Info("Calling DoAuftragReceipt");
//                    DoAuftragReceipt();
                    Log.Info("Calling DoAGExtensionRequest");
                    DoAGExtensionRequest();
//                    Log.Info("Calling DoMissingBeleg");
//                    DoMissingBeleg();
                    Log.Info("Calling Thread.Sleep ("+((long)_timeToSleep * 1000)+")");
                    Thread.Sleep(_timeToSleep * 1000);
                }
                catch (Exception e)
                {
                    Log.Fatal("Message: " + e.Message);
                    Log.Error("Message: " + e.Message);
                    Log.Error("Stacktrace: " + e.StackTrace);
                    Log.Error("Source: " + e.Source);
                    _keepRunning = false;
                }
            }
        }

        private void DoInterest()
        {
            var invMgr = new HTBInvoiceManager.InvoiceManager();
            invMgr.GenerateInterest();
        }

        private void DoWorkflow()
        {
            new KostenCalculator().CalculateKosten();
        }

        private void DoOverdue()
        {
            new InstallmentManager().GenerateOverdueInvoices();
        }

        private void DoMahnung()
        {
            new MahnungManager().GenerateMahnungen(_control.AutoUserId);
        }

        
        private static void DoAGExtensionRequest()
        {
            tblControl control = HTBUtils.GetControlRecord();
            string[] wtimeString = control.KlientNotificationTime.Split(':');
            int wrunHour = Convert.ToInt16(wtimeString[0]);
            int wrunMin = Convert.ToInt16(wtimeString[1]);
            var wrunTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, wrunHour, wrunMin, 0);
            DateTime now = DateTime.Now;
            Log.Info("Extension Request [Run Time=" + wrunTime.ToShortDateString() + " " + wrunTime.ToShortTimeString() + "]");
            Log.Info("Extension Request [NOW =" + now.ToShortDateString() + " " + now.ToShortTimeString() + "]");
            TimeSpan wrunSpan = now.Subtract(wrunTime);
            if (wrunSpan.TotalMinutes >= 1)
            {
                const string extCriteria = "AktIntExtIsRequestSent = 0 and AktIntExtApprovedDate = '01.01.1900' AND AktIntExtDeniedDate = '01.01.1900'";
                var agQuery = "SELECT AuftraggeberId as[IntValue],  AKTIntAGSB AS [StringValue] FROM qryAktenIntExtension WHERE " + extCriteria + " GROUP BY AuftraggeberId, AKTIntAGSB";

                Log.Info("Checking Extension Requests [" + agQuery + "]");

                ArrayList agList = HTBUtils.GetSqlRecords(agQuery, typeof(SingleValue));

                Log.Info("[agList.Count = " + agList.Count + "]");

                var email = ServiceFactory.Instance.GetService<IHTBEmail>();
                var set = new RecordSet();

                foreach (SingleValue agSB in agList)
                {
                    Log.Info("[agSB = " + agSB.StringValue.Trim() + "]");
                    if (agSB.StringValue.Trim() != string.Empty)
                    {
                        Log.Info("Checking communication log table [AG=" + agSB + "] [SB=" + agSB.StringValue + "]");
                        // make sure we have not already sent a request to this auftraggeber
                        agQuery = "SELECT * FROM tblCommunicationLog " +
                                  " WHERE ComLogAuftraggeberID = " + agSB.IntValue +
                                  " AND ComLogAuftraggeberSB = '" + agSB.StringValue + "'" +
                                  " AND ComLogType = " + tblCommunicationLog.COMMUNICATION_TYPE_EXTENSION_REQUEST +
                                  " AND ComLogDate BETWEEN '" + now.ToShortDateString() + " 00:00.000' AND '" + now.ToShortDateString() + " 23:59.999'";

                        var receiptLog = (tblCommunicationLog)HTBUtils.GetSqlSingleRecord(agQuery, typeof(tblCommunicationLog));

                        if (receiptLog == null)
                        {
                            Log.Info("Getting Extension Request [AG=" + agSB + "] [SB=" + agSB.StringValue + "]");
                            var ext = (qryAktenIntExtension)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenIntExtension WHERE AuftraggeberID = " + agSB.IntValue + " AND AKTIntAGSB = '" + agSB.StringValue + "' AND " + extCriteria, typeof(qryAktenIntExtension));
                            if (ext != null)
                            {
                                String body = GetAGExtensionRequestBody(ext);
                                if (body != null)
                                {
                                    var toList =
                                        HTBUtils.GetValidEmailAddressesFromStrings(new[]
                                            {
                                                HTBUtils.GetConfigValue("AG_Extenssion_CC_EMail_Addr"),
                                                ext.AKTIntKSVEMail
                                            });
                                    
                                    Log.Info("EMailing Extension Request to  [" + HTBUtils.GetConfigValue("Default_EMail_Addr") + "]");

                                    email.SendGenericEmail(toList.ToArray(), "Verlängerungsanfrage", body);

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
            if (ext != null)
            {
                return HTBUtils.GetFileText(HTBUtils.GetConfigValue("AG_Extension_Req_Text")).Replace("[name]", ext.AKTIntAGSB)
                            .Replace("[AG_EXTENSION_HREF]", HTBUtils.GetConfigValue("URL_Extension_Req") + HTBUtils.EncodeTo64("AG=" + ext.AuftraggeberID + "&AGSB=" + ext.AKTIntAGSB));
            }
            return null;
        }
        
        private static void DoMissingBeleg()
        {
            const string sqlWhere = "WHERE (KbMissReceivedDate IS NULL OR KbMissReceivedDate = '01.01.1900') ";
            var missingBelegUserList = HTBUtils.GetSqlRecords("SELECT distinct (KbMissUser) IntValue FROM tblKassaBlockMissingNr " + sqlWhere, typeof(SingleValue));
            foreach (SingleValue missingUserIdRec in missingBelegUserList)
            {
                var missingRec = (tblKassaBlockMissingNr)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblKassaBlockMissingNr " + sqlWhere + " AND KbMissUser = " + missingUserIdRec.IntValue + " ORDER BY KbMissDate DESC", typeof(tblKassaBlockMissingNr));
                if ((DateTime.Now.Subtract(missingRec.KbMissDate)).Days >= 7)
                {
                    var user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserID = " + missingRec.KbMissUser, typeof(tblUser));
                    user.UserStatus = 0;
                    RecordSet.Update(user);
                    ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail(new [] { HTBUtils.GetConfigValue("Default_EMail_Addr"), "b.bitri@ecp.or.at" }, "Login gesperrt für Benutzer: " + user.UserVorname + " " + user.UserNachname, "Belege fehlen");
                    #region Notify when last beleg in block gets used

                    #endregion
                }
            }
        }

        private void DoNextStep()
        {
            var mgr = new NextStepManager();
            mgr.GenerateIntNextStep();
            mgr.GenerateMeldeNextStep();
        }

        private void RedirectConsoleOutput() {
            try
            {
                _ostrm = new FileStream(ConfigurationManager.AppSettings["ConsoleRedirect"], FileMode.OpenOrCreate, FileAccess.Write);
                _writer = new StreamWriter (_ostrm);
                Console.SetOut(_writer);
                Console.WriteLine("HTB Nightly OUTPUT\n");
            }
            catch (Exception)
            {
                //Console.WriteLine (e.Message);
                _writer = null;
            }
        }

        private void CloseConsoleOutput()
        {
            if (_writer != null)
            {
                _writer.Close();
                _ostrm.Close();
                _writer.Dispose();
                _ostrm.Dispose();

            }
        }
    }
}
