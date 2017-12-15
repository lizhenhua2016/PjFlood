using GrassrootsFloodCtrl.Logic.Grid;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Grid;
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
    public class GridService : ServiceBase
    {
        public IGridManager GridManager { get; set; }

        public BsTableDataSource<GridViewModel> Get(GetGridList request)
        {
            return GridManager.GetGridList(request);
        }
    }
}
