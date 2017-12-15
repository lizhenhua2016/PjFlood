using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.Model.Sys;
using GrassrootsFloodCtrl.ServiceModel.Sys;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Common;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Route("/LogMy/GetLogStatisList", "POST", Summary = "市县统计")]
    [Api("更新日志")]
    public class GetLogStatisList : IReturn<SuperviseModel>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "起始时间")]
        public string starttime { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "结束时间")]
        public string endtime { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
    }

    [Route("/LogMy/GetLogList", "GET", Summary = "乡镇及其村级更新日志")]
    [Api("更新日志")]
    public class GetLogList : PageQuery, IReturn<BsTableDataSource<LogInfoViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "起始时间")]
        public string starttime { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "结束时间")]
        public string endtime { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "类型")]
        public string typename { get; set; }
    }

    [Route("/LogMy/GetAuditDate", "GET", Summary = "乡镇审核日期")]
    [Api("更新日志")]
    public class GetAuditDate : IReturn<BaseResult>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "镇名")]
        public string adnm { get; set; }
    }

}
