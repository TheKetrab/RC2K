window.mapInterop = {
    initMap: function (mapElementId, waypoints, showWaypoints) {
        
        if (waypoints.length < 2)
            return;

        let origin = {
            lat: waypoints.reduce((acc, x) => acc + x.lat, 0) / waypoints.length,
            lng: waypoints.reduce((acc, x) => acc + x.lng, 0) / waypoints.length
        }
        // Initialize the map centered at the origin
        const map = L.map(mapElementId).setView([origin.lat, origin.lng], 13);

        // Set up the OpenStreetMap tile layer
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '© OpenStreetMap contributors'
        }).addTo(map);

        // markers
        L.marker(waypoints[0], { icon: this.startMarker }).addTo(map)
        debugger;
        if (showWaypoints) {
            waypoints.slice(1, -1).forEach(point => L.marker(point).addTo(map));
        }
        L.marker(waypoints[waypoints.length - 1], { icon: this.endMarker }).addTo(map)

        const coordinates = waypoints.map(p => `${p.lng},${p.lat}`).join(';');
        const osrmUrl = `https://routing.openstreetmap.de/routed-bike/route/v1/driving/${coordinates}?overview=full&geometries=geojson&generate_hints=false&alternatives=true`;

        // Fetch the route from OSRM
        fetch(osrmUrl)
            .then(response => response.json())
            .then(data => {
                debugger;
                if (data.routes && data.routes.length > 0) {
                    // Extract the coordinates from the GeoJSON route geometry
                    const routeCoordinates = data.routes[0].geometry.coordinates;

                    // Map coordinates to Leaflet lat-lng objects
                    const latLngs = routeCoordinates.map(coord => L.latLng(coord[1], coord[0]));

                    // Draw the route as a polyline
                    const polyline = L.polyline(latLngs, { color: 'blue', weight: 5 });
                    polyline.addTo(map);

                    // Fit the map view to the polyline
                    map.fitBounds(polyline.getBounds());
                } else {
                    console.error("No route found");
                }
            })
            .catch(error => console.error("Error fetching route from OSRM:", error));
    },

    startMarker: L.AwesomeMarkers.icon({
        icon: 'flag',
        markerColor: 'green',
        prefix: 'fa',
    }),

    endMarker: L.AwesomeMarkers.icon({
        icon: 'flag',
        markerColor: 'red',
        prefix: 'fa',
        iconColor: 'dark-gray'
    }),

};



window.calculateContainerHeight = (containerId) => {

    const container = document.getElementById(containerId);
    if (!container) return;

    const stageInfo = container.querySelector('.stage-info');
    const stageImg = container.querySelector('.stage-img');
    const stageDescription = container.querySelector('.stage-description');
    const stageTimes = container.querySelector('.stage-times');
    const stageMap = container.querySelector('.stage-map');

    if (stageInfo && stageImg && stageDescription && stageTimes && stageMap) {

        const maxHeight = 50 + Math.max(stageInfo.offsetHeight + stageTimes.offsetHeight,
            stageImg.offsetHeight + stageDescription.offsetHeight + stageMap.offsetHeight);

        container.style.height = maxHeight + 'px';
    }
};