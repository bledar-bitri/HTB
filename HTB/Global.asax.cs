using System;
using System.Configuration;
using System.Reflection;
using HTB.Database;
using HTB.v2.intranetx.routeplanner;

namespace HTB
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                RoutePlanerManager.MaximumRoadsPerThread = Convert.ToInt32(ConfigurationManager.AppSettings["RoutePlannerManager_MaximumRoadsPerThread"]);
            }
            catch
            {
                RoutePlanerManager.MaximumRoadsPerThread = 3000;
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}