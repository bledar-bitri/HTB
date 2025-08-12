using System;
using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.routeplanner
{
    public partial class BingRoutePrintRouteDirections : System.Web.UI.Page
    {
        protected string JsWaypoints;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var currentPosition = (int)Session[GlobalHtmlParams.SessionCurrentPosition];
                var rpManager = (RoutePlanerManager)Session[GlobalHtmlParams.SessionRoutePlannerManager];
                JsWaypoints = rpManager.GetJsWaypoints(currentPosition, RoutePlanerManager.BingMapMaxWaypoints);
            }
        }
    }
}