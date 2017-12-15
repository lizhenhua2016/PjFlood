using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.ServiceModel.AppApi;
using GrassrootsFloodCtrl.ServiceModel.CApp;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.ServiceModel.Village;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.ServiceModel.Country;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;

namespace GrassrootsFloodCtrl.Logic.NoVerify
{
    public interface INoVerifyCAppManager
    {
        #region
        List<TownInfoAppViewModel> GetTownInfo(NoVerifyGetTownInfo request);
        List<TownPersonAppViewModel> GetTownPerson(NoVerifyGetTownPerson request);
        List<TownGridPersonAppViewModel> GetTownGridMan(NoVerifyGetTownGridMan request);
        VillageInfoAppViewModel GetCunDot(NoVerifyGetCunDot request);
        List<AppKeyViewModel> GetKeys(NoVerifyGetKeys request);
        List<AppKeyInfoViewModel> GetKeysInfo(NoVerifyGetKeysInfo request);
        //获取县一级的概况
        List<CountyInfoAppViewModel> GetCountyInfo(NoVerifyGetCountyInfo request);
        #region app统计
        List<AppStaticsViewModel> GetAppStatics(NoVerifyGetAppStatics request);
        List<AppAreaViewModel> GetAppArea(NoVerifyGetAppArea request);
        List<StatiscPerson> CCKHVillageApp(NoVerifyCCKHVillageApp request);
        List<AppRecordAndUserViewModel> GetPersonAppInfo(NoVerifyGetPersonAppInfo request);
        #endregion
        #endregion
    }
}
