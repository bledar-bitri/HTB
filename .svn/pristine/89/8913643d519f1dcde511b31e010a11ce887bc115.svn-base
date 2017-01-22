using System.ServiceProcess;

namespace HTBMailboxMonitor
{
    class Program
    {
        static void Main()
        {
#if (!DEBUG)
            var servicesToRun = new ServiceBase[] 
                                              { 
                                                  new HTBMailboxMonitorService() 
                                              };
            ServiceBase.Run(servicesToRun);
#else
            // Debug code: this allows the process to run as a non-service.
            // It will kick off the service start point, but never kill it.
            // Shut down the debugger to exit

            var service = new HTBMailboxMonitorService();
            service.Run();
            // Put a breakpoint on the following line to always catch
            // the service when it has finished its work
            //System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#endif
        }
    }
}
