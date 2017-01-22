using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using log4net.Config;

namespace HTBMailboxMonitor
{
    partial class HTBMailboxMonitorService : ServiceBase
    {
        private InstallmentMailMonitor _monitor;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static int _timeToSleep = 20;
        private bool _keepRunning = true;

        public HTBMailboxMonitorService()
        {
            InitializeComponent();
            XmlConfigurator.Configure(new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Log4Net.config")));
            try
            {
                _timeToSleep = Convert.ToInt32(ConfigurationManager.AppSettings["TimeToSleep"]);
            }
            catch
            {
                _timeToSleep = 500;
            }
        }

        protected override void OnStart(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Log.Info("Initializing Thread");
            var t = new Thread(Run);
            Log.Info("Starting Thread");
            t.Start();
            Log.Info("DONE FINISH");
        }

        protected override void OnStop()
        {
            _keepRunning = false;
        }

        public void Run()
        {
            while (_keepRunning)
            {
                try
                {

                    _monitor = new InstallmentMailMonitor();
                    Log.Info("Checking Mailbox");
                    _monitor.CheckMailbox();
                    Log.Info("Sleeping");
                    Thread.Sleep(_timeToSleep*1000);
                    //                _keepRunning = false;
                }
                catch(Exception e)
                {
                    Log.Error(e);
                    _keepRunning = false;
                }
            }
        }
    }
}
