<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BingRouteDisplayTablet.aspx.cs" Inherits="HTB.v2.intranetx.routeplanner.tablet.bing.BingRouteDisplayTablet" %>

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
    <script type='text/javascript' src='https://www.bing.com/api/maps/mapcontrol?branch=experimental&callback=loadMapScenario' async defer></script>
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
    <ctl:headerNoMenuTablet ID="header" runat="server" />
    <form id="form1" runat="server">
    <br/>
    <table width="100%">
        <tr>
            <td align="left" class="docText" colspan="2"><a href="BingRoutePlanerTablet.aspx">Neue Rute</a></td>
            <td align="center" class="docText" colspan="2"><asp:HyperLink href="javascript:MM_openBrWindow('BingRoutePrintRouteDirectionsTablet.aspx','Popup2','width=800,height=500,resizable=yes,status=yes,scrollbars=yes,toolbar=yes,menubar=yes')" runat="server" ID="lnkPrintRoute">Reiseroute</asp:HyperLink></td>
            <td align="right" class="docText" colspan="2"><a href="javascript:MM_openBrWindow('BingRoutePrintRouteAddressesTablet.aspx','Popup3','width=800,height=500,resizable=yes,status=yes,scrollbars=yes,toolbar=yes,menubar=yes')">Akten nach Route</a></td>
        </tr>
        <tr>
            <td class="docText">
                <asp:LinkButton runat="server" ID="btnPreviousRoute" CssClass="docText" Text="<< Letzte Abschnitt" OnClick="btnPreviousRoute_Click"/>
            </td>
            <td class="docText">
                <asp:LinkButton runat="server" ID="btnNextRoute" CssClass="docText" Text="Nechste Abschnitt >>" OnClick="btnNextRoute_Click"/>
            </td>
            <td class="docText">
                <asp:LinkButton runat="server" ID="btnOverview" CssClass="docText" Text="Route &Uuml;berblik" OnClick="btnOverview_Click"/>
            </td>
            <td class="docText">
                <asp:LinkButton runat="server" ID="btnDetail" CssClass="docText" Text="Route Detail" OnClick="btnDetail_Click"/>
            </td>
            <td class="docText">
                <asp:LinkButton runat="server" ID="btnReverseRoute" CssClass="docText" Text="Route umkehren" OnClick="btnReverseRoute_Click"/>
            </td>
            <td class="docText">
                <asp:LinkButton runat="server" ID="btnNormalRoute" CssClass="docText" Text="Normale Route" OnClick="btnNormalRoute_Click"/>
            </td>
        </tr>
    </table>
    <br/>
    <div id='mapDiv' style="position: absolute; width: 980px; height: 600px;" />

    <%--<div id='itineraryDiv' style="position: relative; top: 5px; left: 710px; width: 400px;" />--%>
    
    </form>
    <ctl:footer ID="ftr" runat="server" />
</body>
</html>
