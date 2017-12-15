using GrassrootsFloodCtrl.ServiceModel.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Logic.Town;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Town;
using ServiceStack;
using GrassrootsFloodCtrl.ServiceModel.Village;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    [Authenticate]
    public  class TownService:ServiceBase
    {
        public ITownManager TownManager { get; set; }

        public BsTableDataSource<TownPersonLiableViewModel> Get(GetTownList request)
        {
            return TownManager.GetTownList(request);
        }
        public BsTableDataSource<TownPersonLiableViewModel> Get(GetTownList1 request)
        {
            return TownManager.GetTownList1(request);
        }
        public bool Post(SaveTown request)
        {
            return TownManager.SaveTown(request);
        }

        public bool Post(DelTown request)
        {
            return TownManager.DelTown(request.ids);
        }
    }
}
