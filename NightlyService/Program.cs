using System.ServiceProcess;

namespace NightlyService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            #if (!DEBUG)
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
			    { 
				    new HTBNightlyService() 
			    };
                ServiceBase.Run(ServicesToRun);
            #else
                // Debug code: this allows the process to run as a non-service.
                // It will kick off the service start point, but never kill it.
                // Shut down the debugger to exit
                var service = new HTBNightlyService();
                service.Run();
                // Put a breakpoint on the following line to always catch
                // the service when it has finished its work
                //System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            #endif 
        }
    }
}
