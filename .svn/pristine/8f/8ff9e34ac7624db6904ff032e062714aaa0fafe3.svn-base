using System;
using System.Reflection;
using System.Web.UI;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.routeplanter.bingmaps
{
    public partial class BingRouteDisplay : Page
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected string JsWaypoints;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = GlobalUtilArea.GetUserId(Session);

                var rpManager = (RoutePlanerManager) Session[GlobalHtmlParams.SessionRoutePlannerManager];
                if (rpManager.BadAddresses.Count > 0)
                {
                    Response.Redirect("BingRouteAddressFix.aspx");
                }
                else
                {
                    ShowMap(rpManager, 0);
                    Session[GlobalHtmlParams.SessionCurrentPosition] = 0;
                    SetLinksVisible(rpManager);
                }   
            }
        }

        #region Event Handler
        protected void btnPreviousRoute_Click(object sender, EventArgs e)
        {
            var rpManager = (RoutePlanerManager)Session[GlobalHtmlParams.SessionRoutePlannerManager];
            var currentPosition = (int)Session[GlobalHtmlParams.SessionCurrentPosition];
            currentPosition -= RoutePlanerManager.BingMapMaxWaypoints;
            if (currentPosition < 0) currentPosition = 0;
            ShowMap(rpManager, currentPosition);

            Session[GlobalHtmlParams.SessionCurrentPosition] = currentPosition;
            SetLinksVisible(rpManager);
        }
        protected void btnNextRoute_Click(object sender, EventArgs e)
        {
            var rpManager = (RoutePlanerManager)Session[GlobalHtmlParams.SessionRoutePlannerManager];
            var currentPosition = (int)Session[GlobalHtmlParams.SessionCurrentPosition];
            currentPosition += RoutePlanerManager.BingMapMaxWaypoints;
            if (currentPosition > rpManager.GetAllWaypoints().Count)
                currentPosition = rpManager.GetAllWaypoints().Count - 2;
            ShowMap(rpManager, currentPosition);
            Session[GlobalHtmlParams.SessionCurrentPosition] = currentPosition;
            SetLinksVisible(rpManager);
        }
        protected void btnOverview_Click(object sender, EventArgs e)
        {
            var rpManager = (RoutePlanerManager)Session[GlobalHtmlParams.SessionRoutePlannerManager];
            rpManager.IsOverview = true;
            ShowMap(rpManager, 0);
            SetLinksVisible(rpManager);
            Session[GlobalHtmlParams.SessionRoutePlannerManager] = rpManager;
        }
        protected void btnDetail_Click(object sender, EventArgs e)
        {
            var rpManager = (RoutePlanerManager)Session[GlobalHtmlParams.SessionRoutePlannerManager];
            rpManager.IsOverview = false;
            ShowMap(rpManager, 0);
            Session[GlobalHtmlParams.SessionCurrentPosition] = 0;
            SetLinksVisible(rpManager);
            Session[GlobalHtmlParams.SessionRoutePlannerManager] = rpManager;
        }

        protected void btnReverseRoute_Click(object sender, EventArgs e)
        {
            var rpManager = (RoutePlanerManager)Session[GlobalHtmlParams.SessionRoutePlannerManager];
            rpManager.IsReverse = true;
            ShowMap(rpManager, 0);
            SetLinksVisible(rpManager);
            Session[GlobalHtmlParams.SessionRoutePlannerManager] = rpManager;
        }
        protected void btnNormalRoute_Click(object sender, EventArgs e)
        {
            var rpManager = (RoutePlanerManager)Session[GlobalHtmlParams.SessionRoutePlannerManager];
            rpManager.IsReverse = false;
            ShowMap(rpManager, 0);
            SetLinksVisible(rpManager);
            Session[GlobalHtmlParams.SessionRoutePlannerManager] = rpManager;
        }
        #endregion

        private void ShowMap(RoutePlanerManager rpManager, int startIndex)
        {
            Log.Error(string.Format("Waypoints: {0}", rpManager.GetJsWaypoints(0, 100)));
            JsWaypoints = rpManager.IsOverview ? rpManager.GetJsOverwiewWaypoints(RoutePlanerManager.BingMapMaxWaypoints) : rpManager.GetJsWaypoints(startIndex, RoutePlanerManager.BingMapMaxWaypoints);
        }
        
//        /*
        private void ShowMap2(RoutePlanerManager rpManager, int startIndex)
        {
//            var rpm = (RoutePlannerManagerAutomatic)rpManager;
//            rpm.Test();
            rpManager.Test();
            JsWaypoints = rpManager.IsOverview ? rpManager.GetJsOverwiewWaypoints(RoutePlanerManager.BingMapMaxWaypoints) : rpManager.GetJsWaypoints(startIndex, RoutePlanerManager.BingMapMaxWaypoints);
        }
//        */

        private void SetLinksVisible(RoutePlanerManager rpManager)
        {
            if (rpManager.IsOverview)
            {
                btnOverview.Visible = false;
                btnNextRoute.Visible = false;
                btnPreviousRoute.Visible = false;
                lnkPrintRoute.Visible = false;
                btnDetail.Visible = true;
            }
            else
            {
                var waypointCount = rpManager.GetAllWaypoints().Count;
                var currentPosition = (int) Session[GlobalHtmlParams.SessionCurrentPosition];
                btnPreviousRoute.Visible = currentPosition > 0;
                btnNextRoute.Visible = currentPosition + RoutePlanerManager.BingMapMaxWaypoints < waypointCount;
                btnOverview.Visible = RoutePlanerManager.BingMapMaxWaypoints < waypointCount;
                btnDetail.Visible = !btnOverview.Visible;
                lnkPrintRoute.Visible = true;
                btnDetail.Visible = false;
                
            }
            btnReverseRoute.Visible = !rpManager.IsReverse;
            btnNormalRoute.Visible = rpManager.IsReverse;
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