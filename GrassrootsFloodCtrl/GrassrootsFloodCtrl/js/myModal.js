!(function ($, window) {
    'use strict';
    //重置窗口样式
    window.resetmyModalBox = function (object) {
        $('#' + object + '').find('.modal-footer').show();
        $('#' + object + '').find('.modal-dialog').css('width', 900);
        $('#btnModalSave').html('确定').show();
        var btns = $('#' + object + '').find('.modal-footer').children();
        if (btns.length > 2) {
            $.each(btns, function (i, n) {
                var id = $(this).attr('id');
                if (id !== 'btnModalClose' && id !== 'btnModalSave')
                    $(n).remove();
            });
        }
    };

    //弹出窗口
    window.openMyModal = function (object,title, url, options) {
        resetmyModalBox();
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
            $('#' + object + '').find('.modal-footer').hide();

        if (options.otherButtons.length > 0) {
            $.each(options.otherButtons, function (i, n) {
                $('#' + object + '').find('.modal-footer').append($(n));
            });
        }

        $('#modalTitle').html(title);
        if (width && width > 0) {
            $('#' + object + ' .modal-dialog').width(width);
        }
        $('#' + object + '').modal("show");
        if (url.indexOf('.aspx') != -1) {
            var iframe = $('<iframe/>').attr({ 'frameborder': 0, 'width': '100%', 'height': '600', 'src': url }).css({ 'height': '600px' });
            $('#' + object + ' .modal-body').empty().append(iframe);
            if (callback && $.isFunction(callback)) {
                iframe.load(function () {
                    callbackArgs.unshift($(this)[0].contentWindow);
                    callback.apply(null, callbackArgs);
                });
            }
        } else {
            if (undefined != options["htmlStr"]) {
                $('#' + object + ' .modal-body').html(options["htmlStr"]);
            } else {
                $.ajax({
                    url:getUrl(url),
                    cache: false,
                    success: function (data, textStatus, jqXHR) {
                        var docType = "<!doctype html>";
                        if (data.substr(0, docType.length) == docType) {
                            var iframe = $('<iframe/>').attr({ 'frameborder': 0, 'width': '100%', 'height': '300', 'src': url }).css({ 'height': '300px' });
                            iframe[0].innerHTML = data;
                            $('#' + object + ' .modal-body').empty().append(iframe);
                        } else {
                            $('#' + object + ' .modal-body').empty().append(data);
                        }
                    },
                    error: function (jqXHR, textStatus, error) {
                        $('#' + object + ' .modal-body').empty().append('<h3 class="text-danger">发生错误,详见日志: ' + error + '</h3>');
                    }
                });
            }
        }
    };

    window.closeMyModal = function (object) {
        $('#' + object + '').modal("hide");
    };
})(jQuery, window);
