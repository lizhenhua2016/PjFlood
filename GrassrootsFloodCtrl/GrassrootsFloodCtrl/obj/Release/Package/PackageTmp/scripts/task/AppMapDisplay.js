/*
    username:lizhenhua
    action:app展示地图
*/
//main
var myArray = new Array();
var villageadcd, xiaoshijian, dashijian;
$(function () {
    //level1市，level2县，level3乡镇
    //var leve1adcdl, level2adcd2, leveladcd3,level;

    var roleleve = $(".Hiddenforrole").val();

    //alert($("#sessionadcd").val());
    switch (roleleve) {
        case "5":
            $(".province").Linkage($("#sessionadcd").val());
            break;
        case "1":
            $(".province").Linkage($("#sessionadcd").val());
            break;
        case "2":
            $(".province").Linkage($("#sessionadcd").val());
            break;
        case "3":
            $(".province").Linkage($("#sessionadcd").val());
            break;
        case "4":
            $(".province").Linkage($("#sessionadcd").val());
            break;
        default:
        //alert("0");
    }

    //生成一个数组，每次点击的时候把adcd存入
});
var cities = new L.layerGroup([]);
$.fn.extend({
    //获取县级简介
    GetLittleInfoForCounty: function () {
        $.ajax({
            type: "post",
            dataType: "json",
            url: "",
            data: {},
            success: function () {
            }
        });
    },
    //获取应急事件名称
    GetWarnList: function (adcd) {
        $.ajax({
            type: "post",
            url: "/api/AppWarnEvent/RouteGetWarnListByVillageAdcd",
            dataType: "json",
            data: { "adcd": adcd },
            success: function (data) {
                var sb = new StringBuilder();
                var haha = eval(data);
                //alert(haha.toString());
                if (haha.toString() == "") {
                    alert("没有获取到履职数据！");
                } else {
                    sb.append("<ul>");
                    $.each(haha, function (i, item) {
                        sb.append("<li  data-userPhone='" + item.userPhone + "' data-eventid='" + item.id + "'>" + item.eventName + "</li>")
                    });
                    sb.append("</ul>");
                    $("#warnlist").children("ul").empty();//清空
                    $("#warnlist").append(sb.toString());

                    if ($("#warnlist").children("p").children("span").length > 0) {
                        $("#warnlist").children("p").children("span").empty();
                    }

                    //点击小事件触发
                    $("#warnlist").show().children("ul").children("li").on("click", function () {
                        dashijian = $(this).attr("data-eventid");

                        $("#warnlist").GetNextWarnList($(this).attr("data-eventid"), dashijian);
                    });
                }
            }
        });
    },
    //获取预警小事件
    GetNextWarnList: function (warnid, dashijian) {
        //返回
        $("#warnlist").children("ul").remove();

        $("#warnlist").children("p").append("<span class='returnback2 pull-right'><<返回</span>");

        $(".returnback2").click(function () {
            $(this).empty();
            $("#warnlist").GetWarnList(villageadcd);
        });

        //开始
        $.ajax({
            type: "post",
            url: "/api/AppWarnEvent/RouteGetWarnEventNext",
            dataType: "json",
            data: { "WarnEventId": warnid, "villageadcd": villageadcd },
            success: function (data) {
                var sb = new StringBuilder();
                var haha = eval(data);
                sb.append("<ul>");
                $.each(haha, function (i, item) {
                    var messagestr = item.villageMessage;

                    if (messagestr.length > 10) {
                        messagestr = messagestr.substring(0, 10) + "...";
                    }

                    sb.append("<li class='h' data-AppWarnEventId='" + item.appWarnEventId + "' data-warninfoid='" + item.warninfoid + "' data-level='" + item.warninglevel + "'>" + messagestr + "</li>");
                });
                sb.append("</ul>");
                $("#warnlist").append(sb.toString());
                

                $("#warnlist").children("ul").children("li").on("click", function () {
                    xiaoshijian = $(this).attr("data-warninfoid");
                    if ($(".viewduty").length > 0) {
                        $(".viewduty").empty();
                    }

                    $(this).append("<span data-wraninfoid='" + xiaoshijian + "' class='viewduty pull-right'>履职查看</span>").addClass("currentxiaoshijian").siblings().removeClass("currentxiaoshijian");

                    $("#map").AppVillageGpsDisplay(villageadcd, xiaoshijian, dashijian);
                    var urlopen = "/TaskArragement/VilliageDuty?resiveadcd=" + villageadcd + "&warninfoid=" + xiaoshijian;

                    //弹出履职查看
                    $(".viewduty").on("click", function () {
                        layer.open({
                            type: 2,
                            area: ['1024px', '768px'],
                            fixed: false, //不固定
                            maxmin: true,
                            content: urlopen,
                            shade: 0.7,
                            shadeClose: true,
                            title: "信息展示",
                            anim: 5,
                            isOutAnim: true
                        });
                    })
                });
            }
        });
    },
    //获取人员定位，通过小事件
    GetGpsByWarnLevel: function (warnlevel) {
        $.ajax({
            type: "post",
            url: "",
            dataType: "json",
            data: {},
            success: function (data) {
            }
        });
    },
    //获取App人数
    AppGetRegCount: function (adcd, grad) {
        $.ajax({
            type: "post",
            url: "/api/AppApi/AppGetRegCount",
            dataType: "json",
            data: { "adcd": adcd, "grad": grad },
            success: function (data) {
                $(".countyinfo").append("<span style='color:#f17c67'>县级注册人数:<b>" + data + "</b></span>");
            }
        });

        $.ajax({
            type: "post",
            url: "/api/AppApi/AppGetRegCountByCountyAdcdForTown",
            dataType: "json",
            data: { "adcd": adcd },
            success: function (data) {
                $(".countyinfo").append("<span style='color:#f17c67'>镇级注册人数:<b>" + data + "</b></span>");
            }
        });

        $.ajax({
            type: "post",
            url: "/api/AppApi/AppGetRegCountByCountyAdcdForVillage",
            dataType: "json",
            data: { "adcd": adcd },
            success: function (data) {
                $(".countyinfo").append("<span style='color:#f17c67'>村级注册人数:<b>" + data + "</b></span>");
            }
        });
    },

    //显示gps定位
    AppGpsMapDisplay: function (lat, lng) {
        png = "http://ovsqnadmr.bkt.clouddn.com/p1.png";
        var gpspoint = L.marker(L.latLng(lat, lng), {
            icon: L.icon({
                iconUrl: png,
                iconAnchor: new L.Point(9, 9)
            })
        }).bindTooltip(name + mobile, { direction: 'right', noHide: false });
        map.addLayer(gpspoint);//map是在load里面创建好的
        map.setView([lat, lng], 17);//这里是设置视野
        gpspoint.options.userName = name;
        gpspoint.options.mobile = mobile;
    },
    //村网格负责人定位
    AppVillageGpsDisplay: function (villageadcd, xiaoshijian, dashijian) {
        $.ajax({
            type: "post",
            url: "/api/AppApi/AppGetGpsLocation",
            dataType: "json",
            data: { "villageadcd": villageadcd, "warninfoid": xiaoshijian },
            success: function (data) {
                var haha = eval(data);
                cities.clearLayers();
                $.each(haha, function (i, item) {
                    var str = item.location.split(";");

                    //for (var i = 0; i < str.length; i++) {
                    var x = str[0].split(",")[0];
                    var y = str[0].split(",")[1];

                    var mappoint = L.marker(L.latLng(y, x), {
                        icon: L.icon({
                            iconUrl: "http://ovsqnadmr.bkt.clouddn.com/p1.png",
                            iconAnchor: new L.Point(9, 9)
                        })
                    }).bindTooltip(item.receiveUserName + "," + item.username, {
                        offset: [20, 10],
                        direction: "right"
                    }).addTo(cities);
                    map.addLayer(cities);//map是在load里面创建好的
                    map.setView([y, x], 18);//这里是设置视野
                    mappoint.options.receiveUserName = item.receiveUserName;
                    mappoint.options.username = item.username;
                    mappoint.options.xiaoshijian = xiaoshijian;
                    mappoint.options.dashijian = dashijian;

                    mappoint.on("click", function (e) {
                        var urlopen = '/TaskArragement/lvzhi?resevusername=' + e.target.options.username + '&xiaoshijian=' + mappoint.options.xiaoshijian + '&dashijian=' + dashijian;
                        var x="1280px";
                        var y="600px";
                        layer.open({
                            type: 2,
                            area: [x,y],
                            fixed: true, //不固定
                            maxmin: true,
                            content: urlopen,
                            shade: 0.7,
                            shadeClose: true,
                            title: e.target.options.receiveUserName + "信息展示",
                            anim: 5,
                            isOutAnim: true
                        });
                    });
                    //}
                });
            }
        });
    },

    //通过adcd获取县级的app地图展示
    Linkage: function (adcd) {
        $(".province").empty();
        //获取上面部分导航
        $.ajax({
            type: "post",
            url: "/api/ZZTX/GetAdcdInfoByAdcd",
            dataType: "json",
            data: { "adcd": adcd },
            success: function (data) {
                //这里是返回的数组，记录在数组中
                if (data.grade < 3) {
                    myArray.push(data.grade + "," + adcd);
                }

                var sb = new StringBuilder();
                sb.append("<p data-lng='" + data.lng + "' data-lat='" + data.lat + "' data-grad='" + data.grade + "' data-name='" + data.adnm + "' data-adcd='" + data.adcd + "'>当前位置：" + data.adnm + "<span class='returnback pull-right'><<返回</span></p>");
                sb.append("<div class='clear'></div>");

                $(".province").show().append(sb.toString());
                //此处判断省市县乡等级，如果grade大于3了说明到了村级
                if (data.grade > 3) {
                    
                }
                else {
                    //获取下一级全部的信息
                    $(".province").GetNextLevleAdcdInfoByAdcd(adcd);//获取下一级信息
                }
                //地图上显示一个点
                $("#map").DisplayOnMap(data.grade + 1, data.adnm, data.lat, data.lng);

                $(".returnback").on("click", function () {
                    if (data.grade > 2) {
                        var s = myArray.pop().split(",").pop();

                        $(".province").Linkage(s);
                    }
                    else {
                        var j = data.grade - 1;

                        $(".province").Linkage(myArray[j].split(",")[1]);
                        if (j == 0) {
                            myArray.splice(0, myArray.length);
                        }
                    }
                });
            }
        });
    },
    //获取下一级的信息
    GetNextLevleAdcdInfoByAdcd: function (adcd) {
        $.ajax({
            type: "post",
            url: "/api/ZZTX/GetNextLevleAdcdInfoByAdcd",
            dataType: "json",
            data: { "adcd": adcd },
            success: function (data) {
                var sb = new StringBuilder();
                sb.append("<ul>");
                var haha = eval(data);
                $.each(haha, function (i, item) {
                    sb.append("<li data-grade='" + item.grade + "' data-lng='" + item.lng + "' data-lat='" + item.lat + "' data-name='" + item.adnm + "' data-adcd='" + item.adcd + "'>" + item.adnm + "</li>");
                });
                sb.append("</ul>");
                $(".province").show().append(sb.toString());
                $(".province").children("ul").children("li").on("click", function () {
                    $(this).addClass("currentli").siblings().removeClass("currentli");
                    //如果大于3说明已经到村级，递归跳出
                    if ($(this).attr("data-grade") > 3) {
                        //代表到村级了 5是代表村级
                        $("#map").DisplayOnMap(5, $(this).attr("data-name"), $(this).attr("data-lat"), $(this).attr("data-lng"));
                        //显示事件
                        villageadcd = $(this).attr("data-adcd");
                        $("#warnlist").GetWarnList($(this).attr("data-adcd"));
                    }
                    else {
                        $(".province").Linkage($(this).attr("data-adcd"));
                    }
                })
            }
        });
    },
    //地图上显示省市县乡镇的图标
    DisplayOnMap: function (roleid, adnm, lat, lng) {
        var png, zoom;
        switch (roleid) {
            case 2://市级
                zoom = 10;
                png = "/Content/ComprehensiveApp/Images/city.png";
                break;
            case 3://县级
                zoom = 12;
                png = "/Content/ComprehensiveApp/Images/county.png";
                break;
            case 4://镇级
                zoom = 14;
                png = "/Content/ComprehensiveApp/Images/town.png";
                break;
            case 5://省级
                zoom = 18;
                png = "/Images/cun.png";
                break;
            default:
                zoom = 8;
                png = "/#";//"/Images/province.png";
            //png = "";
        }
        var mappoint = L.marker(L.latLng(lat, lng), {
            icon: L.icon({
                iconUrl: png,
                iconAnchor: new L.Point(9, 9)
            })
        }).bindPopup(adnm, { direction: 'right', permanent: true, offset: [20, 10] }).addTo(map);
        map.addLayer(mappoint);//map是在load里面创建好的
        map.setView([lat, lng], zoom);//这里是设置视野
    }
    //获取gps定位
});