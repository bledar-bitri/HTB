<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeafletRouteDisplayTabletApp.aspx.cs" Inherits="HTB.v2.intranetx.routeplanner.tabletApp.LeafletRouteDisplayTabletApp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head runat="server">
  <meta charset="utf-8" />
  <title>Leaflet + OpenRouteService with Red Numbered Waypoints</title>
  <meta name="viewport" content="width=device-width, initial-scale=1.0">

  <!-- Leaflet CSS and JS -->
  <link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.4/dist/leaflet.css" />
  <script src="https://unpkg.com/leaflet@1.9.4/dist/leaflet.js"></script>

  <style>
    #map {
      height: 600px;
    }
    #directions {
      max-height: 200px;
      overflow-y: auto;
      margin-top: 10px;
      font-family: sans-serif;
    }

    /* Custom marker style */
    .leaflet-marker-icon-number {
      background-color: #d62828; /* RED */
      color: white;
      border-radius: 50%;
      text-align: center;
      font-size: 18px;
      font-weight: bold;
      width: 36px;
      height: 36px;
      line-height: 36px;
      border: 2px solid white;
      box-shadow: 0 0 4px rgba(0,0,0,0.5);
    }
  </style>
</head>
<body>

<h2>Leaflet + OpenRouteService with Numbered Red Markers</h2>
<div id="map"></div>
<div id="directions"></div>

<script>
    const apiKey = "<%= HTB.v2.intranetx.routeplanner.RoutePlanerManager.OpenRouteServiceKey %>"; // Replace with your real API key

    // Waypoints with descriptions
    const waypoints = [

        <%= JsCoordinates %>
    ];

    // Initialize map
    const map = L.map('map').setView([<%= FirstLocation%>], 13);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '© OpenStreetMap contributors'
    }).addTo(map);

    // Add numbered red markers
    waypoints.forEach((wp, index) => {
        const icon = L.divIcon({
            className: 'leaflet-marker-icon-number',
            html: index + 1,
            iconSize: [20, 20],
            iconAnchor: [10, 10],

        });
        L.marker([wp.coords[1], wp.coords[0]], { icon: icon })
            .addTo(map)
            .bindPopup(`<strong>Stop ${index + 1}</strong>: ${wp.name}`);
    });

    // Extract coordinates in [lon, lat] order
    const coords = waypoints.map(wp => wp.coords);

    // Fetch route
    fetch("https://api.openrouteservice.org/v2/directions/driving-car/geojson", {
        method: "POST",
        headers: {
            "Authorization": apiKey,
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ coordinates: coords })
    })
        .then(response => response.json())
        .then(data => {
            // Draw route
            const route = L.geoJSON(data, {
                style: { color: "blue", weight: 5 }
            }).addTo(map);

            map.fitBounds(route.getBounds());

            // Show directions
            const steps = data.features[0].properties.segments[0].steps;
            const dirDiv = document.getElementById("directions");
            dirDiv.innerHTML = "<h3>Directions:</h3><ol>" +
                steps.map(s => `<li>${s.instruction} (${Math.round(s.distance)} m)</li>`).join("") +
                "</ol>";
        })
        .catch(err => {
            console.error("Routing error:", err);
            alert("Could not retrieve route. Check your API key or internet connection.");
        });
</script>

</body>
</html>
