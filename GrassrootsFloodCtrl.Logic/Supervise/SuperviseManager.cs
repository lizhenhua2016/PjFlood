using Dy.Common;
using GrassrootsFloodCtrl.Logic.Common;
using GrassrootsFloodCtrl.Logic.Village;
using GrassrootsFloodCtrl.Logic.Village.VillageGrid;
using GrassrootsFloodCtrl.Logic.Village.WorkingGroup;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.Model.Sys;
using GrassrootsFloodCtrl.Model.Town;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.CApp;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;

namespace GrassrootsFloodCtrl.Logic.Supervise
{
    public class SuperviseManager : ManagerBase, ISuperviseManager
    {
        public ILogHelper _ILogHelper { get; set; }
        public IVillageWorkingGroupManage Iworkgroup { get; set; }
        public IVillageGridManage Igrid { get; set; }
        public IVillageTransferPersonManager Itransfer { get; set; }
        public SuperviseModel PersonLiable(SPersonLiable request)
        {
            SuperviseModel sm = new SuperviseModel();
            var _adcd = string.IsNullOrEmpty(request.adcd) ? adcd : request.adcd;
            var _year = request.year == null ? DateTime.Now.Year : request.year.Value;
            var _gridid = request.grid == null ? RowID : request.grid.Value;
            if (_gridid == 0) _gridid = (int)GrassrootsFloodCtrlEnums.RoleEnums.省级用户;
            if (string.IsNullOrEmpty(_adcd)) throw new Exception("行政编码为空！");
            try
            {
                using (var db = DbFactory.Open())
                {
                    List<NextLevelStatics> lsn = null; List<NextLevelStatics> lsc = null;
                    if (_gridid != 4)
                    {
                        #region 公用 取出所有镇的填报情况
                        var sql = "select Towns.adcd,Towns.adnm," +
                            "TownPersonLiable.adcd TownPersonLiableNums, VillageWorkingGroup.VillageADCD VillageWorkingGroupNums," +
                            "VillageGridPersonLiable.VillageADCD VillageGridPersonLiableNums, VillageTransferPerson.adcd VillageTransferPersonNums," +
                            "VillagePic.adcd VillagePicNums from " +
                            "(select * from ADCDInfo where CONVERT(int, SUBSTRING(adcd, 7, 3)) > 0 and  adcd like '%000000') as Towns " +
                            "left join(select distinct(adcd) as adcd from TownPersonLiable where Year=" + _year + ") as TownPersonLiable on SUBSTRING(TownPersonLiable.adcd, 0, 10) = SUBSTRING(Towns.adcd, 0, 10) " +
                            "left join(select distinct(SUBSTRING(VillageADCD, 0, 10)) as VillageADCD from VillageWorkingGroup where Year=" + _year + ") as VillageWorkingGroup on SUBSTRING(Towns.adcd, 0, 10) = VillageWorkingGroup.VillageADCD " +
                            "left join(select distinct(SUBSTRING(VillageADCD, 0, 10)) as VillageADCD from VillageGridPersonLiable where Year=" + _year + ") as VillageGridPersonLiable on SUBSTRING(VillageGridPersonLiable.VillageADCD, 0, 10) = SUBSTRING(Towns.adcd, 0, 10) " +
                            "left join(select distinct(SUBSTRING(adcd, 0, 10)) as adcd from VillageTransferPerson where Year=" + _year + ") as VillageTransferPerson on SUBSTRING(VillageTransferPerson.adcd, 0, 10) = SUBSTRING(Towns.adcd, 0, 10) " +
                            "left join(select distinct(SUBSTRING(adcd, 0, 10)) as adcd from VillagePic where Year=" + _year + ") as VillagePic on SUBSTRING(VillagePic.adcd, 0, 10) = SUBSTRING(Towns.adcd, 0, 10) ";
                        var allTown = db.Select<StatiscTowns>(sql);
                         lsn = new List<NextLevelStatics>();
                        allTown.ForEach(w =>
                        {
                            NextLevelStatics nsz = new NextLevelStatics();
                            nsz.ADCD = w.ADCD; nsz.ADNM = w.ADNM; nsz.Grade = 4;
                        //取填报数据
                        var _a = string.IsNullOrEmpty(w.TownPersonLiableNums);
                            var _b = string.IsNullOrEmpty(w.VillageWorkingGroupNums);
                            var _c = string.IsNullOrEmpty(w.VillageGridPersonLiableNums);
                            var _d = string.IsNullOrEmpty(w.VillageTransferPersonNums);
                            var _e = string.IsNullOrEmpty(w.VillagePicNums);
                            if (!_a || !_b || !_c || !_d || !_e)
                            {
                                nsz.HasReported = 1;
                                nsz.NoReported = 0;
                            }
                            else
                            {
                                nsz.HasReported = 0;
                                nsz.NoReported = 1;
                            }
                            lsn.Add(nsz);
                        });
                        #endregion
                        #region 公用 县级填报
                        var sqlc = "select a.adcd,a.adnm,c.adcd as Reported from (select adcd, adnm from ADCDInfo where CONVERT(int, SUBSTRING(adcd, 5, 2)) > 0 and  adcd like '%000000000') as A "
                            + "left join (select distinct(adcd) from CountryPerson where Year=" + _year + ") as C on A.adcd = C.adcd";
                        var allcounty = db.Select<StatiscCounty>(sqlc);
                        lsc = new List<NextLevelStatics>();
                        allcounty.ForEach(w =>
                        {
                            NextLevelStatics nsc = new NextLevelStatics();
                            nsc.ADCD = w.ADCD; nsc.ADNM = w.ADCDNM; nsc.Grade = 3;
                            //取填报数据
                            if (!string.IsNullOrEmpty(w.Reported))
                            {
                                nsc.HasReported = 1;
                                nsc.NoReported = 0;
                            }
                            else
                            {
                                nsc.HasReported = 0;
                                nsc.NoReported = 1;
                            }
                            lsc.Add(nsc);
                        });
                        #endregion
                    }
                    List<NextLevelStatics> nsxList = null;
                    switch (_gridid)
                    {
                        case (int)GrassrootsFloodCtrlEnums.RoleEnums.省级用户:
                            #region
                            //市
                            var levelcity = db.Select<ADCDInfo>(x => x.adcd.Contains("00000000000") && x.adcd != _adcd).OrderBy(o=>o.adcd).ToList();
                            if (levelcity != null)
                            {
                                nsxList = new List<NextLevelStatics>();
                                levelcity.ForEach(x =>
                                {
                                    var h = 0; var n = 0;
                                    NextLevelStatics nsx = new NextLevelStatics();
                                    nsx.ADCD = x.adcd; nsx.ADNM = x.adnm; nsx.Grade = (int)GrassrootsFloodCtrlEnums.RoleEnums.市级用户;
                                    
                                    //县
                                    var levelcounty = db.Select<ADCDInfo>(y => y.adcd.Contains(x.adcd.Substring(0, 4)) && y.grade == 2 && y.adcd != x.adcd);
                                    levelcounty.ForEach(y =>
                                    {
                                        var geth = lsn.Where(w=> w.ADCD.Contains(y.adcd.Substring(0, 6)) && w.ADCD.Contains("000000") && w.ADCD != y.adcd && w.HasReported == 1);
                                        var getn = lsn.Where(w => w.ADCD.Contains(y.adcd.Substring(0, 6)) && w.ADCD.Contains("000000") && w.ADCD != y.adcd && w.NoReported == 1);
                                        h = h + geth.Count();
                                        n = n + getn.Count();
                                    });
                                    nsx.HasReported = h;
                                    nsx.NoReported = n;
                                    //nsx.GradeName = getGrad(h, n);
                                    nsx.Grade= h==0 && n==0 ? 0: Math.Round(h * 1.00 / (h + n) * 100.0, 0);
                                    nsxList.Add(nsx);
                                });
                                var hall= nsxList.Sum(w => w.HasReported);
                                var nall = nsxList.Sum(w => w.NoReported);
                                //sm.GradeName = getGrad(hall.Value, nall.Value);
                                sm.HasReportedAll = hall;
                                sm.NoReportedAll = nall;
                                sm.TownAll = hall + nall;
                                var faudit1 = db.Select<Model.Audit.Audit>();
                                sm.isSupervise = faudit1 != null ? faudit1.Count() : 0;
                                sm.CCStatics = nsxList.OrderByDescending(w=>w.Grade).ToList();
                                //县汇总
                                var hcounty = lsc.Sum(w=>w.HasReported);
                                var ncounty = lsc.Sum(w=>w.NoReported);
                                //sm.GradeNameCounty = getGrad(hcounty.Value, ncounty.Value);
                                sm.CountyAll = hcounty + ncounty;
                                sm.HasReportedAllCounty = hcounty;
                                sm.NoReportedAllCounty = ncounty;
                                sm.VillageAll = db.Single<ADCDInfo>("select COUNT(*) as Id from ADCDInfo where CONVERT(int,SUBSTRING(adcd,10,3)) > 0").Id;
                                //sm.CStattics = lsc;
                            }
                            #endregion
                            break;
                        case (int)GrassrootsFloodCtrlEnums.RoleEnums.市级用户:
                            #region
                            //取县
                            nsxList = new List<NextLevelStatics>();
                            var levelcountys = db.Select<ADCDInfo>(y => y.adcd.Contains(_adcd.Substring(0, 4)) && y.adcd.Contains("000000000") && y.adcd != _adcd).OrderBy(o=>o.adcd).ToList();
                            levelcountys.ForEach(y =>
                            {
                                NextLevelStatics nsx = new NextLevelStatics();
                                nsx.ADCD = y.adcd; nsx.ADNM = y.adnm; //nsx.Grade = (int)GrassrootsFloodCtrlEnums.RoleEnums.县级用户;
                                var geth = lsn.Where(w => w.ADCD.Contains(y.adcd.Substring(0, 6)) && w.ADCD.Contains("000000") && w.ADCD != y.adcd && w.HasReported == 1);
                                var getn = lsn.Where(w => w.ADCD.Contains(y.adcd.Substring(0, 6)) && w.ADCD.Contains("000000") && w.ADCD != y.adcd && w.NoReported == 1);
                                nsx.HasReported = geth.Count();
                                nsx.NoReported = getn.Count();
                                //nsx.GradeName = getGrad(geth.Count(),getn.Count());
                                nsx.Grade= geth.Count() == 0 && getn.Count() == 0 ? 0 : Math.Round(geth.Count() * 1.00 / (geth.Count() + getn.Count()) * 100.0, 0);
                                nsxList.Add(nsx);
                            });
                            
                            var hs = nsxList.Sum(w => w.HasReported);
                            var ns = nsxList.Sum(w => w.NoReported);
                            sm.HasReportedAll = hs;
                            sm.NoReportedAll = ns;
                            sm.TownAll = hs + ns;
                            sm.CCStatics = nsxList.OrderByDescending(w=>w.Grade).ToList();
                           //sm.GradeName = getGrad(hs.Value,ns.Value);
                            sm.ADNM = db.Single<ADCDInfo>(w => w.adcd == _adcd).adnm;
                            var faudit = db.Select<Model.Audit.Audit>(w=>w.TownADCD.Contains(_adcd.Substring(0,4)));
                            sm.isSupervise = faudit != null ? faudit.Count() : 0;
                            //县汇总
                            List<NextLevelStatics> cList = new List<NextLevelStatics>();
                            var geth1 = lsc.Where(w => w.ADCD.Contains(_adcd.Substring(0, 4)) && w.ADCD.Contains("000000000") && w.HasReported == 1);
                            var getn1 = lsc.Where(w => w.ADCD.Contains(_adcd.Substring(0, 4)) && w.ADCD.Contains("000000000") && w.NoReported == 1);
                            sm.CountyAll = geth1.Count()+ getn1.Count();
                            sm.HasReportedAllCounty = geth1.Count();
                            sm.NoReportedAllCounty = getn1.Count();
                            //sm.GradeNameCounty = getGrad(geth1.Count(),getn1.Count());
                            sm.VillageAll = db.Single<ADCDInfo>("select Count(*) as Id from ADCDInfo where adcd like '"+ _adcd.Substring(0, 4) + "%' and CONVERT(int,SUBSTRING(adcd,10,3)) > 0").Id;
                            #endregion
                            break;
                        case (int)GrassrootsFloodCtrlEnums.RoleEnums.县级用户:
                            #region
                            //取镇
                            nsxList = new List<NextLevelStatics>();
                            var leveltowns = lsn.Where(y => y.ADCD.Contains(_adcd.Substring(0, 6)) && y.ADCD.Contains("000000") && y.ADCD != _adcd).OrderBy(o=>o.ADCD).ToList();
                            leveltowns.ForEach(y =>
                            {
                                NextLevelStatics nsx = new NextLevelStatics();
                                nsx.ADCD = y.ADCD; nsx.ADNM = y.ADNM; nsx.Grade = (int)GrassrootsFloodCtrlEnums.RoleEnums.乡镇用户;
                                nsx.HasReported = y.HasReported;
                                nsx.NoReported = y.NoReported;
                                //nsx.GradeName = getGrad(y.HasReported.Value, y.NoReported.Value);
                                nsx.Grade = y.HasReported.Value == 0 && y.NoReported.Value == 0 ? 0 : Math.Round(y.HasReported.Value * 1.00 / (y.HasReported.Value + y.NoReported.Value) * 100.0, 0);
                                nsxList.Add(nsx);
                            });

                            var ht = lsn.Where(y => y.ADCD.Contains(_adcd.Substring(0, 6)) && y.ADCD.Contains("000000") && y.ADCD != _adcd && y.HasReported == 1).Count();
                            var nt = lsn.Where(y => y.ADCD.Contains(_adcd.Substring(0, 6)) && y.ADCD.Contains("000000") && y.ADCD != _adcd && y.NoReported == 1).Count();
                            sm.HasReportedAll = ht;
                            sm.NoReportedAll = nt;
                            sm.TownAll = ht + nt;
                            sm.CCStatics = nsxList.OrderByDescending(w=>w.Grade).ToList();
                            //sm.GradeName = getGrad(ht, nt);
                            sm.ADNM = db.Single<ADCDInfo>(w => w.adcd == _adcd).adnm;
                            sm.PADNM = db.Single<ADCDInfo>(w => w.adcd == _adcd.Substring(0, 4) + "00000000000").adnm;
                            var faudit2 = db.Select<Model.Audit.Audit>(w=>w.TownADCD.Contains(_adcd.Substring(0,6)));
                            sm.isSupervise = faudit2 != null ? faudit2.Count() : 0;
                            sm.VillageAll = db.Single<ADCDInfo>("select Count(*) as Id from ADCDInfo where adcd like '" + _adcd.Substring(0, 6) + "%' and CONVERT(int,SUBSTRING(adcd,10,3)) > 0").Id;
                            #endregion
                            break;
                        case (int)GrassrootsFloodCtrlEnums.RoleEnums.乡镇用户:
                            sm.ADNM = db.Single<ADCDInfo>(w => w.adcd == _adcd).adnm;
                            sm.PADNM = db.Single<ADCDInfo>(w => w.adcd == _adcd.Substring(0, 4) + "00000000000").adnm;
                            sm.PPADNM = db.Single<ADCDInfo>(w => w.adcd == _adcd.Substring(0, 6) + "000000000").adnm;
                            break;
                    }
                }
                return sm;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public SuperviseModel PersonLiable1(SPersonLiable1 request)
        {
            SuperviseModel sm = new SuperviseModel();
            var _year = request.year == null ? DateTime.Now.Year : request.year.Value;
            try
            {
                //
                using (var db = DbFactory.Open())
                {
                    List<NextLevelStatics> lsn = null;

                    //#region 公用 取出所有镇的填报情况
                    //    var sql = "select Towns.adcd,Towns.adnm," +
                    //        "TownPersonLiable.adcd TownPersonLiableNums, VillageWorkingGroup.VillageADCD VillageWorkingGroupNums," +
                    //        "VillageGridPersonLiable.VillageADCD VillageGridPersonLiableNums, VillageTransferPerson.adcd VillageTransferPersonNums," +
                    //        "VillagePic.adcd VillagePicNums from " +
                    //        "(select * from ADCDInfo where CONVERT(int, SUBSTRING(adcd, 7, 3)) > 0 and  adcd like '%000000') as Towns " +
                    //        "left join(select distinct(adcd) as adcd from TownPersonLiable where Year="+_year+") as TownPersonLiable on SUBSTRING(TownPersonLiable.adcd, 0, 10) = SUBSTRING(Towns.adcd, 0, 10) " +
                    //        "left join(select distinct(SUBSTRING(VillageADCD, 0, 10)) as VillageADCD from VillageWorkingGroup where Year=" + _year + ") as VillageWorkingGroup on SUBSTRING(Towns.adcd, 0, 10) = VillageWorkingGroup.VillageADCD " +
                    //        "left join(select distinct(SUBSTRING(VillageADCD, 0, 10)) as VillageADCD from VillageGridPersonLiable where Year=" + _year + ") as VillageGridPersonLiable on SUBSTRING(VillageGridPersonLiable.VillageADCD, 0, 10) = SUBSTRING(Towns.adcd, 0, 10) " +
                    //        "left join(select distinct(SUBSTRING(adcd, 0, 10)) as adcd from VillageTransferPerson where Year=" + _year + ") as VillageTransferPerson on SUBSTRING(VillageTransferPerson.adcd, 0, 10) = SUBSTRING(Towns.adcd, 0, 10) " +
                    //        "left join(select distinct(SUBSTRING(adcd, 0, 10)) as adcd from VillagePic where Year=" + _year + ") as VillagePic on SUBSTRING(VillagePic.adcd, 0, 10) = SUBSTRING(Towns.adcd, 0, 10) ";
                    //    var allTown = db.Select<StatiscTowns>(sql);
                    //    lsn = new List<NextLevelStatics>();
                    //    allTown.ForEach(w =>
                    //    {
                    //        NextLevelStatics nsz = new NextLevelStatics();
                    //        nsz.ADCD = w.ADCD; nsz.ADNM = w.ADNM; nsz.Grade = 4;
                    //        //取填报数据
                    //        var _a = string.IsNullOrEmpty(w.TownPersonLiableNums);
                    //        var _b = string.IsNullOrEmpty(w.VillageWorkingGroupNums);
                    //        var _c = string.IsNullOrEmpty(w.VillageGridPersonLiableNums);
                    //        var _d = string.IsNullOrEmpty(w.VillageTransferPersonNums);
                    //        var _e = string.IsNullOrEmpty(w.VillagePicNums);
                    //        if (!_a || !_b || !_c || !_d || !_e)
                    //        {
                    //            nsz.HasReported = 1;
                    //            nsz.NoReported = 0;
                    //        }
                    //        else
                    //        {
                    //            nsz.HasReported = 0;
                    //            nsz.NoReported = 1;
                    //        }
                    //        lsn.Add(nsz);
                    //    });
                    //    #endregion
                    //List<NextLevelStatics> nsxList = null;
                    //#region
                    ////市

                    //    nsxList = new List<NextLevelStatics>();

                    //    //县
                    //    var levelcounty = db.Select<ADCDInfo>("select * from ADCDInfo where CONVERT(int,SUBSTRING(adcd,0,4)) > 0 and adcd like '%000000000' and adcd not like '%00000000000' order by adcd asc");
                    //    levelcounty.ForEach(x =>
                    //    {
                    //        NextLevelStatics nsx = new NextLevelStatics();
                    //        nsx.ADCD = x.adcd; nsx.ADNM = x.adnm; nsx.Grade = (int)GrassrootsFloodCtrlEnums.RoleEnums.市级用户;
                    //        var geth = lsn.Where(w => w.ADCD.Contains(x.adcd.Substring(0, 6)) && w.ADCD.Contains("000000")  && w.HasReported == 1);
                    //        var getn = lsn.Where(w => w.ADCD.Contains(x.adcd.Substring(0, 6)) && w.ADCD.Contains("000000")  && w.NoReported == 1);
                    //        var h = geth.Count();
                    //        var n = getn.Count();
                    //        nsx.HasReported = h;
                    //        nsx.NoReported = n;
                    //        nsxList.Add(nsx);
                    //    });


                    //    var hall = nsxList.Sum(w => w.HasReported);
                    //    var nall = nsxList.Sum(w => w.NoReported);
                    //    sm.GradeName = getGrad(hall.Value, nall.Value);
                    //    sm.HasReportedAll = hall;
                    //    sm.NoReportedAll = nall;
                    //    sm.TownAll = hall + nall;
                    //    sm.CCStatics = nsxList;
                    ////sm.CStattics = lsc;

                    //#endregion
                    //#region 未提交审核统计
                    ////var sql = "select SUBSTRING(a.adcd,0,7)+'000000000' as adcd,adnm from (select Audit.TownADCD,ADCDInfo.adcd,ADCDInfo.adnm from Audit right join ADCDInfo" +
                    ////   " on Audit.TownADCD = ADCDInfo.adcd where ADCDInfo.adcd like '%000000' and CONVERT(int, SUBSTRING(ADCDInfo.adcd, 7, 3)) > 0 )  as a where TownADCD is null order by adcd asc";
                    ////var f = db.Select<SuperviseModel>(sql);
                    ////f.ForEach(w =>
                    ////{
                    ////    SuperviseModel sm1 = new SuperviseModel();
                    ////    var f1 = db.Single<ADCDInfo>(x => x.adcd == w.adcd);
                    ////    sm1.PADNM = f1.adnm;
                    ////    sm1.adnm = w.adnm;
                    ////    list.Add(sm1);
                    ////});
                    //#endregion
                    #region 县下辖村统计
                    var sql = "select adcd,adnm from ADCDInfo where adcd like '%000000000' and CONVERT(int,SUBSTRING(adcd,5,2)) > 0 order by adcd asc";
                    var f = db.Select<SuperviseModel>(sql);
                    List<CountyVillage> lcv = new List<CountyVillage>();
                    f.ForEach(w =>
                    {
                        CountyVillage sm1 = new CountyVillage();
                        var f1 = db.Select<ADCDInfo>("adcd like '"+w.adcd.Substring(0,6)+"%' and adcd not like '%000000'");
                        sm1.adnm = w.adnm;
                        sm1.villagecount = f1.Count();
                        lcv.Add(sm1);
                    });
                    sm.CountyVillage = lcv;
                    #endregion
                }
                return sm;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public BsTableDataSource<ADCDDisasterViewModel> GetPersonLiabelList(GetPersonLiabelList request)
        {
            //List<PersonLiabelListModel> plml = new List<PersonLiabelListModel>();
            //PersonLiabelListModel plm = new PersonLiabelListModel();
            if (string.IsNullOrEmpty(request.adcd)) throw new Exception("镇级行政编码不能为空！");
            var _year = request.year == null ? DateTime.Now.Year : request.year;
            using (var db = DbFactory.Open())
            {
                var builder = db.From<ADCDInfo>();
                builder.LeftJoin<ADCDInfo, ADCDDisasterInfo>((x,y)=>x.adcd == y.adcd);//连表
                builder.Where(w=> w.adcd.StartsWith(request.adcd.Substring(0, 9)));//通过adcd查询
                builder.Where<ADCDDisasterInfo>(w => w.Year == _year);
                if (!string.IsNullOrEmpty(request.key))
                    builder.And(x => x.adnm.Contains(request.key));
                var count = db.Count(builder);
                builder.Select("ADCDInfo.adcd,ADCDInfo.adnm,ADCDInfo.lng,ADCDInfo.lat,ADCDDisasterInfo.TotalNum,ADCDDisasterInfo.PointNum,ADCDDisasterInfo.PopulationNum,ADCDDisasterInfo.FXFTRW ");
                var rows = request.PageSize == 0 ? 10 : request.PageSize;
                var skip = request.PageIndex == 0 ? 0 : request.PageIndex * rows;
                builder.Limit(skip, rows);
                var list = db.Select<ADCDDisasterViewModel>(builder);
                List<ADCDDisasterViewModel> newlist = new List<ADCDDisasterViewModel>();
                //到岗人数
                var dgperson = db.Select<AllPersonListViewModel>("select distinct adcd from AppRecord");
                list.ForEach(w =>
                {
                    ADCDDisasterViewModel _adcdvm = new ADCDDisasterViewModel();
                    _adcdvm.adcd = w.adcd;
                    _adcdvm.adnm = w.adnm;
                    _adcdvm.Lng = w.Lng;
                    _adcdvm.Lat = w.Lat;
                    _adcdvm.TotalNum = w.TotalNum;
                    var pointnum = db.Select<VillageTransferPerson>(y => y.adcd == w.adcd);
                    //受灾点
                    if(pointnum != null && pointnum.Count > 0)
                    {
                        if (pointnum[0].IfTransfer == null) { _adcdvm.PointNum = pointnum.Count(); }
                        else
                        {
                            _adcdvm.PointNum = 0;
                        }
                        //受灾害影响人口
                        _adcdvm.PopulationNum = pointnum.Sum(x => x.HouseholderNum);
                    }
                    else
                    {
                        _adcdvm.PointNum = 0;
                        _adcdvm.PopulationNum = 0;
                    }
                    _adcdvm.onperson = dgperson.Where(x=>x.adcd == w.adcd).Count();
                    _adcdvm.FXFTRW = w.FXFTRW;
                    newlist.Add(_adcdvm);
                });
                return new BsTableDataSource<ADCDDisasterViewModel>() { total = count, rows = newlist };
            }
        }
        public BsTableDataSource<StatiscPerson> CCKHVillage(CCKHVillage request)
        {
            //if (string.IsNullOrEmpty(request.adcd)) throw new Exception("adcd不能为空！");
            var listgroup1 = Iworkgroup.GetGroup1(new GetGroup1() { fid = request.fid, name = request.name, adcd = request.adcd, year = request.year, PageSize = 99999 }).rows;
            var grid = Igrid.GetVillageGrid1(new GetVillageGrid1() { fid = request.fid, name = request.name, adcd = request.adcd, year = request.year, PageSize = 99999 }).rows;
            var transfer = Itransfer.GetVillageTransferPerson2(new GetVillageTransferPerson2() { fid = request.fid, name = request.name, adcd = request.adcd, year = request.year, PageSize = 99999 }).rows;
            listgroup1.AddRange(grid);
            listgroup1.AddRange(transfer);
            #region
            var listgroup = new List<StatiscPerson>();
            if (!string.IsNullOrEmpty(request.post))
                listgroup = listgroup1.Where(w => w.post.Contains(request.post)).ToList();
            if (!string.IsNullOrEmpty(request.position))
                listgroup = listgroup1.Where(w => w.post.Contains(request.position)).ToList();
            if (!string.IsNullOrEmpty(request.name))
                listgroup = listgroup1.Where(w => w.personLiable.Contains(request.name)).ToList();
            if (string.IsNullOrEmpty(request.post) && string.IsNullOrEmpty(request.position) && string.IsNullOrEmpty(request.name))
                listgroup = listgroup1;
            #endregion
            List<StatiscPerson> lsp = new List<StatiscPerson>();
            var adnmparents = "";
            using (var db=DbFactory.Open()) {
                var _city = db.Single<ADCDInfo>(w => w.adcd == request.adcd.Substring(0, 4) + "00000000000");
                var _county = db.Single<ADCDInfo>(w => w.adcd == request.adcd.Substring(0, 6) + "000000000");
                var _town = db.Single<ADCDInfo>(w => w.adcd == request.adcd.Substring(0, 9) + "000000");
                adnmparents = _city.adnm + "_" + _county.adnm + "_" + _town.adnm;
            }
            var newlist = listgroup.Select(w => w.personLiable).Distinct().ToList();
            newlist.Remove("");
            newlist.ForEach(w => {
                var f = listgroup.Where(x => x.personLiable == w).ToList();
                var newpost = ""; var phone = "";
                f.ForEach(y => {
                    newpost += y.post + ";";
                    phone += y.handPhone + ';';
                });
                var fphones = phone.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                var fnewpost = newpost.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                StatiscPerson tplvm = new StatiscPerson()
                {
                    personLiable = w,
                    post = string.Join(";", fnewpost),
                    handPhone = string.Join(";", fphones),
                    id = f.FirstOrDefault().id,
                    adcd = f.FirstOrDefault().adcd,
                    adnmparent = adnmparents
                };
                if (request.fid == null)
                {
                    using (var db = DbFactory.Open())
                    {
                        var builder1 = db.From<SpotCheck>();
                        builder1.Where(y => y.adcd == request.adcd && y.year == request.year && y.bycheckman == tplvm.personLiable && y.bycheckphone == tplvm.handPhone).OrderByDescending(o => o.checktime);
                        var fbycheckman = db.Single(builder1);
                        if (fbycheckman != null) { tplvm.checkresult = fbycheckman.checkstatus + "(" + Convert.ToDateTime(fbycheckman.checktime).ToString("yyyy-MM-dd HH:mm:ss") + ")"; }
                        else { tplvm.checkresult = "-"; }
                    }
                }

                lsp.Add(tplvm);
            });
            return new BsTableDataSource<StatiscPerson>() { rows = lsp, total = lsp.Count() };
        }

        public string getGrad(int h,int n)
        {
            var r = "";
            if (h == 0 && n == 0) return "cha";
            var percent = Math.Round(h * 1.00 / (h + n) * 100.0, 0);
            var condition = ConfigurationManager.AppSettings["SuperviseLevel"];
            var c = condition.Split('|');
            foreach (string item in c) {
                var i = item.Split(',');
                if (percent >= int.Parse(i[0]) && percent <= int.Parse(i[1])) r = i[2];
            }
            return r;
        }
        public BaseResult SetCCKH(SetCCKH request)
        {
            BaseResult br = new BaseResult();
            if (string.IsNullOrEmpty(request.adcd)) { br.IsSuccess = false; br.ErrorMsg="adcd不能为空"; return br; }
            if (string.IsNullOrEmpty(request.checkman)) { br.IsSuccess = false; br.ErrorMsg = "抽查人不能为空"; return br; }
            if (string.IsNullOrEmpty(request.checktime)) { br.IsSuccess = false; br.ErrorMsg = "抽查时间不能为空"; return br; }
            if (string.IsNullOrEmpty(request.bycheckman)) { br.IsSuccess = false; br.ErrorMsg = "被抽查人不能为空"; return br; }
            if (string.IsNullOrEmpty(request.bycheckphone)) { br.IsSuccess = false; br.ErrorMsg = "被抽查人手机不能为空"; return br; }
            using (var db=DbFactory.Open())
            {
                try
                {
                    SpotCheck sc = new SpotCheck()
                    {
                        adcd = request.adcd,
                        checkman = request.checkman,
                        checktime = Convert.ToDateTime(request.checktime),
                        checkitems = request.checkitems,
                        bycheckman = request.bycheckman,
                        bycheckphone = request.bycheckphone,
                        checkstatus = request.checkstatus,
                        noremarks = request.noremarks,
                        remarks = request.remarks,
                        uid = request.uid,
                        areas = request.areas,
                        year = DateTime.Now.Year
                    };
                    var realname = db.Single<UserInfo>(w => w.UserName == request.checkman);
                    if (realname != null) sc.checkmanrealname = realname.UserRealName;
                    else sc.checkmanrealname = request.checkmanrealname;
                    operateLog log = new operateLog();
                    log.userName = RealName;
                    log.operateTime = DateTime.Now;
                    log.operateMsg = "抽查人:"+ request.checkman + ",ID号:"+request.uid+","+request.checktime+"抽查了{"+request.areas+"的"+request.bycheckman+"("+request.bycheckphone+")}";
                    List<operateLog> listLog = new List<operateLog>();
                    listLog.Add(log);
                    sc.operateLog = JsonTools.ObjectToJson(listLog);
                    br.IsSuccess = db.Insert(sc) == 1?true:false;
                    #region 日志
                    StringBuilder sb = new StringBuilder();
                    sb.Append("在栏目{监督考核/抽查考核}下,新增了数据{");
                    sb.Append("adcd：" + sc.adcd + ";");
                    sb.Append("抽查人：" + sc.checkman + ";");
                    sb.Append("抽查时间：" + sc.checktime + ";");
                    sb.Append("被抽查人：" + sc.bycheckman + ";");
                    sb.Append("被抽查手机：" + sc.bycheckphone + ";");
                    sb.Append("被抽查人地区：" + sc.areas + ";");
                    sb.Append("不合格项：" + sc.checkitems + ";");
                    sb.Append("不合格项“其他”描述：" + sc.noremarks + ";");
                    sb.Append("备注：" + sc.remarks + ";");
                    sb.Append("}");
                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
                    #endregion
                }
                catch (Exception ex)
                {
                    br.IsSuccess = false;br.ErrorMsg = ex.Message;
                }
            }
            return br;
        }
       public BsTableDataSource<SpotCheckModel> GetCCJLTable(GetCCJLTable request)
        {
            using (var db=DbFactory.Open())
            {
                var builder = db.From<SpotCheck>();
                var _adcd = string.IsNullOrEmpty(request.adcd) ? adcd : request.adcd;
                var where = "1=1 ";
                if (!string.IsNullOrEmpty(_adcd))
                {
                    //查询
                    switch (request.level)
                    {
                        case 1://市
                            where += "AND adcd like '"+_adcd.Substring(0,4)+"%' ";
                            break;
                        case 2://县（区）
                            where += "AND adcd like '" + _adcd.Substring(0, 6) + "%' ";
                            break;
                        case 3://镇街
                            //builder.Where(w => w.adcd.Contains(_adcd.Substring(0, 9)));
                            where += "AND adcd like '" + _adcd.Substring(0, 9) + "%' ";
                            break;
                        case 4://行政村
                            //builder.Where(w => w.adcd.Contains(_adcd.Substring(0, 12)));
                            where += "AND adcd like '" + _adcd.Substring(0, 12) + "%' ";
                            break;
                    }
                    switch (RowID)
                    {
                        case 2:
                            //builder.Where(w => w.adcd.Contains(_adcd.Substring(0, 4)));
                            where += "AND adcd like '" + _adcd.Substring(0, 4) + "%' ";
                            break;
                        case 5:
                            //builder.Where(w => w.adcd.Contains(_adcd.Substring(0, 2)));
                            where += "AND adcd like '" + _adcd.Substring(0, 2) + "%' ";
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(request.name))
                    //builder.Where(w=>w.bycheckman.Contains(request.name));
                    where += "AND bycheckman like '%" + request.name + "%' ";
                if (!string.IsNullOrEmpty(request.checkname))
                    // builder.Where(w=>w.checkmanrealname.Contains(request.checkname));
                    where += "AND checkmanrealname like '%" + request.checkname + "%' ";
                if (!string.IsNullOrEmpty(request.checkStatus))
                    where += "AND checkstatus='"+ request.checkStatus + "' ";

                var _year = request.year == null ? DateTime.Now.Year : request.year.Value;
                //builder.And(w=>w.year == _year);
                where += "AND year = "+_year+"";

                var count = db.Select<SpotCheck>("SELECT areas,adcd,bycheckman,bycheckphone,MAX(Id) as Id FROM SpotCheck WHERE "+ where + " group by areas,adcd,bycheckman,bycheckphone").Count();
                var rows = request.PageSize == 0 ? 10 : request.PageSize;
                var skip1 = request.PageIndex == 0 ? 10 : (request.PageIndex+1) * rows;
                var skip0 = request.PageIndex == 0 ? 0 : request.PageIndex * rows;
                var sql = "select * from(SELECT b.Id, b.areas,b.adcd,b.bycheckman,b.bycheckphone,b.checktime,a.IdCount,b.checkstatus,"
                    + "ROW_NUMBER() OVER(ORDER BY b.id) As RowNum FROM SpotCheck as b right join "
                    + "(SELECT areas, adcd, bycheckman, bycheckphone, MAX(Id) as Id,COUNT(Id) as IdCount FROM SpotCheck WHERE " + where+" "
                    +"group by areas, adcd, bycheckman, bycheckphone) as a on a.Id = b.Id) AS RowConstrainedResult WHERE RowNum > "+ skip0 + " AND RowNum <= "+ skip1 + "";
                var rlist = db.Select<SpotCheckModel>(sql);
                return new BsTableDataSource<SpotCheckModel>() { total = count,rows=rlist};
            }
        }
       public BsTableDataSource<SpotCheckOne> GetCCJLTableOne(GetCCJLTableOne request)
       {
            List<SpotCheckOne> ls = new List<SpotCheckOne>();
            if (string.IsNullOrEmpty(request.adcd)) throw new Exception("adcd为空");
            if (string.IsNullOrEmpty(request.key)) throw new Exception("用户名不能为空");
            if (string.IsNullOrEmpty(request.phone)) throw new Exception("手机不能为空");
            var _year =request.year == null || request.year == 0 ? DateTime.Now.Year : request.year;
            using (var db = DbFactory.Open())
            {
                var builder = db.From<SpotCheck>();
                    builder.Where(w => w.adcd == request.adcd && w.year == _year && w.bycheckman == request.key && w.bycheckphone == request.phone);
                var count = db.Select(builder).Count();
                var rows = request.PageSize == 0 ? 10 : request.PageSize;
                var skip = request.PageIndex == 0 ? 0 : request.PageIndex * rows;
                builder.OrderByDescending(w=>w.checktime);
                builder.Limit(skip, rows);
                var rlist = db.Select<SpotCheck>(builder);
                if(rlist.Count() > 0)
                {
                    SpotCheckOne sco = new SpotCheckOne();
                    sco.areas = rlist.FirstOrDefault().areas;
                    sco.bycheckman = rlist.FirstOrDefault().bycheckman;
                    sco.year = rlist.FirstOrDefault().year.ToString();
                    sco.bycheckphone = rlist.FirstOrDefault().bycheckphone;
                    List<SpotCheck> lsc = new List<SpotCheck>();
                    rlist.ForEach(w => {
                        SpotCheck sc = new SpotCheck()
                        {
                            checkman = w.checkman,
                            checkmanrealname = w.checkmanrealname,
                            checktime = w.checktime,
                            checkitems = w.checkitems,
                            checkstatus = w.checkstatus,
                            remarks = string.IsNullOrEmpty(w.remarks) ? "" : w.remarks,
                            noremarks = string.IsNullOrEmpty(w.noremarks) ? "" : w.noremarks
                        };
                        lsc.Add(sc);
                    });
                    sco.checkItems = lsc;
                    ls.Add(sco);
                }
                return new BsTableDataSource<SpotCheckOne>() { total = ls.Count(), rows = ls };
            }
        }
        #region 到岗人员统计
        public List<AppAreaViewModel> GetCityAppInPostList(GetCityAppInPostList request)
        {
            using (var db = DbFactory.Open())
            {
                var stime = ""; var etime = ""; var where = "";
                if (!string.IsNullOrEmpty(request.starttime) && !string.IsNullOrEmpty(request.endtime))
                {
                    stime = request.starttime + " 00:00:00";
                    etime = request.endtime + " 23:59:59";
                    where = " where addtime >= '"+ stime + "' and addtime <= '"+ etime + "'";
                }
                var _adcd = string.IsNullOrEmpty(request.adcd) ? adcd : request.adcd;
               
                var _year = request.year == null || request.year == 0 ? DateTime.Now.Year : request.year;
                //所有责任人
                var allperson = db.SqlList<StatiscPersonInPost>("EXEC GetVillagePersonByYear @year", new {  year = _year });
                //到岗位人数

                var dgperson = db.Select<AllPersonListViewModel>("select distinct adcd from AppRecord "+where+"");
                List<AppAreaViewModel> _list = null; AppAreaViewModel _model = null;
                 List<AppAreaViewModelSon> _listson = null; AppAreaViewModelSon _modelson = null;
                //市
                switch (RowID)
                {
                    case 5:
                        _list = new List<AppAreaViewModel>();
                        _listson = new List<AppAreaViewModelSon>();
                        if (_adcd == adcd)
                        {
                            var citylist = db.Select<ADCDInfo>(w => w.grade == 1);
                            _model = new AppAreaViewModel();
                            citylist.ForEach(w =>
                            {
                                _modelson = new AppAreaViewModelSon();
                                _modelson.id = w.Id;
                                _modelson.parentId = w.parentId;
                                _modelson.grade = w.grade;
                                _modelson.adcd = w.adcd;
                                _modelson.adnm = w.adnm;
                                _modelson.inperson = allperson.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 4))).Count();
                                _modelson.noperson = dgperson.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 4))).Count();
                                _listson.Add(_modelson);
                            });
                            _model.SonList = _listson;
                            _list.Add(_model);
                        }
                        else if (request.grade == 1)
                        {
                            var countylist = db.Select<ADCDInfo>(w => w.Id == request.id || w.parentId == request.id);
                            _model = new AppAreaViewModel();
                            countylist.Where(w => w.grade == 2).ToList().ForEach(w =>
                              {
                                  _modelson = new AppAreaViewModelSon();
                                  _modelson.id = w.Id;
                                  _modelson.parentId = w.parentId;
                                  _modelson.grade = w.grade;
                                  _modelson.adcd = w.adcd;
                                  _modelson.adnm = w.adnm;
                                  _modelson.inperson = allperson.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 6))).Count();
                                  _modelson.noperson = dgperson.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 6))).Count();
                                  _listson.Add(_modelson);
                              });
                            _model.adnm = countylist.Find(w => w.Id == request.id).adnm;
                            _model.SonList = _listson;
                            _list.Add(_model);
                        }
                        else if (request.grade == 2)
                        {
                            var townlist = db.Select<ADCDInfo>(w => w.adcd == request.adcd.Substring(0,4)+"00000000000" || w.Id == request.id || w.parentId == request.id);
                            _model = new AppAreaViewModel();
                            townlist.Where(w => w.grade == 3).ToList().ForEach(w =>
                            {
                                _modelson = new AppAreaViewModelSon();
                                _modelson.id = w.Id;
                                _modelson.parentId = w.parentId;
                                _modelson.grade = w.grade;
                                _modelson.adcd = w.adcd;
                                _modelson.adnm = w.adnm;
                                _modelson.inperson = allperson.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 9))).Count();
                                _modelson.noperson = dgperson.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 9))).Count();
                                _listson.Add(_modelson);
                            });
                            _model.adnm = townlist.Find(w => w.Id == request.id).adnm;
                            _model.padnm = townlist.Find(w => w.adcd == request.adcd.Substring(0, 4) + "00000000000").adnm;
                            _model.SonList = _listson;
                            _list.Add(_model);
                        }
                        else { }
                        break;
                    case 2:
                        _list = new List<AppAreaViewModel>();
                        _listson = new List<AppAreaViewModelSon>();
                        if (adcd == _adcd)
                        {
                            var countylist = db.Select<ADCDInfo>(w => w.adcd.StartsWith(_adcd.Substring(0,4)) && (w.grade == 2 || w.grade == 1));
                            _model = new AppAreaViewModel();
                            countylist.Where(w => w.grade == 2).ToList().ForEach(w =>
                            {
                                _modelson = new AppAreaViewModelSon();
                                _modelson.id = w.Id;
                                _modelson.parentId = w.parentId;
                                _modelson.grade = w.grade;
                                _modelson.adcd = w.adcd;
                                _modelson.adnm = w.adnm;
                                _modelson.inperson = allperson.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 6))).Count();
                                _modelson.noperson = dgperson.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 6))).Count();
                                _listson.Add(_modelson);
                            });
                            _model.adnm = countylist.Find(w => w.adcd == _adcd).adnm;
                            _model.SonList = _listson;
                            _list.Add(_model);
                        }
                        else if (request.grade == 2)
                        {
                            var townlist = db.Select<ADCDInfo>(w => w.adcd == request.adcd.Substring(0, 4) + "00000000000" || w.Id == request.id || w.parentId == request.id);
                            _model = new AppAreaViewModel();
                            townlist.Where(w => w.grade == 3).ToList().ForEach(w =>
                            {
                                _modelson = new AppAreaViewModelSon();
                                _modelson.id = w.Id;
                                _modelson.parentId = w.parentId;
                                _modelson.grade = w.grade;
                                _modelson.adcd = w.adcd;
                                _modelson.adnm = w.adnm;
                                _modelson.inperson = allperson.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 9))).Count();
                                _modelson.noperson = dgperson.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 9))).Count();
                                _listson.Add(_modelson);
                            });
                            _model.adnm = townlist.Find(w => w.Id == request.id).adnm;
                            _model.padnm = townlist.Find(w => w.adcd == request.adcd.Substring(0, 4) + "00000000000").adnm;
                            _model.SonList = _listson;
                            _list.Add(_model);
                        }
                        else { }
                        break;
                    case 3:
                        _list = new List<AppAreaViewModel>();
                        _listson = new List<AppAreaViewModelSon>();
                        var townlist1 = db.Select<ADCDInfo>(w => (w.adcd.StartsWith(_adcd.Substring(0, 6)) && (w.grade == 3 || w.grade == 2)) || (w.adcd.StartsWith(_adcd.Substring(0, 4)) && w.grade == 1) );
                        _model = new AppAreaViewModel();
                        townlist1.Where(w => w.grade == 3).ToList().ForEach(w =>
                        {
                            _modelson = new AppAreaViewModelSon();
                            _modelson.id = w.Id;
                            _modelson.parentId = w.parentId;
                            _modelson.grade = w.grade;
                            _modelson.adcd = w.adcd;
                            _modelson.adnm = w.adnm;
                            _modelson.inperson = allperson.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 9))).Count();
                            _modelson.noperson = dgperson.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 9))).Count();
                            _listson.Add(_modelson);
                        });
                        _model.adnm = RealName;
                        _model.padnm = townlist1.Find(w => w.grade == 1).adnm;
                        _model.SonList = _listson;
                        _list.Add(_model);
                        break;
                }
                return _list;
            }
        }
        #endregion
    }
}
