//地图操作条全局变量 mapBar
var mapBarClick = false;        //点击记录
var mapBarStop = true;          //动画效果状态
var layerTypeValue = "img";     //地图切换，默认卫星
var MapPanelShow = 0;           //弹出框
var layersList = "";            //弹出框内容

$(function () {
    $(".dituIcon")
        .hover(function () {
            if (mapBarClick) {
                $(this).css("background-position", "-150px 0px");
            } else {
                $(this).css("background-position", "-100px 0px");
            }
        }, function () {
            if (mapBarClick) {
                $(this).css("background-position", "-50px 0px");
            } else {
                $(this).css("background-position", "0px 0px");
            }
        })
        .click(function () {

            var $mapIcon = $(this);

            if (mapBarStop) {
                if (mapBarClick) {
                    mapBarStop = false;
                    mapBarClick = false;

                    //更换图标
                    $mapIcon.css("background-position", "-100px 0px");

                    //弹出框隐藏
                    if (MapPanelShow > 0) {
                        $(".MapPanel").hide().empty();
                        MapPanelShow = 0;
                    }

                    //收缩动画
                    $(".mapBarTdM").animate({
                        width: "8px"
                    }, 200, function () {
                        //背景隐藏
                        $("#mapBarTable").hide();

                        mapBarStop = true;
                    });
                } else {
                    mapBarStop = false;
                    mapBarClick = true;

                    //更换图标
                    $mapIcon.css("background-position", "-150px 0px");

                    //背景显示
                    $("#mapBarTable").show();

                    //展开动画
                    $(".mapBarTdM").animate({
                        width: "370px"
                    }, 500, function () {
                        mapBarStop = true;
                    });
                }
            }
        });

    var position;
    $(".marBarUL li").click(function () {
        var idname = $(this).attr("id");
        switch (idname) {
            case "mapPan":
                //漫游
                break;
            case "mapMeasureLine":
                //测距
                measure();
                break;
            case "mapMeasurePloy":
                //测面
                measureArea();
                break;
            case "mapClearLayerFeature":
                //清除
                clearMap();
                break;
            case "mapPrint":
                //打印
                break;
            case "mapSource":
                //地图切换
                if (MapPanelShow != 2) {
                    if (MapPanelShow > 0) $(".MapPanel").hide().empty();

                    MapPanelShow = 2;

                    /*------------------------------内容开始------------------------------*/

                    layersList = "";

                    layersList += "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";

                    layersList += "<tr><td class=\"tdtl\"></td><td class=\"tdtm\" colspan=\"2\">地图切换</td><td class=\"tdtr\"></td></tr>";
                    layersList += "<tr><td class=\"tdml\"></td><td colspan=\"2\" class=\"tdmm\">";
                    layersList += "<table class=\"maptable\">";
                    layersList += "<tbody>";

                    layersList += "<tr>";
                    layersList += "<td>卫星图</td>";
                    layersList += "<td><input class=\"LayerRad\" type=\"radio\" id=\"img\" name=\"mapType\"" + (layerTypeValue == "img" ? "checked" : "") + "/></td>";
                    layersList += "</tr>";

                    layersList += "<tr>";
                    layersList += "<td>交通图</td>";
                    layersList += "<td><input class=\"LayerRad\" type=\"radio\" id=\"vec\" name=\"mapType\"" + (layerTypeValue == "vec" ? "checked" : "") + "/></td>";
                    layersList += "</tr>";


                    layersList += "</tbody>";
                    layersList += "</table>";
                    layersList += "</td><td class=\"tdmr\"></td></tr>";
                    layersList += "<tr><td class=\"tdbl\"></td><td class=\"tdbm\"></td><td class=\"tdbm2\"></td><td class=\"tdbr\"></td></tr>";
                    layersList += "</table>";

                    $(".MapPanel").html(layersList);
                    $(".maptable tr").last().children().css("border-bottom", "0px");

                    /*------------------------------内容结束------------------------------*/

                    position = $(window).width() - $("#mapSource").offset().left - 30;

                    $(".MapPanel").css({ right: position, bottom: "75px", width: "150px", height: "1px" }).animate({ height: 30 * 2 + 52, opacity: 'show' }, 500);
                }

                break;
            case "mapLayerFeature":

                //图层控制
                if (MapPanelShow != 1) {
                    if (MapPanelShow > 0) $(".MapPanel").hide().empty();

                    MapPanelShow = 1;

                    /*------------------------------内容开始------------------------------*/

                    layersList = "";

                    layersList += "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">";
                    layersList += "<tr><td class=\"tdtl\"></td><td class=\"tdtm\" colspan=\"2\">图层控制</td><td class=\"tdtr\"></td></tr>";
                    layersList += "<tr><td class=\"tdml\"></td><td colspan=\"2\" class=\"tdmm\">";
                    layersList += "<table class=\"maptable\">";
                    layersList += "<tbody>";

                    //循环图层
                    var layerNum = 0;
                    //控制图层
                    var limitLayerNameArray = [{ layername: "河道", layer: _hdLayer }, { layername: "水闸", layer: _szLayer }];

                    for (var i = 0; i < limitLayerNameArray.length; i++) {
                        var theLayer = limitLayerNameArray[i].layer;
                        layersList += "<tr>";
                        layersList += "<td class='layername'>" + limitLayerNameArray[i].layername + "</td>";

                        var vsbStr = "checked";
                        layersList += "<td><input class='LayerSwitch' id='theLayer" + i + "' type='checkbox' " + vsbStr + "/></td>";
                        layersList += "</tr>";
                        layerNum++;
                    }

                    layersList += "</tbody>";
                    layersList += "</table>";
                    layersList += "</td><td class=\"tdmr\"></td></tr>";
                    layersList += "<tr><td class=\"tdbl\"></td><td class=\"tdbm\"></td><td class=\"tdbm2\"></td><td class=\"tdbr\"></td></tr>";
                    layersList += "</table>";

                    //显示图层
                    $(".MapPanel").html(layersList);
                    $(".maptable tr").last().children().css("border-bottom", "0px");

                    //加载开关样式，并添加点击事件
                    $(".LayerSwitch").wrap('<div class="switch switch-mini" data-on-label="显示" data-off-label="隐藏"/>').parent().bootstrapSwitch().on('switch-change', function (e, data) {
                        var inputId = $(this).find(".LayerSwitch").attr("id");
                        var kk = inputId.replace("theLayer", "");
                        var curLayer = limitLayerNameArray[kk].layername == '河道' ? _hdLayer : _szLayer;
                        if (data.value) {
                            if (!_map.hasLayer(curLayer)) 
                                _map.addLayer(curLayer);
                        } else {
                            if (_map.hasLayer(curLayer))
                                _map.removeLayer(curLayer);
                        }
                    });
                    /*------------------------------内容结束------------------------------*/

                    //判断位置
                    position = $(window).width() - $("#mapLayerFeature").offset().left - 30;

                    $(".MapPanel").css({ right: position, bottom: "75px", width: "220px", height: "1px" }).animate({ height: 30 * layerNum + 65 + layerNum, opacity: 'show' }, 500);
                }

                break;
        }
    });

    //地图切换
    $(document).on('click',".LayerRad", function () {
        var type = $(this).attr("id");
        layerTypeValue = type;
        mapTypeClick(type);
    });

    //鼠标移开浮动框，自动关闭
    $(".MapPanel").on("mouseleave", function () {
        CloseMapPanel();
    });
});

//关闭地图工具条浮动框
function CloseMapPanel() {
    if (MapPanelShow) {
        $(".MapPanel").animate({ height: 0, opacity: 'hide' }, 300,
            function() {
                $(".MapPanel").empty();
                MapPanelShow = false;
            });
    }
}