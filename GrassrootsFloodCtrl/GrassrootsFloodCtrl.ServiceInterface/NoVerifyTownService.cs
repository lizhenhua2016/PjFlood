using GrassrootsFloodCtrl.Logic.NoVerify;
using GrassrootsFloodCtrl.ServiceModel.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Logic.Town;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Town;
using ServiceStack;
using GrassrootsFloodCtrl.ServiceModel.Village;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    public class NoVerifyTownService:ServiceBase
    {
        public INoVerifyTownManage NoVerifyTownManage { get; set; }
        public BsTableDataSource<TownPersonLiableViewModel> Get(NoVerifyGetTownList request)
        {
            return NoVerifyTownManage.GetTownList(request);
        }
    }
}
