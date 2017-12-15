using Dy.Common;
using GrassrootsFloodCtrl.Logic.Country;
using GrassrootsFloodCtrl.Logic.Supervise;
using GrassrootsFloodCtrl.Logic.Town;
using GrassrootsFloodCtrl.Logic.Village;
using GrassrootsFloodCtrl.Logic.Village.VillageGrid;
using GrassrootsFloodCtrl.Logic.Village.WorkingGroup;
using GrassrootsFloodCtrl.Logic.ZZTX;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.AppApi;
using GrassrootsFloodCtrl.Model.CountryPerson;
using GrassrootsFloodCtrl.Model.DangerZone;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.Model.Town;

using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.CApp;
using GrassrootsFloodCtrl.ServiceModel.Country;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.ServiceModel.AppApi;

namespace GrassrootsFloodCtrl.Logic.CApp
{
    public class CAppManager : ManagerBase, ICAppManager
    {
        public IZZTXManager _IZZTXManager { get; set; }
        public ISuperviseManager _ISupervise { get; set; }
        public ICountryPersonManager _ICountryPersonManager { get; set; }
        public ITownManager _ITownManager { get; set; }
        public IVillageWorkingGroupManage Iworkgroup { get; set; }
        public IVillageGridManage Igrid { get; set; }
        public IVillageTransferPersonManager Itransfer { get; set; }

