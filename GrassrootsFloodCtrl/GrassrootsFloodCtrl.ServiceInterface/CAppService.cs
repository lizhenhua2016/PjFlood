using GrassrootsFloodCtrl.Logic.CApp;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.ServiceModel.AppApi;
using GrassrootsFloodCtrl.ServiceModel.CApp;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.ServiceModel.Country;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    [Authenticate]
    public class CAppService : ServiceBase
    {
        public ICAppManager CAppManager { get; set; }
        public List<TownInfoAppViewModel> Post(GetTownInfo request)
        {
            return CAppManager.GetTownInfo(request);
        }
        public List<TownPersonAppViewModel> Post(GetTownPerson request)
        {
            return CAppManager.GetTownPerson(request);
        }
        public List<TownGridPersonAppViewModel> Post(GetTownGridMan request)
        {
            return CAppManager.GetTownGridMan(request);
        }
        public VillageInfoAppViewModel Post(GetCunDot request)
        {
            return CAppManager.GetCunDot(request);
        }
        public List<AppKeyViewModel> Post(GetKeys request)
        {
            return CAppManager.GetKeys(request);
        }
        public List<AppKeyInfoViewModel> Post(GetKeysInfo request)
        {
            return CAppManager.GetKeysInfo(request);
        }

        public List<AppStaticsViewModel> Post(GetAppStatics request)
        {
            return CAppManager.GetAppStatics(request);
        }
        public List<AppAreaViewModel> Post(GetAppArea request)
        {
            return CAppManager.GetAppArea(request);
        }
        public List<StatiscPerson> Get(CCKHVillageApp request)
        {
            return CAppManager.CCKHVillageApp(request);
        }
        public List<AppRecordAndUserViewModel> Get(GetPersonAppInfo request)
        {
            return CAppManager.GetPersonAppInfo(request);
        }
        /// <summary>
        /// 这里是获取县级概况的
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<CountyInfoAppViewModel> Post(GetCountyInfo request)
        {
            return CAppManager.GetCountyInfo(request);
        }
    }
}
