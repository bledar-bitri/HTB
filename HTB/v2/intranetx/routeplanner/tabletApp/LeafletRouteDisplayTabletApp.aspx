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
    .emoji-marker-box {
        background-color: white;
        border: 1px solid #2a9d8f;
        border-radius: 4px;
        padding: 1px 3px;
        text-align: center;
        box-shadow: 0 0 1px rgba(0, 0, 0, 0.2);
        font-family: sans-serif;
    }

    .emoji {
        font-size: 10px; /* was 16px */
        line-height: 1;
    }

    .label {
        font-size: 8px; /* was 10px */
        font-weight: bold;
        color: #1c1c1c;
        margin-top: 1px;
    }

  </style>
</head>
<body>

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

    function getCustomIcon(type, index, name) {

        if (type === "start") {
            return L.divIcon({
                className: 'emoji-marker-dynamic',
                html: `
                        <div class="emoji-marker-box">
                            <div class="emoji">🟢</div>
                            <div class="label">Start</div>
                        </div>
                    `,
                iconSize: [40, 40],   // was 70,70
                iconAnchor: [25, 25]

            });
        }
        else if (type === "end") {
            return L.divIcon({
                className: 'emoji-marker-dynamic',
                html: `
                        <div class="emoji-marker-box">
                            <div class="emoji">🏁</div>
                            <div class="label">${index}</div>
                        </div>
                    `,
                iconSize: [40, 40],   // was 70,70
                iconAnchor: [25, 25]
            });

        }
        else {
            return L.divIcon({
                className: 'emoji-marker-dynamic',
                html: `
                        <div class="emoji-marker-box">
                            <div class="emoji">💰</div>
                            <div class="label">${index}</div>
                        </div>
                    `,
                iconSize: [40, 40],   // was 70,70
                iconAnchor: [25, 25]
            });
        }
    }

    for (var i = 0; i < waypoints.length; i++) {
        const wp = waypoints[i];
        const iconType = (i === 0) ? "start" : (i === waypoints.length - 1) ? "end" : "waypoint";
        const icon = getCustomIcon(iconType, i);
        // Add marker for each waypoint
        L.marker([wp.coords[1], wp.coords[0]], { icon: icon })
            .addTo(map)
            .bindPopup(`<strong>${wp.name}</strong>`);
    }

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
