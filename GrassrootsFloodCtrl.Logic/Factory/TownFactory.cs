using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.Model.AppApi;
using GrassrootsFloodCtrl.ServiceModel.AppApi;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.Model.Town;
using System.Data;

namespace GrassrootsFloodCtrl.Logic.Factory
{
    public class TownFactory : ManagerBase, IGradelevelFactory
    {
        public AppLoginModel GetLoginInfo(RoutePostAppLoginInfo request, AppMobileLogin model, IDbConnection db)
        {
            //using (var db = DbFactory.Open())
            //{
                //查询TownPersonLiable该用户是否属于指挥
                //属于指挥就有发送的权限
                var townModel = db.Single<TownPersonLiable>(x=>x.Mobile==request.userName);
                if (townModel != null && townModel.Position == "指挥")
                {
                    return new AppLoginModel
                    {
                        ActionName = "镇级",
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
                        ActionName = "镇级",
                        StatusCode = 1,
                        IsSend = false,
                        Message = "返回登录信息成功",
                        Token = request.token,
                        Adcd = model.adcd,
                        ExistUser = true,
                        Postion = null
                    };
                }

            //}
        }
    }
}
