using System.Collections.Generic;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Village;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.ServiceModel.Common;

namespace GrassrootsFloodCtrl.Logic.Village
{
    /// <summary>
    /// 行政村危险区人员转移
    /// </summary>
    public interface IVillageTransferPersonManager
    {
        /// <summary>
        /// 获取行政村上报和未上报统计信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        List<VillageNumViewModel> GetVillageReportNum(GetVillageReportNum request);
        List<VillageNumViewModel> GetVillageReportNum1(GetVillageReportNum1 request);
        #region 行政村危险区转移人员
        /// <summary>
        /// 获取危险区转移人员行政村 （已上报和未上报的村名）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageViewModel> GetVillageTransferPersonAdcd(GetVillageTransferPersonAdcd request);

        /// <summary>
        /// 获取行政村危险区转移人员列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageTransferPersonViewModel> GetVillageTransferPerson(GetVillageTransferPerson request);
        BsTableDataSource<VillageTransferPersonViewModel> GetVillageTransferPerson1(GetVillageTransferPerson1 request);
        BsTableDataSource<StatiscPerson> GetVillageTransferPerson2(GetVillageTransferPerson2 request);

        /// <summary>
        /// 删除行政村危险区转移人员信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DelVillageTransferPerson(string ids);

        /// <summary>
        /// 保存行政村危险区转移人员信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        BaseResult SaveVillageTransferPerson(SaveVillageTransferPerson request);
        /// <summary>
        /// 根据行政村ADCD删除危险区转移人员
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool DelVillageTransferPersonByADCD(DelVillageTransferPersonByADCD request);

        bool NoVillageTransferPerson(NoVillageTransferPerson request);

        #endregion
    }
}
