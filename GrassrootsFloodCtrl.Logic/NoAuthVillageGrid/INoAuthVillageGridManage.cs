using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;
using GrassrootsFloodCtrl.ServiceModel.Village;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.NoAuthVillageGrid
{
    public interface INoAuthVillageGridManage
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageViewModel> GetVillageGridList(NoAuthGetVillageGridList request);

        BsTableDataSource<VillageGridViewModel> GetGridPersonLiableList(NoAuthGetGridPersonLiableList request);

        BsTableDataSource<VillageGridViewModel> GetVillageGrid(NoAuthGetVillageGrid request);
    }
}
