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

namespace GrassrootsFloodCtrl.Logic.DataShare
{
    public interface IDataShareManager
    {
        /// 县级责任人
        List<DSCountryPersonServiceModel> CountyPersLiableList(CountyPersLiableList request);
        //镇级责任人
        List<DSTownPersonLiableViewModel> TownPersLiableList(TownPersLiableList request);
        //村级工作组
        List<DSVillageWorkingGroupViewModel> VillageGroupPersLiableList(VillageGroupPersLiableList request);
        //村级网格责任人
        List<DSVillageGridViewModel> VillageGridPersLiableList(VillageGridPersLiableList request);
        //村级人员转移清单
        List<DSVillageTransferPersonViewModel> VillageTransferPersLiableList(VillageTransferPersLiableList request);
        //行政区划经纬度
        List<DSADCDDisasterViewModel> ADCDList(ADCDList request);
        //登陆
        List<DataShareReturnModel> DSLogin(DSLogin request);
    }
}
