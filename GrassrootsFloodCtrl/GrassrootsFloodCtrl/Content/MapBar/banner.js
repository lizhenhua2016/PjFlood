var DRAWING = false; //是否正在绘制
var DRAWLAYERS = [];
var ISMEASURE = false;  //是否是量距
var MEASURETOOLTIP;  //量距提示
var MEASUREAREATOOLTIP;  //量面提示
var MEASURERESULT = 0;

var DRAWPOLYLINE; //绘制的折线
var DRAWMOVEPOLYLINE; //绘制过程中的折线
var DRAWPOLYLINEPOINTS = []; //绘制的折线的节点集

var DRAWPOLYGON; //绘制的面
var DRAWMOVEPOLYGON; //绘制过程中的面
var DRAWPOLYGONPOINTS = []; //绘制的面的节点集

var DRAWCIRCLE; //绘制的

//绘制折线
function startDrawLine(func) {

    MEASURERESULT = 0;

    _map.getContainer().style.cursor = 'crosshair';

    _map.on('mousedown', function (e) {
    
        DRAWING = true;

        DRAWPOLYLINEPOINTS.push(e.latlng);
        if (DRAWPOLYLINEPOINTS.length > 1 && ISMEASURE) {
            MEASURERESULT += e.latlng.distanceTo(DRAWPOLYLINEPOINTS[DRAWPOLYLINEPOINTS.length - 2]);
        }
        DRAWPOLYLINE.addLatLng(e.latlng);
    });

    _map.on('mousemove', function (e) {
        if (DRAWING) {
            if (DRAWMOVEPOLYLINE != undefined && DRAWMOVEPOLYLINE != null) {
                _map.removeLayer(DRAWMOVEPOLYLINE);
            }
            var prevPoint = DRAWPOLYLINEPOINTS[DRAWPOLYLINEPOINTS.length - 1];
            DRAWMOVEPOLYLINE = new L.Polyline([prevPoint, e.latlng], shapeOptions);
            _map.addLayer(DRAWMOVEPOLYLINE);

            if (ISMEASURE) {
                var distance = MEASURERESULT + e.latlng.distanceTo(DRAWPOLYLINEPOINTS[DRAWPOLYLINEPOINTS.length - 1]);
                MEASURETOOLTIP.updatePosition(e.latlng);
                MEASURETOOLTIP.updateContent({
                    text: '单击确定点，双击结束！',
                    subtext: "总长：" + (distance / 1000).toFixed(2) + "公里"
                });
            }

        }
    });

    _map.on('dblclick', function (e) {

        _map.getContainer().style.cursor = '';

        if (DRAWING) {

            if (DRAWMOVEPOLYLINE != undefined && DRAWMOVEPOLYLINE != null) {
                _map.removeLayer(DRAWMOVEPOLYLINE);
                DRAWMOVEPOLYLINE = null;
            }

            if (DRAWPOLYLINEPOINTS.length > 1 && ISMEASURE) {

                MEASURERESULT += e.latlng.distanceTo(DRAWPOLYLINEPOINTS[DRAWPOLYLINEPOINTS.length - 2]);

                var distanceLabel = L.marker(DRAWPOLYLINEPOINTS[DRAWPOLYLINEPOINTS.length - 1], {
                    icon: new L.divIcon({
                        className: 'DistanceLabelStyle',
                        iconAnchor: [-8, 15],
                        html: "<span class='bubbleLabel'><span class='bubbleLabel-bot bubbleLabel-bot-left'></span><span class='bubbleLabel-top bubbleLabel-top-left'></span><span>总长：" + (MEASURERESULT / 1000).toFixed(2) + "公里" + "</span></span>"
                    }),
                }).addTo(_map);

                DRAWLAYERS.push(distanceLabel);
            }

            //移除提示框
            if (MEASURETOOLTIP) {
                MEASURETOOLTIP.dispose();
            }

            DRAWLAYERS.push(DRAWPOLYLINE);

            if (func) {
                func(DRAWPOLYLINEPOINTS);
            }

            DRAWPOLYLINEPOINTS = [];
            DRAWING = false;
            ISMEASURE = false;
            _map.off('mousedown');
            _map.off('mousemove');
            _map.off('dblclick');
        }
    });

    var shapeOptions = {
        stroke: true,
        color: '#f06eaa',
        weight: 2,
        opacity: 0.5,
        fill: false,
        clickable: true
    },

    DRAWPOLYLINE = new L.Polyline([], shapeOptions);
    _map.addLayer(DRAWPOLYLINE);

    if (ISMEASURE) {
        MEASURETOOLTIP = new L.Tooltip(_map);
    }
}

