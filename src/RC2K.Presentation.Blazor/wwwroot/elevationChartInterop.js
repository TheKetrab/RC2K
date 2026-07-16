
window.elevationChartInterop = {

    // cache parsed elevation rows per source CSV file so we don't re-fetch/re-parse on every re-render
    fileCache: {},

    init: async function (elementId, stageCode) {

        const container = document.getElementById(elementId);
        if (!container) return;

        container.innerHTML = '';

        if (!stageCode) {
            return;
        }

        let points;
        try {
            points = await this.loadPoints(stageCode);
        } catch (e) {
            container.innerHTML = '<div style="padding: 1em; color: #888;">Elevation data unavailable.</div>';
            return;
        }

        if (!points || points.length === 0) {
            container.innerHTML = '<div style="padding: 1em; color: #888;">Elevation data unavailable.</div>';
            return;
        }

        points = this.smooth(points);

        this.render(container, points);
    },

    // Raw elevation samples are noisy; smooth them with a moving average over distance
    // so the plotted profile reads as a clean curve instead of jagged spikes.
    smooth: function (points, windowMeters = 150) {

        const smoothed = points.map((p, i) => {
            let sum = 0;
            let count = 0;

            for (let j = i; j >= 0 && points[i].distance - points[j].distance <= windowMeters; j--) {
                sum += points[j].elevation;
                count++;
            }
            for (let j = i + 1; j < points.length && points[j].distance - points[i].distance <= windowMeters; j++) {
                sum += points[j].elevation;
                count++;
            }

            return { ...p, elevation: sum / count };
        });

        return smoothed;
    },

    loadPoints: async function (stageCode) {

        const rallyBase = Math.floor(stageCode / 10) * 10;
        const fileName = `stage_waypoints_elevation_${rallyBase + 1}_${rallyBase + 6}.csv`;
        const url = `data/elevation/${fileName}`;

        let rows = this.fileCache[fileName];
        if (!rows) {
            const response = await fetch(url);
            if (!response.ok) {
                throw new Error(`Elevation data file not found: ${fileName}`);
            }
            const text = await response.text();
            rows = this.parseCsv(text);
            this.fileCache[fileName] = rows;
        }

        const stageRows = rows
            .filter(r => r.stageCode === stageCode)
            .sort((a, b) => a.pointIndex - b.pointIndex);

        if (stageRows.length === 0) {
            throw new Error(`No elevation rows found for stage ${stageCode}`);
        }

        const points = [];
        let distance = 0;
        for (let i = 0; i < stageRows.length; i++) {
            const { lat, lng, elevation } = stageRows[i];
            if (i > 0) {
                distance += this.haversine(stageRows[i - 1].lat, stageRows[i - 1].lng, lat, lng);
            }
            points.push({ lat, lng, elevation, distance });
        }

        return points;
    },

    // CSV columns: StageCode,PointIndex,x,y,PointsInStage,elevation (x = lng, y = lat)
    parseCsv: function (text) {

        const lines = text.split(/\r?\n/).filter(l => l.length > 0);
        const rows = [];

        for (let i = 1; i < lines.length; i++) { // skip header
            const cols = lines[i].split(',');
            if (cols.length < 6) continue;

            rows.push({
                stageCode: Number.parseInt(cols[0], 10),
                pointIndex: Number.parseInt(cols[1], 10),
                lng: Number.parseFloat(cols[2]),
                lat: Number.parseFloat(cols[3]),
                elevation: Number.parseFloat(cols[5])
            });
        }

        return rows;
    },

    haversine: function (lat1, lon1, lat2, lon2) {
        const R = 6371000;
        const toRad = d => d * Math.PI / 180;
        const dLat = toRad(lat2 - lat1);
        const dLon = toRad(lon2 - lon1);
        const a = Math.sin(dLat / 2) ** 2 + Math.cos(toRad(lat1)) * Math.cos(toRad(lat2)) * Math.sin(dLon / 2) ** 2;
        return 2 * R * Math.asin(Math.sqrt(a));
    },

    // Builds a smooth SVG path (Catmull-Rom converted to cubic Beziers) through the points
    // instead of straight segments, so the curve renders smoothly even with few samples.
    buildSmoothPath: function (points, xScale, yScale) {

        const pts = points.map(p => [xScale(p.distance), yScale(p.elevation)]);
        if (pts.length < 3) {
            return pts.reduce((d, [x, y], i) => d + (i === 0 ? `M ${x} ${y}` : ` L ${x} ${y}`), '');
        }

        let d = `M ${pts[0][0]} ${pts[0][1]}`;
        for (let i = 0; i < pts.length - 1; i++) {
            const p0 = pts[i - 1] || pts[i];
            const p1 = pts[i];
            const p2 = pts[i + 1];
            const p3 = pts[i + 2] || p2;

            const cp1x = p1[0] + (p2[0] - p0[0]) / 6;
            const cp1y = p1[1] + (p2[1] - p0[1]) / 6;
            const cp2x = p2[0] - (p3[0] - p1[0]) / 6;
            const cp2y = p2[1] - (p3[1] - p1[1]) / 6;

            d += ` C ${cp1x} ${cp1y}, ${cp2x} ${cp2y}, ${p2[0]} ${p2[1]}`;
        }

        return d;
    },

    render: function (container, points) {

        const width = container.clientWidth || 600;
        const height = container.clientHeight || 200;
        const padding = { top: 10, right: 10, bottom: 20, left: 42 };

        // Fixed scale so altitude profiles are visually comparable across stages.
        const minEl = 0;
        const maxEl = 600;
        const elRange = maxEl - minEl;
        const totalDist = points[points.length - 1].distance || 1;

        const xScale = d => padding.left + (d / totalDist) * (width - padding.left - padding.right);
        const yScale = e => {
            const clamped = Math.min(maxEl, Math.max(minEl, e));
            return height - padding.bottom - ((clamped - minEl) / elRange) * (height - padding.top - padding.bottom);
        };

        const svgNS = 'http://www.w3.org/2000/svg';
        const svg = document.createElementNS(svgNS, 'svg');
        svg.setAttribute('width', '100%');
        svg.setAttribute('height', '100%');
        svg.setAttribute('viewBox', `0 0 ${width} ${height}`);
        svg.style.display = 'block';
        svg.style.cursor = 'crosshair';

        let lineD = this.buildSmoothPath(points, xScale, yScale);
        const areaD = `${lineD} L ${xScale(totalDist)} ${height - padding.bottom} L ${xScale(0)} ${height - padding.bottom} Z`;

        const area = document.createElementNS(svgNS, 'path');
        area.setAttribute('d', areaD);
        area.setAttribute('fill', 'rgba(33, 150, 243, 0.15)');
        svg.appendChild(area);

        const line = document.createElementNS(svgNS, 'path');
        line.setAttribute('d', lineD);
        line.setAttribute('fill', 'none');
        line.setAttribute('stroke', '#2196F3');
        line.setAttribute('stroke-width', '2');
        svg.appendChild(line);

        [minEl, maxEl].forEach(e => {
            const label = document.createElementNS(svgNS, 'text');
            label.textContent = `${Math.round(e)} m`;
            label.setAttribute('x', 2);
            label.setAttribute('y', yScale(e) + (e === minEl ? -3 : 10));
            label.setAttribute('font-size', '10');
            label.setAttribute('fill', '#666');
            svg.appendChild(label);
        });

        const hoverLine = document.createElementNS(svgNS, 'line');
        hoverLine.setAttribute('stroke', '#999');
        hoverLine.setAttribute('stroke-dasharray', '3,3');
        hoverLine.setAttribute('y1', padding.top);
        hoverLine.setAttribute('y2', height - padding.bottom);
        hoverLine.style.display = 'none';
        svg.appendChild(hoverLine);

        const hoverDot = document.createElementNS(svgNS, 'circle');
        hoverDot.setAttribute('r', 5);
        hoverDot.setAttribute('fill', '#e53935');
        hoverDot.setAttribute('stroke', 'white');
        hoverDot.setAttribute('stroke-width', '2');
        hoverDot.style.display = 'none';
        svg.appendChild(hoverDot);

        const tooltip = document.createElement('div');
        tooltip.style.position = 'absolute';
        tooltip.style.pointerEvents = 'none';
        tooltip.style.background = 'rgba(0, 0, 0, 0.75)';
        tooltip.style.color = 'white';
        tooltip.style.padding = '2px 6px';
        tooltip.style.borderRadius = '4px';
        tooltip.style.fontSize = '12px';
        tooltip.style.display = 'none';
        tooltip.style.transform = 'translate(-50%, -130%)';
        tooltip.style.whiteSpace = 'nowrap';

        container.style.position = 'relative';
        container.appendChild(svg);
        container.appendChild(tooltip);

        const findNearest = distanceAtX => {
            let nearest = points[0];
            let minDiff = Infinity;
            for (const p of points) {
                const diff = Math.abs(p.distance - distanceAtX);
                if (diff < minDiff) {
                    minDiff = diff;
                    nearest = p;
                }
            }
            return nearest;
        };

        const onMove = evt => {
            const rect = svg.getBoundingClientRect();
            const scaleX = width / rect.width;
            const x = (evt.clientX - rect.left) * scaleX;
            const distanceAtX = ((x - padding.left) / (width - padding.left - padding.right)) * totalDist;

            if (distanceAtX < 0 || distanceAtX > totalDist) {
                onLeave();
                return;
            }

            const p = findNearest(distanceAtX);
            const px = xScale(p.distance);
            const py = yScale(p.elevation);

            hoverLine.setAttribute('x1', px);
            hoverLine.setAttribute('x2', px);
            hoverLine.style.display = '';

            hoverDot.setAttribute('cx', px);
            hoverDot.setAttribute('cy', py);
            hoverDot.style.display = '';

            tooltip.style.display = '';
            tooltip.style.left = `${(px / width) * rect.width}px`;
            tooltip.style.top = `${(py / height) * rect.height}px`;
            tooltip.textContent = `${Math.round(p.elevation)} m · ${(p.distance / 1000).toFixed(2)} km`;

            if (window.mapInterop && window.mapInterop.showHoverMarker) {
                window.mapInterop.showHoverMarker(p.lat, p.lng);
            }
        };

        const onLeave = () => {
            hoverLine.style.display = 'none';
            hoverDot.style.display = 'none';
            tooltip.style.display = 'none';

            if (window.mapInterop && window.mapInterop.hideHoverMarker) {
                window.mapInterop.hideHoverMarker();
            }
        };

        svg.addEventListener('mousemove', onMove);
        svg.addEventListener('mouseleave', onLeave);
    }
};
