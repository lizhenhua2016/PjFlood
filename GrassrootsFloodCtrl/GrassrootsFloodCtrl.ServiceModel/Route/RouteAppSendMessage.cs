using GrassrootsFloodCtrl.Model.AppApi;
using GrassrootsFloodCtrl.ServiceModel.AppApi;
using GrassrootsFloodCtrl.ServiceModel.Common;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Api("发送信息处理")]
    [Route("/AppWarnEvent/GetReceiveAppSendMessage", "Get", Summary = "获取接收消息的列表")]
    public class RouteGetReceiveCountyAppSendMessage : IReturn<AppSendMessageModel>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户账号")]
        public string userName { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "当前页数")]
        public int pageIndex { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "一页多少条")]
        public int pageSize { get; set; }

    }
    [Route("/AppWarnEvent/GetSendAppSendMessage", "Get", Summary = "获取发送消息的列表")]
    public class RouteGetSendCountyAppSendMessage : IReturn<AppSendMessageModel>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户账号")]
        public string userName { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "当前页数")]
        public int pageIndex { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "一页多少条")]
        public int pageSize { get; set; }
    }
    [Route("/AppWarnEvent/UpdateAppSendMessage", "Get", Summary = "更改消息的状态")]
    public class RouteUpdateAppSendMessage : IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "消息的Id")]
        public string id { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户账号")]
        public string userName { get; set; }
    }
    [Route("/AppWarnEvent/GetByIdAppSendMessageDetails", "Get", Summary = "根据Id查询详情")]
    public class RouteByIdAppSendMessageDetails : IReturn<AppSendDetailsModel>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "消息的Id")]
        public string id { get; set; }
    }

    [Route("/AppWarnEvent/GetByWarnInfoIdPerson", "Get", Summary = "根据事件ID获取人员列表")]
    public class RouteGetByWarnInfoIdPerson : IReturn<AppPersonModel>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "任务ID AppWarnInfoID")]
        public int AppWarnInfoID { get; set; }
    }
    [Route("/AppWarnEvent/GetBsByWarnInfoIdPerson", "Get", Summary = "BS端根据事件ID获取人员列表")]
    public class RouteBsGetByWarnInfoIdPerson : IReturn<AppBsPesonModel>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "任务ID AppWarnInfoID")]
        public int AppWarnInfoID { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "当前页数")]
        public int pageIndex { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "一页多少条")]
        public int pageSize { get; set; }
    }


    [Route("/AppWarnEvent/GetByWarnInfoIdCloseEvent", "Get", Summary = "关闭事件")]
    public class RouteGetByWarnInfoIdCloseEvent : IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "任务ID AppWarnInfoID这个关闭时关闭整个任务流程同时在任务列表插入结束时间")]
        public int AppWarnInfoID { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户账号")]
        public string userName { get; set; }
    }
    [Route("/AppWarnEvent/GetEventLevel", "Get", Summary = "获取事件等级列表")]
    public class RouteGetEventLevel : IReturn<AppWarnLevelListModel>
    {

    }
    [Route("/AppWarnEvent/GetPCReceiveAppSendMessage", "Get", Summary = "获取接收消息的列表B/S端手机端不要用")]
    public class RouteGetPCReceiveCountyAppSendMessage : IReturn<AppSendMessageModel>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "事件名称")]
        public string AppWarnEventName { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "消息内容")]
        public string SendMessage { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "开始时间")]
        public DateTime? startDateTime { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "结束时间")]
        public DateTime? endDateTime { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "当前页数")]
        public int pageIndex { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "一页多少条")]
        public int pageSize { get; set; }

    }
    [Route("/AppWarnEvent/GetPCSendAppSendMessage", "Get", Summary = "获取发送消息的列表B/S端手机端不要用")]
    public class RouteGetPCSendCountyAppSendMessage : IReturn<AppSendMessageModel>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = true, DataType = "int", Description = "当前页数")]
        public int pageIndex { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "一页多少条")]
        public int pageSize { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "事件名称")]
        public string AppWarnEventName { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "开始时间")]
        public DateTime? startDateTime { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "结束时间")]
        public DateTime? endDateTime { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "消息内容")]
        public string SendMessage { get; set; }
    }

    [Route("/AppWarnEvent/GetByUserExecutingList", "Get", Summary = "获取当前用户正在执行的任务列表")]
    public class RouteGetByUserExecutingList : IReturn<AppGetByUserExecutingModel>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户账号")]
        public string userName { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户adcd")]
        public string adcd { get; set; }
    }

    [Route("/AppWarnEvent/GetForwardExist", "Get", Summary = "镇级村级转发按钮是否显示")]
    public class RouteGetForwardExist : IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "消息Id")]
        public string Id { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "账户名")]
        public string userName { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "-1为第一次1为点击按钮")]
        public int actionId { get; set; }

    }
    [Route("/AppWarnEvent/AddLoactionAndRecord", "Get", Summary = "村级发送消息的时候记录坐标点和增加履职记录")]
    public class RouteVillageLoaction: IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "消息Id")]
        public string messageId { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "经纬度")]
        public string location { get; set; }

    }
}
