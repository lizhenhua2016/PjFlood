using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Position;
using GrassrootsFloodCtrl.ServiceModel.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.Position
{
    public interface IPositionManager
    {
        BsTableDataSource<PositionViewModel> GetPositionList(GetPositionList request);
    }
}
