using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Grid;
using GrassrootsFloodCtrl.ServiceModel.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.Grid
{
    public interface IGridManager
    {
        BsTableDataSource<GridViewModel> GetGridList(GetGridList request);
    }
}
