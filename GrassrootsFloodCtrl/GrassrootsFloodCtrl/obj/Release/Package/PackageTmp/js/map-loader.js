!(function ($, window) {
    var polygonReg = /\(\((.*?)\)\)/g, match;

    var mapLoader = function (opts) {
        opts = opts || {};

        this.map = null;
        this.shapeLayer = null;
        this.shapes = [];
        this.mapId = opts.mapId || 'map';
        this.mapType = opts.mapType || 'img';
        this.popupFooter = opts.popupFooter || '';

        L.Icon.Default.imagePath = abp.appPath + 'Content/leaflet/images';
        this.szIcon = L.icon({ iconUrl: abp.appPath + 'Content/leaflet/images/sz.png', iconSize: [22, 12] });
        this.bsIcon = L.icon({ iconUrl: abp.appPath + 'Content/leaflet/images/bs.png', iconSize: [36, 15] });
    }

    /*
     * 返回地图对象
     */
    mapLoader.prototype.getMap = function () {
        return this.map;
    };

    /*
     * 返回地图的主形状图层
     */
    mapLoader.prototype.getShapeLayer = function () {
        return this.shapeLayer;
    };

    /*
     * 涝片工程形状
     * @param(data) 形状的几何数据
     */
    mapLoader.prototype.createShape = function (opts) {
        opts = opts || {};
        var data = opts.data,
            styleFn = opts.styleFn,
            markerStyleFn = opts.markerStyleFn,
            shapeCallback = opts.shapeCallback,
            zoomLevel = opts.zoomLevel || 14,
            that = this;
        if (typeof (data) == 'undefined' || data.length == 0 || !this.map)
            return false;

        this.shapes = [];
        var defaultStyle = { color: 'blue' }, featureStyle = {};
        var shape = null, ev;
        $.each(data, function (i, project) {
            if (!project.shapeData || !project.shapeData.shapeLines)
                return;

            if (project.shapeType == "Point") {
                var pointData = project.shapeData.shapeLines[0].latLngs[0];
                if (typeof (markerStyleFn) === 'function') {
                    featureStyle = {};
                    featureStyle = $.extend(featureStyle, (markerStyleFn.call(null, project) || {}));
                } else {
                    featureStyle = { icon: project.sluiceType == 'sz' ? that.szIcon : that.bsIcon };
                }
                shape = L.marker(L.latLng(pointData.lat, pointData.lng), featureStyle);

                shape.options = shape.options || {};
                that.shapeLayer.addLayer(shape);
            } else {
                if (typeof (styleFn) === 'function') {
                    featureStyle = {};
                    featureStyle = undefined != project && project.gcType == "qd" ? { color: 'red' } :
                        $.extend(featureStyle, defaultStyle, (styleFn.call(null, project) || {}));
                } else {
                    featureStyle = defaultStyle;
                }

                //只前只支持以下几种类型，暂不包含（GeometryCollection）
                if (project.shapeType != "Polygon" && project.shapeType != "LineString" &&
                    project.shapeType != "MultiLineString" && project.shapeType != "MultiPolygon") {
                    return;
                }

                //MultiLineString, MultiLinePolygon因为Leaflet.Draw编辑时不支持，所有置换成PolyLine和Polygon分别添加。
                shape = project.shapeType == "Polygon" ? L.polygon([], featureStyle) :
                            project.shapeType == "LineString" ? L.polyline([], featureStyle) : null;
                //project.shapeType == "MultiLineString" ? L.multiPolyline([], featureStyle) :
                //project.shapeType == "MultiPolygon" ? L.multiPolygon([], featureStyle) : null;

                var pointArr = [], subShape;
                $.each(project.shapeData.shapeLines, function (j, line) {
                    subShape = null;
                    if (project.shapeType == "MultiLineString") {
                        subShape = L.polyline([], featureStyle);
                    }
                    if (project.shapeType == "MultiPolygon") {
                        subShape = L.polygon([], featureStyle);
                    }
                    $.each(line.latLngs, function (k, latLng) {
                        if (project.shapeType == "MultiLineString" || project.shapeType == "MultiPolygon") {
                            subShape.addLatLng(L.latLng(latLng.lat, latLng.lng));
                        } else {
                            shape.addLatLng(L.latLng(latLng.lat, latLng.lng));
                        }
                    });

                    if (subShape != null) {
                        subShape.rawData = project;
                        that.shapes.push(subShape);   //cache it

                        that.shapeLayer.addLayer(subShape);

                        subShape.bindLabel(project.name);
                        if ($.isFunction(shapeCallback)) {
                            shapeCallback.call(null, subShape);
                        }
                    }
                });

                shape && that.shapeLayer.addLayer(shape);
            }

            if (shape) {
                shape.rawData = project;
                shape.bindLabel(project.name);

                that.shapes.push(shape);    //cache it
                if ($.isFunction(shapeCallback)) {
                    shapeCallback.call(null, shape);
                }
            }
        });

        var center;
        if (this.shapeLayer && this.shapeLayer.getLayers().length > 0) {
            center = this.shapeLayer.getBounds().getCenter();
        }

        if (center) {
            this.map.setView(center, zoomLevel);
        }
    };

    /*
     *  生成涝片形状
     *  @param(separateLayer) 是否在分开的图层上显示。
     */
    mapLoader.prototype.createWaterLoggingShape = function (opts) {
        opts = opts || {};
        var name = opts.name,
            coordStr = opts.coordStr,
            separateLayer = opts.separateLayer,
            shapeCallback = opts.shapeCallback;

        if (!coordStr)
            return false;

        var shape, points = [], i, pointPair, x, y;
        while ((match = polygonReg.exec(coordStr)) != null) {
            var coords = match[1].split(',');
            for (i = 0; i < coords.length; i++) {
                pointPair = $.trim(coords[i]).split(' ');
                if (pointPair.length == 2) {
                    x = $.trim(pointPair[0]);
                    y = $.trim(pointPair[1]);

                    points.push(new L.latLng(y, x));
                }
            }
        }

        shape = L.polygon(points, { color: '#1D7CAA', fill: true });
        shape.bindLabel(name);
        if (separateLayer === true) {
            var layer = new L.featureGroup();
            layer.addLayer(shape);

            this.map.addLayer(layer);
        } else {
            this.shapeLayer.addLayer(shape);
        }

        if ($.isFunction(shapeCallback)) {
            shapeCallback.call(null, shape);
        }
    };

    /*
     * 搜索形状
     */
    mapLoader.prototype.findShape = function (code) {
        var layerGroups = [this.shapeLayer],
            layerGroup,
            findedLayer;
        for (var key in layerGroups) {
            if (findedLayer)
                break;

            layerGroup = layerGroups[key];
            layerGroup.eachLayer(function (layer) {
                if (layer.rawData && layer.rawData.code === code) {
                    findedLayer = layer;
                }
            });
        }
        return findedLayer || null;
    };

    /*
     * 遍历所有符合条件的形状，并依次调用函数。具体条件根据包含在 RawData 中的属性。
     */
    mapLoader.prototype.mapShapesByParam = function (attr, value, func) {
        if (typeof (func) !== 'function')
            return;

        for (var i = 0; i < this.shapes.length; i++) {
            if (this.shapes[i].rawData && this.shapes[i].rawData[attr] === value)
                func.call(null, this.shapes[i]);
        }
    };

    /*
     * 得到形状样式。
     *  @param(isBuilding) 是否在建
     *  @param(projectType) 工程措施
     */
    mapLoader.prototype.getShapeStyle = function (code, type, isBuilding, isFinished, projectType, sluiceType) {
        var isCurrent = isFinished || projectType == "" || projectType == null || projectType == "现有",
                   color, dashArray = null, weight = 5, iconName;

        isBuilding = !!isFinished ? false : isBuilding;  //如果已完成，则认为非在建。

        // 对于在建的工程，无论河道还是水闸（闸站）全部用绿色（#39ff0c）显示。 还是规划，没有包含在治涝工程中的具体工程
        // 根据不同工程措施类型，显示不同的颜色。
        color = isBuilding ? '#39ff0c' : isCurrent ? '#77C8F3' : 'red';
        if (type === "hd") {
            if (!isBuilding && !isFinished) {
                color = projectType === '清淤疏浚' ? '#0019CA' : projectType === '拓宽疏浚' ? '#EA0B9C' : color;
            }
            if (projectType === '新建' || projectType === '新开河道') {
                if (!isBuilding && !isFinished) color = '#EA0B9C';
                dashArray = '5,5';
                weight = 3;
            }
            return { color: color, dashArray: dashArray, weight: weight };
        } else {
            color = isBuilding ? 'green' : isCurrent ? "blue" : "red";
            iconName = sluiceType === "sz" ? "sz_" + color : "bs_" + color;
            return {
                icon: L.icon({
                    iconUrl: abp.appPath + 'Content/leaflet/images/' + iconName + '.png',
                    iconSize: (sluiceType === 'sz' ? [22, 12] : [36, 15])
                })
            };
        }
    };

    /*
     * 重置地图的宽，高
     */
    mapLoader.prototype.resizeMap = function () {
        //$(window).resize();
        $('#' + this.mapId).height('100%');
    };

    /*
     * 读取当前地图的类型，卫星图(img)或交通图(vec)
     */
    mapLoader.prototype.getMapType = function () {
        return this.mapType;
    },

    /*
     * 切换地图（卫星图或交通图）
     */
    mapLoader.prototype.switchMapType = function (mapType) {
        if (this.mapType === mapType)
            return;

        this.mapType = mapType;
        this.tianDiTuMap.switchMapType(mapType);
    },

    /*
     * 删除地图
     */
    mapLoader.prototype.destroyMap = function () {
        if (this.map)
            this.map.remove();
    };

    /*
     * 清除所有标绘图层上的形状。
     */
    mapLoader.prototype.clearShapes = function () {
        this.shapeLayer.clearLayers();
    };

    /*
     * 创建地图
     */
    mapLoader.prototype.createMap = function (opts, callback) {
        opts = opts || {};
        opts.center = opts.center || [30.33, 119.93];
        opts.zoom = opts.zoom || 12;
        opts.mapType = opts.mapType || 'img';
        opts.doubleClickZoom = true; //禁用双击放大功能

        this.showLegend = opts.showLegend || false;
        this.mapType = opts.mapType;

        if (this.shapeLayer) {
            this.shapeLayer.clearLayers();
            this.shapeLayer = null;
        }

        this.shapes = [];

        if (this.map) {
            this.map.remove();
        }

        this.tianDiTuMap = new L.tianDiTuMap(this.mapId, opts);
        this.map = this.tianDiTuMap.realMap;

        var xjLayer = new L.tileLayer.customArcGISServerLayer("http://114.215.170.1:8085/XJ/", { minZoom: 7, maxZoom: 16 });
        xjLayer.addTo(this.map);

        //绑定事件
        var events = opts.events || {};
        for (ev in events) {
            this.map.on(ev, events[ev]);
        }

        if ($.isFunction(callback)) {
            callback.call(null, this.map);
        }

        this.shapeLayer = new L.featureGroup();
        this.map.addLayer(this.shapeLayer);

        return this.map;
    };

    window.mapLoader = mapLoader;
})(jQuery, window);