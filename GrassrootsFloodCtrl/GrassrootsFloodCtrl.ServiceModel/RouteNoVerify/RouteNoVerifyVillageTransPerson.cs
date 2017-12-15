using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.RouteNoVerify
{
    [Route("/NoVerifyVillageTransferPerson/NoVerifyGetVillageTransferPerson", "Get", Summary = "获取村危险区转移人员,通过乡镇adcd")]
    [Api("村危险区转移人员相关接口")]
    public class NoVerifyGetVillageTransferPerson : PageQuery, IReturn<BsTableDataSource<VillageTransferPersonViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年度")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "审核次数")]
        public int? nums { get; set; }
    }

    [Route("/NoVerifyVillageTransferPerson/NoVerifyGetVillageReportNum", "Get", Summary = "获取行政村上报和未上报统计信息")]
    [Api("村危险区转移人员相关接口")]
    public class NoVerifyGetVillageReportNum : IReturn<List<VillageNumViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "年度（如果不传值，默认为当前年度）")]
        public int Year { get; set; }
    }

    [Route("/NoVerifyVillageTransferPerson/NoVerifyGetVillageTransferPersonAdcd", "Get", Summary = "获取危险区转移人员行政村 （已上报和未上报的村名）")]
    [Api("村危险区转移人员相关接口")]
    public class NoVerifyGetVillageTransferPersonAdcd : PageQuery, IReturn<BsTableDataSource<VillageViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政村名称")]
        public string adnm { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "是否已上报（0：未上报，1：已上报,2:全部）")]
        public int type { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年度")]
        public int year { get; set; }
    }
}
