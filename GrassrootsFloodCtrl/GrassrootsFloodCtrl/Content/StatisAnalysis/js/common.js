/**公共方法****/
var bycheckitems = [];
var additems = [];
var deleteitems = [];
var common = {
    dangerZone: null,
    init: function () {
        var me = this;
        //鼠标移动over,out事件
        $("#stcity a").hover(function () {
            $(this).addClass("regionon");
            var text = $(this).text();
            var adcd = $(this).attr("data-adcd");
            $(this).siblings().find("img").remove();
            $(this).siblings().removeClass("regionon");
            if (adcd == "3300") {
                $(this).html(text + '<img data-level="5" data-name="' + text + '" data-adcd="' + adcd + '" class="checkimg" src="/Content/ComprehensiveApp/images/appendto.png" title="选择" />');
            } else {
                $(this).html(text + '<img data-level="2" data-name="' + text + '" data-adcd="' + adcd + '" class="checkimg" src="/Content/ComprehensiveApp/images/appendto.png" title="选择" /><img data-level="2" data-name="' + text + '" data-adcd="' + adcd + '" class="addimg" src="/Content/ComprehensiveApp/images/spread.png" title="下拉" />');
            }
           
            //新增项区域
            $("#stcity img.checkimg").click(function () {
                $(".selected-box").show();
                var name = $(this).attr("data-name");
                var adcd = $(this).attr("data-adcd");
               
                var f = $.inArray(adcd, bycheckitems);
                //
                var level = $(this).attr("data-level");
                var cityfather = ""; var countyfather = ""; var soncounty = "";
                if (level == 2) {
                    soncounty = adcd.substring(0, 4);
                    cityfather = adcd.substring(0, 4);
                }
                else if (level == 3) {
                    cityfather = adcd.substring(0, 4);
                }
                else if (level == 4) {
                    cityfather = adcd.substring(0, 4);
                    countyfather = adcd.substring(0, 6) + "000000000";
                    soncounty = adcd.substring(0, 6);
                }
                else{}
                //向上判断
                var fcityfather = $.inArray(cityfather, bycheckitems);
                var fcountyfather = $.inArray(countyfather, bycheckitems);
                //向下判断
                var cityfcountyson = false;
                var cityfctownson = false;

                bycheckitems.forEach(function (item, value) {
                    if (item.indexOf(soncounty) != -1) {
                        cityfctownson = true;
                    }
                    if (item.indexOf(cityfather) != -1) {
                        cityfcountyson = true;
                    }
                });
                //
                var f3300 = $.inArray('3300', bycheckitems);
                //是否为3300
                if (bycheckitems.length != 0 && adcd == "3300") {
                    alert("请移除其他选项，在选择全省！");
                    return;
                }
                else if (bycheckitems.length == 1 && f3300 != -1) {
                    alert("已选择全省！请移除全省选项");
                    return;
                } else if (level == 4 && (fcityfather != -1 || fcountyfather != -1)) {
                    alert("已选择市/县,乡镇已在统计中！");
                    return;
                } else if (level == 3 && cityfctownson) {
                    alert("已选择县下的乡镇,请选移除选项！");
                    return;
                } else if (level == 2 && (cityfcountyson || cityfctownson)) {
                    alert("已选择市下的县/乡镇,请选移除选项！");
                    return;
                } else { }
                //是否有重复项
                if (f == -1) {
                    if (adcd == "3300") {
                        $("#stcounty").hide();
                        $("#sttown").hide();
                    }
                    bycheckitems.push(adcd);
                    $(".selected-box").append('<a data-level="' + level + '" data-adcd="' + adcd + '" href="javascript:void(0);">' + name + '</a>');
                    common.getStaticsData(level, adcd, 1);
                }
            });
            //向下扩展
            $("#stcity img.addimg").click(function () {
                var adcd = $(this).attr("data-adcd");
                var f = $.inArray(adcd, bycheckitems);
                var f3300 = $.inArray('3300', bycheckitems);
                var findfather = $.inArray('', bycheckitems);
                //是否为330
                if (bycheckitems.length == 1 && f3300 != -1) {
                    alert("已选择全省！请移除省");
                    return;
                } else if (f != -1) {
                    alert("已选择市！请移除市");
                    return;
                }
                else {
                    $("#stcounty .region-con").html("");
                    $("#sttown").hide();
                    common.getAreaName1(3, adcd);
                }
            });
        }, function () {
            $(this).removeClass("regionon");
            $(this).find("img").remove();
        });
        //移除已选择
        $(document).on('click', ".selected-box a", function () {
            var acount = $(".selected-box a").length;
            var f = $.inArray($(this).attr("data-adcd"), bycheckitems);
            if (f != -1) {
                bycheckitems.splice(f, 1);
            }
            if (acount == 1) {
                $(".selected-box").hide();
                $("#stcounty").hide();
                if (roleId != 3) { $("#sttown").hide(); }
                else {
                    common.getStaticsData("", useradcd, 1);
                }
            }
            var level = $(this).attr("data-level");
            var adcd = $(this).attr("data-adcd");
            $(this).remove();
            common.getStaticsData(level, adcd, -1);
        });
        //行政区划 三级联动选择
        common.getShiInfo();
    },
    //统计--县级选择移除项
    countyCheckItems: function (level, obj) {
        $("" + obj + " a").hover(function () {
            $(this).addClass("regionon");
            var text = $(this).text();
            var adcd = $(this).attr("data-adcd");
            $(this).siblings().find("img").remove();
            $(this).siblings().removeClass("regionon");
            if (level == 3) {
                $(this).html(text + '<img data-level="' + level + '" data-name="' + text + '" data-adcd="' + adcd + '" class="checkimg" src="/Content/ComprehensiveApp/images/appendto.png" title="选择" /><img data-level="' + level + '" data-name="' + text + '" data-adcd="' + adcd + '" class="addimg" src="/Content/ComprehensiveApp/images/spread.png" title="下拉" />');
            } else {
                $(this).html(text + '<img data-level="' + level + '" data-name="' + text + '" data-adcd="' + adcd + '" class="checkimg" src="/Content/ComprehensiveApp/images/appendto.png" title="选择" />');
            }
            //新增项
            $("" + obj + " img.checkimg").click(function () {
                var typeid = $(".selected-box a[data-level='9']").attr("data-type");
                //if (typeid == "" || typeid == null) { alert("请先选择类型"); return; }
                $(".selected-box").show();
                var name = $(this).attr("data-name");
                var adcd = $(this).attr("data-adcd");
                var f = $.inArray(adcd, bycheckitems);
                //向下判断
                var level = $(this).attr("data-level");
                var cityfather = ""; var countyfather = ""; var soncounty = "";
                if (level == 3) {
                    cityfather = adcd.substring(0, 4);
                    soncounty = adcd.substring(0, 6);
                }
                if (level == 4) {
                    cityfather = adcd.substring(0, 4);
                    countyfather = adcd.substring(0, 6) + "000000000";
                }
                //向上判断
                var fcityfather = $.inArray(cityfather, bycheckitems);
                var fcountyfather = $.inArray(countyfather, bycheckitems);
                //向下判断
                var cityfcountyson = false;
                var cityfctownson = false;
                bycheckitems.forEach(function (item, value) {
                    if (item.indexOf(soncounty) != -1) {
                        cityfctownson = true;
                    }
                    if (item.indexOf(cityfather) != -1) {
                        cityfcountyson = true;
                    }
                });
                //
                var f3300 = $.inArray('3300', bycheckitems);
                //是否为3300
                if (bycheckitems.length != 0 && adcd == "3300") {
                    alert("请移除其他选择，在选择全省！");
                    return;
                }
                else if (bycheckitems.length == 1 && f3300 != -1) {
                    alert("已选择全省！");
                    return;
                } else if (level == 3 && fcityfather != -1) {
                    alert("已选择市,县级已在统计中！");
                    return;
                }
                else if (level == 4 && (fcityfather != -1 || fcountyfather != -1)) {
                    alert("已选择市/县,乡镇已在统计中！");
                    return;
                } else if (level == 3 && cityfctownson) {
                    alert("已选择县下的乡镇,请选移除选项！");
                    return;
                } else if (level == 2 && (cityfcountyson || cityfctownson)) {
                    alert("已选择市下的县/乡镇,请选移除选项！");
                    return;
                } else { }
                //是否有重复项
                if (f == -1) {
                    bycheckitems.push(adcd);
                    $(".selected-box").append('<a data-level="' + level + '" data-adcd="' + adcd + '" href="javascript:void(0);">' + name + '</a>');
                    common.getStaticsData(level, adcd, 1,typeid);
                }
            });
            //向下扩展
            $("" + obj + " img.addimg").click(function () {
                if (level == 3) {
                    var adcd = $(this).attr("data-adcd");
                    var f = $.inArray(adcd, bycheckitems);
                    var f3300 = $.inArray('3300', bycheckitems);
                    //是否为3300
                    if (bycheckitems.length == 1 && f3300 != -1) {
                        alert("已选择全省！");
                        return;
                    } else if (f != -1) {
                        alert("已选择县级,乡镇已在统计中！");
                        $("#sttown .region-con").html("");
                        return;
                    } else {
                        common.getAreaName1(4, adcd);
                    }
                }
            });
        }, function () {
            $(this).removeClass("regionon");
            $(this).find("img").remove();
        });
    },
    //统计--获取数据
    getStaticsData: function (level, adcd, t) {
        if (t == 1) {
            if (adcd == "3300" || (adcd != "3300" && $(".selected-box a").length == 1)) {
                //清空所有
                $(".statics_result li b").text(0);
                $(".person-count li a.transperson").text(0);
            }
        }
        $.ajax({
            url: '/api/StatisAnalysis/Statistics',
            type: 'post',
            data: { adcdtype: level, adcd: adcd, year: globalYear },
            dataType: 'json',
            beforeSend: function () {
                $(".selected-box").append("<a style=\" border:0; background:none;\" id=\"staticLoading\" href=\"javascript:void(0);\"><img src=\"/Content/ComprehensiveApp/images/loading.gif\" width=\"22\" height=\"22\" /></a>");
            },
            success: function (result) {
               // $("#statics_result").show();
                //$("#statics_tishi").hide();
                var result = eval(result[0]);
                $.each(result.disasterPoint, function (i, item) {
                    var nums = 0;
                    switch (item.typename) {
                        case "低洼易涝区":
                            nums = parseInt(item.nums);
                            if (item.typeid == 1) {
                                var item1 = parseInt($("#dwynq").text());
                                if (t == 1) {
                                    $("#dwynq").text(item1 + nums);
                                }
                                else if (t == -1) {
                                    $("#dwynq").text(item1 - nums < 0 ? 0 : item1 - nums);
                                } else { }
                            } else {
                                var item1 = parseInt($("#wgdwynq").text());
                                if (t == 1) {
                                    $("#wgdwynq").text(item1 + nums);
                                }
                                else if (t == -1) {
                                    $("#wgdwynq").text(item1 - nums < 0 ? 0 : item1 - nums);
                                } else { }
                            }
                            break;
                        case "堤防险段":
                            nums = parseInt(item.nums);
                            var item2 = parseInt($("#dfxd").text());
                            if (t == 1) {
                                $("#dfxd").text(item2 + nums);
                            } else if (t == -1) {
                                $("#dfxd").text(item2 - nums < 0 ? 0 : item2 - nums);
                            } else { }
                            break;
                        case "地质灾害点":
                            nums = parseInt(item.nums);
                            if (item.typeid == 1) {
                                var item3 = parseInt($("#dzzhd").text());
                                if (t == 1) {
                                    $("#dzzhd").text(item3 + nums);
                                } else if (t == -1) {
                                    $("#dzzhd").text(item3 - nums < 0 ? 0 : item3 - nums);
                                } else { }
                            } else {
                                var item3 = parseInt($("#wgdzzhd").text());
                                if (t == 1) {
                                    $("#wgdzzhd").text(item3 + nums);
                                } else if (t == -1) {
                                    $("#wgdzzhd").text(item3 - nums < 0 ? 0 : item3 - nums);
                                } else { }
                            }
                            break;
                        case "工棚":
                            nums = parseInt(item.nums);
                            var item4 = parseInt($("#gp").text());
                            if (t == 1) {
                                $("#gp").text(item4 + nums);
                            } else if (t == -1) {
                                $("#gp").text(item4 - nums < 0 ? 0 : item4 - nums);
                            } else { }
                            break;
                        case "海塘险段":
                            nums = parseInt(item.nums);
                            var item5 = parseInt($("#htxd").text());
                            if (t == 1) {
                                $("#htxd").text(item5 + nums);
                            } else if (t == -1) {
                                $("#htxd").text(item5 - nums < 0 ? 0 : item5 - nums);
                            } else { }
                            break;
                        case "山洪灾害危险区":
                            nums = parseInt(item.nums);
                            if (item.typeid == 1) {
                                var item6 = parseInt($("#shzhd").text());
                                if (t == 1) {
                                    $("#shzhd").text(item6 + nums);
                                } else if (t == -1) {
                                    $("#shzhd").text(item6 - nums < 0 ? 0 : item6 - nums);
                                } else { }
                            } else {
                                var item6 = parseInt($("#shwxq").text());
                                if (t == 1) {
                                    $("#shwxq").text(item6 + nums);
                                } else if (t == -1) {
                                    $("#shwxq").text(item6 - nums < 0 ? 0 : item6 - nums);
                                } else { }
                            }
                           
                            break;
                        case "危房":
                            nums = parseInt(item.nums);
                            if (item.typeid == 1) {
                                var item7 = parseInt($("#wf").text());
                                if (t == 1) {
                                    $("#wf").text(item7 + nums);
                                } else if (t == -1) {
                                    $("#wf").text(item7 - nums < 0 ? 0 : item7 - nums);
                                } else { }
                            } else {
                                var item7 = parseInt($("#weifang").text());
                                if (t == 1) {
                                    $("#weifang").text(item7 + nums);
                                } else if (t == -1) {
                                    $("#weifang").text(item7 - nums < 0 ? 0 : item7 - nums);
                                } else { }
                            }
                            break;
                        case "屋顶山塘":
                            nums = parseInt(item.nums);
                            var item8 = parseInt($("#wdst").text());
                            if (t == 1) {
                                $("#wdst").text(item8 + nums);
                            } else if (t == -1) {
                                $("#wdst").text(item8 - nums < 0 ? 0 : item8 - nums);
                            } else { }
                            break;
                        case "地下空间":
                            nums = parseInt(item.nums);
                            var item9 = parseInt($("#dxkj").text());
                            if (t == 1) {
                                $("#dxkj").text(item9 + nums);
                            } else if (t == -1) {
                                $("#dxkj").text(item9 - nums < 0 ? 0 : item9 - nums);
                            } else { }
                            break;
                        case "泵站":
                            nums = parseInt(item.nums);
                            var item10 = parseInt($("#bz").text());
                            if (t == 1) {
                                $("#bz").text(item10 + nums);
                            } else if (t == -1) {
                                $("#bz").text(item10 - nums < 0 ? 0 : item10 - nums);
                            } else { }
                            break;
                        case "水库":
                            nums = parseInt(item.nums);
                            var item11 = parseInt($("#sk").text());
                            if (t == 1) {
                                $("#sk").text(item11 + nums);
                            } else if (t == -1) {
                                $("#sk").text(item11 - nums < 0 ? 0 : item11 - nums);
                            } else { }
                            break;
                        case "水闸":
                            nums = parseInt(item.nums);
                            var item12 = parseInt($("#sz").text());
                            if (t == 1) {
                                $("#sz").text(item12 + nums);
                            } else if (t == -1) {
                                $("#sz").text(item12 - nums < 0 ? 0 : item12 - nums);
                            } else { }
                            break;
                        case "其它":
                            nums = parseInt(item.nums);
                            if (item.typeid == 1) {
                                var item13 = parseInt($("#qt").text());
                                if (t == 1) {
                                    $("#qt").text(item13 + nums);
                                } else if (t == -1) {
                                    $("#qt").text(item13 - nums < 0 ? 0 : item13 - nums);
                                } else { }
                            } else {
                                var item13 = parseInt($("#qita").text());
                                if (t == 1) {
                                    $("#qita").text(item13 + nums);
                                } else if (t == -1) {
                                    $("#qita").text(item13 - nums < 0 ? 0 : item13 - nums);
                                } else { }
                            }
                            break;
                        case "山塘":
                            nums = parseInt(item.nums);
                            var item14 = parseInt($("#st").text());
                            if (t == 1) {
                                $("#st").text(item14 + nums);
                            } else if (t == -1) {
                                $("#st").text(item14 - nums < 0 ? 0 : item14 - nums);
                            } else { }
                            break
                        case "堤防":
                            nums = parseInt(item.nums);
                            var item15 = parseInt($("#difang").text());
                            if (t == 1) {
                                $("#difang").text(item15 + nums);
                            } else if (t == -1) {
                                $("#difang").text(item15 - nums < 0 ? 0 : item15 - nums);
                            } else { }
                            break;
                        case "海塘":
                            nums = parseInt(item.nums);
                            var item16 = parseInt($("#haitang").text());
                            if (t == 1) {
                                $("#haitang").text(item16 + nums);
                            } else if (t == -1) {
                                $("#haitang").text(item16 - nums < 0 ? 0 : item16 - nums);
                            } else { }
                            break;
                        case "电站":
                            nums = parseInt(item.nums);
                            var item17 = parseInt($("#dianzhan").text());
                            if (t == 1) {
                                $("#dianzhan").text(item17 + nums);
                            } else if (t == -1) {
                                $("#dianzhan").text(item17 - nums < 0 ? 0 : item17 - nums);
                            } else { }
                            break;
                        case "圩区":
                            nums = parseInt(item.nums);
                            var item18 = parseInt($("#yuqu").text());
                            if (t == 1) {
                                $("#yuqu").text(item18 + nums);
                            } else if (t == -1) {
                                $("#yuqu").text(item18 - nums < 0 ? 0 : item18 - nums);
                            } else { }
                            break;
                        case "渡槽":
                            nums = parseInt(item.nums);
                            var item19 = parseInt($("#ducao").text());
                            if (t == 1) {
                                $("#ducao").text(item19 + nums);
                            } else if (t == -1) {
                                $("#ducao").text(item19 - nums < 0 ? 0 : item19 - nums);
                            } else { }
                            break;
                        case "山洪灾害危险区":
                            nums = parseInt(item.nums);
                            var item20 = parseInt($("#shwxq").text());
                            if (t == 1) {
                                $("#shwxq").text(item20 + nums);
                            } else if (t == -1) {
                                $("#shwxq").text(item20 - nums < 0 ? 0 : item20 - nums);
                            } else { }
                            break;
                        case "工棚":
                            nums = parseInt(item.nums);
                            var item23 = parseInt($("#gongpeng").text());
                            if (t == 1) {
                                $("#gongpeng").text(item23 + nums);
                            } else if (t == -1) {
                                $("#gongpeng").text(item23 - nums < 0 ? 0 : item23 - nums);
                            } else { }
                            break;
                        case "下沉式立交桥":
                            nums = parseInt(item.nums);
                            var item24 = parseInt($("#xcsljq").text());
                            if (t == 1) {
                                $("#xcsljq").text(item24 + nums);
                            } else if (t == -1) {
                                $("#xcsljq").text(item24 - nums < 0 ? 0 : item24 - nums);
                            } else { }
                            break;
                        case "物资仓库":
                            nums = parseInt(item.nums);
                            var item26 = parseInt($("#wzck").text());
                            if (t == 1) {
                                $("#wzck").text(item26 + nums);
                            } else if (t == -1) {
                                $("#wzck").text(item26 - nums < 0 ? 0 : item26 - nums);
                            } else { }
                            break;
                        case "避灾安置场所":
                            nums = parseInt(item.nums);
                            var item27 = parseInt($("#bzazcs").text());
                            if (t == 1) {
                                $("#bzazcs").text(item27 + nums);
                            } else if (t == -1) {
                                $("#bzazcs").text(item27 - nums < 0 ? 0 : item27 - nums);
                            } else { }
                            break;
                    }
                });

                var countyPLNums = parseInt(result.countyPLNums);
                var countyperson = parseInt($("#dangerType .countyperson").text());
                if (t == 1) {
                    $(".countyperson").text(countyPLNums + countyperson);
                } else if (t == -1) {
                    $(".countyperson").text(countyperson - countyPLNums < 0 ? 0 : countyperson - countyPLNums);
                } else { }

                var townPLNums = parseInt(result.townPLNums);
                var townperson = parseInt($("#dangerType .townperson").text());
                if (t == 1) {
                    $(".townperson").text(townperson + townPLNums);
                } else if (t == -1) {
                    $(".townperson").text(townperson - townPLNums < 0 ? 0 : townperson - townPLNums);
                } else { }

                var villagePLNums = parseInt(result.villagePLNums);
                var villageperson = parseInt($("#dangerType .villageperson").text());
                if (t == 1) {
                    $(".villageperson").text(villageperson + villagePLNums);
                } else if (t == -1) {
                    var r = (villageperson - villagePLNums) < 0 ? 0 : villageperson - villagePLNums;
                    var acount = $(".selected-box a").length;
                    if (acount == 0) r = 0;
                    $(".villageperson").text(r);
                } else { }

                var transferPersonNums = parseInt(result.transferPersonNums);
                var transperson = parseInt($("#dangerType .transperson").text());
                if (t == 1) {
                    $(".transperson").text(isNaN(transperson + transferPersonNums) ? 0 : transperson + transferPersonNums);
                } else if (t == -1) {
                    var tpnums = transperson - transferPersonNums;
                    if (isNaN(tpnums)) { $(".transperson").text(0); } else {
                        $(".transperson").text(tpnums < 0 ? 0 : tpnums);
                    }
                } else { }

                $(".selected-box").find("a#staticLoading").remove();
            },
            error: function () {
                $(".selected-box").find("a#staticLoading").remove();
            }
        });
    },
    //统计获取县镇
    getAreaName1: function (levels, adcd) {
        var lihtml = "";
        $.ajax({
            type: "get",
            url: "/api/ZZTX/GetADCDInfo",
            data: { adcd: adcd + "00000000000", levle: levels, PageSize: 999 },
            dataType: "json",
            success: function (data) {
                if (data != "" && null != data) {
                    var data = eval(data.rows);
                    $.each(data, function (i, item) {
                        lihtml += '<a data-adcd="' + item.adcd + '" href="javascript:void(0);">' + item.adnm + '</a>';
                    });
                }
                switch (levels) {
                    case 3:
                        //县
                        $("#stcounty").show();
                        $("#stcounty .region-con").html(lihtml);
                        //事件
                        common.countyCheckItems(3, "#stcounty .region-con");
                        break;
                    case 4:
                        //乡镇
                        $("#sttown").show();
                        $("#sttown .region-con").html(lihtml);
                        common.countyCheckItems(4, "#sttown .region-con");
                        break;
                }
            }
        });
    },
    //三级联动 市
    getShiInfo: function () {
        common.getAreaName(2, "");
    },
    //三级联动 县
    getQuXianInfo: function (adcd) {
        common.getAreaName(3, adcd);
    },
    //三级联动 乡镇
    getZhenInfo: function (adcd) {
       // $(".searchArealist ul.areas_zhen").remove();
        common.getAreaName(4, adcd);
    },
    //获取行政地方名
    getAreaName: function (levels, adcd) {
        var lihtml = "";
       
    $.ajax({
        
        type: "get",
        url: "/api/ZZTX/GetADCDInfo",
        data: { adcd: adcd, levle: levels, PageSize: 999 },
        dataType: "json",
        success: function (data) {
            if (data != "" && null != data) {
                var data = eval(data.rows);
                $.each(data, function (i, item) {
                    lihtml += '<option data-zbx="' + item.lat + '" data-zby="' + item.lng + '" data-areas="' + item.adnm + '" data-adcd="' + item.adcd + '">' + item.adnm + '</option>';
                });
            }
            switch(levels){
                case 2:
                    //市 
                    var html = '<option value="-1">市</option>' + lihtml;
                    $("#City").html(html);
                    //事件
                    $("#City").change(function(){
                        var name = $(this).find("option:selected").text();
                        var adcd = $(this).find("option:selected").attr("data-adcd");
                        common.getQuXianInfo(adcd);
                        $("#County").show();
                        var x = $(this).find("option:selected").attr('data-zbx');
                        var y = $(this).find("option:selected").attr('data-zby');
                        //common.setMapPosition(x, y, 10, adcd, 1, name);
                    });
                    break;
                case 3:
                    //县
                    var html = '<option value="-1">县(市、区)</option>' + lihtml;
                    $("#County").html(html);
                    //事件
                    $("#County").change(function(){
                        var name = $(this).find("option:selected").text();
                        var adcd = $(this).find("option:selected").attr("data-adcd");
                        common.getZhenInfo(adcd);
                        $("#Town").show();
                        var x = $(this).find("option:selected").attr('data-zbx');
                        var y = $(this).find("option:selected").attr('data-zby');
                       // common.setMapPosition(x, y, 12, adcd, 2, name);
                    });
                    break;
                case 4:
                    //乡镇
                    var html = '<option value="-1">乡镇(街道)</option>' + lihtml;
                    $("#Town").html(html);
                    //事件
                    $("#Town").change(function () {
                        var name = $(this).find("option:selected").text();
                        var adcd = $(this).find("option:selected").attr("data-adcd");
                    });
                    break;
            }

        }
    });
    }
}
common.init();