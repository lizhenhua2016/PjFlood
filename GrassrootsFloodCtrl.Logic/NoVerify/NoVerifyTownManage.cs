using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dy.Common;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.Model.Town;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack.OrmLite;
using System.Collections;
using GrassrootsFloodCtrl.Logic.Common;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.Logic.ZZTX;
using GrassrootsFloodCtrl.Logic.Village.WorkingGroup;
using GrassrootsFloodCtrl.Logic.Supervise;
using GrassrootsFloodCtrl.Logic.Country;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;
using System;

namespace GrassrootsFloodCtrl.Logic.NoVerify
{
    public class NoVerifyTownManage : ManagerBase, INoVerifyTownManage
    {
        public BsTableDataSource<TownPersonLiableViewModel> GetTownList(NoVerifyGetTownList request)
        {
            using (var db = DbFactory.Open())
            {
                //if (string.IsNullOrEmpty(adcd))
                //    throw new Exception("请重新登录");
                if (request.year == null)
                    throw new Exception("年度异常");
                if (string.IsNullOrEmpty(request.adcd))
                {
                    var builder = db.From<TownPersonLiable>();
                    builder.LeftJoin<TownPersonLiable, ADCDInfo>((x, y) => x.adcd == y.adcd);

                    if (adcd.Length == 15 && adcd.IndexOf("000000") > 0)//登陆的是乡镇用户
                        builder.And(x => x.adcd.StartsWith(adcd.Substring(0, 9)));
                    else if (adcd.Length == 6 && adcd.IndexOf("00") < 0)//登陆的是县级用户
                        builder.And(x => x.adcd.StartsWith(adcd.Substring(0, 6)));
                    else if (adcd == GrassrootsFloodCtrlEnums.AreaCode.省级编码.ToString())//管理员
                    {

                    }
                    else
                        throw new Exception("登陆用户的所属行政区划编码不正确");
                    if (!string.IsNullOrEmpty(request.name))
                        builder.And(x => x.Name.Contains(request.name));
                    if (!string.IsNullOrEmpty(request.Position))
                        builder.And(x => x.Position.Contains(request.Position));
                    if (!string.IsNullOrEmpty(request.Post))
                        builder.And(x => x.Post.Contains(request.Post));
                    builder.And(x => x.Year == request.year);
                    if (request.Id != 0)
                        builder.And(x => x.Id == request.Id);
                    builder.Select<TownPersonLiable, ADCDInfo>(
                        (x, y) =>
                            new
                            {
                                adcd = x.adcd,
                                adnm = y.adnm,
                                id = x.Id,
                                name = x.Name,
                                Position = x.Position,
                                Post = x.Post,
                                Mobile = x.Mobile,
                                Remark = x.Remark,
                                CreateTime = x.CreateTime,
                                Year = x.Year,
                                operateLog = x.operateLog
                            });
                    var count = db.Count(builder);

                    if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                        builder.OrderBy(x => request.Sort);
                    else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                        builder.OrderByDescending(x => request.Sort);
                    else
                        builder.OrderBy(x => x.Id);

                    var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                    var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                    builder.Limit(PageIndex, PageSize);
                    var list = db.Select<TownPersonLiableViewModel>(builder);

                    return new BsTableDataSource<TownPersonLiableViewModel>() { rows = list, total = count };
                }
                else
                {
                    var builder = db.From<TownPersonLiable>();
                    builder.And(x => x.adcd == request.adcd);
                    builder.And(x => x.Year == request.year);
                    if (request.nums != null && request.nums.Value > 1) builder.And(x => x.AuditNums == request.nums);
                    var count = db.Count(builder);

                    if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                        builder.OrderBy(x => request.Sort);
                    else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                        builder.OrderByDescending(x => request.Sort);
                    else
                        builder.OrderBy(x => x.Id);

                    var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                    var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                    builder.Limit(PageIndex, PageSize);
                    var list = db.Select<TownPersonLiableViewModel>(builder);

                    return new BsTableDataSource<TownPersonLiableViewModel>() { rows = list, total = count };
                }
            }
        }

    }
}
