function saveFeature(layer, type, waterLoggingId, projectSeqNo, projectId) {
    var isNew = type === 0, url, formData, rawData, drawFlag;
    if (layer.options) {
        drawFlag = layer.options.drawFlag;
        rawData = layer.rawData;
        if ($.isFunction(waterLoggingId)) {
            waterLoggingId = waterLoggingId.call(window);
        }

        if (drawFlag === "createWaterlogging") {
            //新增涝片范围
            abp.ajax({
                url: getUrl('/Water/SaveWaterloggingRange'),
                data: { id: waterLoggingId, geoJson: JSON.stringify(layer.toGeoJSON()) }
            }).done(function (data) {
                abp.notify.success('保存涝片范围成功。');
                isNew && layer.bindLabel(data);
            });
        } else if (drawFlag === "createRiver") {
            if (isNew) {
                url = getUrl('/Water/CreateRiver');
                formData = { waterLoggingId: waterLoggingId, geoJson: JSON.stringify(layer.toGeoJSON()) };
            } else {
                url = getUrl('/Water/UpdateRiverRange');
                formData = { riverCode: rawData.code, geoJson: JSON.stringify(layer.toGeoJSON()), projectSeqNo: projectSeqNo, projectId: projectId };
            }
            //新增河道
            abp.ajax({
                url: url,
                data: formData
            }).done(function (data) {
                abp.notify.success('保存河道成功。');
                if (isNew) {
                    layer.rawData = data;
                    openModal('编辑河道', getUrl('/Water/EditInfo', { code: data.code, type: 'hd' }));
                    layer.on('click', function (e) {
                        shapeClick({ name: data.name, code: data.code, type: "hd" });
                    });
                    layer.bindLabel(data.name);
                }
            });
        } else {
            if (layer.options.icon && layer.options.icon.options) {
                drawFlag = layer.options.icon.options.drawFlag;
                if (drawFlag) {
                    var sluiceType = drawFlag === "createSluice" ? "sz" : "bs";
                    if (isNew) {
                        url = getUrl('/Water/CreateSluice');
                        formData = { waterLoggingId: waterLoggingId, geoJson: JSON.stringify(layer.toGeoJSON()), sluiceType: sluiceType };
                    } else {
                        url = getUrl('/Water/UpdateSluiceRange');
                        formData = { sluiceCode: rawData.code, geoJson: JSON.stringify(layer.toGeoJSON()), projectSeqNo: projectSeqNo };
                    }
                    //新增水闸或闸站
                    abp.ajax({
                        url: url,
                        data: formData
                    }).done(function (data) {
                        abp.notify.success('保存闸成功。');
                        if (isNew) {
                            layer.rawData = data;
                            openModal('编辑水闸', getUrl('/Water/EditInfo', { code: data.code, type: 'sz' }));
                            layer.on('click', function (e) {
                                shapeClick({ name: data.name, code: data.code, type: "sz" });
                            });
                            layer.bindLabel(data.name);
                        }
                    });
                }
            }
        }
    }
}

function createFeature(layer, waterLoggingId) {
    //新增涝片范围或新增工程
    saveFeature(layer, 0, waterLoggingId);
}

function updateFeatures(layers, waterLoggingId, projectSeqNo, projectId) {
    layers.eachLayer(function (layer) {
        saveFeature(layer, 1, waterLoggingId, projectSeqNo, projectId);
    });
}

