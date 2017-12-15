var cities = new L.layerGroup([]);//构建地图上图层集合
$.fn.extend({
    getpostcodebyusername: function (username, warninfoid) {
        $.ajax({
            type: "post",
            url: "/api/AppApi/AppGetPostByUserName2",
            dataType: "json",
            data: {"username": username, "warninfoid": warninfoid},
            success: function (data) {
                var haha = eval(data);
                $.each(haha, function (i, item) {
                    $("#post").append("<li><a href='#aa" + i + "' data-toggle='tab'>" + item.postcode + "</a></li>");
                });
                $("#post").children("li").first().addClass("active");
                //这里是判断第一个li
                var str = $("#post").children("li").first().text();

                $(".abcd").gettaskrecordbyusername(username, warninfoid, str);
                $("#mapabc").getlocationbyusername(username, warninfoid, str);
                //点击切换tab
                $("#post").children("li").on("click", function () {
                    $(".abcd").gettaskrecordbyusername(username, warninfoid, $(this).text());
                });
            }

        });
    },
    getlocationbyusername: function (username, warninfoid, litext) {
        var lineStyle = {
            color: "#e6000b",
            weight: 5
        };

        if (litext != "") {

            $.ajax({
                type: "post",
                url: "/api/AppApi/AppGetLocationOnLvZhi",
                dataType: "json",
                data: {"username": username, "warninfoid": warninfoid, "postcode": litext},
                success: function (data) {
                    var location = "";
                    var haha = eval(data);
                    $.each(haha, function (i, item) {
                        location += item.location + ";";
                    });

                    if (location != "") {

                        var arrlist = location.split(";");
                        var xstr = arrlist[0].split(",")[0];
                        var ystr = arrlist[0].split(",")[1];
                        var pointArray = new Array();
                        for (var j = 0; j < arrlist.length; j++) {
                            var x = parseFloat(arrlist[j].split(",")[0]);
                            var y = parseFloat(arrlist[j].split(",")[1]);
                            if (x > 0 && y > 0) {
                                pointArray.push(new L.LatLng(y, x));
                            }

                        }

                        var lineFeature = new L.Polyline(pointArray, lineStyle);

                        var mappoint = L.marker(L.latLng(ystr, xstr), {
                            icon: L.icon({
                                iconUrl: "http://ovsqnadmr.bkt.clouddn.com/p1.png",
                                iconAnchor: new L.Point(9, 9)
                            })
                        });
                        map.addLayer(lineFeature);
                        map.addLayer(mappoint);
                        map.setView([ystr, xstr], 18);

                    }
                }

            });
        }
    },
    gettaskrecordbyusername: function (username, warninfoid, litext) {
        if (litext != "") {
            $.ajax({
                type: "post",
                dataType: "json",
                url: "/api/AppApi/AppGetTaskRecordOnLvZhi",
                data: {"username": username, "warninfoid": warninfoid, "postcode": litext},
                success: function (data) {
                    var haha = eval(data);
                    var abcarrlist = new Array();
                    $(".abcd").empty();
                    $.each(haha, function (i, item) {
                        var filestr = "";
                        var aa = "";
                        if (item.filestr != "") {
                            
                            abcarrlist = item.filestr.split("|");
                            $.each(abcarrlist, function (i) {
                                
                                if (i > 0) {
                                    aa += "<span><a class='photo'  href='" + abcarrlist[i] + "'></a></span>";
                                }
                            });
                            
                            filestr += "<span><a class='photo'  href='" + abcarrlist[0] + "'>查看图片</a></span>";

                        }

                        $(".abcd").append("<p><span>" + item.addtime + "</span><span>" + item.values + "</span>" + filestr + "<span>" + aa + "</span></p>");//<span>"+item.filestr+"</span>

                    });
                    $(".photo").colorbox({rel: "photo", transition: "fade"});
                }
            });
        }
    }


});