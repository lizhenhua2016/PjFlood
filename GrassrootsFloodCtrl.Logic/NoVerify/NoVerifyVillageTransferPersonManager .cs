using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dy.Common;
using GrassrootsFloodCtrl.Logic.ZZTX;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.Model.Town;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack.OrmLite;
using System.Linq.Expressions;
using GrassrootsFloodCtrl.Logic.Common;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;
using GrassrootsFloodCtrl.Model.DangerZone;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;

namespace GrassrootsFloodCtrl.Logic.NoVerify
{
    public class NoVerifyVillageTransferPersonManager : ManagerBase, INoVerifyVillageTransferPersonManager
    {
        public BsTableDataSource<VillageTransferPersonViewModel> NoVerifyGetVillageTransferPerson(NoVerifyGetVillageTransferPerson request)
        {
            using (var db = DbFactory.Open())
            {
                //if (string.IsNullOrEmpty(adcd))
                //    throw new Exception("请重新登录");
                //if (request.year==null)
                //    throw new Exception("年度异常");

                var builder = db.From<ADCDInfo>();
                builder.LeftJoin<ADCDInfo, VillageTransferPerson>((x, y) => x.adcd == y.adcd);
                builder.LeftJoin<VillageTransferPerson, DangerZone>((x, y) => x.DangerZoneType == y.DangerZoneName);
                if (!string.IsNullOrEmpty(request.adcd))
                    builder.And(x => x.adcd == request.adcd);
                if (request.year != null)
                    builder.And<VillageTransferPerson>(y => y.Year == request.year);
                if (request.nums != null && request.nums > 1) builder.And<VillageTransferPerson>(y => y.AuditNums == request.nums);
                builder.Select("VillageTransferPerson.*,ADCDInfo.adnm,DangerZone.Id as DId");
                var count = db.Count(builder);

                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(x => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                    builder.OrderByDescending(x => request.Sort);
                else
                    builder.OrderByDescending(x => x.CreateTime);

                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : request.PageIndex * PageSize;
                builder.Limit(PageIndex, PageSize);
                var list = db.Select<VillageTransferPersonViewModel>(builder);

                return new BsTableDataSource<VillageTransferPersonViewModel>() { rows = list, total = count };
            }
        }

        public List<VillageNumViewModel> NoVerifyGetVillageReportNum(NoVerifyGetVillageReportNum request)
        {
          
            using (var db = DbFactory.Open())
            {
                var year = DateTime.Now.Year;
                if (request.Year != 0)
                    year = request.Year;
                var list = new List<VillageNumViewModel>();

                #region 从ADCDInfo表中获取行政村的总数量 

                var builder = db.From<ADCDInfo>();
                adcd = "331081101000000";//大溪水镇
                builder.Where(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != "331081101000000");
                var adcdList = db.Select(builder).Select(x => x.adcd);
                //if(adcdList.Count()==0)
                //    throw  new Exception("行政村信息不存在");
                #endregion

                var count = 0;
                #region 获取行政村信息上报统计

                if (adcdList.Count() != 0)
                {
                    var ADCDDisasterInfoBuilder = db.From<ADCDDisasterInfo>();
                    // ADCDDisasterInfoBuilder.Where(x => x.Year == year && Sql.In(x.adcd, adcdList));
                    ADCDDisasterInfoBuilder.Where(x => x.Year == year && x.adcd.StartsWith(adcd.Substring(0, 9)));
                    ADCDDisasterInfoBuilder.SelectDistinct(x => x.adcd);
                    count = db.Select(ADCDDisasterInfoBuilder).Count;
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村信息,
                        NoReported = adcdList.Count() - count,
                        HasReported = count
                    });
                }

                #endregion

                #region 获取行政村防汛防台工作组上报统计

                if (adcdList.Count() != 0)
                {
                    var VillageWorkingGroupBuilder = db.From<VillageWorkingGroup>();
                    //VillageWorkingGroupBuilder.Where(x => x.Year == year && Sql.In(x.VillageADCD, adcdList));
                    VillageWorkingGroupBuilder.Where(x => x.Year == year && x.VillageADCD.StartsWith(adcd.Substring(0, 9)));
                    VillageWorkingGroupBuilder.SelectDistinct(x => x.VillageADCD);
                    count = db.Select(VillageWorkingGroupBuilder).Count;
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村防汛防台工作组,
                        NoReported = adcdList.Count() - count,
                        HasReported = count
                    });
                }

                #endregion

                #region 获取行政村网格责任人上报统计

