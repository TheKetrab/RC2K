
window.mapInterop = {
    mapInstance: null,
    hoverMarker: null,
    initMap: async function (mapElementId, waypoints, showWaypoints, api, path) {
        
        if (waypoints.length < 2)
            throw new Error("Start and Finish required.");

        // Initialize the map centered at the origin
        let origin = {
            lat: waypoints.reduce((acc, x) => acc + x.lat, 0) / waypoints.length,
            lng: waypoints.reduce((acc, x) => acc + x.lng, 0) / waypoints.length
        }

        if (this.mapInstance !== undefined && this.mapInstance !== null) {
            this.mapInstance.remove();
        }
        this.hoverMarker = null; // stale reference would target the removed map

        const map = L.map(mapElementId).setView([origin.lat, origin.lng], 13);
        this.mapInstance = map;

        // Set up the OpenStreetMap tile layer
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '© OpenStreetMap contributors'
        }).addTo(map);
        map.on('zoomend', () => this.setMarkersSize());
        map.on('layeradd', (e) => this.appendScaleToTransform(e));

        // Markers
        L.marker(waypoints[0], { icon: this.startMarker }).addTo(map)
        if (showWaypoints) {
            waypoints.slice(1, -1).forEach(point => L.marker(point).addTo(map));
        }
        L.marker(waypoints[waypoints.length - 1], { icon: this.endMarker }).addTo(map)

        // Path
        let routeCoordinates;
        let result;
        if (!path) {
            const coordinates = waypoints.map(p => `${p.lng},${p.lat}`).join(';');
            if (coordinates.indexOf("-1,-1") > 0) {
                // multi-road
                result = "";
                const parts = coordinates.split(';-1,-1;');
                for (let i = 0; i < parts.length; i++) {

                    if (i % 2 == 0) { // waypoints
                        const coords = await this.getRoute(parts[i], api);
                        if (i != 0) {
                            result += ";";
                        }
                        result += coords.map(x => x.join(',')).join(';');
                    }
                    else { // explicit path
                        result += ";" + parts[i];
                    }
                }
                routeCoordinates = result.split(';').map(x => x.split(',').map(y => Number.parseFloat(y)));
            }
            else {
                routeCoordinates = await this.getRoute(coordinates, api);
                result = routeCoordinates.map(x => x.join(',')).join(';');
            }
        } else {
            routeCoordinates = path.split(';').map(x => x.split(',').map(y => Number.parseFloat(y)));
            result = null;
        }

        const latLngs = routeCoordinates.map(coord => L.latLng(coord[1], coord[0]));
        const polyline = L.polyline(latLngs, { color: 'blue', weight: 5 });
        polyline.addTo(map);
        map.fitBounds(polyline.getBounds());

        this.setMarkersSize();

        return result; // returns newly calculated path or null it was previously cached
    },

    initMultiMap:
    /**
     * @param {string} mapElementId - HTML id
     * @param {number[][]} allWaypoints - waypoints of all paths to calculate origin
     * @param {number[][][]} startsEnds - starts and ends markers
     * @param {string[]} path - list of already computed paths
     */
    async function (mapElementId, waypoints, startsEnds, paths) {

        if (waypoints.length < 2)
            throw new Error("Start and Finish required.");

        // Initialize the map centered at the origin
        let origin = {
            lat: waypoints.reduce((acc, x) => acc + x.lat, 0) / waypoints.length,
            lng: waypoints.reduce((acc, x) => acc + x.lng, 0) / waypoints.length
        }

        if (this.mapInstance !== undefined && this.mapInstance !== null) {
            this.mapInstance.remove();
        }
        this.hoverMarker = null; // stale reference would target the removed map

        const map = L.map(mapElementId).setView([origin.lat, origin.lng], 13);
        this.mapInstance = map;

        // Set up the OpenStreetMap tile layer
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '© OpenStreetMap contributors'
        }).addTo(map);
        map.on('zoomend', () => this.setMarkersSize());
        map.on('layeradd', (e) => this.appendScaleToTransform(e) );

        // Markers
        for (const se of startsEnds) {
            L.marker(se[0], { icon: this.startMarkerMini }).addTo(map)
            L.marker(se[1], { icon: this.endMarkerMini }).addTo(map)
        }

        // Path
        const polylines = [];
        for (const path of paths) {
            const routeCoordinates = path.split(';').map(x => x.split(',').map(y => Number.parseFloat(y)));
            const latLngs = routeCoordinates.map(coord => L.latLng(coord[1], coord[0]));
            const polyline = L.polyline(latLngs, { color: 'blue', weight: 5 });
            polyline.addTo(map);
            polylines.push(polyline);
        }

        const group = new L.featureGroup(polylines).addTo(map);
        map.fitBounds(group.getBounds());

        this.setMarkersSize();
    },

    showHoverMarker: function (lat, lng) {

        if (!this.mapInstance) return;

        if (!this.hoverMarker) {
            this.hoverMarker = L.circleMarker([lat, lng], {
                radius: 8,
                color: '#e53935',
                weight: 2,
                fillColor: '#ffffff',
                fillOpacity: 1,
                interactive: false
            }).addTo(this.mapInstance);
        } else {
            this.hoverMarker.setLatLng([lat, lng]);
        }
    },

    hideHoverMarker: function () {

        if (this.hoverMarker && this.mapInstance) {
            this.mapInstance.removeLayer(this.hoverMarker);
        }
        this.hoverMarker = null;
    },

    setMarkersSize: function () {

        if (!this.mapInstance) return;

        const currentZoom = this.mapInstance.getZoom();
        const defaultZoomThreshold = 10;
        const microZoomThreshold = 7;

        // Grab the actual HTML element of the map
        const mapContainer = this.mapInstance.getContainer();

        if (currentZoom < microZoomThreshold) {
            mapContainer.classList.remove("map-zoomed-out");
            mapContainer.classList.add("map-zoomed-out-max");
        } else if (currentZoom < defaultZoomThreshold) {
            mapContainer.classList.remove("map-zoomed-out-max");
            mapContainer.classList.add("map-zoomed-out");
        } else {
            mapContainer.classList.remove("map-zoomed-out-max");
            mapContainer.classList.remove("map-zoomed-out");
        }
    },

    appendScaleToTransform: function (e) {

        if (e.layer instanceof L.Marker) {
            const marker = e.layer;
            const originalSetPos = marker._setPos;

            // hook
            marker._setPos = function (pos) {
                originalSetPos.call(this, pos);
                const computedStyle = window.getComputedStyle(this._icon);
                if (this._icon) {
                    this._icon.style.transform += ` scale(var(--my-custom-scale))`;
                    this._icon.style.transformOrigin = 'bottom center';
                }
            };
        }
    },

    getRoute:
    /**
     * @param {string} coordinates - x1,y1;x2,y2
     * @returns {number[][]}
     */
    async function (coordinates, api) {

        let osrmUrl;
        const osrmApiQueryParams = 'overview=full&geometries=geojson&generate_hints=false&alternatives=true'; 
        if (api == 'bike') {
            osrmUrl = `https://routing.openstreetmap.de/routed-bike/route/v1/bike/${coordinates}?${osrmApiQueryParams}`;
        } else if (api == 'foot') {
            osrmUrl = `https://routing.openstreetmap.de/routed-foot/route/v1/foot//${coordinates}?${osrmApiQueryParams}`;
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

    startMarkerMini: L.AwesomeMarkers.icon({
        icon: 'flag',
        markerColor: 'green',
        prefix: 'fa',
        iconSize: [35,46]
    }),

    endMarker: L.AwesomeMarkers.icon({
        icon: 'flag',
        markerColor: 'red',
        prefix: 'fa',
        iconColor: 'dark-gray'
    }),

    endMarkerMini: L.AwesomeMarkers.icon({
        icon: 'flag',
        markerColor: 'red',
        prefix: 'fa',
        iconColor: 'dark-gray',
        iconSize: [35,46]
    }),

};
