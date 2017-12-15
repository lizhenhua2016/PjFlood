using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.AppApi;
using GrassrootsFloodCtrl.Model.SumAppMessage;
using GrassrootsFloodCtrl.ServiceModel.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.SumAppMessage
{
    public interface ISumAppMessageLogic
    {
        BsTableDataSource<AppSumEventModel> GetSumEventUI(RouteSearchEventModel requset);
        BsTableDataSource<SumAppWarnInfoModel> GetSumWarnInfoUI(RouteSearcWarnInfoModel requset);
        BsTableDataSource<SumReadModel> GetSumReadUI(RouteSearcReadModel requset);
    }
}
