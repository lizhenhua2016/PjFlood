/*************地图实列化**********/
//公用图层
var _map;

//工程图层
var _skLayer, _shd, _dzzhd, _weifang, _dwynyq, _wdst, _dfhtxgxf, _anzidian, _wzck, _apptown;
//危险区
var _cundotLayer;
//村
var _cunLayer, _cunOneDotLayser;
//参数
var gcType = "";

var map = {
	// 图层合集
	overLayerGroup: {
		apptown: { typeID: 3, color: '#da251c', iconName: 'apptown' },
		appvillage: { typeID: 4, color: '#ffc107', iconName: 'appvillage' },
		shd: { typeID: 10, color: '#da251c', iconName: 'shd' },
		dzzhd: { typeID: 11, color: '#112522', iconName: 'dzzhd' },
		weifang: { typeID: 12, color: '#0189ff', iconName: 'weifang' },
		dwynyq: { typeID: 15, color: '#ffc107', iconName: 'dwynyq' },
		wdst: { typeID: 2, color: '#e8ce7e', iconName: 'wdst' },
		dfhtxgxf: { typeID: 4, color: '#003b86', iconName: 'dfhtxgxf' },
		anzidian: { typeID: 17, color: '#2cab36', iconName: 'anzidian' },
		wzck: { typeID:16, color: '#00923f', iconName: 'wzck' },
	},
	init:function(){
		_map = L.map('mapID', {
			'center':[29.05, 119.38],
			'zoom': 8,
			'minZoom': 7,
			'maxZoom': 18,
			'zoomControl': false});
		_map.addLayer(map.initmapLayers(1));
		//L.control.zoom({ zoomInTitle: '放大', zoomOutTitle: '缩小' }).addTo(_map);
		//加载本地的wmts切片，其上有流域、河流等信息
		/*
		var localTileLayer = new L.tileLayer.customArcGISServerLayer("http://122.224.98.154:8056/assets/images/town/", { minZoom: 9, maxZoom: 16 });
		localTileLayer.id = "arcgistile";
		_map.addLayer(localTileLayer);
		*/
	   
		var localShengLayer = new L.tileLayer.customArcGISServerLayer("http://114.215.170.1:8085/XJ/", { minZoom: 7, maxZoom: 15 });
			localShengLayer.id = "arcgistileShen";
			_map.addLayer(localShengLayer);
		
		var editableLayers = new L.FeatureGroup();
		_map.addLayer(editableLayers);

		var MyCustomMarker = L.Icon.extend({
			options: {
				shadowUrl: null,
				iconAnchor: new L.Point(12, 12),
				iconSize: new L.Point(24, 24),
				iconUrl: 'link/to/image.png'
			}
		});

		var options = {
			position: 'topright',
			draw: {
				polyline: {
					shapeOptions: {
						color: '#f357a1',
						weight: 10
					}
				},
				polygon: {
					allowIntersection: false, // Restricts shapes to simple polygons
					drawError: {
						color: '#e1e100', // Color the shape will turn when intersects
						message: '<strong>Oh snap!<strong> you can\'t draw that!' // Message that will show when intersect
					},
					shapeOptions: {
						color: '#bada55'
					}
				},
				circle: false, // Turns off this drawing tool
				rectangle: {
					shapeOptions: {
						clickable: false
					}
				},
				marker: {
					//icon: new MyCustomMarker()
				}
			},
			edit: {
				featureGroup: editableLayers, //REQUIRED!!
				remove: false
			}
		};

		var drawControl = new L.Control.Draw(options);
		//map.addControl(drawControl);
		
		_map.on('draw:created', function (e) {
			var type = e.layerType,
				layer = e.layer;

			if (type === 'rectangle') {
				L.marker([30.31503, 119.42141], {
					icon: L.icon({
						iconUrl: "/Images/num.png",
						iconAnchor: new L.Point(12, 12),
						iconSize: new L.Point(24, 24)
					}),
					riseOnHover: true
				}).addTo(_map);
				//editableLayers.addLayer();
				//layer.bindPopup('A popup!');
			}

			//editableLayers.addLayer(layer);
		});
		//初始化图层
		map.initLayers();
		//定义图层事件
		map.loadMouse();
		//地图事件
		_map.on('click', function(e){
			//$(".searchArealist").empty();
			//$(".search-areas a").text("行政区划");
			//$(".searchArealist").hide();
		   // var xy = e.latlng.toString().replace("LatLng(", "").replace(")", "").split(',');
		   // $("#textFrameSearch").val("" + xy[1].trim() + "," + xy[0].trim() + "");
		});
		//移动事件
		_map.on("mousemove", function (e) {
		    var lat = e.latlng.lat;
		    var lng = e.latlng.lng;
		    $("#curX").html("经度：" + lng);
		    $("#curY").html("纬度：" + lat);
		});
		
		//缩放事件
		_map.on("zoomend",function(e){
			var _room=_map.getZoom();
			//$("#textFrameSearch").val(_room);
			if(_room  <= 11){
				map.hideLayer();
			}else{
				map.showLayer();
			}

			if (_room >= 14) {
				_cunLayer.eachLayer(function (layer) {
					layer.showLabel();
				});
			} else {
				_cunLayer.eachLayer(function (layer) {
					layer.hideLabel();
				});
			}

			//app聚类显示隐藏
			if (_room >= 12 && _room <= 13 && appAdcd != "") {//县级
				map.generateFeatureLayerByRegionFromApp(map.overLayerGroup['apptown'], appAdcd);
				map.overLayerGroup['apptown'].countyLayer.setVisible(true);
				map.overLayerGroup['appvillage'].townLayer.setVisible(false);
				//map.overLayerGroup['apptown'].iconLayer.setVisible(false);
			} else if (_room > 13 && _room <= 14 && appAdcd != "") {//镇级
				map.generateFeatureLayerByRegionFromApp(map.overLayerGroup['appvillage'], appAdcd);
				map.overLayerGroup['apptown'].countyLayer.setVisible(false);
				map.overLayerGroup['appvillage'].townLayer.setVisible(true);
			   // map.overLayerGroup['appvillage'].iconLayer.setVisible(false);
			} else {
				
			}
		});

		$.each(this.overLayerGroup, function (key, val) {
			val.cityLayer = L.featureGroup([]);
			val.countyLayer = L.featureGroup([]);
			val.townLayer = L.featureGroup([]);
			val.iconLayer = L.featureGroup([]);
			val.hideLayer = function () {
				this.cityLayer.clearLayers();
				this.countyLayer.clearLayers();
				this.townLayer.clearLayers();
				this.iconLayer.clearLayers();
			}
			_map.addLayer(val.cityLayer);
			_map.addLayer(val.countyLayer);
			_map.addLayer(val.townLayer);
			_map.addLayer(val.iconLayer);
        });
        //地图缩放事件
        map.zoomend();
	},
	showLayer:function(){
		_cunLayer.setVisible(true);
		_shd.setVisible(true);
		_dzzhd.setVisible(true);
		_weifang.setVisible(true);
		_dwynyq.setVisible(true);
		_wdst.setVisible(true);
		_dfhtxgxf.setVisible(true);
		_anzidian.setVisible(true);
		_wzck.setVisible(true);
		_cunOneDotLayser.setVisible(true);
	},
	hideLayer:function(){
		_cunLayer.setVisible(false);
		_shd.setVisible(false);
		_dzzhd.setVisible(false);
		_weifang.setVisible(false);
		_dwynyq.setVisible(false);
		_wdst.setVisible(false);
		_dfhtxgxf.setVisible(false);
		_anzidian.setVisible(false);
		_wzck.setVisible(false);
		_cunOneDotLayser.setVisible(false);
	},
	initLayers:function(){
		_cundotLayer = new L.featureGroup([]);
		_map.addLayer(_cundotLayer);
		
		_cunLayer = new L.featureGroup([]);
		_map.addLayer(_cunLayer);
		
		_shd = new L.featureGroup([]);
		_map.addLayer(_shd);
		
		_dzzhd = new L.featureGroup([]);
		_map.addLayer(_dzzhd);
		
		_weifang = new L.featureGroup([]);
		_map.addLayer(_weifang);
		
		_dwynyq = new L.featureGroup([]);
		_map.addLayer(_dwynyq);
		
		_wdst=new L.featureGroup([]);
		_map.addLayer(_wdst);
		
		_dfhtxgxf=new L.featureGroup([]);
		_map.addLayer(_dfhtxgxf);
		
		_anzidian=new L.featureGroup([]);
		_map.addLayer(_anzidian);
		
		_wzck=new L.featureGroup([]);
		_map.addLayer(_wzck);
		
		_cunOneDotLayser=new L.featureGroup([]);
		_map.addLayer(_cunOneDotLayser);
	},
	loadMouse:function(){
		//地质灾害点
		_dzzhd.on("click", function (e) {
			map.selectdot(e, "click");
		});
		_dzzhd.on("mouseover", function (e) {
			map.selectdot(e, "over");
		});
		_dzzhd.on("mouseout", function (e) {
			map.selectdot(e, "out");
		});
		//危房
		_weifang.on("click", function (e) {
			map.selectdot(e, "click");
		});
		_weifang.on("mouseover", function (e) {
			map.selectdot(e, "over");
		});
		_weifang.on("mouseout", function (e) {
			map.selectdot(e, "out");
		});
		//避灾场所
		_anzidian.on("click", function (e) {
			map.selectdot(e, "click");
		});
		_anzidian.on("mouseover", function (e) {
			map.selectdot(e, "over");
		});
		_anzidian.on("mouseout", function (e) {
			map.selectdot(e, "out");
		});
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
	Set_TuCeng_InMap:function(type){
		var datanow=$(".search-areas").attr("data-now");
		var urls="../content/jsdata/tuceng.json";
		switch(datanow){
			case "太平街道":
				urls="../content/jsdata/tucengtaiping.json";
			break;
			case "大溪镇":
				urls="../content/jsdata/tucengdaxi.json";
			break;
		}
		$.ajax({
			type: "GET",
			url: urls,
			dataType: "json",
			success: function (result) {
				if (result != "" && result != null) {
					webapps.DisplayFeatures(result[type]);
				}
			}
		})
	},

	// 加载聚类图标
	generateFeatureLayerByRegion: function (layerConfig) {
		var me = this;
		var scale = _map.getZoom();
		var tempLayerGroup, _bounds, _viewBounds;
		layerConfig.cityLayer.clearLayers();
		layerConfig.countyLayer.clearLayers();
		layerConfig.townLayer.clearLayers();
		layerConfig.iconLayer.clearLayers();
		if (scale < 9){
			scale = 'city';
			tempLayerGroup = layerConfig.cityLayer;
		} else if (scale >= 9 && scale <= 10) {
			scale = 'county';
			tempLayerGroup = layerConfig.countyLayer;
		} else if (scale > 10 && scale <= 12) {
			scale = 'town';
			tempLayerGroup = layerConfig.townLayer;
		} else {
			scale = 'icon';
			tempLayerGroup = layerConfig.iconLayer;
			_bounds = _map.getBounds();
			_viewBounds = "POLYGON((" + _bounds.getWest() + " " + _bounds.getSouth() + ","
				+ _bounds.getWest() + " " + _bounds.getNorth() + ","
				+ _bounds.getEast() + " " + _bounds.getNorth() + ","
				+ _bounds.getEast() + " " + _bounds.getSouth() + ","
				+ _bounds.getWest() + " " + _bounds.getSouth() + "))";
			me.loading();
        }
        alert(111);
		$.ajax({
			type: "post",
			url: "/Services/GetCunDot.ashx?type=6",
			data: { ADCD: "", scale: scale, ftype: layerConfig.typeID, setview: _viewBounds },
			datatype: "json",
			success: function (data) {
				var data = eval(data);
				if (null != data && data != "") {
					var html = "";
					$.each(data, function (i, item) {
						divClass = 'leaflet-marker-icon q-marker-cluster ';
						divHtml = '<div style="background-color:' + (layerConfig.color || '#2b76e9') + '"><span>' + item.count + '</span></div>';
						if (item.count == 1 || _viewBounds) {
							myIcon = L.divIcon({
								iconSize: [10, 10],
								iconAnchor: [10, 10],
								className: 'leaflet-divlabel',
								html: "<img src='/Content/ComprehensiveApp/images/" + (layerConfig.iconName || "weifang")
									+ ".png' style='width:18px;height:18px;' align='middle'/>"
							});
						} else if (item.count < 100) {
							divClass += ' q-marker-cluster-small cId_' + (i + 1);
						} else if (item.count < 1000) {
							divClass += ' q-marker-cluster-medium cId_' + (i + 1);
						} else {
							divClass += ' q-marker-cluster-large cId_' + (i + 1);
						}
						if (item.count != 1)
							myIcon = L.divIcon({ 'className': divClass, 'html': divHtml });
						_labelText = item.name; // + "-" + me._typeName;
						if (item.latitude != "" && item.longitude != "") {
							var pointFeature = L.marker([item.latitude, item.longitude], { icon: myIcon })
							.bindLabel(_labelText, { direction: 'right', noHide: true });
							pointFeature.options.pid = (i + 1);
							pointFeature.options.properties = item;
							// 绑定图上图标的点击事件
							pointFeature.on("click", function (e) {//点击聚类点，放到到详细信息
								if (scale == "icon") {
									var _sRecord = e.target.options.properties;
									map.CunRemoveList(_sRecord.ADCD, layerConfig.typeID);
								}
							   //
								//application.map.setView([e.target._latlng.lat, e.target._latlng.lng], 13);
							});
							tempLayerGroup.addLayer(pointFeature);
						}
					});
					me.bringIconToTop();
				}
				me.loadFinished();
			}
		});
	},
	CunRemoveList: function (adcd,tid) {
		$("#FrameTuCengInfo").show();
		$.ajax({
			type: "post",
			url: "/Services/GetCunDot.ashx?type=3",
			data: { ADCD: adcd,tid:tid },
			datatype: "json",
			success: function (data) {
				var data = eval(data);
				if (null != data && data != "") {
					$("#FrameTuCengInfo span.townname").html(data[0].xname + " <label data-adcd='"+data[0].zadcd+"' id='zhen_tc'>" + data[0].zname + "</label> <label data-adcd='" + adcd + "' id='cun_tc'>" + data[0].cname + "</label>");
					var html = "";
					$.each(data, function (i, item) {
						html += '<table><tbody><tr>';
						html += '<td width="100" rowspan="3" class="subtitle"><img src="/Content/ComprehensiveApp/images/wg_' + item.WangGeID + '.png" style="width:55px; height:37px;" onerror="this.src=\'\'"><br>' + item.WangGeName + '</td>';
						html += '<td class="rtext">户主： ' + item.InfluenceName + ' ' + item.InfluenceHandPhone + '</td>';
						html += '<td class="rtext">居住人数：' + item.InfluenceNum + '人</td>';
						html += '<td>&nbsp;</td>';
						html += '</tr>';
						html += '<tr>';
						html += ' <td class="rtext">转移责任人：' + item.RemoveName + ' ' + item.RemoveHandPhone + '</td>';
						html += '<td class="rtext">预警责任人：' + item.WarningName + ' ' + item.WarningHandPhone + '</td>';
						html += ' <td>&nbsp;</td>';
						html += '</tr>';
						html += '<tr>';
						html += '<td class="rtext">避灾场所：' + item.TransferSiteName + '</td>';
						html += '<td class="rtext">避灾场所管理员：' + item.TransferSiteManager + ' ' + item.TransferSiteManagerPhone + '</td>';
						html += '<td class="rtext">有无安全鉴定：' + item.SafetyIdentification + '</td>';
						html += '</tr>';
						html += '</tbody></table>';
					});
					$("#FrameTuCengInfo div.zrr-con").html(html);
					$("#cun_tc").click(function () {
						common.CunInfo($(this).attr("data-adcd"));
						$("#FrameTownInfo").remove("style");
					});
					$("#zhen_tc").click(function () {
						common.TownInfo($(this).attr("data-adcd"));
						$("#FrameTownInfo").attr("style", "z-index:1000");
					});
				}
			}
		});
	},
	//加载App统计聚合图标
	generateFeatureLayerByRegionFromApp: function (layerConfig, _adcd) {
		var me = this;
		var scale = _map.getZoom();
		var tempLayerGroup, _bounds, _viewBounds;
		layerConfig.countyLayer.clearLayers();
		layerConfig.townLayer.clearLayers();
		layerConfig.iconLayer.clearLayers();
		
		if (scale >= 12 && scale <= 13) {
			scale = 'county';
			tempLayerGroup = layerConfig.countyLayer;
		} else if (scale > 13 && scale <= 14) {
			scale = 'town';
			tempLayerGroup = layerConfig.townLayer;
		} else {
			
		}
		$.ajax({
			type: "post",
			url: "/api/CApp/GetAppStatics",
			data: { adcd: _adcd, scale: scale },
			dataType: "json",
			success: function (data) {
				if (null != data && data != "") {
					var rows = eval(data);
					var html = "";
					$.each(rows, function (i, item) {
						if (item.count > 0) {
							divClass = 'leaflet-marker-icon q-marker-cluster ';
							divHtml = '<div style="background-color:' + (layerConfig.color || '#2b76e9') + '"><span>' + item.count + '</span></div>';
							if (item.count < 100) {
								divClass += ' q-marker-cluster-small cId_' + (i + 1);
							} else if (item.count < 1000) {
								divClass += ' q-marker-cluster-medium cId_' + (i + 1);
							} else {
								divClass += ' q-marker-cluster-large cId_' + (i + 1);
							}
							//if (item.count != 1)
							myIcon = L.divIcon({ 'className': divClass, 'html': divHtml });
							_labelText = item.name; // + "-" + me._typeName;
							if (item.latitude != "" && item.longitude != "") {
								var pointFeature = L.marker([item.latitude, item.longitude], { icon: myIcon })
								.bindLabel(_labelText, { direction: 'right', noHide: true });
								pointFeature.options.pid = (i + 1);
								pointFeature.options.properties = item;
								// 绑定图上图标的点击事件
								pointFeature.on("click", function (e) {//点击聚类点，放到到详细信息
									if (scale == "town") {
										//村事件
										var _sRecord = e.target.options.properties;
										$("#FrameCunInfoApp").show();
										$("#FrameCunInfoApp").attr("data-lat", item.latitude);
										$("#FrameCunInfoApp").attr("data-lng", item.longitude);
										$("#FrameCunInfoApp").attr("data-name", item.name);
										common.CunPersonLiableList(_sRecord.adcd);
									}
								});
								tempLayerGroup.addLayer(pointFeature);
							}
						}
					});
					me.bringIconToTop();
				}
				me.loadFinished();
			}
		});
	},
	// 把图标层挪到地图上来，避免压盖现象
	bringIconToTop: function () {
		$(".leaflet-marker-pane div").each(function () {
			if (Number($(this).css("z-index")) < 0)
				$(this).css("z-index", "10");
		});
	},

	// 加载进度条
	loading: function () {
		layer.load(1, {
			shade: [0.5, '#ddd'] //0.1透明度的白色背景
		});
	},
    //进度条加在结束
	loadFinished: function () {
		layer.closeAll('loading');
    },
    zoomend: function () {
        _map.on("zoomend", function (e) {
            var _room = _map.getZoom();
            if (_room < 11) {
                $(".mapauth").children("p").first().show().siblings().hide();
            } else {
                $(".mapauth").children("p").last().show().siblings().hide();
            }
		});
    }
}
map.init();