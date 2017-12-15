using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.NoTownRoute;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.ServiceModel.Village;

namespace GrassrootsFloodCtrl.Logic.NoAuthticationTown
{
    public interface NoAuthticationITownManager
    {
        /// <summary>
        /// 获取镇街责任人列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
       BsTableDataSource<TownPersonLiableViewModel> NoAuthticationGetTownList(NoAuthticationGetTownList request);
        BsTableDataSource<TownPersonLiableViewModel> NoAuthticationGetTownList1(NoAuthticationGetTownList1 request);
        /// <summary>
        /// 保存镇级责任人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool NoAuthticationSaveTown(NoAuthticationSaveTown request);
        /// <summary>
        /// 删除镇级防汛防台责任人
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool NoAuthticationDelTown(string ids);
   
    }
}
