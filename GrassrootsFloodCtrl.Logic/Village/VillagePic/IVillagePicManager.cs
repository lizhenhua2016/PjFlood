using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Village;

namespace GrassrootsFloodCtrl.Logic.Village
{
    /// <summary>
    /// 行政村形势图
    /// </summary>
    public interface IVillagePicManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageViewModel> GetVillagePicAdcd(GetVillagePicAdcd request);
        /// <summary>
        /// 保存形势图路径等等
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool SaveVillagePic(SaveVillagePic request);
        /// <summary>
        /// 删除行政村防汛防台形势图
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool DelVillagePic(DelVillagePic request);
        /// <summary>
        /// 获取已上报的行政村防汛防台形势图
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        VillagePic GetVillagePicByAdcdAndYear(GetVillagePicByAdcdAndYear request);
        /// <summary>
        /// 获取已上报的行政村防汛防台形势图列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillagePicViewModel> GetVillagePicList(GetVillagePicList request);
    }
}
