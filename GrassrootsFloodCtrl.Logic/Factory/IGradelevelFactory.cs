using GrassrootsFloodCtrl.Model.AppApi;
using GrassrootsFloodCtrl.ServiceModel.AppApi;
using GrassrootsFloodCtrl.ServiceModel.Route;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.Factory
{
    public interface IGradelevelFactory
    {
        AppLoginModel GetLoginInfo(RoutePostAppLoginInfo request, AppMobileLogin model,IDbConnection db);
    }
}
