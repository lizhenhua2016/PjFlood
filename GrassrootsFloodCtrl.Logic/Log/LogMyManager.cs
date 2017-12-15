using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.Model.Sys;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.Sys;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.Model.Audit;
using GrassrootsFloodCtrl.ServiceModel.Audit;

namespace GrassrootsFloodCtrl.Logic.Log
{
    public class LogMyManager : ManagerBase, ILogMyManager
    {
        #region 市，县 统计列表
        public SuperviseModel GetLogStatisList(GetLogStatisList request)
        {
            using (var db = DbFactory.Open())
            {
                var stime = ""; var etime = "";
                if (string.IsNullOrEmpty(request.starttime) && string.IsNullOrEmpty(request.endtime))
                {
                    stime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd 00:00:00");
                    etime = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                }
                else
                {
                    stime = request.starttime+" 00:00:00";
                    etime = request.endtime+ " 23:59:59";
                }
                var _adcd = string.IsNullOrEmpty(request.adcd) ? adcd : request.adcd;

                SuperviseModel sm = new SuperviseModel();
                var loglist = db.Select<LogStatisItem>("select adcd,COUNT(adcd) adcdount from LogInfo where (OperationType='新增' or OperationType='更新') and SUBSTRING(adcd,7,3) > 0 and adcd like '%000000' and tm >= '" + stime + "' and tm <= '" + etime + "' group by adcd order by adcd asc");
                var townList = db.Select<ADCDInfo>(w => w.adcd.EndsWith("000000") && w.adcd.Substring(6, 3) != "000");
                List<LogTownStatisItem> _list = null;
                List<LogStatisItem> loglistCounty = new List<LogStatisItem>();
                List<ADCDInfo> adcdlist = null; var parentid = "";
                switch (RowID)
                {
                    case 5:
                        LogTownStatisItem ns = null;
                        if (_adcd == adcd)
                        {
                            var citylist = db.Select<ADCDInfo>(w => w.adcd.EndsWith("00000000000") && w.adcd != "330000000000000").OrderBy(o => o.adcd).ToList();
                            _list = new List<LogTownStatisItem>();
                            citylist.ForEach(w =>
                            {
                                ns = new LogTownStatisItem();
                                var fcity = loglist.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 4))).ToList();
                                ns.townUpdateNum = fcity == null ? 0 : fcity.Count;
                                ns.ADNM = w.adnm;
                                ns.ADCD = w.adcd;
                                var ftown = townList.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 4))).ToList();
                                ns.townAllNum = ftown == null ? 0 : ftown.Count;
                                _list.Add(ns);
                            });
                        }
                        else if (_adcd.Contains("00000000000"))
                        {
                            loglistCounty = db.Select<LogStatisItem>("select adcd,COUNT(adcd) adcdount from LogInfo where (OperationType='新增' or OperationType='更新') and Operation not like '%审核%' and SUBSTRING(adcd,5,2) > 0 and adcd like '%000000000' and tm >= '" + stime + "' and tm <= '" + etime + "' group by adcd order by adcd asc");
                            sm.adnm = db.Single<ADCDInfo>(w => w.adcd == _adcd).adnm;
                            _list = getCountyList(db, request, loglist, townList, loglistCounty, _adcd, _list);
                        }
                        else if (_adcd.Contains("000000"))
                        {
                            parentid = _adcd.Substring(0, 4) + "00000000000";
                            adcdlist = db.Select<ADCDInfo>(w => w.adcd == _adcd || w.adcd == parentid);
                            //县名
                            sm.GradeNameCounty = adcdlist.Single(w => w.adcd == _adcd).adnm;
                            //市名
                            sm.GradeName = adcdlist.Single(w => w.adcd == parentid).adnm;
                            //镇列表
                            _list = getTownList(db, request, loglist, townList, loglistCounty, _adcd, stime, etime, _list);
                        }
                        else { }

                        sm.LSTownStatics = _list;
                        break;
                    case 2:
                        //loglistCounty = db.Select<LogStatisItem>("select adcd,COUNT(adcd) adcdount from LogInfo where (OperationType='新增' or OperationType='更新') and SUBSTRING(adcd,5,2) > 0 and adcd like '%000000000' and tm > '" + stime + "' and tm < '" + etime + "' group by adcd order by adcd asc");
                        //_list = getCountyList(db, request, loglist, townList, loglistCounty, _adcd, _list);
                        //sm.LSTownStatics = _list;
                        if (_adcd.Contains("00000000000"))
                        {
                            loglistCounty = db.Select<LogStatisItem>("select adcd,COUNT(adcd) adcdount from LogInfo where (OperationType='新增' or OperationType='更新') and Operation not like '%审核%' and SUBSTRING(adcd,5,2) > 0 and adcd like '%000000000' and tm >= '" + stime + "' and tm <= '" + etime + "' group by adcd order by adcd asc");
                            sm.adnm = db.Single<ADCDInfo>(w => w.adcd == _adcd).adnm;
                            _list = getCountyList(db, request, loglist, townList, loglistCounty, _adcd, _list);
                        }
                        else if (_adcd.Contains("000000"))
                        {
                             parentid = _adcd.Substring(0, 4) + "00000000000";
                            adcdlist = db.Select<ADCDInfo>(w => w.adcd == _adcd || w.adcd == parentid);
                            //县名
                            sm.GradeNameCounty = adcdlist.Single(w => w.adcd == _adcd).adnm;
                            //市名
                            sm.GradeName = adcdlist.Single(w => w.adcd == parentid).adnm;
                            //镇列表
                            _list = getTownList(db, request, loglist, townList, loglistCounty, _adcd, stime, etime, _list);
                        }else { }
                        sm.LSTownStatics = _list;
                        break;
                    case 3:
                        var pid = _adcd.Substring(0, 4) + "00000000000";
                        var f = db.Select<ADCDInfo>(w => w.adcd == _adcd || w.adcd == pid);
                        //县名
                        sm.GradeNameCounty = f.Single(w => w.adcd == _adcd).adnm;
                        //市名
                        sm.GradeName = f.Single(w => w.adcd == pid).adnm;
                        //镇列表
                        _list = getTownList(db, request, loglist, townList, loglistCounty, _adcd, stime, etime, _list);

                        sm.LSTownStatics = _list;
                        break;
                }
                return sm;
            }
        }

        public List<LogTownStatisItem> getCountyList(System.Data.IDbConnection db, GetLogStatisList request, List<LogStatisItem> loglist, List<ADCDInfo> townList, List<LogStatisItem> loglistCounty, string _adcd, List<LogTownStatisItem> _list)
        {
            LogTownStatisItem ns = null;
            var countlist = db.Select<ADCDInfo>(w => w.adcd.EndsWith("000000000") && w.adcd.StartsWith(_adcd.Substring(0, 4)) && w.adcd != _adcd).OrderBy(o => o.adcd).ToList();
            _list = new List<LogTownStatisItem>();
            countlist.ForEach(w =>
            {
                ns = new LogTownStatisItem();
                var fcitycount = loglist.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 6))).ToList();
                ns.townUpdateNum = fcitycount == null ? 0 : fcitycount.Count;
                ns.ADNM = w.adnm;
                ns.ADCD = w.adcd;
                var ftown = townList.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 6))).ToList();
                ns.townAllNum = ftown == null ? 0 : ftown.Count;
                ns.countyIfUpdate = loglistCounty.Count == 0 ? 0 : loglistCounty.Find(x => x.adcd == w.adcd) != null ? 1 : 0;
                _list.Add(ns);
            });
            return _list;
        }

        public List<LogTownStatisItem> getTownList(System.Data.IDbConnection db, GetLogStatisList request, List<LogStatisItem> loglist, List<ADCDInfo> townList, List<LogStatisItem> loglistCounty, string _adcd, string stime, string etime, List<LogTownStatisItem> _list)
        {
            LogTownStatisItem ns = null; //330102000000000
            var towlist = db.Select<ADCDInfo>(w => w.adcd.StartsWith(_adcd.Substring(0, 6)) && w.grade == 3 && w.adcd != _adcd).OrderBy(o => o.adcd).ToList();
            _list = new List<LogTownStatisItem>();
            towlist.ForEach(w =>
            {
                ns = new LogTownStatisItem();
                ns.ADCD = w.adcd;
                ns.ADNM = w.adnm;
                var r = getTownPersonNum(db, w.adcd, stime, etime);
                if (r != null)
                {
                    ns.townPersonNum = r.townPersonNum;
                    ns.workgroupPersonNum = r.workgroupPersonNum;
                    ns.gridPersonNum = r.gridPersonNum;
                    ns.transferPersonNum = r.transferPersonNum;
                    ns.picPersonNum = r.picPersonNum;
                }

                //var a = db.SqlList<LogTownStatisItem>("EXEC LogTownPStatics @stime,@etime,@townadcd,@adcd", new { stime = stime, etime = etime, townadcd = adcd.Substring(0, 9), adcd = adcd }).ToList().FirstOrDefault();
                //ns.townPersonNum = a != null ?  a.townPersonNum : 0; 
                //var b = db.SqlList<LogTownStatisItem>("EXEC LogWorkingGroupPStatics @stime,@etime,@townadcd,@adcd", new { stime = stime, etime = etime, townadcd = adcd.Substring(0, 9), adcd = adcd }).ToList().FirstOrDefault();
                //ns.workgroupPersonNum = b != null ? b.workgroupPersonNum:0;
                //var c = db.SqlList<LogTownStatisItem>("EXEC LogGridPStatics @stime,@etime,@townadcd,@adcd", new { stime = stime, etime = etime, townadcd = adcd.Substring(0, 9), adcd = adcd }).ToList().FirstOrDefault();
                //ns.gridPersonNum = c != null ? c.gridPersonNum : 0;
                //var d = db.SqlList<LogTownStatisItem>("EXEC LogTransferPStatics @stime,@etime,@townadcd,@adcd", new { stime = stime, etime = etime, townadcd = adcd.Substring(0, 9), adcd = adcd }).ToList().FirstOrDefault();
                //ns.transferPersonNum =d != null ?  d.transferPersonNum:0;
                //var e = db.SqlList<LogTownStatisItem>("EXEC LogPicPStatics @stime,@etime,@townadcd,@adcd", new { stime = stime, etime = etime, townadcd = adcd.Substring(0, 9), adcd = adcd }).ToList().FirstOrDefault();
                //ns.picPersonNum = e != null ? e.picPersonNum : 0;

                _list.Add(ns);
            });
            return _list;
        }
        public LogTownStatisItem getTownPersonNum(System.Data.IDbConnection db, string adcd, string stime, string etime)
        {
            //var sql = "select z.adcd,z.adnm,a.townPersonNum,b.workgroupPersonNum,c.gridPersonNum,d.transferPersonNum,e.picPersonNum from adcdinfo z left join"
            //    +" (select adcd, COUNT(adcd) townPersonNum from LogInfo where adcd like '" + adcd.Substring(0, 9) + "%' and(OperationType = '新增' or OperationType = '更新') and Operation like '%防汛防台责任人%' and tm > '" + stime + "' and tm < '" + etime + "' group by adcd) a"
            //    +" on z.adcd = a.adcd left join"
            //    +" (select adcd, COUNT(adcd) workgroupPersonNum from LogInfo where adcd like '" + adcd.Substring(0, 9) + "%' and(OperationType = '新增' or OperationType = '更新') and Operation like '%防汛防台工作组%' and tm > '" + stime + "' and tm < '" + etime + "' group by adcd ) b"
            //    +" on z.adcd = b.adcd left join"
            //    +" (select adcd, COUNT(adcd) gridPersonNum from LogInfo where adcd like '" + adcd.Substring(0, 9) + "%' and(OperationType = '新增' or OperationType = '更新') and Operation like '%网格责任人%' and tm > '" + stime + "' and tm < '" + etime + "' group by adcd ) c"
            //    +" on z.adcd = c.adcd left join"
            //    +" (select adcd, COUNT(adcd) transferPersonNum from LogInfo where adcd like '" + adcd.Substring(0, 9) + "%' and(OperationType = '新增' or OperationType = '更新') and Operation like '%转移人员清单%' and tm > '" + stime + "' and tm < '" + etime + "' group by adcd ) d"
            //    +" on z.adcd = d.adcd left join"
            //    +" (select adcd, COUNT(adcd) picPersonNum from LogInfo where adcd like '" + adcd.Substring(0, 9) + "%' and(OperationType = '新增' or OperationType = '更新') and Operation like '%形势图%' and tm > '" + stime + "' and tm < '" + etime + "' group by adcd ) e"
            //    +" on z.adcd = e.adcd where z.adcd = '"+adcd+"'";
            // return db.Single<LogTownStatisItem>(sql);
            return db.SqlList<LogTownStatisItem>("EXEC LogStatics @stime,@etime,@townadcd,@adcd", new { stime = stime, etime = etime, townadcd= adcd.Substring(0, 9) ,adcd=adcd}).ToList().FirstOrDefault();
        }

        #endregion

        #region 镇村列表
        public BsTableDataSource<LogInfoViewModel> GetLogList(GetLogList request)
        {
            if (string.IsNullOrEmpty(request.typename)) throw new Exception("类型不能为空");
            BsTableDataSource<LogInfoViewModel> rlog = new BsTableDataSource<LogInfoViewModel>();
            switch (request.typename)
            {
                case "townperson":
                    rlog = GetLogList(request, "防汛防台责任人");
                    break;
                case "groupperson":
                    rlog = GetLogList(request, "防汛防台工作组");
                    break;
                case "gridperson":
                    rlog = GetLogList(request, "网格责任人");
                    break;
                case "transferperson":
                    rlog = GetLogList(request, "转移人员清单");
                    break;
                case "picperson":
                    rlog = GetLogList(request, "形势图");
                    break;
            }
            return rlog;
        }
        public BsTableDataSource<LogInfoViewModel> GetLogList(GetLogList request, string typename)
        {
            using (var db = DbFactory.Open())
            {
                var stime = ""; var etime = "";
                if (string.IsNullOrEmpty(request.starttime) && string.IsNullOrEmpty(request.starttime))
                {
                    stime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    etime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    stime = request.starttime;
                    etime = request.endtime;
                }
                var builder = db.From<LogInfo>();
              
                builder.Where(w => w.adcd == request.adcd && w.Operation.Contains(typename) && (w.OperationType == OperationTypeEnums.新增 || w.OperationType == OperationTypeEnums.更新));
                builder.Where(w => w.tm > Convert.ToDateTime(stime) && w.tm < Convert.ToDateTime(etime));
                var count = db.Select(builder).Count;

                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(o => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                    builder.OrderByDescending(o => request.Sort);
                else
                    builder.OrderByDescending(o => o.tm);

                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : request.PageIndex * PageSize;
                builder.Limit(PageIndex, PageSize);
                var _list = db.Select<LogInfoViewModel>(builder);

                return new BsTableDataSource<LogInfoViewModel>() { rows = _list, total = count };
            }
        }

        public BaseResult GetAuditDate(GetAuditDate request)
        {
            BaseResult br = new BaseResult();
            using (var db = DbFactory.Open())
            {
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                var f_town = db.Single<Model.Audit.Audit>(w=>w.TownADCD == request.adcd && w.Year == _year );
                if(f_town != null)
                {
                   if(f_town.Status == 0 || f_town.Status == -1)
                    {
                        //不通过
                        var f = db.Select<LogInfo>(w => w.adcd.EndsWith("000000000") && w.adcd.StartsWith(request.adcd.Substring(0, 4)) && (w.Operation.Contains("市级审核") || w.Operation.Contains("县级审核")) && w.Operation.Contains(request.adnm));
                        var city = f.Where(w => w.adcd == request.adcd.Substring(0, 4) + "00000000000").OrderByDescending(o => o.tm).FirstOrDefault();
                        var county = f.Where(w => w.adcd == request.adcd.Substring(0, 6) + "000000000").OrderByDescending(o => o.tm).FirstOrDefault();
                        if (city != null)
                        {
                            br.ErrorMsg = city.tm.ToString();
                        }
                        if (county != null) { br.Others = county.tm.ToString(); }
                    }else
                    {
                        if (!string.IsNullOrEmpty(f_town.CountyAuditTime.ToString())) br.Others = f_town.CountyAuditTime.ToString();
                        if (!string.IsNullOrEmpty(f_town.CityAuditTime.ToString())) br.ErrorMsg = f_town.CityAuditTime.ToString();
                    }
                }
            }
            return br;
        }
        #endregion
    }
}
