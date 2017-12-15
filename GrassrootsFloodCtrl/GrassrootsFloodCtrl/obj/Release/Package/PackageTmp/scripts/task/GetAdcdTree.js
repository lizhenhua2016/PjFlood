$(function () {
    $(".province").show();
    $(".province").html("<ul><li dataname='浙江省'><b>浙江省</b></li></ul>");
    $(".province ul li").click(function () {
        GetAdcdTreeprovince(".province", "330000000000000", 0, "浙江省", 1);
    });
    $("#warnlist>ul>li").on("click", function () {
        alert("abc");
        //alert($(this).attr("data-eventid"));
    });
});

/**
 * 整体显示
 * @param {any} doucmentname
 * @param {any} adcd
 * @param {any} level
 * @param {any} dataname
 * @param {any} type
 */
function GetAdcdTreeprovince(doucmentname, adcd, level, dataname, type) {
    //alert(level);

    //这里是等级

    // 这里是当在县级时显示事件

    // 这里是获取层级显示

    if (type > 1) {
        level = Number(level) - 1;
    }
    else {
        if (level == 2) {
            $("#warnlist").show();
            $.ajax({
                type: "get",
                url: "/api/AppWarnEvent/GetWarnList",
                dataType: "json",
                data: { "adcd": adcd },
                success: function (data) {
                    var sb = new StringBuilder();
                    var haha = eval(data);
                    var hahaha = eval(haha.warnList);
                    sb.append("<ul>");

                    $.each(hahaha, function (i, item) {
                        sb.append("<li  data-userPhone='" + item.userPhone + "' data-eventid='" + item.id + "'>" + item.eventName + "</li>")
                    });

                    sb.append("</ul>");
                    $("#warnlist").append(sb.toString());

                    $("#warnlist").children("ul").children("li").on("click", function () {
                        //alert($(this).attr("data-eventid"));
                        GetGpsByVillageAdcd($(this).attr("data-eventid"), adcd);
                    });
                }
            });
        }
        if (level > 3) {
            return false;
        }
        level = Number(level) + 1;
    }

    //获取顶部
    GetAdcdonReturn(adcd, level, type);
    //获取下一级的全部的
    GetAdcdNextAll(doucmentname, level, adcd, type);
}

/**
 * 这里是获取地图展示
 * @param {any} lat
 * @param {any} lng
 * @param {any} level
 */
function GetMapPoint(adcd, lat, lng, level) {
    var png;
    //alert(level);
    var zoom;
    switch (level) {
        case 1:
            zoom = 10;
            png = "/Content/ComprehensiveApp/Images/city.png";
            var mappoint = L.marker(L.latLng(lat, lng), {
                icon: L.icon({
                    iconUrl: png,
                    iconAnchor: new L.Point(9, 9)
                })
            });
            map.addLayer(mappoint);//map是在load里面创建好的
            map.setView([lat, lng], zoom);//这里是设置视野
            break;
        case 2:
            zoom = 12;
            png = "/Content/ComprehensiveApp/Images/county.png";
            var mappoint = L.marker(L.latLng(lat, lng), {
                icon: L.icon({
                    iconUrl: png,
                    iconAnchor: new L.Point(9, 9)
                })
            });
            map.addLayer(mappoint);//map是在load里面创建好的
            map.setView([lat, lng], zoom);//这里是设置视野
            break;
        case 3:
            zoom = 14;
            $.ajax({
                type: "post",
                url: "/api/ZZTX/GetAdcdByUseradcd",
                dataType: "json",
                data: { "UserAdcd": adcd, "actiontype": 1 },
                success: function (data) {
                    var haha = eval(data);
                    $.each(haha, function (i, item) {
                        var mappoint2 = L.marker(L.latLng(item.lat, item.lng), {
                            icon: L.icon({
                                iconUrl: "/Images/cun.png",
                                iconAnchor: new L.Point(9, 9)
                            })
                        });
                        map.addLayer(mappoint2);//map是在load里面创建好的
                    });
                }
            });
            png = "/Content/ComprehensiveApp/Images/town.png";
            var mappoint = L.marker(L.latLng(lat, lng), {
                icon: L.icon({
                    iconUrl: png,
                    iconAnchor: new L.Point(9, 9)
                })
            });
            map.addLayer(mappoint);//map是在load里面创建好的
            map.setView([lat, lng], zoom);//这里是设置视野
            break;
        case 4:
            zoom = 16;
            png = "/Images/cun.png";
            var mappoint = L.marker(L.latLng(lat, lng), {
                icon: L.icon({
                    iconUrl: png,
                    iconAnchor: new L.Point(9, 9)
                })
            });
            map.addLayer(mappoint);//map是在load里面创建好的
            map.setView([lat, lng], zoom);//这里是设置视野
            break;
        default:
            png = "/Images/cun.png";
            var mappoint = L.marker(L.latLng(lat, lng), {
                icon: L.icon({
                    iconUrl: png,
                    iconAnchor: new L.Point(9, 9)
                })
            });
            map.addLayer(mappoint);//map是在load里面创建好的
            map.setView([lat, lng], zoom);//这里是设置视野
            zoom = 10;
    }

    //layer.alert(zoom);
}
/**/
function GetWarnList(adcd) {
    $.ajax({
        type: "post",
        url: "/ZZTX/GetAdcdByUseradcd",
        dataType: "json",
        data: { "UserAdcd": adcd },
        success: function (data) {
            var haha = eval(data);
            $.each(haha, function (i, item) {
            });
        }
    });
}

