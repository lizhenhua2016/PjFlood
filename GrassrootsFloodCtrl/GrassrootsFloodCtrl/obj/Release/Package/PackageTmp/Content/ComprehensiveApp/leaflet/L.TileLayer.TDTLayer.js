/*
 * L.TileLayer.TDTLayer is used for tianditu tile layers.
 */
L.TileLayer.TdtTileLayer = L.TileLayer.extend({
    defaultWmtsParams: {
        service: 'WMTS',
        request: 'GetTile',
        version: '1.0.0',
        layer: '',
        style: '',
        tilematrixSet: '',
        format: 'tiles'
    },

    initialize: function (url, options) { // (String, Object)
        this._url = url;
        var wmtsParams = L.extend({}, this.defaultWmtsParams),
        tileSize = options.tileSize || this.options.tileSize;
        if (options.detectRetina && L.Browser.retina) {
            wmtsParams.width = wmtsParams.height = tileSize * 2;
        } else {
            wmtsParams.width = wmtsParams.height = tileSize;
        }

        for (var i in options) {
            // all keys that are not TileLayer options go to WMTS params
            if (!this.options.hasOwnProperty(i) && i != "matrixIds") {
                wmtsParams[i] = options[i];
            }
        }

        this.wmtsParams = wmtsParams;
        this.matrixIds = options.matrixIds || this.getDefaultMatrix();
        L.setOptions(this, options);
    },

    onAdd: function (map) {
        L.TileLayer.prototype.onAdd.call(this, map);
    },

    //瓦片坐标、缩放级别计算瓦片号
    getTileUrl: function (tilePoint, zoom) { // (Point, Number) -> String
        var map = this._map;
        var crs = map.options.crs;
        var tileSize = this.options.tileSize;

        //north-west左上角切片坐标
        var nwPoint = tilePoint.multiplyBy(tileSize);
        nwPoint.x += 1;
        nwPoint.y -= 1;
        //south-east右下角切片坐标
        var sePoint = nwPoint.add(new L.Point(tileSize, tileSize));

        //north-west左上角经纬度坐标
        var nw = crs.project(map.unproject(nwPoint, zoom));
        //south-east右下角经纬度坐标
        var se = crs.project(map.unproject(sePoint, zoom));

        var tilewidth = se.x - nw.x;
        //zoom = map.getZoom();
        zoom = tilePoint.z;
        var ident = this.matrixIds[zoom].identifier;
        var x0 = this.matrixIds[zoom].topLeftCorner.lng;
        var y0 = this.matrixIds[zoom].topLeftCorner.lat;
        
        var tilecol = Math.floor((nw.x - x0) / tilewidth);
        var tilerow = -Math.floor((nw.y - y0) / tilewidth);

        var lUrl = this._url;

        if (!lUrl) { //如果初始化时没有输入url，则需要从lyrAttribute中取
            if (this.options.lyrAttribute) {
                var distinctLayer = this.options.lyrAttribute; 

                var lZoom = this._getZoomForUrl();
                if (distinctLayer.length > 0) {
                    for (var i = 0; i < distinctLayer.length; i++) {
                        if (lZoom <= distinctLayer[i].maxZoom && lZoom >= distinctLayer[i].minZoom) {
                            lUrl = distinctLayer[i].url;
                            this.wmtsParams.layer = distinctLayer[i].layer;
                            this.wmtsParams.format = distinctLayer[i].format;
                            break;
                        }
                    }
                }
            }
        }
        if (lUrl) {
            url = L.Util.template(lUrl, { s: this._getSubdomain(tilePoint) });
            return url + L.Util.getParamString(this.wmtsParams, lUrl) + "&tilematrix=" + ident + "&tilerow=" + tilerow + "&tilecol=" + tilecol;
        }
    },

    setParams: function (params, noRedraw) {
        L.extend(this.wmtsParams, params);
        if (!noRedraw) {
            this.redraw();
        }
        return this;
    },

    //获取默认国家2000坐标系的TielMatrix
    getDefaultMatrix : function () {
        var matrixIds4490 = new Array(20);
        for (var i= 1; i<21; i++) {
            matrixIds4490[i]= {
                identifier    : "" + i,
                topLeftCorner : new L.LatLng(90,-180)
            };
        }
        return matrixIds4490;
    }

});

L.tileLayer.tdtTileLayer = function (url, options) {
    return new L.TileLayer.TdtTileLayer(url, options);
};