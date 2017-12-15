////实例化编辑器
//var ue = UE.getEditor('overviewEditor');


function setFileUpload() {

    var upTmpl = $.templates('#upTemplate');

    var panoramaImgUploader = new FileUploader('PanoramaImgUp', 'PanoramaImgContainer', 'PanoramaImgProgress',
    '/api/UploadFile?format=json',
    {
        multiParams: { 'allowedFileTypes': 'jpg,jpeg,png', 'fileFolde': 'ProjectAttach' },
        multi_selection: false,
        filters: {
            max_file_size: '20mb',
            mime_types: [
                { title: "图片", extensions: "jpg,jpeg,png" }
            ]
        },
        uploadCallback: function (e, file, r) {
            $("#PanoramaImgUp>span").html(ObjectFileFormat(r));
            $("#PanoramaImg").val(JSON.stringify(r));
        }
    });
    panoramaImgUploader.init();


    //var bgImgUploader = new FileUploader('BgImgUp', 'BgImgContainer', 'BgImgProgress',
    //'/api/UploadFile?format=json',
    //{
    //    multiParams: { 'allowedFileTypes': 'jpg,jpeg,png', 'fileFolde': 'ProjectAttach' },
    //    multi_selection: false,
    //    filters: {
    //        max_file_size: '20mb',
    //        mime_types: [
    //            { title: "图片", extensions: "jpg,jpeg,png" }
    //        ]
    //    },
    //    uploadCallback: function (e, file, r) {
    //        $("#BgImgUp>span").html(ObjectFileFormat(r));
    //        $("#BgImg").val(JSON.stringify(r));
    //    }
    //});
    //bgImgUploader.init();


    //var gcFaceImgUploader = new FileUploader('btnUpGcFaceImg', 'GcFaceImgContainer', 'GcFaceImgProgress',
    // '/api/UploadFile?format=json',
    // {
    //     multiParams: {
    //         'allowedFileTypes': 'jpg,jpeg,png',
    //         'allowedFileSize': (1024 * 1024 * 20),
    //         'fileFolde': 'ProjectAttach'
    //     },
    //     filters: {
    //         max_file_size: '20mb',
    //         mime_types: [
    //             { title: "工程面貌图片", extensions: "jpg,jpeg,png" }
    //         ]
    //     },
    //     uploadCallback: function (e, file, r) {
    //         $('#GcFaceImgProgress').empty();
    //         var html = upTmpl.render(r);
    //         $('#GcFaceImgList').append(html);

    //         resetGcFaceImg();
    //     }
    // });

    //gcFaceImgUploader.init();

    //var gcface = $("#hidGcFaceImgs").val();

    //if (gcface != "") {
    //    var faceImgs = gcface.split('|');

    //    for (var i = 0; i < faceImgs.length; i++) {
    //        var face = faceImgs[i].split(',');
    //        var str = "<li class=\"file-item2\"><a href=\"" + face[0]
    //             + "\" target=\"_blank\" title=\"" + face[1] + "\"><img src=\"" + face[0]
    //             + "\" width=\"50\" height=\"50\" alt=\"" + face[1] + "\" /></a>" +
    //             "<button type=\"button\" class=\"close\" name=\"btnClose\" onclick=\"delOverViewPic(this)\" style=\"position:relative;top:10px;left:10px;\">&times;</button></li>";

    //        $('#GcFaceImgList').append(str);
    //    }
    //}

    try {
        var r = JSON.parse($("#PanoramaImg").val());
        $("#PanoramaImgUp>span").html(ObjectFileFormat(r));
    }
    catch (e) {

    }


    try {
        var r = JSON.parse($("#BgImg").val());
        $("#BgImgUp>span").html(ObjectFileFormat(r));
    }
    catch (e) {

    }
}

//function resetGcFaceImg() {
//    var imgs = "";

//    $('#GcFaceImgList').find('a').each(function () {
//        imgs += $(this).attr('href') + "," + $(this).attr("title") + "|";
//    });

//    if (imgs.length > 0) {
//        imgs = imgs.substr(0, imgs.length - 1);
//    }

//    $("#hidGcFaceImgs").val(imgs);
//}

//function delOverViewPic(obj) {
//    $(obj).closest('li').remove();
//    resetGcFaceImg();
//}


function ObjectFileFormat(r) {
    if (r.fileName != undefined)
        return (r.fileName.length > 10 ? r.fileName.substring(0, 10) + ".." : r.fileName) + "." + r.fileType;
    else
        return "添加上传";
}

$(function () {
    setFileUpload();
});

