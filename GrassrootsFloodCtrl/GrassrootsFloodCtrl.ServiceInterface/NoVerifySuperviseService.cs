using GrassrootsFloodCtrl.Logic.NoVerify;
using GrassrootsFloodCtrl.Logic.Supervise;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.ServiceModel.CApp;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    public class NoVerifySuperviseService:ServiceBase
    {
        public INoVerifySuperviseManage NoVerifySuperviseServiceManage { get; set; }

        public SuperviseModel Post(NoVerifySPersonLiable request)
        {
            return NoVerifySuperviseServiceManage.NoVerifySPersonLiable(request);
        }

        public BsTableDataSource<ADCDDisasterViewModel> Post(NoVerifyGetPersonLiabelList request)
        {
            return NoVerifySuperviseServiceManage.NoVerifyGetPersonLiabelList(request);
        }
    }
}
