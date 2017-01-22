<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetLatAndLon.aspx.cs" Inherits="HTB.v2.intranetx.GetLatAndLon" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="http://maps.google.com/maps?file=api&amp;v=2&amp;key=ABQIAAAAgrj58PbXr2YriiRDqbnL1RSqrCjdkglBijPNIIYrqkVvD1R4QxRl47Yh2D_0C1l5KXQJGrbkSDvXFA" type="text/javascript"></script>
    <script type="text/javascript">
        var geocoder;
        function load() {
            if (GBrowserIsCompatible()) {    
                //var map = new GMap2(document.getElementById("map"));
                //map.addControl(new GSmallMapControl());
                //map.addControl(new GMapTypeControl());
                //var center = new GLatLng(48.89364, 2.33739);
                //map.setCenter(center, 15);
                geocoder = new GClientGeocoder();
                //var marker = new GMarker(center, { draggable: true });
                //map.addOverlay(marker);
                //document.getElementById("lat").innerHTML = center.lat().toFixed(5);
                //document.getElementById("lng").innerHTML = center.lng().toFixed(5);
                /*
                GEvent.addListener(marker, "dragend", function () {
                    var point = marker.getPoint();
                    map.panTo(point);
                    document.getElementById("lat").innerHTML = point.lat().toFixed(5);
                    document.getElementById("lng").innerHTML = point.lng().toFixed(5);

                });


                GEvent.addListener(map, "moveend", function () {
                    map.clearOverlays();
                    var center = map.getCenter();
                    var marker = new GMarker(center, { draggable: true });
                    map.addOverlay(marker);
                    document.getElementById("lat").innerHTML = center.lat().toFixed(5);
                    document.getElementById("lng").innerHTML = center.lng().toFixed(5);


                    GEvent.addListener(marker, "dragend", function () {
                        var point = marker.getPoint();
                        map.panTo(point);
                        document.getElementById("lat").innerHTML = point.lat().toFixed(5);
                        document.getElementById("lng").innerHTML = point.lng().toFixed(5);

                    });

                });
                */
            }
        }

        function showAddress(address) {
            //var map = new GMap2(document.getElementById("map"));
            //map.addControl(new GSmallMapControl());
            //map.addControl(new GMapTypeControl());
            document.form1.address2.value = address;
            if (geocoder) {
                geocoder.getLatLng(address,
                    function (point) {
                        if (!point) {
                            alert(address + " not found");
                        }
                        else {
                            document.getElementById("lat").innerHTML = point.lat().toFixed(5);
                            document.getElementById("lng").innerHTML = point.lng().toFixed(5);

                            document.form1.txtLat.value = "**Lat" + point.lat().toFixed(5) + "Lat**";
                            document.form1.txtLon.value = "**Lgn" + point.lng().toFixed(5) + "Lgn**";

                            /*
                            map.clearOverlays();
                            map.setCenter(point, 14);
                            var marker = new GMarker(point, { draggable: true });
                            map.addOverlay(marker);

                            GEvent.addListener(marker, "dragend", function () {
                            var pt = marker.getPoint();
                            map.panTo(pt);
                            document.getElementById("lat").innerHTML = pt.lat().toFixed(5);
                            document.getElementById("lng").innerHTML = pt.lng().toFixed(5);
                            });


                            GEvent.addListener(map, "moveend", function () {
                            map.clearOverlays();
                            var center = map.getCenter();
                            var marker = new GMarker(center, { draggable: true });
                            map.addOverlay(marker);
                            document.getElementById("lat").innerHTML = center.lat().toFixed(5);
                            document.getElementById("lng").innerHTML = center.lng().toFixed(5);

                            GEvent.addListener(marker, "dragend", function () {
                            var pt = marker.getPoint();
                            map.panTo(pt);
                            document.getElementById("lat").innerHTML = pt.lat().toFixed(5);
                            document.getElementById("lng").innerHTML = pt.lng().toFixed(5);
                            });

                            });
                            */
                        }
                    }
        );
            }
            }
    </script>
</head>
<body onload="load();showAddress(document.form1.address.value);" onunload="GUnload()">
    <form id="form1" runat="server">
    
    <p>
        <b>Find coordinates using the name and/or address of the place</b></p>
    <p>
        Submit the full location : number, street, city, country. For big cities and famous places, the country is optional. "Bastille Paris" or "Opera Sydney" will do.
        <br />
    </p>
    <p>
        <input type="text" size="60" name="address" value="<%=Request["address"] %>" />
        <input value="Search!" type="button" onclick="javascirpt:showAddress(address.value);"/>
        <input type="text" size="60" name="address2" />
    </p>
    <p align="left">
        <table bgcolor="#FFFFCC" width="300">
            <tr>
                <td>
                    <b>Latitude</b>
                </td>
                <td>
                    <b>Longitude</b>
                </td>
            </tr>
            <tr>
                <td id="lat">
                </td>
                <td id="lng">
                </td>
            </tr>
            <tr>
                <td>
                    <input type="text" id="txtLat" name="txtLat"/>
                    <input type="text" id="txtLon" name="txtLon"/>
                </td>
            </tr>
        </table>
    </p>
           
    </form>
</body>
 <script type="text/javascript">
     showAddress('Slavi-Souceck-Str 11, 5026, Salzburg, Austria');
     showAddress('Slavi-Souceck-Str 11, 5026, Salzburg, Austria');
    </script>
</html>
