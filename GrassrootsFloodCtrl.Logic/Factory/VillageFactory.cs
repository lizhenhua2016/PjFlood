using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model.AppApi;
using GrassrootsFloodCtrl.ServiceModel.AppApi;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.ServiceModel.Common;
using System.Configuration;
using System.Data;

namespace GrassrootsFloodCtrl.Logic.Factory
{
    public class VillageFactory : ManagerBase, IGradelevelFactory
    {
        public AppLoginModel GetLoginInfo(RoutePostAppLoginInfo request, AppMobileLogin model, IDbConnection db)
        {
            //using (var db = DbFactory.Open())
            //{
                //村级的时候需要将岗位信息返回出去
                //村长有转发的权利可以显示转发按钮
                var villageModel = db.Single<VillageWorkingGroup>(x=>x.HandPhone==request.userName);
                var infoList = db.SqlList<VillagePerson>(
                    "EXEC AppVillageUserPostInfo @handphone",
                    new { handphone = request.userName});
                // 需要转移的人员总数
                var totalnum = db.Select<VillageTransferPerson>(w => w.adcd == model.adcd)
                    .Sum(w => w.HouseholderNum);
                List<postInfo> postList = new List<postInfo>();
                postInfo postModel = null;
                //村级的职位信息
                var vgroup = ConfigurationManager.AppSettings["村级工作组"].Split(',');

                var village = ConfigurationManager.AppSettings["村级网格"].Split(',');
                infoList.ForEach(
                    w =>
                    {

                        postModel = new postInfo { postCode = w.Post };
                        if (vgroup.Contains(w.Post)) postModel.postTypecode = "村级工作组";
                        if (village.Contains(w.Post)) postModel.postTypecode = "村级网格";
                        if (postList.Count(x => x.postCode == postModel.postCode && x.postTypecode == postModel.postTypecode) == 0)
                        {
                            postModel.transferNum = postModel.postCode == "人员转移组" ? totalnum : 0;

                            postList.Add(postModel);
                        }
                    });
                if (villageModel != null && villageModel.Post == "村级主要负责人")
                {
                    return new AppLoginModel
                    {
                        ActionName = "村级",
                        StatusCode = 1,
                        IsSend = true,
                        Message = "返回登录信息成功",
                        Token = request.token,
                        Adcd = model.adcd,
                        ExistUser = true,
                        Postion = postList

                    };
                }
                else
                {
                    return new AppLoginModel
                    {
                        ActionName = "村级",
                        StatusCode = 1,
                        IsSend = false,
                        Message = "返回登录信息成功",
                        Token = request.token,
                        Adcd = model.adcd,
                        ExistUser = true,
                        Postion = postList

                    };
                }
           // }
        }
    }
}