//添加标绘控件
function createDrawControl(mapLoader, showDrawWaterlogging) {
    var drawLayer = mapLoader.getShapeLayer(),
        map = mapLoader.getMap();

    var drawOptions = false;
    if (showDrawWaterlogging) {
        drawOptions = {
            polygon: {
                shapeOptions: {
                    drawFlag: 'createWaterlogging',
                    color: '#1d7caa'
                }
            },
            polyline: false,
            marker: false,
            rectangle: false,
            circle: false
        }
    }

    var drawControl = new L.Control.Draw({
        draw: drawOptions,
        edit: {
            featureGroup: drawLayer
        }
    });

    var customDraw = new L.Control.CustomDraw({
        actionContainerId: 'customActionBar',
        items: [
            {
                elId: 'linkNewRiverPolyline',
                featureType: 'polyline',
                featureOpts: {
                    shapeOptions: {
                        drawFlag: 'createRiver',
                        color: 'red'
                    }
                }
            },
            {
                elId: 'linkNewRiverPolygon',
                featureType: 'polygon',
                featureOpts: {
                    shapeOptions: {
                        drawFlag: 'createRiver',
                        color: 'red'
                    }
                }
            },
             {
                 elId: 'btnNewSluice',
                 featureType: 'marker',
                 featureOpts: {
                     icon: L.icon({
                         iconUrl: '/Content/leaflet/images/sz.png',
                         iconSize: [22, 12],
                         drawFlag: 'createSluice'
                     })
                 }
             },
              {
                  elId: 'btnNewSluiceSite',
                  featureType: 'marker',
                  featureOpts: {
                      icon: L.icon({
                          iconUrl: '/Content/leaflet/images/bs.png',
                          iconSize: [36, 15],
                          drawFlag: 'createSluiceSite'
                      })
                  }
              }
        ]
    });

    map.addControl(drawControl);
    map.addControl(customDraw);
}

function bindMap(opts) {
    opts = opts || {};
    var map = opts.map,
        waterLoggingId = opts.waterLoggingId,
        drawLayer = opts.drawLayer,
        loadCallback = opts.loadCallback,
        projectSeqNo = opts.projectSeqNo,
        projectId = opts.projectId;

    //工程，新增，编辑，删除操作
    map.on('draw:created', function (e) {
        var layer = e.layer;

        createFeature(layer, waterLoggingId);
        drawLayer.addLayer(layer);
    })
    .on('draw:edited', function (e) {
        updateFeatures(e.layers, waterLoggingId, projectSeqNo, projectId);
    })
    .on('draw:deletestart', function (e) {
        drawLayer.eachLayer(function (layer) {
            layer.off('click');
        });
    })
    .on('draw:deletestop', function (e) {
        drawLayer.clearLayers();

        loadCallback(false);
    })
    .on('draw:deleted', function (e) {
        e.layers.eachLayer(function (layer) {
            var drawFlag, code;
            if (layer.options) {
                drawFlag = layer.options.drawFlag;
                code = layer.rawData.code;
                if (drawFlag === "createRiver") {
                    abp.ajax({
                        url: getUrl('/Water/DeleteRiver'),
                        data: { code: code }
                    }).done(function (data) {
                        abp.notify.success('删除河道成功');
                    });
                } else {
                    if (layer.options.icon && layer.options.icon.options) {
                        drawFlag = layer.options.icon.options.drawFlag;
                        if (drawFlag === "createSluice" || drawFlag === "createSluiceSite") {
                            abp.ajax({
                                url: getUrl('/Water/DeleteSluice'),
                                data: { code: code }
                            }).done(function (data) {
                                abp.notify.success('删除水闸成功');
                            });
                        }
                    }
                }
            }
        });
    });
}

var flag = true;
var timer = null;
//选中
function selectFeature(layer) {

    //清除
    //clearFeature(layer);

    //选中
    flag = true;
    if (timer != null) clearInterval(timer);
    timer = setInterval("start('" + layer + "')", 600);
}

//清除
function clearFeature(layer) {
    if (layer != "") {
        var style = {
            color: "#e6000b",
            weight: 4
        };
      layer.setStyle(style);
    }
}

//定时执行
function start(layer) {

 
    var style = "";

    if (layer != null) {
        if (flag) {
            style = {
                icon: L.icon({ 'iconUrl': '/images/cun.png', iconSize: [22.6, 33.3] }),
                weight: 4
            };
            flag = false;
        } else {
            style = {
                icon: L.icon({ 'iconUrl': '/images/marker.png', iconSize: [22.6, 33.3] }),
                color: "#e6000b",
                weight: 4
            };
            flag = true;
        }
        layer.setStyle(style);
    }
}

