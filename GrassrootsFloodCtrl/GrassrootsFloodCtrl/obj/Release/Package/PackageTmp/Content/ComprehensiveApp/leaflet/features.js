var feature = {
    // module:
    // application/Function/features

    // summary:
    // Function

    // description:
    // leaflet要素控制

    // 采用leaflet wicket将标准WKT坐标转换为要素
    createFeatures: function (/*对输入的记录进行分析并加载在图层上*/geom, styleConfig) {
        var wkt, obj;
        if (geom != null && geom != "" && geom != {}) {
            wkt = new Wkt.Wkt();
            // Catch any malformed WKT strings
            try {
                wkt.read(geom);
            } catch (e1) {
                try {
                    wkt.read(el.value.replace('\n', '').replace('\r', '').replace('\t', ''));
                } catch (e2) {
                    if (e2.name === 'WKTError') {
                        alert('Wickt不能解析该wkt坐标。请检查括号是否成对，建议移除后添加的tabs和面和线后再试。');
                        return;
                    }
                }
            }
            // Make an object
            obj = wkt.toObject(styleConfig || application.map.defaults);//图层样式
            // Add listeners for overlay editing events
            if (wkt.type === 'polygon' || wkt.type === 'linestring') {
            }
            /*
            if (Wkt.isArray(obj)) { // Distinguish multigeometries (Arrays) from objects
                for (i in obj) {
                    if (obj.hasOwnProperty(i) && !Wkt.isArray(obj[i])) {
                        obj[i].addTo(application.map);
                    }
                }
            } else {
                obj.addTo(application.map); // Add it to the map
            }
            */
            return obj;
        }
    }
};