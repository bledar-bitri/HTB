using System;
using System.Web.Services;
using HTB.v2.intranetx.progress;

namespace HTB.v2.intranetx.WS
{
    
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WsProgress : WebService
    {

        [WebMethod]
        public string GetProgress(string taskId)
        {
            return DateTime.Now.ToString();
        }

        #region Progress Indicator

        [WebMethod()]
        public TaskStatus GetBingRoutePlannerTaskStatus()
        {
            string taskID = Context.User.Identity.Name + ":BingRoutePlanner";

            return Context.Cache[taskID] as TaskStatus;
        }

        [WebMethod()]
        public TaskStatus GetBingRoutePlannerForAktenTaskStatus()
        {
            string taskID = Context.User.Identity.Name + ":BingRoutePlanerForAkten";

            return Context.Cache[taskID] as TaskStatus;
        }

        [WebMethod()]
        public TaskStatus GetBingRoutePlannerAutomaticTaskStatus()
        {
            string taskID = Context.User.Identity.Name + ":BingRoutePlannerAutomatic";

            return Context.Cache[taskID] as TaskStatus;
        }

        [WebMethod()]
        public string GetBingRoutePlannerAutomaticTaskStatusTabletApp(string userId)
        {
            string taskID = userId + ":BRPA";
            var ts = (TaskStatus) Context.Cache[taskID];
            if(ts != null)
            {
                return ts.Progress + "  " + ts.ProgressText + "  " + ts.ProgressTextLong;
            }
            return "NOTHING";
        }

        [WebMethod()]
        public TaskStatus GetAktReassignmentTaskStatus()
        {
            string taskID = Context.User.Identity.Name + ":AktReassignment";

            return Context.Cache[taskID] as TaskStatus;
        }
        #endregion
    }
}
