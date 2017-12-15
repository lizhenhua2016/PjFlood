$.fn.extend({
    //岗位
    onduty: function ($el, messageid) {
        $.ajax({
            type: "post",
            url: "/api/AppReport/RouteAppDutyPost",
            dataType: "json",
            data: {"messageid": messageid},
            success: function (data) {
                var haha = eval(data);
                $.each(haha, function (i, item) {
                    $el.append("<li><a data-toggle='tab'>" + item.postcode + "</a></li>");
                });
                $el.children("li").first().addClass("active");
                var str = $el.children().first().text();
                $(".abcd").onduty2($(".abcd"), messageid, str);

                $el.children("li").on("click",function () {
                    
                    $(".abcd").onduty2($(".abcd"), messageid, $(this).text());
                });
            }
        });
    },
    //履职记录
    onduty2: function ($el, messageid, postcode) {
        $el.empty();
        $.ajax({
            type: "post",
            url: "/api/AppReport/RouteAppRecordByMessageId",
            dataType: "json",
            data: {"messageid": messageid, "postcode": postcode},
            success: function (data) {
                var haha = eval(data);
                var abcarrlist = new Array();
                $.each(haha, function (i, item) {
                    var filestr = "";
                    var aa = "";
                    if (item.filestr!=""){
                        abcarrlist = item.filestr.split("|");
                        $.each(abcarrlist, function (i) {
                            if (i > 0) {
                                aa += "<span><a class='photo'  href='" + abcarrlist[i] + "'></a></span>";
                            }
                        });
                        filestr += "<span><a class='photo'  href='" + abcarrlist[0] + "'>查看图片</a></span>";
                    }
                    $el.append("<p><span>" + item.addtime + "</span><span>" + item.values + "</span>" + filestr + "<span>" + aa + "</span></p>");
                    //$el.append("<p><span>" + item.addtime + "</span><span>" + item.values + "</span></p>");
                });
                $(".photo").colorbox({rel: "photo", transition: "fade"});
            }
        });
    },
    //地图展示
    onduty3:function (messageid) {
        
        var lineStyle = {
            color: "#e6000b",
            weight: 3
        };
        $.ajax({
            type:"post",
            url:"/api/AppReport/RouteAppLocation",
            dataType:"json",
            data:{"messageid":messageid},
            success:function (data) {
                var haha= eval(data);
                var location="";
                $.each(haha, function (i,item) {                    
                    location+=item.location+";";
                });
                
                if (location!=""){
                    var arrlist=location.split(";");
                    var xstr=arrlist[0].split(",")[0];
                    
                    var ystr=arrlist[0].split(",")[1];
                    var pointArray = new Array();
                    for (var j = 0; j < arrlist.length; j++) {
                        var x = parseFloat(arrlist[j].split(",")[0]);
                        var y = parseFloat(arrlist[j].split(",")[1]);
                        if (x > 0 && y > 0) { pointArray.push(new L.LatLng(y, x)); }
                    }
                    
                    var lineFeature = new L.Polyline(pointArray, lineStyle);
                    
                    map.addLayer(lineFeature);
                    
                    map.setView([ystr, xstr], 18);

                }
            }
        });
    }
});