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
using GrassrootsFloodCtrl.Logic.NoVerify;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;

namespace GrassrootsFloodCtrl.ServiceInterface
{
   
    public class NoVerifyCAppService : ServiceBase
    {
        public INoVerifyCAppManager CAppManager { get; set; }
        public List<TownInfoAppViewModel> Post(NoVerifyGetTownInfo request)
        {
            return CAppManager.GetTownInfo(request);
        }
        public List<TownPersonAppViewModel> Post(NoVerifyGetTownPerson request)
        {
            return CAppManager.GetTownPerson(request);
        }
        public List<TownGridPersonAppViewModel> Post(NoVerifyGetTownGridMan request)
        {
            return CAppManager.GetTownGridMan(request);
        }
        public VillageInfoAppViewModel Post(NoVerifyGetCunDot request)
        {
            return CAppManager.GetCunDot(request);
        }
        public List<AppKeyViewModel> Post(NoVerifyGetKeys request)
        {
            return CAppManager.GetKeys(request);
        }
        public List<AppKeyInfoViewModel> Post(NoVerifyGetKeysInfo request)
        {
            return CAppManager.GetKeysInfo(request);
        }

        public List<AppStaticsViewModel> Post(NoVerifyGetAppStatics request)
        {
            return CAppManager.GetAppStatics(request);
        }
        public List<AppAreaViewModel> Post(NoVerifyGetAppArea request)
        {
            return CAppManager.GetAppArea(request);
        }
        public List<StatiscPerson> Get(NoVerifyCCKHVillageApp request)
        {
            return CAppManager.CCKHVillageApp(request);
        }
        public List<AppRecordAndUserViewModel> Get(NoVerifyGetPersonAppInfo request)
        {
            return CAppManager.GetPersonAppInfo(request);
        }
        /// <summary>
        /// 这里是获取县级概况的
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<CountyInfoAppViewModel> Post(NoVerifyGetCountyInfo request)
        {
            return CAppManager.GetCountyInfo(request);
        }
    }
}
