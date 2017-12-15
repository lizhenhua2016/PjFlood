using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack;
using GrassrootsFloodCtrl.ServiceModel.Route;

namespace GrassrootsFloodCtrl.ServiceModel.RouteNoVerify
{
    [Route("/NoVerifyVillageGrid/NoVerifyGetVillageGrid", "GET", Summary = "接口：查看村下面所有网格责任人，通过村adcd")]
    [Api("村网格责任人相关接口")]
    public class NoVerifyGetVillageGrid : PageQuery, IReturn<BsTableDataSource<VillageGridViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "审核次数")]
        public int? nums { get; set; }
    }
    [Route("/NoAuthVillageGrid/NoAuthGetVillageGridList", "GET", Summary = "接口：获取村,通过镇adcd")]
    [Api("村网格责任人相关接口")]
    public class NoAuthGetVillageGridList : PageQuery, IReturn<BsTableDataSource<VillageViewModel>>
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
    [Route("/NoAuthVillageGrid/NoAuthGetGridPersonLiableList", "GET", Summary = "接口：获取所有当前账号下所有责任人人员")]
    [Api("村网格责任人相关接口")]
    public class NoAuthGetGridPersonLiableList : PageQuery, IReturn<BsTableDataSource<VillageGridViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "乡镇adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "关键字")]
        public string key { get; set; }
    }

    [Route("/NoAuthVillageGrid/NoAuthGetVillageGrid", "GET", Summary = "接口：查看村下面所有网格责任人，通过村adcd")]
    [Api("村网格责任人相关接口")]
    public class NoAuthGetVillageGrid : PageQuery, IReturn<BsTableDataSource<VillageGridViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "审核次数")]
        public int? nums { get; set; }
    }
}
