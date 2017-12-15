using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.CApp;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.NoAuditRoute;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.RouteNoVerify
{
    [Route("/NoVerifySupervise/NoVerifySPersonLiable", "Post", Summary = "接口：责任考核")]
    [Api("监督考核")]
    public class NoVerifySPersonLiable : IReturn<SuperviseModel>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "市县adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "行政层级id")]
        public int? grid { get; set; }
    }

    [Route("/NoVerifySupervise/NoVerifyGetPersonLiabelList", "Post", Summary = "接口：镇级责任人汇总")]
    [Api("监督考核")]
    public class NoVerifyGetPersonLiabelList : PageQuery, IReturn<BsTableDataSource<ADCDDisasterViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "类型id")]
        public int? pltype { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "职务")]
        public string position { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "岗位")]
        public string post { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "姓名")]
        public string key { get; set; }
    }

}
