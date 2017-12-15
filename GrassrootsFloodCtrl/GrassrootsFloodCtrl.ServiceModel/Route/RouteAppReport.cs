using System.Collections.Generic;
using GrassrootsFloodCtrl.ServiceModel.AppReport;
using ServiceStack;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Api("手机端app报表")]
    [Route("/AppReport/AppReportByVillageAdcdAndWarninfoid", "post", Summary = "通过小事件id和村adcd获取报表")]
    public class RouteAppReportByVillageAdcdAndWarninfoid : PageQuery, IReturn<AppReportByVillageAdcdAndWarninfoid>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = true, DataType = "int", Description = "小事件id")]
        public int warninfoid { get; set; }
    }

    [Api("手机端报表")]
    [Route("/AppReport/RouteAppRecordByMessageId", "post", Summary = "通过meessageid获取履职信息")]
    public class RouteAppRecordByMessageId : IReturn<List<AppVillageOnDuty>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "messageid")]
        public string MessageId { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "岗位")]
        public string postcode { get; set; }
    }

    [Api("手机端报表")]
    [Route("/AppReport/RouteAppDutyPost", "post", Summary = "通过meessageid获取岗位信息")]
    public class RouteAppDutyPost : IReturn<List<AppPostCode>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "messageid")]
        public string MessageId { get; set; }
    }

    [Api("手机端报表")]
    [Route("/AppReport/RouteAppLocation", "post", Summary = "通过messageid获取location")]
    public class RouteAppLocation : IReturn<List<AppLocation>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "messageid")]
        public string messageid { get; set; }
    }

    [Api("手机端报表")]
    [Route("/AppReport/RouteGetRegMobile", "post", Summary = "app注册人员统计")]
    public class RouteGetRegMobile : IReturn<List<AppLocation>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "messageid")]
        public string mobile { get; set; }
    }
}