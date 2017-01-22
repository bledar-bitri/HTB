<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BingRouteDisplay.aspx.cs" Inherits="HTB.v2.intranetx.routeplanter.bingmaps.BingRouteDisplay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="/v2/intranet/styles/inksys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            margin-left: 5px;
            margin-top: 5px;
            margin-right: 5px;
            margin-bottom: 5px;
            background-image: url(../images/osxback.gif);
        }
    </style>
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

              // Create start and end waypoints

              //              directionsManager.addWaypoint(new Microsoft.Maps.Directions.Waypoint({ address: "Slavi-Souceck-Str. 11, 5026, Salzburg, Austria" }));
              //              directionsManager.addWaypoint(new Microsoft.Maps.Directions.Waypoint({ address: "Schwartzparkstrasse 15, 5020, Salzburg, Austria" }));
              //              directionsManager.addWaypoint(new Microsoft.Maps.Directions.Waypoint({ address: "Rennbahnstrasse 4, 5020, Salzburg, Austria" }));
              
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
    </script>
</head>
<body onload="GetMap();">
    <ctl:headerNoMenu ID="header" runat="server" />
    <form id="form1" runat="server">
    <table>
        <tr>
            <td>
                <asp:HyperLink href="javascript:MM_openBrWindow('BingRoutePrintRouteDirections.aspx','Popup2','width=800,height=500,resizable=yes,status=yes,scrollbars=yes,toolbar=yes,menubar=yes')" runat="server" ID="lnkPrintRoute">Diesen Abschnitt drucken</asp:HyperLink>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <a href="javascript:MM_openBrWindow('BingRoutePrintRouteAddresses.aspx','Popup2','width=800,height=500,resizable=yes,status=yes,scrollbars=yes,toolbar=yes,menubar=yes')">Reiseroute drucken</a>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <a href="javascript:MM_openBrWindow('/v2/intranetx/aktenint/PrintAuftrag.aspx?<%= HTB.v2.intranetx.util.GlobalHtmlParams.SOURCE %>=<%= HTB.v2.intranetx.util.GlobalHtmlParams.RoutePlanner %>', 'Popup2','width=800,height=500,resizable=yes,status=yes,scrollbars=yes,toolbar=yes,menubar=yes')">Aktliste drucken</a>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <a href="javascript:MM_openBrWindow('/v2/intranetx/aktenint/PrintAuftrag.aspx?RV=Y&<%=HTB.v2.intranetx.util.GlobalHtmlParams.SOURCE %>=<%=HTB.v2.intranetx.util.GlobalHtmlParams.RoutePlanner %>', 'Popup2','width=800,height=500,resizable=yes,status=yes,scrollbars=yes,toolbar=yes,menubar=yes')">Aktenliste mit RV drucken</a>
                <br/>&nbsp;<br/>
                <asp:LinkButton runat="server" ID="btnPreviousRoute" Text="<< Letzte Abschnitt" OnClick="btnPreviousRoute_Click"/>&nbsp;&nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="btnNextRoute" Text="Nechste Abschnitt >>" OnClick="btnNextRoute_Click"/>&nbsp;&nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="btnOverview" Text="Route &Uuml;berblik" OnClick="btnOverview_Click"/>
                <asp:LinkButton runat="server" ID="btnDetail" Text="Route Detail" OnClick="btnDetail_Click"/>&nbsp;&nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="btnReverseRoute" Text="Route umkehren" OnClick="btnReverseRoute_Click"/>
                <asp:LinkButton runat="server" ID="btnNormalRoute" Text="Normale Route" OnClick="btnNormalRoute_Click"/>
                <br/>&nbsp;
            </td>
        </tr>
    </table>
    
    <div id='mapDiv' style="position: absolute; width: 1200px; height: 800px;" />

    <div id='itineraryDiv' style="position: relative; top: 5px; left: 1210px; width: 600px;" />
    
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
