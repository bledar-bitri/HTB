<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BingRouteDisplayTabletApp.aspx.cs" Inherits="HTB.v2.intranetx.routeplanter.bingmaps.tabletApp.BingRouteDisplayTabletApp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    
    <script type="text/javascript" src="http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0&&mkt=de-DE"></script>
    <script type="text/javascript">
          var map = null;

          function GetMap() {
              // Initialize the map
              map = new Microsoft.Maps.Map(
                  document.getElementById("mapDiv"), { 
                      credentials: "<%= HTB.v2.intranetx.routeplanter.RoutePlanerManager.BingMapsKey %>", 
                      mapTypeId: Microsoft.Maps.MapTypeId.road 
                   });
              Microsoft.Maps.loadModule('Microsoft.Maps.Directions', { callback: directionsModuleLoaded });

          }

          function directionsModuleLoaded() {
              // Initialize the DirectionsManager
              var directionsManager = new Microsoft.Maps.Directions.DirectionsManager(map);
                
              <%= JsWaypoints %>
              // Set the id of the div to use to display the directions
              //directionsManager.setRenderOptions({ itineraryContainer: document.getElementById('itineraryDiv') });

              // Set Request Options
              directionsManager.setRequestOptions({ avoidTraffic: true, routeOptimization: Microsoft.Maps.Directions.RouteOptimization.shortestTime });
              
              // Specify a handler for when an error occurs
              Microsoft.Maps.Events.addHandler(directionsManager, 'directionsError', displayError);

              // Calculate directions, which displays a route on the map
              directionsManager.calculateDirections();

          }

          function displayError(e) {
              alert(e.message);
          }
    </script>
</head>
<body onload="GetMap();">

    <form id="form1" runat="server">
        
    <div id='mapDiv' style="position: absolute; width: 760px; height: 500px;" />

    </form>
</body>
</html>
