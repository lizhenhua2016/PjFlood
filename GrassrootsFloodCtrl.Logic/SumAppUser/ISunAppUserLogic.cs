using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.SumAppUser;
using GrassrootsFloodCtrl.ServiceModel.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.SumAppUser
{
    public interface ISunAppUserLogic
    {
        List<SelectSumAppUserList> GetSelectSumAppUserList(RouteSumAppUser request);
        BsTableDataSource<Model.SumAppUser.SumAppUser> GetSumAppUserList(RouteNoAppUser request);
        List<SelectSumAppUserList> GetSelectSumAppUserList2(RouteSumAppUser2 request);
    }
}