/**
 * 根据adcd获取信息
 * @param {any} adcd
 */
function GetAdcdonReturn(adcd, level, type) {
    $(".province").empty();
    var sb1 = new StringBuilder();
    $.ajax({
        type: "post",
        url: "/api/ZZTX/AppGetAdcdInfoByAdcd",
        dataType: "json",
        data: { "UserAdcd": adcd, "ActionType": type },
        success: function (data) {
            $.each(data, function (i, item) {
                sb1.append("<p>当前位置:<span class='currentpostion' data-level='" + level + "'  data-name='" + item.adnm + "' data-adcd='" + adcd + "'>" + item.adnm + "</span><span class='pull-right returnback'>返回</span></p>");
            });

            $(".province").append(sb1.toString()+"<div class='clear'></div>");
        }
    });
}
/**
 * 获取下一级的全部的信息
 * @param {any} adcd
 * @param {any} type
 */
function GetAdcdNextAll(doucmentname, level, adcd, type) {
    $.ajax({
        type: "post",
        url: "/api/ZZTX/GetAdcdByUseradcd",
        dataType: "json",
        data: { "UserAdcd": adcd, "actiontype": type },
        success: function (data) {
            var sb = new StringBuilder();
            var haha = eval(data);
            sb.append("<ul>");
            $.each(haha, function (i, item) {
                sb.append("<li dataid='" + item.id + "' dataname='" + item.adnm + "' datalat='" + item.lat + "' datalng='" + item.lng + "' adcd='" + item.adcd + "'>" + item.adnm + "</li>");
            });
            sb.append("</ul>");
            $(doucmentname).append(sb.toString()).show();
            //点击li触发事件获取下一级
            $(doucmentname).children("ul").children("li").on("click", function () {
                GetMapPoint($(this).attr("adcd"), $(this).attr("datalat"), $(this).attr("datalng"), level);//地图展示
                GetAdcdTreeprovince(doucmentname, $(this).attr("adcd"), level, $(this).attr("dataname"), 1);//根据adcd获取下一级
            });
            /*
            * 返回按钮
            *     */
            $(".returnback").on("click", function () {
                GetAdcdTreeprovince(doucmentname, $(".currentpostion").attr("data-adcd"), $(".currentpostion").attr("data-level"), $(".currentpostion").attr("data-name"), 2);
            });
        }
    });
}

function GetGpsByVillageAdcd(warnid, adcd) {
    $.ajax({
        type: "post",
        url: "/api/AppApi/GetGpsList",
        dataType: "json",
        data: { "AppWarnEventId": warnid },
        success: function (data) {
            var haha = eval(data);
            var hahaha = eval(haha.rows)
            $.each(hahaha, function (i, item) {
                GetPeapoleGps(item.userRealName, item.userName, item.lat, item.lng);
            });
        }
    });
}

function GetPeapoleGps(name, mobile, lat, lng) {
    //alert(lat);

    png = "http://ovsqnadmr.bkt.clouddn.com/p1.png";
    var gpspoint = L.marker(L.latLng(lat, lng), { icon: L.icon({ iconUrl: png, iconAnchor: new L.Point(9, 9) }) }).bindTooltip(name + mobile, { direction: 'right', noHide: false });
    map.addLayer(gpspoint);//map是在load里面创建好的
    map.setView([lat, lng], 17);//这里是设置视野
    gpspoint.options.userName = name;
    gpspoint.options.mobile = mobile;
    $(gpspoint).on("click", function (e) {   
        layer.open({
            type: 2,
            area: ['1024px', '768px'],
            fixed: false, //不固定
            maxmin: true,
            content: '/TaskArragement/guiji?receiveUserName='+e.target.options.mobile ,
            shade: 0.9,
            shadeClose: true,
            title: e.target.options.userName + "信息展示",
            anim: 5,
            isOutAnim: true
        });
    });
}