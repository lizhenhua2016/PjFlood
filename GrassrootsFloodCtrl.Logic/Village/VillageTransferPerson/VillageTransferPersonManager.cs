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

namespace GrassrootsFloodCtrl.Logic.Village
{ 
    /// <summary>
  /// 行政村危险区人员转移
  /// </summary>
    public class VillageTransferPersonManager : ManagerBase, IVillageTransferPersonManager
    {
        #region 行政村危险区转移人员清单

        public IZZTXManager ZZTXManager { get; set; }
        public ILogHelper _ILogHelper { get; set; }
        /// <summary>
        /// 获取行政村上报和未上报统计信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<VillageNumViewModel> GetVillageReportNum(GetVillageReportNum request)
        {
           
            using (var db=DbFactory.Open())
            {
                var year = DateTime.Now.Year;
                if (request.Year != 0)
                    year = request.Year;
                var list = new List<VillageNumViewModel>();

                #region 从ADCDInfo表中获取行政村的总数量 

                var builder = db.From<ADCDInfo>();
                builder.Where(x=> x.adcd.StartsWith(adcd.Substring(0, 9))&&x.adcd!=adcd);
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
                TownPersonBuilder.Where(x => x.Year == year && x.adcd==adcd);
                count =dyConverter.ToInt32(db.Count(TownPersonBuilder));
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
                var iscount = db.Select<ADCDInfo>("select adcd from ADCDInfo where adcd like '%"+ adcd.Substring(0, 9) + "%' and adcd != "+adcd+" and lng > 0 and lat > 0");
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
        public List<VillageNumViewModel> GetVillageReportNum1(GetVillageReportNum1 request)
        {
            using (var db = DbFactory.Open())
            {
                var year = DateTime.Now.Year;
                if (string.IsNullOrEmpty(adcd)) throw new Exception("镇级编码不能为空！");
                var _adcd = request.adcd;
                if (request.Year != 0)
                    year = request.Year;
                var list = new List<VillageNumViewModel>();

                #region 从ADCDInfo表中获取行政村的总数量 

                var builder = db.From<ADCDInfo>();
                builder.Where(x => x.adcd.StartsWith(_adcd.Substring(0, 9)) && x.adcd != _adcd);
                var adcdList = db.Select(builder).Select(x => x.adcd);
                //if(adcdList.Count()==0)
                //    throw  new Exception("行政村信息不存在");
                #endregion

                var count = 0;
                #region 获取行政村信息上报统计

                if (adcdList.Count() != 0)
                {
                    var ADCDDisasterInfoBuilder = db.From<ADCDDisasterInfo>();
                    ADCDDisasterInfoBuilder.Where(x => x.Year == year && Sql.In(x.adcd, adcdList));
                    ADCDDisasterInfoBuilder.SelectDistinct(x => x.adcd);
                    count = db.Select(ADCDDisasterInfoBuilder).Count;
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村信息,
                        NoReported = adcdList.Count() - count,
                        HasReported = count
                    });
                }else
                {
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村信息,
                        NoReported = 0,
                        HasReported = 0
                    });
                }

                #endregion

                #region 获取行政村防汛防台工作组上报统计

