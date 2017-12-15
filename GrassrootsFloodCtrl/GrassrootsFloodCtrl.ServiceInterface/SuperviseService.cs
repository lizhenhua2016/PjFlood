using GrassrootsFloodCtrl.Logic.Supervise;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.ServiceModel.CApp;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    [Authenticate]
    public class SuperviseService:ServiceBase
    {
        public ISuperviseManager SuperviseManager { get; set;}
        public SuperviseModel Post(SPersonLiable request)
        {
            return SuperviseManager.PersonLiable(request);
        }
        public SuperviseModel Post(SPersonLiable1 request)
        {
            return SuperviseManager.PersonLiable1(request);
        }
        public BsTableDataSource<ADCDDisasterViewModel> Post(GetPersonLiabelList request)
        {
            return SuperviseManager.GetPersonLiabelList(request);
        }
        public BsTableDataSource<StatiscPerson> Get(CCKHVillage request)
        {
            return SuperviseManager.CCKHVillage(request);
        }
        public BaseResult Post(SetCCKH request)
        {
            return SuperviseManager.SetCCKH(request);
        }

        public BsTableDataSource<SpotCheckModel> Post(GetCCJLTable request)
        {
            return SuperviseManager.GetCCJLTable(request);
        }

        public BsTableDataSource<SpotCheckOne> Post(GetCCJLTableOne request)
        {
            return SuperviseManager.GetCCJLTableOne(request);
        }
        public List<AppAreaViewModel> POST(GetCityAppInPostList request)
        {
            return SuperviseManager.GetCityAppInPostList(request);
        }
    }
}
