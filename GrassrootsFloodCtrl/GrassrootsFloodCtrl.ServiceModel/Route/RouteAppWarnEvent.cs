using GrassrootsFloodCtrl.ServiceModel.AppApi;
using GrassrootsFloodCtrl.ServiceModel.Common;
using ServiceStack;
using System.Collections.Generic;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Route("/AppWarnEvent/AddAppWarnEvent", "POST", Summary = "接口:增加事件信息,默认不启动预警")]
    [Api("预警事件")]
    public class RouteAddAppWarnEvent : IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户账号")]
        public string userName { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "事件名称")]
        public string EventName { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
    }

    [Route("/AppWarnEvent/GetWarnSelect", "Get", Summary = "接口:获取改该县要发送预警的列表")]
    [Api("预警事件")]
    public class RouteGetAppWarnSelect : IReturn<AppWarnViewModel>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户adcd")]
        public string adcd { get; set; }
    }

    [Route("/AppWarnEvent/GetTownWarnSelect", "Get", Summary = "接口:获取镇级要发送预警的列表")]
    [Api("预警事件")]
    public class RouteTownGetAppWarnSelect : IReturn<AppWarnViewModel>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户adcd")]
        public string adcd { get; set; }
    }

    [Route("/AppWarnEvent/GetWarnList", "Get", Summary = "接口:获取所有事件的列表")]
    [Api("预警事件")]
    public class RouteGetAppWarnList : IReturn<AppWarnViewModel>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "用户adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "事件名称")]
        public string EventName { get; set; }
    }

    [Route("/AppWarnEvent/SearchWarnEventName", "Get", Summary = "模糊查询任务名称")]
    [Api("预警事件")]
    public class RouteSearchWarnEventName : IReturn<AppWarnEventMoel>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "任务名称")]
        public string WarnEventName { get; set; }
    }

    [Route("/AppWarnEvent/SearchWarnInfoName", "Get", Summary = "模糊查事件名称")]
    [Api("预警事件")]
    public class RouteSearchWarnInfoName : IReturn<AppWarnEventMoel>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "事件名称")]
        public string WarnInfoName { get; set; }
    }

    [Route("/AppWarnEvent/GetMessageNoReadCount", "Get", Summary = "查询消息未读的数量")]
    [Api("预警事件")]
    public class RouteGetMessageNoReadCount : IReturn<AppMessageRemind>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "手机号")]
        public string userName { get; set; }
    }

    [Route("/AppWarnEvent/RouteGetWarnListByWarnId", "post", Summary = "根据事件ID获取全部的预警等级信息")]
    [Api("预警事件")]
    public class RouteGetWarnListByWarnId : IReturn<List<Model.AppApi.AppWarnInfo>>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "事件的ID")]
        public int WarnId { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "发布事件的Adcd")]
        public string UserAdcd { get; set; }
    }

    [Route("/AppWarnEvent/RouteGetWarnListByVillageAdcd", "post", Summary = "点击村弹出应急管理大事件")]
    [Api("预警事件")]
    public class RouteGetWarnListByVillageAdcd : IReturn<AppWarnEventMoel2>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd")]
        public string adcd { get; set; }
    }
}