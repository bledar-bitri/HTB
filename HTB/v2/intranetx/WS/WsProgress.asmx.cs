using System;
using System.Web.Services;
using FileHelpers;
using HTB.v2.intranetx.progress;
using HTBUtilities;

namespace HTB.v2.intranetx.WS
{
    
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WsProgress : WebService
    {
        private const string Delimiter = ";";

        [WebMethod]
        public string GetProgress(string taskId)
        {
            return DateTime.Now.ToString();
        }

        #region Progress Indicator

        [WebMethod()]
        public TaskStatus GetBingRoutePlannerTaskStatus()
        {
            string taskId = Context.User.Identity.Name + ":BingRoutePlanner";

            return Context.Cache[taskId] as TaskStatus;
        }

        [WebMethod()]
        public TaskStatus GetBingRoutePlannerForAktenTaskStatus()
        {
            string taskId = Context.User.Identity.Name + ":BingRoutePlanerForAkten";

            return Context.Cache[taskId] as TaskStatus;
        }

        [WebMethod()]
        public TaskStatus GetBingRoutePlannerAutomaticTaskStatus()
        {
            string taskId = Context.User.Identity.Name + ":BingRoutePlannerAutomatic";

            return Context.Cache[taskId] as TaskStatus;
        }

        [WebMethod()]
        public string GetRoutePlanerTaskStatus(string userId)
        {
            var taskId = userId + HtbConstants.RoutePlannerAutomaticTaskStatus;
            var ts = (TaskStatus) Context.Cache[taskId];
            if(ts != null)
            {
                return ts.Progress + Delimiter + ts.ProgressText + Delimiter + ts.ProgressTextLong;
            }
            return "NOTHING";
        }
        [WebMethod()]
        public string GetAddressFixAndRecalculationTaskStatus(string userId)
        {
            var taskId = userId + HtbConstants.AddressFixAndRecalculationAutomaticTaskStatus;
            var ts = (TaskStatus) Context.Cache[taskId];
            if(ts != null)
            {
                return ts.Progress + Delimiter + ts.ProgressText + Delimiter + ts.ProgressTextLong;
            }
            return "NOTHING";
        }

        [WebMethod()]
        public TaskStatus GetAktReassignmentTaskStatus()
        {
            string taskId = Context.User.Identity.Name + ":AktReassignment";

            return Context.Cache[taskId] as TaskStatus;
        }
        #endregion
    }
}
