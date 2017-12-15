/*
 * leaflet 的扩展。
 * 1. 增加适用于天地图的CRS（EPSG4490）
 * 2. 增加自定义加载边界的TileLayer(CustomArcGISServerLayer)（暂只适用于临安市，需要外部的层文件支持）
 * 3. 增加可定义多个地图接口地址和绽放级别的TileLayer（SplitZoomLevelLayer),用于支持不同绽放级别下从不同地图服务接口加载。
 *
 * (c) 2015 junwei.hu
 */

/*
 * 适用于天地图的CRS
 */
L.CRS.EPSG4490 = L.extend({}, L.CRS, {
    code: 'EPSG:4490',

    projection: L.Projection.LonLat,
    transformation: new L.Transformation(1 / 360, 0.5, -1 / 360, 0.25)
});

/*
 * 用于加载自定义图层（目前仅限于临安市的地理边界）
 */
L.TileLayer.CustomArcGISServerLayer = L.TileLayer.extend({
    initialize: function (url, options) { // (String, Object)
        this._url = url;
        L.setOptions(this, options);
    },

    getTileUrl: function (tilePoint, zoom) { // (Point, Number) -> String
        this._adjustTilePoint(tilePoint);
        var x = tilePoint.x;
        var y = tilePoint.y;
        var rowID = "00000000" + y.toString(16);
        var colID = "00000000" + x.toString(16);
        rowID = rowID.substr(rowID.length - 8);
        colID = colID.substr(colID.length - 8);
        return this._url + (this._getZoomForUrl() - 1) + "/R" + rowID + "/C" + colID + ".png";
    }
});

L.tileLayer.customArcGISServerLayer = function (url, options) {
    return new L.TileLayer.CustomArcGISServerLayer(url, options);
};

/*
 * 创建图层时支持按不同缩放级别加载不同的地址。
 * 参数 {sources} 为一个数组，每项包含：url, minZoom, maxZoom, subdomains.
 */
L.TileLayer.SplitZoomLevelLayer = L.TileLayer.extend({
    initialize: function (sources, options) {  // (Array, Object)
        options = L.setOptions(this, options);

        if (options.bounds) {
            options.bounds = L.latLngBounds(options.bounds);
        }

        this._sources = sources || [];
        if (this._sources.length == 0)
            throw new Error("must set least one map sources.");

        for (var i = 0; i < this._sources.length; i++) {
            var source = this._sources[i];
            source.minZoom = source.minZoom || 0;
            source.maxZoom = source.maxZoom || 18;
            source.subdomains = source.subdomains || [];
        }
    },

    getTileUrl: function (tilePoint) { // (Point) -> String
        var zoom = tilePoint.z,
            url,
            subdomains;
        for (var i = 0; i < this._sources.length; i++) {
            var source = this._sources[i];
            if (zoom >= source.minZoom && zoom <= source.maxZoom) {
                url = source.url;
                subdomains = source.subdomains;
                break;
            }
        }

        return L.Util.template(url, L.extend({
            s: this._getSubdomain(tilePoint, subdomains),
            z: tilePoint.z,
            x: tilePoint.x,
            y: tilePoint.y
        }, this.options));
    },

    setMapSource: function (sources) {
        this._sources = sources || [];
    },

    _getSubdomain: function (tilePoint, subdomains) {
        if (!subdomains || subdomains.length == 0)
            return '';
        var index = Math.abs(tilePoint.x + tilePoint.y) % subdomains.length;
        return subdomains[index];
    }
});

L.tileLayer.splitZoomLevelLayer = function (sources, options) {
    return new L.TileLayer.SplitZoomLevelLayer(sources, options);
};

