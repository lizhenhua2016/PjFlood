/*
 * 类型于 leaflet.draw 的可自定义的Draw控件，支持把画线，画面等功能绑定到自定义的html元素中。
 * 该控件信赖于 leafet.draw，所以必须先加载该控件。
 * 
 *  @param(items) 提供一个数组，每一项代表一个标绘项。每个标绘项可包含3个参数，分别是：
 *      elId: (string) 表示待绑定的html控件的id。（必须）
 *      featureType: (string) 表示具体的形状类型，可支持： polyline, polygon, marker, rectangle, circle五种。（必须）
 *      featureOpts: (object) 表示具体形状的参数。参数的格式同 leaflet.draw 里针对每个类型的参数。详见 @see(https://github.com/Leaflet/Leaflet.draw)
 *  @param(actionContainerId) 一个字符串。表示标绘时的操作区的父容器id。操作区一般包含‘删除最一个点’和‘取消’两项。目前样式信赖于bootstrap 3.3.5。
 *  @example
 *      var customDraw = new L.Control.CustomDraw({
 *			items: [
 *				{
 *					elId: 'btnPolygon',
 *					featureType: 'polygon',
 *					featureOpts:{
 *						metric: false,
 *						shapeOptions:{
 *							color: '#00ff99'
 *						}
 *					}
 *				},
 *				{
 *					elId: 'btnLine',
 *					featureType: 'polyline'
 *				}
 *			],
 *			actionContainerId: 'actionContainer'
 *		});
 *
 *		map.addControl(customDraw);
 * 
 *  @version 1.0
 *  @author junwei.hu 2015
 */
L.Control.CustomDraw = L.Control.extend({
    options: {
        items: [],
        actionContainerId: ''
    },

    initialize: function (options) {
        L.Control.prototype.initialize.call(this, options);

        this.options.items = this.options.items || [];
        this._features = {};
        this._activeFeature = null;
    },

    enabled: function () {
        return this._activeFeature !== null;
    },

    disable: function () {
        if (!this.enabled()) {
            return;
        }

        this._activeFeature.disable();
    },

    onAdd: function (map) {
        var item, el, feature, typeId;
        for (var i = 0; i < this.options.items.length; i++) {
            item = this.options.items[i];
            el = L.DomUtil.get(item.elId);
            if (!el) continue;

            feature = this._getFeature(map, item);
            if (feature) {
                feature.type = feature.type + item.elId; //重设形状的type,防止有多个同类型的形状时冲突。
                this._features[feature.type] = feature;

                L.DomEvent.on(el, 'click', feature.enable, feature);

                feature.on('enabled', this._handlerActivated, this)
                  .on('disabled', this._handlerDeactivated, this);
            }
        }

        return L.DomUtil.create('div');
    },

    onRemove: function () {
        //do nothing
    },

    setDrawingOptions: function (options) { },

    _handlerActivated: function (e) {
        this.disable();

        this._activeFeature = this._features[e.handler];

        this._showActionToolbar();
    },

    _handlerDeactivated: function () {
        this._hideActionToolbar();

        this._activeFeature = null;
    },

    _showActionToolbar: function () {
        var container = L.DomUtil.get(this.options.actionContainerId);
        if (!container)
            return;

        container.style.display = 'block';

        //remove all old actions
        while (container.firstChild) {
            container.removeChild(container.firstChild);
        }

        var handler = this._activeFeature;
        var actions = this._getActions(handler);
        var ul = L.DomUtil.create('ul', 'list-inline', container),
          li, link;

        for (var i = 0; i < actions.length; i++) {
            if ('enabled' in actions[i] && !actions[i].enabled)
                continue;

            li = L.DomUtil.create('li', '', ul);
            link = L.DomUtil.create('a', '', li);

            L.DomUtil.addClass(link, 'bg-info');
            link.href = '#';

            if (actions[i].text)
                link.innerHTML = actions[i].text;

            if (actions[i].title)
                link.title = actions[i].title;

            L.DomEvent
              .on(link, 'click', L.DomEvent.stopPropagation)
              .on(link, 'mousedown', L.DomEvent.stopPropagation)
              .on(link, 'dbclick', L.DomEvent.stopPropagation)
              .on(link, 'click', L.DomEvent.preventDefault)
              .on(link, 'click', actions[i].callback, actions[i].context);
        }
    },

    _hideActionToolbar: function () {
        var container = L.DomUtil.get(this.options.actionContainerId);
        if (container) {
            container.style.display = 'none';
        }
    },

    _getFeature: function (map, item) {
        var featureOpts = item.featureOpts || {},
          featureType = item.featureType;

        if (featureType === "polyline") {
            return new L.Draw.Polyline(map, featureOpts);
        } else if (featureType === "polygon") {
            return new L.Draw.Polygon(map, featureOpts);
        } else if (featureType === "marker") {
            return new L.Draw.Marker(map, featureOpts);
        } else if (featureType === "circle") {
            return new L.Draw.Circle(map, featureOpts);
        } else if (featureType === "rectangle") {
            return new L.Draw.Rectangle(map, featureOpts);
        }
        return null;
    },

    _getActions: function (handler) {
        return [{
            enabled: handler.deleteLastVertex,
            title: L.drawLocal.draw.toolbar.undo.title,
            text: L.drawLocal.draw.toolbar.undo.text,
            callback: handler.deleteLastVertex,
            context: handler
        }, {
            title: L.drawLocal.draw.toolbar.actions.title,
            text: L.drawLocal.draw.toolbar.actions.text,
            callback: this.disable,
            context: this 
        }];
    }
});
