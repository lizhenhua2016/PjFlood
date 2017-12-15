/**公共方法****/
var bycheckitems = [];
var additems = [];
var deleteitems = [];
//app聚类时对于某一个地区的父adcd；
var appAdcd = 0;
var common = {
    dangerZone: null,
    init: function () {
        var me = this;
        //$(".search-areas a").text("行政区划");
        $("#TuCheng").hide();
        //搜索框焦点获取事件
        $("#textFrameSearch").keyup(function () {
            me.showaDelKey();
        });
        $(".layer-control .head").click(function () {
            if ($("#TuCheng").css("display") == 'none') {
                $("#TuCheng").show();
                $(this).find("a").css("background", "tcup.png");
            } else {
                $("#TuCheng").hide();
            }
        });
        //common.loadFrameXingZhengChun();
        /***类型对象选择*****/
        /***删除按钮事件***/
        $("#aDelKey").click(function () {
            $("#textFrameSearch").val("");
            me.hidaDelKey();
            me.hideFrameXingZhengChun();
            me.hideFrameSearchAll();
            $("#topId").val("");
            $("#topType").val("");
            $("#FrameCunInfo").hide();
            $("#textFrameSearch").attr("data-type", "");
        });
        /****关键字匹配****/
        //$('#textFrameSearch').typeahead({
        //    source: function (query, process) {
        //       var newquery = common.excludeSpecial(query);
        //        return $.ajax({
        //            url: '/api/CApp/GetKeys',
        //            type: 'post',
        //            data: { name: newquery },
        //            dataType: 'json',
        //            success: function (result) {
        //                // 这里的数据解析根据后台传入格式的不同而不同
        //                $("#topId").val("");
        //                $("#topType").val("");
        //                var json = eval(result);
        //                var resultList = json.map(function (item) {
        //                    var aItem = { id: item.adcd, name: item.name, ctype: item.ctype };
        //                    return JSON.stringify(aItem);
        //                });
        //                return process(resultList);
        //            },
        //            error: function () {
        //                $("#topId").val("");
        //                $("#topType").val("");
        //            }
        //        });
        //    },
        //    matcher: function (obj) {
        //        var item = JSON.parse(obj);
        //        return ~item.name.toLowerCase().indexOf(this.query.toLowerCase())
        //    },

        //    sorter: function (items) {
        //        var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
        //        while (aItem = items.shift()) {
        //            var item = JSON.parse(aItem);
        //            if (!item.name.toLowerCase().indexOf(this.query.toLowerCase()))
        //                beginswith.push(JSON.stringify(item));
        //            else if (~item.name.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
        //            else caseInsensitive.push(JSON.stringify(item));
        //        }
        //        return beginswith.concat(caseSensitive, caseInsensitive)
        //    },
        //    highlighter: function (obj) {
        //        var item = JSON.parse(obj);
        //        var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&')
        //        return item.name.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
        //            return '<strong>' + match + '</strong>'
        //        })
        //    },
        //    updater: function (obj) {
        //        var item = JSON.parse(obj);
        //        $('#topId').attr('value', item.id);
        //        $('#topType').attr('value',item.ctype);
        //        //me.searchWay("", item.id, item.ctype);
        //        return item.name;
        //    }
        //});

        /**查询事件***/
        $(".search-btn").click(function () {
            var key = $("#textFrameSearch").val();
            var adcd = $("#topId").val();
            var ctype = $("#topType").val();
            $("#FrameSearchAll").html("");
            common.searchWay(key, adcd, ctype);
        });

        //责任人详情
        $(".showZeRenRenInfo").click(function () {
            $("#FrameZeRenRenInfo").show();
            var stype = $("#textFrameSearch").attr("data-type");
            var town = $("#s2id_loc_town .select2-chosen").text();
            var cun = $("#s2id_loc_cun .select2-chosen").text();
            switch (stype) {
                case "4":
                    if (cun == "行政村") {
                        $("#FrameZeRenRenTown").hide();
                        $("#FrameZeRenRenCun").hide();
                    }
                    else {
                        $("#FrameZeRenRenTown").hide();
                        $("#FrameZeRenRenCun").hide();
                    }
                    break;
                default:
                    $("#FrameSearchAll").hide();
                    break;
            }
        });
        //村详情返回
        $(".showReturnFrameSearchAll").click(function () {
            $("#FrameCunInfo").removeAttr("style").hide();
            $("#FrameTownInfo").show();
            var stype = $("#textFrameSearch").attr("data-type");
            var town = $("#s2id_loc_town .select2-chosen").text();
            var cun = $("#s2id_loc_cun .select2-chosen").text();
            switch (stype) {
                case "1":
                    if (cun == "行政村") {
                        var rtype = $(this).attr("dataType");
                        if (rtype == "0") {
                            //$("#FrameSearchTown").show();
                            $("#FrameSearchCun").hide();
                        } else {
                            //$("#FrameTownInfo").show();
                            //$("#FrameSearchTown").show();
                            $("#FrameCunInfo").hide();
                        }
                    }
                    else {
                        $("#FrameSearchTown").hide();
                        $("#FrameSearchCun").show();
                    }
                    break;
                case "4":
                    if (cun == "行政村") {
                        $("#FrameZeRenRenTown").show();
                        $("#FrameZeRenRenCun").hide();
                    }
                    else {
                        $("#FrameZeRenRenTown").hide();
                        $("#FrameZeRenRenCun").show();
                    }
                    break;
                default:

                    break;
            }
            $("#FrameZeRenRenInfo").hide();
        });
        $(".AppPersonListReturn").click(function () {
            $("#FrameCunInfoApp").hide();
        });
        //镇详情返回
        $("#RTT").click(function (e) {
            //$("#FrameTownInfo").removeAttr("style").hide();
            var w = -$(".left-layer").width();
            var _left = $("#FrameTownInfo").offset().left;
            if (_left >= 0) {
                $("#FrameTownInfo").animate({ left: w }, 300, 'swing', function () {
                    $("#FrameTownInfo a#RTT").hide().removeClass("townreturn").addClass("townsh").fadeIn(500);
                });
            } else {
                $("#FrameTownInfo a#RTT").removeClass("townsh").addClass("townreturn");
                $("#FrameTownInfo").animate({ left: 0 }, 300, 'swing', function () {
                });
            }
        });
        //县详情返回
        $("#RTT2").click(function (e) {
            //$("#FrameTownInfo").removeAttr("style").hide();
            var w = -$(".left-layer").width();
            var _left = $("#FrameCountyInfo").offset().left;
            if (_left >= 0) {
                $("#FrameCountyInfo").animate({ left: w }, 300, 'swing', function () {
                    $("#FrameCountyInfo a#RTT2").hide().removeClass("townreturn").addClass("townsh").fadeIn(500);
                });
            } else {
                $("#FrameCountyInfo a#RTT2").removeClass("townsh").addClass("townreturn");
                $("#FrameCountyInfo").animate({ left: 0 }, 300, 'swing', function () {
                });
            }
        });
        $("#RTTApp").click(function (e) {
            //$("#FrameTownInfo").removeAttr("style").hide();
            var w = -$("#projectCheck").width();
            var _left = $("#projectCheck").offset().left;
            var lw = $(window).width() + w;
            if (_left == lw) {
                $("#projectCheck").animate({ right: w }, 300, 'swing', function () {
                    $("#RTTApp a").removeClass("appclose").addClass("appopen").fadeIn(330);
                });
            } else {
                $("#RTTApp a").removeClass("appopen").addClass("appclose");
                $("#projectCheck").animate({ right: 0 }, 300, 'swing', function () {
                });
            }
        });
        $("#FrameTuCengInfo .ReturnToTown").click(function (e) {
            $("#FrameTuCengInfo").removeAttr("style").hide(100);
        });
        //tab切换
        $("#FrameCunInfo .tabbox a").click(function () {
            var index = $(this).index();
            $(this).attr('class', "curr").siblings('a').attr('class', '');
            $('#FrameCunInfo .context').eq(index).show().siblings('.context').hide();
            var adcd = $("#FrameCunInfo").attr("data-adcd");
            switch (index) {
                case 1:
                    //防汛防台工作组
                    common.CunPersonLiable(adcd);
                    break;
                case 2:
                    //网格责任人
                    common.CunWangGE(adcd);
                    break;
                case 3:
                    //人员转移清单
                    common.CunRemoveList(adcd);
                    break;
                case 4:
                    //防汛防台形势图
                    common.CunPic(adcd);
                    break;
                // case 5:
                //     //到岗人员统计
                //     common.CunPersonLiableList(adcd, $("#FrameCunInfo").attr("data-name"));
                //     break;
            }
            if (index === "3") { $("#TuCheng").show(100); }
            else { $("#TuCheng").hide(); }
        });
        $("#FrameTownInfo .tabbox a").click(function () {
            var index = $(this).index(); $("#TuCheng").hide();
            $(this).attr('class', "curr").siblings('a').attr('class', '');
            $('#FrameTownInfo .context').eq(index).show().siblings('.context').hide();
            var adcd = $("#FrameTownInfo").attr("data-adcd");
            switch (index) {
                case 1:
                    common.TownPersonLiable(adcd);
                    break;
                case 2:
                    common.TownWangGeZRR(adcd);
                    break;
            }
        });
        $("#FrameCountyInfo .tabbox a").click(function () {
            var index = $(this).index(); $("#TuCheng").hide();
            $(this).attr('class', "curr").siblings('a').attr('class', '');
            $('#FrameCountyInfo .context').eq(index).show().siblings('.context').hide();
            var adcd = $("#FrameCountyInfo").attr("data-adcd");
            switch (index) {
            case 1:
                    common.CountyLeaderInfo(adcd);
                break;
            case 2:
                common.TownWangGeZRR(adcd);
                break;
            }
        });
        /*********图层s*************/
        $("#TuCheng li").click(function () {
            var dataType = $(this).find("input").attr("data-type");
            if ($(this).find("input").is(":checked")) {
                map.generateFeatureLayerByRegion(map.overLayerGroup[dataType]);
            } else {
                map.overLayerGroup[dataType].hideLayer();
            }
        });
        if (_useradcd == "330000000000000") {
            common.getShiInfo();
        }
        //行政区划 三级联动选择 search-areas
        if (_level !== null && _level !== "") {
            
            switch (_level) {
                case 1:
                    common.getShiInfo();
                    break;
                case 2:
                    common.getAreaName2(3, _useradcd, "");
                    break;
                case 3:
                    common.getAreaName2();
                    break;
            }
        }

        $("#zhen_szd").click(function () {
            var adcd = $("#FrameTownInfo").attr("data-adcd");
            var html = "";
            $("#zhen_wanggetj").show();
            $.ajax({
                type: "post",
                url: "/Services/GetZhenInfo.ashx?type=3",
                data: { ADCD: adcd },
                dataType: "json",
                success: function (data) {
                    var data = eval(data);
                    if (null != data && data != "") {
                        $.each(data, function (i, item) {
                            html += '<table><tbody><tr><td width="100" rowspan="3" class="subtitle" data-wgid="' + item.ID + '"><img src="/Content/ComprehensiveApp/images/wg_' + item.ID + '.png" style="width:45px; height:27px;" onerror="this.src=\'\'" /><br />' + item.WangGeName + '</td>';
                            html += ' <td class="rtext">受灾点： ' + item.DangerCount + '个 </td>';
                            html += ' <td class="rtext">影响人数：' + item.InfluenceNum + '人</td>';
                            html += '<td>&nbsp;</td>';
                            html += '</tr><tr> ';
                            html += '<td class="rtext">转移责任人数：' + item.RemoveMans + '人</td>';
                            html += '<td class="rtext">预警责任人数：' + item.WarningMans + '人 </td>';
                            html += '<td>&nbsp;</td>';
                            html += '</tr></tbody></table>';
                        });
                    } else {
                        $("#zhen_wanggetj").html(html);
                    }
                    $("#zhen_wanggetj").html(html);
                    $("#zhen_wanggetj td.subtitle").click(function () {
                        var typeid = $(this).attr("data-wgid");
                        var adcd = $("#FrameTownInfo").attr("data-adcd");
                        $.ajax({
                            type: "GET",
                            url: '/Services/GetCunDot.ashx?type=4',
                            dataType: "json",
                            data: { ADCD: adcd, wgid: typeid },
                            success: function (result) {
                                if (result != "" && result != null) {
                                    //webapps.DisplayFeatures();
                                }
                            }
                        })
                    });
                }
            });
        });
        //统计
        $(".info-countbtn").click(function () {
            $(".info-count").show();
        });
        $(".info-close").click(function () {
            $(".info-count").hide();
        });
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
            //新增项
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
                } else { }
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
            }
            $(this).remove();
            var level = $(this).attr("data-level");
            var adcd = $(this).attr("data-adcd");
            common.getStaticsData(level, adcd, -1);
        });
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
                    common.getStaticsData(level, adcd, 1);
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
        $.ajax({
            url: '/api/CApp/Statistics',
            type: 'post',
            data: { adcdtype: level, adcd: adcd, year: globalYear },
            dataType: 'json',
            //async:false,
            beforeSend: function () {
                $(".selected-box").append("<a style=\" border:0; background:none;\" id=\"staticLoading\" href=\"javascript:void(0);\"><img src=\"/Content/ComprehensiveApp/images/loading.gif\" width=\"22\" height=\"22\" /></a>");
            },
            success: function (result) {
                var result = eval(result[0]);
                $.each(result.disasterPoint, function (i, item) {
                    var nums = 0;
                    switch (item.typename) {
                        case "低洼易涝区":
                            nums = parseInt(item.nums);
                            var item1 = parseInt($("#dwynq").text());
                            if (t == 1) {
                                $("#dwynq").text(item1 + nums);
                            }
                            else if (t == -1) {
                                $("#dwynq").text(item1 - nums < 0 ? 0 : item1 - nums);
                            } else { }
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
                            var item3 = parseInt($("#dzzhd").text());
                            if (t == 1) {
                                $("#dzzhd").text(item3 + nums);
                            } else if (t == -1) {
                                $("#dzzhd").text(item3 - nums < 0 ? 0 : item3 - nums);
                            } else { }
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
                            var item6 = parseInt($("#shzhd").text());
                            if (t == 1) {
                                $("#shzhd").text(item6 + nums);
                            } else if (t == -1) {
                                $("#shzhd").text(item6 - nums < 0 ? 0 : item6 - nums);
                            } else { }
                            break;
                        case "危房":
                            nums = parseInt(item.nums);
                            var item7 = parseInt($("#wf").text());
                            if (t == 1) {
                                $("#wf").text(item7 + nums);
                            } else if (t == -1) {
                                $("#wf").text(item7 - nums < 0 ? 0 : item7 - nums);
                            } else { }
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
                            var item13 = parseInt($("#qita").text());
                            if (t == 1) {
                                $("#qita").text(item13 + nums);
                            } else if (t == -1) {
                                $("#qita").text(item13 - nums < 0 ? 0 : item13 - nums);
                            } else { }
                            break;
                    }
                });
                var countyPLNums = parseInt(result.countyPLNums);
                var countyperson = parseInt($("#countyperson").text());
                if (t == 1) {
                    $("#countyperson").text(countyPLNums + countyperson);
                } else if (t == -1) {
                    $("#countyperson").text(countyperson - countyPLNums < 0 ? 0 : countyperson - countyPLNums);
                } else { }

                var townPLNums = parseInt(result.townPLNums);
                var townperson = parseInt($("#townperson").text());
                if (t == 1) {
                    $("#townperson").text(townperson + townPLNums);
                } else if (t == -1) {
                    $("#townperson").text(townperson - townPLNums < 0 ? 0 : townperson - townPLNums);
                } else { }

                var villagePLNums = parseInt(result.villagePLNums);
                var villageperson = parseInt($("#villageperson").text());
                if (t == 1) {
                    $("#villageperson").text(villageperson + villagePLNums);
                } else if (t == -1) {
                    console.log(adcd + "=" + level + "=" + villageperson + "=" + villagePLNums);
                    var r = (villageperson - villagePLNums) < 0 ? 0 : villageperson - villagePLNums;
                    var acount = $(".selected-box a").length;
                    if (acount == 0) r = 0;
                    $("#villageperson").text(r);
                } else { }

                var transferPersonNums = parseInt(result.transferPersonNums);
                var transperson = parseInt($("#transperson").text());
                if (t == 1) {
                    $("#transperson").text(transperson + transferPersonNums);
                } else if (t == -1) {
                    $("#transperson").text(transperson - transferPersonNums < 0 ? 0 : transperson - transferPersonNums);
                } else { }
                $(".selected-box").find("a#staticLoading").remove();
            },
            error: function () {
                $(".selected-box").find("a#staticLoading").remove();
            }
        });
    },
    searchWay: function (key, adcd, ctype) {
        common.showFrameSearchAll();
        if ($.trim(key) == "") {
            alert("请输入关键字");
            return;
        }
        var stype = $("#textFrameSearch").attr("data-type");
        var html = "";
        var num = 0;
        $.ajax({
            url: '/api/CApp/GetKeysInfo',
            type: 'post',
            data: { name: key, adcd: adcd, year: globalYear, ctype: ctype },
            dataType: 'json',
            beforeSend: function () {
                $("#FrameSearchAll").addClass("searchlist");
                $("#FrameSearchAll").html("<div class=\"listcontent\"><table class=\"showZeRenRenInfo\"><tr><td>数据统计中<img src=\"/Content/ComprehensiveApp/images/loading.gif\" width=\"80\" height=\"80\" /></td></tr></table></div>");
            },
            success: function (result) {
                if (result == "") {
                    $("#FrameSearchAll").html("<div class=\"listcontent\"><table class=\"showZeRenRenInfo\"><tr><td>暂无相匹配的数据</td></tr></table></div>");
                    return;
                }
                var json = eval(result);
                $.each(json, function (i, item) {
                    num = num + 1;
                    html += '<div class="listcontent" data-adcd="' + item.adcd + '" ';
                    if (item.types != 0 && item.types != 4) {
                        html += 'data-zw="' + item.zhiWuName + '" data-post="' + item.zhiWuName + '" data-mobile="' + item.mobile + '"';
                    }
                    html += ' data-areas="' + item.areas + '" data-type="' + item.types + '">';
                    html += '<table class="showZeRenRenInfo">';
                    html += '<tr>';
                    html += '<td width="30" rowspan="3" valign="top"><div class="num">' + num + '</div></td>';
                    html += '<td class="name">' + item.name + '</td>';
                    html += '<td colspan="2" width=100 align="right" style="padding-right:15px;"><span>' + item.typeName + '</span></td>';
                    html += '</tr>';
                    if (item.types != 0 && item.types != 4) {
                        html += '<tr><td colspan="2" style="font-size:12px;">级别：' + item.zhiWuName + '</td></tr>';
                    }
                    html += '<tr><td colspan="2" style="font-size:12px;">所在地区：' + item.areas + '</td></tr>';
                    html += '</table>';
                    html += '</div>';
                });
                if (html != "") {
                    $("#FrameSearchAll").addClass("searchlist");
                    $("#FrameSearchAll").html(html);
                }
                $(".listcontent").bind("click", function () {
                    var adcd = $(this).attr("data-adcd");
                    var areas = $(this).attr("data-areas");
                    var type = $(this).attr("data-type");
                    var name = $(this).find("td.name").text();
                    var post = $(this).attr("data-post");
                    var mobile = $(this).attr("data-mobile");
                    var id = $(this).attr("data-id");
                    //0镇4村1镇责任人2村责任人3县责任人
                    switch (type) {
                        //镇
                        case "0":
                            common.TownInfo(adcd, 1);
                            break;
                        //村
                        case "4":
                            common.CunInfo(adcd);
                            break;
                        //人
                        case "1":
                            common.RenInfo(adcd, areas, name, mobile, post);
                            break;
                        case "2":
                            common.RenInfo(adcd, areas, name, mobile, post);
                            break;
                        case "3":
                            common.RenInfo(adcd, areas, name, mobile, post);
                            break;
                        default: break;
                    }
                });
            },
            error: function () {
                $("#FrameSearchAll").html("<div class=\"listcontent\"><table class=\"showZeRenRenInfo\"><tr><td>暂没匹配到相关内容，请输入更精准的关键字</td></tr></table></div>");
            }
        });
    },
    tabInitCun: function () {
        $("#FrameCunInfo .tabbox a:first").attr('class', "curr").siblings('a').attr('class', '');
        $('#FrameCunInfo .context').eq(0).show().siblings('.context').hide();
    },
    tabInitTown: function () {
        $("#FrameTownInfo .tabbox a:first").attr('class', "curr").siblings('a').attr('class', '');
        $('#FrameTownInfo .context').eq(0).show().siblings('.context').hide();
    },
    tabInitCounty: function () {
        $("#FrameCountyInfo .tabbox a:first").attr('class', "curr").siblings('a').attr('class', '');
        $('#FrameCountyInfo .context').eq(0).show().siblings('.context').hide();
    },
    //三级联动 市
    getShiInfo: function () {
        common.getAreaName(2, "");
    },
    //三级联动 县
    getQuXianInfo: function (adcd, name) {
        //$(".searchArealist ul.areas_quxian").remove();
        //$(".searchArealist ul.areas_zhen").remove();
        common.getAreaName(3, adcd, name);
    },
    //三级联动 乡镇
    getZhenInfo: function (adcd, name) {
        $(".searchArealist ul.areas_zhen").remove();
        common.getAreaName(4, adcd, name);
    },
    //获取行政地方名
    getAreaName: function (levels, adcd, name) {
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
                        lihtml += '<li data-zbx="' + item.lat + '" data-zby="' + item.lng + '" data-areas="' + item.adnm + '" data-adcd="' + item.adcd + '">' + item.adnm + '</li>';
                    });
                }
                switch (levels) {
                    case 2:
                        $(".searchArealist").empty();
                        //市
                        var html = '<ul class="areas_shi">' + lihtml + '</ul>';
                        $(".searchArealist").append(html);
                        //事件
                        $(".areas_shi li").click(function () {
                            var name = $(this).text();
                            $(".search-areas a").text(name);
                            var adcd = $(this).attr("data-adcd");
                            common.getQuXianInfo(adcd, name);
                            $(this).addClass("selected");
                            $(this).siblings('li').removeClass("selected");

                            var x = $(this).attr('data-zbx');
                            var y = $(this).attr('data-zby');
                            common.setMapPosition(x, y, 10, adcd, 1, name);
                            $(this).parent().hide();
                        });
                        break;
                    case 3:
                        //县
                        var newlihtml = '<li style="width:90%" data-type="returnLast">当前位置:<a href="javascript:void(0)">' + name + '</a><a href="javascript:void(0)" style="float:right;"><<返回</a></li>' + lihtml;
                        var html = '<ul class="areas_quxian">' + newlihtml + '</ul>';
                        $(".searchArealist ul.areas_shi").after(html);
                        //事件
                        $(".areas_quxian li").click(function () {
                            var name = $(this).text();
                            var adcd = $(this).attr("data-adcd");
                            var datatype = $(this).attr("data-type");
                            if (datatype == "returnLast") {
                                $(this).parent().remove();
                                $(".areas_shi").show();
                                return;
                            } else {
                                $(this).parent().hide();
                            }
                            common.getZhenInfo(adcd, name);
                            $(".search-areas a").text(name);
                            $(this).addClass("selected");
                            $(this).siblings('li').removeClass("selected");

                            var x = $(this).attr('data-zbx');
                            var y = $(this).attr('data-zby');

                            common.setMapPosition(x, y, 12, adcd, 2, name);
                        });
                        break;
                    case 4:
                        //乡镇
                        if (lihtml == "") lihtml = '<li data-zb="" data-areas="" data-adcd="">数据收集中....</li>';
                        var newlihtml = '<li style="width:90%" data-type="returnLast">当前位置:<a href="javascript:void(0)">' + name + '</a><a href="javascript:void(0)" style="float:right;"><<返回</a></li>' + lihtml;
                        var html = '<ul class="areas_zhen">' + newlihtml + '</ul>';
                        $(".searchArealist ul.areas_quxian").after(html);
                        //事件
                        $(".areas_zhen li").click(function () {
                            var datatype = $(this).attr("data-type");
                            var adcd = $(this).attr("data-adcd");
                            if (datatype === "returnLast") {
                                $(this).parent().remove();
                                $(".areas_quxian").show();
                                return;
                            }
                            // $(".areas_shi").show();
                            var name = $(this).text();
                            $(".search-areas").attr("data-now", name);
                            $(".search-areas a").text(name);

                            var adcd = $(this).attr("data-adcd");

                            var y = parseFloat($(this).attr('data-zby'));//.toFixed(5);
                            var x = parseFloat($(this).attr('data-zbx'));//.toFixed(5);
                            var oldy = y;
                            y = parseFloat(parseFloat(y) - 0.04532).toFixed(5);
                            // $(".areas_zhen").remove();
                            // $(".areas_quxian").remove();
                            common.setMapPosition(x, y, 13, adcd, 3, name, oldy);
                        });
                        break;
                }
            }
        });
    },
    //通过登录的ADCD获取信息
    getAreaNameBySession: function (adcd) {
        var lihtml = "";
        $.ajax({
            type: "get",
            url: "/api/ZZTX/GetADCDInfoBySession",
            data: { "adcd": adcd },
            dataType: "json",
            success: function (data) {
                var data = eval(data);
                $.each(data, function (i, item) {
                    lihtml += '<li data-zbx="' + item.lat + '" data-zby="' + item.lng + '" data-areas="' + item.adnm + '" data-adcd="' + item.adcd + '">' + item.adnm + '</li>';
                });

                $(".searchArealist").empty();
                //市
                var html = '<ul class="areas_shi">' + lihtml + '</ul>';
                $(".searchArealist").append(html);
                //事件
                $(".areas_shi li").click(function () {
                    var name = $(this).text();
                    $(".search-areas a").text(name);
                    var adcd = $(this).attr("data-adcd");
                    common.getQuXianInfo(adcd, name);
                    $(this).addClass("selected");
                    $(this).siblings('li').removeClass("selected");

                    var x = $(this).attr('data-zbx');
                    var y = $(this).attr('data-zby');
                    common.setMapPosition(x, y, 10, adcd, 1, name);
                    $(this).parent().hide();
                });
            },
            error: function () {
                layer.alert("错误");
            }
        });
    },
    getAreaName2: function (levels, adcd, name) {
        var lihtml = "";
        
        $.ajax({
            type: "get",
            url: "/api/ZZTX/GetADCDInfoForCounty",
            data: { adcd: adcd, levle: levels, PageSize: 999 },
            dataType: "json",
            success: function (data) {
                if (data !== "" && null != data) {
                    var data = eval(data.rows);
                    $.each(data, function (i, item) {
                        lihtml += '<li data-zbx="' + item.lat + '" data-zby="' + item.lng + '" data-areas="' + item.adnm + '" data-adcd="' + item.adcd + '">' + item.adnm + '</li>';
                    });
                }
                switch (levels) {
                    case 3:
                        $(".searchArealist").empty();
                        //市
                        var html = '<ul class="areas_quxian">' + lihtml + '</ul>';
                        $(".searchArealist").append(html);
                        //事件
                        $(".areas_quxian li").click(function () {
                            var name = $(this).text();
                            $(".search-areas a").text(name);
                            var adcd = $(this).attr("data-adcd");
                            common.getAreaName2(4, adcd, name);
                            $(this).addClass("selected");
                            $(this).siblings('li').removeClass("selected");

                            var x = $(this).attr('data-zbx');
                            var y = $(this).attr('data-zby');
                            common.setMapPosition(x, y, 10, adcd, 2, name);
                            $(this).parent().hide();
                        });
                        break;

                    case 4:
                        //乡镇
                        if (lihtml === "") lihtml = '<li data-zb="" data-areas="" data-adcd="">数据收集中....</li>';
                        var newlihtml = '<li style="width:90%" data-type="returnLast">当前位置:<a href="javascript:void(0)">' + name + '</a><a href="javascript:void(0)" style="float:right;"><<返回</a></li>' + lihtml;
                        var html = '<ul class="areas_zhen">' + newlihtml + '</ul>';
                        $(".searchArealist ul.areas_quxian").after(html);
                        //事件
                        $(".areas_zhen li").click(function () {
                            var datatype = $(this).attr("data-type");
                            var adcd = $(this).attr("data-adcd");
                            if (datatype === "returnLast") {
                                $(this).parent().remove();
                                $(".areas_quxian").show();
                                return;
                            }
                            // $(".areas_shi").show();
                            var name = $(this).text();
                            $(".search-areas").attr("data-now", name);
                            $(".search-areas a").text(name);

                            var adcd = $(this).attr("data-adcd");

                            var y = parseFloat($(this).attr('data-zby'));//.toFixed(5);
                            var x = parseFloat($(this).attr('data-zbx'));//.toFixed(5);
                            var oldy = y;
                            y = parseFloat(parseFloat(y) - 0.04532).toFixed(5);
                            // $(".areas_zhen").remove();
                            // $(".areas_quxian").remove();
                            common.setMapPosition(x, y, 13, adcd, 3, name, oldy);
                        });
                        break;
                }
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
    //村 防汛防台工作组
    CunPersonLiable: function (adcd) {
        $.ajax({
            url: "/api/VillageWorkingGroup/GetGroupOne",
            data: { adcd: adcd, year: globalYear, PageSize: 200 },
            type: "GET",
            dataType: "json",
            success: function (data) {
                if (data.total <= 0) {
                    $("#cun_Group").html("暂无数据");
                    return;
                }
                var rows = data.rows;
                var BigGroup = ""; var SmallGroup = "";
                $.each(rows, function (i, item) {
                    if (item.postid === 1 || item.postid === 2) {
                        BigGroup += '<div class="gzz"><span>' + item.post + '</span>';
                        var member = "";
                        $.each(item.datas, function (j, bitem) {
                            member += "<div>" + bitem.name + "";
                            if (bitem.position !== "" && bitem.position != null) { member += "(" + bitem.position + ")"; }
                            member += " " + bitem.mobile + "</div>";
                        });
                        BigGroup += member + '</div>';
                    }
                    else {
                        SmallGroup += '<table><tr><td class="subtitle" width="100">' + item.post + '</td><td style="padding:10px;">';
                        $.each(item.datas, function (j, sitem) {
                            SmallGroup += "" + sitem.name + "";
                            if (sitem.position !== "" && sitem.position != null) { SmallGroup += "(" + sitem.position + ")"; }
                            SmallGroup += " " + sitem.mobile + "; ";
                        });
                        SmallGroup = SmallGroup.substr(0, SmallGroup.length - 1);
                        SmallGroup += '</td></tr></table>';
                    }
                    var bg = '';
                    if (BigGroup !== "") {
                        bg += '<div class="gzz-t" id="cun-zrr-BigGroup">' + BigGroup + '</div>';
                    }
                    if (SmallGroup !== "") { bg += '<div class="gzz-con" id="cun-zrr-SmallGroup">' + SmallGroup + '</div>'; }
                    $("#cun_Group").html(bg === "" ? "暂无数据" : bg);
                });
            }
        });
    },
    //村 APP人员列表
    CunPersonLiableList: function (adcd, villagename) {
        $.ajax({
            url: "/api/CApp/CCKHVillageApp",
            data: { adcd: adcd, year: globalYear, PageSize: 999 },
            type: "GET",
            dataType: "json",
            beforeSend: function () {
                $("#cun_AppList").html("<img src='/Content/ComprehensiveApp/images/loading.gif' width='400' height='400' >");
            },
            success: function (data) {
                if (data == null || data == "") {
                    $("#cun_AppList").html("<tr><td colspan='4'>暂无数据</td></tr>");
                    return;
                }
                var rows = eval(data);
                var html = "";
                $("#FrameCunInfoApp .cunname").html(rows[0].adnmparent + "_" + villagename);
                $.each(rows, function (i, item) {
                    var postlists = item.apppost.split(',');
                    html += '<tr class="apptable" data-lz="' + item.checkresult + '" data-post="' + item.apppost + '" data-time="' + item.addtime + '" data-mobile="' + item.handPhone + '" data-adcd="' + item.adcd + '">';
                    //html += '<td class="subtitle" width="50">' + (i + 1) + '</td>';
                    html += '<td style="padding:10px; width:80px;vertical-align:middle;">' + item.personLiable + '</td>';
                    html += '<td style="padding:10px; width:80px;">';
                    for (var j = 0; j < postlists.length; j++) {
                        html += '' + postlists[j] + '<br>';
                    }
                    html += '</td>';
                    html += '<td style="padding:10px; width:80px;vertical-align:middle;" >' + item.handPhone + '</td>';
                    html += '<td style="padding:10px; width:80px;vertical-align:middle;" >' + item.addtime + '</td>';
                    //html += '<td style="padding:10px; width:80px;vertical-align:middle;" >';
                    //if (item.checkresult == "1") {
                    //    html += ' 已履职';
                    //}
                    //html += '</td>';
                    html += '</tr>';
                    $("#cun_AppList").html(html);
                });
                $("#cun_AppList .apptable").click(function () {
                    if ($(this).attr("data-lz") == "1") {
                        var adcd = $(this).attr("data-adcd");
                        var post = $(this).attr("data-post");
                        var mobile = $(this).attr("data-mobile");
                        //
                        //openModal(rows[0].adnmparent + "_" + $("#FrameCunInfoApp").attr("data-name"), '/ComprehensiveApp/AppIndex?adcd=' + adcd + '&post=' + post + '&mobile=' + mobile + '&name=' + rows[0].adnmparent + '_'+$("#FrameCunInfoApp").attr("data-name")+'&lng=' + $("#FrameCunInfoApp").attr("data-lng") + '&lat=' + $("#FrameCunInfoApp").attr("data-lat") + '', { width: 800 });
                        common.openModalIframe(rows[0].adnmparent + "_" + villagename, '/ComprehensiveApp/AppIndex?adcd=' + adcd + '&stime=' + $(this).attr("data-time") + '&post=' + post + '&mobile=' + mobile + '&name=' + villagename + '&lng=' + $("#FrameCunInfo").attr("data-lng") + '&lat=' + $("#FrameCunInfo").attr("data-lat") + '', ["1000px", "" + $(window).height() + "px"]);
                    }
                });
            }
        });
    },
    //村 网格责任人
    CunWangGE: function (adcd) {
        $.ajax({
            url: "/api/VillageGrid/GetVillageGrid",
            data: { adcd: adcd, year: globalYear, PageSize: 200 },
            type: "GET",
            dataType: "json",
            success: function (data) {
                if (data.total <= 0) {
                    $("#cun_WangGe").html("暂无数据");
                    return;
                }
                var rows = data.rows;
                var html = '';
                $.each(data.rows, function (i, item) {
                    html += '<div class="zrr-box"><span class="left-t">' + item.villageGridName + '';
                    html += '(' + item.gridName + ')<br/><b>' + item.position + '</b></span>';
                    html += '<span>责任人：' + item.personLiable + '</span><span>手机：' + item.handPhone + '</span></div>';
                });
                $("#cun_WangGe").html(html);
            }
        });
    },
    //村 人员转移清单
    CunRemoveList: function (adcd) {
        $.ajax({
            url: "/api/VillageTransferPerson/GetVillageTransferPerson1",
            data: { adcd: adcd, year: globalYear, PageSize: 200 },
            type: "GET",
            dataType: "json",
            success: function (data) {
                if (data.total <= 0) {
                    $("#cun_RemoveList").html("暂无数据");
                    return;
                }
                var rows = data.rows;
                var html = '';
                $.each(rows, function (i, item) {
                    if (item.ifTransfer == 1) {
                        html = "无可转移人员";
                        return;
                    }
                    html += '<table><tbody><tr>';
                    html += '<td width="100" rowspan="3" class="subtitle"><img src="/Content/Supervise/images/wg_' + item.dId + '.png" style="width:45px;" onerror="this.src=\'\'"><br>' + (item.dangerZoneType == null ? "-" : item.dangerZoneType) + '</td>';
                    html += '<td class="rtext">户主： ' + (item.householderName == null ? "-" : item.householderName) + ' ' + (item.householderMobile == null ? "-" : item.householderMobile) + '</td>';
                    html += '<td class="rtext">居住人数：' + (item.householderNum == null ? "-" : item.householderNum) + '人</td>';
                    html += '<td>&nbsp;</td>';
                    html += '</tr>';
                    html += '<tr>';
                    html += ' <td class="rtext">转移责任人：' + (item.personLiableName == null ? "-" : item.personLiableName) + ' ' + (item.personLiableMobile == null ? "-" : item.personLiableMobile) + '</td>';
                    html += '<td class="rtext">预警责任人：' + (item.warnPersonLiableName == null ? "-" : item.warnPersonLiableName) + ' ' + (item.warnPersonLiableMobile == null ? "-" : item.warnPersonLiableMobile) + '</td>';
                    html += ' <td>&nbsp;</td>';
                    html += '</tr>';
                    html += '<tr>';
                    html += '<td class="rtext">避灾场所：' + (item.disasterPreventionName == null ? "-" : item.disasterPreventionName) + '</td>';
                    html += '<td class="rtext">避灾场所管理员：' + (item.disasterPreventionManager == null ? "-" : item.disasterPreventionManager) + ' ' + (item.disasterPreventionManagerMobile == null ? "-" : item.disasterPreventionManagerMobile) + '</td>';
                    html += '<td class="rtext">有无安全鉴定：' + (item.safetyIdentification ? "有" : "无") + '</td>';
                    html += '</tr>';
                    html += '</tbody></table>';
                });
                $("#cun_RemoveList").html(html);
            }
        });
    },
    //村 防汛防台形势图
    CunPic: function (adcd) {
        
        $.ajax({
            url: '/api/VillagePic/GetVillagePicByAdcdAndYear',
            type: "Get",
            data: { "adcd": adcd, year: globalYear },
            dataType: "json",
            success: function (data) {
                if (data) {
                    if (data.path != "") {
                        var files = data.path.split('|');
                        var html = '', show = "";
                        for (var i = 0; i < files.length; i++) {
                            var pic = JSON.parse(files[i]).fileSrc;
                            //if (i == 0) {
                            //    $("#lightgallery a:first").attr("data-responsive", pic).attr("data-src", pic);
                            //    $("#lightgallery img:first").attr("src", pic);
                            //} else {
                            //   html += '<a data-responsive="' + pic + '" data-src="' + pic + '" data-sub-html="" style="margin:15px; display:block">'
                            // + '<img src="' + pic + '" style="width:800px; height:524px;' + show + '" onerror="this.src=\'\'" /></a>';
                            // }
                            // html += pic +'|';
                            html += '<a href="' + pic + '" data-size="1600x1067" data-med="' + pic + '" data-med-size="1024x683" style="margin-bottom:15px; display:block" data-author=""><img src="' + pic + '" alt="" /><figure></figure></a>';
                        }
                        //var pics = html.substring(0, html.length - 1);
                        //$("#mypic").val(pics);
                        $("#demo-test-gallery").html(html);
                    }
                } else {
                    $("#demo-test-gallery").html("暂无图片");
                }
            }
        });
    },
    //村 概括
    CunInfo: function (adcd) {
        $("#loadingDiv").show();
        $("#FrameTownInfo").hide();
        //取数据
        $.ajax({
            type: "post",
            url: "/api/CApp/GetCunDot",
            data: { adcd: adcd, year: globalYear },
            dataType: "json",
            success: function (data) {
                var item = eval(data);
                if (null != item && item != "") {
                    /*****/
                    //地图定位
                    if (_cunOneDotLayser != null) _cunOneDotLayser.clearLayers();
                    var y = item.lgt;
                    var x = item.ltt;
                    if (!common.XYVerification(x, y)) { alert("坐标信息异常！"); return; }
                    //var point = L.marker([x, y], {
                    //    icon: L.icon({
                    //        iconUrl: "/Content/ComprehensiveApp/Images/cun.png",
                    //        iconAnchor: new L.Point(12, 12),
                    //        iconSize: new L.Point(24, 24),
                    //        className: 'leaflet-divlabel'
                    //    }),
                    //    riseOnHover: true
                    //}).bindLabel(item.name, { direction: 'right', noHide: false });//.bindPopup("<img src='content/images/w.png' style='width:400px;height:200px' align='middle'/><br><span style='font-size: 16px;'></span>");
                    var point = L.marker([x, y], {
                        icon: L.icon({
                            iconUrl: "/Images/cun.png",
                            iconAnchor: new L.Point(12, 12)
                            // iconSize: new L.Point(18, 18)
                        }),
                        riseOnHover: true
                    }).bindLabel('<div name="marker" class="map_mark"><div class="map_mark_inner"><span class="map_mark_name">' + item.name + ' </span> </div></div>', { direction: 'right', noHide: false });//.bindPopup("");
                    point.on("click", function (e) {
                        common.CunInfo1(item);
                    });
                    _cunOneDotLayser.addLayer(point);
                    //重新定位中心点
                    y = parseFloat(parseFloat(y) - 0.002).toFixed(3);
                    _map.setView([x, y], 18);
                    /******/
                    common.CunInfo1(item);
                }
            }
        });
    },
    CunInfo1: function (item) {
        //概况
        $("#FrameCunInfo").attr("data-adcd", item.adcd);
        $("#cun_gk_Mans").text(item.allManNums);
        $("#cun_gk_DisaterPoint").text(item.disasterPoint);
        $("#cun_gk_DisaterMans").text(item.disasterManNums);
        //
        $("#cun_gk_VillageMans").text(item.villagePersonNums);
        $("#cun_gk_VillageInPostMans").text(item.villageInPostNums);
        //
        $("#FrameCunInfo .cunname").html(item.shiName + " " + item.zhenName + " " + item.name);
        $("#FrameCunInfo").attr("data-name", item.name);
        $("#FrameCunInfo").attr("data-lat", item.ltt);
        $("#FrameCunInfo").attr("data-lng", item.lgt);
        if (item.qrpath != "") $("#qrcode").html("<span><img src=\"" + item.qrpath + "\" height=\"70\" /></span>");
        var hwg = "<p></p>";
        if (null != item.rows && item.rows != "") {
            $.each(item.rows, function (i, item) {
                if (item.wanggeCount > 0) hwg += '<div>' + item.wanggeName + '<br/><span>' + item.wanggeCount + '</span> </div>';
            });
        }
        $("#cun_gk_WangGeNums").html(hwg);
        common.tabInitCun();
        $("#FrameCunInfo").show(100);
    },
    //镇 概括
    TownInfo: function (adcd, t) {
        $("#loadingDiv").show();
        var html = "";
        $.ajax({
            type: "post",
            url: "/api/CApp/GetTownInfo",
            data: { "adcd": adcd },
            dataType: "json",
            success: function (data) {
                
                var data = eval(data);
               //alert(data[0]);
                if (null != data && data != "") {
                    // alert(data[0].shiName + data[0].zhenName);
                    $("#FrameTownInfo").attr("data-adcd", adcd);
                    $("#FrameTownInfo .townname").html(data[0].shiName + " " + data[0].zhenName);
                    $("#zhen_cun_all").text(data[0].cuncount);
                    //
                    html += '<ul>';
                    $.each(data[0].cuns, function (i, item) {
                        if (i < 49) {
                            html += '<li><a href="javascript:void(0);" data-adcd="' + item.adcd + '">' + item.adnm + '</a></li>';
                        }
                        if (i == 49) {
                            html += '<li class="limore">更多</li>';
                        }
                        if (i > 49) {
                            html += '<li class="lihide"><a href="javascript:void(0);" data-adcd="' + item.adcd + '">' + item.adnm + '</a></li>';
                        }
                    });
                    html += '<li class="lisq">收起</li></ul>';
                    $("#zhen_cun_list").html(html);
                    $("#zhen_szd").text(data[0].zdPoint);
                    $("#zhen_yxr").text(data[0].zdManNums);
                    $("#zhen_benji").text(data[0].zhenBenji);
                    $("#zhen_xzc").text(data[0].cunBenji);
                    //
                    var hwg = "<p></p>";
                    $.each(data[0].rows, function (i, item) {
                        if (item.wanggeCount > 0) hwg += '<div>' + item.wanggeName + '<br/><span>' + item.wanggeCount + '</span> </div>';
                    });
                    $("#zhen_wangge_list").html(hwg);
                    common.tabInitTown();
                    $("#FrameTownInfo").attr("style", "");
                    $("#FrameTownInfo a#RTT").removeClass("townsh").addClass("townreturn");
                    $("#FrameTownInfo").show(100);
                    if (t) {
                        
                        var y = parseFloat(data[0].ltt);//.toFixed(5);
                        var x = parseFloat(data[0].lgt);//.toFixed(5);
                        var oldy = y;
                        y = parseFloat(parseFloat(y) - 0.11260).toFixed(5);
                        common.setMapPosition(x, y, 12, adcd, 3, data[0].zhenName, oldy);
                    }
                    
                }
                $("#zhen_cun_list li.limore").click(function () {
                    $(this).nextAll().removeClass("lihide").addClass("lishow");
                    $(this).hide();
                    $(this).siblings(".lisq").show();
                });
                $("#zhen_cun_list li.lisq").click(function () {
                    $(this).siblings(".lishow").removeClass("lishow").addClass("lihide");
                    $(this).siblings(".limore").show();
                    $(this).hide();
                });
                $("#zhen_cun_list a").click(function () {
                    var adcd = $(this).attr("data-adcd");
                    common.CunInfo(adcd);
                });
            }
        });
        /**********************/
    },
    CountyInfo: function (adcd, t) {
        $("#loadingDiv").show();
        var html = "";
        
        $.ajax({
            type: "post",
            url: "/api/CApp/GetCountyInfo",
            data: { adcd: adcd },
            dataType: "json",
            success: function (data) {
                var data = eval(data);
                if (null != data && data != "") {
                    $("#FrameCountyInfo").attr("data-adcd", adcd);
                    $("#FrameCountyInfo .townname").html(data[0].cityName + " " + data[0].countyName);//哪个市哪个县
                    $("#towncount").text(data[0].townCount);//这里是有多少个镇
                    //
                    html += '<ul>';
                    $.each(data[0].towns, function (i, item) {
                        if (i < 49) {
                            html += '<li><a href="javascript:void(0);" data-adcd="' + item.adcd + '">' + item.adnm + '</a></li>';
                        }
                        if (i == 49) {
                            html += '<li class="limore">更多</li>';
                        }
                        if (i > 49) {
                            html += '<li class="lihide"><a href="javascript:void(0);" data-adcd="' + item.adcd + '">' + item.adnm + '</a></li>';
                        }
                    });
                    html += '<li class="lisq">收起</li></ul>';

                    $("#County_Town_list").html(html);
                    $("#CountyDangerPoint").text(data[0].zdPoint);
                    $("#CountyAffect").text(data[0].zdManNums);
                    $("#CountyLeader").text(data[0].countyBenJi);
                    $("#TownLeader").text(data[0].zhenBenji);
                    //
                    var hwg = "<p></p>";
                    $.each(data[0].rows, function (i, item) {
                        if (item.wanggeCount > 0) hwg += '<div>' + item.wanggeName + '<br/><span>' + item.wanggeCount + '</span> </div>';
                    });
                    $("#county_wangge_list").html(hwg);
                    
                    common.tabInitCounty();

                    $("#FrameCountyInfo").attr("style", "");
                    $("#FrameCountyInfo a#RTT2").removeClass("townsh").addClass("townreturn");
                    $("#FrameCountyInfo").show(100);
                    
                    if (t) {
                        var y = parseFloat(data[0].ltt);//.toFixed(5);
                        var x = parseFloat(data[0].lgt);//.toFixed(5);
                        var oldy = y;
                        y = parseFloat(parseFloat(y) - 0.11260).toFixed(5);
                        common.setMapPosition(x, y, 12, adcd, 2, data[0].zhenName, oldy);
                    }
                }
                $("#County_Town_list li.limore").click(function () {
                    $(this).nextAll().removeClass("lihide").addClass("lishow");
                    $(this).hide();
                    $(this).siblings(".lisq").show();
                });
                $("#County_Town_list li.lisq").click(function () {
                    $(this).siblings(".lishow").removeClass("lishow").addClass("lihide");
                    $(this).siblings(".limore").show();
                    $(this).hide();
                });
                $("#County_Town_list a").click(function () {
                    var adcd = $(this).attr("data-adcd");
                    
                    common.TownInfo(adcd,1);
                    $("#FrameCountyInfo").hide();
                });
            }
        });
        /**********************/
    },
    //镇 防指成员
    TownPersonLiable: function (adcd) {
        var html = ""; var htmls = "";
        $.ajax({
            type: "post",
            url: "/api/CApp/GetTownPerson",
            data: { adcd: adcd, year: globalYear },
            dataType: "json",
            success: function (data) {
                var data = eval(data);
                if (null != data && data != "") {
                    $.each(data, function (i, item) {
                        switch (item.gwid) {
                            case 1:
                                html += '<table><tr><td class="subtitle" width="100"><img src="/Content/ComprehensiveApp/images/zhico.png"/>' + item.gwName + '</td>';
                                $.each(item.datas, function (i, item1) {
                                    html += '<td style="padding:10px;">' + item1.name + ' ' + item1.mobile + ' </td>';
                                });
                                html += '</tr></table>';
                                break;
                            case 2:
                                html += '<table><tr><td class="subtitle" width="100"><img src="/Content/ComprehensiveApp/images/fzhico.png"/>' + item.gwName + '</td><td style="padding:10px;">';
                                $.each(item.datas, function (i, item1) {
                                    html += '' + item1.name + ' ' + item1.mobile + '; ';
                                });
                                html += '</td></tr></table>';
                                break;
                            case 3:
                                html += '<table><tr><td class="subtitle" width="100"><img src="/Content/ComprehensiveApp/images/cyico.png"/><br>' + item.gwName + '</td><td style="padding:10px;">';
                                $.each(item.datas, function (i, item1) {
                                    html += '' + item1.name + '; ';
                                });
                                html += '</td></tr></table>';
                                break;
                            default:
                                htmls += '<table><tr>';
                                htmls += '<td class="subtitle" width="100">' + item.gwName + '</td><td style="padding:10px;">';
                                $.each(item.datas, function (i, item2) {
                                    htmls += '' + item2.name + ' ' + item2.mobile + '; ';
                                });
                                htmls += '</td></tr></table>';
                                break;
                        }
                    });
                    $("#zhen_fzcy_Big").html(html);
                    $("#zhen_fzcy_Small").html(htmls);
                }
            }
        });
    },
    //镇 网格责任人
    TownWangGeZRR: function (adcd) {
        var html = "";
        $.ajax({
            type: "post",
            url: "/api/CApp/GetTownGridMan",
            data: { ADCD: adcd },
            dataType: "json",
            success: function (data) {
                var data = eval(data);
                if (null != data && data != "") {
                    $.each(data, function (i, item) {
                        html += '<div class="zrr-box">';
                        html += '<span class="left-tc" data-adcd="' + item.cunADCD + '">' + item.cunName + '</span>';
                        $.each(item.rows, function (j, item1) {
                            html += ' <span>' + item1.post + '：' + item1.personLiable + '';
                            if (item1.position != "" && item1.position != null) html += '（' + item1.position + '）';
                            html += ' ' + item1.handPhone + '</span>';
                        });
                        html += '</div>';
                    });
                    $("#zhen_wanggezrr").html(html);
                }
                $("#zhen_wanggezrr span.left-tc").click(function () {
                    var adcd = $(this).attr("data-adcd");
                    common.CunInfo(adcd);
                });
            }
        });
    },
    //县 防指成员
    CountyLeaderInfo:function(adcd) {
        $.ajax({
            url: "/api/CountryPerson/GetCountryPersonList1",
            data: { adcd: adcd, year: globalYear, PageSize: 999 },
            type: "GET",
            dataType: "json",
            beforeSend: function () {
                //$("#cun_AppList").html("<img src='/Content/ComprehensiveApp/images/loading.gif' width='400' height='400' >");
            },
            success: function (data) {
                //if (data == null || data == "") {
                //    $("#cun_AppList").html("<tr><td colspan='4'>暂无数据</td></tr>");
                //    return;
                //}
                var rows = eval(data);
                //alert(JSON.stringify(rows));
                //alert(JSON.stringify(r.rows[0].id));
                var html = "";
                var htmls = "";
                var zhihui = "";
                var fuzhihui = "";
                var chengyuan = "";
                var zhonghezu = "";

                var jianceyujing = "";
                var renyuanzhuanyi = "";
                var qiangxianjiuyuan = "";
                var xuanchuanbaodao = "";
                var houqing = "";



                $.each(rows, function (i, item) {
                    //alert(i);
                    //alert(item);
                    if (i === "rows") {
                        var json = eval(item);
                        //alert(JSON.stringify(json));
                        zhihui = '<table><tr><td class="subtitle" width="100px"><img src="/Content/ComprehensiveApp/images/zhico.png"/>指挥</td>';
                        fuzhihui = '<table><tr><td class="subtitle" width="100px"><img src="/Content/ComprehensiveApp/images/fzhico.png"/>副指挥</td><td style="padding:10px;">';
                        chengyuan = '<table><tr><td class="subtitle" width="100px"><img src="/Content/ComprehensiveApp/images/cyico.png"/><br>成员</td><td style="padding:10px;">';
                        zhonghezu = '<table><tr><td class="subtitle" width="100px">综合组</td><td style="padding:10px;">';
                        jianceyujing = '<table><tr><td class="subtitle" width="100px">监测预警组</td><td style="padding:10px;">';
                        renyuanzhuanyi = '<table><tr><td class="subtitle" width="100px">人员转移组</td><td style="padding:10px;">';
                        qiangxianjiuyuan = '<table><tr><td class="subtitle" width="100px">抢险救援组</td><td style="padding:10px;">';
                        xuanchuanbaodao = '<table><tr><td class="subtitle" width="100px">宣传报道组</td><td style="padding:10px;">';
                        houqing = '<table><tr><td class="subtitle" width="100px">后勤保障组</td><td style="padding:10px;">';

                        $.each(json, function (i, item) {
                            //alert(item.gwid);
                            switch (item.gwid) {
                            case 1:
                                
                                zhihui += '<td style="padding:10px;">' + item.userName + ' ' + item.phone + ' </td>';
                                break;
                            case 2:
                                
                                fuzhihui += '' + item.userName + ' ' + item.phone + '; ';
                                
                                break;
                            case 3:
                                
                                chengyuan += '' + item.userName + ' ' + item.phone + '; ';
                               
                                break;
                            case 4:

                                    zhonghezu += '' + item.userName + ' ' + item.phone + '; ';

                                break;
                            case 5:

                                    jianceyujing += '' + item.userName + ' ' + item.phone + '; ';

                                break;
                            case 6:

                                    renyuanzhuanyi += '' + item.userName + ' ' + item.phone + '; ';

                                break;
                            case 7:

                                    qiangxianjiuyuan += '' + item.userName + ' ' + item.phone + '; ';

                                break;
                            case 8:

                                    xuanchuanbaodao += '' + item.userName + ' ' + item.phone + '; ';

                                break;
                            case 9:

                                    houqing += '' + item.userName + ' ' + item.phone + '; ';

                                break;
                            default:
                                  
                                break;
                            }

                        });
                        zhihui += '</tr></table>';
                        fuzhihui += '</td></tr></table>';
                        chengyuan += '</td></tr></table>';
                        zhonghezu += '</td></tr></table>';
                        jianceyujing += '</td></tr></table>';
                        renyuanzhuanyi += '</td></tr></table>';
                        qiangxianjiuyuan += '</td></tr></table>';
                        xuanchuanbaodao += '</td></tr></table>';
                        houqing += '</td></tr></table>';
                        //alert(jianceyujing);
                    }

                    
                    $("#county_fzcy_Big").html(zhihui + fuzhihui + chengyuan);

                    $("#county_fzcy_Small").html(zhonghezu+jianceyujing + renyuanzhuanyi + qiangxianjiuyuan + xuanchuanbaodao + houqing);
                    //layer.alert(html);
                    //var postlists = item.post.split('；');

                    //html += '<tr class="apptable" data-lz="' + item.checkresult + '" data-post="' + item.post + '"  data-mobile="' + item.phone + '" data-adcd="' + item.adcd + '">';
                    //html += '<td class="subtitle" width="50">' + (i + 1) + '</td>';
                    //html += '<td style="padding:10px; width:80px;vertical-align:middle;">' + item.personLiable + '</td>';
                    //html += '<td style="padding:10px; width:80px;">';
                    //for (var j = 0; j < postlists.length; j++) {
                    //    html += '' + postlists[j] + '<br>';
                    //}
                    //html += '</td>';
                    //html += '<td style="padding:10px; width:80px;vertical-align:middle;" >' + item.phone + '</td>';
                    ////html += '<td style="padding:10px; width:80px;vertical-align:middle;" >' + item.addtime + '</td>';
                    ////html += '<td style="padding:10px; width:80px;vertical-align:middle;" >';
                    ////if (item.checkresult == "1") {
                    ////    html += ' 已履职';
                    ////}
                    ////html += '</td>';
                    //html += '</tr>';
                   
                });
                //$("#cun_AppList .apptable").click(function () {
                //    if ($(this).attr("data-lz") == "1") {
                //        var adcd = $(this).attr("data-adcd");
                //        var post = $(this).attr("data-post");
                //        var mobile = $(this).attr("data-mobile");
                //        //
                //        //openModal(rows[0].adnmparent + "_" + $("#FrameCunInfoApp").attr("data-name"), '/ComprehensiveApp/AppIndex?adcd=' + adcd + '&post=' + post + '&mobile=' + mobile + '&name=' + rows[0].adnmparent + '_'+$("#FrameCunInfoApp").attr("data-name")+'&lng=' + $("#FrameCunInfoApp").attr("data-lng") + '&lat=' + $("#FrameCunInfoApp").attr("data-lat") + '', { width: 800 });
                //        common.openModalIframe(rows[0].adnmparent + "_" + villagename, '/ComprehensiveApp/AppIndex?adcd=' + adcd + '&stime=' + $(this).attr("data-time") + '&post=' + post + '&mobile=' + mobile + '&name=' + villagename + '&lng=' + $("#FrameCunInfo").attr("data-lng") + '&lat=' + $("#FrameCunInfo").attr("data-lat") + '', ["1000px", "" + $(window).height() + "px"]);
                //    }
                //});
            }
        });
    },
    //责任人信息
    RenInfo: function (adcd, areas, name, mobile, post) {
        $("#manName").html(name);
        $("#manAreas").html(areas);
        $("#manGW").html(post);
        $("#manHandPhone").html(mobile);
        $("#FrameZeRenRenInfo").show();
    },
    //地图市、区县、镇定位
    setMapPosition: function (x, y, level, adcd, type, name, oldy) {
        if (name === "市") return;
        //加载本地的wmts切片，其上有流域、河流等信息
        var localTileLayer = new L.tileLayer.customArcGISServerLayer("http://114.215.170.1:8085/XJ/", { minZoom: 9, maxZoom: 16 });
        localTileLayer.id = "arcgistile";
        _map.addLayer(localTileLayer);
        if (type === 3) {
            common.getMapDot(adcd);
        }
        var imgurl = "";
        switch (type) {
            case 1: imgurl = "/Content/ComprehensiveApp/Images/city.png"; break;
            case 2: imgurl = "/Content/ComprehensiveApp/Images/county.png"; break;
            case 3: imgurl = "/Content/ComprehensiveApp/Images/town.png"; break;
        }
        if (common.XYVerification(x, y)) {
            var newy = "";
            if (null != oldy && oldy != "") { newy = oldy; }
            else { newy = y; }
            var point = L.marker([x, newy], {
                icon: L.icon({
                    iconUrl: imgurl,
                    iconAnchor: new L.Point(12, 12)
                    // iconSize: new L.Point(18, 18)
                }),
                riseOnHover: true
            }).bindLabel('<div name="marker" class="map_mark"><div class="map_mark_inner"><span class="map_mark_name">' + name + ' </span> </div></div>', { direction: 'right', noHide: false });//.bindPopup("");
            if (type === 2) {
                common.CountyInfo(adcd, 0);
                point.on("click", function (e) {
                    common.CountyInfo(adcd, 0);
                });
            }
            if (type === 3) {
                common.TownInfo(adcd, 0);
                point.on("click", function (e) {
                    common.TownInfo(adcd, 0);
                });
            }
            _cunLayer.addLayer(point);
            _map.setView([x, y], level);
        } else {
            alert(name + "坐标数据异常！");
        }
    },
    getMapDot: function (adcd) {
        if (_cunLayer != null) _cunLayer.clearLayers();
        if (_cunOneDotLayser != null) _cunOneDotLayser.clearLayers();
        $.ajax({
            type: "get",
            url: "/api/ZZTX/GetADCDInfo",
            data: { adcd: adcd, levle: 51, PageSize: 999 },
            dataType: "json",
            success: function (data) {
                var data = eval(data.rows);
                if (null != data && data != "") {
                    $.each(data, function (i, item) {
                        if (common.XYVerification(item.lat, item.lng)) {
                            var point = L.marker([item.lat, item.lng], {
                                icon: L.icon({
                                    iconUrl: "/Images/cun.png",
                                    iconAnchor: new L.Point(12, 12)
                                    //iconSize: new L.Point(18, 18)
                                }),
                                riseOnHover: true
                            }).bindLabel('<div name="marker" class="map_mark"><div class="map_mark_inner"><span class="map_mark_name">' + item.adnm + ' </span> </div></div>', { direction: 'right', noHide: false });
                            //气泡
                            //point.bindPopup('<div class="PopupAlert">' +
                            //    '<div><b>' + item.adnm + '</b></div>' +
                            //    '<table>' +
                            //    '<tr><th>总人口：</th><td colspan=3>' + (item.totalNum || "-") + '</td></tr>' +
                            //    '<tr><th>影响人口：</th><td colspan=3>' + (item.populationNum || "-") + '</td></tr>' +
                            //    '<tr><th>受灾点：</th><td colspan=3>' + (item.checkContacts || "-") + '</td></tr>' +
                            //    '</table>' +
                            //    '</div>', { offset: [0, 7] });

                            point.on("click", function (e) {
                                common.CunInfo(item.adcd);
                            });
                            _cunLayer.addLayer(point);
                        } else {
                            alert("坐标信息异常"); return;
                        }
                    });
                }
            }
        });
    },

    hideFrameXingZhengChun: function () { $("#FrameXingZhengChun").hide(); },
    hidaDelKey: function () { $("#aDelKey").hide(); },
    showaDelKey: function () { $("#aDelKey").show(); },
    hideFrameSearchAll: function () { $("#FrameSearchAll").hide(); },
    showFrameSearchAll: function () { $("#FrameSearchAll").show(); },
    //初始化区域
    initArea: function () {
        showLocation();
        $('#loc_city').empty();
        $('#loc_town').empty();
        $('#loc_cun').empty();
        $("#s2id_loc_city span.select2-chosen").text("县级市");
        $("#s2id_loc_town span.select2-chosen").text("乡镇");
        $("#s2id_loc_cun span.select2-chosen").text("行政村");
    },
    excludeSpecial: function (s) {
        // 去掉转义字符
        s = s.replace(/[\'\"\\\/\b\f\n\r\t]/g, '');
        // 去掉特殊字符
        s = s.replace(/[\@\#\$\%\^\&\*\{\}\:\"\L\<\>\?]/);
        return s;
    },
    XYVerification: function (x, y) {
        if (x != "" && y != "" && x != null && y != null && x != 0 && y != 0 && x != "undefined" && y != "undefined" && x != 'NaN' && y != 'NaN') {
            return true;
        }
        else { return false; }
    },
    openModalIframe: function (title, url, parames) {
        layer.open({
            type: 2,
            title: title,
            shadeClose: true,
            shade: 0.8,
            fixed: false, //不固定
            maxmin: true,
            area: parames,
            content: url
        });
    },
    //App获取市
    getAppCity: function () {
        common.getAppAreaName(1, 31323);
    },
    getAppAreaName: function (_grade, _parentid) {
        var lihtml = "";
        $.ajax({
            type: "post",
            url: "/api/CApp/GetAppArea",
            data: { grade: _grade, parentid: _parentid },
            dataType: "json",
            success: function (data) {
                if (data != "" && null != data) {
                    var data = eval(data);
                    $.each(data, function (i, item) {
                        lihtml += '<div class="city panel-heading">';
                        lihtml += '<a data-toggle="collapse" data-v="0" data-parent="#accordion" href="#collapse' + item.id + '" data-zbx="' + item.lat + '" data-adcd="' + item.adcd + '" data-zby="' + item.lng + '">' + item.adnm + '<span>应到岗<i>(' + item.inperson + ')</i>实到岗<em>(' + item.noperson + ')</em></span></a></div>';
                        lihtml += '<div id="collapse' + item.id + '" class="county panel-collapse collapse">';
                        var datason = eval(item.sonList);
                        $.each(datason, function (j, itemson) {
                            lihtml += '<div class="areason" data-zbx="' + itemson.lat + '" data-adcd="' + itemson.adcd + '" data-zby="' + itemson.lng + '" data-grade="' + itemson.grade + '" data-pid="' + itemson.id + '">' + itemson.adnm + '<span>应到岗<i>(' + itemson.inperson + ')</i>实到岗<em>(' + itemson.noperson + ')</em></span></div>';
                        });
                        lihtml += '</div>';
                    });
                    $("#accordion").html(lihtml);
                    $(".panel-heading a").click(function () {
                        var pid = $(this).attr("data-pid");
                        var dv = $(this).attr("data-v");
                        var adcd = $(this).attr("data-adcd");
                        var name = $(this).text();
                        if (dv == 0) {
                            //设置是否在展开的时候获取
                            $(this).attr("data-v", 1);
                            //
                            var x = $(this).attr('data-zbx');
                            var y = $(this).attr('data-zby');
                            common.setMapPosition(x, y, 10, adcd, 1, name);
                        } else {
                            //收起是重新设置
                            $(this).attr("data-v", 0);
                        }
                    });
                    $(".areason").click(function () {
                        var x = $(this).attr('data-zbx');
                        var y = $(this).attr('data-zby');
                        var adcd = $(this).attr('data-adcd');
                        var name = $(this).text();
                        common.setMapPosition(x, y, 12, adcd, 2, name);
                        appAdcd = adcd;
                    });
                }
            }
        });
    }
}
common.init();