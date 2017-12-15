using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.ServiceModel.Village;

namespace GrassrootsFloodCtrl.Logic.Town
{
   public interface ITownManager
   {
        /// <summary>
        /// 获取镇街责任人列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
       BsTableDataSource<TownPersonLiableViewModel> GetTownList(GetTownList request);
        BsTableDataSource<TownPersonLiableViewModel> GetTownList1(GetTownList1 request);
        /// <summary>
        /// 保存镇级责任人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool SaveTown(SaveTown request);
        /// <summary>
        /// 删除镇级防汛防台责任人
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DelTown(string ids);
   
    }
}
