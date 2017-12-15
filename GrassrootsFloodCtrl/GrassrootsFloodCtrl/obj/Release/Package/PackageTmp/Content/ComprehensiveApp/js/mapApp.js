/*************地图实列化**********/
//公用图层
var _mapApp;
//村
var _cunAppLayer; var _cunAppGJ; var _cunAppGJDot;
//参数
var gcType = "";

var mapApp = {
	// 图层合集
	overLayerGroup: {
		apptown: { typeID: 3, color: '#da251c', iconName: 'apptown' },
		appvillage: { typeID: 4, color: '#ffc107', iconName: 'appvillage' }
	},

	init: function () {
		_mapApp = L.map('mapAppID', {
			'center':[29.05, 119.38],
			'zoom': 8,
			'minZoom': 7,
			'maxZoom': 18,
			'zoomControl': false});

		//var loader = new window.mapLoader({
		//    mapId: 'mapAppID'
		//});
		//_mapApp = loader.createMap({ mapType: 'googlesatelite', crs: L.CRS.EPSG900913 });
		//L.control.mousePosition().addTo(_mapApp);

		_mapApp.addLayer(mapApp.initmapLayers(1));
		L.control.zoom({ zoomInTitle: '放大', zoomOutTitle: '缩小' }).addTo(_mapApp);
		//加载本地的wmts切片，其上有流域、河流等信息
   
		var localShengLayer = new L.tileLayer.customArcGISServerLayer("http://114.215.170.1:8085/XJ/", { minZoom: 7, maxZoom: 15 });
			localShengLayer.id = "arcgistileShen";
			_mapApp.addLayer(localShengLayer);
		
		//var editableLayers = new L.FeatureGroup();
		//_mapApp.addLayer(editableLayers);

		//var MyCustomMarker = L.Icon.extend({
		//    options: {
		//        shadowUrl: null,
		//        iconAnchor: new L.Point(12, 12),
		//        iconSize: new L.Point(24, 24),
		//        iconUrl: 'link/to/image.png'
		//    }
		//});

	   
		//初始化图层
		mapApp.initLayers();
		//定义图层事件
		mapApp.loadMouse();
		//地图事件
		//_mapApp.on('click', function (e) {
			//$(".searchArealist").empty();
			//$(".search-areas a").text("行政区划");
			//$(".searchArealist").hide();
		   // var xy = e.latlng.toString().replace("LatLng(", "").replace(")", "").split(',');
		   // $("#textFrameSearch").val("" + xy[1].trim() + "," + xy[0].trim() + "");
		//});

        
		//移动事件
		//_map.on("mousemove", function (e) {
		//    var lat = e.latlng.lat;
		//    var lng = e.latlng.lng;
		//    $("#curX").html("经度：" + lng);
		//    $("#curY").html("纬度：" + lat);
		//});
		
		//缩放事件
		//_mapApp.on("zoomend", function (e) {
			//var _room = _mapApp.getZoom();
			//if(_room  <= 11){
			//    mapApp.hideLayer();
			//}else{
			//    mapApp.showLayer();
			//}

			//if (_room >= 14) {
			//    _cunAppLayer.eachLayer(function (layer) {
			//        layer.showLabel();
			//    });
			//} else {
			//    _cunAppLayer.eachLayer(function (layer) {
			//        layer.hideLabel();
			//    });
			//}
		//});
       
	},
	showLayer:function(){
		_cunAppLayer.setVisible(true);
		_cunAppGJ.setVisible(true);
		_cunAppGJDot.setVisible(true);
	},
	hideLayer:function(){
		_cunAppLayer.setVisible(false);
		_cunAppGJ.setVisible(false);
		_cunAppGJDot.setVisible(false);
	},
	initLayers:function(){
		_cunAppLayer = new L.featureGroup([]);
		_mapApp.addLayer(_cunAppLayer);

		_cunAppGJ = new L.featureGroup([]);
		_mapApp.addLayer(_cunAppGJ);

		_cunAppGJDot = new L.featureGroup([]);
		_mapApp.addLayer(_cunAppGJDot);
	},
	loadMouse:function(){
		////地质灾害点
		//_dzzhd.on("click", function (e) {
		//    mapApp.selectdot(e, "click");
		//});
		//_dzzhd.on("mouseover", function (e) {
		//    mapApp.selectdot(e, "over");
		//});
		//_dzzhd.on("mouseout", function (e) {
		//    mapApp.selectdot(e, "out");
		//});
		////危房
		//_weifang.on("click", function (e) {
		//    mapApp.selectdot(e, "click");
		//});
		//_weifang.on("mouseover", function (e) {
		//    mapApp.selectdot(e, "over");
		//});
		//_weifang.on("mouseout", function (e) {
		//    mapApp.selectdot(e, "out");
		//});
		////避灾场所
		//_anzidian.on("click", function (e) {
		//    mapApp.selectdot(e, "click");
		//});
		//_anzidian.on("mouseover", function (e) {
		//    mapApp.selectdot(e, "over");
		//});
		//_anzidian.on("mouseout", function (e) {
		//    mapApp.selectdot(e, "out");
		//});
	},
	selectdot:function(e,type){
		//参数
		var pnt = e.latlng;

		var feature = e.layer;
		var id = feature.options.sm_id;
		var name = feature.options.sm_capital;
		var gcType = feature.options.sm_sid;

		pnt = pnt == undefined ? feature.getLatLng() : pnt;
		if (type == "click") {
			//webapps.openInfoWin(id, name, gcType, pnt);
			webapps.openWindow(name, gcType, pnt);
		}
		else if (type == "over") {}
		else if (type == "out") {}
	},
	initmapLayers:function(index){
		var lyrGroup = L.layerGroup();
		var baseMaps = [{
			title: "交通图",
			cls: "vecmap",
			thumbnailUrl: "images/basemap/vector.jpg",
			subdomains: [0, 1, 2, 3, 4, 5, 6],
			tileSize: 256,
			style: "",
			tilematrixSet: "c",
			layers: [[{
				title: "天地图底图",
				url: "http://t{s}.tianditu.com/vec_c/wmts",
				type: "Wmts",
				layer: 'vec',
				format: "tile",
				minZoom: 0,
				maxZoom: 8
			}, {
				title: "浙江省天地图底图",
				url: "http://t{s}.tianditu.com/vec_c/wmts",
				type: "Wmts",
				layer: 'vec',
				format: 'tile',
				minZoom: 8,
				maxZoom: 18
			}], [{
				title: "天地图中文注记",
				url: "http://t{s}.tianditu.com/cva_c/wmts",
				type: "Wmts",
				layer: 'cva',
				format: "tile",
				minZoom: 0,
				maxZoom: 8

			}, {
				title: "浙江省天地图中文注记",
				url: "http://t{s}.tianditu.com/cva_c/wmts",
				type: "Wmts",
				layer: 'cva',
				format: 'tile',
				minZoom: 8,
				maxZoom: 18
			}]]
		}, {
			title: "影像图",
			cls: "imgmap",
			thumbnailUrl: "images/basemap/imagery.jpg",
			tileSize: 256,
			style: "",
			subdomains: [0, 1, 2, 3, 4, 5, 6],
			tilematrixSet: "c",
			layers: [[{
				title: "天地图底图",
				url: "http://t{s}.tianditu.com/img_c/wmts",
				type: "Wmts",
				layer: 'img',
				format: 'tile',
				minZoom: 0,
				maxZoom: 8
			}, {
				title: "浙江省天地图底图",
				url: "http://t{s}.tianditu.com/img_c/wmts",
				type: "Wmts",
				layer: 'img',
				format: 'tile',
				minZoom: 8,
				maxZoom: 18
			}], [{
				title: "天地图中文注记",
				url: "http://t{s}.tianditu.com/cia_c/wmts",
				type: "Wmts",
				layer: 'cia',
				format: 'tile',
				minZoom: 0,
				maxZoom: 8
			}, {
				title: "浙江省天地图中文注记",
				url: "http://t{s}.tianditu.com/cia_c/wmts",
				type: "Wmts",
				layer: 'cia',
				format: 'tile',
				minZoom: 8,
				maxZoom: 18
			}]]
		}, {
			title: "地形图",
			cls: "topmap",
			thumbnailUrl: "images/basemap/imagery.jpg",
			tileSize: 256,
			style: "",
			subdomains: [0, 1, 2, 3, 4, 5, 6],
			tilematrixSet: "c",
			layers: [[{
				title: "天地图底图",
				url: "http://t{s}.tianditu.com/ter_c/wmts",
				type: "Wmts",
				layer: 'ter',
				format: 'tile',
				minZoom: 0,
				maxZoom: 14
			}], [{
				title: "天地图中文注记",
				url: "http://t{s}.tianditu.com/cta_c/wmts",
				type: "Wmts",
				layer: 'cta',
				format: 'tile',
				minZoom: 0,
				maxZoom: 14
			}]]
		}];
		var layersInfo = baseMaps[index];
		for (i = 0; i < 2; i++) { //i=0时是地图，i=1时是注记
			templayer = new L.tileLayer.tdtTileLayer(null, {
				subdomains: layersInfo.subdomains,
				tileSize: layersInfo.tileSize,
				style: layersInfo.style,
				tilematrixSet: layersInfo.tilematrixSet,
				lyrAttribute: layersInfo.layers[i]
			});
			lyrGroup.addLayer(templayer);
		}
		return lyrGroup;
	},
 
	// 把图标层挪到地图上来，避免压盖现象
	bringIconToTop: function () {
		$(".leaflet-marker-pane div").each(function () {
			if (Number($(this).css("z-index")) < 0)
				$(this).css("z-index", "10");
		});
	},

	//// 加载进度条
	//loading: function () {
	//    layer.load(1, {
	//        shade: [0.5, '#ddd'] //0.1透明度的白色背景
	//    });
	//},

	//loadFinished: function () {
	//    layer.closeAll('loading');
	//},
}
mapApp.init();