//绘制多边形
function startDrawPolygon(func) {

    MEASURERESULT = 0;

    _map.getContainer().style.cursor = 'crosshair';

    _map.on('mousedown', function (e) {
        DRAWING = true;
        DRAWPOLYGONPOINTS.push(e.latlng);
        DRAWPOLYGON.addLatLng(e.latlng);
    });

    _map.on('mousemove', function (e) {
        if (DRAWING) {
            if (DRAWMOVEPOLYGON != undefined && DRAWMOVEPOLYGON != null) {
                _map.removeLayer(DRAWMOVEPOLYGON);
            }
            var prevPoint = DRAWPOLYGONPOINTS[DRAWPOLYGONPOINTS.length - 1];
            var firstPoint = DRAWPOLYGONPOINTS[0];
            DRAWMOVEPOLYGON = new L.Polygon([firstPoint, prevPoint, e.latlng], shapeOptions);
            _map.addLayer(DRAWMOVEPOLYGON);

            if (ISMEASURE && DRAWPOLYGONPOINTS.length > 1) {
                var tempPoints = [];
                for (var i = 0; i < DRAWPOLYGONPOINTS.length; i++) {
                    tempPoints.push(DRAWPOLYGONPOINTS[i]);
                }
                tempPoints.push(e.latlng);
                var distance = CalArea(tempPoints);
                MEASUREAREATOOLTIP.updatePosition(e.latlng);
                MEASUREAREATOOLTIP.updateContent({
                    text: '单击确定点，双击结束！',
                    subtext: "总面积：" + (distance / 1000000).toFixed(3) + '平方公里'
                });
            }
        }
    });

    _map.on('dblclick', function (e) {
        _map.getContainer().style.cursor = '';

        if (DRAWING) {

            if (DRAWMOVEPOLYGON != undefined && DRAWMOVEPOLYGON != null) {
                _map.removeLayer(DRAWMOVEPOLYGON);
                DRAWMOVEPOLYGON = null;
            }

            if (DRAWPOLYGONPOINTS.length > 2 && ISMEASURE) {

                MEASURERESULT = CalArea(DRAWPOLYGONPOINTS);

                var distanceLabel = L.marker(e.latlng, {
                    icon: new L.divIcon({
                        className: 'DistanceLabelStyle',
                        iconAnchor: [-8, 15],
                        html: "<span class='bubbleLabel'><span class='bubbleLabel-bot bubbleLabel-bot-left'></span><span class='bubbleLabel-top bubbleLabel-top-left'></span><span>总面积：" + (MEASURERESULT / 1000000).toFixed(3) + "平方公里" + "</span></span>"
                    }),
                }).addTo(_map);

                DRAWLAYERS.push(distanceLabel);
            }

            //移除提示框
            if (MEASUREAREATOOLTIP) {
                MEASUREAREATOOLTIP.dispose();
            }

            DRAWLAYERS.push(DRAWPOLYGON);

            if (func) {
                func(DRAWPOLYGONPOINTS);
            }

            DRAWPOLYGONPOINTS = [];
            DRAWING = false;
            ISMEASURE = false;
            _map.off('mousedown');
            _map.off('mousemove');
            _map.off('dblclick');
        }
    });

    var shapeOptions = {
        stroke: true,
        color: '#f06eaa',
        weight: 2,
        opacity: 0.5,
        fill: true,
        fillColor: null,
        fillOpacity: 0.2,
        clickable: true
    },

    DRAWPOLYGON = new L.Polygon([], shapeOptions);
    _map.addLayer(DRAWPOLYGON);

    if (ISMEASURE) {
        MEASUREAREATOOLTIP = new L.Tooltip(_map);
    }
}

//量距
function measure() {

    ISMEASURE = true;
    startDrawLine();
}

//量面
function measureArea() {
    ISMEASURE = true;
    startDrawPolygon();
}

//清除标绘图层
function clearMap() {
    if (MEASURETOOLTIP) {
        MEASURETOOLTIP.dispose();
    }
    if (MEASUREAREATOOLTIP) {
        MEASUREAREATOOLTIP.dispose();
    }
    for (var i = 0; i < DRAWLAYERS.length; i++) {
        _map.removeLayer(DRAWLAYERS[i]);
    }
    DRAWLAYERS = [];
}

//面积计算
function CalArea(latLngs) {
    var pointsCount = latLngs.length,
		area = 0.0,
		d2r = L.LatLng.DEG_TO_RAD,
		p1, p2;

    if (pointsCount > 2) {
        for (var i = 0; i < pointsCount; i++) {
            p1 = latLngs[i];
            p2 = latLngs[(i + 1) % pointsCount];
            area += ((p2.lng - p1.lng) * d2r) *
					(2 + Math.sin(p1.lat * d2r) + Math.sin(p2.lat * d2r));
        }
        area = area * 6378137.0 * 6378137.0 / 2.0;
    }

    return Math.abs(area);
}