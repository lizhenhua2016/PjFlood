using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Route("/VillageGrid/GetVillageGridList", "GET", Summary = "接口：获取村,通过镇adcd")]
    [Api("村网格责任人相关接口")]
    public class GetVillageGridList : PageQuery, IReturn<BsTableDataSource<VillageViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "状态0/1")]
        public int? status { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "关键字")]
        public string key { get; set; }
    }
    [Route("/VillageGrid/GetVillageGrid", "GET", Summary = "接口：查看村下面所有网格责任人，通过村adcd")]
    [Api("村网格责任人相关接口")]
    public class GetVillageGrid : PageQuery, IReturn<BsTableDataSource<VillageGridViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "审核次数")]
        public int? nums { get; set; }
    }
    [Route("/VillageGrid/GetVillageGrid1", "GET", Summary = "接口：同人同港处理")]
    [Api("村网格责任人相关接口")]
    public class GetVillageGrid1 : PageQuery, IReturn<BsTableDataSource<StatiscPerson>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "审核次数")]
        public int? nums { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "姓名")]
        public string name { get; set; }

        [ApiMember(IsRequired = false, Description = "范围类型", DataType = "int")]
        public int? fid { get; set; }

    }
    [Route("/VillageGrid/GetVillageGridOne", "GET", Summary = "接口：查看村下面所有网格责任人，通过村adcd")]
    [Api("村网格责任人相关接口")]
    public class GetVillageGridOne : PageQuery, IReturn<BsTableDataSource<VillageGridViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }
    [Route("/VillageGrid/SaveVillageGrid", "POST", Summary = "接口：新增村下面责任人")]
    [Api("村网格责任人相关接口")]
    public class SaveVillageGrid : PageQuery, IReturn<BaseResult>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "ID")]
        public int? id { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string villageadcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "责任人")]
        public string personLiable { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "村网格名")]
        public string villagegridname { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "网格名")]
        public string gridname { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "职位")]
        public string position { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "手机")]
        public string handphone { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "备注")]
        public string remark { get; set; }
    }
    [Route("/VillageGrid/DeleteVillageGrid", "POST", Summary = "接口：删除村下面的责任人")]
    [Api("村网格责任人相关接口")]
    public class DeleteVillageGrid : PageQuery, IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "责任人id")]
        public string id { get; set; }
    }
    [Route("/VillageGrid/DelVillageGrid", "POST", Summary = "接口：删除村")]
    [Api("村网格责任人相关接口")]
    public class DelVillageGrid : PageQuery, IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "年份")]
        public string year { get; set; }
    }
    
    [Route("/VillageGrid/GetGridPersonLiableList", "GET", Summary = "接口：获取所有当前账号下所有责任人人员")]
    [Api("村网格责任人相关接口")]
    public class GetGridPersonLiableList : PageQuery, IReturn<BsTableDataSource<VillageGridViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "乡镇adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "关键字")]
        public string key { get; set; }
    }
    [Route("/VillageGrid/GetGridPersonLiable", "GET", Summary = "接口：获取一条网格责任人人员信息")]
    [Api("村网格责任人相关接口")]
    public class GetGridPersonLiable : PageQuery, IReturn<BsTableDataSource<VillageGridViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "乡镇adcd")]
        public int? id { get; set; }
    }

    #region 上传下载
    [Route("/VillageGrid/DownGridFileModel", "GET", Summary = "接口：下载网格责任人模板")]
    [Api("防汛防台工作组相关接口")]
    public class DownGridFileModel : PageQuery, IReturn<UploadFileViewModel>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }
    //
    [Route("/VillageGrid/UploadGridFiles", "POST", Summary = "接口：网格责任人批量上传")]
    [Api("防汛防台工作组相关接口")]
    public class UploadGridFiles : PageQuery, IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "文件路径")]
        public string fpath { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }
    [Route("/VillageGrid/DownLoadOneGrid", "POST", Summary = "接口：下载村网格责任人人员")]
    [Api("防汛防台工作组相关接口")]
    public class DownLoadOneGrid : PageQuery, IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "村名")]
        public string adcdname { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }
    [Route("/VillageGrid/DownLoadGrid", "POST", Summary = "接口：下载镇网格责任人人员")]
    [Api("防汛防台工作组相关接口")]
    public class DownLoadGrid : PageQuery, IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "镇名")]
        public string adcdname { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }
    #endregion

}
