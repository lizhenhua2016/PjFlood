using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;
using GrassrootsFloodCtrl.ServiceModel.Village;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.NoVerify
{
    public interface INoVerifyVillageGridManage
    {
        BsTableDataSource<VillageGridViewModel> NoVerifyGetVillageGrid(NoVerifyGetVillageGrid request);
    }
}
