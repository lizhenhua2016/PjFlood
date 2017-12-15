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
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;
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

namespace GrassrootsFloodCtrl.Logic.NoVerify
{
    public class NoVerifySuperviseManage : ManagerBase, INoVerifySuperviseManage
    {
        public BsTableDataSource<ADCDDisasterViewModel> NoVerifyGetPersonLiabelList(NoVerifyGetPersonLiabelList request)
        {
            //List<PersonLiabelListModel> plml = new List<PersonLiabelListModel>();
            //PersonLiabelListModel plm = new PersonLiabelListModel();
            if (string.IsNullOrEmpty(request.adcd)) throw new Exception("镇级行政编码不能为空！");
            var _year = request.year == null ? DateTime.Now.Year : request.year;
            using (var db = DbFactory.Open())
            {
                var builder = db.From<ADCDInfo>();
                builder.LeftJoin<ADCDInfo, ADCDDisasterInfo>((x, y) => x.adcd == y.adcd);
                builder.Where(w => w.adcd.StartsWith(request.adcd.Substring(0, 9)));
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
                    if (pointnum != null && pointnum.Count > 0)
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
                    _adcdvm.onperson = dgperson.Where(x => x.adcd == w.adcd).Count();
                    _adcdvm.FXFTRW = w.FXFTRW;
                    newlist.Add(_adcdvm);
                });
                return new BsTableDataSource<ADCDDisasterViewModel>() { total = count, rows = newlist };
            }
        }

        public SuperviseModel NoVerifySPersonLiable(NoVerifySPersonLiable request)
        {
            SuperviseModel sm = new SuperviseModel();
            var _adcd = string.IsNullOrEmpty(request.adcd) ? adcd : request.adcd;
            var _year = request.year == null ? DateTime.Now.Year : request.year.Value;
            var _gridid = request.grid == null ? RowID : request.grid.Value;
            if (_gridid == 0) _gridid = (int)GrassrootsFloodCtrlEnums.RoleEnums.省级用户;
            if (string.IsNullOrEmpty(_adcd)) throw new Exception("行政编码为空！");
            try
            {
                //
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
                            var levelcity = db.Select<ADCDInfo>(x => x.adcd.Contains("00000000000") && x.adcd != _adcd).OrderBy(o => o.adcd).ToList();
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
                                        var geth = lsn.Where(w => w.ADCD.Contains(y.adcd.Substring(0, 6)) && w.ADCD.Contains("000000") && w.ADCD != y.adcd && w.HasReported == 1);
                                        var getn = lsn.Where(w => w.ADCD.Contains(y.adcd.Substring(0, 6)) && w.ADCD.Contains("000000") && w.ADCD != y.adcd && w.NoReported == 1);
                                        h = h + geth.Count();
                                        n = n + getn.Count();
                                    });
                                    nsx.HasReported = h;
                                    nsx.NoReported = n;
                                    //nsx.GradeName = getGrad(h, n);
                                    nsx.Grade = h == 0 && n == 0 ? 0 : Math.Round(h * 1.00 / (h + n) * 100.0, 0);
                                    nsxList.Add(nsx);
                                });
                                var hall = nsxList.Sum(w => w.HasReported);
                                var nall = nsxList.Sum(w => w.NoReported);
                                //sm.GradeName = getGrad(hall.Value, nall.Value);
                                sm.HasReportedAll = hall;
                                sm.NoReportedAll = nall;
                                sm.TownAll = hall + nall;
                                var faudit1 = db.Select<Model.Audit.Audit>();
                                sm.isSupervise = faudit1 != null ? faudit1.Count() : 0;
                                sm.CCStatics = nsxList.OrderByDescending(w => w.Grade).ToList();
                                //县汇总
                                var hcounty = lsc.Sum(w => w.HasReported);
                                var ncounty = lsc.Sum(w => w.NoReported);
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
                            var levelcountys = db.Select<ADCDInfo>(y => y.adcd.Contains(_adcd.Substring(0, 4)) && y.adcd.Contains("000000000") && y.adcd != _adcd).OrderBy(o => o.adcd).ToList();
                            levelcountys.ForEach(y =>
                            {
                                NextLevelStatics nsx = new NextLevelStatics();
                                nsx.ADCD = y.adcd; nsx.ADNM = y.adnm; //nsx.Grade = (int)GrassrootsFloodCtrlEnums.RoleEnums.县级用户;
                                var geth = lsn.Where(w => w.ADCD.Contains(y.adcd.Substring(0, 6)) && w.ADCD.Contains("000000") && w.ADCD != y.adcd && w.HasReported == 1);
                                var getn = lsn.Where(w => w.ADCD.Contains(y.adcd.Substring(0, 6)) && w.ADCD.Contains("000000") && w.ADCD != y.adcd && w.NoReported == 1);
                                nsx.HasReported = geth.Count();
                                nsx.NoReported = getn.Count();
                                //nsx.GradeName = getGrad(geth.Count(),getn.Count());
                                nsx.Grade = geth.Count() == 0 && getn.Count() == 0 ? 0 : Math.Round(geth.Count() * 1.00 / (geth.Count() + getn.Count()) * 100.0, 0);
                                nsxList.Add(nsx);
                            });

                            var hs = nsxList.Sum(w => w.HasReported);
                            var ns = nsxList.Sum(w => w.NoReported);
                            sm.HasReportedAll = hs;
                            sm.NoReportedAll = ns;
                            sm.TownAll = hs + ns;
                            sm.CCStatics = nsxList.OrderByDescending(w => w.Grade).ToList();
                            //sm.GradeName = getGrad(hs.Value,ns.Value);
                            sm.ADNM = db.Single<ADCDInfo>(w => w.adcd == _adcd).adnm;
                            var faudit = db.Select<Model.Audit.Audit>(w => w.TownADCD.Contains(_adcd.Substring(0, 4)));
                            sm.isSupervise = faudit != null ? faudit.Count() : 0;
                            //县汇总
                            List<NextLevelStatics> cList = new List<NextLevelStatics>();
                            var geth1 = lsc.Where(w => w.ADCD.Contains(_adcd.Substring(0, 4)) && w.ADCD.Contains("000000000") && w.HasReported == 1);
                            var getn1 = lsc.Where(w => w.ADCD.Contains(_adcd.Substring(0, 4)) && w.ADCD.Contains("000000000") && w.NoReported == 1);
                            sm.CountyAll = geth1.Count() + getn1.Count();
                            sm.HasReportedAllCounty = geth1.Count();
                            sm.NoReportedAllCounty = getn1.Count();
                            //sm.GradeNameCounty = getGrad(geth1.Count(),getn1.Count());
                            sm.VillageAll = db.Single<ADCDInfo>("select Count(*) as Id from ADCDInfo where adcd like '" + _adcd.Substring(0, 4) + "%' and CONVERT(int,SUBSTRING(adcd,10,3)) > 0").Id;
                            #endregion
                            break;
                        case (int)GrassrootsFloodCtrlEnums.RoleEnums.县级用户:
                            #region
                            //取镇
                            nsxList = new List<NextLevelStatics>();
                            var leveltowns = lsn.Where(y => y.ADCD.Contains(_adcd.Substring(0, 6)) && y.ADCD.Contains("000000") && y.ADCD != _adcd).OrderBy(o => o.ADCD).ToList();
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
                            sm.CCStatics = nsxList.OrderByDescending(w => w.Grade).ToList();
                            //sm.GradeName = getGrad(ht, nt);
                            sm.ADNM = db.Single<ADCDInfo>(w => w.adcd == _adcd).adnm;
                            sm.PADNM = db.Single<ADCDInfo>(w => w.adcd == _adcd.Substring(0, 4) + "00000000000").adnm;
                            var faudit2 = db.Select<Model.Audit.Audit>(w => w.TownADCD.Contains(_adcd.Substring(0, 6)));
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
    }
}
