using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.CApp;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.NoVerify
{
    public interface INoVerifySuperviseManage
    {
        SuperviseModel NoVerifySPersonLiable(NoVerifySPersonLiable request);
        BsTableDataSource<ADCDDisasterViewModel> NoVerifyGetPersonLiabelList(NoVerifyGetPersonLiabelList request);
    }
}
