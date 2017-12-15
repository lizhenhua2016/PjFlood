using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Route("/QR/QRGroupOne", "GET", Summary = "接口：查看村下面所有防汛防台工作组责任人，通过村adcd")]
    [Api("防汛防台工作组相关接口")]
    public class QRGroupOne : PageQuery, IReturn<BsTableDataSource<StaticsVillageGroup>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/QR/QRVillageGrid", "GET", Summary = "接口：查看村下面所有网格责任人，通过村adcd")]
    [Api("村网格责任人相关接口")]
    public class QRVillageGrid : PageQuery, IReturn<BsTableDataSource<VillageGridViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }


    [Route("/QR/QRVillageTransferPerson", "Get", Summary = "获取村危险区转移人员,通过乡镇adcd")]
    [Api("村危险区转移人员相关接口")]
    public class QRVillageTransferPerson : PageQuery, IReturn<BsTableDataSource<VillageTransferPersonViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年度")]
        public int? year { get; set; }
    }

    [Route("/QR/QRVillagePicByAdcdAndYear", "Get", Summary = "获取已上报的行政村防汛防台形势图")]
    [Api("村防汛形势图相关接口")]
    public class QRVillagePicByAdcdAndYear : IReturn<VillagePic>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = true, DataType = "int", Description = "年度")]
        public int? year { get; set; }

    }
}
