using GrassrootsFloodCtrl.Logic.Position;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Position;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    [Authenticate]
    public class PositionService:ServiceBase
    {
        public IPositionManager PositionManager { get; set; }

        public BsTableDataSource<PositionViewModel> Get(GetPositionList request)
        {
            return PositionManager.GetPositionList(request);
        }
    }
}
