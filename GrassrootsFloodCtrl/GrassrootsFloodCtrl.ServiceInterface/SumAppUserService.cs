using GrassrootsFloodCtrl.Logic.SumAppUser;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.SumAppUser;
using GrassrootsFloodCtrl.ServiceModel.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceInterface
{

    public class SumAppUserService: ServiceBase
    {
        public ISunAppUserLogic logic { get; set; }
        public List<SelectSumAppUserList> Get(RouteSumAppUser request)
        {
            return logic.GetSelectSumAppUserList(request);
        }
        public List<SelectSumAppUserList> Get(RouteSumAppUser2 request)
        {
            return logic.GetSelectSumAppUserList2(request);
        }
        public BsTableDataSource<Model.SumAppUser.SumAppUser> Get(RouteNoAppUser request)
        {
            return logic.GetSumAppUserList(request);
        }
    }
}