                if (adcdList.Count() != 0)
                {
                    var VillageWorkingGroupBuilder = db.From<VillageWorkingGroup>();
                    VillageWorkingGroupBuilder.Where(x => x.Year == year && Sql.In(x.VillageADCD, adcdList));
                    VillageWorkingGroupBuilder.SelectDistinct(x => x.VillageADCD);
                    count = db.Select(VillageWorkingGroupBuilder).Count;
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村防汛防台工作组,
                        NoReported = adcdList.Count() - count,
                        HasReported = count
                    });
                }else
                {
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村防汛防台工作组,
                        NoReported = 0,
                        HasReported = 0
                    });
                }

                #endregion

                #region 获取行政村网格责任人上报统计

                if (adcdList.Count() != 0)
                {
                    var VillageGridPersonLiableBuilder = db.From<VillageGridPersonLiable>();
                    VillageGridPersonLiableBuilder.Where(x => x.Year == year && Sql.In(x.VillageADCD, adcdList));
                    VillageGridPersonLiableBuilder.SelectDistinct(x => x.VillageADCD);
                    count = db.Select(VillageGridPersonLiableBuilder).Count;
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村网格责任人,
                        NoReported = adcdList.Count() - count,
                        HasReported = count
                    });
                }
                else
                {
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村网格责任人,
                        NoReported = 0,
                        HasReported = 0
                    });
                }
                #endregion

                #region 获取行政村危险区转移人员上报统计

                if (adcdList.Count() != 0)
                {
                    var VillageTransferPersonBuilder = db.From<VillageTransferPerson>();
                    VillageTransferPersonBuilder.Where(x => x.Year == year && Sql.In(x.adcd, adcdList));
                    VillageTransferPersonBuilder.SelectDistinct(x => x.adcd);
                    count = db.Select(VillageTransferPersonBuilder).Count;
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村危险区人员转移清单,
                        NoReported = adcdList.Count() - count,
                        HasReported = count
                    });
                }else
                {
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村危险区人员转移清单,
                        NoReported = 0,
                        HasReported = 0
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
                    VillagePicBuilder.Where(x => x.Year == year && Sql.In(x.adcd, adcdList));
                    VillagePicBuilder.SelectDistinct(x => x.adcd);
                    count = db.Select(VillagePicBuilder).Count;
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村防汛防台形势图,
                        NoReported = adcdList.Count() - count,
                        HasReported = count
                    });
                }else
                {
                    list.Add(new VillageNumViewModel()
                    {
                        ZZTX = GrassrootsFloodCtrlEnums.ZZTXEnums.行政村防汛防台形势图,
                        NoReported =0,
                        HasReported = 0
                    });
                }
                #endregion
                //
                return list;
            }
        }
        /// <summary>
        /// 获取危险区转移人员行政村 （已上报和未上报的村名）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<VillageViewModel> GetVillageTransferPersonAdcd(GetVillageTransferPersonAdcd request)
        {
            using (var db=DbFactory.Open())
            {
                var year = DateTime.Now.Year;
                if (request.year != 0)
                    year = request.year;
                var builder = db.From<ADCDInfo>();
                builder.LeftJoin<ADCDInfo, VillageTransferPerson>((x, y) => x.adcd == y.adcd);
                builder.Where<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd);
               
                if (!string.IsNullOrEmpty(request.adnm))
                    builder.And(x=>x.adnm.Contains(request.adnm));
                builder.SelectDistinct(x => new {x.Id,x.adcd,x.adnm});
                if (request.type == 0)//0：未上报，1：已上报
                    builder.And<VillageTransferPerson>(y =>(y.DangerZoneName == string.Empty||y.DangerZoneName == null) && y.IfTransfer == null);
                else if (request.type == 1)
                    builder.And<VillageTransferPerson>(y => (y.DangerZoneName != "" && y.Year==year) || (y.IfTransfer == 1 && y.Year == year));
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
                return new BsTableDataSource<VillageViewModel>() {total = count,rows = list};
                
            }
        }


        /// <summary>
        /// 获取行政村危险区转移人员列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<VillageTransferPersonViewModel> GetVillageTransferPerson(GetVillageTransferPerson request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(adcd))
                    throw new Exception("请重新登录");
                //if (request.year==null)
                //    throw new Exception("年度异常");

                var builder = db.From<VillageTransferPerson>();
                builder.LeftJoin<VillageTransferPerson, ADCDInfo>((x, y) => x.adcd == y.adcd);
                builder.Where<VillageTransferPerson>(x=>x.DangerZoneName!="" && x.DangerZoneName!=null);
                if (adcd.Length == 15 && adcd.IndexOf("000000") > 0)//登陆的是乡镇用户
                    builder.And(y => y.adcd.StartsWith(adcd.Substring(0, 9)));
                else if (adcd.Length == 6 && adcd.IndexOf("00") < 0)//登陆的是县级用户
                    builder.And(y => y.adcd.StartsWith(adcd.Substring(0, 6)));
                else if (adcd == ((int)GrassrootsFloodCtrlEnums.AreaCode.省级编码).ToString()+"000000000")//管理员
                {

                }
                else
                    throw new Exception("登陆用户的所属行政区划编码不正确");
                if (!string.IsNullOrEmpty(request.name))
                    builder.And(y => y.DangerZoneName.Contains(request.name));
                if (!string.IsNullOrEmpty(request.adnm))
                    builder.And<ADCDInfo>(x => x.adnm.Contains(request.adnm));
                if (!string.IsNullOrEmpty(request.adcd))
                    builder.And(x => x.adcd==request.adcd);
                if (request.id!=0)
                    builder.And(x => x.ID==request.id);
                if (request.adcdId != 0)
                    builder.And<ADCDInfo>(x => x.Id == request.adcdId);
                if (request.year != null)
                    builder.And(y => y.Year == request.year);
                builder.Select("VillageTransferPerson.*,ADCDInfo.adnm");
                var count = db.Count(builder);

                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(x => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                    builder.OrderByDescending(x => request.Sort);
                else
                    builder.OrderBy(x => x.ID);

                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : request.PageIndex  * PageSize;
                builder.Limit(PageIndex, PageSize);
                var list = db.Select<VillageTransferPersonViewModel>(builder);

                return new BsTableDataSource<VillageTransferPersonViewModel>() { rows = list, total = count };
            }
        }
        /// <summary>
        /// 一个村转移人员清单合并
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<VillageTransferPersonViewModel> GetVillageTransferPerson1(GetVillageTransferPerson1 request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(adcd))
                    throw new Exception("请重新登录");
                //if (request.year==null)
                //    throw new Exception("年度异常");

                var builder = db.From<ADCDInfo>();
                builder.LeftJoin<ADCDInfo, VillageTransferPerson>((x, y) => x.adcd == y.adcd);
                builder.LeftJoin<VillageTransferPerson, DangerZone>((x,y)=>x.DangerZoneType == y.DangerZoneName);
                if (!string.IsNullOrEmpty(request.adcd))
                    builder.And(x => x.adcd == request.adcd);
                if (request.year != null)
                    builder.And<VillageTransferPerson>(y => y.Year == request.year);
                if(request.nums != null && request.nums > 1) builder.And<VillageTransferPerson>(y => y.AuditNums == request.nums);
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
        public BsTableDataSource<StatiscPerson> GetVillageTransferPerson2(GetVillageTransferPerson2 request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(adcd))
                    throw new Exception("请重新登录");
                //if (request.year==null)
                //    throw new Exception("年度异常");

                var builder = db.From<ADCDInfo>();
                builder.LeftJoin<ADCDInfo, VillageTransferPerson>((x, y) => x.adcd == y.adcd);
                builder.LeftJoin<VillageTransferPerson, DangerZone>((x, y) => x.DangerZoneType == y.DangerZoneName);
                if (!string.IsNullOrEmpty(request.adcd))
                    builder.And(x => x.adcd == request.adcd);
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                builder.And<VillageTransferPerson>(y => y.Year == _year);
               
                builder.Select("VillageTransferPerson.*,ADCDInfo.adnm,DangerZone.Id as DId");
                var count = db.Count(builder);

                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(x => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                    builder.OrderByDescending(x => request.Sort);
                else
                    builder.OrderBy(x => x.adcd);

                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : request.PageIndex * PageSize;
                builder.Limit(PageIndex, PageSize);
                var list = db.Select<VillageTransferPersonViewModel>(builder);
                var newlist1 = list.Select(w => w.PersonLiableName).Distinct().ToList();
                var newlist2 = list.Select(w => w.WarnPersonLiableName).Distinct().ToList();
                var newlist3 = list.Select(w=>w.DisasterPreventionManager).Distinct().ToList();
                List<VillageTransferPersonViewModel> rlist = new List<VillageTransferPersonViewModel>();
                newlist1.ForEach(w =>
                {
                    var f = list.Where(x => x.PersonLiableName == w).ToList();
                    var newpost = ""; var phone = "";
                    f.ForEach(y =>
                    {
                        //newpost += y.PersonLiablePost + ";";
                        phone += y.PersonLiableMobile + ';';
                    });
                    var fphones = phone.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    var fnewpost = newpost.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    VillageTransferPersonViewModel tplvm = new VillageTransferPersonViewModel()
                    {
                        PersonLiableName = w,
                        PersonLiablePost = "人员转移组", //string.Join(";", fnewpost),
                        PersonLiableMobile = string.Join(";", fphones),
                        ID = f.FirstOrDefault().ID
                    };
                    rlist.Add(tplvm);
                });
                newlist2.ForEach(w =>
                {
                    var f = list.Where(x => x.WarnPersonLiableName == w).ToList();
                    var newpost = ""; var phone = "";
                    f.ForEach(y =>
                    {
                        //newpost += y.WarnPersonLiablePost + ";";
                        phone += y.WarnPersonLiableMobile + ';';
                    });
                    var fphones = phone.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    var fnewpost = newpost.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    VillageTransferPersonViewModel tplvm = new VillageTransferPersonViewModel()
                    {
                        PersonLiableName = w,
                        PersonLiablePost = "监测预警组", //string.Join(";", fnewpost),
                        PersonLiableMobile = string.Join(";", fphones),
                        ID = f.FirstOrDefault().ID
                    };
                    rlist.Add(tplvm);
                });
                newlist3.ForEach(w =>
                {
                    var f = list.Where(x => x.DisasterPreventionManager == w).ToList();
                    var phone = "";
                    f.ForEach(y =>
                    {
                        phone += y.DisasterPreventionManagerMobile + ';';
                    });
                    var fphones = phone.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    VillageTransferPersonViewModel tplvm = new VillageTransferPersonViewModel()
                    {
                        PersonLiableName = w,
                        PersonLiablePost= "管理员",
                        PersonLiableMobile = string.Join(";", fphones),
                        ID = f.FirstOrDefault().ID
                    };
                    rlist.Add(tplvm);
                });
                List<StatiscPerson> rlist1 = new List<StatiscPerson>();
                var newlist4 = rlist.Select(w => w.PersonLiableName).Distinct().ToList();
                newlist4.Remove("");
                newlist4.Remove(null);
                newlist4.ForEach(w=> {
                    var f = rlist.Where(x => x.PersonLiableName == w).ToList();
                    var newpost = ""; var phone = "";
                    f.ForEach(y =>
                    {
                        newpost += y.PersonLiablePost + ";";
                        phone += y.PersonLiableMobile + ';';
                    });
                    var fphones = phone.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    var fnewpost = newpost.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    StatiscPerson tplvm = new StatiscPerson()
                    {
                        personLiable = w,
                        post = string.Join(";", fnewpost),
                        handPhone = string.Join(";", fphones),
                        id = f.FirstOrDefault().ID,
                        adcd = f.FirstOrDefault().adcd
                    };
                    rlist1.Add(tplvm);
                });
                return new BsTableDataSource<StatiscPerson>() { rows = rlist1, total = rlist1.Count() };
            }
        }

        /// <summary>
        /// 删除行政村危险区转移人员信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DelVillageTransferPerson(string ids)
        {
            using (var db = DbFactory.Open())
            {
                ArrayList arr = new ArrayList();
                string[] arrs = ids.Split(',');
                for (int i = 0; i < arrs.Length; i++)
                {
                    var id = int.Parse(arrs[i]);
                    arr.Add(id);
                    #region 日志
                    var _vgr = db.Single<VillageTransferPerson>(w => w.ID == id);
                    StringBuilder sb = new StringBuilder();
                    var adcdInfo = ZZTXManager.GetADCDInfoByADCD(_vgr.adcd);
                    sb.Append("在栏目{组织责任/行政村危险区人员转移清单}下,删除了数据{");
                    sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                    sb.Append("危险区（点）名称：" + _vgr.DangerZoneName + ";");
                    sb.Append("类别：" + _vgr.DangerZoneType + ";");
                    sb.Append("避灾场所管理员：" + _vgr.DisasterPreventionManager + ";");
                    sb.Append("避灾场所管理员手机：" + _vgr.DisasterPreventionManagerMobile + ";");
                    sb.Append("避灾场所名称：" + _vgr.DisasterPreventionName + ";");
                    sb.Append("户主手机：" + _vgr.HouseholderMobile + ";");
                    sb.Append("户主姓名：" + _vgr.HouseholderName + ";");
                    sb.Append("居住人数：" + _vgr.HouseholderNum + ";");
                    sb.Append("纬度：" + _vgr.Lat + ";");
                    sb.Append("经度：" + _vgr.Lng + ";");
                    sb.Append("责任人手机：" + _vgr.PersonLiableMobile + ";");
                    sb.Append("责任人姓名：" + _vgr.PersonLiableName + ";");
                    sb.Append("责任人职务：" + _vgr.PersonLiablePost + ";");
                    sb.Append("位置：" + _vgr.Position + ";");
                    sb.Append("有无安全鉴定：" + _vgr.SafetyIdentification + ";");
                    sb.Append("预警责任人手机：" + _vgr.WarnPersonLiableMobile + ";");
                    sb.Append("预警责任人职务：" + _vgr.WarnPersonLiablePost + ";");
                    sb.Append("预警责任人姓名：" + _vgr.WarnPersonLiableName + ";");
                    sb.Append("年度：" + _vgr.Year + ";");
                    sb.Append("}");
                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.删除);
                    #endregion
                }
                string _newids = string.Join(",", arrs);
                //先取出数据
                var rlist = db.Select<VillageTFStaictis>("select distinct(adcd),SUM(HouseholderNum) as HouseholderNum,COUNT(adcd) as PointNum from VillageTransferPerson where ID in(" + _newids + ") group by adcd ");
                //删除
                var r = db.Delete<VillageTransferPerson>(x => Sql.In(x.ID, arr)) > 0;
                if(r)
                {
                    //村 受灾点 受灾害影响人口 更新
                    foreach (VillageTFStaictis item in rlist)
                    {
                        using (var dbADCDStatics = DbFactory.Open())
                        {
                            var dbbuilder = dbADCDStatics.From<ADCDDisasterInfo>().Where(w => w.adcd == item.adcd).Update(x => new { x.PointNum, x.PopulationNum });
                            dbADCDStatics.UpdateAdd(() => new ADCDDisasterInfo { PointNum = -item.PointNum, PopulationNum = -item.HouseholderNum }, dbbuilder);
                        }
                    }
                }
                return r;
            }
        }

        /// <summary>
        /// 保存行政村危险区转移人员信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public BaseResult SaveVillageTransferPerson(SaveVillageTransferPerson request)
        {
            using (var db=DbFactory.Open())
            {
                var adcdInfo=ZZTXManager.GetADCDInfoByADCD(request.adcd);
                   var info = new VillageTransferPerson();
                info.adcd = request.adcd;
                info.DangerZoneName = request.DangerZoneName;
                info.DangerZoneType = request.DangerZoneType;
                info.DisasterPreventionManager = request.DisasterPreventionManager;
                info.DisasterPreventionManagerMobile = request.DisasterPreventionManagerMobile;
                info.DisasterPreventionName = request.DisasterPreventionName;
                info.HouseholderMobile = request.HouseholderMobile;
                info.HouseholderName = request.HouseholderName;
                info.HouseholderNum = request.HouseholderNum;
                info.Lat = request.Lat;
                info.Lng = request.Lng;
                info.PersonLiableMobile = request.PersonLiableMobile;
                info.PersonLiableName = request.PersonLiableName;
                info.PersonLiablePost = request.PersonLiablePost;
                info.Position = request.Position;
                info.SafetyIdentification = request.SafetyIdentification;
                info.WarnPersonLiableMobile = request.WarnPersonLiableMobile;
                info.WarnPersonLiablePost = request.WarnPersonLiablePost;
                info.WarnPersonLiableName = request.WarnPersonLiableName;
                info.Year = DateTime.Now.Year;
                info.Remark = request.Remark;
                //新数据
                List<VillageTransferPerson> _newdata = new List<VillageTransferPerson>();
                _newdata.Add(info);
                //
                operateLog log = new operateLog();
                log.userName = RealName;
                log.operateTime=DateTime.Now;

                if (request.Id != 0)
                {
                    #region 旧数据，新数据
                    //取出数据
                    //写入更新记录
                    var rf = db.Single<VillageTransferPerson>(w => w.ID == request.Id);
                    var _olddata = new VillageTransferPerson()
                    {
                        adcd = rf.adcd,
                        DangerZoneName = rf.DangerZoneName,
                        DangerZoneType = rf.DangerZoneType,
                        DisasterPreventionManager = rf.DisasterPreventionManager,
                        DisasterPreventionManagerMobile = rf.DisasterPreventionManagerMobile,
                        DisasterPreventionName = rf.DisasterPreventionName,
                        HouseholderMobile = rf.HouseholderMobile,
                        HouseholderName = rf.HouseholderName,
                        HouseholderNum = rf.HouseholderNum,
                        Lat = rf.Lat,
                        Lng = rf.Lng,
                        PersonLiableMobile = rf.PersonLiableMobile,
                        PersonLiableName = rf.PersonLiableName,
                        PersonLiablePost = rf.PersonLiablePost,
                        Position = rf.Position,
                        SafetyIdentification = rf.SafetyIdentification,
                        WarnPersonLiableMobile = rf.WarnPersonLiableMobile,
                        WarnPersonLiablePost = rf.WarnPersonLiablePost,
                        WarnPersonLiableName = rf.WarnPersonLiableName,
                        Year = rf.Year
                    };
                    if (AuditNums != null)
                    {
                        List<VillageTransferPerson> _listOldData = new List<VillageTransferPerson>();
                        _listOldData.Add(_olddata);
                        info.AuditNums = AuditNums.Value + 1;
                        //旧数据写入实体
                        info.OldData = JsonTools.ObjectToJson(_listOldData);
                        //新数据写入实体
                        info.NewData = JsonTools.ObjectToJson(_newdata);
                    }
                    #endregion
                    //数据更新前取出历史数据
                    var rlist = db.Select<VillageTransferPerson>(w=>w.ID == request.Id);
                    //
                    info.ID = request.Id;
                    log.operateMsg = "更新"+ request.Year+"年"+adcdInfo.adnm+"，名为："+request.DangerZoneName+"危险区(点)转移人员信息";
                    List<operateLog> listLog = new List<operateLog>();
                    listLog.Add(log);
                    info.operateLog = JsonTools.ObjectToJson(listLog);
                    var r = db.Update(info) == 1;
                    #region 日志
                    StringBuilder sb = new StringBuilder();
                    sb.Append("在栏目{组织责任/行政村危险区人员转移清单}下,更新数据{");
                    sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                    sb.Append("危险区（点）名称：" + _olddata.DangerZoneName + ";");
                    sb.Append("类别：" + _olddata.DangerZoneType + ";");
                    sb.Append("避灾场所管理员：" + _olddata.DisasterPreventionManager + ";");
                    sb.Append("避灾场所管理员手机：" + _olddata.DisasterPreventionManagerMobile + ";");
                    sb.Append("避灾场所名称：" + _olddata.DisasterPreventionName + ";");
                    sb.Append("户主手机：" + _olddata.HouseholderMobile + ";");
                    sb.Append("户主姓名：" + _olddata.HouseholderName + ";");
                    sb.Append("居住人数：" + _olddata.HouseholderNum + ";");
                    sb.Append("纬度：" + _olddata.Lat + ";");
                    sb.Append("经度：" + _olddata.Lng + ";");
                    sb.Append("责任人手机：" + _olddata.PersonLiableMobile + ";");
                    sb.Append("责任人姓名：" + _olddata.PersonLiableName + ";");
                    sb.Append("责任人职务：" + _olddata.PersonLiablePost + ";");
                    sb.Append("位置：" + _olddata.Position + ";");
                    sb.Append("有无安全鉴定：" + _olddata.SafetyIdentification + ";");
                    sb.Append("预警责任人手机：" + _olddata.WarnPersonLiableMobile + ";");
                    sb.Append("预警责任人职务：" + _olddata.WarnPersonLiablePost + ";");
                    sb.Append("预警责任人姓名：" + _olddata.WarnPersonLiableName + ";");
                    sb.Append("年度：" + _olddata.Year + ";");
                    sb.Append("}为{");
                    sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                    sb.Append("危险区（点）名称：" + info.DangerZoneName + ";");
                    sb.Append("类别：" + info.DangerZoneType + ";");
                    sb.Append("避灾场所管理员：" + info.DisasterPreventionManager + ";");
                    sb.Append("避灾场所管理员手机：" + info.DisasterPreventionManagerMobile + ";");
                    sb.Append("避灾场所名称：" + info.DisasterPreventionName + ";");
                    sb.Append("户主手机：" + info.HouseholderMobile + ";");
                    sb.Append("户主姓名：" + info.HouseholderName + ";");
                    sb.Append("居住人数：" + info.HouseholderNum + ";");
                    sb.Append("纬度：" + info.Lat + ";");
                    sb.Append("经度：" + info.Lng + ";");
                    sb.Append("责任人手机：" + info.PersonLiableMobile + ";");
                    sb.Append("责任人姓名：" + info.PersonLiableName + ";");
                    sb.Append("责任人职务：" + info.PersonLiablePost + ";");
                    sb.Append("位置：" + info.Position + ";");
                    sb.Append("有无安全鉴定：" + info.SafetyIdentification + ";");
                    sb.Append("预警责任人手机：" + info.WarnPersonLiableMobile + ";");
                    sb.Append("预警责任人职务：" + info.WarnPersonLiablePost + ";");
                    sb.Append("预警责任人姓名：" + info.WarnPersonLiableName + ";");
                    sb.Append("年度：" + info.Year + ";");
                    sb.Append("}");
                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
                    #endregion
                    //if (r)
                    //{
                    //    //村 受灾点 受灾害影响人口 更新
                    //    var oldNum = rlist.FirstOrDefault().HouseholderNum;
                    //    var newNum = request.HouseholderNum;
                    //    var newhouseholdernum = newNum - oldNum;
                    //    using (var dbADCDStatics = DbFactory.Open())
                    //    {
                    //        var _year = request.Year == 0 ? DateTime.Now.Year : request.Year;
                    //        var dbbuilder = dbADCDStatics.From<ADCDDisasterInfo>().Where(w => w.adcd == request.adcd && w.Year==_year).Update(x => new { x.PopulationNum });
                    //        dbADCDStatics.UpdateAdd(() => new ADCDDisasterInfo { PopulationNum = newhouseholdernum }, dbbuilder);
                    //    }
                    //}
                    BaseResult br = new BaseResult();
                    br.IsSuccess = r;
                    return br;
                }
                else
                {
                    BaseResult br = null;
                    var f = db.Single<VillageTransferPerson>(w => w.adcd == request.adcd && w.IfTransfer == 1);
                    if (f != null) {
                        br = new BaseResult();
                        br.IsSuccess = false;
                        br.ErrorMsg = "保存失败！村被标记过无转移人员，新增人员时请先在已上报里删除村。";
                        return br;
                    }
                    #region 旧数据，新数据
                        //写入更新记录
                    if (AuditNums != null)
                    {
                        info.AuditNums = AuditNums.Value + 1;
                    }
                    #endregion
                    info.CreateTime = DateTime.Now;
                    log.operateMsg = "新增" + request.Year + "年" + adcdInfo.adnm + "，名为：" + request.DangerZoneName + "危险区(点)转移人员信息";
                    List<operateLog> listLog = new List<operateLog>();
                    listLog.Add(log);
                    info.operateLog = JsonTools.ObjectToJson(listLog);
                    var r = db.Insert(info) == 1;
                    #region 日志
                    StringBuilder sb = new StringBuilder();
                    sb.Append("在栏目{组织责任/行政村危险区人员转移清单}下,新增数据{");
                    sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                    sb.Append("危险区（点）名称：" + info.DangerZoneName + ";");
                    sb.Append("类别：" + info.DangerZoneType + ";");
                    sb.Append("避灾场所管理员：" + info.DisasterPreventionManager + ";");
                    sb.Append("避灾场所管理员手机：" + info.DisasterPreventionManagerMobile + ";");
                    sb.Append("避灾场所名称：" + info.DisasterPreventionName + ";");
                    sb.Append("户主手机：" + info.HouseholderMobile + ";");
                    sb.Append("户主姓名：" + info.HouseholderName + ";");
                    sb.Append("居住人数：" + info.HouseholderNum + ";");
                    sb.Append("纬度：" + info.Lat + ";");
                    sb.Append("经度：" + info.Lng + ";");
                    sb.Append("责任人手机：" + info.PersonLiableMobile + ";");
                    sb.Append("责任人姓名：" + info.PersonLiableName + ";");
                    sb.Append("责任人职务：" + info.PersonLiablePost + ";");
                    sb.Append("位置：" + info.Position + ";");
                    sb.Append("有无安全鉴定：" + info.SafetyIdentification + ";");
                    sb.Append("预警责任人手机：" + info.WarnPersonLiableMobile + ";");
                    sb.Append("预警责任人职务：" + info.WarnPersonLiablePost + ";");
                    sb.Append("预警责任人姓名：" + info.WarnPersonLiableName + ";");
                    sb.Append("年度：" + info.Year + ";");
                    sb.Append("}");
                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
                    #endregion
                    //if (r)
                    //{
                    //    //村 受灾点 受灾害影响人口 更新
                    //    using (var dbADCDStatics = DbFactory.Open())
                    //    {
                    //        var _year = request.Year == 0 ? DateTime.Now.Year : request.Year;
                    //        var dbbuilder = dbADCDStatics.From<ADCDDisasterInfo>().Where(w=>w.adcd==request.adcd && w.Year == _year).Update(x => new { x.PointNum, x.PopulationNum });
                    //        dbADCDStatics.UpdateAdd(()=>new ADCDDisasterInfo { PointNum=1,PopulationNum=request.HouseholderNum }, dbbuilder);
                    //    }
                    //}
                    br = new BaseResult();
                    br.IsSuccess = r;
                    return br;
                }
            }
        }

       public bool NoVillageTransferPerson(NoVillageTransferPerson request)
        {
            using (var db = DbFactory.Open())
            {
                var adcdInfo = ZZTXManager.GetADCDInfoByADCD(request.adcd);
                var info = new VillageTransferPerson();
                info.adcd = request.adcd;
                info.Year = DateTime.Now.Year;
                info.IfTransfer = 1;
                if(AuditNums != null) { info.AuditNums = AuditNums + 1; }
                //新数据
                List<VillageTransferPerson> _newdata = new List<VillageTransferPerson>();
                _newdata.Add(info);
                //
                operateLog log = new operateLog();
                log.userName = RealName;
                log.operateTime = DateTime.Now;
                log.operateMsg = "" + DateTime.Now.Year + "年村"+ adcdInfo.adnm+ "提交了暂无危险区(点)转移人员的信息";
                List<operateLog> listLog = new List<operateLog>();
                listLog.Add(log);
                info.operateLog = JsonTools.ObjectToJson(listLog);
                var f = db.Single<VillageTransferPerson>(w => w.adcd == request.adcd && w.IfTransfer == 1);
                if (f == null)
                {
                    var r = db.Insert(info) == 1;
                    #region 日志
                    StringBuilder sb = new StringBuilder();
                    sb.Append("在栏目{组织责任/行政村危险区人员转移清单}下,新增数据{");
                    sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                    sb.Append("},改村暂无危险区(点)转可移人员信息");
                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
                    #endregion
                    return r;
                }else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 根据行政村ADCD删除危险区转移人员
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool DelVillageTransferPersonByADCD(DelVillageTransferPersonByADCD request)
        {
            using (var db=DbFactory.Open())
            {
                var year = request.Year != 0 ? request.Year : DateTime.Now.Year;
                #region 日志
                StringBuilder sb = new StringBuilder();
                sb.Append("在栏目{组织责任/行政村危险区人员转移清单}下,删除了村{" + request.adcd + "}及其数据");
                _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.删除);
                #endregion
                return db.Delete<VillageTransferPerson>(x => x.adcd == request.adcd && x.Year== year) >0;
            }
        }

        #endregion
    }
}
