/* =============================
* Map utilbar v1.0.0
* =============================
* Author: 
*==============================*/

L.Control.UtilBar = L.Control.extend({
    options: {
        position: 'bottomright',
        line: true,
        polygon: true,
        clear: true,
        mapSwitch: true,
        layerSwitch: false
    },

    initialize: function (options) {
        options = options || {};
        L.Control.prototype.initialize.call(this, options);
        //other init

        if (!options.mapLoader)
            throw new Error('必须提供 mapLoader 参数');

        if (!options.jQuery)
            throw new Error('请提供 jQuery 对象');

        this.mapLoader = options.mapLoader;
        this.$ = options.jQuery;

        L.setOptions(this, options);
    },

    onAdd: function (map) {
        this.map = map;
        this.drawLayers = new L.featureGroup();
        this.map.addLayer(this.drawLayers);

        //根据设置添加按钮。
        var topContainer = L.DomUtil.create('div', 'utilbar-container'),
            barContainer = L.DomUtil.create('ul', 'utilbar-bar-container'),
            iconContainer = L.DomUtil.create('div', 'utilbar-icon-container'),
            itemPanel = L.DomUtil.create('div', 'utilbar-item-panel');

        var barKeys = ['line', 'polygon', 'clear', 'mapSwitch', 'layerSwitch'],
            titles = ['测距', '测面', '清除', '地图切换', '图层控制'];
        var barItem = L.DomUtil.create('li', 'utilbar-bar-left');
        barContainer.appendChild(barItem);

        for (var i = 0; i < barKeys.length; i++) {
            if (this.options[barKeys[i]]) {
                barItem = L.DomUtil.create('li', ('utilbar-bar-' + barKeys[i]));
                barItem.title = titles[i];
                L.DomEvent.on(barItem, 'click', this['_action_' + barKeys[i]], this);
                barContainer.appendChild(barItem);
            }
        }

        barItem = L.DomUtil.create('li', 'utilbar-bar-right-gap');
        barContainer.appendChild(barItem);

        barItem = L.DomUtil.create('li', 'utilbar-bar-right');
        barContainer.appendChild(barItem);

        barContainer.style.display = 'none'; //don't show at first

        topContainer.appendChild(barContainer);

        var iconLink = L.DomUtil.create('a', 'utilbar-icon-link');
        iconContainer.appendChild(iconLink);
        L.DomEvent.on(iconLink, 'click', this._barExpand, this);

        topContainer.appendChild(iconContainer);
        topContainer.appendChild(itemPanel);

        this.barContainer = barContainer;
        this.iconContainer = iconContainer;
        this.itemPanel = itemPanel;

        return topContainer;
    },

    onRemove: function () {
        //todo
    },

    _barExpand: function () {
        this.itemPanel.style.display = 'none';
        var isOpen = L.DomUtil.hasClass(this.iconContainer, 'open');
        if (isOpen) {
            this.barContainer.style.display = 'none';
            L.DomUtil.removeClass(this.iconContainer, 'open');
        } else {
            this.barContainer.style.display = 'block';
            L.DomUtil.addClass(this.iconContainer, 'open');
        }
    },

    //测距
    _action_line: function () {
        var shapeOptions = {
            color: '#f06eaa',
            weight: 2,
            opacity: 0.9
        };

        var drawShape = new L.Polyline([], shapeOptions);
        this._action_shape(drawShape, shapeOptions, 'polyline');
    },

    //测面
    _action_polygon: function () {
        var shapeOptions = {
            color: '#f06eaa',
            weight: 2,
            opacity: 0.9
        };

        var drawShape = new L.Polygon([], shapeOptions);
        this._action_shape(drawShape, shapeOptions, 'polygon');
    },

    _action_shape: function (drawShape, shapeOptions, shapeType) {
        var measureResult = 0,
            drawing = false,
            drawPoints = [],
            measureTooltip,
            drawMoveShape,
            that = this,
            map = this.map;

        this.drawLayers.addLayer(drawShape);
        measureTooltip = new L.Tooltip(this.map);

        map.getContainer().style.cursor = 'crosshair';

        function lineMousedown(e) {
            drawing = true;

            drawPoints.push(e.latlng);
            if (shapeType === 'polyline' && drawPoints.length > 1) {
                measureResult += e.latlng.distanceTo(drawPoints[drawPoints.length - 2]);
            }
            drawShape.addLatLng(e.latlng);
        }

        function lineMouseMove(e) {

            if (drawing) {
                if (drawMoveShape != undefined && drawMoveShape != null) {
                    that.drawLayers.removeLayer(drawMoveShape);
                }
                var prevPoint = drawPoints[drawPoints.length - 1],
                    firstPoint = drawPoints[0],
                    distance;
                if (shapeType === 'polygon') {
                    drawMoveShape = new L.Polygon([firstPoint, prevPoint, e.latlng], shapeOptions);
                    distance = that._calcArea(drawPoints);
                    measureTooltip.updateContent({
                        text: '单击确定点，双击结束！',
                        subtext: "总面积：" + (distance / 1000000).toFixed(3) + "平方公里"
                    });
                } else {
                    drawMoveShape = new L.Polyline([prevPoint, e.latlng], shapeOptions);

                    distance = measureResult + e.latlng.distanceTo(drawPoints[drawPoints.length - 1]);
                    measureTooltip.updateContent({
                        text: '单击确定点，双击结束！',
                        subtext: "总长：" + (distance / 1000).toFixed(2) + "公里"
                    });
                }
                that.drawLayers.addLayer(drawMoveShape);
                measureTooltip.updatePosition(e.latlng);
            }
        }

        function lineDbClick(e) {
            alert();
            var text = '', area = 0;
            map.getContainer().style.cursor = '';

            if (drawing) {
                if (drawMoveShape != undefined && drawMoveShape != null) {
                    that.drawLayers.removeLayer(drawMoveShape);
                    drawMoveShape = null;
                }

                if (drawPoints.length > 1) {

                    if (shapeType === 'polyline') {
                        measureResult += e.latlng.distanceTo(drawPoints[drawPoints.length - 2]);
                        text = "总长：" + (measureResult / 1000).toFixed(2) + "公里";
                    } else {
                        area = that._calcArea(drawPoints);
                        text = "总面积：" + (area / 1000000).toFixed(3) + "平方公里";
                    }
                    var distanceLabel = L.marker(drawPoints[drawPoints.length - 1], {
                        icon: new L.divIcon({
                            className: 'DistanceLabelStyle',
                            iconAnchor: [-8, 15],
                            html: "<span class='bubbleLabel'><span class='bubbleLabel-bot bubbleLabel-bot-left'></span><span class='bubbleLabel-top bubbleLabel-top-left'></span><span>" + text + "</span></span>"
                        }),
                    });

                    that.drawLayers.addLayer(distanceLabel);
                }

                //移除提示框
                if (measureTooltip) {
                    measureTooltip.dispose();
                }

                drawPoints = [];
                drawing = false;
                map.off('mousedown', lineMousedown);
                map.off('mousemove', lineMouseMove);
                map.off('dblclick', lineDbClick);
            }
        }

        map.on('mousedown', lineMousedown);
        map.on('mousemove', lineMouseMove);
        map.on('dblclick', lineDbClick);
    },

    _action_clear: function () {
        this.drawLayers.clearLayers();
    },

    _action_mapSwitch: function (e) {
        var html = [],
            mapType = this.mapLoader.getMapType(),
            mapLoader = this.mapLoader,
            $ = this.$;

        html.push("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
        html.push("<tr><td class='tdtl'></td><td class='tdtm' colspan='2'>地图切换</td><td class='tdtr'></td></tr>");
        html.push("<tr><td class='tdml'></td><td colspan='2' class='tdmm'>");
        html.push("<table class='maptable'>");
        html.push("<tbody>");

        html.push("<tr>");
        html.push("<td>卫星图</td>");
        html.push("<td><input class='LayerRad' type='radio' mapType='img' name='mapType'" + (mapType == "img" ? "checked" : "") + "/></td>");
        html.push("</tr>");

        html.push("<tr>");
        html.push("<td>交通图</td>");
        html.push("<td><input class='LayerRad' type='radio' mapType='vec' name='mapType'" + (mapType == "vec" ? "checked" : "") + "/></td>");
        html.push("</tr>");


        html.push("</tbody>");
        html.push("</table>");
        html.push("</td><td class='tdmr'></td></tr>");
        html.push("<tr><td class='tdbl'></td><td class='tdbm'></td><td class='tdbm2'></td><td class='tdbr'></td></tr>");
        html.push("</table>");
        html.push("<div class='itemPanelClose'>&times;</div>")

        //this.itemPanel.innerHTML = html.join('');
        $('.utilbar-item-panel').html(html.join('')).css({ right: 92 }).slideDown();

        $(document).off('click', '.utilbar-item-panel input[name="mapType"]')
            .on('click', '.utilbar-item-panel input[name="mapType"]', function (e) {
                var mapType = $(this).attr('mapType');
                mapLoader.switchMapType(mapType);
            });

        $(document).off('click', '.utilbar-item-panel .itemPanelClose')
            .on('click', '.utilbar-item-panel .itemPanelClose', function () {
                $('.utilbar-item-panel').slideUp();
            });
    },

    _action_layerSwitch: function (e) {
        var html = [],
            item,
            checked = false,
            mapLoader = this.mapLoader,
            $ = this.$,
            that = this;

        html.push("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
        html.push("<tr><td class='tdtl'></td><td class='tdtm' colspan='2'>图层控制</td><td class='tdtr'></td></tr>");
        html.push("<tr><td class='tdml'></td><td colspan='2' class='tdmm'>");
        html.push("<table class='maptable'>");
        html.push("<tbody>");

        for (title in this.options.layerSwitch) {
            item = this.options.layerSwitch[title];
            hide = item.hide;

            html.push('<tr>');
            html.push('<td class="layername">' + title + '</td>');
            html.push('<td>');
            html.push(' <div class="switch switch-mini" data-on-label="显示" data-off-label="隐藏">');
            html.push('  <input class="LayerSwitch" layerName="' + title + '" type="checkbox" ' + (hide ? '' : ' checked') + ' />');
            html.push(' </div>');
            html.push('</td>');
            html.push('</tr>');
        }

        html.push("</tbody>");
        html.push("</table>");
        html.push("</td><td class='tdmr'></td></tr>");
        html.push("<tr><td class='tdbl'></td><td class='tdbm'></td><td class='tdbm2'></td><td class='tdbr'></td></tr>");
        html.push("</table>");
        html.push("<div class='itemPanelClose'>&times;</div>")

        $('.utilbar-item-panel').html(html.join('')).css({ right: 52 }).slideDown();

        //加载开关样式，并添加点击事件
        $(".utilbar-item-panel .switch").bootstrapSwitch().on('switch-change', function (e, data) {
            var name = $(this).find('input').attr('layerName'),
                shapeType = that.options.layerSwitch[name].shapeType,
                mapFunc = that.options.layerSwitch[name].mapFunc;
            that.options.layerSwitch[name].hide = !data.value;
            if (typeof (mapFunc) === 'function') {
                mapFunc.call(null, [name, data.value, mapLoader.shapeLayer]);
            } else {
                mapLoader.mapShapesByParam('type', shapeType, function (layer) {
                    data.value ?
                        mapLoader.getMap().addLayer(layer) :
                        mapLoader.getMap().removeLayer(layer);
                });
            }
        });

        $(document).off('click', '.utilbar-item-panel .itemPanelClose')
            .on('click', '.utilbar-item-panel .itemPanelClose', function () {
                $('.utilbar-item-panel').slideUp();
            });
    },

    //面积计算
    _calcArea: function (latLngs) {
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
});