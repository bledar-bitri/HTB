using System;
using System.Reflection;
using Autofac;
using log4net;

namespace HTBServices
{
    public abstract class AbstractServiceFactory
    {
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        protected IContainer Kernel { get; set; }
        
        protected virtual void InitializeServices()
        {
            try
            {
                Kernel = ConfigureServices();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                Logger.Error(ex.StackTrace);
                throw;
            }
        }
        
        public virtual T GetService<T>()
        {
            using (var scope = Kernel.BeginLifetimeScope())
            {
                return scope.Resolve<T>();
            }
        
        }
        
        protected abstract IContainer ConfigureServices();

    }
}
