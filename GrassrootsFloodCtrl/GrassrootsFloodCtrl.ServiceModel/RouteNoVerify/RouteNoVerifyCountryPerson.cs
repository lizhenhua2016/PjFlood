using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.CountryPerson;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Country;
using GrassrootsFloodCtrl.ServiceModel.Excel;
using GrassrootsFloodCtrl.ServiceModel.NoAuditRoute;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.RouteNoVerify
{

    #region 批量导入人县级城防体系接口
    [Route("/NoVerifyCountryPerson/NoVerifyImportExcel", "Post", Summary = "导入责任人")]
    [Api("县级城防体系接口")]
    public class NoVerifyCountryImportExcel : IReturn<ReturnInsertStatus>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "年度")]
        public int year { get; set; }

        [ApiMember(IsRequired = true, DataType = "sting", Description = "文件路径")]
        public string filePath { get; set; }

        [ApiMember(IsRequired = true, DataType = "int", Description = "类型（6.添加县级城防体系人员）")]
        public int typeId { get; set; }
    }
    #endregion
    #region 获取县级城防体系人员列表
    [Route("/NoVerifyCountryPerson/NoVerifyGetCountryPersonList", "Get", Summary = "获取县级责任人列表接口")]
    [Api("县级城防体系接口")]
    public class NoVerifyGetCountryPersonList : PageQuery, IReturn<BsTableDataSource<CountryPersonServiceModel>>
    {
        [ApiMember(IsRequired = false, Description = "自增Id", DataType = "int")]
        public int Id { get; set; }
        [ApiMember(IsRequired = false, Description = "岗位", DataType = "string")]
        public string Position { get; set; }
        [ApiMember(IsRequired = false, Description = "职务", DataType = "string")]
        public string Post { get; set; }
        [ApiMember(IsRequired = false, Description = "姓名", DataType = "string")]
        public string name { get; set; }
        [ApiMember(IsRequired = false, Description = "adcd", DataType = "string")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, Description = "年度", DataType = "int")]
        public int? year { get; set; }
        [ApiMember(IsRequired = true, Description = "编码", DataType = "string")]
        public string acad { get; set; }
        [ApiMember(IsRequired = false, Description = "审核次数", DataType = "int")]
        public int? nums { get; set; }
    }
    [Route("/NoVerifyCountryPerson/NoVerifyGetCountryPersonList1", "Get", Summary = "同人多岗统计")]
    [Api("县级城防体系接口")]
    public class NoVerifyGetCountryPersonList1 : PageQuery, IReturn<BsTableDataSource<CountryPersonServiceModel>>
    {
        [ApiMember(IsRequired = true, Description = "adcd", DataType = "string")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, Description = "岗位", DataType = "string")]
        public string post { get; set; }

        [ApiMember(IsRequired = false, Description = "职位", DataType = "string")]
        public string position { get; set; }

        [ApiMember(IsRequired = false, Description = "姓名", DataType = "string")]
        public string name { get; set; }

        [ApiMember(IsRequired = false, Description = "年份", DataType = "int")]
        public int? year { get; set; }
    }
    #endregion
    #region 导出execl
    [Route("/NoVerifyCountryPerson/NoVerifyGetExportExecl", "Get", Summary = "获取县级责任人列表接口")]
    [Api("县级城防体系接口")]
    public class NoVerifyGetExportExecl : IReturn<ExportExeclCountryPerson>
    {
        [ApiMember(IsRequired = true, Description = "年度", DataType = "int")]
        public int year { get; set; }

        [ApiMember(IsRequired = true, Description = "类型", DataType = "int")]
        public int typeId { get; set; }

        [ApiMember(IsRequired = true, DataType = "int", Description = "第一行需要合并的单元格数量")]
        public int mergeCellNum { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "第一行需要合并的内容")]
        public string mergeCellContent { get; set; }

    }
    #endregion
    #region 保存县级责任人接口
    [Route("/NoVerifyCountryPerson/NoVerifySaveCountryPerson", "Post", Summary = "保存县级责任人接口")]
    [Api("县级责任人相关接口")]
    public class NoVerifySaveCountryPerson : IReturn<bool>
    {
        [ApiMember(IsRequired = false, Description = "自增ID", DataType = "int")]
        public int Id { get; set; }
        [ApiMember(IsRequired = false, Description = "岗位", DataType = "string")]
        public string Position { get; set; }
        [ApiMember(IsRequired = false, Description = "职务", DataType = "string")]
        public string Post { get; set; }
        [ApiMember(IsRequired = true, Description = "姓名", DataType = "string")]
        public string name { get; set; }
        [ApiMember(IsRequired = false, Description = "手机号码", DataType = "string")]
        public string Mobile { get; set; }
        [ApiMember(IsRequired = false, Description = "备注", DataType = "string")]
        public string Remark { get; set; }
    }
    #endregion


    [Route("/NoVerifyCountryPerson/NoVerifyDeleteCountryPerson", "Post", Summary = "删除镇街责任人接口")]
    [Api("县级责任人相关接口")]
    public class NoVerifyDeleteCountryPerson : IReturn<bool>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "多个ID以逗号隔开")]
        public string ids { get; set; }
    }

    [Route("/NoVerifyCountryPerson/NoVerifyAddCountryCheck", "Get", Summary = "提交审核")]
    [Api("县级责任人相关接口")]
    public class NoVerifyAddCountryCheck : IReturn<ReturnCountyCheck>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "")]
        public int year { get; set; }
    }

    [Route("/NoVerifyCountryPerson/NoVerifyCheckStatus", "Get", Summary = "提交审核")]
    [Api("县级责任人相关接口")]
    public class NoVerifyCountryStatus : IReturn<ReturnCountyCheck>
    {

    }
}
