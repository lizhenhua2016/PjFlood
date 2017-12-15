!(function ($, window) {
    'use strict';
    window.SHORTGC = { xxsk: "库", dzxsk: "库", st: "塘", ht: "塘", df: "堤", sz: "闸", bz: "泵", gq: "灌", wq: "圩", ncgsgc: "水", sdz: "电", swz: "文", sk: "库", qd: "渠" };
    window.GCENUM = { xxsk: "小型水库", dzxsk: "大中型水库", st: "山塘", ht: "海塘", df: "堤防", sz: "水闸", bz: "泵站", gq: "灌区", wq: "圩区", ncgsgc: "农村供水工程", sdz: "水电站", swz: "水文站", sk: "水库", qd: "渠道" };
    //重置窗口样式
    window.resetModalBox = function () {
        $('#modalBox').find('.modal-footer').show();
        $('#modalBox').find('.modal-dialog').css('width', 950);

        $('#btnModalSave').html('确定').show();
        var btns = $('#modalBox').find('.modal-footer').children();
        if (btns.length > 2) {
            $.each(btns, function (i, n) {
                var id = $(this).attr('id');
                if (id !== 'btnModalClose' && id !== 'btnModalSave')
                    $(n).remove();
            });
        }
    };

    //弹出窗口
    window.openModal = function (title, url, options) {
        resetModalBox();

        url = abp.appPath + url;
        url = url.replace("//", "/");
        options = options || {};
        var width = options.width || 0,
            callback = options.callback || null,
            callbackArgs = options.args || [],
            showSaveButton = options.showSaveButton === false ? false : true;
        options.saveButtonName = options.saveButtonName || '确定';
        options.otherButtons = options.otherButtons || [];
        options.hideFooter = !!options.hideFooter;

        showSaveButton ? $('#btnModalSave').show() : $('#btnModalSave').hide();
        $('#btnModalSave').html(options.saveButtonName);

        if (options.hideFooter)
            $('#modalBox').find('.modal-footer').hide();

        if (options.otherButtons.length > 0) {
            $.each(options.otherButtons, function (i, n) {
                $('#modalBox').find('.modal-footer').append($(n));
            });
        }

        $('#modalTitle').html(title);
        if (width && width > 0) {
            $('#modalBox .modal-dialog').width(width);
        }
        $('#modalBox').modal("show");
        if (url.indexOf('.aspx') != -1) {
            var iframe = $('<iframe/>').attr({ 'frameborder': 0, 'width': '100%', 'height': '600', 'src': url }).css({ 'height': '600px' });
            $('#modalBox .modal-body').empty().append(iframe);
            if (callback && $.isFunction(callback)) {
                iframe.load(function () {
                    callbackArgs.unshift($(this)[0].contentWindow);
                    callback.apply(null, callbackArgs);
                });
            }
        } else {
            if (undefined != options["htmlStr"]) {
                $('#modalBox .modal-body').html(options["htmlStr"]);
            } else {
                $.ajax({
                    url:getUrl(url),
                    cache: false,
                    success: function (data, textStatus, jqXHR) {
                        var docType = "<!doctype html>";
                        if (data.substr(0, docType.length) == docType) {
                            var iframe = $('<iframe/>').attr({ 'frameborder': 0, 'width': '100%', 'height': '300', 'src': url }).css({ 'height': '300px' });
                            iframe[0].innerHTML = data;
                            $('#modalBox .modal-body').empty().append(iframe);
                        } else {
                            $('#modalBox .modal-body').empty().append(data);
                        }
                    },
                    error: function (jqXHR, textStatus, error) {
                        $('#modalBox .modal-body').empty().append('<h3 class="text-danger">发生错误,详见日志: ' + error + '</h3>');
                    }
                });
            }
            //$('#modalBox .modal-body').load(getUrl(url));
        }
    };

    window.closeModal = function () {
        $('#modalBox').modal("hide");
    };

    //取完整地址
    window.getUrl = function (baseUrl, para, excludeTicks) {
        baseUrl = baseUrl.charAt(0) == '/' ? baseUrl.substr(1, baseUrl.length) : baseUrl;
        baseUrl = abp.appPath + baseUrl;
        para = para || {};
        if ($.isPlainObject(para) === false) {
            alert("参数para必须是普通对象");
            return;
        }
        if (!excludeTicks && baseUrl.indexOf('_t=') > -1)
            para['_t'] = new Date().getTime();
        var urlParts = [], delimiter = '?';
        delimiter = baseUrl.indexOf('?') != -1 ? '&' : '?';
        urlParts.push(baseUrl, delimiter, $.param(para));
        return urlParts.join('');
    };

    window.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
        var r = window.location.search.substr(1).match(reg);  //匹配目标参数
        if (r != null) return unescape(r[2]); return null; //返回参数值
    }

    //转换json日期格式
    window.parseJsonDate = function (jsonDate) {
        if (!jsonDate || jsonDate.length < 6)
            return null;
        //格式类似：/Date(1410364800000-0000)/
        return new Date(parseInt(jsonDate.substr(6)));
    };

    //统一的上传错误显示方法
    window.showUploadError = function (up, err) {
        var error = '';
        if (err.response) {
            var start = err.response.indexOf('<title>') + '<title>'.length,
                end = err.response.indexOf('</title>');
            error = err.response.substring(start, end);
        }
        alert("上传失败 #" + err.code + ": " + err.message + " " + error);
    };

    window.FileUploader = function (browserButtonId, containerId, processContainerId, uploadUrl, options) {
        options = options || {};
        options.multiParams = options.multiParams || {};
        options.filters = options.filters || {
            max_file_size: '20mb',
            mime_types: [
                { title: "附件", extensions: "doc,docx,xls,xlsx,ppt,pptx,pdf,txt" }
            ]
        };
        options.uploadCallback = options.uploadCallback || function () { };

        var uploadCallback = function (e, file, res) {
            var r = $.parseJSON(res.response);

            options.uploadCallback.call(null, e, file, r);
        };

        var uploader = new plupload.Uploader({
            runtimes: 'html5,flash',
            browse_button: browserButtonId,
            container: document.getElementById(containerId),
            url: uploadUrl,
            flash_swf_url: abp.appPath + 'Content/plupload/Moxie.swf',
            multipart_params: options.multiParams,
            filters: options.filters,
            init: {
                PostInit: function () {
                    $('#' + processContainerId).html('');
                },
                FilesAdded: function (up, files) {
                    plupload.each(files, function (file) {
                        document.getElementById(processContainerId).innerHTML += '<div id="' + file.id + '">' + file.name + ' (' + plupload.formatSize(file.size) + ') <b></b></div>';
                    });

                    uploader.start();
                },
                UploadProgress: function (up, file) {
                    document.getElementById(file.id).getElementsByTagName('b')[0].innerHTML = '<span>' + file.percent + "%</span>";
                },
                FileUploaded: uploadCallback,
                Error: function (up, err) {
                    window.showUploadError(up, err);
                }
            }
        });
        return uploader;
    };


    window.DomFileUploader = function (browserButton, container, processContainer, uploadUrl, options) {
        options = options || {};
        options.multiParams = options.multiParams || {};
        options.filters = options.filters || {
            max_file_size: '20mb',
            mime_types: [
                { title: "附件", extensions: "doc,docx,xls,xlsx,ppt,pptx,pdf,txt" }
            ]
        };
        options.uploadCallback = options.uploadCallback || function () { };

        var uploadCallback = function (e, file, res) {
            var r = $.parseJSON(res.response);

            options.uploadCallback.call(null, e, file, r);
        };

        var uploader = new plupload.Uploader({
            runtimes: 'html5,flash',
            browse_button: browserButton,
            container: container,
            url: uploadUrl,
            flash_swf_url: abp.appPath + 'Content/plupload/Moxie.swf',
            multipart_params: options.multiParams,
            filters: options.filters,
            init: {
                PostInit: function () {
                    processContainer.html('');
                },
                FilesAdded: function (up, files) {
                    plupload.each(files, function (file) {
                        processContainer.innerHTML += '<div id="' + file.id + '">' + file.name + ' (' + plupload.formatSize(file.size) + ') <b></b></div>';
                    });

                    uploader.start();
                },
                UploadProgress: function (up, file) {
                    document.getElementById(file.id).getElementsByTagName('b')[0].innerHTML = '<span>' + file.percent + "%</span>";
                },
                FileUploaded: uploadCallback,
                Error: function (up, err) {
                    window.showUploadError(up, err);
                }
            }
        });

        return uploader;
    };

    var getOptionVal = function (hiddenId, selectId) {
        $("#" + hiddenId).val("");
        var val = $("#" + selectId).next('span').find('option').map(function () {
            return $(this).val();
        }).get().join(',');
        $("#" + hiddenId).val(val);
    }

    window.LoadDataSelect2 = function (selectId, hiddenId, text, value, url, searchName, defaultData, jsonfield) {
        var data = { id: "" };
        data[text] = "请选择";
        data[value] = "";
        defaultData = defaultData || data;
        if (!defaultData["id"]) defaultData["id"] = "";
        jsonfield = jsonfield || "rows";
        $("#" + selectId).select2({
            ajax: {
                url: url,
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    params.page = params.page || 1;
                    params.term = (params.term == " " ? "" : params.term);
                    var paramObj = {
                        //UnitName: params.term, // search term
                        offset: (params.page - 1) * 100,
                        limit: 100
                    };
                    paramObj[searchName] = params.term;
                    return paramObj;
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;
                    if (data && data.rows.length > 0 && data.rows[0]["id"] == undefined) {
                        for (var i = 0; i < data.rows.length; i++) {
                            data.rows[i]["id"] = data.rows[i][value];
                        }
                    }
                    return {
                        results: eval("data." + jsonfield),
                        pagination: {
                            more: (params.page * 100) < data.total
                        }
                    };
                },
                cache: true
            },
            language: "zh-CN",
            width: "300px",
            escapeMarkup: function (markup) {
                return markup;
            }, // let our custom formatter work
            data: [defaultData],
            minimumInputLength: 1,
            templateResult: function (row) {
                if (row.loading) return row.text;
                var markup = "<option value='" + row[value] + "'>" + row[text] + "</option>";
                if (row.description) {
                    markup += "<div class='select2-result-repository__description'>" + row.description + "</div>";
                }
                return markup;
            },
            templateSelection: function (row,option) {
                if (row) {
                    return "<option value='" + row[value] + "'>" + row[text] + "</option>";
                }
                return '<option value="">请输入关键字搜索</option>';
            }
        }).on('select2:open', function (evt) {
            if ($(".select2-search__field").val() == "") {
                setTimeout(function () {
                    $(".select2-search__field").val(" ").keydown().keyup().val("");
                }, 200);
            }
        }).on('select2:select', function (evt) {
            getOptionVal(hiddenId, selectId);
        }).on('select2:unselect',function() {
            getOptionVal(hiddenId, selectId);
        });
    }

    

    //tree helper
    var treeHelper = function (selIds) {
        this.selIds = selIds;
        this.selectedIds = [];
        this.tree = null;
    };

    //自动下钻树到指定节点
    treeHelper.prototype.syncTreeNodeExpand = function (evt, treeId, treeNode, msg) {
        if (!this.selIds || this.selIds.length == 0)
            return;

        if (this.selectedIds.length == 0) {
            var parts = this.selIds.split(',');
            for (var i = 0; i < parts.length; i++) {
                var ids = parts[i].split('.');
                var idStatus = [];
                for (var j = 0; j < ids.length; j++) {
                    idStatus.push({ id: ids[j], checked: 0 }); //checked=0,没处理，=1部分处理，2=全部处理
                }
                this.selectedIds.push({ fullIds: idStatus });
            }
        }
        if (this.tree == null) {
            this.tree = $.fn.zTree.getZTreeObj(treeId);
        }

        forloop:
            for (var i = 0; i < this.selectedIds.length; i++) {
                var ids = this.selectedIds[i].fullIds;
                for (var j = 0; j < ids.length; j++) {
                    if (ids[j].checked == 2)
                        continue;
                    var node = this.tree.getNodeByParam('id', ids[j].id, null);
                    if (node) {
                        if (j == ids.length - 1) {
                            this.tree.checkNode(node, true, false);
                        } else if (node.isParent && !node.open) {
                            this.tree.expandNode(node, true);
                            ids[j].checked = 2;
                            break forloop;
                        }
                    }
                    ids[j].checked = 2;
                }
            }
    };

    window.TreeHelper = treeHelper;

    //日期格式化
    Date.prototype.formatDate = function (format) {
        var date = this,
            day = date.getDate(),
            month = date.getMonth() + 1,
            year = date.getFullYear(),
            hours = date.getHours(),
            minutes = date.getMinutes(),
            seconds = date.getSeconds();

        if (!format) {
            format = "yyyy-MM-dd";
        }

        format = format.replace("MM", month.toString().replace(/^(\d)$/, '0$1'));

        if (format.indexOf("yyyy") > -1) {
            format = format.replace("yyyy", year.toString());
        } else if (format.indexOf("yy") > -1) {
            format = format.replace("yy", year.toString().substr(2, 2));
        }

        format = format.replace("dd", day.toString().replace(/^(\d)$/, '0$1'));

        if (format.indexOf("t") > -1) {
            if (hours > 11) {
                format = format.replace("t", "pm");
            } else {
                format = format.replace("t", "am");
            }
        }

        if (format.indexOf("HH") > -1) {
            format = format.replace("HH", hours.toString().replace(/^(\d)$/, '0$1'));
        }

        if (format.indexOf("hh") > -1) {
            if (hours > 12) {
                hours -= 12;
            }

            if (hours === 0) {
                hours = 12;
            }
            format = format.replace("hh", hours.toString().replace(/^(\d)$/, '0$1'));
        }

        if (format.indexOf("mm") > -1) {
            format = format.replace("mm", minutes.toString().replace(/^(\d)$/, '0$1'));
        }

        if (format.indexOf("ss") > -1) {
            format = format.replace("ss", seconds.toString().replace(/^(\d)$/, '0$1'));
        }

        return format;
    };

    /*
    简单分页功能grid
    url： 数据请求地址
    opt： 参数，
    */
    var SimpleGrid = function (url, opt) {
        this.url = url;
        this.tmpl = opt.template ? $.templates(opt.template) : null;
        this.container = opt.container;
        $.extend(this, opt);
        // this.rowFunc = opt.rowFunc;
        // this.queryFunc = opt.queryFunc;
        // this.afterFunc = opt.afterFunc;
        this.pagerContainer = opt.pagerContainer || 'pageContainer';
        this.limit = opt.limit || 12;
        this.order = opt.order || 'asc';
        this.sort = opt.sort;
        this.total = 0;
        this.offset = 0;
        this.pager = null;
        this.pageIndex = opt.pageIndex || 0;
        this.beforeSend = opt.beforeSend;
        this.type = opt.type || 'POST';
    };

    SimpleGrid.prototype.refresh = function () {
        this.load(this.pageIndex);
    };

    SimpleGrid.prototype.load = function (pageIndex) {
        this.loadPage(pageIndex);
    };

    SimpleGrid.prototype.loadPage = function (pageIndex) {
        this.pageIndex = pageIndex;
        var that = this, i;
        var queryParam = this.queryFunc ? this.queryFunc.call(null) : {};
        queryParam.limit = this.limit;
        queryParam.pageSize = this.limit;
        queryParam.pageIndex = pageIndex;
        queryParam.order = this.order;
        if (this.sort)
            queryParam.sort = this.sort;
        queryParam.offset = pageIndex * this.limit;

        var url = this.url;// window.getUrl(this.url, queryParam);
        abp.ui.setBusy($('#mainContent'), abp.ajax({
            url: url,
            data: queryParam,
            type: that.type,
            beforeSend: this.beforeSend
        }).done(function (data) {
            that.container.empty();
            var total = 0;
            var rows = [];
            if ($.isFunction(that.statFunc)) {
                var r = that.statFunc.call(null, data);
                total = r.total;
                rows = r.rows;
            } else {
                total = data.total;
                rows = data.rows;
            }
            
            if (total <= that.limit) {
                $('#' + that.pagerContainer).hide();
            } else {
                $('#' + that.pagerContainer).show();
            }

            if (total > 0 && rows.length == 0) {
                that.load(that.pageIndex - 1);
            }

            if (that.pager === null || that.total != total) {
                that.total = total;
                that.pager = $('#' + that.pagerContainer).pagination({
                    items: that.total,
                    itemsOnPage: that.limit,
                    cssStyle: 'light-theme',
                    prevText: '上一页',
                    nextText: '下一页',
                    onPageClick: function (pageNumber, e) {
                        that.load(pageNumber - 1);
                    }
                });
            }

            for (i = 0; i < rows.length; i++) {
                if (that.tmpl) {
                    //使用模板绑定,jsrender
                    var html = that.tmpl.render(rows[i]);
                    that.container.append(html);
                } else {
                    that.rowFunc.call(that, rows[i], i, rows, data);
                }
            }

            if (that.afterFunc) that.afterFunc.call(that, data);
        }));
    };

    window.SimpleGrid = SimpleGrid;
    try {
        $.fn.modal.Constructor.prototype.enforceFocus = function () { }; //解决select2在model内搜索框无效
    } catch (err) { }
    //...
    $(function () {
        $.views && $.views.settings.allowCode(true);
        try{
            //设置默认Validator消息
            $.validator.setDefaults({
                debug: true,

                success: function ($error, element) {
                    $(element).tooltip('destroy');
                },

                errorPlacement: function ($place, $element) {
                    $place.hide();
                    $element.attr('title', $place.text()).attr('data-placement', 'bottom');
                    $element.tooltip('destroy');
                    $element.tooltip('show');
                }
            });
      
        $.validator.messages = {
            required: "必填字段.",
            remote: "请修正该字段.",
            email: "请输入有效邮件地址.",
            url: "请输入有效 URL.",
            date: "请输入有效日期.",
            dateISO: "请输入有效日期 ( ISO ).",
            number: "请输入有效数字.",
            digits: "请只输入数字.",
            equalTo: "请再次输入相同的值.",
            maxlength: $.validator.format("最大支持 {0} 个字符."),
            minlength: $.validator.format("请至少输入 {0} 个字符."),
            rangelength: $.validator.format("只支持输入 {0} 到 {1} 个字符长度."),
            range: $.validator.format("请输入 {0} 到 {1} 范围内的值."),
            max: $.validator.format("请输入小于等于 {0} 的值."),
            min: $.validator.format("请输入大于等于 {0} 的值.")
        };
        } catch (err) { }
    });
})(jQuery, window);




