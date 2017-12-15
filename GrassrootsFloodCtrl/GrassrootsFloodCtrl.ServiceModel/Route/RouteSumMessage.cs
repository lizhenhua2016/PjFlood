using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.SumAppMessage;
using GrassrootsFloodCtrl.ServiceModel.AppApi;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Route("/SumAppMessage/EventList", "GET", Summary = "获取事件的列表")]
    [Api("统计消息报表")]
    public class RouteSearchEventModel:PageQuery,IReturn<BsTableDataSource<AppSumEventModel>>
    {
        public string CityAdcd { get; set; }
        public string CountryAdcd { get; set; }
        public string EventName { get; set; }
    }
    [Route("/SumAppMessage/WarnInfoList", "GET", Summary = "获取小事件的列表")]
    [Api("统计消息报表")]
    public class RouteSearcWarnInfoModel : PageQuery, IReturn<BsTableDataSource<SumAppWarnInfoModel>>
    {
        public string EventId { get; set; }
    }
    [Route("/SumAppMessage/ReadList", "GET", Summary = "获取事件的列表")]
    [Api("统计消息报表")]
    public class RouteSearcReadModel : PageQuery, IReturn<BsTableDataSource<SumReadModel>>
    {
        public string WarnInfoId { get; set; }
        public bool IsRead { get; set; }
    }
}
