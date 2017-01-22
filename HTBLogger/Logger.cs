using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
namespace HTBLogger
{
    public class Logger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        // NOTE that using System.Reflection.MethodBase.GetCurrentMethod().DeclaringType
        // is equivalent to typeof(LoggingExample) but is more portable
        // i.e. you can copy the code directly into another class without
        // needing to edit the code.

        private static bool isInitialized = false;

        public static void Init()
        {
            XmlConfigurator.Configure(new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Log4Net.config")));
            Console.WriteLine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Log4Net.config"));
            //BasicConfigurator.Configure();
            isInitialized = true;
        }
        public static void LogInfo(string message)
        {
            if (isInitialized && log.IsInfoEnabled) log.Info(message);
            Console.WriteLine("INFO: "+message);
        }

        public static void LogDebug(string message)
        {
            if (isInitialized && log.IsDebugEnabled) log.Info(message);
            Console.WriteLine("DEBUG: "+message);
        }

        public static void LogError(string message)
        {
            if (isInitialized) log.Error(message);
            Console.WriteLine("ERROR: "+message);
        }
    }
}