/*数组定义*/
function Vector() {
    this.data = new Array();
    this.add = Vector_add;
    this.remove = Vector_remove;
    this.elementAt = Vector_elementAt;
    this.setElementAt = Vector_setElementAt;
    this.insert = Vector_insert;
    this.contains = Vector_contains;
    this.length = Vector_length;
    this.toString = Vector_toString;
}

function Vector_add(item) {
    this.data[this.data.length] = item;
}

function Vector_remove(index) {
    var data = this.data;
    data[index] = null;
    var tmpdata = new Array();
    var newindex = 0;
    for (var i = 0; i < data.length; i++) {
        if (data[i] != null) {
            tmpdata[newindex] = data[i];
            newindex++;
        }
    }
    this.data = tmpdata;
}

function Vector_removeItem(item) {
    var data = this.data;
    var tmpdata = new Array();
    var newindex = 0;
    for (var i = 0; i < data.length; i++) {
        if (data[i] != item) {
            tmpdata[newindex] = data[i];
        }
        newindex++;
    }
    this.data = tmpdata;
}

function Vector_elementAt(index) {
    return this.data[index];
}

function Vector_setElementAt(index, item) {
    this.data[index] = item;
}

function Vector_insert(index, item) {
    if (index == this.data.length) {
        this.add(item);
        return;
    }
    var data = this.data;
    var tmpdata = new Array();
    var newindex = 0;
    for (var i = 0; i < data.length; i++) {
        if (i == index) {
            tmpdata[i] = item;
            newindex++;
        }
        tmpdata[newindex] = data[i];
        newindex++;
    }
    this.data = tmpdata;
}

function Vector_contains(item) {
    for (var i = 0; i < this.data.length; i++) {
        if (this.data[i] == item) {
            return true;
        }
    }
    return false;
}

function Vector_length() {
    return this.data.length;
}

function Vector_toString() {
    var dataString = "[   ";
    var data = this.data;
    for (var i = 0; i < data.length; i++) {
        dataString += data[i] + "   ";
    }
    dataString += "] ";
    return dataString;
}

function TabNullData() {
    $('#editContainer .needTab>span').each(function (i, d) {
        var a = " ";
        if (parseInt($(d).text()) == 0 || $(d).text() == " " || $(d).text() == "") {
            $(this).parent('.needTab').html('');
        }
;        if ($(d).text().substring(0, 8) == "0001/1/1") {
            $(this).parent('.needTab').html('');
        }
    });

}
