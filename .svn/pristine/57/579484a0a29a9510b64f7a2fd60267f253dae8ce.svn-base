using System;
using HTB.v2.intranetx.permissions;
using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.routeplanter.bingmaps
{
    public partial class BingRouteLoader : System.Web.UI.Page
    {
        private readonly PermissionsRoutePlanner _permissions = new PermissionsRoutePlanner();
        protected void Page_Load(object sender, EventArgs e)
        {
            _permissions.LoadPermissions(GlobalUtilArea.GetUserId(Session));
            int routeUser = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.USER]);
            string routeName = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.NAME]);
            RoutePlanerManager rpManager = FileSerializer<RoutePlanerManager>.DeSerialize(RoutePlanerManager.GetRouteFilePath(routeUser, routeName));
            if(rpManager != null)
            {
                Session[GlobalHtmlParams.SessionRoutePlannerManager] = rpManager;
                if(GlobalUtilArea.IsTablet())
                    Response.Redirect("tablet/BingRouteDisplayTablet.aspx");
                else
                    Response.Redirect("BingRouteDisplay.aspx");
            }
        }
    }
}