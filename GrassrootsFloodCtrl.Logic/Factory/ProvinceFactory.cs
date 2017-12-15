using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model.AppApi;
using GrassrootsFloodCtrl.ServiceModel.AppApi;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack.OrmLite;
using System.Data;
using GrassrootsFloodCtrl.Model.Sys;

namespace GrassrootsFloodCtrl.Logic.Factory
{
    public class ProvinceFactory : ManagerBase, IGradelevelFactory
    {
        public AppLoginModel GetLoginInfo(RoutePostAppLoginInfo request, AppMobileLogin model,IDbConnection db)
        {
            var userInfoModel = db.Single<UserInfo>(x => x.UserName == request.userName);
            if (userInfoModel != null)
            {
                return new AppLoginModel
                {
                    ActionName = "省级",
                    StatusCode = 1,
                    IsSend = true,
                    Message = "没有登录应急管理权限",
                    Token = request.token,
                    Adcd = model.adcd,
                    ExistUser = true,
                    Postion = null
                };
            }
            return new AppLoginModel
            {
                ActionName = "省级",
                StatusCode = 1,
                IsSend = true,
                Message = "没有登录应急管理权限",
                Token = request.token,
                Adcd = model.adcd,
                ExistUser = false,
                Postion = null
            };


        }
    }
}