        #region 综合应用
        /// <summary>
        /// 镇概括
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<TownInfoAppViewModel> GetTownInfo(GetTownInfo request)
        {
            if (string.IsNullOrEmpty(request.adcd) || !request.adcd.Contains("000000")) throw new Exception("adcd无效！");
            var _year = request.year == null ? DateTime.Now.Year : request.year;
            using (var db = DbFactory.Open())
            {
                List<TownInfoAppViewModel> list = new List<TownInfoAppViewModel>();
                TownInfoAppViewModel _model = new TownInfoAppViewModel();
                //村信息
                var villagelist = _IZZTXManager.GetADCDInfo(new GetADCDInfo() { adcd = request.adcd, levle = 51, PageSize = 9999 });
                _model.cuncount = villagelist != null ? Convert.ToInt32(villagelist.total) : 0;
                _model.cuns = villagelist != null ? villagelist.rows : null;
                //转移清单
                var transferlist = db.Select<VillageTransferPerson>(w => w.adcd.Contains(request.adcd.Substring(0, 9)) && w.Year == _year && w.IfTransfer == null);
                _model.ZDPoint = transferlist != null ? transferlist.Count() : 0;
                _model.ZDManNums = transferlist != null && transferlist.Count() > 0 ? transferlist.Sum(w => w.HouseholderNum) : 0;
                //镇本级
                var townpersons = db.Select<TownPersonLiable>(w => w.adcd == request.adcd && w.Year == _year);
                _model.ZhenBenji = townpersons != null && townpersons.Count() > 0 ? townpersons.Select(w => w.Name).Distinct().Count() : 0;
                //村本级
                //var villagegridperson = db.Select<>(w => w.PersonLiable != "" && w.VillageADCD.Contains(request.adcd.Substring(0, 9))).Select(w => w.PersonLiable).Distinct();
                //var villagegroupperson = db.Select<VillageWorkingGroup>(w => w.PersonLiable != "" && w.VillageADCD.Contains(request.adcd.Substring(0, 9))).Select(w => w.PersonLiable).Distinct();
                //var villagepersons1 = db.Select<VillageTransferPerson>(w => w.PersonLiableName != "" && w.adcd.Contains(request.adcd.Substring(0, 9))).Select(w => w.PersonLiableName).Distinct();
                //var villagepersons2 = db.Select<VillageTransferPerson>(w => w.DisasterPreventionManager != "" && w.adcd.Contains(request.adcd.Substring(0, 9))).Select(w => w.DisasterPreventionManager).Distinct();
                //var villagepersons3 = db.Select<VillageTransferPerson>(w => w.WarnPersonLiableName != "" && w.adcd.Contains(request.adcd.Substring(0, 9))).Select(w => w.WarnPersonLiableName).Distinct();
                //var villagepersons0 = villagepersons1.Union(villagepersons2).Union(villagepersons3).Union(villagegridperson).Union(villagegroupperson);
                var villagepersons0 = db.Select<VillageGridPersonLiable>("select t.name as name from("
                        + " select distinct PersonLiable as name from VillageWorkingGroup where Year=" + _year + " and PersonLiable != '' and VillageADCD like '" + request.adcd.Substring(0, 9) + "%'"
                        + " union select distinct PersonLiable as name from VillageGridPersonLiable where Year=" + _year + " and PersonLiable != '' and VillageADCD like '" + request.adcd.Substring(0, 9) + "%'"
                        + " union select distinct PersonLiableName as name from VillageTransferPerson where Year=" + _year + " and PersonLiableName != '' and adcd like '" + request.adcd.Substring(0, 9) + "%'"
                        + " union select distinct WarnPersonLiableName as name from VillageTransferPerson where Year=" + _year + " and WarnPersonLiableName != '' and adcd like '" + request.adcd.Substring(0, 9) + "%'"
                        + " union select distinct DisasterPreventionManager as name from VillageTransferPerson where Year=" + _year + " and DisasterPreventionManager != '' and adcd like '" + request.adcd.Substring(0, 9) + "%') as t").Count();
                _model.CunBenji = villagepersons0;

                //水利工程
                List<GridModel> listg = new List<GridModel>();
                GridModel gm;
                if (transferlist != null && transferlist.Count() > 0)
                {
                    gm = new GridModel();
                    gm.wanggeName = "危房";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "危房").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "山洪灾害危险区";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "山洪灾害危险区").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "地质灾害点";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "地质灾害点").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "低洼易涝区";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "低洼易涝区").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "屋顶山塘";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "屋顶山塘").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "工棚";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "工棚").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "堤防险段";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "堤防险段").Count();
                    listg.Add(gm);

                    gm = new GridModel();
                    gm.wanggeName = "海塘险段";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "堤防险段").Count();
                    listg.Add(gm);

                    gm = new GridModel();
                    gm.wanggeName = "其它";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "其它").Count();
                    listg.Add(gm);
                }
                _model.rows = listg;
                //乡镇名 所属 县
                var areas = db.Select<ADCDInfo>(w => w.adcd.Contains(request.adcd.Substring(0, 6)) && w.adcd.Contains("000000"));
                var towninfo = areas.Single(w => w.adcd == request.adcd);
                _model.ZhenName = towninfo.adnm;
                _model.LGT = towninfo.lat == null ? 0 : towninfo.lat.Value;
                _model.LTT = towninfo.lng == null ? 0 : towninfo.lng.Value;
                _model.ShiName = areas.Single(w => w.adcd == request.adcd.Substring(0, 6) + "000000000").adnm;
                list.Add(_model);
                return list;
            }
        }

        /// <summary>
        /// 镇防指成员
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<TownPersonAppViewModel> GetTownPerson(GetTownPerson request)
        {
            if (string.IsNullOrEmpty(request.adcd) || !request.adcd.Contains("000000")) throw new Exception("adcd无效！");
            using (var db = DbFactory.Open())
            {
                List<TownPersonAppViewModel> list = new List<TownPersonAppViewModel>();
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                var getList = _ITownManager.GetTownList(new GetTownList() { adcd = request.adcd, year = _year, PageSize = 9999 });
                if (getList != null && getList.total > 0)
                {
                    var gridlist = db.Select<Model.Post.Post>(w => w.PostType == "镇级防汛防台责任人").OrderBy(o => o.orderId).ToList();
                    gridlist.ForEach(w =>
                    {
                        TownPersonAppViewModel _model = new TownPersonAppViewModel();
                        var flist = getList.rows.Where(x => x.Position == w.PostName).ToList();
                        if (flist != null)
                        {
                            _model.GWID = w.orderId.Value;
                            _model.GWName = w.PostName;
                            _model.Datas = flist;
                            list.Add(_model);
                        }
                    });
                }
                return list;
            }
        }
        /// <summary>
        /// 镇网格责任人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<TownGridPersonAppViewModel> GetTownGridMan(GetTownGridMan request)
        {
            if (string.IsNullOrEmpty(request.adcd) || !request.adcd.Contains("000000")) throw new Exception("adcd无效！");
            var _year = request.year == null ? DateTime.Now.Year : request.year;
            using (var db = DbFactory.Open())
            {
                List<TownGridPersonAppViewModel> list = new List<TownGridPersonAppViewModel>();
                var grouplist = db.Select<VillageWorkingGroup>(w => w.VillageADCD.Contains(request.adcd.Substring(0, 9)) && w.Year == _year && w.VillageADCD != request.adcd);

                VillageWorkingGroupViewModel _model1;
                var adcdlist = db.Select<ADCDInfo>(w => w.adcd.Contains(request.adcd.Substring(0, 9)) && w.adcd != request.adcd);
                adcdlist.ForEach(w =>
                {
                    TownGridPersonAppViewModel _model = new TownGridPersonAppViewModel();
                    _model.CunADCD = w.adcd;
                    _model.CunName = w.adnm;
                    if (grouplist != null)
                    {
                        List<VillageWorkingGroupViewModel> vwgv = new List<VillageWorkingGroupViewModel>();
                        var f1 = grouplist.Where(x => x.VillageADCD == w.adcd && x.Post == "乡级包片领导").FirstOrDefault();
                        if (f1 != null)
                        {
                            _model1 = new VillageWorkingGroupViewModel();
                            _model1.Post = f1.Post;
                            _model1.PersonLiable = f1.PersonLiable;
                            _model1.Position = f1.Position;
                            _model1.HandPhone = f1.HandPhone;
                            vwgv.Add(_model1);
                        }

                        var f2 = grouplist.Where(x => x.VillageADCD == w.adcd && x.Post == "驻村干部").FirstOrDefault();
                        if (f2 != null)
                        {
                            _model1 = new VillageWorkingGroupViewModel();
                            _model1.Post = f2.Post;
                            _model1.PersonLiable = f2.PersonLiable;
                            _model1.Position = f2.Position;
                            _model1.HandPhone = f2.HandPhone;
                            vwgv.Add(_model1);
                        }
                        _model.Rows = vwgv;
                    }
                    list.Add(_model);
                });
                return list;
            }
        }
        /// <summary>
        /// 村概况
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public VillageInfoAppViewModel GetCunDot(GetCunDot request)
        {
            if (string.IsNullOrEmpty(request.adcd)) throw new Exception("adcd无效！");
            var _year = request.year == null ? DateTime.Now.Year : request.year;
            using (var db = DbFactory.Open())
            {
                VillageInfoAppViewModel _model = new VillageInfoAppViewModel();
                _model.ADCD = request.adcd;
                var sql = "select a.TotalNum as AllManNums,b.pointnum as DisasterPoint,b.pointmans as DisasterManNums, c.lng as LGT ,c.lat as LTT,c.adnm as Name,d.qrpath from ADCDDisasterInfo as a left join (" +
                    "select COUNT(*) as pointnum, SUM(HouseholderNum) as pointmans, adcd from VillageTransferPerson where adcd = '" + request.adcd + "' and IfTransfer is null and Year = " + _year + "" +
                    "group by adcd ) as b on a.adcd = b.adcd left join ADCDInfo as c on a.adcd=c.adcd left join ADCDQRCode d on a.adcd=d.adcd where a.adcd = '" + request.adcd + "' and Year = " + _year + "";
                var allinfo = db.Single<VillageInfoAppViewModel>(sql);
                _model.AllManNums = allinfo.AllManNums != null ? allinfo.AllManNums : 0;
                _model.DisasterPoint = allinfo.DisasterPoint != null ? allinfo.DisasterPoint : 0;
                _model.DisasterManNums = allinfo.DisasterManNums != null ? allinfo.DisasterManNums : 0;
                _model.Name = string.IsNullOrEmpty(allinfo.Name) ? "" : allinfo.Name;
                _model.qrpath = string.IsNullOrEmpty(allinfo.qrpath) ? "" : allinfo.qrpath;
                var areas = db.Select<ADCDInfo>(w => w.adcd.Contains(request.adcd.Substring(0, 4)) && w.adcd.EndsWith("000000"));
                if (areas != null)
                {
                    _model.ZhenName = areas.Single(x => x.adcd == request.adcd.Substring(0, 9) + "000000").adnm;
                    _model.ShiName = areas.Single(x => x.adcd == request.adcd.Substring(0, 6) + "000000000").adnm;
                }
                _model.LGT = allinfo.LGT;
                _model.LTT = allinfo.LTT;
                //责任人数
                var _villagePersonNums = db.SqlList<VillageGridPersonLiable>("EXEC VillagePersonCount @adcd", new { adcd = request.adcd });
                _model.villagePersonNums = _villagePersonNums == null ? 0 : _villagePersonNums.Count();
                //到岗人数
                var builderInPostNums = db.From<AppRecord>();
                builderInPostNums.Where(w => w.adcd == request.adcd);
                builderInPostNums.SelectDistinct(x => new { x.adcd });
                var _villageInPostNums = db.Select(builderInPostNums);
                _model.villageInPostNums = _villageInPostNums == null ? 0 : _villageInPostNums.Count();
                //水利工程
                List<GridModel> listg = new List<GridModel>();
                var sl = db.Select<VillageTransferPerson>(w => w.adcd == request.adcd && w.Year == _year);
                GridModel gm;
                if (sl != null && sl.Count() > 0)
                {
                    gm = new GridModel();
                    gm.wanggeName = "危房";
                    gm.wanggeCount = sl.Where(w => w.DangerZoneType == "危房").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "山洪灾害危险区";
                    gm.wanggeCount = sl.Where(w => w.DangerZoneType == "山洪灾害危险区").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "地质灾害点";
                    gm.wanggeCount = sl.Where(w => w.DangerZoneType == "地质灾害点").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "低洼易涝区";
                    gm.wanggeCount = sl.Where(w => w.DangerZoneType == "低洼易涝区").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "屋顶山塘";
                    gm.wanggeCount = sl.Where(w => w.DangerZoneType == "屋顶山塘").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "工棚";
                    gm.wanggeCount = sl.Where(w => w.DangerZoneType == "工棚").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "堤防险段";
                    gm.wanggeCount = sl.Where(w => w.DangerZoneType == "堤防险段").Count();
                    listg.Add(gm);

                    gm = new GridModel();
                    gm.wanggeName = "海塘险段";
                    gm.wanggeCount = sl.Where(w => w.DangerZoneType == "海塘险段").Count();
                    listg.Add(gm);

                    gm = new GridModel();
                    gm.wanggeName = "其它";
                    gm.wanggeCount = sl.Where(w => w.DangerZoneType == "其它").Count();
                    listg.Add(gm);
                }
                _model.rows = listg;
                return _model;
            }
        }
        /// <summary>
        /// 返回关键字匹配
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<AppKeyViewModel> GetKeys(GetKeys request)
        {
            if (string.IsNullOrEmpty(request.name)) throw new Exception("关键字不能为空！");
            if (string.IsNullOrEmpty(adcd)) throw new Exception("adcd不能为空！");
            var _year = request.year == null ? DateTime.Now.Year : request.year;
            using (var db = DbFactory.Open())
            {
                List<AppKeyViewModel> _list = new List<AppKeyViewModel>();
                AppKeyViewModel m;
                #region 县 乡镇 责任人：0镇4村1镇责任人2村责任人3县责任人
                if (RowID == 5)
                {
                    //省
                    //乡镇
                    var prov = db.Select<AppKeyViewModel>("select adcd as ADCD,adnm as Name from ADCDInfo where (CONVERT(int,SUBSTRING(adcd,7,3)) > 0 or CONVERT(int,SUBSTRING(adcd,10,3)) > 0) and  adnm like '%" + request.name.Trim() + "%'");
                    prov.ForEach(w =>
                    {
                        m = new AppKeyViewModel();
                        m.ADCD = w.ADCD;
                        m.Name = w.Name;
                        //0镇4村
                        m.ctype = w.ADCD.LastIndexOf("000000") > 1 ? 0 : 4;
                        _list.Add(m);
                    });
                    #region
                    //镇责任人
                    var townpreson = db.Select<AppKeyViewModel>("select distinct(Ltrim(rtrim(Name))) as Name,ADCD, ctype=1 from TownPersonLiable where Year=" + _year + " and Name like '%" + request.name.Trim() + "%'");
                    //村责任人
                    var workingperson = db.Select<AppKeyViewModel>("select distinct(PersonLiable) as Name,VillageADCD as ADCD, ctype=2 from VillageWorkingGroup where Year=" + _year + " and PersonLiable like '%" + request.name.Trim() + "%'");
                    var gridperson = db.Select<AppKeyViewModel>("select distinct(PersonLiable) as Name,VillageADCD as ADCD, ctype=2 from VillageGridPersonLiable where Year=" + _year + " and PersonLiable like '%" + request.name.Trim() + "%'");
                    //var transperson = db.Select<AppKeyViewModel>("select  distinct(Ltrim(rtrim(PersonLiableName))) AS Name,ADCD, ctype=2 from VillageTransferPerson where Year=" + _year + " and PersonLiableName != '' and PersonLiableName like '%" + request.name.Trim() + "%'");
                    //var transperson1 = db.Select<AppKeyViewModel>("select  distinct(Ltrim(rtrim(WarnPersonLiableName))) AS Name,ADCD, ctype=2 from VillageTransferPerson where Year=" + _year + " and WarnPersonLiableName != '' and WarnPersonLiableName like '%" + request.name.Trim() + "%'");
                    //var transperson2 = db.Select<AppKeyViewModel>("select  distinct(Ltrim(rtrim(DisasterPreventionManager))) AS Name,ADCD, ctype=2 from VillageTransferPerson where Year=" + _year + " and DisasterPreventionManager != '' and DisasterPreventionManager like '%" + request.name.Trim() + "%' ");
                    //transperson.AddRange(transperson1);
                    //transperson.AddRange(transperson2);
                    var transperson = db.Select<AppKeyViewModel>("select LTRIM(RTRIM(t.name)) as name,t.adcd from(select distinct PersonLiableName as Name,adcd from VillageTransferPerson where Year=" + _year + " and PersonLiableName like '%" + request.name.Trim() + "%'" +
                        " union select distinct WarnPersonLiableName as Name, adcd from VillageTransferPerson where Year = " + _year + " and WarnPersonLiableName like '%" + request.name.Trim() + "%'" +
                        " union select distinct DisasterPreventionManager as Name, adcd from VillageTransferPerson where Year = " + _year + " and DisasterPreventionManager like '%" + request.name.Trim() + "%') as t");
                    List<AppKeyViewModel> transplist = new List<AppKeyViewModel>();
                    transperson.ForEach(w =>
                    {
                        var f = transplist.Where(x => x.ADCD == w.ADCD && x.Name == w.Name).ToList();
                        if (f == null)
                        {
                            AppKeyViewModel akv = new AppKeyViewModel()
                            {
                                Name = w.Name,
                                ADCD = w.ADCD,
                                ctype = w.ctype
                            };
                            transplist.Add(akv);
                        }
                    });
                    //县责任人
                    var countyperson = db.Select<AppKeyViewModel>("select distinct(Ltrim(rtrim(UserName))) as Name,ADCD, ctype=3 from CountryPerson where Year=" + _year + " and UserName like '%" + request.name.Trim() + "%'");
                    townpreson.AddRange(workingperson);
                    townpreson.AddRange(gridperson);
                    townpreson.AddRange(countyperson);
                    townpreson.AddRange(transplist);
                    _list.AddRange(townpreson);
                    #endregion
                }
                else if (RowID == 2)
                {
                    //市
                    var city = db.Select<AppKeyViewModel>("select adcd as ADCD,adnm as Name from ADCDInfo where adcd like '" + adcd.Substring(0, 4) + "%' and (CONVERT(int,SUBSTRING(adcd,7,3)) > 0 or CONVERT(int,SUBSTRING(adcd,10,3)) > 0) and  adnm like '%" + request.name.Trim() + "%'");
                    city.ForEach(w =>
                    {
                        m = new AppKeyViewModel();
                        m.ADCD = w.ADCD;
                        m.Name = w.Name;
                        _list.Add(m);
                    });
                    #region
                    //镇责任人
                    var townpreson = db.Select<AppKeyViewModel>("select distinct(Ltrim(rtrim(Name))) as Name,ADCD, ctype=1 from TownPersonLiable where Year=" + _year + " and adcd like '" + adcd.Substring(0, 4) + "%' and (CONVERT(int,SUBSTRING(adcd,7,3)) > 0 or CONVERT(int,SUBSTRING(adcd,10,3)) > 0) and Name like '%" + request.name.Trim() + "%'");
                    //村责任人
                    var workingperson = db.Select<AppKeyViewModel>("select distinct(PersonLiable) as Name,VillageADCD as ADCD, ctype=2 from VillageWorkingGroup where Year=" + _year + " and VillageADCD like '" + adcd.Substring(0, 4) + "%' and (CONVERT(int,SUBSTRING(VillageADCD,7,3)) > 0 or CONVERT(int,SUBSTRING(VillageADCD,10,3)) > 0) and PersonLiable like '%" + request.name.Trim() + "%'");
                    var gridperson = db.Select<AppKeyViewModel>("select distinct(PersonLiable) as Name,VillageADCD as ADCD, ctype=2 from VillageGridPersonLiable where  Year=" + _year + " and VillageADCD like '" + adcd.Substring(0, 4) + "%' and (CONVERT(int,SUBSTRING(VillageADCD,7,3)) > 0 or CONVERT(int,SUBSTRING(VillageADCD,10,3)) > 0) and PersonLiable like '%" + request.name.Trim() + "%'");
                    var transperson = db.Select<AppKeyViewModel>("select distinct(Ltrim(rtrim(PersonLiableName))) AS Name,ADCD, ctype=2 from VillageTransferPerson where Year=" + _year + " and adcd like '" + adcd.Substring(0, 4) + "%' and (CONVERT(int,SUBSTRING(adcd,7,3)) > 0 or CONVERT(int,SUBSTRING(adcd,10,3)) > 0) and PersonLiableName != '' and PersonLiableName like '%" + request.name.Trim() + "%'");
                    var transperson1 = db.Select<AppKeyViewModel>("select distinct(Ltrim(rtrim(WarnPersonLiableName))) AS Name,ADCD, ctype=2 from VillageTransferPerson where Year=" + _year + " and adcd like '" + adcd.Substring(0, 4) + "%' and (CONVERT(int,SUBSTRING(adcd,7,3)) > 0 or CONVERT(int,SUBSTRING(adcd,10,3)) > 0) and WarnPersonLiableName != '' and WarnPersonLiableName like '%" + request.name.Trim() + "%'");
                    var transperson2 = db.Select<AppKeyViewModel>("select distinct(Ltrim(rtrim(DisasterPreventionManager))) AS Name,ADCD, ctype=2 from VillageTransferPerson where Year=" + _year + " and adcd like '" + adcd.Substring(0, 4) + "%' and (CONVERT(int,SUBSTRING(adcd,7,3)) > 0 or CONVERT(int,SUBSTRING(adcd,10,3)) > 0) and DisasterPreventionManager != '' and DisasterPreventionManager like '%" + request.name.Trim() + "%' ");
                    transperson.AddRange(transperson1);
                    transperson.AddRange(transperson2);
                    List<AppKeyViewModel> transplist = new List<AppKeyViewModel>();
                    transperson.ForEach(w =>
                    {
                        var f = transplist.Where(x => x.ADCD == w.ADCD && x.Name == w.Name).ToList();
                        if (f == null)
                        {
                            AppKeyViewModel akv = new AppKeyViewModel()
                            {
                                Name = w.Name,
                                ADCD = w.ADCD,
                                ctype = w.ctype
                            };
                            transplist.Add(akv);
                        }
                    });
                    //县责任人
                    var countyperson = db.Select<AppKeyViewModel>("select distinct(Ltrim(rtrim(UserName))) as Name,ADCD, ctype=3 from CountryPerson where Year=" + _year + " and UserName like '%" + request.name.Trim() + "%'");
                    townpreson.AddRange(workingperson);
                    townpreson.AddRange(gridperson);
                    townpreson.AddRange(countyperson);
                    townpreson.AddRange(transplist);
                    _list.AddRange(townpreson);
                    #endregion
                }
                else if (RowID == 3)
                {
                    //县
                    var county = db.Select<AppKeyViewModel>("select adcd as ADCD,adnm as Name from ADCDInfo where Year=" + _year + " and adcd like '" + adcd.Substring(0, 6) + "%' and (CONVERT(int,SUBSTRING(adcd,7,3)) > 0 or CONVERT(int,SUBSTRING(adcd,10,3)) > 0) and  adnm like '%" + request.name.Trim() + "%'");
                    county.ForEach(w =>
                    {
                        m = new AppKeyViewModel();
                        m.ADCD = w.ADCD;
                        m.Name = w.Name;
                        _list.Add(m);
                    });
                }
                else
                {

                }
                #endregion
                return _list;
            }
        }
        /// <summary>
        /// 返回关键字全站查询结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<AppKeyInfoViewModel> GetKeysInfo(GetKeysInfo request)
        {
            if (request.name == null) throw new Exception("关键字无效！");
            var _year = request.year == null ? DateTime.Now.Year : request.year;

            List<AppKeyInfoViewModel> _list = new List<AppKeyInfoViewModel>();
            using (var db = DbFactory.Open())
            {
                var adcdlist = db.Select<ADCDInfo>();
                if (!string.IsNullOrEmpty(request.adcd) && request.ctype != null)
                {
                    #region 标准数据  0镇4村1镇责任人2村责任人3县责任人
                    switch (request.ctype)
                    {
                        //乡镇
                        case 0:
                            _list = GetTown(adcdlist, request.adcd, request.name);
                            break;
                        //镇责任人
                        case 1:
                            _list = GetTownList(adcdlist, request.adcd, request.name, _year);
                            break;
                        //村责任人
                        case 2:
                            _list = CCKHVillage(adcdlist, request.adcd, request.name, _year);
                            break;
                        //县责任人
                        case 3:
                            _list = GetCountryPersonList1(adcdlist, request.adcd, request.name, _year);
                            break;
                        //行政村
                        case 4:
                            _list = GetVillage(adcdlist, request.adcd, request.name);
                            break;
                    }
                    #endregion
                }
                else
                {
                    //乡镇
                    _list.AddRange(GetTown(adcdlist, "", request.name));
                    //行政村
                    _list.AddRange(GetVillage(adcdlist, "", request.name));
                    //镇责任人
                    _list.AddRange(GetTownList(adcdlist, "", request.name, _year));
                    //村责任人
                    _list.AddRange(CCKHVillage(adcdlist, "", request.name, _year));
                    //县责任人
                    _list.AddRange(GetCountryPersonList1(adcdlist, "", request.name, _year));
                }
                return _list;
            }
        }
        /// <summary>
        /// app端数据聚合统计
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<AppStaticsViewModel> GetAppStatics(GetAppStatics request)
        {
            List<AppStaticsViewModel> _list = new List<AppStaticsViewModel>();
            using (var db = DbFactory.Open())
            {
                var _pid = 0;
                switch (request.scale)
                {
                    case "county":
                        _pid = 3;
                        break;
                    case "town":
                        _pid = 4;
                        break;
                }

                var builder = db.From<AppLoginVCode>();
                builder.LeftJoin<AppLoginVCode, AppRecord>((x, y) => x.token == y.token);
                builder.Where<AppRecord>(w => w.adcd != "");
                builder.Select("distinct AppLoginVCode.userName,AppLoginVCode.token,AppRecord.adcd");
                var r = db.Select<ADCDCountViewModel>(builder);
                var townlist = db.Select<ADCDInfo>(w => w.grade == _pid && w.adcd.StartsWith(request.adcd.Substring(0, 6)));
                AppStaticsViewModel ssvm = null;
                townlist.ForEach(w =>
                {
                    ssvm = new AppStaticsViewModel();
                    try
                    {
                        if (_pid == 3) { ssvm.count = r.Where(x => x.adcd.Contains(w.adcd.Substring(0, 9))).Count(); }
                        else { ssvm.count = r.Where(x => x.adcd == w.adcd && w.grade == _pid).Count(); }
                    }
                    catch (Exception ex) { ssvm.count = 0; }
                    ssvm.adcd = w.adcd;
                    ssvm.latitude = w.lat;
                    ssvm.name = w.adnm;
                    ssvm.longitude = w.lng;
                    _list.Add(ssvm);
                });
                return _list;
            }
        }

        public List<AppAreaViewModel> GetAppArea(GetAppArea request)
        {
            List<AppAreaViewModel> _list = null;
            using (var db = DbFactory.Open())
            {
                //所有责任人
                var allperson = db.Select<AllPersonListViewModel>(" select distinct PersonLiable name, VillageADCD adcd from VillageWorkingGroup where Year = 2017"
 + " union select distinct PersonLiable name, VillageADCD adcd from VillageGridPersonLiable where Year = 2017 "
 + " union select distinct PersonLiableName name, adcd from VillageTransferPerson where PersonLiableName != '' and PersonLiableName <> null and Year = 2017"
 + " union select distinct WarnPersonLiableName name, adcd from VillageTransferPerson where WarnPersonLiableName != '' and WarnPersonLiableName is not null and Year = 2017"
 + " union select distinct DisasterPreventionManager name, adcd from VillageTransferPerson where DisasterPreventionManager <> '' and DisasterPreventionManager <> null and Year = 2017");
                //到岗位人数
                var dgperson = db.Select<AllPersonListViewModel>("select distinct adcd from AppRecord");
                //市
                _list = new List<AppAreaViewModel>();
                var builder = db.From<ADCDInfo>();
                builder.Select("*");
                builder.Where(w => w.grade == request.grade && w.parentId == request.parentid);
                var list = db.Select<AppAreaViewModel>(builder);
                //县
                var builderson = db.From<ADCDInfo>();
                builderson.Select("*");
                builderson.Where(w => w.grade == 2);
                var listson = db.Select<AppAreaViewModelSon>(builderson);
                AppAreaViewModel _model = null;
                list.ForEach(w =>
                {
                    _model = new AppAreaViewModel();
                    _model.id = w.id;
                    _model.parentId = w.parentId;
                    _model.grade = w.grade;
                    _model.adcd = w.adcd;
                    _model.adnm = w.adnm;
                    _model.lng = w.lng;
                    _model.lat = w.lat;
                    _model.inperson = allperson.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 4))).Count();
                    _model.noperson = dgperson.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 4))).Count();
                    //_model.SonList = listson.Where(x=>x.parentId == w.id).ToList();
                    var sonlist = listson.Where(x => x.parentId == w.id).ToList();
                    List<AppAreaViewModelSon> _sonlist = new List<AppAreaViewModelSon>();
                    sonlist.ForEach(x =>
                    {
                        AppAreaViewModelSon ason = new AppAreaViewModelSon();
                        ason.id = x.id;
                        ason.parentId = x.parentId;
                        ason.grade = x.grade;
                        ason.adcd = x.adcd;
                        ason.adnm = x.adnm;
                        ason.lng = x.lng;
                        ason.lat = x.lat;
                        ason.inperson = allperson.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6))).Count();
                        ason.noperson = dgperson.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6))).Count();
                        _sonlist.Add(ason);
                    });
                    _model.SonList = _sonlist;
                    _list.Add(_model);
                });
                return _list;
            }
        }
        public List<StatiscPerson> CCKHVillageApp(CCKHVillageApp request)
        {
            var listgroup1 = Iworkgroup.GetGroup1(new GetGroup1() { fid = request.fid, name = request.name, adcd = request.adcd, year = request.year, PageSize = 99999 }).rows;
            var grid = Igrid.GetVillageGrid1(new GetVillageGrid1() { fid = request.fid, name = request.name, adcd = request.adcd, year = request.year, PageSize = 99999 }).rows;
            var transfer = Itransfer.GetVillageTransferPerson2(new GetVillageTransferPerson2() { fid = request.fid, name = request.name, adcd = request.adcd, year = request.year, PageSize = 99999 }).rows;
            listgroup1.AddRange(grid);
            listgroup1.AddRange(transfer);
            #region
            var listgroup = new List<StatiscPerson>();
            listgroup = listgroup1;
            #endregion
            List<StatiscPerson> lsp = new List<StatiscPerson>();
            var adnmparents = "";
            using (var db = DbFactory.Open())
            {
                var _city = db.Single<ADCDInfo>(w => w.adcd == request.adcd.Substring(0, 4) + "00000000000");
                var _county = db.Single<ADCDInfo>(w => w.adcd == request.adcd.Substring(0, 6) + "000000000");
                var _town = db.Single<ADCDInfo>(w => w.adcd == request.adcd.Substring(0, 9) + "000000");
                adnmparents = _city.adnm + "_" + _county.adnm + "_" + _town.adnm;
            }
            var newlist = listgroup.Select(w => w.personLiable).Distinct().ToList();
            newlist.Remove("");
            //履职情况
            List<ADCDCountViewModel> listappInfo = null; List<ADCDCountViewModel> listappInfoPerson = null;
            using (var db = DbFactory.Open())
            {
                var builder = db.From<AppLoginVCode>();
                builder.LeftJoin<AppLoginVCode, AppRecord>((x, y) => x.token == y.token);
                builder.Where<AppRecord>(w => w.adcd == request.adcd);
                builder.OrderByDescending<AppRecord>(o=>o.addtime);
                builder.Select("distinct AppLoginVCode.userName,AppLoginVCode.token,AppRecord.adcd,AppRecord.addtime");
                listappInfo = db.Select<ADCDCountViewModel>(builder);
            }
            //职务统计
            var apppostlist = Enum.GetNames(typeof(Model.Enums.GrassrootsFloodCtrlEnums.AppPost)).Cast<string>().ToList();
            //
            newlist.ForEach(w =>
            {
                var f = listgroup.Where(x => x.personLiable == w).ToList();

                var newpost = ""; var phone = ""; var checkresult = ""; listappInfoPerson = new List<ADCDCountViewModel>();
                f.ForEach(y =>
                {
                    newpost += y.post + ";";
                    var p = y.post.Split('_')[0];
                    phone += y.handPhone + ';';
                });

                var fphones = phone.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                var fnewpost = newpost.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                try
                {//
                    listappInfoPerson = listappInfo.Where(x => Sql.In(x.userName, string.Join(";", fphones)) && x.adcd == f.FirstOrDefault().adcd).ToList();
                    checkresult = listappInfoPerson.Count() > 0 ? "1" : "0";
                }
                catch (Exception ex)
                {
                    checkresult = "0";
                }
                if (checkresult != "0")
                {
                    StatiscPerson tplvm = new StatiscPerson()
                    {
                        personLiable = w,
                        post = string.Join(";", fnewpost),
                        handPhone = string.Join(";", fphones),
                        id = f.FirstOrDefault().id,
                        adcd = f.FirstOrDefault().adcd,
                        adnmparent = adnmparents,
                        checkresult = checkresult,
                        addtime= listappInfoPerson.FirstOrDefault().addtime
                    };

                    var plist = tplvm.post.Split(';').ToList();
                    var listapp = new List<string>();
                    plist.ForEach(x =>
                    {
                        var plistitem = x.Split('_')[0];
                        if (apppostlist.Contains(plistitem) && !listapp.Contains(plistitem)) { listapp.Add(plistitem); }
                        else if(plistitem == "监测员" && !listapp.Contains(Model.Enums.GrassrootsFloodCtrlEnums.AppPost.管理员.ToString())) { listapp.Add(Model.Enums.GrassrootsFloodCtrlEnums.AppPost.管理员.ToString()); }
                        else if ((plistitem == "预警员") && !listapp.Contains(Model.Enums.GrassrootsFloodCtrlEnums.AppPost.监测预警组.ToString())) { listapp.Add(Model.Enums.GrassrootsFloodCtrlEnums.AppPost.监测预警组.ToString()); }
                        else { }
                    });
                    tplvm.apppost = string.Join(",", listapp);
                    tplvm.apppostcount = listapp.Count();

                    lsp.Add(tplvm);
                }
            });
            return lsp;
        }


        /// <summary>
        /// 县概括
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<CountyInfoAppViewModel> GetCountyInfo(GetCountyInfo request)
        {
            if (string.IsNullOrEmpty(request.adcd) || !request.adcd.Contains("000000")) throw new Exception("adcd无效！");
            var _year = request.year == null ? DateTime.Now.Year : request.year;
            using (var db = DbFactory.Open())
            {
                List<CountyInfoAppViewModel> list = new List<CountyInfoAppViewModel>();
                CountyInfoAppViewModel _model = new CountyInfoAppViewModel();
                //镇信息
                var townlist = _IZZTXManager.GetADCDInfo(new GetADCDInfo() { adcd = request.adcd, levle = 4, PageSize = 9999 });
                _model.TownCount = townlist != null ? Convert.ToInt32(townlist.total) : 0;
                _model.Towns = townlist != null ? townlist.rows : null;
                //转移清单
                var transferlist = db.Select<VillageTransferPerson>(w => w.adcd.Contains(request.adcd.Substring(0, 6)) && w.Year == _year && w.IfTransfer == null);
                _model.ZDPoint = transferlist != null ? transferlist.Count() : 0;
                _model.ZDManNums = transferlist != null && transferlist.Count() > 0 ? transferlist.Sum(w => w.HouseholderNum) : 0;
                //县本级
                var Countypersons = db.Select<CountryPerson>(w => w.adcd == request.adcd && w.Year == _year);
                _model.CountyBenJi = Countypersons != null && Countypersons.Count() > 0 ? Countypersons.Select(w => w.UserName).Distinct().Count() : 0;
                //村本级
                //var villagegridperson = db.Select<>(w => w.PersonLiable != "" && w.VillageADCD.Contains(request.adcd.Substring(0, 9))).Select(w => w.PersonLiable).Distinct();
                //var villagegroupperson = db.Select<VillageWorkingGroup>(w => w.PersonLiable != "" && w.VillageADCD.Contains(request.adcd.Substring(0, 9))).Select(w => w.PersonLiable).Distinct();
                //var villagepersons1 = db.Select<VillageTransferPerson>(w => w.PersonLiableName != "" && w.adcd.Contains(request.adcd.Substring(0, 9))).Select(w => w.PersonLiableName).Distinct();
                //var villagepersons2 = db.Select<VillageTransferPerson>(w => w.DisasterPreventionManager != "" && w.adcd.Contains(request.adcd.Substring(0, 9))).Select(w => w.DisasterPreventionManager).Distinct();
                //var villagepersons3 = db.Select<VillageTransferPerson>(w => w.WarnPersonLiableName != "" && w.adcd.Contains(request.adcd.Substring(0, 9))).Select(w => w.WarnPersonLiableName).Distinct();
                //var villagepersons0 = villagepersons1.Union(villagepersons2).Union(villagepersons3).Union(villagegridperson).Union(villagegroupperson);
//                var villagepersons0 = db.Select<VillageGridPersonLiable>("select t.name as name from("
//                                                                         + " select distinct PersonLiable as name from VillageWorkingGroup where Year=" + _year + " and PersonLiable != '' and VillageADCD like '" + request.adcd.Substring(0, 9) + "%'"
//                                                                         + " union select distinct PersonLiable as name from VillageGridPersonLiable where Year=" + _year + " and PersonLiable != '' and VillageADCD like '" + request.adcd.Substring(0, 9) + "%'"
//                                                                         + " union select distinct PersonLiableName as name from VillageTransferPerson where Year=" + _year + " and PersonLiableName != '' and adcd like '" + request.adcd.Substring(0, 9) + "%'"
//                                                                         + " union select distinct WarnPersonLiableName as name from VillageTransferPerson where Year=" + _year + " and WarnPersonLiableName != '' and adcd like '" + request.adcd.Substring(0, 9) + "%'"
//                                                                         + " union select distinct DisasterPreventionManager as name from VillageTransferPerson where Year=" + _year + " and DisasterPreventionManager != '' and adcd like '" + request.adcd.Substring(0, 9) + "%') as t").Count();



                var villagepersons0 = db.Select<VillageGridPersonLiable>("SELECT adcd FROM dbo.TownPersonLiable WHERE adcd IN (SELECT adcd FROM adcdinfo WHERE parentId=(SELECT Id FROM dbo.ADCDInfo WHERE adcd="+request.adcd+") AND grade=3 )").Count();
                _model.ZhenBenji = villagepersons0;

                //水利工程
                List<GridModel> listg = new List<GridModel>();
                GridModel gm;
                if (transferlist != null && transferlist.Count() > 0)
                {
                    gm = new GridModel();
                    gm.wanggeName = "危房";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "危房").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "山洪灾害危险区";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "山洪灾害危险区").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "地质灾害点";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "地质灾害点").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "低洼易涝区";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "低洼易涝区").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "屋顶山塘";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "屋顶山塘").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "工棚";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "工棚").Count();
                    listg.Add(gm);
                    gm = new GridModel();
                    gm.wanggeName = "堤防险段";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "堤防险段").Count();
                    listg.Add(gm);

                    gm = new GridModel();
                    gm.wanggeName = "海塘险段";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "堤防险段").Count();
                    listg.Add(gm);

                    gm = new GridModel();
                    gm.wanggeName = "其它";
                    gm.wanggeCount = transferlist.Where(w => w.DangerZoneType == "其它").Count();
                    listg.Add(gm);
                }
                _model.rows = listg;
                //County名 所属 city
                var areas = db.Select<ADCDInfo>(w => w.adcd.Contains(request.adcd.Substring(0, 4)) && w.adcd.Contains("000000"));
                var CountyInfo = areas.Single(w => w.adcd == request.adcd);
                _model.CountyName = CountyInfo.adnm;
                _model.LGT = CountyInfo.lat == null ? 0 : CountyInfo.lat.Value;
                _model.LTT = CountyInfo.lng == null ? 0 : CountyInfo.lng.Value;
                _model.CityName = areas.Single(w => w.adcd == request.adcd.Substring(0, 6) + "000000000").adnm;
                list.Add(_model);
                return list;
            }
        }
        #endregion

        #region 公共方法
        //乡镇
        public List<AppKeyInfoViewModel> GetTown(List<ADCDInfo> adcdlist, string _adcd, string _name)
        {
            List<AppKeyInfoViewModel> _list = new List<AppKeyInfoViewModel>();
            using (var db = DbFactory.Open())
            {
                List<ADCDInfo> townlist = null;
                ADCDInfo townlist1 = null;
                switch (RowID)
                {
                    case 5:
                        //省
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            townlist = db.Select<ADCDInfo>("CONVERT(int,SUBSTRING(adcd,0,6)) > 0 and adcd like '%000000' and adcd not like '%000000000' and adnm like '%" + _name.Trim() + "%'");
                        }
                        else
                        {
                            townlist1 = adcdlist.Single(w => w.adcd == _adcd.Substring(0, 6) + "000000000");
                        }
                        break;
                    case 2:
                        //市
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            townlist = db.Select<ADCDInfo>(w => w.adcd.StartsWith(adcd.Substring(0, 4)) && w.adnm.Contains(_name));
                        }
                        else
                        {
                            townlist1 = adcdlist.Single(w => w.adcd == _adcd.Substring(0, 6) + "000000000");
                        }
                        break;
                    case 3:
                        //县
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            townlist = db.Select<ADCDInfo>(w => w.adcd.StartsWith(adcd.Substring(0, 6)) && w.adnm.Contains(_name));
                        }
                        else
                        {
                            townlist1 = adcdlist.Single(w => w.adcd == _adcd.Substring(0, 6) + "000000000");
                        }
                        break;
                }
                if (townlist != null)
                {
                    townlist.ForEach(w =>
                    {
                        AppKeyInfoViewModel _model = new AppKeyInfoViewModel();
                        _model.ADCD = w.adcd;
                        _model.Name = w.adnm;
                        _model.TypeName = "镇";
                        _model.Types = 0;
                        _model.Areas = adcdlist.Single(x => x.adcd == w.adcd.Substring(0, 6) + "000000000").adnm;
                        _list.Add(_model);
                    });
                }
                else
                {
                    AppKeyInfoViewModel _model = new AppKeyInfoViewModel();
                    _model.ADCD = _adcd;
                    _model.Name = _name;
                    _model.TypeName = "镇";
                    _model.Types = 0;
                    _model.Areas = townlist1 != null ? townlist1.adnm : "";
                    _list.Add(_model);
                }
            }
            return _list;
        }
        //镇责任人
        public List<AppKeyInfoViewModel> GetTownList(List<ADCDInfo> adcdlist, string _adcd, string _name, int? _year)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<TownPersonLiable>();
                switch (RowID)
                {
                    case 5:
                        //省
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            builder.Where(w => w.Name.Contains(_name) && w.Year == _year);
                        }
                        else
                        {
                            builder.And(x => x.adcd == _adcd && x.Year == _year && x.Name.Contains(_name));
                        }
                        break;
                    case 2:
                        //市
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            builder.Where(w => w.Name.Contains(_name) && w.adcd.StartsWith(adcd.Substring(0, 4)) && w.Year == _year);
                        }
                        else
                        {
                            builder.And(x => x.adcd == _adcd && x.Year == _year && x.Name.Contains(_name));
                        }
                        break;
                    case 3:
                        //县
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            builder.Where(w => w.Name.Contains(_name) && w.adcd.StartsWith(adcd.Substring(0, 6)) && w.Year == _year);
                        }
                        else
                        {
                            builder.And(x => x.adcd == _adcd && x.Year == _year && x.Name.Contains(_name));
                        }
                        break;
                }

                var list = db.Select<TownPersonLiableViewModel>(builder);
                var newlist = list.Select(w => w.Name).Distinct().ToList();
                List<AppKeyInfoViewModel> _list = new List<AppKeyInfoViewModel>();
                newlist.ForEach(w =>
                {
                    var f = list.Where(x => x.Name == w).ToList();
                    var newpost = ""; var phone = "";
                    f.ForEach(y =>
                    {
                        newpost += y.Position + "_" + y.Post + ";";
                        phone += y.Mobile + ';';
                    });
                    var fphones = phone.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    ADCDInfo shi = new ADCDInfo();
                    ADCDInfo town = new ADCDInfo();
                    if (string.IsNullOrEmpty(_adcd))
                    {
                        shi = adcdlist.Single(x => x.adcd == f.FirstOrDefault().adcd.Substring(0, 6) + "000000000");
                        town = adcdlist.Single(x => x.adcd == f.FirstOrDefault().adcd);
                    }
                    else
                    {
                        shi = adcdlist.Single(x => x.adcd == _adcd.Substring(0, 6) + "000000000");
                        town = adcdlist.Single(x => x.adcd == _adcd);
                    }

                    AppKeyInfoViewModel tplvm = new AppKeyInfoViewModel()
                    {
                        ADCD = f.FirstOrDefault().adcd,
                        Name = w,
                        TypeName = "人",
                        Types = 1,
                        Areas = shi.adnm + " " + town.adnm,
                        ZhiWuName = newpost.TrimEnd(';'),
                        Mobile = string.Join(";", fphones)
                    };
                    _list.Add(tplvm);
                });
                return _list;
            }
        }
        #region 村责任人
        public List<AppKeyInfoViewModel> CCKHVillage(List<ADCDInfo> adcdlist, string _adcd, string _name, int? _year)
        {
            var listgroup = GetGroup1(_name, _adcd, _year);
            var grid = GetVillageGrid1(_name, _adcd, _year);
            var transfer = GetVillageTransferPerson3(_name, _adcd, _year);
            listgroup.AddRange(grid);
            listgroup.AddRange(transfer);
            List<AppKeyInfoViewModel> _list = new List<AppKeyInfoViewModel>();
            var newlist = listgroup.Select(w => w.personLiable).Distinct().ToList();
            newlist.Remove("");
            using (var db = DbFactory.Open())
            {
                //var fcounty = db.Select<ADCDInfo>(w => w.adcd.Contains(_adcd.Substring(0, 4)) && w.adcd.EndsWith("000"));
                newlist.ForEach(w =>
                {
                    var f = listgroup.Where(x => x.personLiable == w).ToList();
                    var newpost = ""; var phone = ""; var county = ""; var town = ""; var cun = "";
                    f.ForEach(y =>
                    {
                        newpost += y.post + ";";
                        phone += y.handPhone + ';';
                    });
                    var fphones = phone.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    var fnewpost = newpost.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    if (string.IsNullOrEmpty(_adcd))
                    {
                        county = adcdlist.Single(x => x.adcd == f.FirstOrDefault().adcd.Substring(0, 6) + "000000000").adnm;
                        town = adcdlist.Single(x => x.adcd == f.FirstOrDefault().adcd.Substring(0, 9) + "000000").adnm;
                        cun = adcdlist.Single(x => x.adcd == f.FirstOrDefault().adcd).adnm;
                    }
                    else
                    {
                        county = adcdlist.Single(x => x.adcd == _adcd.Substring(0, 6) + "000000000").adnm;
                        town = adcdlist.Single(x => x.adcd == _adcd.Substring(0, 9) + "000000").adnm;
                        cun = adcdlist.Single(x => x.adcd == _adcd).adnm;
                    }
                    AppKeyInfoViewModel tplvm = new AppKeyInfoViewModel()
                    {
                        ADCD = _adcd,
                        Name = w,
                        TypeName = "人",
                        Types = 2,
                        Areas = county + " " + town + " " + cun,
                        ZhiWuName = string.Join(";", fnewpost),
                        Mobile = string.Join(";", fphones)
                    };
                    _list.Add(tplvm);
                });
            }
            return _list;
        }

        public List<StatiscPerson> GetGroup1(string _name, string _adcd, int? _year)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<VillageWorkingGroup>();
                switch (RowID)
                {
                    case 5:
                        //省
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            builder.Where(w => w.PersonLiable.Contains(_name) && w.PersonLiable != "" && w.PersonLiable != null && w.Year == _year);
                        }
                        else
                        {
                            builder.Where<VillageWorkingGroup>(x => x.Year == _year && x.VillageADCD == _adcd && x.PersonLiable != "" && x.PersonLiable != null && x.PersonLiable.Contains(_name));
                        }
                        break;
                    case 2:
                        //市
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            builder.Where(w => w.PersonLiable.Contains(_name) && w.PersonLiable != "" && w.PersonLiable != null && w.VillageADCD.StartsWith(adcd.Substring(0, 4)) && w.Year == _year);
                        }
                        else
                        {
                            builder.Where<VillageWorkingGroup>(x => x.Year == _year && x.VillageADCD == _adcd && x.PersonLiable != "" && x.PersonLiable != null && x.PersonLiable.Contains(_name));
                        }
                        break;
                    case 3:
                        //县
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            builder.Where(w => w.PersonLiable.Contains(_name) && w.PersonLiable != "" && w.PersonLiable != null && w.VillageADCD.StartsWith(adcd.Substring(0, 6)) && w.Year == _year);
                        }
                        else
                        {
                            builder.Where<VillageWorkingGroup>(x => x.Year == _year && x.VillageADCD == _adcd && x.PersonLiable != "" && x.PersonLiable != null && x.PersonLiable.Contains(_name));
                        }
                        break;
                }
                builder.Select("distinct PersonLiable,Post,Position,HandPhone,VillageADCD");
                var RList = db.Select<VillageWorkingGroupViewModel>(builder);
                var newlist = RList.Select(w => w.PersonLiable).Distinct().ToList();
                List<StatiscPerson> rlist = new List<StatiscPerson>();
                newlist.ForEach(w =>
                {
                    var userpost = ""; var phones = "";
                    var f = RList.Where(y => y.PersonLiable == w).ToList();
                    f.ForEach(x =>
                    {
                        userpost += x.Post + "_" + x.Position + ";";
                        phones += x.HandPhone + ';';
                    });
                    var rphones = phones.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    StatiscPerson vwgv = new StatiscPerson()
                    {
                        personLiable = w,
                        post = userpost.TrimEnd(';'),
                        handPhone = string.Join(";", rphones),
                        id = f.FirstOrDefault().ID,
                        adcd = f.FirstOrDefault().VillageADCD
                    };
                    rlist.Add(vwgv);
                });
                return rlist;
            }
        }

        public List<StatiscPerson> GetVillageGrid1(string _name, string _adcd, int? _year)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<VillageGridPersonLiable>();
                switch (RowID)
                {
                    case 5:
                        //省
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            builder.Where(w => w.PersonLiable.Contains(_name) && w.PersonLiable != "" && w.PersonLiable != null && w.Year == _year);
                        }
                        else
                        {
                            builder.Where<VillageGridPersonLiable>(x => x.Year == _year && x.VillageADCD == _adcd && x.PersonLiable != "" && x.PersonLiable != null && x.PersonLiable.Contains(_name));
                        }
                        break;
                    case 2:
                        //市
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            builder.Where(w => w.PersonLiable.Contains(_name) && w.PersonLiable != "" && w.PersonLiable != null && w.VillageADCD.StartsWith(adcd.Substring(0, 4)) && w.Year == _year);
                        }
                        else
                        {
                            builder.Where<VillageGridPersonLiable>(x => x.Year == _year && x.VillageADCD == _adcd && x.PersonLiable != "" && x.PersonLiable != null && x.PersonLiable.Contains(_name));
                        }
                        break;
                    case 3:
                        //县
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            builder.Where(w => w.PersonLiable.Contains(_name) && w.PersonLiable != "" && w.PersonLiable != null && w.VillageADCD.StartsWith(adcd.Substring(0, 6)) && w.Year == _year);
                        }
                        else
                        {
                            builder.Where<VillageGridPersonLiable>(x => x.Year == _year && x.VillageADCD == _adcd && x.PersonLiable != "" && x.PersonLiable != null && x.PersonLiable.Contains(_name));
                        }
                        break;
                }

                var RList = db.Select<VillageGridViewModel>(builder);
                var newlist = RList.Select(w => w.PersonLiable).Distinct().ToList();
                List<StatiscPerson> rlist = new List<StatiscPerson>();
                newlist.ForEach(w =>
                {
                    var f = RList.Where(x => x.PersonLiable == w).ToList();
                    var newpost = ""; var phone = "";
                    f.ForEach(y =>
                    {
                        newpost += y.Position + ";";
                        phone += y.HandPhone + ';';
                    });
                    var fphones = phone.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    var fnewpost = newpost.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    StatiscPerson tplvm = new StatiscPerson()
                    {
                        personLiable = w,
                        post = string.Join(";", fnewpost),
                        handPhone = string.Join(";", fphones),
                        id = f.FirstOrDefault().ID,
                        adcd = f.FirstOrDefault().VillageADCD
                    };
                    rlist.Add(tplvm);
                });
                return rlist;
            }
        }
        public List<StatiscPerson> GetVillageTransferPerson2(string _name, string _adcd, int? _year)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<VillageTransferPerson>();
                //builder.LeftJoin<ADCDInfo, VillageTransferPerson>((x, y) => x.adcd == y.adcd);
                switch (RowID)
                {
                    case 5:
                        //省
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            builder.Where(y => y.Year == _year && y.PersonLiableName.Contains(_name) || y.WarnPersonLiableName.Contains(_name) || y.DisasterPreventionManager.Contains(_name));
                        }
                        else
                        {
                            builder.Where(y => y.adcd == _adcd && y.Year == _year && y.PersonLiableName.Contains(_name) || y.WarnPersonLiableName.Contains(_name) || y.DisasterPreventionManager.Contains(_name));
                        }
                        break;
                    case 2:
                        //市
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            builder.Where(w => w.PersonLiableName.Contains(_name) || w.WarnPersonLiableName.Contains(_name) || w.DisasterPreventionManager.Contains(_name) && w.adcd.StartsWith(adcd.Substring(0, 4)) && w.Year == _year);
                        }
                        else
                        {
                            builder.Where(y => y.adcd == _adcd && y.Year == _year && y.PersonLiableName.Contains(_name) || y.WarnPersonLiableName.Contains(_name) || y.DisasterPreventionManager.Contains(_name));
                        }
                        break;
                    case 3:
                        //县
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            builder.Where(w => w.PersonLiableName.Contains(_name) || w.WarnPersonLiableName.Contains(_name) || w.DisasterPreventionManager.Contains(_name) && w.adcd.StartsWith(adcd.Substring(0, 6)) && w.Year == _year);
                        }
                        else
                        {
                            builder.Where(y => y.adcd == _adcd && y.Year == _year && y.PersonLiableName.Contains(_name) || y.WarnPersonLiableName.Contains(_name) || y.DisasterPreventionManager.Contains(_name));
                        }
                        break;
                }

                // builder.Select("VillageTransferPerson.*,ADCDInfo.adnm");
                var list = db.Select<VillageTransferPersonViewModel>(builder);
                var n1 = list.Where(w => w.PersonLiableName != "" && w.PersonLiableName != null && w.PersonLiableName.Contains(_name));
                var newlist1 = n1.Select(w => w.PersonLiableName).Distinct().ToList();
                var n2 = list.Where(w => w.WarnPersonLiableName != "" && w.WarnPersonLiableName != null && w.WarnPersonLiableName.Contains(_name));
                var newlist2 = n2.Select(w => w.WarnPersonLiableName).Distinct().ToList();
                var n3 = list.Where(w => w.DisasterPreventionManager != "" && w.DisasterPreventionManager != null && w.DisasterPreventionManager.Contains(_name));
                var newlist3 = n3.Select(w => w.DisasterPreventionManager).Distinct().ToList();
                List<VillageTransferPersonViewModel> rlist = new List<VillageTransferPersonViewModel>();
                newlist1.ForEach(w =>
                {
                    var f = list.Where(x => x.PersonLiableName == w).ToList();
                    var newpost = ""; var phone = "";
                    f.ForEach(y =>
                    {
                        newpost += y.PersonLiablePost + ";";
                        phone += y.PersonLiableMobile + ';';
                    });
                    var fphones = phone.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    var fnewpost = newpost.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    VillageTransferPersonViewModel tplvm = new VillageTransferPersonViewModel()
                    {
                        PersonLiableName = w,
                        PersonLiablePost = string.Join(";", fnewpost),
                        PersonLiableMobile = string.Join(";", fphones),
                        adcd = f.FirstOrDefault().adcd
                    };
                    rlist.Add(tplvm);
                });
                newlist2.ForEach(w =>
                {
                    var f = list.Where(x => x.WarnPersonLiableName == w).ToList();
                    var newpost = ""; var phone = "";
                    f.ForEach(y =>
                    {
                        newpost += y.WarnPersonLiablePost + ";";
                        phone += y.WarnPersonLiableMobile + ';';
                    });
                    var fphones = phone.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    var fnewpost = newpost.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    VillageTransferPersonViewModel tplvm = new VillageTransferPersonViewModel()
                    {
                        PersonLiableName = w,
                        PersonLiablePost = string.Join(";", fnewpost),
                        PersonLiableMobile = string.Join(";", fphones),
                        adcd = f.FirstOrDefault().adcd
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
                        PersonLiablePost = "",
                        PersonLiableMobile = string.Join(";", fphones),
                        adcd = f.FirstOrDefault().adcd
                    };
                    rlist.Add(tplvm);
                });
                List<StatiscPerson> rlist1 = new List<StatiscPerson>();
                var newlist4 = rlist.Select(w => w.PersonLiableName).Distinct().ToList();
                //newlist4.Remove("");
                //newlist4.Remove(null);
                newlist4.ForEach(w =>
                {
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
                return rlist1;
            }
        }
        //GetVillageTransferPerson2 升级方法
        public List<StatiscPerson> GetVillageTransferPerson3(string _name, string _adcd, int? _year)
        {
            List<VillageTransferPersonViewModel> newlist = null;
            switch (RowID)
            {
                case 5:
                    //省
                    if (string.IsNullOrEmpty(_adcd))
                    {
                        newlist = getpersonList("", _name, _year);
                    }
                    else
                    {
                        newlist = getpersonList(" AND adcd=" + _adcd + "", _name, _year);
                    }
                    break;
                case 2:
                    //市
                    if (string.IsNullOrEmpty(_adcd))
                    {
                        newlist = getpersonList(" AND adcd LIKE '" + adcd.Substring(0, 4) + "%'", _name, _year);
                    }
                    else
                    {
                        newlist = getpersonList(" AND adcd=" + _adcd + "", _name, _year);
                    }
                    break;
                case 3:
                    //县
                    if (string.IsNullOrEmpty(_adcd))
                    {
                        newlist = getpersonList(" AND adcd LIKE '" + adcd.Substring(0, 6) + "%'", _name, _year);
                    }
                    else
                    {
                        newlist = getpersonList(" AND adcd=" + _adcd + "", _name, _year);
                    }
                    break;
            }
            List<StatiscPerson> rlist1 = new List<StatiscPerson>();
            var newlist4 = newlist.Select(w => w.PersonLiableName).Distinct().ToList();
            newlist4.ForEach(w =>
            {
                var f = newlist.Where(x => x.PersonLiableName == w).ToList();
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
            return rlist1;

        }
        public List<VillageTransferPersonViewModel> getpersonList(string where, string _name, int? _year)
        {
            List<VillageTransferPersonViewModel> newlist = null;
            using (var db = DbFactory.Open())
            {
                string sql = "select distinct t.PersonLiableName,t.PersonLiablePost,t.PersonLiableMobile,t.adcd from ("
               + " select distinct PersonLiableName,PersonLiablePost,PersonLiableMobile,adcd from VillageTransferPerson"
               + " where PersonLiableName != '' and PersonLiableName is not null and PersonLiableName like '%" + _name + "%' and Year=" + _year + " " + where + ""
                + " union select distinct WarnPersonLiableName as PersonLiableName,WarnPersonLiablePost as PersonLiablePost,WarnPersonLiableMobile as PersonLiableMobile,adcd from VillageTransferPerson"
                + " where WarnPersonLiableName != '' and WarnPersonLiableName is not null and WarnPersonLiableName like '%" + _name + "%' and Year=" + _year + " " + where + ""
                + " union select distinct DisasterPreventionManager as PersonLiableName,DisasterPreventionManagerMobile as PersonLiableMobile,PersonLiablePost = '',adcd from VillageTransferPerson"
                 + " where DisasterPreventionManager != '' and DisasterPreventionManager is not null and DisasterPreventionManager like '%" + _name + "%' and Year=" + _year + " " + where + ""
                + ") t";
                newlist = db.Select<VillageTransferPersonViewModel>(sql);
            }

            return newlist;
        }
        #endregion
        //县责任人
        public List<AppKeyInfoViewModel> GetCountryPersonList1(List<ADCDInfo> adcdlist, string _adcd, string _name, int? _year)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<CountryPerson>();
                //if (!string.IsNullOrEmpty(_adcd))
                //    bulider.Where(x => x.adcd == _adcd);
                //#region
                //if (!string.IsNullOrEmpty(_name))
                //    bulider.Where(w => w.UserName.Contains(_name));
                //#endregion
                //bulider.And(x => x.Year == _year);
                switch (RowID)
                {
                    case 5:
                        //省
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            builder.Where(w => w.UserName.Contains(_name) && w.Year == _year);
                        }
                        else
                        {
                            builder.And(x => x.adcd == _adcd && x.Year == _year && x.UserName.Contains(_name));
                        }
                        break;
                    case 2:
                        //市
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            builder.Where(w => w.UserName.Contains(_name) && w.adcd.StartsWith(adcd.Substring(0, 4)) && w.Year == _year);
                        }
                        else
                        {
                            builder.And(x => x.adcd == _adcd && x.Year == _year && x.UserName.Contains(_name));
                        }
                        break;
                    case 3:
                        //县
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            builder.Where(w => w.UserName.Contains(_name) && w.adcd.StartsWith(adcd.Substring(0, 6)) && w.Year == _year);
                        }
                        else
                        {
                            builder.And(x => x.adcd == _adcd && x.Year == _year && x.UserName.Contains(_name));
                        }
                        break;
                }
                var list = db.Select<CountryPersonServiceModel>(builder);
                var newlist = list.Select(w => w.UserName).Distinct().ToList();

                List<AppKeyInfoViewModel> _list = new List<AppKeyInfoViewModel>();
                newlist.ForEach(w =>
                {
                    var f = list.Where(x => x.UserName == w).ToList();
                    var newpost = ""; var phones = ""; var fshiname = ""; var fcountyname = "";
                    f.ForEach(y =>
                   {
                       newpost += y.Position + "_" + y.Post + ";";
                       phones += y.Phone + ";";
                   });
                    var rphones = phones.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    if (string.IsNullOrEmpty(_adcd))
                    {
                        fshiname = adcdlist.Single(x => x.adcd == f.FirstOrDefault().adcd.Substring(0, 4) + "00000000000").adnm;
                        fcountyname = adcdlist.Single(x => x.adcd == f.FirstOrDefault().adcd.Substring(0, 6) + "000000000").adnm;
                    }
                    else
                    {
                        fshiname = adcdlist.Single(x => x.adcd == _adcd.Substring(0, 4) + "00000000000").adnm;
                        fcountyname = adcdlist.Single(x => x.adcd == _adcd.Substring(0, 6) + "000000000").adnm;
                    }
                    AppKeyInfoViewModel cpsm = new AppKeyInfoViewModel()
                    {
                        ADCD = f.FirstOrDefault().adcd,
                        Name = w,
                        TypeName = "人",
                        Types = 3,
                        Areas = fshiname + " " + fcountyname,
                        ZhiWuName = newpost.TrimEnd(';'),
                        Mobile = string.Join(";", rphones)
                    };
                    _list.Add(cpsm);
                });
                return _list;
            }
        }
        //行政村
        public List<AppKeyInfoViewModel> GetVillage(List<ADCDInfo> adcdlist, string _adcd, string _name)
        {

            AppKeyInfoViewModel _model = null;
            List<AppKeyInfoViewModel> _list = new List<AppKeyInfoViewModel>();
            using (var db = DbFactory.Open())
            {
                List<ADCDInfo> fvshi = null;
                var countyname = ""; var townmname = "";
                switch (RowID)
                {
                    case 5:
                        //省
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            fvshi = db.Select<ADCDInfo>("CONVERT(int,SUBSTRING(adcd,10,3)) > 0 and adnm like '%" + _name.Trim() + "%'").ToList();
                            fvshi.ForEach(x =>
                            {
                                _model = new AppKeyInfoViewModel();
                                countyname = adcdlist.Single(w => w.adcd == x.adcd.Substring(0, 6) + "000000000").adnm;
                                townmname = adcdlist.Single(w => w.adcd == x.adcd.Substring(0, 9) + "000000").adnm;
                                _model.ADCD = x.adcd;
                                _model.Name = x.adnm;
                                _model.TypeName = "村";
                                _model.Types = 4;
                                _model.Areas = countyname + " " + townmname;
                                _list.Add(_model);
                            });
                        }
                        else
                        {
                            _model = new AppKeyInfoViewModel();
                            countyname = adcdlist.Single(w => w.adcd == _adcd.Substring(0, 6) + "000000000").adnm;
                            townmname = adcdlist.Single(w => w.adcd == _adcd.Substring(0, 9) + "000000").adnm;
                            _model.ADCD = _adcd;
                            _model.Name = _name;
                            _model.TypeName = "村";
                            _model.Types = 4;
                            _model.Areas = countyname + " " + townmname;
                            _list.Add(_model);
                        }
                        break;
                    case 2:
                        //市
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            fvshi = db.Select<ADCDInfo>("CONVERT(int,SUBSTRING(adcd,10,3)) > 0 and adcd like '" + adcd.Substring(0, 4) + "%' and adnm like '%" + _name.Trim() + "%'").ToList();
                            fvshi.ForEach(x =>
                            {
                                _model = new AppKeyInfoViewModel();
                                countyname = adcdlist.Single(w => w.adcd == x.adcd.Substring(0, 6) + "000000000").adnm;
                                townmname = adcdlist.Single(w => w.adcd == x.adcd.Substring(0, 9) + "000000").adnm;
                                _model.ADCD = x.adcd;
                                _model.Name = x.adnm;
                                _model.TypeName = "村";
                                _model.Types = 4;
                                _model.Areas = countyname + " " + townmname;
                                _list.Add(_model);
                            });
                        }
                        else
                        {
                            _model = new AppKeyInfoViewModel();
                            countyname = adcdlist.Single(w => w.adcd == _adcd.Substring(0, 6) + "000000000").adnm;
                            townmname = adcdlist.Single(w => w.adcd == _adcd.Substring(0, 9) + "000000").adnm;
                            _model.ADCD = _adcd;
                            _model.Name = _name;
                            _model.TypeName = "村";
                            _model.Types = 4;
                            _model.Areas = countyname + " " + townmname;
                            _list.Add(_model);
                        }
                        break;
                    case 3:
                        //县
                        if (string.IsNullOrEmpty(_adcd))
                        {
                            fvshi = db.Select<ADCDInfo>("CONVERT(int,SUBSTRING(adcd,10,3)) > 0 and adcd like '" + adcd.Substring(0, 6) + "%' and adnm like '%" + _name.Trim() + "%'").ToList();
                            fvshi.ForEach(x =>
                            {
                                _model = new AppKeyInfoViewModel();
                                countyname = adcdlist.Single(w => w.adcd == x.adcd.Substring(0, 6) + "000000000").adnm;
                                townmname = adcdlist.Single(w => w.adcd == x.adcd.Substring(0, 9) + "000000").adnm;
                                _model.ADCD = x.adcd;
                                _model.Name = x.adnm;
                                _model.TypeName = "村";
                                _model.Types = 4;
                                _model.Areas = countyname + " " + townmname;
                                _list.Add(_model);
                            });
                        }
                        else
                        {
                            _model = new AppKeyInfoViewModel();
                            countyname = adcdlist.Single(w => w.adcd == _adcd.Substring(0, 6) + "000000000").adnm;
                            townmname = adcdlist.Single(w => w.adcd == _adcd.Substring(0, 9) + "000000").adnm;
                            _model.ADCD = _adcd;
                            _model.Name = _name;
                            _model.TypeName = "村";
                            _model.Types = 4;
                            _model.Areas = countyname + " " + townmname;
                            _list.Add(_model);
                        }
                        break;
                }
            }
            return _list;
        }

        public List<AppRecordAndUserViewModel> GetPersonAppInfo(GetPersonAppInfo request)
        {
            List<AppRecordAndUserViewModel> _list = new List<AppRecordAndUserViewModel>();
            using (var db = DbFactory.Open())
            {
                var builder = db.From<AppRecord>();
                builder.LeftJoin<AppRecord, AppLoginVCode>((x, y) => x.username == y.userName);
                if(request.post == "监测预警组") builder.Where(w => w.adcd == request.adcd && w.postCode== "预警员");
                else if (request.post == "监测员") builder.Where(w => w.adcd == request.adcd && w.postCode == "管理员");
                else builder.Where(w => w.adcd == request.adcd && w.postCode == request.post);
                if(!string.IsNullOrEmpty(request.starttime.Trim()) && !string.IsNullOrEmpty(request.endtime.Trim()))
                {
                    builder.Where(w=>w.addtime >= Convert.ToDateTime(request.starttime.Trim()) && w.addtime <= Convert.ToDateTime(request.endtime.Trim()));
                }
                builder.Where<AppLoginVCode>(w => w.userName == request.mobile);
                builder.Select("AppRecord.*,AppLoginVCode.username").OrderByDescending(w => w.addtime);
                var list= db.Select<AppRecordAndUserViewModel>(builder);
                return list;
            }
        }
        #endregion

    }
}
