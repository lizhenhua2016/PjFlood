using System.Collections.Generic;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.DangerZone;
using GrassrootsFloodCtrl.ServiceModel.Route;

namespace GrassrootsFloodCtrl.Logic
{
    public interface IDangerZoneManager
    {
        /// <summary>
        /// 获取危险点类型列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<DangerZoneViewModel> GetDangerZoneList(GetDangerZoneList request);

        /// <summary>
        /// 获取危险点类型列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        List<DangerZoneViewModel> GetDangerZone(GetDangerZone request);
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool SaveDangerZone(SaveDangerZone request);
    }
}
