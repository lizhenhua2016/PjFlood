/**公共方法****/
var bycheckitems = [];
var additems = [];
var deleteitems = [];
var common1 = {
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
                else { }
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

                    if ($(".selected-box a").length == 1) common1.clearStatics();
                    if ($("input[name='optionscheckboxsinline']:checked").length == 0) {
                        common1.getStaticsData(level, adcd, 1, "", "");
                    } else {
                        $("input[name='optionscheckboxsinline']:checked").each(function () {
                            var mytype = $(this).attr("datatype");
                            var typenames = mytype.split('_');
                            common1.getStaticsData(level, adcd, 1, $(this).val(), typenames[0]);
                        });
                    }
                   
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
                    common1.getAreaName1(3, adcd);
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
                    common1.getStaticsData(3, "", 1, "", "");
                }
            }
            var level = $(this).attr("data-level");
            var adcd = $(this).attr("data-adcd");
            $(this).remove();

            //if ($("input[name='optionscheckboxsinline']:checked").length == 0) {
            //    common1.getStaticsData(level, adcd, -1, "", "");
            //} else {
            //    $("input[name='optionscheckboxsinline']:checked").each(function () {
            //        var mytype = $(this).attr("datatype");
            //        var typenames = mytype.split('_'); 
            //        common1.getStaticsData(level, adcd, -1, $(this).val(), typenames[0]);
            //    });
            //}
        });
        common1.getShiInfo();
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
                //var typeid = $(".selected-box a[data-level='9']").attr("data-type");
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

                    if ($(".selected-box a").length == 1) { common1.clearStatics(); }
                    if ($("input[name='optionscheckboxsinline']:checked").length == 0) {
                        common1.getStaticsData(level, adcd, 1, "", "");
                    } else {
                        $("input[name='optionscheckboxsinline']:checked").each(function () {
                            var mytype = $(this).attr("datatype");
                            var typenames = mytype.split('_');
                            common1.getStaticsData(level, adcd, 1, $(this).val(), typenames[0]);
                        });
                    }
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
                        common1.getAreaName1(4, adcd);
                    }
                }
            });
        }, function () {
            $(this).removeClass("regionon");
            $(this).find("img").remove();
        });
    },
    //统计--获取数据
    //第一次：level==adcd==t=1=typename==postlevel=
    //带类型不带区域：level==adcd==t=1=typename=管理员=postlevel=villageposition
    //先类型+区域：level=2=adcd=3303=t=1=typename=副指挥=postlevel=county
    //带区域不带类型：level=2=adcd=3306=t=1=typename==postlevel=
    //区域+类型：level=3=adcd=330324000000000=t=1=typename=驻村干部=postlevel=village

    getStaticsData: function (level, adcd, t, typename, postlevel) {
        $.ajax({
            url: '/api/StatisAnalysis/StatisticsByPost',
            type: 'post',
            data: { adcdtype: level, typename: typename, typelevel: postlevel, adcd: adcd, year: globalYear },
            dataType: 'json',
            beforeSend: function () {
                $(".selected-box").append("<a style=\" border:0; background:none;\" id=\"staticLoading\" href=\"javascript:void(0);\"><img src=\"/Content/ComprehensiveApp/images/loading.gif\" width=\"22\" height=\"22\" /></a>");
            },
            success: function (result) {
                if(typename != "" && adcd == ""){
                    //1、选择了岗位，根据客户权限显示获取所选岗位数据
                    common1.noAddStatics(result);
                } else if (adcd != "" && typename == ""){
                    //2、选择了区域，按区域统计所有并叠加显示所有岗位
                    common1.AddStatics(result, t);
                    if (t == 1) {
                        if (adcd == "3300" || (adcd != "3300" && $(".selected-box a").length == 1)) {
                            $(".countyperson").text(0);
                            $(".townperson").text(0);
                            $(".villageperson").text(0);
                        }
                        else if ($(".selected-box a").length == 2) {
                            $(".countyperson").text(0);
                            $(".townperson").text(0);
                            $(".villageperson").text(0);
                        } else { }
                    }
                    common1.personliable(result,t);
                }else if(typename != "" && adcd != ""){
                    //3、选择了岗位和区域，按区域统计并叠加显示选择的岗位
                    common1.AddStatics(result, t);
                    //if (t == 1) {
                    //    if (adcd == "3300" || (adcd != "3300" && $(".selected-box a").length == 1)) {
                    //        $(".countyperson").text(0);
                    //        $(".townperson").text(0);
                    //        $(".villageperson").text(0);
                    //    } else if ($(".selected-box a").length == 2) {
                    //        $(".countyperson").text(0);
                    //        $(".townperson").text(0);
                    //        $(".villageperson").text(0);
                    //    }else{}
                    //}
                } else if (typename == "" && adcd == "") {
                    //4、初次加载,既没有选择岗位，也没有选择区域，根据权限显示所有
                    common1.noAddStatics(result);
                    common1.personliable(result, 1);
                } else { }
              
                $(".selected-box").find("a#staticLoading").remove();
            },
            error: function () {
                $(".selected-box").find("a#staticLoading").remove();
            }
        });
    },
    noAddStatics: function (object) {
        var result = eval(object[0].disasterPoint);
        $.each(result, function (i, item) {
            switch (item.postlevel) {
                case "town":
                    $("#town li").each(function () {
                        if ($(this).find("p").text() == item.post) $(this).find("b").text(item.nums);
                    });
                    break;
                case "village":
                    $("#village li").each(function () {
                        if ($(this).find("p").text() == item.post) $(this).find("b").text(item.nums);
                    });
                    break;
                case "villageposition":
                    $("#village li").each(function () {
                        if ($(this).find("p").text() == item.post) $(this).find("b").text(item.nums);
                    });
                    break;
                case "county":
                    $("#county li").each(function () {
                        if ($(this).find("p").text() == item.post) $(this).find("b").text(item.nums);
                    });
                    break;
            }
        });
    },
    AddStatics: function (object,t) {
        var result = eval(object[0].disasterPoint);
        $.each(result, function (i, item) {
            var nums = item.nums;
            switch (item.postlevel) {
                case "town":
                    $("#town li").each(function () {
                        if ($(this).find("p").text() == item.post) {
                            var item1 = parseInt($(this).find("b").text());
                            if (t == 1) {
                                $(this).find("b").text(item1 + nums);
                            }
                            else if (t == -1) {
                                var b=item1 - nums;
                                $(this).find("b").text(b < 0 ? 0 : b);
                            } else { }
                        }
                    });
                    break;
                case "village":
                    $("#village li").each(function () {
                        if ($(this).find("p").text() == item.post) {
                            var item2 = parseInt($(this).find("b").text());
                            if (t == 1) {
                                $(this).find("b").text(item2 + nums);
                            }
                            else if (t == -1) {
                                var b = item2 - nums;
                                $(this).find("b").text(b < 0 ? 0 : b);
                            } else { }
                        }
                    });
                    break;
                case "villageposition":
                    $("#village li").each(function () {
                        if ($(this).find("p").text() == item.post) {
                            var item3 = parseInt($(this).find("b").text());
                            if (t == 1) {
                                $(this).find("b").text(item3 + nums);
                            }
                            else if (t == -1) {
                                var b = item3 - nums;
                                $(this).find("b").text(b < 0 ? 0 : b);
                            } else { }
                        }
                    });
                    break;
                case "county":
                    $("#county li").each(function () {
                        if ($(this).find("p").text() == item.post) {
                            var item4 = parseInt($(this).find("b").text());
                            if (t == 1) {
                                $(this).find("b").text(item4 + nums);
                            }
                            else if (t == -1) {
                                var b = item4 - nums;
                                $(this).find("b").text(b < 0 ? 0 : b);
                            } else { }
                        }
                    });
                    break;
            }
        });
    },
    personliable: function (result, t) {
        var countyPLNums = parseInt(result[0].countyPLNums);
        var countyperson = parseInt($(".countyperson").text());
        if (t == 1) {
            $(".countyperson").text(countyPLNums + countyperson);
        } else if (t == -1) {
            $(".countyperson").text(countyperson - countyPLNums < 0 ? 0 : countyperson - countyPLNums);
        } else { }

        var townPLNums = parseInt(result[0].townPLNums);
        var townperson = parseInt($(".townperson").text());
        if (t == 1) {
            $(".townperson").text(townperson + townPLNums);
        } else if (t == -1) {
            $(".townperson").text(townperson - townPLNums < 0 ? 0 : townperson - townPLNums);
        } else { }

        var villagePLNums = parseInt(result[0].villagePLNums);
        var villageperson = parseInt($(".villageperson").text());
        if (t == 1) {
            $(".villageperson").text(villageperson + villagePLNums);
        } else if (t == -1) {
            var r = (villageperson - villagePLNums) < 0 ? 0 : villageperson - villagePLNums;
            var acount = $(".selected-box a").length;
            if (acount == 0) r = 0;
            $(".villageperson").text(r);
        } else { }
    },
    clearStatics: function () {
        $("#town li b").text(0);
        $("#village li b").text(0);
        $("#county li b").text(0);
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
                        common1.countyCheckItems(3, "#stcounty .region-con");
                        break;
                    case 4:
                        //乡镇
                        $("#sttown").show();
                        $("#sttown .region-con").html(lihtml);
                        common1.countyCheckItems(4, "#sttown .region-con");
                        break;
                }
            }
        });
    },
    //三级联动 市
    getShiInfo: function () {
        common1.getAreaName(2, "");
    },
    //三级联动 县
    getQuXianInfo: function (adcd) {
        common1.getAreaName(3, adcd);
    },
    //三级联动 乡镇
    getZhenInfo: function (adcd) {
        // $(".searchArealist ul.areas_zhen").remove();
        common1.getAreaName(4, adcd);
    },
    getVillageInfo:function (adcd){
        common1.getAreaName(51, adcd);
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
                switch (levels) {
                    case 2:
                        //市 
                        var html = '<option value="-1">市</option>' + lihtml;
                        $("#City").html(html);
                        //事件
                        $("#City").change(function () {
                            var name = $(this).find("option:selected").text();
                            var adcd = $(this).find("option:selected").attr("data-adcd");
                            common1.getQuXianInfo(adcd);
                            $("#County").show();
                            var x = $(this).find("option:selected").attr('data-zbx');
                            var y = $(this).find("option:selected").attr('data-zby');
                           // common1.setMapPosition(x, y, 10, adcd, 1, name);
                        });
                        break;
                    case 3:
                        //县
                        var html = '<option value="-1">县(市、区)</option>' + lihtml;
                        $("#County").html(html);
                        //事件
                        $("#County").change(function () {
                            var name = $(this).find("option:selected").text();
                            var adcd = $(this).find("option:selected").attr("data-adcd");
                            common1.getZhenInfo(adcd);
                            $("#Town").show();
                            var x = $(this).find("option:selected").attr('data-zbx');
                            var y = $(this).find("option:selected").attr('data-zby');
                           // common1.setMapPosition(x, y, 12, adcd, 2, name);
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
                            common1.getVillageInfo(adcd);
                        });
                        break;
                    case 51:
                        //村
                        var html = '<option value="-1">行政村</option>' + lihtml;
                        $("#Village").html(html);
                        //事件
                        $("#Village").change(function () {
                            var name = $(this).find("option:selected").text();
                            var adcd = $(this).find("option:selected").attr("data-adcd");
                        });
                        break;
                }

            }
        });
    }
}
common1.init();