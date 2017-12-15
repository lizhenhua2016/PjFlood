using GrassrootsFloodCtrl.Logic.NoAuthVillageGrid;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;
using GrassrootsFloodCtrl.ServiceModel.Village;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    public class NoAuthVillageService:ServiceBase
    {
        public INoAuthVillageGridManage VillageGridMange { get; set; }
        public BsTableDataSource<VillageViewModel> GET(NoAuthGetVillageGridList request)
        {
            return VillageGridMange.GetVillageGridList(request);
        }

        public BsTableDataSource<VillageGridViewModel> Get(NoAuthGetGridPersonLiableList request)
        {
            return VillageGridMange.GetGridPersonLiableList(request);
        }

        public BsTableDataSource<VillageGridViewModel> Get(NoAuthGetVillageGrid request)
        {
            return VillageGridMange.GetVillageGrid(request);
        }
    }
}