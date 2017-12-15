using GrassrootsFloodCtrl.Logic.SumAppMessage;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.AppApi;
using GrassrootsFloodCtrl.Model.SumAppMessage;
using GrassrootsFloodCtrl.ServiceModel.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    public class AppSumMessageService:ServiceBase
    {
        public ISumAppMessageLogic sumMessage { get; set; }
        public BsTableDataSource<AppSumEventModel> Get(RouteSearchEventModel request)
        {
            return sumMessage.GetSumEventUI(request);
        }
        public BsTableDataSource<SumAppWarnInfoModel> Get(RouteSearcWarnInfoModel request)
        {
            return sumMessage.GetSumWarnInfoUI(request);
        }
        public BsTableDataSource<SumReadModel> Get(RouteSearcReadModel request)
        {
            return sumMessage.GetSumReadUI(request);
        }
    }
}
