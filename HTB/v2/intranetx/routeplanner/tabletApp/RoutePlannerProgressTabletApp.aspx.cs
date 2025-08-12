using HTB.v2.intranetx.progress;
using HTB.v2.intranetx.util;
using HTBUtilities;
using System;
using System.Reflection;
using System.Web.UI;

namespace HTB.v2.intranetx.routeplanner.tabletApp
{
    public partial class RoutePlannerProgressTabletApp : Page
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
        
        protected void Page_Load(object sender, EventArgs e)
        {
            var _userId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.INKASSANT_ID]);

            var taskID = _userId + HtbConstants.RoutePlannerAutomaticTaskStatus;

            var ts = (TaskStatus)Context.Cache[taskID];
            if (ts != null)
                Response.Write("ID: "+_userId+"   taskID: "+taskID+"   "+DateTime.Now.ToLongTimeString() + "  " + ts.Progress + " PCT.     Text: " + ts.ProgressText + "    Long Text:" + ts.ProgressTextLong);
            else
            {
                Response.Write(DateTime.Now.ToShortTimeString() + " Nothing found");
            }
        }
    }
}