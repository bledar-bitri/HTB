<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BingRoutePrintRouteDirections.aspx.cs" Inherits="HTB.v2.intranetx.routeplanner.BingRoutePrintRouteDirections" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="/v2/intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0&&mkt=de-DE"></script>
    <script type="text/javascript">
          var map = null;

          function GetMap() {
              // Initialize the map
              map = new Microsoft.Maps.Map(
                  document.getElementById("mapDiv"), { 
                      credentials: "<%= HTB.v2.intranetx.routeplanner.RoutePlanerManager.BingMapsKey %>", 
                      mapTypeId: Microsoft.Maps.MapTypeId.road 
                   });
              Microsoft.Maps.loadModule('Microsoft.Maps.Directions', { callback: directionsModuleLoaded });

          }

          function directionsModuleLoaded() {
              // Initialize the DirectionsManager
              var directionsManager = new Microsoft.Maps.Directions.DirectionsManager(map);

              // Create start and end waypoints

              <%= JsWaypoints %>
              // Set the id of the div to use to display the directions
              directionsManager.setRenderOptions({ itineraryContainer: document.getElementById('itineraryDiv') });

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
          
          function UpdatePage(results, context) {
              alert("called");
            StopProgress();
            GetMap();
            var label = document.getElementById("Results");
            label.innerHTML = results;
              
            var button1 = document.getElementById("btnSubmit");
            button1.disabled = false;
        }

        function printdiv(printpage)
        {
            var headstr = "<html><head> </head><body>";
            var footstr = "</body>";
            var newstr = document.all.item(printpage).innerHTML;
            var oldstr = document.body.innerHTML;
            document.body.innerHTML = headstr+newstr+footstr;
            window.print(); 
            document.body.innerHTML = oldstr;
            return false;
        }
    </script>
</head>
<body onload="GetMap();">
    <form id="form1" runat="server">
        <div id='itineraryDiv' style="position: relative; top: 5px; width: 600px;" />
        <div id='mapDiv' style="position: relative; width: 1px; visibility:hidden" />
    </form>
</body>
</html>
