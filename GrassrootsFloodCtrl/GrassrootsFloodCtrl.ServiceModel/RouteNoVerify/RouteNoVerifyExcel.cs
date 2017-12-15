using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Excel;
using ServiceStack;
using ServiceStack.Web;

namespace GrassrootsFloodCtrl.ServiceModel.RouteNoVerify
{
    [Route("/NoVerifyExcel/NoVerifyexportExcel", "Get", Summary = "导出责任人")]
    [Api("责任人下载导入相关接口")]
    public class NoVerifyexportExcel:IReturn<UploadFileViewModel>
    {
        [ApiMember(IsRequired = true, DataType = "sting", Description = "文件名称")]
        public string fileName { get; set; }

        [ApiMember(IsRequired = true, DataType = "int", Description = "第一行需要合并的单元格数量")]
        public int mergeCellNum { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "第一行需要合并的内容")]
        public string mergeCellContent { get; set; }

        [ApiMember(IsRequired = true, DataType = "int", Description = "下载的类型（0:行政村受灾信息，1：行政村防汛防台工作组，2：行政村网格，3：行政村转移人员清单，4：镇级防汛防台责任，5：行政村信息，6.添加县级城防体系人员）")]
        public int typeId { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年度")]
        public int year { get; set; }
        [ApiMember(IsRequired = false, DataType = "sting", Description = "村  行区划编码")]
        public string adcd { get; set; }
    }

    [Route("/NoVerifyExcel/NoVerifydownLoadExcelModel", "Get", Summary = "获取责任人下载模板")]
    [Api("责任人下载导入相关接口")]
    public class NoVerifydownLoadExcelModel : IReturn<UploadFileViewModel>
    {
        [ApiMember(IsRequired = true, DataType = "sting", Description = "文件名称")]
        public string fileName { get; set; }

        [ApiMember(IsRequired = true, DataType = "int", Description = "第一行需要合并的单元格数量")]
        public int mergeCellNum { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "第一行需要合并的内容")]
        public string mergeCellContent { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "下载的类型（0:行政村受灾信息，1：行政村防汛防台工作组，2：行政村网格，3：行政村转移人员清单，4：镇级防汛防台责任，5：行政村信息，6.添加县级城防体系人员）")]
        public int typeId { get; set; }
    }

    [Route("/NoVerifyExcel/NoVerifyImportExcel", "Post", Summary = "导入责任人")]
    [Api("责任人下载导入相关接口")]
    public class NoVerifyImportExcel : IReturn<ExcelViewModel>
    {

        [ApiMember(IsRequired = true, DataType = "int", Description = "年度")]
        public int year { get; set; }

        [ApiMember(IsRequired = true, DataType = "sting", Description = "文件路径")]
        public string filePath { get; set; }

        [ApiMember(IsRequired = true, DataType = "int", Description = "类型（0:行政村受灾信息，1：行政村防汛防台工作组，2：行政村网格，3：行政村转移人员清单，4：镇级防汛防台责任，5：行政村信息，6.添加县级城防体系人员）")]
        public int typeId { get; set; }
    }
}