                if (adcdList.Count() != 0)
                {
                    var VillageGridPersonLiableBuilder = db.From<VillageGridPersonLiable>();
                    //Sql.In(x.VillageADCD, adcdList)
                    VillageGridPersonLiableBuilder.Where(x => x.Year == year && x.VillageADCD.StartsWith(adcd.Substring(0, 9)));
                    VillageGridPersonLiableBuilder.SelectDistinct(x => x.VillageADCD);
                    count = db.Select(VillageGridPersonLiableBuilder).Count;
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村网格责任人,
                        NoReported = adcdList.Count() - count,
                        HasReported = count
                    });
                }

                #endregion

                #region 获取行政村危险区转移人员上报统计

                if (adcdList.Count() != 0)
                {
                    var VillageTransferPersonBuilder = db.From<VillageTransferPerson>();
                    //Sql.In(x.adcd, adcdList)
                    VillageTransferPersonBuilder.Where(x => x.Year == year && x.adcd.StartsWith(adcd.Substring(0, 9)));
                    VillageTransferPersonBuilder.SelectDistinct(x => x.adcd);
                    count = db.Select(VillageTransferPersonBuilder).Count;
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村危险区人员转移清单,
                        NoReported = adcdList.Count() - count,
                        HasReported = count
                    });
                }

                #endregion

                #region 获取镇街防汛防台责任人数量上报统计

                var TownPersonBuilder = db.From<TownPersonLiable>();
                TownPersonBuilder.Where(x => x.Year == year && x.adcd == adcd);
                count = dyConverter.ToInt32(db.Count(TownPersonBuilder));
                list.Add(new VillageNumViewModel() { ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.镇级防汛防台责任人, NoReported = 0, HasReported = count });

                #endregion

                #region 获取行政村防汛形势图上报统计

                if (adcdList.Count() != 0)
                {
                    var VillagePicBuilder = db.From<VillagePic>();
                    //Sql.In(x.adcd, adcdList)
                    VillagePicBuilder.Where(x => x.Year == year && x.adcd.StartsWith(adcd.Substring(0, 9)));
                    VillagePicBuilder.SelectDistinct(x => x.adcd);
                    count = db.Select(VillagePicBuilder).Count;
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村防汛防台形势图,
                        NoReported = adcdList.Count() - count,
                        HasReported = count
                    });
                }

                #endregion
                #region 
                var iscount = db.Select<ADCDInfo>("select adcd from ADCDInfo where adcd like '%" + adcd.Substring(0, 9) + "%' and adcd != " + adcd + " and lng > 0 and lat > 0");
                var nocount = db.Select<ADCDInfo>("select adcd from ADCDInfo where adcd like '%" + adcd.Substring(0, 9) + "%' and adcd != " + adcd + " and ((lng is null and lat is null) or (lng = 0 and lat =0))");
                list.Add(new VillageNumViewModel()
                {
                    //行政村信息审批统计
                    ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村标绘,
                    NoReported = nocount.Count,
                    HasReported = iscount.Count
                });
                #endregion
                //

                return list;
            }
        }

        public List<VillageNumViewModel> GetVillageReportNum(NoVerifyGetVillageReportNum request)
        {

            using (var db = DbFactory.Open())
            {
                var year = DateTime.Now.Year;
                if (request.Year != 0)
                    year = request.Year;
                var list = new List<VillageNumViewModel>();

                #region 从ADCDInfo表中获取行政村的总数量 

                var builder = db.From<ADCDInfo>();
                builder.Where(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd);
                var adcdList = db.Select(builder).Select(x => x.adcd);
                //if(adcdList.Count()==0)
                //    throw  new Exception("行政村信息不存在");
                #endregion

                var count = 0;
                #region 获取行政村信息上报统计

                if (adcdList.Count() != 0)
                {
                    var ADCDDisasterInfoBuilder = db.From<ADCDDisasterInfo>();
                    // ADCDDisasterInfoBuilder.Where(x => x.Year == year && Sql.In(x.adcd, adcdList));
                    ADCDDisasterInfoBuilder.Where(x => x.Year == year && x.adcd.StartsWith(adcd.Substring(0, 9)));
                    ADCDDisasterInfoBuilder.SelectDistinct(x => x.adcd);
                    count = db.Select(ADCDDisasterInfoBuilder).Count;
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村信息,
                        NoReported = adcdList.Count() - count,
                        HasReported = count
                    });
                }

                #endregion

                #region 获取行政村防汛防台工作组上报统计

                if (adcdList.Count() != 0)
                {
                    var VillageWorkingGroupBuilder = db.From<VillageWorkingGroup>();
                    //VillageWorkingGroupBuilder.Where(x => x.Year == year && Sql.In(x.VillageADCD, adcdList));
                    VillageWorkingGroupBuilder.Where(x => x.Year == year && x.VillageADCD.StartsWith(adcd.Substring(0, 9)));
                    VillageWorkingGroupBuilder.SelectDistinct(x => x.VillageADCD);
                    count = db.Select(VillageWorkingGroupBuilder).Count;
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村防汛防台工作组,
                        NoReported = adcdList.Count() - count,
                        HasReported = count
                    });
                }

                #endregion

                #region 获取行政村网格责任人上报统计

                if (adcdList.Count() != 0)
                {
                    var VillageGridPersonLiableBuilder = db.From<VillageGridPersonLiable>();
                    //Sql.In(x.VillageADCD, adcdList)
                    VillageGridPersonLiableBuilder.Where(x => x.Year == year && x.VillageADCD.StartsWith(adcd.Substring(0, 9)));
                    VillageGridPersonLiableBuilder.SelectDistinct(x => x.VillageADCD);
                    count = db.Select(VillageGridPersonLiableBuilder).Count;
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村网格责任人,
                        NoReported = adcdList.Count() - count,
                        HasReported = count
                    });
                }

                #endregion

                #region 获取行政村危险区转移人员上报统计

                if (adcdList.Count() != 0)
                {
                    var VillageTransferPersonBuilder = db.From<VillageTransferPerson>();
                    //Sql.In(x.adcd, adcdList)
                    VillageTransferPersonBuilder.Where(x => x.Year == year && x.adcd.StartsWith(adcd.Substring(0, 9)));
                    VillageTransferPersonBuilder.SelectDistinct(x => x.adcd);
                    count = db.Select(VillageTransferPersonBuilder).Count;
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村危险区人员转移清单,
                        NoReported = adcdList.Count() - count,
                        HasReported = count
                    });
                }

                #endregion

                #region 获取镇街防汛防台责任人数量上报统计

                var TownPersonBuilder = db.From<TownPersonLiable>();
                TownPersonBuilder.Where(x => x.Year == year && x.adcd == adcd);
                count = dyConverter.ToInt32(db.Count(TownPersonBuilder));
                list.Add(new VillageNumViewModel() { ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.镇级防汛防台责任人, NoReported = 0, HasReported = count });

                #endregion

                #region 获取行政村防汛形势图上报统计

                if (adcdList.Count() != 0)
                {
                    var VillagePicBuilder = db.From<VillagePic>();
                    //Sql.In(x.adcd, adcdList)
                    VillagePicBuilder.Where(x => x.Year == year && x.adcd.StartsWith(adcd.Substring(0, 9)));
                    VillagePicBuilder.SelectDistinct(x => x.adcd);
                    count = db.Select(VillagePicBuilder).Count;
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村防汛防台形势图,
                        NoReported = adcdList.Count() - count,
                        HasReported = count
                    });
                }

                #endregion
                #region 
                var iscount = db.Select<ADCDInfo>("select adcd from ADCDInfo where adcd like '%" + adcd.Substring(0, 9) + "%' and adcd != " + adcd + " and lng > 0 and lat > 0");
                var nocount = db.Select<ADCDInfo>("select adcd from ADCDInfo where adcd like '%" + adcd.Substring(0, 9) + "%' and adcd != " + adcd + " and ((lng is null and lat is null) or (lng = 0 and lat =0))");
                list.Add(new VillageNumViewModel()
                {
                    //行政村信息审批统计
                    ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村标绘,
                    NoReported = nocount.Count,
                    HasReported = iscount.Count
                });
                #endregion
                //

                return list;
            }
        }

        public BsTableDataSource<VillageViewModel> GetVillageTransferPersonAdcd(NoVerifyGetVillageTransferPersonAdcd request)
        {
            using (var db = DbFactory.Open())
            {
                var year = DateTime.Now.Year;
                if (request.year != 0)
                    year = request.year;
                var builder = db.From<ADCDInfo>();
                builder.LeftJoin<ADCDInfo, VillageTransferPerson>((x, y) => x.adcd == y.adcd);
                builder.Where<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd);

                if (!string.IsNullOrEmpty(request.adnm))
                    builder.And(x => x.adnm.Contains(request.adnm));
                builder.SelectDistinct(x => new { x.Id, x.adcd, x.adnm });
                if (request.type == 0)//0：未上报，1：已上报
                    builder.And<VillageTransferPerson>(y => (y.DangerZoneName == string.Empty || y.DangerZoneName == null) && y.IfTransfer == null);
                else if (request.type == 1)
                    builder.And<VillageTransferPerson>(y => (y.DangerZoneName != "" && y.Year == year) || (y.IfTransfer == 1 && y.Year == year));
                else if (request.type == 2)
                {
                }
                else
                    throw new Exception("上报状态不正确");

                var count = db.Select(builder).Count;
                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(x => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) &&
                         request.Order == "desc")
                    builder.OrderByDescending(x => request.Sort);
                else
                    builder.OrderBy(x => x.adcd);

                var rows = request.PageSize == 0 ? 10 : request.PageSize;
                var skip = request.PageIndex == 0 ? 0 : (request.PageIndex - 1) * rows;

                builder.Limit(skip, rows);
                var list = db.Select<VillageViewModel>(builder);
                return new BsTableDataSource<VillageViewModel>() { total = count, rows = list };

            }
        }
    }
}
