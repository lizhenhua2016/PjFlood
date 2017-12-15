using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack;
using GrassrootsFloodCtrl.ServiceModel.Route;

namespace GrassrootsFloodCtrl.ServiceModel.RouteNoVerify
{
    [Route("/NoVerifyVillagePic/NoVerifyGetVillagePicByAdcdAndYear", "Get", Summary = "获取已上报的行政村防汛防台形势图")]
    [Api("村防汛形势图相关接口")]
    public class NoVerifyGetVillagePicByAdcdAndYear : IReturn<VillagePic>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "年度")]
        public int year { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "审核次数")]
        public int? nums { get; set; }
    }

    [Route("/NoVerifyVillagePic/NoVerifyGetVillagePicList", "Get", Summary = "获取已上报的行政村防汛防台形势图列表")]
    [Api("村防汛形势图相关接口")]
    public class NoVerifyGetVillagePicList : PageQuery, IReturn<BsTableDataSource<VillagePicViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政村adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "年度")]
        public int year { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政村名称")]
        public string adnm { get; set; }
    }

    [Route("/NoVerifyVillagePic/NoVerifyGetVillagePicAdcd", "Get", Summary = "获取行政村防汛防台形势图（已上报和未上报的村名）")]
    [Api("村防汛形势图相关接口")]
    public class NoVerifyGetVillagePicAdcd : PageQuery, IReturn<BsTableDataSource<VillageViewModel>>
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
