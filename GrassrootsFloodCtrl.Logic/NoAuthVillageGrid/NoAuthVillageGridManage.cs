using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;
using GrassrootsFloodCtrl.ServiceModel.Village;
using GrassrootsFloodCtrl.Logic.Common;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.Model.Enums;

namespace GrassrootsFloodCtrl.Logic.NoAuthVillageGrid
{
    public class NoAuthVillageGridManage : ManagerBase, INoAuthVillageGridManage
    {
        public ILogHelper _ILogHelper { get; set; }

        public BsTableDataSource<VillageGridViewModel> GetGridPersonLiableList(NoAuthGetGridPersonLiableList request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(adcd))
                { throw new Exception("请重新登录"); }
                //取镇下面的所有行政村
                var builder = db.From<VillageGridPersonLiable>();
                builder.LeftJoin<VillageGridPersonLiable, ADCDInfo>((x, y) => x.VillageADCD == y.adcd);
                if (adcd.Length == 15 && adcd.IndexOf("000000") > 0)//登陆的是乡镇用户
                    builder.Where(x => x.VillageADCD.StartsWith(adcd.Substring(0, 9)) && x.VillageADCD != adcd.ToString());
                else if (adcd.Length == 6 && adcd.IndexOf("00") < 0)//登陆的是县级用户
                    builder.Where(x => x.VillageADCD.StartsWith(adcd.Substring(0, 6)));
                else if (adcd == GrassrootsFloodCtrlEnums.AreaCode.省级编码.ToString())//管理员
                {
                }
                else
                {
                    throw new Exception("登陆用户的所属行政区划编码不正确");
                }
                if (!string.IsNullOrEmpty(request.key))
                {
                    builder.Where<ADCDInfo>(w => w.adnm.Contains(request.key));
                }
                var _year = null != request.year && !string.IsNullOrEmpty(request.year.ToString()) ? request.year : System.DateTime.Now.Year;
                builder.Where<VillageGridPersonLiable>(y => y.VillageADCD != null && y.Year == _year);
                builder.Select<VillageGridPersonLiable, ADCDInfo>((x, y) => new
                {
                    id = x.ID,
                    VillageADCD = x.VillageADCD,
                    adnm = y.adnm,
                    PersonLiable = x.PersonLiable,
                    GridName = x.GridName,
                    VillageGridName = x.VillageGridName,
                    Position = x.Position,
                    HandPhone = x.HandPhone,
                    Remarks = x.Remarks,
                    AddTime = x.AddTime,
                    EditTime = x.EditTime,
                    Year = x.Year
                });
                var count = db.Select(builder).Count;

                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(o => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                    builder.OrderByDescending(o => request.Sort);
                else
                    builder.OrderBy(o => o.ID);

                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                builder.Limit(PageIndex, PageSize);
                var RList = db.Select<VillageGridViewModel>(builder);

                return new BsTableDataSource<VillageGridViewModel>() { rows = RList, total = count };

            }
        }

        public BsTableDataSource<VillageGridViewModel> GetVillageGrid(NoAuthGetVillageGrid request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(adcd))
                { throw new Exception("请重新登录"); }
                var builder = db.From<VillageGridPersonLiable>();
                builder.LeftJoin<VillageGridPersonLiable, ADCDInfo>((x, y) => x.VillageADCD == y.adcd);
                var _year = null != request.year && !string.IsNullOrEmpty(request.year.ToString()) ? request.year : System.DateTime.Now.Year;
                builder.Where<VillageGridPersonLiable>(x => x.Year == _year && x.VillageADCD == request.adcd);
                if (request.nums != null && request.nums > 1) builder.Where<VillageGridPersonLiable>(w => w.AuditNums == request.nums);
                builder.Select(" VillageGridPersonLiable.*,ADCDInfo.adnm");
                var count = db.Count(builder);

                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(o => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                    builder.OrderByDescending(o => request.Sort);
                else
                    builder.OrderByDescending(o => o.ID);

                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                builder.Limit(PageIndex, PageSize);
                var RList = db.Select<VillageGridViewModel>(builder);

                return new BsTableDataSource<VillageGridViewModel>() { rows = RList, total = count };
            }
        }

        public BsTableDataSource<VillageViewModel> GetVillageGridList(NoAuthGetVillageGridList request)
        {
            try
            {
                using (var db = DbFactory.Open())
                {
                    if (string.IsNullOrEmpty(adcd))
                    { throw new Exception("请重新登录"); }
                    //取镇下面的所有行政村
                    var builder = db.From<ADCDInfo>();
                    builder.LeftJoin<ADCDInfo, VillageGridPersonLiable>((x, y) => x.adcd == y.VillageADCD);
                    if (adcd.Length == 15 && adcd.IndexOf("000000") > 0)//登陆的是乡镇用户
                        builder.Where<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd.ToString());
                    else if (adcd.Length == 6 && adcd.IndexOf("00") < 0)//登陆的是县级用户
                        builder.Where<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 6)));
                    else if (adcd == GrassrootsFloodCtrlEnums.AreaCode.省级编码.ToString())//管理员
                    {
                    }
                    else
                    {
                        throw new Exception("登陆用户的所属行政区划编码不正确");
                    }
                    if (!string.IsNullOrEmpty(request.key))
                    {
                        builder.Where(w => w.adnm.Contains(request.key));
                    }
                    builder.SelectDistinct(w => new { w.adcd, w.adnm });

                    //类型0/1 未上传/已上传 
                    var _year = null != request.year && !string.IsNullOrEmpty(request.year.ToString()) ? request.year : System.DateTime.Now.Year;
                    if (null != request.status && request.status.Value == 0)
                    {
                        builder.Where<VillageGridPersonLiable>(y => y.VillageADCD == null);
                    }
                    else if (null != request.status && request.status.Value == 1)
                    {
                        builder.Where<VillageGridPersonLiable>(y => y.VillageADCD != null && y.Year == _year);
                    }
                    else
                    {
                        throw new Exception("抱歉,参数异常！");
                    }

                    var count = db.Select(builder).Count;

                    if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                        builder.OrderBy(o => request.Sort);
                    else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                        builder.OrderByDescending(o => request.Sort);
                    else
                        builder.OrderBy(o => o.adcd);
                    if (null != request.status && request.status.Value == 0)
                    {
                        var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                        var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex - 1) * PageSize;
                        builder.Limit(PageIndex, PageSize);
                    }
                    var RList = db.Select<VillageViewModel>(builder);

                    return new BsTableDataSource<VillageViewModel>() { rows = RList, total = count };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("系统异常,请刷新：" + ex.Message);
            }
        }
    }
}
