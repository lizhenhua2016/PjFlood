function GetWarnEvent() {
    var htmlstr = ""
    $.ajax({
        type: "get",
        url: "/api/AppWarnEvent/GetWarnList",
        dataType: "json",
        data: {},
        success: function (data) {
            var list = eval(data.warnList);
            $.each(list, function (i, item) {
                $(".warnlist").append("<li data-warnid='" + item.id + "'><b>" + item.eventName + "</b><span><b>" + (item.isStartWarning ? '<b class="processing">进行中</b>' : '<b class="closing">已关闭</b>') + "</b></span></li>");
            });

            $(".warnlist li").on("click", function () {
                //alert($(this).attr("data-warnid"));
                var wid = $(this).attr("data-warnid");
                $.ajax({
                    type: "post",
                    url: "/api/AppWarnEvent/RouteGetWarnListByWarnId",
                    dataType: "json",
                    data: { "WarnId": wid },
                    success: function (data) {
                        var haha = eval(data);
                        $(".warnlist").empty();
                        $.each(haha, function (i, item) {
                            $(".warnlist").append("<li data-warnid='" + item.id + "'>" + "预警等级:" + item.warnLevel + "级" + item.warnMessage + "</li>");
                        });
                        $("#sidebar h4").append("<a class='aareturn'>返回</a>");

                        $(".aareturn").on("click", function () {
                            $(".aareturn").empty();
                            $(".warnlist").empty();
                            GetWarnEvent();
                        });
                    }
                });

                GetWarnList(wid);
            });

            $(".warnlist li").on("mouseover", function () {
                $(this).addClass("current");
            });
            $(".warnlist li").on("mouseout ", function () {
                $(this).removeClass("current");
            });
        }
    });
}

function GetWarnList(warnid) {
    $.ajax({
        type: "post",
        url: "/api/AppApi/GetGpsList",
        dataType: "json",
        data: { "AppWarnEventId": warnid },
        success: function (data) {
            var list = eval(data);
            $.each(list.rows, function (i, item) {
                //alert(item.userName);
                var cun = L.marker(L.latLng(item.lat, item.lng), { icon: L.icon({ iconUrl: "/Images/p2.png", iconAnchor: new L.Point(9, 9) }) }).bindLabel('<div name="marker" class="map_mark"><div class="map_mark_inner"><span class="map_mark_name">' + item.userRealName + '</span> </div></div>', { direction: 'right', noHide: false });
                _map.addLayer(cun);
                _map.setView([item.lat, item.lng], 16);
                cun.options.userName = item.userName;
                $(cun).on("click", function (e) {
                    //layer.alert(e.target.options.userName);

                    layer.open({
                        type: 2,
                        area: ['1024px', '768px'],
                        fixed: false, //不固定
                        maxmin: true,
                        content: '/TaskArragement/guiji/?receiveUserName=' + '' + e.target.options.userName + '',
                        shade: 0.7,
                        shadeClose: true,
                        title: e.target.options.userName + "信息展示",
                        anim: 5,
                        isOutAnim: true
                    });
                });
            });
        }
    });
}

$(function () {
    $("#sidebar").hide();


    $('[name="status"]').bootstrapSwitch({
        onText: "不显示",
        offText: "显示",
        onColor: "success",
        offColor: "info",
        size: "small",
        onSwitchChange: function (event, state) {
            $("#sidebar").toggle();            
        }
    })
});