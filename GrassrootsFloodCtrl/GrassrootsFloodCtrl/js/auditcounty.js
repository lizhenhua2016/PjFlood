/**公共方法****/

$(function () {
    $("body").attr("style", "background:#f9f9f9;");
    //加载统计
    getZhenInfo(2, _adcd);
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
        if (ids == "") { abp.notify.warn("请选择待审批县"); return; }
        abp.ajax({
            url: "/api/Audit/PostAudit",
            data: { adcd: _adcd, ids: ids, t: 1, remarks: remarks },
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
        openModal('审批不通过批示', '/Audit/AuditNo/?t=1&id=' + ids, { width: 750 });
    });
});

function getZhenInfo(level, adcd) {
    abp.ajax({
        url: "/api/Audit/GetAuditApplicationCounty",
        data: { adcd: adcd, level: level },
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
                    case -1: status = "审批不通过";
                        html += '<div class="town-box"><div class="town-dsp"><div class="town-dsp-in" data-ids="' + rows[i].countyADCD + '"  data-num="' + rows[i].auditNums + '" data-id="' + rows[i].id + '" data-s="' + rows[i].status + '" data-name="' + rows[i].adnm + '">' + rows[i].adnm + '</div></div><a href="javascript:void(0);"   class="town-btn-dsp">' + status + '</a></div>';
                        break;
                    case 1: status = "待审批";
                        html += '<div class="town-box"><div class="town-dsp"><div class="town-dsp-in" data-ids="' + rows[i].countyADCD + '" data-id="' + rows[i].id + '" data-num="' + rows[i].auditNums + '" data-s="' + rows[i].status + '" data-name="' + rows[i].adnm + '">' + rows[i].adnm + '</div></div><a href="javascript:void(0);" data-id="' + rows[i].id + '"  class="town-btn-dsp">' + status + '</a><div data-id="' + rows[i].id + '" class="BatchApproval ">&nbsp;</div></div>';
                        break;
                    case 3: status = "已审批";
                        html += '<div class="town-box"><div class="town-yp"><div class="town-yp-in" data-ids="' + rows[i].countyADCD + '" data-num="' + rows[i].auditNums + '" data-id="' + rows[i].id + '" data-s="' + rows[i].status + '" data-name="' + rows[i].adnm + '">' + rows[i].adnm + '</div></div><a href="javascript:void(0);" data-id="' + rows[i].id + '"  class="town-btn-yp">' + status + '</a></div>';
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
            window.location.href = "/Audit/CountyPerson/?id="+$(this).attr("data-id")+"&adcd=" + $(this).attr("data-ids") + "&s=" + $(this).attr("data-s") + "&t=2&nums=" + $(this).attr("data-num") + "&name=" + $(this).attr("data-name");
        });
    });
}