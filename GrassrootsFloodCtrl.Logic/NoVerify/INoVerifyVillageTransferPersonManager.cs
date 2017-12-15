using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dy.Common;
using GrassrootsFloodCtrl.Logic.ZZTX;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.Model.Town;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack.OrmLite;
using System.Linq.Expressions;
using GrassrootsFloodCtrl.Logic.Common;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;
using GrassrootsFloodCtrl.Model.DangerZone;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;

namespace GrassrootsFloodCtrl.Logic.NoVerify
{
    public interface INoVerifyVillageTransferPersonManager 
    {
        /// <summary>
        /// 获取行政村危险区转移人员列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageTransferPersonViewModel> NoVerifyGetVillageTransferPerson(NoVerifyGetVillageTransferPerson request);
        List<VillageNumViewModel> NoVerifyGetVillageReportNum(NoVerifyGetVillageReportNum request);

        List<VillageNumViewModel> GetVillageReportNum(NoVerifyGetVillageReportNum request);

        /// <summary>
        /// 获取危险区转移人员行政村 （已上报和未上报的村名）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageViewModel> GetVillageTransferPersonAdcd(NoVerifyGetVillageTransferPersonAdcd request);
    }
}
