using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Village;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.Model.Supervise;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    
    [Route("/VillageWorkingGroup/GetList", "GET", Summary = "接口：获取村,通过镇adcd")]
    [Api("防汛防台工作组相关接口")]
    public class GetList : PageQuery,IReturn<BsTableDataSource<VillageViewModel>>
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
    [Route("/VillageWorkingGroup/GetGroup","GET",Summary = "接口：查看村下面所有防汛防台工作组责任人，通过村adcd")]
    [Api("防汛防台工作组相关接口")]
    public class GetGroup : PageQuery, IReturn<BsTableDataSource<VillageWorkingGroupViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "审核次数")]
        public int? nums { get; set; }
    }
    [Route("/VillageWorkingGroup/GetGroup1", "GET", Summary = "接口：同人多岗统计")]
    [Api("防汛防台工作组相关接口")]
    public class GetGroup1 : PageQuery, IReturn<BsTableDataSource<StatiscPerson>>
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
    [Route("/VillageWorkingGroup/GetGroupOne", "GET", Summary = "接口：查看村下面所有防汛防台工作组责任人，通过村adcd")]
    [Api("防汛防台工作组相关接口")]
    public class GetGroupOne : PageQuery, IReturn<BsTableDataSource<StaticsVillageGroup>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }
    [Route("/VillageWorkingGroup/SaveGroup", "POST", Summary = "接口：新增村下面责任人")]
    [Api("防汛防台工作组相关接口")]
    public class SaveGroup : PageQuery, IReturn<BaseResult>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "ID")]
        public int? id { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string villageadcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "责任人")]
        public string personLiable { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "岗位")]
        public string post { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "职位")]
        public string position { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "手机")]
        public string handphone { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "备注")]
        public string remark { get; set; }
    }
    [Route("/VillageWorkingGroup/DeleteGroup", "POST", Summary = "接口：删除村下面的责任人")]
    [Api("防汛防台工作组相关接口")]
    public class DeleteGroup : PageQuery, IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "责任人id")]
        public string id { get; set; }
    }
    [Route("/VillageWorkingGroup/DelVillageGroup", "POST", Summary = "接口：删除村")]
    [Api("防汛防台工作组相关接口")]
    public class DelVillageGroup : PageQuery, IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村ADCD")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "年份")]
        public string year { get; set; }
    }
    
    [Route("/VillageWorkingGroup/GetPersonLiableList", "GET", Summary = "接口：获取所有当前账号下所有防汛防台工作组人员")]
    [Api("防汛防台工作组相关接口")]
    public class GetPersonLiableList : PageQuery, IReturn<BsTableDataSource<VillageViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "乡镇adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "关键字")]
        public string key { get; set; }
    }
    [Route("/VillageWorkingGroup/GetPersonLiable", "GET", Summary = "接口：获取一条防汛防台工作组人员信息")]
    [Api("防汛防台工作组相关接口")]
    public class GetPersonLiable : PageQuery, IReturn<BsTableDataSource<VillageViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "乡镇adcd")]
        public int? id { get; set; }
    }
    #region 上传下载
    [Route("/VillageWorkingGroup/DownVWGFileModel", "GET", Summary = "接口：下载防汛防台工作组责任人模板")]
    [Api("防汛防台工作组相关接口")]
    public class GetVWGFileModel : PageQuery, IReturn<UploadFileViewModel>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }
    //
    [Route("/VillageWorkingGroup/UploadWGFiles", "POST", Summary = "接口：防汛防台工作组批量上传")]
    [Api("防汛防台工作组相关接口")]
    public class UploadWGFiles : PageQuery, IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "文件路径")]
        public string fpath { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }
    [Route("/VillageWorkingGroup/DownLoadOneVillage", "POST",Summary = "接口：下载村防汛防台工作组人员")]
    [Api("防汛防台工作组相关接口")]
    public class DownLoadOneVillage : PageQuery, IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "村名")]
        public string adcdname { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }
    [Route("/VillageWorkingGroup/DownLoadVillage", "POST", Summary = "接口：下载镇防汛防台工作组人员")]
    [Api("防汛防台工作组相关接口")]
    public class DownLoadVillage : PageQuery, IReturn<BaseResult>
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
