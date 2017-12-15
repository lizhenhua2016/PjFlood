using GrassrootsFloodCtrl.Logic.DataShare;
using GrassrootsFloodCtrl.ServiceModel.Country;
using GrassrootsFloodCtrl.ServiceModel.DataShare;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Sys;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.ServiceModel.Village;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    public class DataShareService : ServiceBase
    {
        public IDataShareManager DataShareManager { get; set; }
        /// 县级责任人
        public List<DSCountryPersonServiceModel> POST(CountyPersLiableList request)
        {
            return DataShareManager.CountyPersLiableList(request);
        }
        //镇级责任人
        public List<DSTownPersonLiableViewModel> POST(TownPersLiableList request)
        {
            return DataShareManager.TownPersLiableList(request);
        }
        //村级工作组
        public List<DSVillageWorkingGroupViewModel> POST(VillageGroupPersLiableList request)
        {
            return DataShareManager.VillageGroupPersLiableList(request);
        }
        //村级网格责任人
        public List<DSVillageGridViewModel> POST(VillageGridPersLiableList request)
        {
            return DataShareManager.VillageGridPersLiableList(request);
        }

        //村级人员转移清单
        public List<DSVillageTransferPersonViewModel> POST(VillageTransferPersLiableList request)
        {
            return DataShareManager.VillageTransferPersLiableList(request);
        }

        //行政区划经纬度
        public List<DSADCDDisasterViewModel> POST(ADCDList request)
        {
            return DataShareManager.ADCDList(request);
        }

        //登陆
        public List<DataShareReturnModel> POST(DSLogin request)
        {
            return DataShareManager.DSLogin(request);
        }
    }
}
