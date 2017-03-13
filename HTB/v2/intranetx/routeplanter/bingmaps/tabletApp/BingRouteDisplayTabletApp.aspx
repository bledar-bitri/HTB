<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BingRouteDisplayTabletApp.aspx.cs" Inherits="HTB.v2.intranetx.routeplanter.bingmaps.tabletApp.BingRouteDisplayTabletApp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    
    <script type='text/javascript' src='https://www.bing.com/api/maps/mapcontrol?branch=release' async defer></script>
    <script type="text/javascript">
          var map = null;

          function GetMap() {
              // Initialize the map
              map = new Microsoft.Maps.Map('#mapDiv', { 
                      credentials: '<%= HTB.v2.intranetx.routeplanter.RoutePlanerManager.BingMapsKey %>', 
                      mapTypeId: Microsoft.Maps.MapTypeId.road 
                   });
              Microsoft.Maps.loadModule('Microsoft.Maps.Directions', function () {
                  //Create an instance of the directions manager.
                  directionsManager = new Microsoft.Maps.Directions.DirectionsManager(map);

                  
                  //Create waypoints to route between.
                  <%= JsWaypoints %>
                  //Specify the element in which the itinerary will be rendered.
                  directionsManager.setRenderOptions({ itineraryContainer: '#directionsItinerary' });

                  //Calculate directions.
                  directionsManager.calculateDirections();
              });

          }

          function displayError(e) {
              alert(e.message);
          }
    </script>
</head>
<body onload="GetMap();">

    <form id="form1" runat="server">
        
    <div id='mapDiv' style="position: absolute; width: 760px; height: 500px;" ></div>

    </form>
</body>
</html>
