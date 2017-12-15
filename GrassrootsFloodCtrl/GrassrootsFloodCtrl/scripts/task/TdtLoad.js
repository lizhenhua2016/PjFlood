//这里是设置地图显示的类型
$("#map").height($(document).height()-60).width($(document).width());


var normalm = L.tileLayer.chinaProvider('TianDiTu.Normal.Map', {//普通地图
    maxZoom: 18,
    minZoom: 5
}),
    normala = L.tileLayer.chinaProvider('TianDiTu.Normal.Annotion', {
        maxZoom: 18,
        minZoom: 5
    }),
    imgm = L.tileLayer.chinaProvider('TianDiTu.Satellite.Map', {
        maxZoom: 18,
        minZoom: 5
    }),
    imga = L.tileLayer.chinaProvider('TianDiTu.Satellite.Annotion', {
        maxZoom: 18,
        minZoom: 5
    });

var normal = L.layerGroup([normalm, normala]), image = L.layerGroup([imgm, imga]);//合并起来显示
//这里是选择图层
var baseLayers = {
    "地图": normal,
    "影像": image,
}

var overlayLayers = {
}

var map = L.map("map", {
    center: [31.59, 120.29],
    zoom: 8,
    layers: [image],
    zoomControl: false
});

L.control.layers(baseLayers, overlayLayers).addTo(map);
L.control.zoom({
    zoomInTitle: '放大',
    zoomOutTitle: '缩小'
}).addTo(map);