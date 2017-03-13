using System;
using System.Reflection;
using System.Web.UI;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.routeplanter.bingmaps.tabletApp
{
    public partial class BingRouteDisplayTabletApp : Page
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected string JsWaypoints;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string routeName = GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.ROAD_NAME]);
                int routeUser = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.INKASSANT_ID]);
                bool reverseRoute = GlobalUtilArea.StringToBool(GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.REVERSE_ROUTE]));
            
                RoutePlanerManager rpManager = FileSerializer<RoutePlanerManager>.DeSerialize(RoutePlanerManager.GetRouteFilePath(routeUser, routeName));
                rpManager.IsReverse = reverseRoute;
                //ShowMap(rpManager, 0);
                Session[GlobalHtmlParams.SessionRoutePlannerManager] = rpManager;

//                var rpManager = (RoutePlanerManager) Session[GlobalHtmlParams.SessionRoutePlannerManager];
                if (rpManager.BadAddresses.Count > 0)
                {
                    Response.Redirect("BingRouteAddressFixTablet.aspx");
                }
                else
                {
                    ShowMap(rpManager, 0);
                    Session[GlobalHtmlParams.SessionCurrentPosition] = 0;
                }   
            }
        }

        private void ShowMap(RoutePlanerManager rpManager, int startIndex)
        {
            JsWaypoints = rpManager.IsOverview ? rpManager.GetJsOverwiewWaypoints(RoutePlanerManager.BingMapMaxWaypoints) : rpManager.GetJsWaypoints(startIndex, RoutePlanerManager.BingMapMaxWaypoints);
        }

        
        private void ShowDebug(RoutePlanerManager rpManager)
        {
            Response.Write("<BR>LOCATIONS<BR>===========================<br>");
            foreach (City city in rpManager.Cities)
            {
                Response.Write(city.Address.ID + "&nbsp;&nbsp;&nbsp;" + city.Address.Address + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;LAT:" + city.Location.Locations[0].Latitude + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;LGN:" + city.Location.Locations[0].Longitude + "<br>");
            }

            Response.Write("<BR><br><strong>BAD LOCATIONS<BR>===========================<br>");
            foreach (AddressWithID address in rpManager.BadAddresses)
            {
                Response.Write(address.ID + "&nbsp;&nbsp;&nbsp;" + address.Address + "<br>");
            }
            Response.Write("<BR><BR>===========================<br></strong>");
            
            Response.Write("<BR><br><strong>MULTIPLE LOCATIONS<BR>===========================<br>");
            foreach (AddressWithID address in rpManager.BadAddresses)
            {
                foreach (AddressLocation l in address.SuggestedAddresses)
                {
                    Response.Write(l.Address.Address + "<br>");
                }
            }
            Response.Write("<BR><BR>===========================<br></strong>");
            
            Response.Write("<BR>ROADS<BR>===========================<br>");
            foreach (Road road in rpManager.Roads)
            {
                Response.Write(road.From.Address.Address + " =====> " + road.To.Address.Address + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + road.Distance);
                Response.Write("<br>");
            }

            Response.Write("<BR><BR>BEST TOUR============================<BR><BR>");
            Response.Write(rpManager.GetDebugBestTour());
            Response.Write("<BR><BR>============================<BR><BR>");
        }
    }
}