/*创建针对天地图的地图*/
L.tianDiTuMap = function (mapEl, options) {
    if (!(this instanceof L.tianDiTuMap)) {
        return new L.tianDiTuMap(mapEl, options);
    }

    options.crs = options.crs || L.CRS.EPSG4490;
    //options.crs = options.crs || L.CRS.EPSG900913;
    options.zoom = options.zoom || 12;

    var map = new L.map(mapEl, options);
    var mapType = options.mapType || 'img',
        mapSources = this.getMapSources(mapType),
        textLayerUrl = this.getTextLayerUrl(mapType);

    this.mapType = mapType;
    this.tileLayer = new L.tileLayer.splitZoomLevelLayer(
        mapSources,
        {
            minZoom: options.minZoom || 9,
            maxZoom: options.maxZoom || 18
        },
        [1, 2]);

    this.tileLayer.addTo(map);

    this.textLayer = new L.tileLayer(
        textLayerUrl,
        {
            subdomains: [0, 1, 2, 3, 4, 5, 6, 7],
            minZoom: 9,
            maxZoom: 18,
            maxNativeZoom: 18,
            attribution: ''
        });

    this.textLayer.addTo(map);

    this.realMap = map;
    //return this;
};

L.tianDiTuMap.prototype.getTextLayerUrl = function (mapType) {
    if (mapType == "googlemap") {
        return "";
    }
    else if (mapType == "googlesatelite") {
        return "";
    }
    else if (mapType == "googlesatelite") {
        return "";
    }
    else {
        return 'http://t{s}.tianditu.cn/DataServer?T=' + (mapType === 'img' ? 'cia_c' : 'cva_c') + '&X={x}&Y={y}&L={z}';
    }
};

L.tianDiTuMap.prototype.getMapSources = function (mapType) {
    if (mapType == "googlemap") {
        return [{ url: 'http://mt{s}.google.cn/vt/lyrs=m@235000000&hl=zh-CN&gl=CN&src=app&x={x}&y={y}&z={z}&s=Galileo', subdomains: [0, 1, 2, 3], minZoom: 8, maxZoom: 18, maxNativeZoom: 18 }];
    }
    else if (mapType == "googlesatelite") {
        return [{ url: 'http://mt{s}.google.cn/vt/lyrs=y&hl=zh-CN&gl=CN&src=app&x={x}&y={y}&z={z}&s=G', subdomains: [0, 1, 2, 3], minZoom: 8, maxZoom: 18, maxNativeZoom: 18 }];
    }
    else if (mapType == "googlesatelite") {
        return [{ url: 'http://mt{s}.google.cn/vt/lyrs=t@131,r@216000000&hl=zh-CN&gl=CN&src=app&x={x}&y={y}&z={z}&s=Gal', subdomains: [0, 1, 2, 3], minZoom: 8, maxZoom: 18, maxNativeZoom: 18 }];
    } else if (mapType == "water") {
        return [{ url: 'http://114.215.249.116:6080/arcgis/rest/services/zjsl/zjslmap/MapServer/WMTS', subdomains: [0, 1, 2, 3], minZoom: 8, maxZoom: 18, maxNativeZoom: 18 }];
    }
    else {
        var imgFlag = mapType == 'img' ? 'img_c' : 'vec_c';
        var url = mapType == 'img' ?
            'http://srv.zjditu.cn/ZJDOM_2D/wmts?SERVICE=WMTS&VERSION=1.0.0&REQUEST=GetTile&LAYER=ZJDOM2W1&FORMAT=image/jpeg&TILEMATRIXSET=Matrix_0&TILEMATRIX={z}&TILEROW={y}&TILECOL={x}' :
            'http://srv.zjditu.cn/ZJEMAP_2D/wmts?SERVICE=WMTS&VERSION=1.0.0&REQUEST=GetTile&LAYER=ZJEMAP&FORMAT=image/png&TILEMATRIXSET=TileMatrixSet0&TILEMATRIX={z}&STYLE=default&TILEROW={y}&TILECOL={x}';
        return [
          {
              url: 'http://t{s}.tianditu.cn/DataServer?T=' + imgFlag + '&X={x}&Y={y}&L={z}', minZoom: 0, maxZoom: 13, subdomains: [0, 1, 2, 3, 4, 5, 6, 7], attribution: ''
          },
          {
              url: url, minZoom: 14, maxZoom: 18, attribution: ''
          }
        ];
    }
};

/*
 * 地图切换（交通图，卫星图）
 */
L.tianDiTuMap.prototype.switchMapType = function (mapType) {
    var mapSources = this.getMapSources(mapType),
        textLayerUrl = this.getTextLayerUrl(mapType);

    this.tileLayer.setMapSource(mapSources);
    this.tileLayer.redraw();

    this.textLayer.setUrl(textLayerUrl);
};