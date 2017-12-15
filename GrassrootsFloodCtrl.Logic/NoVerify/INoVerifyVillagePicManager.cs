using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Village;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.NoVerify
{
    public interface INoVerifyVillagePicManager
    {
        /// <summary>
        /// 获取已上报的行政村防汛防台形势图
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        VillagePic NoVerifyGetVillagePicByAdcdAndYear(NoVerifyGetVillagePicByAdcdAndYear request);

        /// <summary>
        /// 获取已上报的行政村防汛防台形势图列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillagePicViewModel> GetVillagePicList(NoVerifyGetVillagePicList request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageViewModel> GetVillagePicAdcd(NoVerifyGetVillagePicAdcd request);
    }
}
