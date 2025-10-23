window.mapInterop = {
    initMap: async function (mapElementId, waypoints, showWaypoints, api, path) {
        
        if (waypoints.length < 2)
            throw new Error("Start and Finish required.");

        // Initialize the map centered at the origin
        let origin = {
            lat: waypoints.reduce((acc, x) => acc + x.lat, 0) / waypoints.length,
            lng: waypoints.reduce((acc, x) => acc + x.lng, 0) / waypoints.length
        }
        const map = L.map(mapElementId).setView([origin.lat, origin.lng], 13);

        // Set up the OpenStreetMap tile layer
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '© OpenStreetMap contributors'
        }).addTo(map);

        // Markers
        L.marker(waypoints[0], { icon: this.startMarker }).addTo(map)
        debugger;
        if (showWaypoints) {
            waypoints.slice(1, -1).forEach(point => L.marker(point).addTo(map));
        }
        L.marker(waypoints[waypoints.length - 1], { icon: this.endMarker }).addTo(map)

        // Path
        var routeCoordinates;
        var result;
        if (!path) {
            const coordinates = waypoints.map(p => `${p.lng},${p.lat}`).join(';');
            routeCoordinates = await this.getRoute(coordinates, api);
            result = routeCoordinates.map(x => x.join(',')).join(';');
        } else {
            routeCoordinates = path.split(';').map(x => x.split(',').map(y => parseFloat(y)));
            result = null;
        }

        const latLngs = routeCoordinates.map(coord => L.latLng(coord[1], coord[0]));
        const polyline = L.polyline(latLngs, { color: 'blue', weight: 5 });
        polyline.addTo(map);
        map.fitBounds(polyline.getBounds());

        return result; // returns newly calculated path or null it was previously cached
    },

    getRoute:
    /**
     * @param {string} coordinates - x1,y1;x2,y2
     * @returns {number[][]}
     */
    async function (coordinates, api) {

        var osrmUrl;
        const osrmApiQueryParams = 'overview=full&geometries=geojson&generate_hints=false&alternatives=true'; 
        if (api == 'bike') {
            osrmUrl = `https://routing.openstreetmap.de/routed-bike/route/v1/driving/${coordinates}?${osrmApiQueryParams}`;
        } else {
            osrmUrl = `https://routing.openstreetmap.de/routed-car/route/v1/driving/${coordinates}?${osrmApiQueryParams}`;
        }

        const osrmData = await fetch(osrmUrl)
            .then(response => response.json());

        if (!osrmData.routes || osrmData.routes.length == 0) {
            throw new Error("No route found.");
        }

        const routeCoordinates = osrmData.routes[0].geometry.coordinates;
        return routeCoordinates;
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