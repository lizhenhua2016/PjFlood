using GrassrootsFloodCtrl.ServiceModel.Common;
using ServiceStack;
using System;
using GrassrootsFloodCtrl.ServiceModel.Route;
using Dy.Common;
using GrassrootsFloodCtrl.Logic.ZZTX;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    [Authenticate]
    public class UploadFileService : ServiceBase
    {
        public IZZTXManager ZZTXManager { get; set; }

        public UploadFileViewModel POST(RouteUploadFile request)
        {
            var uploadVm = new UploadFileViewModel();
            var files = Request.Files;
            if (files.Length == 1)
            {
                var file = files[0];

                #region 上传文件
                try
                {
                    if (null != file && file.ContentLength != 0L)
                    {
                        var info = ZZTXManager.GetADCDInfoByADCD(adcd);
                        if (info == null) {
                            throw new Exception("用户信息为空请重新登陆！");
                        }
                        var fileFolde = "/Upload/" + (info == null ? adcd : info.adnm) + (request.fileFolde != null && request.fileFolde.Trim() != "" ? "/" + request.fileFolde : "");// " / Upload/funds/DocumentBasis";

                        var allowedFileTypes = request.allowedFileTypes;// "doc,docx,pdf";
                        var allowedFileSize = request.allowedFileSize != 0 ? request.allowedFileSize : 20480;// 20480;

                        var fileName = file.FileName;
                        if (fileName.Contains(":"))
                        {
                            var lastIndex = fileName.LastIndexOf("\\");
                            var leftLength = fileName.Length - lastIndex - 1;

                            fileName = fileName.Substring(lastIndex + 1, leftLength);
                        }

                        /*第一步上传文件*/
                        FileUploader uploader = new FileUploader(fileFolde, DateTime.Now.ToString("yyyyMMddHHmmssff-") + fileName.Trim(), allowedFileTypes, allowedFileSize);
                       
                        var flag = uploader.UploadFile(file);
                        uploadVm.isSuccess = flag;
                        if (flag)
                        {
                            uploadVm.fileName = fileName.Substring(0, fileName.LastIndexOf("."));
                            uploadVm.fileSrc = uploader.SaveFilePath;
                            uploadVm.flieSize = (file.ContentLength / 1024) + "kb";
                            uploadVm.fileType = fileName.Substring(fileName.LastIndexOf(".") + 1, fileName.Length - fileName.LastIndexOf(".") - 1);
                            uploadVm.message = "上传成功";
                        }
                        else
                            uploadVm.message = "上传失败";
                    }
                }catch(Exception ex)
                {
                    uploadVm.isSuccess = false;
                    uploadVm.message = "文件超出规定大小5MB";
                }
                #endregion 上传文件
            }
            return uploadVm;
        }
    }
}
