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

namespace GrassrootsFloodCtrl.Logic.CApp
{
    public interface ICAppManager
    {
        #region
        List<TownInfoAppViewModel> GetTownInfo(GetTownInfo request);
        List<TownPersonAppViewModel> GetTownPerson(GetTownPerson request);
        List<TownGridPersonAppViewModel> GetTownGridMan(GetTownGridMan request);
        VillageInfoAppViewModel GetCunDot(GetCunDot request);
        List<AppKeyViewModel> GetKeys(GetKeys request);
        List<AppKeyInfoViewModel> GetKeysInfo(GetKeysInfo request);
        //获取县一级的概况
        List<CountyInfoAppViewModel> GetCountyInfo(GetCountyInfo request);
        #region app统计
        List<AppStaticsViewModel> GetAppStatics(GetAppStatics request);
        List<AppAreaViewModel> GetAppArea(GetAppArea request);
        List<StatiscPerson>  CCKHVillageApp(CCKHVillageApp request);
        List<AppRecordAndUserViewModel> GetPersonAppInfo(GetPersonAppInfo request);
        #endregion
        #endregion
    }
}
