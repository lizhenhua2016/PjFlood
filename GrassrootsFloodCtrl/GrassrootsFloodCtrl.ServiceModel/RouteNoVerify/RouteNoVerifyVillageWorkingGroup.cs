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

namespace GrassrootsFloodCtrl.ServiceModel.NoAuditRoute
{
    [Route("/NoVerifyVillageWorkingGroup/NoVerifyGetGroup", "GET", Summary = "接口：查看村下面所有防汛防台工作组责任人，通过村adcd")]
    [Api("防汛防台工作组相关接口")]
    public class NoVerifyGetGroup : PageQuery, IReturn<BsTableDataSource<VillageWorkingGroupViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "审核次数")]
        public int? nums { get; set; }
    }

    [Route("/NoVerifyVillageWorkingGroup/NoVerifyGetList", "GET", Summary = "接口：获取村,通过镇adcd")]
    [Api("防汛防台工作组相关接口")]
    public class NoVerifyGetList : PageQuery, IReturn<BsTableDataSource<VillageViewModel>>
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

    [Route("/NoVerifyVillageWorkingGroup/NoVerifyGetPersonLiableList", "GET", Summary = "接口：获取所有当前账号下所有防汛防台工作组人员")]
    [Api("防汛防台工作组相关接口")]
    public class NoVerifyGetPersonLiableList : PageQuery, IReturn<BsTableDataSource<VillageViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "乡镇adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "关键字")]
        public string key { get; set; }
    }


    #region 上传下载
    [Route("/NoVerifyVillageWorkingGroup/NoVerifyDownVWGFileModel", "GET", Summary = "接口：下载防汛防台工作组责任人模板")]
    [Api("防汛防台工作组相关接口")]
    public class NoVerifyGetVWGFileModel : PageQuery, IReturn<UploadFileViewModel>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }
    //
    [Route("/NoVerifyVillageWorkingGroup/NoVerifyUploadWGFiles", "POST", Summary = "接口：防汛防台工作组批量上传")]
    [Api("防汛防台工作组相关接口")]
    public class NoVerifyUploadWGFiles : PageQuery, IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "文件路径")]
        public string fpath { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }
    [Route("/NoVerifyVillageWorkingGroup/NoVerifyDownLoadOneVillage", "POST", Summary = "接口：下载村防汛防台工作组人员")]
    [Api("防汛防台工作组相关接口")]
    public class NoVerifyDownLoadOneVillage : PageQuery, IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "村名")]
        public string adcdname { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }
    [Route("/NoVerifyVillageWorkingGroup/NoVerifyDownLoadVillage", "POST", Summary = "接口：下载镇防汛防台工作组人员")]
    [Api("防汛防台工作组相关接口")]
    public class NoVerifyDownLoadVillage : PageQuery, IReturn<BaseResult>
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
