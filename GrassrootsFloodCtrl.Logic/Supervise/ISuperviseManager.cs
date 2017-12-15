using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.CApp;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.Supervise
{
    public interface ISuperviseManager
    {
        SuperviseModel PersonLiable(SPersonLiable request);
        BsTableDataSource<ADCDDisasterViewModel> GetPersonLiabelList(GetPersonLiabelList request);
        SuperviseModel PersonLiable1(SPersonLiable1 request);
        BsTableDataSource<StatiscPerson> CCKHVillage(CCKHVillage request);
        BaseResult SetCCKH(SetCCKH request);
        BsTableDataSource<SpotCheckModel> GetCCJLTable(GetCCJLTable request);
        BsTableDataSource<SpotCheckOne> GetCCJLTableOne(GetCCJLTableOne request);

        List<AppAreaViewModel> GetCityAppInPostList(GetCityAppInPostList request);
    }
}
