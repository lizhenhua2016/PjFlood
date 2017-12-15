using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.ServiceModel.AppApi;
using GrassrootsFloodCtrl.ServiceModel.Route;

using GrassrootsFloodCtrl.Model.AppApi;
using GrassrootsFloodCtrl.Model.Sys;
using ServiceStack.OrmLite;
using System.Data;

namespace GrassrootsFloodCtrl.Logic.Factory
{
    public class CountryFactory : ManagerBase, IGradelevelFactory
    {
        public AppLoginModel GetLoginInfo(RoutePostAppLoginInfo request, AppMobileLogin model,IDbConnection db)
        {
            //using (var db = DbFactory.Open())
            //{
                //判断是否是在userInfo中存在,存在就做为发出者
                //不存在不作为发出者
                var userInfoModel = db.Single<UserInfo>(x => x.UserName == request.userName);
                if (userInfoModel != null)
                {
                    return new AppLoginModel
                    {
                        ActionName = "县级",
                        StatusCode = 1,
                        IsSend = true,
                        Message = "返回登录信息成功",
                        Token = request.token,
                        Adcd = model.adcd,
                        ExistUser = true,
                        Postion = null
                    };
                }
                else
                {
                    return new AppLoginModel
                    {
                        ActionName="县级",
                        StatusCode=1,
                        IsSend=false,
                        Message = "返回登录信息成功",
                        Token = request.token,
                        Adcd = model.adcd,
                        ExistUser = true,
                        Postion = null
                    };
                }
            }
        //}
    }
}
