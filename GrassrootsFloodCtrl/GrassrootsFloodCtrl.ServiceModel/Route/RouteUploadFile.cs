using ServiceStack;
using ServiceStack.Web;
using System.Web;
using GrassrootsFloodCtrl.ServiceModel.Common;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Route("/UploadFile", "POST", Summary="上传文件接口")]
    [Api("上传文件相关接口")]
    public class RouteUploadFile : IReturn<UploadFileViewModel>
    {
        [ApiMember(IsRequired = true, DataType = "sting", Description = "文件允许类型")]
        public string allowedFileTypes { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "文件允许大小")]
        public int allowedFileSize { get; set; }

        [ApiMember(IsRequired = false, DataType = "sting", Description = "文件保存目录")]
        public string fileFolde { get; set; }

        [ApiMember(IsRequired = true, DataType = "file", Description = "上传文件")]
        public HttpPostedFile PostFile { get; set; }

        public IHttpFile httpFile { get; set; }
    }

    [Route("/UploadFile/GetFileModel", "POST", Summary = "下载模板接口")]
    [Api("上传文件相关接口")]
    public class GetFileModel : IReturn<UploadFileViewModel>
    {
        [ApiMember(IsRequired = true, DataType = "sting", Description = "文件名称")]
        public string fileName { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "第一行需要合并的单元格数量")]
        public int mergeCellNum { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "第一行需要合并的内容")]
        public string mergeCellContent { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "下载的类型（0：行政村防汛防台工作组，1：行政村网格，2：行政村转移人员清单，3：镇级防汛防台责任）")]
        public int typeId { get; set; }
    }


}