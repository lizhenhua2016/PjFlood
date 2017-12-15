/**公共方法****/

$(function(){
    $("body").attr("style", "background:#f9f9f9;");
    //加载统计
    getZhenInfo(2,_adcd);
    //行政区划 三级联动选择
    $(".search-areas").click(function () {
        $(".searchArealist").empty();
        getQuXianInfo(_adcd);
    });
    //批量选择
    $(".batchcheck").click(function () {
        $(".BatchApproval").addClass("town-choice");
    });
    $(".batchuncheck").click(function () {
        $(".BatchApproval").removeClass("town-choice");
    });
    //审批通过
    $(".sp").click(function () {
        var a = Array();
        $(".town-choice").each(function () {
            a.push($(this).attr("data-id"));
        });
        var remarks = "";
        var ids = a.join(',');
        if (ids == "") { abp.notify.warn("请选择待审批乡镇"); return; }
        abp.ajax({
            url: "/api/Audit/PostAudit",
            data: { adcd: _adcd, ids: ids,t:0, remarks: remarks },
            type: "Post"
        }).done(function (data) {
            if (data.isSuccess == "false") {
                abp.notify.warn(data.errorMsg);
            } else {
                abp.notify.success("操作成功！");
                getZhenInfo(2, _adcd);
            }
        });
    });
    //审批不通过
    $(".spno").click(function () {
        var a = Array();
        $(".town-choice").each(function () {
            a.push($(this).attr("data-id"));
        });
        var remarks = "";
        var ids = a.join(',');
        if (ids == "") { abp.notify.warn("请选择要批示的乡镇"); return; }
        openModal('审批不通过批示', '/Audit/AuditNo/?t=0&id=' + ids, { width: 750 });
    });
});
    //三级联动 县
     function getQuXianInfo(adcd) {
        $(".searchArealist ul.areas_quxian").remove();
        $(".searchArealist ul.areas_zhen").remove();
        getAreaName(3,adcd);
    }
    //获取行政地方名
     function getAreaName(levels,_adcd) {
        var lihtml = "";
        abp.ajax({
            url: "/api/Audit/GetAreaList",
            data: { adcd: _adcd, tid: levels },
            type: "post"
        }).done(function (data) {
            if (data != "" && null != data) {
                var data = eval(data);
                $.each(data, function (i, item) {
                    lihtml += '<li data-adcd="' + item.adcd + '">' + item.adnm + '</li>';
                });
                lihtml += '<li data-adcd="" style="float:right; color:red;">关闭</li>';
                //县
                var html = '<ul class="areas_quxian">' + lihtml + '</ul>';
                $(".searchArealist").append(html);
                //事件
                $(".areas_quxian li").click(function () {
                    var name = $(this).text();
                    var adcd = $(this).attr("data-adcd");
                    if (adcd == "") {
                        $(".searchArealist").empty();
                    }
                    else{
                        $(".search-areas").text(name);
                        getZhenInfo(3,adcd);
                        //$(this).addClass("selected");
                        //$(this).siblings('li').removeClass("selected");
                        $(".searchArealist").empty();
                    }
                });
            }
        });
    }
     function getZhenInfo(level, adcd) {
         abp.ajax({
             url: "/api/Audit/GetAuditApplication",
             data: { adcd: adcd, level:level},
             type: "Get"
         }).done(function (data) {
             var html = '';
             if (data.other != null) {
                 var c = data.other.split('|');
                 $("#lable0").html(c[0]);
                 $("#lable1").html(c[1]);
                 $("#lable2").html(c[2]);
                 $("#lable3").html(c[3]);
             }
             var rows = data.rows;
             for (var i = 0; i < data.rows.length; i++) {
                 //未提交
                 if (rows[i].status == null) {
                     html += '<div class="town-box"><div class="town-wtj"><div class="town-wtj-in">' + rows[i].adnm + '</div></div><a href="javascript:void(0);" class="town-btn-wtj">未提交</a></div>';
                 }
                     //已经提交
                 else {
                     var status = rows[i].status;
                     switch (status) {
                         case 0: status = "县审批不通过";
                             html += '<div class="town-box"><div class="town-dsp"><div class="town-dsp-in" style="background:#FF00FF;" data-ids="' + rows[i].id + '"  data-num="' + rows[i].auditNums + '" data-id="' + rows[i].townADCD + '" data-s="' + rows[i].status + '" data-name="' + rows[i].adnm + '">' + rows[i].adnm + '</div></div><a href="javascript:void(0);" data-id="' + rows[i].id + '"  class="town-btn-dsp">' + status + '</a></div>';
                             break;
                         case 1: status = "待审批";
                             html += '<div class="town-box"><div class="town-dsp"><div class="town-dsp-in" data-ids="' + rows[i].id + '" data-num="' + rows[i].auditNums + '" data-id="' + rows[i].townADCD + '" data-s="' + rows[i].status + '" data-name="' + rows[i].adnm + '">' + rows[i].adnm + '</div></div><a href="javascript:void(0);" data-id="' + rows[i].id + '"  class="town-btn-dsp">' + status + '</a></div>';
                             break;
                         case 2: status = "县已审市未批";
                             html += '<div class="town-box"><div class="town-yswp"><div class="town-yswp-in" data-num="' + rows[i].auditNums + '" data-ids="' + rows[i].id + '" data-id="' + rows[i].townADCD + '" data-s="' + rows[i].status + '" data-name="' + rows[i].adnm + '">' + rows[i].adnm + '</div></div><a href="javascript:void(0);" data-id="' + rows[i].id + '"  class="town-btn-yswp">' + status + '</a><div data-id="' + rows[i].id + '" class="BatchApproval ">&nbsp;</div></div>';
                             break;
                         case 3: status = "县已审市已批";
                             html += '<div class="town-box"><div class="town-yp"><div class="town-yp-in" data-num="' + rows[i].auditNums + '" data-ids="' + rows[i].id + '" data-id="' + rows[i].townADCD + '" data-s="' + rows[i].status + '" data-name="' + rows[i].adnm + '">' + rows[i].adnm + '</div></div><a href="javascript:void(0);" data-id="' + rows[i].id + '"  class="town-btn-yp">' + status + '</a></div>';
                             break;
                         case -1: status = "市审批不通过";
                             html += '<div class="town-box"><div class="town-yp"><div class="town-yp-in" style="background:red;" data-ids="' + rows[i].id + '"  data-num="' + rows[i].auditNums + '" data-id="' + rows[i].townADCD + '" data-s="' + rows[i].status + '" data-name="' + rows[i].adnm + '">' + rows[i].adnm + '</div></div><a href="javascript:void(0);" data-id="' + rows[i].id + '"  class="town-btn-yp">' + status + '</a></div>';
                             break;
                     }
                 }
             }
             $("#AuditList").html(html);

             //单个审批选择
             $(".BatchApproval").click(function () {
                 var f = $('div').hasClass("town-choice");
                 if (f) { $(this).removeClass("town-choice"); }
                 else {
                     $(this).addClass("town-choice");
                 }
             });

             //详情
             $(".town-dsp-in,.town-yswp-in,.town-yp-in").click(function () {
                 window.location.href = "/Audit/TownInfo/?id=" + $(this).attr("data-id") + "&ids=" + $(this).attr("data-ids") + "&s=" + $(this).attr("data-s") + "&t=2&nums=" + $(this).attr("data-num") + "&name=" + $(this).attr("data-name");
             });
         });
     }