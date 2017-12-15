using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.ServiceModel.CApp;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.ServiceModel.Village;
using GrassrootsFloodCtrl.Model.DangerZone;
using GrassrootsFloodCtrl.ServiceModel.Country;
using GrassrootsFloodCtrl.Model.CountryPerson;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.Model.Town;
using GrassrootsFloodCtrl.Model.AppApi;

namespace GrassrootsFloodCtrl.Logic.StatisAnalysis
{
    public class StatisAnalysisManager : ManagerBase, IStatisAnalysisManager
    {
        #region 审批统计
        public BsTableDataSource<StatisAnalysisViewModel> GetStatisAnalysisList(GetStatisAnalysisList request)
        {
            List<StatisAnalysisViewModel> _list = new List<StatisAnalysisViewModel>();
            List<ADCDInfo> f = null;
            using (var db = DbFactory.Open())
            {
                switch (RowID)
                {
                    case 5: //省级用户
                        f = db.Select<ADCDInfo>(w => w.adcd.EndsWith("000000") && w.adcd != "330000000000000").OrderBy(w => w.adcd).ToList();
                        break;
                    case 2: //市级用户
                        f = db.Select<ADCDInfo>(w => w.adcd.EndsWith("000000") && w.adcd.StartsWith(adcd.Substring(0, 4)) && w.adcd != "330000000000000").OrderBy(w => w.adcd).ToList();
                        break;
                }
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                var a = db.Select<Model.Audit.Audit>(w => w.Year == _year).ToList();
                //市
                var cityList = f.Where(w => w.adcd.EndsWith("00000000000")).ToList();
                var num = 0;
                List<int> numList = new List<int>();
                cityList.ForEach(w =>
                {
                    //县
                    var countyList = f.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 4)) && x.adcd != w.adcd.Substring(0, 4) + "00000000000" && x.adcd.EndsWith("000000000")).ToList();
                    StatisAnalysisViewModel _model = null;
                    var nums = 1;
                    countyList.ForEach(x =>
                    {
                        _model = new StatisAnalysisViewModel();
                        _model.cityName = w.adnm;
                        _model.countyName = x.adnm;
                        _model.townNum = f.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.adcd.EndsWith("000000") && y.adcd != x.adcd).Count();
                        _model.ApprovalStatusN1 = a.Where(y => y.TownADCD.StartsWith(x.adcd.Substring(0, 6)) && y.Status == -1).Count();
                        _model.ApprovalStatus0 = a.Where(y => y.TownADCD.StartsWith(x.adcd.Substring(0, 6)) && y.Status == 0).Count();
                        _model.ApprovalStatus1 = a.Where(y => y.TownADCD.StartsWith(x.adcd.Substring(0, 6)) && y.Status == 1).Count();
                        _model.ApprovalStatus2 = a.Where(y => y.TownADCD.StartsWith(x.adcd.Substring(0, 6)) && y.Status == 2).Count();
                        _model.ApprovalStatus3 = a.Where(y => y.TownADCD.StartsWith(x.adcd.Substring(0, 6)) && y.Status == 3).Count();
                        _list.Add(_model);
                        nums++;
                    });
                    //合计
                    _model = new StatisAnalysisViewModel();
                    _model.cityName = w.adnm;
                    _model.countyName = "小计";
                    _model.townNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.townNum);
                    _model.ApprovalStatusN1 = _list.Where(x => x.cityName == w.adnm).Sum(y => y.ApprovalStatusN1);
                    _model.ApprovalStatus0 = _list.Where(x => x.cityName == w.adnm).Sum(y => y.ApprovalStatus0);
                    _model.ApprovalStatus1 = _list.Where(x => x.cityName == w.adnm).Sum(y => y.ApprovalStatus1);
                    _model.ApprovalStatus2 = _list.Where(x => x.cityName == w.adnm).Sum(y => y.ApprovalStatus2);
                    _model.ApprovalStatus3 = _list.Where(x => x.cityName == w.adnm).Sum(y => y.ApprovalStatus3);

                    if (num == 0) { _model.numstr = "0," + nums; numList.Add(nums); }
                    else
                    {
                        _model.numstr = numList[num - 1] + "," + nums; numList.Add(numList[num - 1] + nums);
                    }
                    num++;
                    _list.Add(_model);
                });
                if(cityList.Count >1)
                {
                    StatisAnalysisViewModel statisModel = new StatisAnalysisViewModel();
                    statisModel.cityName = "全省";
                    statisModel.countyName = "汇总";
                    statisModel.townNum = _list.Where(x => x.countyName == "小计").Sum(y => y.townNum); //乡镇总数
                    statisModel.ApprovalStatusN1 = _list.Where(x => x.countyName == "小计").Sum(y => y.ApprovalStatusN1);
                    statisModel.ApprovalStatus0 = _list.Where(x => x.countyName == "小计").Sum(y => y.ApprovalStatus0);
                    statisModel.ApprovalStatus1 = _list.Where(x => x.countyName == "小计").Sum(y => y.ApprovalStatus1);
                    statisModel.ApprovalStatus2 = _list.Where(x => x.countyName == "小计").Sum(y => y.ApprovalStatus2);
                    statisModel.ApprovalStatus3 = _list.Where(x => x.countyName == "小计").Sum(y => y.ApprovalStatus3);
                    statisModel.numstr = (numList[num - 1]) + "," + 1;
                    _list.Add(statisModel);

                }

            }
            return new BsTableDataSource<StatisAnalysisViewModel>() { total = _list.Count, rows = _list };
        }

        #endregion
        #region 类型统计 责任人统计
        public BsTableDataSource<StatisTypeInfo> GetStatisCountyPerson(GetStatisCountyPerson request)
        {
            List<StatisTypeInfo> _list = null;

            using (var db = DbFactory.Open())
            {
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                _list = new List<StatisTypeInfo>();
                List<ADCDInfo> cityList = null; List<ADCDInfo> countyList1 = null;
                if (!string.IsNullOrEmpty(request.cityadcd))
                {
                    cityList = db.Select<ADCDInfo>(w => w.adcd == request.cityadcd);
                    countyList1 = db.Select<ADCDInfo>(w => w.adcd.EndsWith("000000000") && !w.adcd.EndsWith("00000000000") && w.adcd.StartsWith(request.cityadcd.Substring(0, 4)) && w.adcd != "330000000000000").ToList();
                }
                else
                {
                    switch (RowID)
                    {
                        case 5:
                            cityList = db.Select<ADCDInfo>(w => w.adcd.EndsWith("00000000000") && w.adcd != "330000000000000");
                            countyList1 = db.Select<ADCDInfo>(w => w.adcd.EndsWith("000000000") && !w.adcd.EndsWith("00000000000") && w.adcd != "330000000000000").ToList();
                            break;
                        case 2:
                            cityList = db.Select<ADCDInfo>(w => w.adcd == adcd);
                            countyList1 = db.Select<ADCDInfo>(w => w.adcd.EndsWith("000000000") && !w.adcd.EndsWith("00000000000") && w.adcd.StartsWith(adcd.Substring(0, 4)) && w.adcd != "330000000000000").ToList();
                            break;
                        case 3:
                            // _list = getList();
                            break;
                    }

                }
                var countyPersonL = db.Select<CountryPerson>(w => w.Year == _year);
                var num = 0;
                List<int> numList = new List<int>();
                cityList.ForEach(w =>
                {
                    //县
                    var countyList = countyList1.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 4))).ToList();
                    StatisTypeInfo _model = null;
                    var nums = 1;
                    countyList.ForEach(x =>
                    {
                        _model = new StatisTypeInfo();
                        _model.cityName = w.adnm;
                        _model.countyName = x.adnm;
                        _model.countyadcd = x.adcd;
                        _model.personLiableNum = countyPersonL.Where(y => y.adcd == x.adcd).Count();
                        _list.Add(_model);
                        nums++;
                    });
                    //合计
                    _model = new StatisTypeInfo();
                    _model.cityName = w.adnm;
                    _model.countyName = "小计";
                    _model.personLiableNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.personLiableNum);
                    if (num == 0) { _model.numstr = "0," + nums; numList.Add(nums); }
                    else
                    {
                        _model.numstr = numList[num - 1] + "," + nums; numList.Add(numList[num - 1] + nums);
                    }
                    num++;
                    _list.Add(_model);
                });
            }
            return new BsTableDataSource<StatisTypeInfo>() { total = _list.Count, rows = _list };
        }

        #endregion
        #region 类型统计
        /// <summary>
        /// 综合应用类型统计
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<CAppStaticsViewModel> Statics(Statistics request)
        {
            // if (request.adcdtype == null && string.IsNullOrEmpty(request.adcd)) throw new Exception("参数异常");
            List<CAppStaticsViewModel> _list = new List<CAppStaticsViewModel>();
            using (var db = DbFactory.Open())
            {
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                var _adcd = request.adcd;
                var _tid = request.adcdtype == null ? RowID : request.adcdtype;
                switch (_tid)
                {
                    case 5:
                        //省
                        _list = GetStaticsList(_year);
                        break;
                    case 2:
                        //市"
                        _list = GetStaticsList1(4, _adcd, _year);
                        break;
                    case 3:
                        //县
                        _list = GetStaticsList1(6, _adcd, _year);
                        break;
                    case 4:
                        //镇
                        _list = GetStaticsList1(9, _adcd, _year);
                        break;
                }
            }
            return _list;
        }

        /// <summary>
        /// 获取危险区某一个类型的责任人信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<VillageTransferPersonViewModel> GetStatisTypeInfoOneWXQ(GetStatisTypeInfoOneWXQ request)
        {
            using (var db = DbFactory.Open())
            {
                List<VillageTransferPersonViewModel> RList = null;
                if (string.IsNullOrEmpty(request.adcd)) throw new Exception("adcd不能为空");
                if (string.IsNullOrEmpty(request.typename)) throw new Exception("类型名不能为空");
                var builder = db.From<VillageTransferPerson>();
                builder.LeftJoin<VillageTransferPerson, ADCDInfo>((x, y) => x.adcd == y.adcd);
                switch (request.type)
                {
                    case "personLiable":
                        builder.Where(w => w.DangerZoneType == request.typename && w.PersonLiableName != "" && w.PersonLiableName != null);
                        break;
                    case "warnPersonLiable":
                        builder.Where(w => w.DangerZoneType == request.typename && w.WarnPersonLiableName != "" && w.WarnPersonLiableName != null);
                        break;
                    case "disasterPreventionManager":
                        builder.Where(w => w.DangerZoneType == request.typename && w.DisasterPreventionManager != "" && w.DisasterPreventionManager != null);
                        break;
                }
                if (request.adcd.Contains("000000000")) { builder.Where(w => w.adcd.StartsWith(request.adcd.Substring(0, 6))); }
                else { builder.Where(w => w.adcd.StartsWith(request.adcd.Substring(0, 9))); }
                builder.Select("VillageTransferPerson.*,ADCDInfo.adnm");
                var count = db.Select(builder).Count;
                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                builder.Limit(PageIndex, PageSize);
                RList = db.Select<VillageTransferPersonViewModel>(builder);

                return new BsTableDataSource<VillageTransferPersonViewModel>() { total = count, rows = RList };
            }
        }
        /// <summary>
        /// 获取网格某一个类型的责任人信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<VillageGridViewModel> GetStatisTypeInfoOneWG(GetStatisTypeInfoOneWG request)
        {
            using (var db = DbFactory.Open())
            {
                List<VillageGridViewModel> RList = null;
                if (string.IsNullOrEmpty(request.adcd)) throw new Exception("adcd不能为空");
                if (string.IsNullOrEmpty(request.typename)) throw new Exception("类型名不能为空");
                var builder = db.From<VillageGridPersonLiable>();
                builder.LeftJoin<VillageGridPersonLiable, ADCDInfo>((x, y) => x.VillageADCD == y.adcd);
                builder.Where(w => w.GridName == request.typename && w.PersonLiable != "" && w.PersonLiable != null);

                if (request.adcd.Contains("000000000")) { builder.Where(w => w.VillageADCD.StartsWith(request.adcd.Substring(0, 6))); }
                else { builder.Where(w => w.VillageADCD.StartsWith(request.adcd.Substring(0, 9))); }
                builder.Select("VillageGridPersonLiable.*,ADCDInfo.adnm");
                var count = db.Select(builder).Count;
                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                builder.Limit(PageIndex, PageSize);
                RList = db.Select<VillageGridViewModel>(builder);

                return new BsTableDataSource<VillageGridViewModel>() { total = count, rows = RList };
            }
        }
        /// <summary>
        /// 网格类型中，不同分类的数据获取
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<StatisTypeInfo> GetStatisTypeInfoList(GetStatisTypeInfoList request)
        {
            List<StatisTypeInfo> _list = null;

            using (var db = DbFactory.Open())
            {
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                var builderAdcdinfo = db.From<ADCDInfo>();
                if (!string.IsNullOrEmpty(request.cityname))
                {
                    builderAdcdinfo.Where(w => w.adcd.StartsWith(request.cityname.Substring(0, 4)) && w.adcd != "330000000000000");
                }
                else
                {
                    switch (RowID)
                    {
                        case 5:
                            builderAdcdinfo.Where(w => w.adcd.EndsWith("000000") && w.adcd != "330000000000000");
                            break;
                        case 2:
                            builderAdcdinfo.Where(w => w.adcd.EndsWith("000000") && w.adcd.StartsWith(adcd.Substring(0, 4)));
                            break;
                        case 3:
                            //builderAdcdinfo.Where(w => w.adcd.EndsWith("000000") && w.adcd != "330000000000000");
                            break;
                    }

                }
                var f = db.Select<ADCDInfo>(builderAdcdinfo);
                List<int> numList = null; _list = new List<StatisTypeInfo>();
                if (request.type == "wxq")
                {
                    //#region 危险区
                    var a = db.Select<VillageTransferPerson>(w => w.Year == _year && w.DangerZoneType == request.typename).ToList();
                    var cityList = f.Where(w => w.adcd.EndsWith("00000000000")).OrderBy(w => w.adcd).ToList();
                    var num = 0;
                    numList = new List<int>();
                    cityList.ForEach(w =>
                    {
                        //县
                        var countyList = f.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 4)) && x.adcd != w.adcd.Substring(0, 4) + "00000000000" && x.adcd.EndsWith("000000000")).OrderBy(x => x.adcd).ToList();
                        StatisTypeInfo _model = null;
                        var nums = 1;
                        countyList.ForEach(x =>
                        {
                            _model = new StatisTypeInfo();
                            _model.cityName = w.adnm;
                            _model.countyName = x.adnm;
                            _model.countyadcd = x.adcd;
                            _model.townnum = f.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.adcd.EndsWith("000000") && y.adcd != x.adcd).Count();
                            _model.householderNum = a.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6))).Sum(y => y.HouseholderNum);
                            _model.personLiableNum = a.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.PersonLiableName != "").GroupBy(y => new { y.adcd, y.PersonLiableName }).Count();
                            _model.warnPersonLiableNum = a.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.WarnPersonLiableName != "").GroupBy(y => new { y.WarnPersonLiableName, y.adcd }).Count();
                            _model.disasterPreventionManagerNum = a.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.DisasterPreventionManager != "").GroupBy(y => new { y.DisasterPreventionManager, y.adcd }).Count();
                            _model.typeNum = a.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6))).Count();
                            _list.Add(_model);
                            nums++;
                        });
                        //合计
                        _model = new StatisTypeInfo();
                        _model.cityName = w.adnm;
                        _model.countyName = "小计";
                        _model.townnum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.townnum);
                        _model.householderNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.householderNum);
                        _model.personLiableNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.personLiableNum);
                        _model.warnPersonLiableNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.warnPersonLiableNum);
                        _model.disasterPreventionManagerNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.disasterPreventionManagerNum);
                        _model.typeNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.typeNum);
                        if (num == 0) { _model.numstr = "0," + nums; numList.Add(nums); }
                        else
                        {
                            _model.numstr = numList[num - 1] + "," + nums; numList.Add(numList[num - 1] + nums);
                        }
                        num++;
                        _list.Add(_model);
                    });
                    if (cityList.Count > 1)
                    {
                        StatisTypeInfo _modeltotal = new StatisTypeInfo();
                        _modeltotal.cityName = "全省";
                        _modeltotal.countyName = "汇总";
                        _modeltotal.townnum = _list.Where(x => x.countyName == "小计").Sum(y => y.townnum);
                        _modeltotal.householderNum = _list.Where(x => x.countyName == "小计").Sum(y => y.householderNum);
                        _modeltotal.personLiableNum = _list.Where(x => x.countyName == "小计").Sum(y => y.personLiableNum);
                        _modeltotal.warnPersonLiableNum = _list.Where(x => x.countyName == "小计").Sum(y => y.warnPersonLiableNum);
                        _modeltotal.disasterPreventionManagerNum = _list.Where(x => x.countyName == "小计").Sum(y => y.disasterPreventionManagerNum);
                        _modeltotal.typeNum = _list.Where(x => x.countyName == "小计").Sum(y => y.typeNum);
                        _modeltotal.numstr = (numList[num - 1]) + "," + 1;
                        _list.Add(_modeltotal);
                    }
                    //#endregion

                    #region 危险区
                    //var a = db.Select<VillageTransferPerson>(w => w.Year == _year && w.DangerZoneType == request.typename).ToList();
                    //var cityList = f.Where(w => w.adcd.EndsWith("00000000000")).ToList();
                    //var sql = "select H.adcd,h.adnm cityName,I.adcd as countyadcd ,I.countyName,I.personLiableNum,I.warnPersonLiableNum,I.disasterPreventionManagerNum from ADCDInfo H left join ("
                    //           + "select T.adcd,T.adnm countyName,T.personLiableNum,F.warnPersonLiableNum,G.disasterPreventionManagerNum from "
                    //           + "("
                    //           + "select A.adcd, A.adnm, COUNT(B.PersonLiableName) as personLiableNum  from"
                    //           + "(select adcd, adnm from ADCDInfo  where CONVERT(int, SUBSTRING(adcd, 5, 2)) > 0 and  adcd like '%000000000') A"
                    //           + " left join("
                    //           + "select adcd, PersonLiableName, COUNT(*) as C from VillageTransferPerson where YEAR = " + _year + " and DangerZoneType = '" + request.typename + "' and PersonLiableName != '' group by adcd, PersonLiableName"
                    //           + ") B on SUBSTRING(A.adcd, 1, 6) = SUBSTRING(B.adcd, 1, 6) group by A.adcd, A.adnm"
                    //           + ") T"
                    //           + " left join"
                    //           + "("
                    //           + "select A.adcd, A.adnm, COUNT(B.WarnPersonLiableName) as warnPersonLiableNum  from"
                    //           + "(select adcd, adnm from ADCDInfo  where CONVERT(int, SUBSTRING(adcd, 5, 2)) > 0 and  adcd like '%000000000') A"
                    //           + " left join(select adcd, WarnPersonLiableName, COUNT(*) as C from VillageTransferPerson where YEAR = " + _year + " and DangerZoneType = '" + request.typename + "' and WarnPersonLiableName != '' group by adcd, WarnPersonLiableName"
                    //           + ") B on SUBSTRING(A.adcd, 1, 6) = SUBSTRING(B.adcd, 1, 6) group by A.adcd, A.adnm"
                    //           + ") F on T.adcd = F.adcd"
                    //           + " left join"
                    //           + "("
                    //           + "select A.adcd, A.adnm, COUNT(B.DisasterPreventionManager) as disasterPreventionManagerNum  from"
                    //           + "(select adcd, adnm from ADCDInfo  where CONVERT(int, SUBSTRING(adcd, 5, 2)) > 0 and  adcd like '%000000000') A"
                    //           + " left join(select adcd, DisasterPreventionManager, COUNT(*) as C from VillageTransferPerson where YEAR = " + _year + " and DangerZoneType = '" + request.typename + "' and DisasterPreventionManager != '' group by adcd, DisasterPreventionManager) B"
                    //           + " on SUBSTRING(A.adcd, 1, 6) = SUBSTRING(B.adcd, 1, 6) group by A.adcd, A.adnm"
                    //           + ") G on T.adcd = G.adcd ) I on SUBSTRING(H.adcd,1,4) = SUBSTRING(I.adcd,1,4)"
                    //           + "where SUBSTRING(H.adcd,1,4) > 0 and H.adcd like '%00000000000' and H.adcd != '330000000000000' order by H.adcd,I.adcd asc";
                    //var countyList = db.Select<StatisTypeInfo>(sql);
                    //StatisTypeInfo _model = null;
                    //var i = 0;
                    //countyList.ForEach(w => {
                    //    var b = _list.Exists(x => x.cityName == w.cityName);
                    //    if (!b && _list.Count > 1)
                    //    {
                    //        var cityname = _list[i - 1].cityName;
                    //        //合计
                    //        _model = new StatisTypeInfo();
                    //        _model.cityName = cityname;
                    //        _model.countyName = "小计";
                    //        _model.townnum = _list.Where(x => x.cityName == cityname).Sum(y => y.townnum);
                    //        _model.householderNum = _list.Where(x => x.cityName == cityname).Sum(y => y.householderNum);
                    //        _model.personLiableNum = _list.Where(x => x.cityName == cityname).Sum(y => y.personLiableNum);
                    //        _model.warnPersonLiableNum = _list.Where(x => x.cityName == cityname).Sum(y => y.warnPersonLiableNum);
                    //        _model.disasterPreventionManagerNum = _list.Where(x => x.cityName == cityname).Sum(y => y.disasterPreventionManagerNum);
                    //        _model.typeNum = _list.Where(x => x.cityName == cityname).Sum(y => y.typeNum);
                    //        _list.Add(_model);
                    //    }
                    //    //县
                    //    _model = new StatisTypeInfo();
                    //    _model.cityName = w.cityName;
                    //    _model.countyName = w.countyName;
                    //    _model.townnum = f.Where(y => y.adcd.StartsWith(w.countyadcd.Substring(0, 6)) && y.adcd.EndsWith("000000") && y.adcd != w.adcd).Count();
                    //    _model.householderNum = a.Where(y => y.adcd.StartsWith(w.countyadcd.Substring(0, 6))).Sum(y => y.HouseholderNum);
                    //    _model.personLiableNum = w.personLiableNum;//a.Where(y => y.adcd.StartsWith(x.adcd.Substring(1, 6)) && y.PersonLiableName != "").Count();
                    //    _model.warnPersonLiableNum = w.warnPersonLiableNum; //a.Where(y => y.adcd.StartsWith(x.adcd.Substring(1, 6)) && y.WarnPersonLiableName != "").Count();
                    //    _model.disasterPreventionManagerNum = w.disasterPreventionManagerNum;//a.Where(y => y.adcd.StartsWith(x.adcd.Substring(1, 6)) && y.DisasterPreventionManager != "").Count();
                    //    _model.typeNum = a.Where(y => y.adcd.StartsWith(w.countyadcd.Substring(0, 6))).Count();
                    //    _list.Add(_model);
                    //    i++;

                    //});
                    //if (cityList.Count > 1)
                    //{
                    //    StatisTypeInfo _modeltotal = new StatisTypeInfo();
                    //    _modeltotal.cityName = "";
                    //    _modeltotal.countyName = "汇总";
                    //    _modeltotal.townnum = _list.Where(x => x.countyName == "小计").Sum(y => y.townnum);
                    //    _modeltotal.householderNum = _list.Where(x => x.countyName == "小计").Sum(y => y.householderNum);
                    //    _modeltotal.personLiableNum = _list.Where(x => x.countyName == "小计").Sum(y => y.personLiableNum);
                    //    _modeltotal.warnPersonLiableNum = _list.Where(x => x.countyName == "小计").Sum(y => y.warnPersonLiableNum);
                    //    _modeltotal.disasterPreventionManagerNum = _list.Where(x => x.countyName == "小计").Sum(y => y.disasterPreventionManagerNum);
                    //    _modeltotal.typeNum = _list.Where(x => x.countyName == "小计").Sum(y => y.typeNum);
                    //    _list.Add(_modeltotal);
                    //}
                    #endregion
                }
                else
                {
                    #region 网格
                    var a = db.Select<VillageGridPersonLiable>(w => w.Year == _year && w.GridName == request.typename).ToList();
                    var cityList = f.Where(w => w.adcd.EndsWith("00000000000")).OrderBy(w => w.adcd).ToList();
                    var num = 0;
                    numList = new List<int>();
                    cityList.ForEach(w =>
                    {
                        //县
                        var countyList = f.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 4)) && x.adcd != w.adcd.Substring(0, 4) + "00000000000" && x.adcd.EndsWith("000000000")).OrderBy(x => x.adcd).ToList();
                        StatisTypeInfo _model = null;
                        var nums = 1;
                        countyList.ForEach(x =>
                        {
                            _model = new StatisTypeInfo();
                            _model.cityName = w.adnm;
                            _model.countyName = x.adnm;
                            _model.countyadcd = x.adcd;
                            _model.townnum = f.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.adcd.EndsWith("000000") && y.adcd != x.adcd).Count();
                            _model.typeNum = a.Where(y => y.VillageADCD.StartsWith(x.adcd.Substring(0, 6))).Count();
                            _model.personLiableNum = a.Where(y => y.VillageADCD.StartsWith(x.adcd.Substring(0, 6)) && y.PersonLiable != "" && y.PersonLiable != null).GroupBy(y => y.PersonLiable).Count();
                            _list.Add(_model);
                            nums++;
                        });
                        //合计
                        _model = new StatisTypeInfo();
                        _model.cityName = w.adnm;
                        _model.countyName = "小计";
                        _model.townnum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.townnum);
                        _model.typeNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.typeNum);
                        _model.personLiableNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.personLiableNum);
                        if (num == 0) { _model.numstr = "0," + nums; numList.Add(nums); }
                        else
                        {
                            _model.numstr = numList[num - 1] + "," + nums; numList.Add(numList[num - 1] + nums);
                        }
                        num++;
                        _list.Add(_model);
                    });
                    if (cityList.Count > 1)
                    {
                        StatisTypeInfo _modeltotal = new StatisTypeInfo();
                        _modeltotal.cityName = "全省";
                        _modeltotal.countyName = "汇总";
                        _modeltotal.townnum = _list.Where(x => x.countyName == "小计").Sum(y => y.townnum);
                        _modeltotal.personLiableNum = _list.Where(x => x.countyName == "小计").Sum(y => y.personLiableNum);
                        _modeltotal.typeNum = _list.Where(x => x.countyName == "小计").Sum(y => y.typeNum);
                        _modeltotal.numstr = (numList[num - 1]) + "," + 1;
                        _list.Add(_modeltotal);
                    }
                    #endregion
                }
            }
            return new BsTableDataSource<StatisTypeInfo>() { rows = _list, total = _list.Count() };
        }
        public List<StatisTypeInfo> getStatisTypeInfoListProv(GetStatisTypeInfoList request)
        {
            using (var db = DbFactory.Open())
            {
                List<StatisTypeInfo> _list = new List<StatisTypeInfo>();
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                var builderAdcdinfo = db.From<ADCDInfo>();
                if (!string.IsNullOrEmpty(request.cityname))
                {
                    builderAdcdinfo.Where(w => w.adcd.StartsWith(request.cityname.Substring(0, 4)) && w.adcd != "330000000000000");
                }
                else
                {
                    builderAdcdinfo.Where(w => w.adcd.EndsWith("000000") && w.adcd != "330000000000000");
                }
                var f = db.Select<ADCDInfo>(builderAdcdinfo);
                List<int> numList = null;
                if (request.type == "wxq")
                {
                    //#region 危险区
                    var a = db.Select<VillageTransferPerson>(w => w.Year == _year && w.DangerZoneType == request.typename).ToList();
                    var cityList = f.Where(w => w.adcd.EndsWith("00000000000")).ToList();
                    var num = 0;
                    numList = new List<int>();
                    cityList.ForEach(w =>
                    {
                        //县
                        var countyList = f.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 4)) && x.adcd != w.adcd.Substring(0, 4) + "00000000000" && x.adcd.EndsWith("000000000")).ToList();
                        StatisTypeInfo _model = null;
                        var nums = 1;
                        countyList.ForEach(x =>
                        {
                            _model = new StatisTypeInfo();
                            _model.cityName = w.adnm;
                            _model.countyName = x.adnm;
                            _model.countyadcd = x.adcd;
                            _model.townnum = f.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.adcd.EndsWith("000000") && y.adcd != x.adcd).Count();
                            _model.householderNum = a.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6))).Sum(y => y.HouseholderNum);
                            _model.personLiableNum = a.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.PersonLiableName != "").GroupBy(y => new { y.adcd, y.PersonLiableName }).Count();
                            _model.warnPersonLiableNum = a.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.WarnPersonLiableName != "").GroupBy(y => new { y.WarnPersonLiableName, y.adcd }).Count();
                            _model.disasterPreventionManagerNum = a.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.DisasterPreventionManager != "").GroupBy(y => new { y.DisasterPreventionManager, y.adcd }).Count();
                            _model.typeNum = a.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6))).Count();
                            _list.Add(_model);
                            nums++;
                        });
                        //合计
                        _model = new StatisTypeInfo();
                        _model.cityName = w.adnm;
                        _model.countyName = "小计";
                        _model.townnum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.townnum);
                        _model.householderNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.householderNum);
                        _model.personLiableNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.personLiableNum);
                        _model.warnPersonLiableNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.warnPersonLiableNum);
                        _model.disasterPreventionManagerNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.disasterPreventionManagerNum);
                        _model.typeNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.typeNum);
                        if (num == 0) { _model.numstr = "0," + nums; numList.Add(nums); }
                        else
                        {
                            _model.numstr = numList[num - 1] + "," + nums; numList.Add(numList[num - 1] + nums);
                        }
                        num++;
                        _list.Add(_model);
                    });
                    if (cityList.Count > 1)
                    {
                        StatisTypeInfo _modeltotal = new StatisTypeInfo();
                        _modeltotal.cityName = "全省";
                        _modeltotal.countyName = "汇总";
                        _modeltotal.townnum = _list.Where(x => x.countyName == "小计").Sum(y => y.townnum);
                        _modeltotal.householderNum = _list.Where(x => x.countyName == "小计").Sum(y => y.householderNum);
                        _modeltotal.personLiableNum = _list.Where(x => x.countyName == "小计").Sum(y => y.personLiableNum);
                        _modeltotal.warnPersonLiableNum = _list.Where(x => x.countyName == "小计").Sum(y => y.warnPersonLiableNum);
                        _modeltotal.disasterPreventionManagerNum = _list.Where(x => x.countyName == "小计").Sum(y => y.disasterPreventionManagerNum);
                        _modeltotal.typeNum = _list.Where(x => x.countyName == "小计").Sum(y => y.typeNum);
                        _list.Add(_modeltotal);
                    }
                    //#endregion

                    #region 危险区
                    //var a = db.Select<VillageTransferPerson>(w => w.Year == _year && w.DangerZoneType == request.typename).ToList();
                    //var cityList = f.Where(w => w.adcd.EndsWith("00000000000")).ToList();
                    //var sql = "select H.adcd,h.adnm cityName,I.adcd as countyadcd ,I.countyName,I.personLiableNum,I.warnPersonLiableNum,I.disasterPreventionManagerNum from ADCDInfo H left join ("
                    //           + "select T.adcd,T.adnm countyName,T.personLiableNum,F.warnPersonLiableNum,G.disasterPreventionManagerNum from "
                    //           + "("
                    //           + "select A.adcd, A.adnm, COUNT(B.PersonLiableName) as personLiableNum  from"
                    //           + "(select adcd, adnm from ADCDInfo  where CONVERT(int, SUBSTRING(adcd, 5, 2)) > 0 and  adcd like '%000000000') A"
                    //           + " left join("
                    //           + "select adcd, PersonLiableName, COUNT(*) as C from VillageTransferPerson where YEAR = " + _year + " and DangerZoneType = '" + request.typename + "' and PersonLiableName != '' group by adcd, PersonLiableName"
                    //           + ") B on SUBSTRING(A.adcd, 1, 6) = SUBSTRING(B.adcd, 1, 6) group by A.adcd, A.adnm"
                    //           + ") T"
                    //           + " left join"
                    //           + "("
                    //           + "select A.adcd, A.adnm, COUNT(B.WarnPersonLiableName) as warnPersonLiableNum  from"
                    //           + "(select adcd, adnm from ADCDInfo  where CONVERT(int, SUBSTRING(adcd, 5, 2)) > 0 and  adcd like '%000000000') A"
                    //           + " left join(select adcd, WarnPersonLiableName, COUNT(*) as C from VillageTransferPerson where YEAR = " + _year + " and DangerZoneType = '" + request.typename + "' and WarnPersonLiableName != '' group by adcd, WarnPersonLiableName"
                    //           + ") B on SUBSTRING(A.adcd, 1, 6) = SUBSTRING(B.adcd, 1, 6) group by A.adcd, A.adnm"
                    //           + ") F on T.adcd = F.adcd"
                    //           + " left join"
                    //           + "("
                    //           + "select A.adcd, A.adnm, COUNT(B.DisasterPreventionManager) as disasterPreventionManagerNum  from"
                    //           + "(select adcd, adnm from ADCDInfo  where CONVERT(int, SUBSTRING(adcd, 5, 2)) > 0 and  adcd like '%000000000') A"
                    //           + " left join(select adcd, DisasterPreventionManager, COUNT(*) as C from VillageTransferPerson where YEAR = " + _year + " and DangerZoneType = '" + request.typename + "' and DisasterPreventionManager != '' group by adcd, DisasterPreventionManager) B"
                    //           + " on SUBSTRING(A.adcd, 1, 6) = SUBSTRING(B.adcd, 1, 6) group by A.adcd, A.adnm"
                    //           + ") G on T.adcd = G.adcd ) I on SUBSTRING(H.adcd,1,4) = SUBSTRING(I.adcd,1,4)"
                    //           + "where SUBSTRING(H.adcd,1,4) > 0 and H.adcd like '%00000000000' and H.adcd != '330000000000000' order by H.adcd,I.adcd asc";
                    //var countyList = db.Select<StatisTypeInfo>(sql);
                    //StatisTypeInfo _model = null;
                    //var i = 0;
                    //countyList.ForEach(w => {
                    //    var b = _list.Exists(x => x.cityName == w.cityName);
                    //    if (!b && _list.Count > 1)
                    //    {
                    //        var cityname = _list[i - 1].cityName;
                    //        //合计
                    //        _model = new StatisTypeInfo();
                    //        _model.cityName = cityname;
                    //        _model.countyName = "小计";
                    //        _model.townnum = _list.Where(x => x.cityName == cityname).Sum(y => y.townnum);
                    //        _model.householderNum = _list.Where(x => x.cityName == cityname).Sum(y => y.householderNum);
                    //        _model.personLiableNum = _list.Where(x => x.cityName == cityname).Sum(y => y.personLiableNum);
                    //        _model.warnPersonLiableNum = _list.Where(x => x.cityName == cityname).Sum(y => y.warnPersonLiableNum);
                    //        _model.disasterPreventionManagerNum = _list.Where(x => x.cityName == cityname).Sum(y => y.disasterPreventionManagerNum);
                    //        _model.typeNum = _list.Where(x => x.cityName == cityname).Sum(y => y.typeNum);
                    //        _list.Add(_model);
                    //    }
                    //    //县
                    //    _model = new StatisTypeInfo();
                    //    _model.cityName = w.cityName;
                    //    _model.countyName = w.countyName;
                    //    _model.townnum = f.Where(y => y.adcd.StartsWith(w.countyadcd.Substring(0, 6)) && y.adcd.EndsWith("000000") && y.adcd != w.adcd).Count();
                    //    _model.householderNum = a.Where(y => y.adcd.StartsWith(w.countyadcd.Substring(0, 6))).Sum(y => y.HouseholderNum);
                    //    _model.personLiableNum = w.personLiableNum;//a.Where(y => y.adcd.StartsWith(x.adcd.Substring(1, 6)) && y.PersonLiableName != "").Count();
                    //    _model.warnPersonLiableNum = w.warnPersonLiableNum; //a.Where(y => y.adcd.StartsWith(x.adcd.Substring(1, 6)) && y.WarnPersonLiableName != "").Count();
                    //    _model.disasterPreventionManagerNum = w.disasterPreventionManagerNum;//a.Where(y => y.adcd.StartsWith(x.adcd.Substring(1, 6)) && y.DisasterPreventionManager != "").Count();
                    //    _model.typeNum = a.Where(y => y.adcd.StartsWith(w.countyadcd.Substring(0, 6))).Count();
                    //    _list.Add(_model);
                    //    i++;

                    //});
                    //if (cityList.Count > 1)
                    //{
                    //    StatisTypeInfo _modeltotal = new StatisTypeInfo();
                    //    _modeltotal.cityName = "";
                    //    _modeltotal.countyName = "汇总";
                    //    _modeltotal.townnum = _list.Where(x => x.countyName == "小计").Sum(y => y.townnum);
                    //    _modeltotal.householderNum = _list.Where(x => x.countyName == "小计").Sum(y => y.householderNum);
                    //    _modeltotal.personLiableNum = _list.Where(x => x.countyName == "小计").Sum(y => y.personLiableNum);
                    //    _modeltotal.warnPersonLiableNum = _list.Where(x => x.countyName == "小计").Sum(y => y.warnPersonLiableNum);
                    //    _modeltotal.disasterPreventionManagerNum = _list.Where(x => x.countyName == "小计").Sum(y => y.disasterPreventionManagerNum);
                    //    _modeltotal.typeNum = _list.Where(x => x.countyName == "小计").Sum(y => y.typeNum);
                    //    _list.Add(_modeltotal);
                    //}
                    #endregion
                }
                else
                {
                    #region 网格
                    var a = db.Select<VillageGridPersonLiable>(w => w.Year == _year && w.GridName == request.typename).ToList();
                    var cityList = f.Where(w => w.adcd.EndsWith("00000000000")).ToList();
                    var num = 0; numList = new List<int>();
                    cityList.ForEach(w =>
                    {
                        //县
                        var countyList = f.Where(x => x.adcd.StartsWith(w.adcd.Substring(0, 4)) && x.adcd != w.adcd.Substring(0, 4) + "00000000000" && x.adcd.EndsWith("000000000")).ToList();
                        StatisTypeInfo _model = null;
                        var nums = 1;
                        countyList.ForEach(x =>
                        {
                            _model = new StatisTypeInfo();
                            _model.cityName = w.adnm;
                            _model.countyName = x.adnm;
                            _model.countyadcd = x.adcd;
                            _model.townnum = f.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.adcd.EndsWith("000000") && y.adcd != x.adcd).Count();
                            _model.typeNum = a.Where(y => y.VillageADCD.StartsWith(x.adcd.Substring(0, 6))).Count();
                            _model.personLiableNum = a.Where(y => y.VillageADCD.StartsWith(x.adcd.Substring(0, 6)) && y.PersonLiable != "" && y.PersonLiable != null).GroupBy(y => y.PersonLiable).Count();
                            _list.Add(_model);
                            nums++;
                        });
                        //合计
                        _model = new StatisTypeInfo();
                        _model.cityName = w.adnm;
                        _model.countyName = "小计";
                        _model.townnum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.townnum);
                        _model.typeNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.typeNum);
                        _model.personLiableNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.personLiableNum);
                        if (num == 0) { _model.numstr = "0," + nums; numList.Add(nums); }
                        else
                        {
                            _model.numstr = numList[num - 1] + "," + nums; numList.Add(numList[num - 1] + nums);
                        }
                        num++;
                        _list.Add(_model);
                    });
                    if (cityList.Count > 1)
                    {
                        StatisTypeInfo _modeltotal = new StatisTypeInfo();
                        _modeltotal.cityName = "全省";
                        _modeltotal.countyName = "汇总";
                        _modeltotal.townnum = _list.Where(x => x.countyName == "小计").Sum(y => y.townnum);
                        _modeltotal.personLiableNum = _list.Where(x => x.countyName == "小计").Sum(y => y.personLiableNum);
                        _modeltotal.typeNum = _list.Where(x => x.countyName == "小计").Sum(y => y.typeNum);
                        _list.Add(_modeltotal);
                    }
                    #endregion
                }

                return _list;
            }
        }

        public BsTableDataSource<TownPersonLiableViewModel> GetStatisTownPerson(GetStatisTownPerson request)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<TownPersonLiable>();
                builder.LeftJoin<TownPersonLiable, ADCDInfo>((x, y) => x.adcd == y.adcd);
                if (!string.IsNullOrEmpty(request.adcd))
                {
                    if (int.Parse(request.adcd.Substring(6, 3)) > 0)//乡镇
                        builder.And(x => x.adcd.StartsWith(request.adcd.Substring(0, 9)));
                    else if (int.Parse(request.adcd.Substring(4, 2)) > 0)//县级
                        builder.And(x => x.adcd.StartsWith(request.adcd.Substring(0, 6)));
                    else if (int.Parse(request.adcd.Substring(2, 2)) > 0) //市级
                        builder.And(x => x.adcd.StartsWith(request.adcd.Substring(0, 4)));
                }
                else if (string.IsNullOrEmpty(request.adcd))
                {
                    switch (RowID)
                    {
                        case 5: builder.And(x => x.adcd.StartsWith(adcd.Substring(0, 2))); break;
                        case 2: builder.And(x => x.adcd.StartsWith(adcd.Substring(0, 4))); break;
                        case 3: break;
                    }
                }
                else
                {

                }
                if (!string.IsNullOrEmpty(request.townname))
                    builder.Where<ADCDInfo>(x => x.adnm.Contains(request.townname));
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                builder.And(x => x.Year == _year);

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
        }

        public BsTableDataSource<StatisVillagePersonViewModel> GetStatisVillagePerson(GetStatisVillagePerson request)
        {
            using (var db = DbFactory.Open())
            {
                var _year = request.year == null ? DateTime.Now.Year : request.year;

                var where = ""; var where1 = ""; var where2 = "";
                if (!string.IsNullOrEmpty(request.adcd))
                {
                    if (int.Parse(request.adcd.Substring(6, 3)) > 0)//乡镇
                    {
                        where = " AND VillageADCD like '" + request.adcd.Substring(0, 9) + "%'";
                        where1 = " AND adcd like '" + request.adcd.Substring(0, 9) + "%'";
                        where2 = " AND a.adcd like '" + request.adcd.Substring(0, 9) + "%'";
                    }
                    else if (int.Parse(request.adcd.Substring(4, 2)) > 0)//县级
                    {
                        where = " AND VillageADCD like '" + request.adcd.Substring(0, 6) + "%'";
                        where1 = " AND adcd like '" + request.adcd.Substring(0, 6) + "%'";
                        where2 = " AND a.adcd like '" + request.adcd.Substring(0, 6) + "%'";
                    }
                    else if (int.Parse(request.adcd.Substring(2, 2)) > 0) //市级
                    {
                        where = " AND VillageADCD like '" + request.adcd.Substring(0, 4) + "%'";
                        where1 = " AND adcd like '" + request.adcd.Substring(0, 4) + "%'";
                        where2 = " AND a.adcd like '" + request.adcd.Substring(0, 4) + "%'";
                    }
                    else { }
                }
                else if (string.IsNullOrEmpty(request.adcd))
                {
                    switch (RowID)
                    {
                        case 5:
                            where = " AND VillageADCD like '" + adcd.Substring(0, 2) + "%'";
                            where1 = " AND adcd like '" + adcd.Substring(0, 2) + "%'";
                            where2 = " AND a.adcd like '" + adcd.Substring(0, 2) + "%'";
                            break;
                        case 2:
                            where = " AND VillageADCD like '" + adcd.Substring(0, 4) + "%'";
                            where1 = " AND adcd like '" + adcd.Substring(0, 4) + "%'";
                            where2 = " AND a.adcd like '" + adcd.Substring(0, 4) + "%'";
                            break;
                    }
                }
                else { }
                if (!string.IsNullOrEmpty(request.villagename))
                    where += " AND adnm like '%" + request.villagename + "%'";

                var sql = " SELECT A.adcd,a.adnm,B.WGPersONCount,c.GridPersONCount,D.PersONLiableCount,E.WarnPersONLiableCount,F.DisasterPreventiONCount,ROW_NUMBER() OVER(ORDER BY A.adcd) As RowNum   FROM ADCDInfo A"
               + " left join"
               + " (SELECT VillageADCD adcd, COUNT(VillageADCD) WGPersONCount FROM VillageWorkingGroup WHERE Year = " + _year + " AND PersONLiable != '' AND PersONLiable is not null " + where + " GROUP BY VillageADCD) B"
               + " ON A.adcd = B.adcd"
               + " left join"
               + " (SELECT VillageADCD adcd, COUNT(VillageADCD) GridPersONCount FROM VillageGridPersONLiable WHERE Year = " + _year + " AND PersONLiable != '' AND PersONLiable is not null " + where + " GROUP BY VillageADCD ) C"
               + " ON A.adcd = C.adcd"
               + " left join"
               + " (SELECT adcd, COUNT(adcd)PersONLiableCount FROM VillageTransferPersON WHERE PersONLiableName != '' AND PersONLiableName is not null AND Year = " + _year + " " + where1 + " GROUP BY adcd ) D"
               + " ON A.adcd = D.adcd"
               + " left join"
               + " (SELECT adcd, COUNT(adcd)WarnPersONLiableCount FROM VillageTransferPersON WHERE WarnPersONLiableName != '' AND WarnPersONLiableName is not null AND Year = " + _year + " " + where1 + " GROUP BY adcd ) E"
               + " ON A.adcd = E.adcd"
               + " left join"
               + " (SELECT adcd, COUNT(adcd)DisasterPreventiONCount FROM VillageTransferPersON WHERE DisasterPreventiONManager != '' AND DisasterPreventiONManager is not null AND Year = " + _year + " " + where1 + " GROUP BY adcd) F"
               + " ON a.adcd = f.adcd"
               + " WHERE SUBSTRING(a.adcd, 10, 3) > 0 " + where2 + "";

                var count = db.Select<StatisVillagePersonViewModel>(sql).Count();

                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                var newsql = "";
                if (PageIndex == 0) newsql = " SELECT top " + PageSize + " * FROM (" + sql + ") AS RowCONstrainedResult";
                else newsql = " SELECT  * FROM (" + sql + ")" + " AS RowCONstrainedResult WHERE RowNum > " + PageIndex + " AND RowNum <= " + (PageIndex + PageSize) + "";
                var list = db.Select<StatisVillagePersonViewModel>(newsql);

                return new BsTableDataSource<StatisVillagePersonViewModel>() { rows = list, total = count };
            }
        }
        #endregion

        #region 责任人统计
        /// <summary>
        /// 省
        /// </summary>
        /// <param name="_year"></param>
        /// <returns></returns>
        public List<CAppStaticsViewModel> GetStaticsList(int? _year)
        {
            List<CAppStaticsViewModel> _list = new List<CAppStaticsViewModel>();
            CAppStaticsViewModel _model = new CAppStaticsViewModel();
            using (var db = DbFactory.Open())
            {
                //危险区 网格
                var DangerPoint = db.Select<DisasterPointItem>("SELECT * FROM (SELECT  DangerZoneType typename,COUNT(DangerZoneType) as nums,typeid=1 FROM VillageTransferPerson where Year=" + _year + " and DangerZoneType != '' and DangerZoneType is not null and DangerZoneType != '工棚' group by DangerZoneType" +
                    " UNION SELECT  GridName typename,COUNT(GridName) as nums,typeid=2 FROM VillageGridPersonLiable where Year=" + _year + " group by GridName) T");
                _model.DisasterPoint = DangerPoint;
                #region
                //县级责任人
                // _model.CountyPLNums = db.Single<CAppStaticsViewModel>("select COUNT(*) as CountyPLNums from (select ltrim(rtrim(UserName)) name, adcd from CountryPerson where Year=" + _year + " group by ltrim(rtrim(UserName)),adcd) t").CountyPLNums;
                //镇级责任
                // _model.TownPLNums = db.Single<CAppStaticsViewModel>("select COUNT(*) as TownPLNums from (select ltrim(rtrim(Name)) name, adcd from TownPersonLiable where Year=" + _year + " group by ltrim(rtrim(Name)),adcd) t").TownPLNums;
                //村级责任人
                //_model.VillagePLNums = db.Single<CAppStaticsViewModel>("select count(*) as VillagePLNums  from("
                //    + " select distinct PersonLiable name, VillageADCD adcd from VillageWorkingGroup where Year = " + _year + ""
                //    + " union select distinct PersonLiable name, VillageADCD adcd from VillageGridPersonLiable where Year = " + _year + ""
                //    + " union select distinct PersonLiableName name, adcd from VillageTransferPerson where PersonLiableName != '' and PersonLiableName is not null and Year = " + _year + ""
                //    + " union select distinct WarnPersonLiableName name, adcd from VillageTransferPerson where WarnPersonLiableName != '' and WarnPersonLiableName is not null and Year = " + _year + ""
                //    + " union select distinct DisasterPreventionManager name, adcd from VillageTransferPerson where DisasterPreventionManager != '' and DisasterPreventionManager is not null and Year = " + _year + ") as t").VillagePLNums;

                //转移人员
                var sum = db.Single<VillageTransferPerson>("SELECT SUM(HouseholderNum) as HouseholderNum FROM VillageTransferPerson WHERE HouseholderNum > 0 AND DangerZoneType !='工棚' AND Year = " + _year + "");
                _model.TransferPersonNums = sum.HouseholderNum;
                #endregion
                _list.Add(_model);
                return _list;
            }
        }
        /// <summary>
        /// 省下各地区
        /// </summary>
        /// <param name="num"></param>
        /// <param name="_adcd"></param>
        /// <param name="_year"></param>
        /// <returns></returns>
        public List<CAppStaticsViewModel> GetStaticsList1(int num, string _adcd, int? _year)
        {
            List<CAppStaticsViewModel> _list = new List<CAppStaticsViewModel>();
            CAppStaticsViewModel _model = new CAppStaticsViewModel();
            //var f = db.Select<VillageTransferPerson>(w=>w.DangerZoneType != "" && w.DangerZoneType != "其他").GroupBy(w=>w.DangerZoneType).Select(w=> new { typename=w.Key, nums=w.Count() });
            using (var db = DbFactory.Open())
            {
                //危险区 网格
                var DangerPoint = db.Select<DisasterPointItem>("SELECT * FROM (SELECT  DangerZoneType typename,COUNT(DangerZoneType) as nums,typeid=1 FROM VillageTransferPerson where Year=" + _year + " and DangerZoneType != '' and DangerZoneType is not null and DangerZoneType != '工棚' AND adcd like '" + _adcd.Substring(0, num) + "%' group by DangerZoneType" +
                        " UNION SELECT  GridName typename,COUNT(GridName) as nums,typeid=2 FROM VillageGridPersonLiable where Year=" + _year + " and VillageADCD like '" + _adcd.Substring(0, num) + "%'  group by GridName) T");
                _model.DisasterPoint = DangerPoint;
                //转移人员
                _model.TransferPersonNums = db.Single<CAppStaticsViewModel>("SELECT SUM(HouseholderNum) as TransferPersonNums FROM VillageTransferPerson WHERE HouseholderNum > 0 AND DangerZoneType != '工棚' AND adcd like '" + _adcd.Substring(0, num) + "%' and Year = " + _year + "").TransferPersonNums;

                //县级责任人
                // _model.CountyPLNums = db.Single<CAppStaticsViewModel>("select COUNT(*) AS CountyPLNums from (select ltrim(rtrim(UserName)) name, adcd from CountryPerson where Year=" + _year + " and adcd like '" + _adcd.Substring(0, num) + "%' group by ltrim(rtrim(UserName)),adcd) T").CountyPLNums;
                //镇级责任
                // _model.TownPLNums = db.Single<CAppStaticsViewModel>("select COUNT(*) AS TownPLNums from (select ltrim(rtrim(Name)) name, adcd from TownPersonLiable where Year=" + _year + " and adcd like '" + _adcd.Substring(0, num) + "%' group by ltrim(rtrim(Name)),adcd) T").TownPLNums;
                //村级责任人
                //_model.VillagePLNums = db.Single<CAppStaticsViewModel>("select COUNT(*) AS VillagePLNums from("
                //     + " select distinct PersonLiable name, VillageADCD adcd from VillageWorkingGroup where Year = " + _year + " and VillageADCD like '" + _adcd.Substring(0, num) + "%'"
                //     + " union select distinct PersonLiable name, VillageADCD adcd from VillageGridPersonLiable where Year = " + _year + " and VillageADCD like '" + _adcd.Substring(0, num) + "%'"
                //     + " union select distinct PersonLiableName name, adcd from VillageTransferPerson where PersonLiableName != '' and PersonLiableName is not null and Year = " + _year + " and adcd like '" + _adcd.Substring(0, num) + "%'"
                //     + " union select distinct WarnPersonLiableName name, adcd from VillageTransferPerson where WarnPersonLiableName != '' and WarnPersonLiableName is not null and Year = " + _year + " and adcd like '" + _adcd.Substring(0, num) + "%'"
                //     + " union select distinct DisasterPreventionManager name, adcd from VillageTransferPerson where DisasterPreventionManager != '' and DisasterPreventionManager is not null and Year = " + _year + " and adcd like '" + _adcd.Substring(0, num) + "%') as t").VillagePLNums;

                _list.Add(_model);
                return _list;
            }
        }
        #endregion

        #region 岗位统计
        /// <summary>
        /// 按类型岗位统计
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<CAppStaticsByPostViewModel> StatisticsByPost(StatisticsByPost request)
        {
            List<CAppStaticsByPostViewModel> _list = new List<CAppStaticsByPostViewModel>();
            _list = GetStaticsByPostList(request);
            return _list;
        }

        public List<CAppStaticsByPostViewModel> GetStaticsByPostList(StatisticsByPost request)
        {
            List<CAppStaticsByPostViewModel> _list = new List<CAppStaticsByPostViewModel>();
            CAppStaticsByPostViewModel cspv = new CAppStaticsByPostViewModel();
            var _year = request.year == null ? DateTime.Now.Year : request.year;
            var _adcd = request.adcd;
            var _adcdlevel = request.adcdtype;
            var _typename = request.typename;
            var _typelevel = request.typelevel;

            using (var db = DbFactory.Open())
            {
                var where = ""; var whereadcd = "";
                if (string.IsNullOrEmpty(_adcd) && string.IsNullOrEmpty(_typename) && string.IsNullOrEmpty(_typelevel))
                {
                    if (RowID == 2)
                    {
                        where = " AND VillageAdcd like '" + adcd.Substring(0, 4) + "%'";
                        whereadcd = " AND adcd like '" + adcd.Substring(0, 4) + "%'";
                    }
                    else if (RowID == 3)
                    {
                        where = " AND VillageAdcd like '" + adcd.Substring(0, 6) + "%'";
                        whereadcd = " AND adcd like '" + adcd.Substring(0, 6) + "%'";
                    }
                    var sql = "select * from(select COUNT(Position) nums,Position post,postlevel='town' from TownPersonLiable where Year=" + _year + " " + whereadcd + " group by Position" +
                    " union select COUNT(Post) nums,post,postlevel='village' from VillageWorkingGroup where Year=" + _year + " " + where + " group by Post" +
                    " union select COUNT(Position) nums,Position post,postlevel='villageposition' from VillageGridPersonLiable where Year=" + _year + " and (Position='巡查员' or Position='管理员' or Position='预警员' or Position='监测员' or Position='联络员') " + where + " group by Position" +
                    " union select COUNT(Position) nums,Position post,postlevel='county' from CountryPerson where Year=" + _year + " " + whereadcd + " group by Position) T";
                    cspv.DisasterPoint = db.Select<CAppStaticsPLViewModel>(sql);
                    //县级责任人
                    cspv.CountyPLNums = db.Single<CAppStaticsByPostViewModel>("select COUNT(*) as CountyPLNums from (select ltrim(rtrim(UserName)) name, adcd from CountryPerson where Year=" + _year + " " + whereadcd + " group by ltrim(rtrim(UserName)),adcd) t").CountyPLNums;
                    //镇级责任
                    cspv.TownPLNums = db.Single<CAppStaticsByPostViewModel>("select COUNT(*) as TownPLNums from (select ltrim(rtrim(Name)) name, adcd from TownPersonLiable where Year=" + _year + " " + whereadcd + " group by ltrim(rtrim(Name)),adcd) t").TownPLNums;
                    //村级责任人
                    cspv.VillagePLNums = db.Single<CAppStaticsByPostViewModel>("select count(*) as VillagePLNums  from("
                        + " select distinct PersonLiable name, VillageADCD adcd from VillageWorkingGroup where Year = " + _year + "" + where + ""
                        + " union select distinct PersonLiable name, VillageADCD adcd from VillageGridPersonLiable where Year = " + _year + "" + where + ""
                        + " union select distinct PersonLiableName name, adcd from VillageTransferPerson where PersonLiableName != '' and PersonLiableName is not null and Year = " + _year + "" + whereadcd + ""
                        + " union select distinct WarnPersonLiableName name, adcd from VillageTransferPerson where WarnPersonLiableName != '' and WarnPersonLiableName is not null and Year = " + _year + "" + whereadcd + ""
                        + " union select distinct DisasterPreventionManager name, adcd from VillageTransferPerson where DisasterPreventionManager != '' and DisasterPreventionManager is not null and Year = " + _year + "" + whereadcd + ") as t").VillagePLNums;

                }
                else if (string.IsNullOrEmpty(_adcd) && !string.IsNullOrEmpty(_typename) && !string.IsNullOrEmpty(_typelevel))
                {
                    if (RowID == 2)
                    {
                        where = " AND VillageAdcd like '" + adcd.Substring(0, 4) + "%'";
                        whereadcd = " AND adcd like '" + adcd.Substring(0, 4) + "%'";
                    }
                    else
                    {
                        where = " AND VillageAdcd like '" + adcd.Substring(0, 6) + "%'";
                        whereadcd = " AND adcd like '" + adcd.Substring(0, 6) + "%'";
                    }
                    var sql = "select * from(select COUNT(Position) nums,Position post,postlevel='town' from TownPersonLiable where Year=" + _year + " and Position='" + _typename + "' " + whereadcd + " group by Position" +
                    " union select COUNT(Post) nums,post,postlevel='village' from VillageWorkingGroup where Year=" + _year + " and post='" + _typename + "' " + where + " group by Post" +
                    " union select COUNT(Position) nums,Position post,postlevel='villageposition' from VillageGridPersonLiable where Year=" + _year + " and Position='" + _typename + "' and (Position='巡查员' or Position='管理员' or Position='预警员' or Position='监测员' or Position='联络员') " + where + " group by Position" +
                    " union select COUNT(Position) nums,Position post,postlevel='county' from CountryPerson where Year=" + _year + " and Position='" + _typename + "' " + whereadcd + " group by Position) T";
                    cspv.DisasterPoint = db.Select<CAppStaticsPLViewModel>(sql);


                }
                else if (!string.IsNullOrEmpty(_adcd) && !string.IsNullOrEmpty(_typename))
                {
                    switch (_adcdlevel)
                    {
                        case 5:
                            where = " AND VillageAdcd like '" + _adcd.Substring(0, 2) + "%'";
                            whereadcd = " AND adcd like '" + _adcd.Substring(0, 2) + "%'";
                            break;
                        case 2:
                            where = " AND VillageAdcd like '" + _adcd.Substring(0, 4) + "%'";
                            whereadcd = " AND adcd like '" + _adcd.Substring(0, 4) + "%'";
                            break;
                        case 3:
                            where = " AND VillageAdcd like '" + _adcd.Substring(0, 6) + "%'";
                            whereadcd = " AND adcd like '" + _adcd.Substring(0, 6) + "%'";
                            break;
                        case 4:
                            where = " AND VillageAdcd like '" + _adcd.Substring(0, 9) + "%'";
                            whereadcd = " AND adcd like '" + _adcd.Substring(0, 9) + "%'";
                            break;
                    }
                    switch (_typelevel)
                    {
                        case "town":
                            cspv.DisasterPoint = db.Select<CAppStaticsPLViewModel>("select COUNT(Position) nums,Position post,postlevel='town' from TownPersonLiable where Year=" + _year + " and Position='" + _typename + "' " + whereadcd + " group by Position");
                            break;
                        case "village":
                            cspv.DisasterPoint = db.Select<CAppStaticsPLViewModel>("select COUNT(Post) nums,post,postlevel='village' from VillageWorkingGroup where Year=" + _year + " and post='" + _typename + "' " + where + " group by Post");
                            break;
                        case "villageposition":
                            cspv.DisasterPoint = db.Select<CAppStaticsPLViewModel>("select COUNT(Position) nums,Position post,postlevel='villageposition' from VillageGridPersonLiable where Year=" + _year + " and Position='" + _typename + "' and (Position='巡查员' or Position='管理员' or Position='预警员' or Position='监测员' or Position='联络员') " + where + " group by Position");
                            break;
                        case "county":
                            cspv.DisasterPoint = db.Select<CAppStaticsPLViewModel>("select COUNT(Position) nums,Position post,postlevel='county' from CountryPerson where Year=" + _year + " and Position='" + _typename + "' " + whereadcd + " group by Position");
                            break;
                    }
                }
                else if (!string.IsNullOrEmpty(_adcd) && string.IsNullOrEmpty(_typename))
                {
                    switch (_adcdlevel)
                    {
                        case 5:
                            where = " AND VillageAdcd like '" + _adcd.Substring(0, 2) + "%'";
                            whereadcd = " AND adcd like '" + _adcd.Substring(0, 2) + "%'";
                            break;
                        case 2:
                            where = " AND VillageAdcd like '" + _adcd.Substring(0, 4) + "%'";
                            whereadcd = " AND adcd like '" + _adcd.Substring(0, 4) + "%'";
                            break;
                        case 3:
                            where = " AND VillageAdcd like '" + _adcd.Substring(0, 6) + "%'";
                            whereadcd = " AND adcd like '" + _adcd.Substring(0, 6) + "%'";
                            break;
                        case 4:
                            where = " AND VillageAdcd like '" + _adcd.Substring(0, 9) + "%'";
                            whereadcd = " AND adcd like '" + _adcd.Substring(0, 9) + "%'";
                            break;
                    }
                    var sql = "select * from(select COUNT(Position) nums,Position post,postlevel='town' from TownPersonLiable where Year=" + _year + " " + whereadcd + " group by Position" +
                    " union select COUNT(Post) nums,post,postlevel='village' from VillageWorkingGroup where Year=" + _year + " " + where + " group by Post" +
                    " union select COUNT(Position) nums,Position post,postlevel='villageposition' from VillageGridPersonLiable where Year=" + _year + " and (Position='巡查员' or Position='管理员' or Position='预警员' or Position='监测员' or Position='联络员') " + where + " group by Position" +
                    " union select COUNT(Position) nums,Position post,postlevel='county' from CountryPerson where Year=" + _year + " " + whereadcd + " group by Position) T";
                    cspv.DisasterPoint = db.Select<CAppStaticsPLViewModel>(sql);
                    //县级责任人
                    cspv.CountyPLNums = db.Single<CAppStaticsByPostViewModel>("select COUNT(*) as CountyPLNums from (select ltrim(rtrim(UserName)) name, adcd from CountryPerson where Year=" + _year + " " + whereadcd + " group by ltrim(rtrim(UserName)),adcd) t").CountyPLNums;
                    //镇级责任
                    cspv.TownPLNums = db.Single<CAppStaticsByPostViewModel>("select COUNT(*) as TownPLNums from (select ltrim(rtrim(Name)) name, adcd from TownPersonLiable where Year=" + _year + " " + whereadcd + " group by ltrim(rtrim(Name)),adcd) t").TownPLNums;
                    //村级责任人
                    cspv.VillagePLNums = db.Single<CAppStaticsByPostViewModel>("select count(*) as VillagePLNums  from("
                        + " select distinct PersonLiable name, VillageADCD adcd from VillageWorkingGroup where Year = " + _year + "" + where + ""
                        + " union select distinct PersonLiable name, VillageADCD adcd from VillageGridPersonLiable where Year = " + _year + "" + where + ""
                        + " union select distinct PersonLiableName name, adcd from VillageTransferPerson where PersonLiableName != '' and PersonLiableName is not null and Year = " + _year + "" + whereadcd + ""
                        + " union select distinct WarnPersonLiableName name, adcd from VillageTransferPerson where WarnPersonLiableName != '' and WarnPersonLiableName is not null and Year = " + _year + "" + whereadcd + ""
                        + " union select distinct DisasterPreventionManager name, adcd from VillageTransferPerson where DisasterPreventionManager != '' and DisasterPreventionManager is not null and Year = " + _year + "" + whereadcd + ") as t").VillagePLNums;

                }
                else { }

            }
            _list.Add(cspv);
            return _list;
        }
        public List<CAppStaticsByPostViewModel> GetStaticsByPostList1(int num, StatisticsByPost request)
        {
            List<CAppStaticsByPostViewModel> _list = new List<CAppStaticsByPostViewModel>();
            CAppStaticsByPostViewModel _model = new CAppStaticsByPostViewModel();
            var _year = request.year == null ? DateTime.Now.Year : request.year;
            var _adcd = string.IsNullOrEmpty(request.adcd) ? adcd : request.adcd;
            var typename = request.typename;
            var typeleve = request.typelevel;

            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(typename) && string.IsNullOrEmpty(typeleve))
                {
                    var sql = "select * from(select COUNT(Position) nums,Position post,postlevel='town' from TownPersonLiable where Year=" + _year + " and adcd like '" + _adcd.Substring(0, num) + "%' group by Position" +
                    " union select COUNT(Post) nums,post,postlevel='village' from VillageWorkingGroup where Year=" + _year + " and VillageADCD like '" + _adcd.Substring(0, num) + "%' group by Post" +
                    " union select COUNT(Position) nums,Position post,postlevel='villageposition' from VillageGridPersonLiable where Year=" + _year + " and VillageADCD like '" + _adcd.Substring(0, num) + "%' and (Position='巡查员' or Position='管理员' or Position='预警员' or Position='监测员' or Position='联络员') group by Position" +
                    " union select COUNT(Position) nums,Position post,postlevel='county' from CountryPerson where Year=" + _year + " and adcd like '" + _adcd.Substring(0, num) + "%' group by Position) T";
                    _model.DisasterPoint = db.Select<CAppStaticsPLViewModel>(sql);
                }
                else
                {
                    switch (typeleve)
                    {
                        case "town":
                            _model.DisasterPoint = db.Select<CAppStaticsPLViewModel>("select COUNT(Position) nums,Position post,postlevel='town' from TownPersonLiable where Year=" + _year + " and adcd like '" + _adcd.Substring(0, num) + "%' and Position='" + typename + "' group by Position");
                            break;
                        case "village":
                            _model.DisasterPoint = db.Select<CAppStaticsPLViewModel>("select COUNT(Post) nums,post,postlevel='village' from VillageWorkingGroup where Year=" + _year + " and VillageADCD like '" + _adcd.Substring(0, num) + "%' and Post='" + typename + "' group by Post");
                            break;
                        case "villageposition":
                            _model.DisasterPoint = db.Select<CAppStaticsPLViewModel>("select COUNT(Position) nums,Position post,postlevel='villageposition' from VillageGridPersonLiable where Year=" + _year + " and VillageADCD like '" + _adcd.Substring(0, num) + "%' and Position='" + typename + "' group by Position");
                            break;
                        case "county":
                            _model.DisasterPoint = db.Select<CAppStaticsPLViewModel>("select COUNT(Position) nums,Position post,postlevel='county' from CountryPerson where Year=" + _year + " and adcd like '" + _adcd.Substring(0, num) + "%' and Position='" + typename + "' group by Position");
                            break;
                    }
                }
                //县级责任人
                _model.CountyPLNums = db.Single<CAppStaticsViewModel>("select COUNT(*) AS CountyPLNums from (select ltrim(rtrim(UserName)) name, adcd from CountryPerson where Year=" + _year + " and adcd like '" + _adcd.Substring(0, num) + "%' group by ltrim(rtrim(UserName)),adcd) T").CountyPLNums;
                //镇级责任
                _model.TownPLNums = db.Single<CAppStaticsViewModel>("select COUNT(*) AS TownPLNums from (select ltrim(rtrim(Name)) name, adcd from TownPersonLiable where Year=" + _year + " and adcd like '" + _adcd.Substring(0, num) + "%' group by ltrim(rtrim(Name)),adcd) T").TownPLNums;
                //村级责任人
                _model.VillagePLNums = db.Single<CAppStaticsViewModel>("select COUNT(*) AS VillagePLNums from("
                + " select distinct PersonLiable name, VillageADCD adcd from VillageWorkingGroup where Year = " + _year + " and VillageADCD like '" + _adcd.Substring(0, num) + "%'"
                + " union select distinct PersonLiable name, VillageADCD adcd from VillageGridPersonLiable where Year = " + _year + " and VillageADCD like '" + _adcd.Substring(0, num) + "%'"
                + " union select distinct PersonLiableName name, adcd from VillageTransferPerson where PersonLiableName != '' and PersonLiableName is not null and Year = " + _year + " and adcd like '" + _adcd.Substring(0, num) + "%'"
                + " union select distinct WarnPersonLiableName name, adcd from VillageTransferPerson where WarnPersonLiableName != '' and WarnPersonLiableName is not null and Year = " + _year + " and adcd like '" + _adcd.Substring(0, num) + "%'"
                + " union select distinct DisasterPreventionManager name, adcd from VillageTransferPerson where DisasterPreventionManager != '' and DisasterPreventionManager is not null and Year = " + _year + " and adcd like '" + _adcd.Substring(0, num) + "%') as t").VillagePLNums;

            }
            _list.Add(_model);
            return _list;
        }
        public BsTableDataSource<StatisticsByPostInfoViewModel> StatisticsByPostOne(StatisticsByPostOne request)
        {
            if (string.IsNullOrEmpty(request.typename)) throw new Exception("类型名不能为空");
            var _year = request.year == null ? DateTime.Now.Year : request.year;
            using (var db = DbFactory.Open())
            {
                var typename = request.typename;
                List<StatisticsByPostInfoViewModel> _list = null;
                var adcdlike = ""; var count = 0;
                var _adcd = string.IsNullOrEmpty(request.adcd) ? adcd : request.adcd;
                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                if (RowID == 5 || RowID == 2)
                {
                    switch (request.type)
                    {
                        case "town":
                            var buildertown = db.From<TownPersonLiable>();
                            buildertown.LeftJoin<TownPersonLiable, ADCDInfo>((x, y) => x.adcd == y.adcd);
                            if (!string.IsNullOrEmpty(_adcd) && _adcd.Contains("0000000000000"))
                            {
                                adcdlike = adcd.Substring(0, 2);
                            }
                            else if (!string.IsNullOrEmpty(_adcd) && _adcd.Contains("00000000000"))
                            {
                                adcdlike = _adcd.Substring(0, 4);
                            }
                            else if (!string.IsNullOrEmpty(_adcd) && _adcd.Contains("000000000"))
                            {
                                adcdlike = _adcd.Substring(0, 6);
                            }
                            else if (!string.IsNullOrEmpty(_adcd) && _adcd.Contains("000000"))
                            {
                                adcdlike = _adcd.Substring(0, 9);
                            }
                            else
                            {
                                //adcdlike = adcd.Substring(0, 2);
                            }
                            buildertown.Where(w => w.Year == _year && w.adcd.StartsWith(adcdlike) && w.Position == typename);
                            buildertown.Select("TownPersonLiable.Name,TownPersonLiable.Position Post,TownPersonLiable.Post Position,TownPersonLiable.Remark,TownPersonLiable.Mobile,ADCDInfo.adnm");
                            count = db.Select(buildertown).Count;
                            buildertown.Limit(PageIndex, PageSize);
                            _list = db.Select<StatisticsByPostInfoViewModel>(buildertown);
                            break;
                        case "village":
                            //_list = db.Select<StatisticsByPostInfoViewModel>("select COUNT(Post) nums,post,postlevel='village' from VillageWorkingGroup where Year=" + _year + " and VillageADCD like '" + _adcd.Substring(0, num) + "%' and Post='" + typename + "' group by Post");
                            var buildervillage = db.From<VillageWorkingGroup>();
                            buildervillage.LeftJoin<VillageWorkingGroup, ADCDInfo>((x, y) => x.VillageADCD == y.adcd);
                            if (!string.IsNullOrEmpty(_adcd) && _adcd.Contains("0000000000000"))
                            {
                                adcdlike = adcd.Substring(0, 2);
                            }
                            else if (!string.IsNullOrEmpty(_adcd) && _adcd.Contains("00000000000"))
                            {
                                adcdlike = _adcd.Substring(0, 4);
                            }
                            else if (!string.IsNullOrEmpty(_adcd) && _adcd.Contains("000000000"))
                            {
                                adcdlike = _adcd.Substring(0, 6);
                            }
                            else if (!string.IsNullOrEmpty(_adcd) && _adcd.Contains("000000"))
                            {
                                adcdlike = _adcd.Substring(0, 9);
                            }
                            else
                            {
                                //adcdlike = adcd.Substring(0, 2);
                            }
                            buildervillage.Where(w => w.Year == _year && w.VillageADCD.StartsWith(adcdlike) && w.Post == typename);
                            buildervillage.Select("VillageWorkingGroup.PersonLiable Name,VillageWorkingGroup.Position,VillageWorkingGroup.Post,VillageWorkingGroup.Remarks, VillageWorkingGroup.HandPhone Mobile,ADCDInfo.adnm");
                            count = db.Select(buildervillage).Count;
                            buildervillage.Limit(PageIndex, PageSize);
                            _list = db.Select<StatisticsByPostInfoViewModel>(buildervillage);
                            break;
                        case "villageposition":
                            //_list = db.Select<StatisticsByPostInfoViewModel>("select COUNT(Position) nums,Position post,postlevel='villageposition' from VillageGridPersonLiable where Year=" + _year + " and VillageADCD like '" + _adcd.Substring(0, num) + "%' and Position='" + typename + "' group by Position");
                            var buildervillageposition = db.From<VillageGridPersonLiable>();
                            buildervillageposition.LeftJoin<VillageGridPersonLiable, ADCDInfo>((x, y) => x.VillageADCD == y.adcd);
                            if (!string.IsNullOrEmpty(_adcd) && _adcd.Contains("0000000000000"))
                            {
                                adcdlike = adcd.Substring(0, 2);
                            }
                            else if (!string.IsNullOrEmpty(_adcd) && _adcd.Contains("00000000000"))
                            {
                                adcdlike = _adcd.Substring(0, 4);
                            }
                            else if (!string.IsNullOrEmpty(_adcd) && _adcd.Contains("000000000"))
                            {
                                adcdlike = _adcd.Substring(0, 6);
                            }
                            else if (!string.IsNullOrEmpty(_adcd) && _adcd.Contains("000000"))
                            {
                                adcdlike = _adcd.Substring(0, 9);
                            }
                            else
                            {
                                // adcdlike = adcd.Substring(0, 2);
                            }
                            buildervillageposition.Where(w => w.Year == _year && w.VillageADCD.StartsWith(adcdlike) && w.Position == typename);
                            buildervillageposition.Select("VillageGridPersonLiable.PersonLiable name,VillageGridPersonLiable.Position,VillageGridPersonLiable.Post,VillageGridPersonLiable.HandPhone mobile,VillageGridPersonLiable.Remarks,ADCDInfo.adnm");
                            count = db.Select(buildervillageposition).Count;
                            buildervillageposition.Limit(PageIndex, PageSize);
                            _list = db.Select<StatisticsByPostInfoViewModel>(buildervillageposition);
                            break;
                        case "county":
                            //_list = db.Select<StatisticsByPostInfoViewModel>("select COUNT(Position) nums,Position post,postlevel='county' from CountryPerson where Year=" + _year + " and adcd like '" + _adcd.Substring(0, num) + "%' and Position='" + typename + "' group by Position");
                            var buildercounty = db.From<CountryPerson>();
                            buildercounty.LeftJoin<CountryPerson, ADCDInfo>((x, y) => x.adcd == y.adcd);
                            if (!string.IsNullOrEmpty(_adcd) && _adcd.Contains("0000000000000"))
                            {
                                adcdlike = adcd.Substring(0, 2);
                            }
                            else if (!string.IsNullOrEmpty(_adcd) && _adcd.Contains("00000000000"))
                            {
                                adcdlike = _adcd.Substring(0, 4);
                            }
                            else if (!string.IsNullOrEmpty(_adcd) && _adcd.Contains("000000000"))
                            {
                                adcdlike = _adcd.Substring(0, 6);
                            }
                            else if (!string.IsNullOrEmpty(_adcd) && _adcd.Contains("000000"))
                            {
                                adcdlike = _adcd.Substring(0, 9);
                            }
                            else
                            {
                                //adcdlike = adcd.Substring(0, 2);
                            }
                            buildercounty.Where(w => w.Year == _year && w.adcd.StartsWith(adcdlike) && w.Position == typename);
                            buildercounty.Select("CountryPerson.UserName,CountryPerson.Position Post,CountryPerson.Post Position,CountryPerson.Remark,CountryPerson.Phone mobile,ADCDInfo.adnm");
                            count = db.Select(buildercounty).Count;
                            buildercounty.Limit(PageIndex, PageSize);
                            _list = db.Select<StatisticsByPostInfoViewModel>(buildercounty);
                            break;
                    }
                }
                else
                {
                    //县级
                }
                return new BsTableDataSource<StatisticsByPostInfoViewModel>() { total = count, rows = _list };
            }
        }
        #endregion

        #region 转移人员统计详情查看
        public List<StaticsVillageTransferModel> StaticsVillageTransfer(StaticsVillageTransfer request)
        {
            List<StaticsVillageTransferModel> newlist = new List<StaticsVillageTransferModel>();
            if (string.IsNullOrEmpty(request.items)) throw new Exception("参数异常");
            var _year = request.year == null ? DateTime.Now.Year : request.year;
            using (var db = DbFactory.Open())
            {
                var items = request.items.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (var i = 0; i < items.Length; i++)
                {
                    var item = items[i].Split('|');
                    switch (int.Parse(item[1]))
                    {
                        case 5:
                            getDangerNum(newlist, "", _year);
                            break;
                        case 2:
                            getDangerNum(newlist, item[0].Substring(0, 4), _year);
                            break;
                        case 3:
                            getDangerNum(newlist, item[0].Substring(0, 6), _year);
                            break;
                        case 4:
                            getDangerNum(newlist, item[0].Substring(0, 9), _year);
                            break;
                        case 9:
                            getDangerNumAll(newlist, _year);
                            break;
                    }
                }
            }
            return newlist;
        }
        public List<StaticsVillageTransferModel> getDangerNum(List<StaticsVillageTransferModel> newlist, string adcd, int? year)
        {
            using (var db = DbFactory.Open())
            {
                StaticsVillageTransferModel svt = null;
                var DangerZones = db.Select<DangerZone>();
                if (newlist.Count == 0)
                {
                    DangerZones.ForEach(w =>
                    {
                        svt = new StaticsVillageTransferModel();
                        svt.typename = w.DangerZoneName;
                        svt.nums = 0;
                        newlist.Add(svt);
                    });
                }
                DangerZones.ForEach(w =>
                {
                    var f = newlist.Single(x => x.typename == w.DangerZoneName);
                    if (adcd != "")
                    {
                        // var n = db.Single<VillageTransferPerson>(x => x.DangerZoneType == w.DangerZoneName && x.HouseholderNum > 0 && x.Year == year && x.adcd.StartsWith(adcd)).Sum(x => x.HouseholderNum);
                        var n = db.Single<VillageTransferPerson>("SELECT SUM(HouseholderNum) as HouseholderNum FROM VillageTransferPerson WHERE adcd LIKE '" + adcd + "%' AND DangerZoneType = '" + w.DangerZoneName + "' AND Year = " + year + " AND HouseholderNum > 0");
                        if (n != null) f.nums = f.nums + n.HouseholderNum;
                        else f.nums = f.nums + 0;
                    }
                    else
                    {
                        var n = db.Single<VillageTransferPerson>("SELECT SUM(HouseholderNum) as HouseholderNum FROM VillageTransferPerson WHERE DangerZoneType = '" + w.DangerZoneName + "' AND Year = " + year + " AND HouseholderNum > 0");
                        if (n != null) f.nums = f.nums + n.HouseholderNum;
                        else f.nums = f.nums + 0;
                    }
                });
                return newlist;
            }
        }
        public List<StaticsVillageTransferModel> getDangerNumAll(List<StaticsVillageTransferModel> newlist, int? year)
        {
            using (var db = DbFactory.Open())
            {
                StaticsVillageTransferModel svt = null;
                var DangerZones = db.Select<DangerZone>();
                if (newlist.Count == 0)
                {
                    DangerZones.ForEach(w =>
                    {
                        svt = new StaticsVillageTransferModel();
                        svt.typename = w.DangerZoneName;
                        svt.nums = 0;
                        newlist.Add(svt);
                    });
                }

                DangerZones.ForEach(w =>
                {
                    var f = newlist.Single(x => x.typename == w.DangerZoneName);
                    if (adcd != "")
                    {
                        // var n = db.Single<VillageTransferPerson>(x => x.DangerZoneType == w.DangerZoneName && x.HouseholderNum > 0 && x.Year == year && x.adcd.StartsWith(adcd)).Sum(x => x.HouseholderNum);
                        var n = db.Single<VillageTransferPerson>("SELECT SUM(HouseholderNum) as HouseholderNum FROM VillageTransferPerson WHERE adcd LIKE '" + adcd + "%' AND DangerZoneType = '" + w.DangerZoneName + "' AND Year = " + year + " AND HouseholderNum > 0");
                        if (n != null) f.nums = f.nums + n.HouseholderNum;
                        else f.nums = f.nums + 0;
                    }
                    else
                    {
                        var n = db.Single<VillageTransferPerson>("SELECT SUM(HouseholderNum) as HouseholderNum FROM VillageTransferPerson WHERE DangerZoneType = '" + w.DangerZoneName + "' AND Year = " + year + " AND HouseholderNum > 0");
                        if (n != null) f.nums = f.nums + n.HouseholderNum;
                        else f.nums = f.nums + 0;
                    }
                });
                return newlist;
            }
        }

        public BsTableDataSource<StatisTransferPersonViewModel> GetStatisTransferPerson(GetStatisTransferPerson request)
        {
            using (var db = DbFactory.Open())
            {
                List<StatisTransferPersonViewModel> _list = new List<StatisTransferPersonViewModel>();
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                var builderAdcdinfo = ""; List<int> numList = null;
                if (string.IsNullOrEmpty(request.adcd))
                {
                    switch (RowID)
                    {
                        case 5:
                            builderAdcdinfo = "select adcd,countyadcd,adnm,countyname from (select adcd, adnm from ADCDInfo where adcd like '%00000000000' and adcd != '330000000000000') a"
                               + " left join (select adcd countyadcd, adnm countyname from ADCDInfo where adcd like '%000000000' and SUBSTRING(adcd, 5, 2) > 0) b"
                               + " on SUBSTRING(a.adcd, 1, 4) = SUBSTRING(b.countyadcd, 1, 4)";
                            break;
                        case 2:
                            builderAdcdinfo = "select adcd,countyadcd,adnm,countyname from (select adcd, adnm from ADCDInfo where adcd = '" + adcd + "') a"
                               + " left join (select adcd countyadcd, adnm countyname from ADCDInfo where adcd like '%000000000' and SUBSTRING(adcd, 5, 2) > 0) b"
                               + " on SUBSTRING(a.adcd, 1, 4) = SUBSTRING(b.countyadcd, 1, 4)";
                            break;
                    }
                }
                else
                {
                    builderAdcdinfo = "select adcd,countyadcd,adnm,countyname from (select adcd, adnm from ADCDInfo where adcd = '" + request.adcd + "') a"
                               + " left join (select adcd countyadcd, adnm countyname from ADCDInfo where adcd like '%000000000' and SUBSTRING(adcd, 5, 2) > 0) b"
                               + " on SUBSTRING(a.adcd, 1, 4) = SUBSTRING(b.countyadcd, 1, 4)";
                }
                var cityList = db.Select<StatisTransferADCD>(builderAdcdinfo);
                //List<int> numList = new List<int>(); ; var cityname = ""; var num = 0;
                //StatisTransferPersonViewModel _model = null;
                //cityList.ForEach(w=> {
                //    if(w.adnm != cityname && cityname != "")
                //    {
                //        //合计
                //        _model = new StatisTransferPersonViewModel();
                //        _model.cityName = cityname;
                //        _model.countyName = "小计";
                //        _model.difangxianduanNum = _list.Where(x => x.cityName == cityname).Sum(y => y.difangxianduanNum);
                //        var nums = _list.Where(x => x.cityName == cityname).Count()+1;
                //        if (num == 0) { _model.numstr = "0," + nums; numList.Add(nums); }
                //        else
                //        {
                //            _model.numstr = numList[num - 1] + "," + nums; numList.Add(numList[num - 1] + nums);
                //        }
                //        _list.Add(_model);
                //    }
                //    cityname = w.adnm;
                //    var a = db.Select<VillageTransferPerson>("select adcd, DangerZoneType, sum(HouseholderNum) HouseholderNum from VillageTransferPerson where Year = " + _year + " and DangerZoneType != '' and DangerZoneType is not null and adcd like '"+w.countyadcd.Substring(0,6)+"%' group by adcd, DangerZoneType").ToList();
                //    _model = new StatisTransferPersonViewModel();
                //    _model.cityName = w.adnm;
                //    _model.countyadcd = w.countyadcd;
                //    _model.countyName = w.countyname;
                //    var difangxianduanNum = a.Where(x => x.DangerZoneType == "堤防险段").FirstOrDefault();
                //    _model.difangxianduanNum = difangxianduanNum != null ? difangxianduanNum.HouseholderNum :0;
                //    _list.Add(_model);
                //    num++;
                //});
                #region 转移责任人
                //var a = db.Select<VillageGridPersonLiable>(w => w.Year == _year && w.GridName == request.typename).ToList();
                var cityList1 = cityList.Select(w => new { w.adnm, w.adcd }).Distinct().ToList();
                var num = 0; numList = new List<int>();

                cityList1.ForEach(w =>
                {
                    //县
                    var countyList = cityList.Where(x => x.countyadcd.StartsWith(w.adcd.Substring(0, 4))).ToList();
                    StatisTransferPersonViewModel _model = null;
                    var nums = 1;
                    countyList.ForEach(x =>
                    {
                        var f = db.Select<VillageTransferPerson>("select adcd, DangerZoneType, sum(HouseholderNum) HouseholderNum from VillageTransferPerson where Year = " + _year + " and HouseholderNum > 0 and DangerZoneType != '' and DangerZoneType is not null and adcd like '" + x.adcd.Substring(0, 4) + "%' group by adcd, DangerZoneType").ToList();

                        _model = new StatisTransferPersonViewModel();
                        _model.cityName = w.adnm;
                        _model.countyName = x.countyname;
                        _model.countyadcd = x.countyadcd;
                        //堤防险段
                        var difangxianduanNum = f.Where(y => y.DangerZoneType == "堤防险段" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.difangxianduanNumWXQ = difangxianduanNum != null ? difangxianduanNum.Sum(y => y.HouseholderNum) : 0;
                        //危房
                        var weifangNum = f.Where(y => y.DangerZoneType == "危房" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.weifangNumWXQ = weifangNum != null ? weifangNum.Sum(y => y.HouseholderNum) : 0;
                        //山洪灾害危险区
                        var shanhongNum = f.Where(y => y.DangerZoneType == "山洪灾害危险区" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.shanhongNumWXQ = shanhongNum != null ? shanhongNum.Sum(y => y.HouseholderNum) : 0;
                        //地质灾害点
                        var dizhizaihaiNum = f.Where(y => y.DangerZoneType == "地质灾害点" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.dizhizaihaiNumWXQ = dizhizaihaiNum != null ? dizhizaihaiNum.Sum(y => y.HouseholderNum) : 0;
                        //低洼易涝区
                        var diwayilaoNum = f.Where(y => y.DangerZoneType == "低洼易涝区" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.diwayilaoNumWXQ = diwayilaoNum != null ? diwayilaoNum.Sum(y => y.HouseholderNum) : 0;
                        //屋顶山塘
                        var wudingshantangNum = f.Where(y => y.DangerZoneType == "屋顶山塘" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.wudingshantangNumWXQ = wudingshantangNum != null ? wudingshantangNum.Sum(y => y.HouseholderNum) : 0;
                        //海塘险段
                        var haitangxianduanNum = f.Where(y => y.DangerZoneType == "海塘险段" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.haitangxianduanNumWXQ = haitangxianduanNum != null ? haitangxianduanNum.Sum(y => y.HouseholderNum) : 0;
                        //其它
                        var qitaNum = f.Where(y => y.DangerZoneType == "其它" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.qitaNumWXQ = qitaNum != null ? qitaNum.Sum(y => y.HouseholderNum) : 0;

                        _list.Add(_model);
                        nums++;
                    });
                    //合计
                    _model = new StatisTransferPersonViewModel();
                    _model.cityName = w.adnm;
                    _model.countyName = "小计";
                    _model.difangxianduanNumWXQ = _list.Where(x => x.cityName == w.adnm).Sum(y => y.difangxianduanNumWXQ);
                    _model.weifangNumWXQ = _list.Where(x => x.cityName == w.adnm).Sum(y => y.weifangNumWXQ);
                    _model.shanhongNumWXQ = _list.Where(x => x.cityName == w.adnm).Sum(y => y.shanhongNumWXQ);
                    _model.dizhizaihaiNumWXQ = _list.Where(x => x.cityName == w.adnm).Sum(y => y.dizhizaihaiNumWXQ);
                    _model.diwayilaoNumWXQ = _list.Where(x => x.cityName == w.adnm).Sum(y => y.diwayilaoNumWXQ);
                    _model.wudingshantangNumWXQ = _list.Where(x => x.cityName == w.adnm).Sum(y => y.wudingshantangNumWXQ);
                    _model.haitangxianduanNumWXQ = _list.Where(x => x.cityName == w.adnm).Sum(y => y.haitangxianduanNumWXQ);
                    _model.qitaNumWXQ = _list.Where(x => x.cityName == w.adnm).Sum(y => y.qitaNumWXQ);
                    if (num == 0) { _model.numstr = "0," + nums; numList.Add(nums); }
                    else
                    {
                        _model.numstr = numList[num - 1] + "," + nums; numList.Add(numList[num - 1] + nums);
                    }
                    num++;
                    _list.Add(_model);
                });
                if (cityList1.Count > 1)
                {
                    StatisTransferPersonViewModel _modeltotal = new StatisTransferPersonViewModel();
                    _modeltotal.cityName = "全省";
                    _modeltotal.countyName = "汇总";
                    _modeltotal.difangxianduanNumWXQ = _list.Where(x => x.countyName == "小计").Sum(y => y.difangxianduanNumWXQ);
                    _modeltotal.weifangNumWXQ = _list.Where(x => x.countyName == "小计").Sum(y => y.weifangNumWXQ);
                    _modeltotal.shanhongNumWXQ = _list.Where(x => x.countyName == "小计").Sum(y => y.shanhongNumWXQ);
                    _modeltotal.dizhizaihaiNumWXQ = _list.Where(x => x.countyName == "小计").Sum(y => y.dizhizaihaiNumWXQ);
                    _modeltotal.diwayilaoNumWXQ = _list.Where(x => x.countyName == "小计").Sum(y => y.diwayilaoNumWXQ);
                    _modeltotal.wudingshantangNumWXQ = _list.Where(x => x.countyName == "小计").Sum(y => y.wudingshantangNumWXQ);
                    _modeltotal.haitangxianduanNumWXQ = _list.Where(x => x.countyName == "小计").Sum(y => y.haitangxianduanNumWXQ);
                    _modeltotal.qitaNumWXQ = _list.Where(x => x.countyName == "小计").Sum(y => y.qitaNumWXQ);
                    _modeltotal.numstr = (numList[num - 1]) + "," + 1;
                    _list.Add(_modeltotal);
                }
                #endregion
                return new BsTableDataSource<StatisTransferPersonViewModel>() { rows = _list, total = _list.Count() };
            }
        }

        public BsTableDataSource<StatisTransferPersonViewModel> GetStatisDangerTypeAll(GetStatisDangerTypeAll request)
        {
            using (var db = DbFactory.Open())
            {
                List<StatisTransferPersonViewModel> _list = new List<StatisTransferPersonViewModel>();
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                var builderAdcdinfo = ""; List<int> numList = null;
                if (string.IsNullOrEmpty(request.adcd))
                {
                    switch (RowID)
                    {
                        case 5:
                            builderAdcdinfo = "select adcd,countyadcd,adnm,countyname from (select adcd, adnm from ADCDInfo where adcd like '%00000000000' and adcd != '330000000000000') a"
                               + " left join (select adcd countyadcd, adnm countyname from ADCDInfo where adcd like '%000000000' and SUBSTRING(adcd, 5, 2) > 0) b"
                               + " on SUBSTRING(a.adcd, 1, 4) = SUBSTRING(b.countyadcd, 1, 4)";
                            break;
                        case 2:
                            builderAdcdinfo = "select adcd,countyadcd,adnm,countyname from (select adcd, adnm from ADCDInfo where adcd = '" + adcd + "') a"
                               + " left join (select adcd countyadcd, adnm countyname from ADCDInfo where adcd like '%000000000' and SUBSTRING(adcd, 5, 2) > 0) b"
                               + " on SUBSTRING(a.adcd, 1, 4) = SUBSTRING(b.countyadcd, 1, 4)";
                            break;
                        case 3:

                            break;
                    }
                }
                else
                {
                    builderAdcdinfo = "select adcd,countyadcd,adnm,countyname from (select adcd, adnm from ADCDInfo where adcd = '" + request.adcd + "') a"
                               + " left join (select adcd countyadcd, adnm countyname from ADCDInfo where adcd like '%000000000' and SUBSTRING(adcd, 5, 2) > 0) b"
                               + " on SUBSTRING(a.adcd, 1, 4) = SUBSTRING(b.countyadcd, 1, 4)";
                }
                var cityList = db.Select<StatisTransferADCD>(builderAdcdinfo);
                #region 转移责任人
                //var a = db.Select<VillageGridPersonLiable>(w => w.Year == _year && w.GridName == request.typename).ToList();
                var cityList1 = cityList.Select(w => new { w.adnm, w.adcd }).Distinct().ToList();
                var num = 0; numList = new List<int>();
                cityList1.ForEach(w =>
                {
                    //县
                    var countyList = cityList.Where(x => x.countyadcd.StartsWith(w.adcd.Substring(0, 4))).ToList();
                    StatisTransferPersonViewModel _model = null;
                    var nums = 1;
                    countyList.ForEach(x =>
                    {
                        var f = db.Select<VillageTransferPerson>("select adcd,DangerZoneType,COUNT(DangerZoneType) HouseholderNum from VillageTransferPerson  where Year = " + _year + " and DangerZoneType != '' and DangerZoneType is not null and adcd like '" + w.adcd.Substring(0, 4) + "%' group by adcd, DangerZoneType");
                        //var f = db.Select<VillageTransferPerson>("select adcd, DangerZoneType, sum(HouseholderNum) HouseholderNum from VillageTransferPerson where Year = " + _year + " and DangerZoneType != '' and DangerZoneType is not null and adcd like '" + w.adcd.Substring(0, 4) + "%' group by adcd, DangerZoneType").ToList();
                        _model = new StatisTransferPersonViewModel();
                        _model.cityName = w.adnm;
                        _model.countyName = x.countyname;
                        _model.countyadcd = x.countyadcd;
                        //堤防险段
                        var difangxianduanNum = f.Where(y => y.DangerZoneType == "堤防险段" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.difangxianduanNumWXQ = difangxianduanNum != null ? difangxianduanNum.Sum(y => y.HouseholderNum) : 0;
                        //危房
                        var weifangNum = f.Where(y => y.DangerZoneType == "危房" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.weifangNumWXQ = weifangNum != null ? weifangNum.Sum(y => y.HouseholderNum) : 0;
                        //山洪灾害危险区
                        var shanhongNum = f.Where(y => y.DangerZoneType == "山洪灾害危险区" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.shanhongNumWXQ = shanhongNum != null ? shanhongNum.Sum(y => y.HouseholderNum) : 0;
                        //地质灾害点
                        var dizhizaihaiNum = f.Where(y => y.DangerZoneType == "地质灾害点" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.dizhizaihaiNumWXQ = dizhizaihaiNum != null ? dizhizaihaiNum.Sum(y => y.HouseholderNum) : 0;
                        //低洼易涝区
                        var diwayilaoNum = f.Where(y => y.DangerZoneType == "低洼易涝区" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.diwayilaoNumWXQ = diwayilaoNum != null ? diwayilaoNum.Sum(y => y.HouseholderNum) : 0;
                        //屋顶山塘
                        var wudingshantangNum = f.Where(y => y.DangerZoneType == "屋顶山塘" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.wudingshantangNumWXQ = wudingshantangNum != null ? wudingshantangNum.Sum(y => y.HouseholderNum) : 0;
                        //海塘险段
                        var haitangxianduanNum = f.Where(y => y.DangerZoneType == "海塘险段" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.haitangxianduanNumWXQ = haitangxianduanNum != null ? haitangxianduanNum.Sum(y => y.HouseholderNum) : 0;
                        //其它
                        var qitaNum = f.Where(y => y.DangerZoneType == "其它" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.qitaNumWXQ = qitaNum != null ? qitaNum.Sum(y => y.HouseholderNum) : 0;

                        _list.Add(_model);
                        nums++;
                    });
                    //合计
                    _model = new StatisTransferPersonViewModel();
                    _model.cityName = w.adnm;
                    _model.countyName = "小计";
                    _model.difangxianduanNumWXQ = _list.Where(x => x.cityName == w.adnm).Sum(y => y.difangxianduanNumWXQ);
                    _model.weifangNumWXQ = _list.Where(x => x.cityName == w.adnm).Sum(y => y.weifangNumWXQ);
                    _model.shanhongNumWXQ = _list.Where(x => x.cityName == w.adnm).Sum(y => y.shanhongNumWXQ);
                    _model.dizhizaihaiNumWXQ = _list.Where(x => x.cityName == w.adnm).Sum(y => y.dizhizaihaiNumWXQ);
                    _model.diwayilaoNumWXQ = _list.Where(x => x.cityName == w.adnm).Sum(y => y.diwayilaoNumWXQ);
                    _model.wudingshantangNumWXQ = _list.Where(x => x.cityName == w.adnm).Sum(y => y.wudingshantangNumWXQ);
                    _model.haitangxianduanNumWXQ = _list.Where(x => x.cityName == w.adnm).Sum(y => y.haitangxianduanNumWXQ);
                    _model.qitaNumWXQ = _list.Where(x => x.cityName == w.adnm).Sum(y => y.qitaNumWXQ);
                    if (num == 0) { _model.numstr = "0," + nums; numList.Add(nums); }
                    else
                    {
                        _model.numstr = numList[num - 1] + "," + nums; numList.Add(numList[num - 1] + nums);
                    }
                    num++;
                    _list.Add(_model);
                });
                if (cityList1.Count > 1)
                {
                    StatisTransferPersonViewModel _modeltotal = new StatisTransferPersonViewModel();
                    _modeltotal.cityName = "全省";
                    _modeltotal.countyName = "汇总";
                    _modeltotal.difangxianduanNumWXQ = _list.Where(x => x.countyName == "小计").Sum(y => y.difangxianduanNumWXQ);
                    _modeltotal.weifangNumWXQ = _list.Where(x => x.countyName == "小计").Sum(y => y.weifangNumWXQ);
                    _modeltotal.shanhongNumWXQ = _list.Where(x => x.countyName == "小计").Sum(y => y.shanhongNumWXQ);
                    _modeltotal.dizhizaihaiNumWXQ = _list.Where(x => x.countyName == "小计").Sum(y => y.dizhizaihaiNumWXQ);
                    _modeltotal.diwayilaoNumWXQ = _list.Where(x => x.countyName == "小计").Sum(y => y.diwayilaoNumWXQ);
                    _modeltotal.wudingshantangNumWXQ = _list.Where(x => x.countyName == "小计").Sum(y => y.wudingshantangNumWXQ);
                    _modeltotal.haitangxianduanNumWXQ = _list.Where(x => x.countyName == "小计").Sum(y => y.haitangxianduanNumWXQ);
                    _modeltotal.qitaNumWXQ = _list.Where(x => x.countyName == "小计").Sum(y => y.qitaNumWXQ);
                    _modeltotal.numstr = (numList[num - 1]) + "," + 1;
                    _list.Add(_modeltotal);
                }
                #endregion
                return new BsTableDataSource<StatisTransferPersonViewModel>() { rows = _list, total = _list.Count() };
            }
        }

        public BsTableDataSource<StatisGridTypeAllViewModel> GetStatisGridTypeAll(GetStatisGridTypeAll request)
        {
            using (var db = DbFactory.Open())
            {
                List<StatisGridTypeAllViewModel> _list = new List<StatisGridTypeAllViewModel>();
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                var builderAdcdinfo = ""; List<int> numList = null;
                if (string.IsNullOrEmpty(request.adcd))
                {
                    switch (RowID)
                    {
                        case 5:
                            builderAdcdinfo = "select adcd,countyadcd,adnm,countyname from (select adcd, adnm from ADCDInfo where adcd like '%00000000000' and adcd != '330000000000000') a"
                               + " left join (select adcd countyadcd, adnm countyname from ADCDInfo where adcd like '%000000000' and SUBSTRING(adcd, 5, 2) > 0) b"
                               + " on SUBSTRING(a.adcd, 1, 4) = SUBSTRING(b.countyadcd, 1, 4)";
                            break;
                        case 2:
                            builderAdcdinfo = "select adcd,countyadcd,adnm,countyname from (select adcd, adnm from ADCDInfo where adcd = '" + adcd + "') a"
                               + " left join (select adcd countyadcd, adnm countyname from ADCDInfo where adcd like '%000000000' and SUBSTRING(adcd, 5, 2) > 0) b"
                               + " on SUBSTRING(a.adcd, 1, 4) = SUBSTRING(b.countyadcd, 1, 4)";
                            break;
                        case 3:

                            break;
                    }
                }
                else
                {
                    builderAdcdinfo = "select adcd,countyadcd,adnm,countyname from (select adcd, adnm from ADCDInfo where adcd = '" + request.adcd + "') a"
                               + " left join (select adcd countyadcd, adnm countyname from ADCDInfo where adcd like '%000000000' and SUBSTRING(adcd, 5, 2) > 0) b"
                               + " on SUBSTRING(a.adcd, 1, 4) = SUBSTRING(b.countyadcd, 1, 4)";
                }
                var cityList = db.Select<StatisTransferADCD>(builderAdcdinfo);
                #region 转移责任人
                //var a = db.Select<VillageGridPersonLiable>(w => w.Year == _year && w.GridName == request.typename).ToList();
                var cityList1 = cityList.Select(w => new { w.adnm, w.adcd }).Distinct().ToList();
                var num = 0; numList = new List<int>();
                cityList1.ForEach(w =>
                {
                    //县
                    var countyList = cityList.Where(x => x.countyadcd.StartsWith(w.adcd.Substring(0, 4))).ToList();
                    StatisGridTypeAllViewModel _model = null;
                    var nums = 1;
                    countyList.ForEach(x =>
                    {
                        //var f = db.Select<VillageTransferPerson>("select adcd,DangerZoneType,COUNT(DangerZoneType) HouseholderNum from VillageTransferPerson  where Year = " + _year + " and DangerZoneType != '' and DangerZoneType is not null and adcd like '" + w.adcd.Substring(0, 4) + "%' group by adcd, DangerZoneType");
                        // var f = db.Select<DisasterPointItem>("select adcd, DangerZoneType, sum(HouseholderNum) HouseholderNum from VillageTransferPerson where Year = " + _year + " and DangerZoneType != '' and DangerZoneType is not null and adcd like '" + w.adcd.Substring(0, 4) + "%' group by adcd, DangerZoneType").ToList();
                        // var f = db.Select<DisasterPointItem>("SELECT * FROM (SELECT  DangerZoneType typename,COUNT(DangerZoneType) as nums,typeid=1,adcd FROM VillageTransferPerson where Year=" + _year + " and DangerZoneType != '' and DangerZoneType != '工棚' and adcd like '" + w.adcd.Substring(0, 4) + "%' group by DangerZoneType,adcd" +
                        // " UNION SELECT  GridName typename,COUNT(GridName) as nums,typeid=2,VillageADCD adcd FROM VillageGridPersonLiable where Year=" + _year + " and VillageADCD like '" + w.adcd.Substring(0, 4) + "%' group by GridName,VillageADCD) T");
                        var f = db.Select<DisasterPointItem>("SELECT  GridName typename,COUNT(GridName) as nums,typeid=2,VillageADCD adcd FROM VillageGridPersonLiable where Year=" + _year + " and VillageADCD like '" + w.adcd.Substring(0, 4) + "%' group by GridName,VillageADCD");
                        _model = new StatisGridTypeAllViewModel();
                        _model.cityName = w.adnm;
                        _model.countyName = x.countyname;
                        _model.countyadcd = x.countyadcd;
                        //水库
                        var shuiku = f.Where(y => y.typename == "水库" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.shuiku = shuiku != null ? shuiku.Sum(y => y.nums) : 0;
                        //水闸
                        var shuizha = f.Where(y => y.typename == "水闸" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.shuizha = shuizha != null ? shuizha.Sum(y => y.nums) : 0;
                        //泵站
                        var bengzhan = f.Where(y => y.typename == "泵站" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.bengzhan = bengzhan != null ? bengzhan.Sum(y => y.nums) : 0;
                        //地下空间
                        var dixiakongjian = f.Where(y => y.typename == "地下空间" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.dixiakongjian = dixiakongjian != null ? dixiakongjian.Sum(y => y.nums) : 0;
                        //山塘
                        var shantang = f.Where(y => y.typename == "山塘" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.shantang = shantang != null ? shantang.Sum(y => y.nums) : 0;
                        //堤防
                        var difang = f.Where(y => y.typename == "堤防" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.difang = difang != null ? difang.Sum(y => y.nums) : 0;
                        //海塘
                        var haitang = f.Where(y => y.typename == "海塘" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.haitang = haitang != null ? haitang.Sum(y => y.nums) : 0;
                        //电站
                        var dianzhan = f.Where(y => y.typename == "电站" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.dianzhan = dianzhan != null ? dianzhan.Sum(y => y.nums) : 0;
                        //圩区
                        var weiqu = f.Where(y => y.typename == "圩区" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.weiqu = weiqu != null ? weiqu.Sum(y => y.nums) : 0;
                        //渡槽
                        var ducao = f.Where(y => y.typename == "渡槽" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.ducao = ducao != null ? ducao.Sum(y => y.nums) : 0;
                        //山洪灾害危险区
                        var shanhongweixian = f.Where(y => y.typename == "山洪灾害危险区" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.shanhongweixian = shanhongweixian != null ? shanhongweixian.Sum(y => y.nums) : 0;
                        //地质灾害点
                        var dizhizaihai = f.Where(y => y.typename == "地质灾害点" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.dizhizaihai = dizhizaihai != null ? dizhizaihai.Sum(y => y.nums) : 0;
                        //危房
                        var weifang = f.Where(y => y.typename == "危房" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.weifang = weifang != null ? weifang.Sum(y => y.nums) : 0;
                        //下沉式立交桥
                        var xiachengshi = f.Where(y => y.typename == "下沉式立交桥" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.xiachengshi = xiachengshi != null ? xiachengshi.Sum(y => y.nums) : 0;
                        //低洼易涝区
                        var diwayilao = f.Where(y => y.typename == "低洼易涝区" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.diwayilao = diwayilao != null ? diwayilao.Sum(y => y.nums) : 0;
                        //物资仓库
                        var wuzhichangku = f.Where(y => y.typename == "物资仓库" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.wuzhichangku = wuzhichangku != null ? wuzhichangku.Sum(y => y.nums) : 0;
                        //避灾安置场所
                        var bizaianzhi = f.Where(y => y.typename == "避灾安置场所" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.bizaianzhi = bizaianzhi != null ? bizaianzhi.Sum(y => y.nums) : 0;
                        //其它
                        var qitaNum = f.Where(y => y.typename == "其它" && y.adcd.StartsWith(x.countyadcd.Substring(0, 6)));
                        _model.qitaNum = qitaNum != null ? qitaNum.Sum(y => y.nums) : 0;

                        _list.Add(_model);
                        nums++;
                    });
                    //合计
                    _model = new StatisGridTypeAllViewModel();
                    _model.cityName = w.adnm;
                    _model.countyName = "小计";
                    _model.shuiku = _list.Where(x => x.cityName == w.adnm).Sum(y => y.shuiku);
                    _model.shuizha = _list.Where(x => x.cityName == w.adnm).Sum(y => y.shuizha);
                    _model.bengzhan = _list.Where(x => x.cityName == w.adnm).Sum(y => y.bengzhan);
                    _model.dixiakongjian = _list.Where(x => x.cityName == w.adnm).Sum(y => y.dixiakongjian);
                    _model.haitang = _list.Where(x => x.cityName == w.adnm).Sum(y => y.haitang);
                    _model.difang = _list.Where(x => x.cityName == w.adnm).Sum(y => y.difang);
                    _model.shantang = _list.Where(x => x.cityName == w.adnm).Sum(y => y.shantang);
                    _model.dianzhan = _list.Where(x => x.cityName == w.adnm).Sum(y => y.dianzhan);
                    _model.ducao = _list.Where(x => x.cityName == w.adnm).Sum(y => y.ducao);
                    _model.shanhongweixian = _list.Where(x => x.cityName == w.adnm).Sum(y => y.shanhongweixian);
                    _model.dizhizaihai = _list.Where(x => x.cityName == w.adnm).Sum(y => y.dizhizaihai);
                    _model.weifang = _list.Where(x => x.cityName == w.adnm).Sum(y => y.weifang);
                    _model.xiachengshi = _list.Where(x => x.cityName == w.adnm).Sum(y => y.xiachengshi);
                    _model.diwayilao = _list.Where(x => x.cityName == w.adnm).Sum(y => y.diwayilao);
                    _model.wuzhichangku = _list.Where(x => x.cityName == w.adnm).Sum(y => y.wuzhichangku);
                    _model.bizaianzhi = _list.Where(x => x.cityName == w.adnm).Sum(y => y.bizaianzhi);
                    _model.qitaNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.qitaNum);
                    _model.weiqu = _list.Where(x => x.cityName == w.adnm).Sum(y => y.weiqu);
                    if (num == 0) { _model.numstr = "0," + nums; numList.Add(nums); }
                    else
                    {
                        _model.numstr = numList[num - 1] + "," + nums; numList.Add(numList[num - 1] + nums);
                    }
                    num++;
                    _list.Add(_model);
                });
                if (cityList1.Count > 1)
                {
                    StatisGridTypeAllViewModel _modeltotal = new StatisGridTypeAllViewModel();
                    _modeltotal.cityName = "全省";
                    _modeltotal.countyName = "汇总";
                    _modeltotal.shuiku = _list.Where(x => x.countyName == "小计").Sum(y => y.shuiku);
                    _modeltotal.shuizha = _list.Where(x => x.countyName == "小计").Sum(y => y.shuizha);
                    _modeltotal.bengzhan = _list.Where(x => x.countyName == "小计").Sum(y => y.bengzhan);
                    _modeltotal.dixiakongjian = _list.Where(x => x.countyName == "小计").Sum(y => y.dixiakongjian);
                    _modeltotal.haitang = _list.Where(x => x.countyName == "小计").Sum(y => y.haitang);
                    _modeltotal.difang = _list.Where(x => x.countyName == "小计").Sum(y => y.difang);
                    _modeltotal.shantang = _list.Where(x => x.countyName == "小计").Sum(y => y.shantang);
                    _modeltotal.dianzhan = _list.Where(x => x.countyName == "小计").Sum(y => y.dianzhan);
                    _modeltotal.ducao = _list.Where(x => x.countyName == "小计").Sum(y => y.ducao);
                    _modeltotal.shanhongweixian = _list.Where(x => x.countyName == "小计").Sum(y => y.shanhongweixian);
                    _modeltotal.dizhizaihai = _list.Where(x => x.countyName == "小计").Sum(y => y.dizhizaihai);
                    _modeltotal.weifang = _list.Where(x => x.countyName == "小计").Sum(y => y.weifang);
                    _modeltotal.xiachengshi = _list.Where(x => x.countyName == "小计").Sum(y => y.xiachengshi);
                    _modeltotal.diwayilao = _list.Where(x => x.countyName == "小计").Sum(y => y.diwayilao);
                    _modeltotal.wuzhichangku = _list.Where(x => x.countyName == "小计").Sum(y => y.wuzhichangku);
                    _modeltotal.bizaianzhi = _list.Where(x => x.countyName == "小计").Sum(y => y.bizaianzhi);
                    _modeltotal.qitaNum = _list.Where(x => x.countyName == "小计").Sum(y => y.qitaNum);
                    _modeltotal.weiqu = _list.Where(x => x.countyName == "小计").Sum(y => y.weiqu);

                    _modeltotal.numstr = (numList[num - 1]) + "," + 1;
                    _list.Add(_modeltotal);
                }
                #endregion
                return new BsTableDataSource<StatisGridTypeAllViewModel>() { rows = _list, total = _list.Count() };
            }
        }

        public BsTableDataSource<VillageGridViewModel> GetStatisTypeInfoCountyWG(GetStatisTypeInfoCountyWG request)
        {
            using (var db = DbFactory.Open())
            {
                List<VillageGridViewModel> RList = null;
                var _adcd = "";
                var builder = db.From<VillageGridPersonLiable>();
                builder.LeftJoin<VillageGridPersonLiable, ADCDInfo>((x, y) => x.VillageADCD == y.adcd);
                builder.Where(w => w.GridName == request.typename && w.PersonLiable != "" && w.PersonLiable != null);
                if (!string.IsNullOrEmpty(request.adcdradio))
                {
                    _adcd = request.adcdradio;
                    builder.Where(w => w.VillageADCD.StartsWith(_adcd.Substring(0, 9)));
                }
                else if (!string.IsNullOrEmpty(request.adcdchecks))
                {
                    _adcd = request.adcdchecks;
                    var adcds = request.adcdchecks.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var sql = " (";
                    foreach (string adcd in adcds)
                    {
                        sql += "VillageGridPersonLiable.VillageADCD like '" + adcd.Substring(0, 9) + "%' OR ";
                    }
                    var newql = sql.Remove(sql.LastIndexOf("OR"), 2);
                    builder.And(newql + ")");
                }
                else
                {
                    _adcd = adcd;
                    builder.Where(w => w.VillageADCD.StartsWith(_adcd.Substring(0, 6)));
                }

                builder.Select("VillageGridPersonLiable.*,ADCDInfo.adnm");
                var count = db.Select(builder).Count;
                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                builder.Limit(PageIndex, PageSize);
                RList = db.Select<VillageGridViewModel>(builder);

                return new BsTableDataSource<VillageGridViewModel>() { total = count, rows = RList };
            }
        }
        //县级获取危险点个数
        public BsTableDataSource<VillageTransferPersonViewModel> GetStatisTypeInfoCountyWXQ(GetStatisTypeInfoCountyWXQ request)
        {
            // throw new NotImplementedException();
            using (var db = DbFactory.Open())
            {
                List<VillageTransferPersonViewModel> RList = null;

                var builder = db.From<VillageTransferPerson>();
                builder.LeftJoin<VillageTransferPerson, ADCDInfo>((x, y) => x.adcd == y.adcd);

                builder.Where(w => w.DangerZoneType == request.typename);
                var _adcd = "";
                if (!string.IsNullOrEmpty(request.adcdradio))
                {
                    _adcd = request.adcdradio;
                    builder.Where(w => w.adcd.StartsWith(_adcd.Substring(0, 9)));
                }
                else if (!string.IsNullOrEmpty(request.adcdchecks))
                {
                    _adcd = request.adcdchecks;
                    var adcds = request.adcdchecks.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var sql = " (";
                    foreach (string adcd in adcds)
                    {
                        sql += "VillageTransferPerson.adcd like '" + adcd.Substring(0, 9) + "%' OR ";
                    }
                    var newql = sql.Remove(sql.LastIndexOf("OR"), 2);
                    builder.And(newql + ")");
                }
                else
                {
                    _adcd = adcd;
                    builder.Where(w => w.adcd.StartsWith(_adcd.Substring(0, 6)));
                }

                builder.Select("VillageTransferPerson.*,ADCDInfo.adnm");
                var count = db.Select(builder).Count;
                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                builder.Limit(PageIndex, PageSize);
                RList = db.Select<VillageTransferPersonViewModel>(builder);

                return new BsTableDataSource<VillageTransferPersonViewModel>() { total = count, rows = RList };
            }
        }

        public BsTableDataSource<StatisTransferPersonViewModel> GetStatisCountyDangerTypeAll(GetStatisCountyDangerTypeAll request)
        {
            using (var db = DbFactory.Open())
            {
                List<StatisTransferPersonViewModel> _list = new List<StatisTransferPersonViewModel>();
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                var builderAdcdinfo = ""; List<int> numList = null;

                builderAdcdinfo = "select * from ADCDInfo where SUBSTRING(adcd, 7, 3) > 0 and adcd like '%000000' and adcd like '" + adcd.Substring(0, 6) + "%'";

                var countyList = db.Select<StatisTransferADCD>(builderAdcdinfo);
                #region 转移责任人
                var num = 0; numList = new List<int>();
                //县
                StatisTransferPersonViewModel _model = null;
                var nums = 1;
                countyList.ForEach(x =>
                    {
                        var f = db.Select<VillageTransferPerson>("select adcd,DangerZoneType,COUNT(DangerZoneType) HouseholderNum from VillageTransferPerson  where Year = " + _year + " and DangerZoneType != '' and DangerZoneType is not null and adcd like '" + x.adcd.Substring(0, 9) + "%' group by adcd, DangerZoneType");
                        //var f = db.Select<VillageTransferPerson>("select adcd, DangerZoneType, sum(HouseholderNum) HouseholderNum from VillageTransferPerson where Year = " + _year + " and DangerZoneType != '' and DangerZoneType is not null and adcd like '" + w.adcd.Substring(0, 4) + "%' group by adcd, DangerZoneType").ToList();
                        _model = new StatisTransferPersonViewModel();
                        _model.countyName = x.adnm;
                        _model.countyadcd = x.adcd;
                        //堤防险段
                        var difangxianduanNum = f.Where(y => y.DangerZoneType == "堤防险段" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.difangxianduanNumWXQ = difangxianduanNum != null ? difangxianduanNum.Sum(y => y.HouseholderNum) : 0;
                        //危房
                        var weifangNum = f.Where(y => y.DangerZoneType == "危房" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.weifangNumWXQ = weifangNum != null ? weifangNum.Sum(y => y.HouseholderNum) : 0;
                        //山洪灾害危险区
                        var shanhongNum = f.Where(y => y.DangerZoneType == "山洪灾害危险区" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.shanhongNumWXQ = shanhongNum != null ? shanhongNum.Sum(y => y.HouseholderNum) : 0;
                        //地质灾害点
                        var dizhizaihaiNum = f.Where(y => y.DangerZoneType == "地质灾害点" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.dizhizaihaiNumWXQ = dizhizaihaiNum != null ? dizhizaihaiNum.Sum(y => y.HouseholderNum) : 0;
                        //低洼易涝区
                        var diwayilaoNum = f.Where(y => y.DangerZoneType == "低洼易涝区" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.diwayilaoNumWXQ = diwayilaoNum != null ? diwayilaoNum.Sum(y => y.HouseholderNum) : 0;
                        //屋顶山塘
                        var wudingshantangNum = f.Where(y => y.DangerZoneType == "屋顶山塘" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.wudingshantangNumWXQ = wudingshantangNum != null ? wudingshantangNum.Sum(y => y.HouseholderNum) : 0;
                        //海塘险段
                        var haitangxianduanNum = f.Where(y => y.DangerZoneType == "海塘险段" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.haitangxianduanNumWXQ = haitangxianduanNum != null ? haitangxianduanNum.Sum(y => y.HouseholderNum) : 0;
                        //其它
                        var qitaNum = f.Where(y => y.DangerZoneType == "其它" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.qitaNumWXQ = qitaNum != null ? qitaNum.Sum(y => y.HouseholderNum) : 0;

                        _list.Add(_model);
                        nums++;
                    });
                //合计
                _model = new StatisTransferPersonViewModel();
                _model.countyName = "小计";
                _model.difangxianduanNumWXQ = _list.Sum(y => y.difangxianduanNumWXQ);
                _model.weifangNumWXQ = _list.Sum(y => y.weifangNumWXQ);
                _model.shanhongNumWXQ = _list.Sum(y => y.shanhongNumWXQ);
                _model.dizhizaihaiNumWXQ = _list.Sum(y => y.dizhizaihaiNumWXQ);
                _model.diwayilaoNumWXQ = _list.Sum(y => y.diwayilaoNumWXQ);
                _model.wudingshantangNumWXQ = _list.Sum(y => y.wudingshantangNumWXQ);
                _model.haitangxianduanNumWXQ = _list.Sum(y => y.haitangxianduanNumWXQ);
                _model.qitaNumWXQ = _list.Sum(y => y.qitaNumWXQ);
                if (num == 0) { _model.numstr = "0," + nums; numList.Add(nums); }
                else
                {
                    _model.numstr = numList[num - 1] + "," + nums; numList.Add(numList[num - 1] + nums);
                }
                num++;
                _list.Add(_model);
                #endregion
                return new BsTableDataSource<StatisTransferPersonViewModel>() { rows = _list, total = _list.Count() };
            }
        }

        public BsTableDataSource<StatisGridTypeAllViewModel> GetStatisCountyGridTypeAll(GetStatisCountyGridTypeAll request)
        {
            using (var db = DbFactory.Open())
            {
                List<StatisGridTypeAllViewModel> _list = new List<StatisGridTypeAllViewModel>();
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                var builderAdcdinfo = ""; List<int> numList = null;

                builderAdcdinfo = "select * from ADCDInfo where SUBSTRING(adcd, 7, 3) > 0 and adcd like '%000000' and adcd like '" + adcd.Substring(0, 6) + "%'";

                var countyList = db.Select<StatisTransferADCD>(builderAdcdinfo);
                #region 转移责任人
                //var a = db.Select<VillageGridPersonLiable>(w => w.Year == _year && w.GridName == request.typename).ToList();
                //var cityList1 = cityList.Select(w => new { w.adnm, w.adcd }).Distinct().ToList();
                var num = 0; numList = new List<int>();

                //县
                StatisGridTypeAllViewModel _model = null;
                var nums = 1;
                countyList.ForEach(x =>
                    {
                        var f = db.Select<DisasterPointItem>(" SELECT GridName typename,COUNT(GridName) as nums,VillageADCD adcd FROM VillageGridPersonLiable where Year=" + _year + " and VillageADCD like '" + x.adcd.Substring(0, 9) + "%' group by GridName,VillageADCD");
                        _model = new StatisGridTypeAllViewModel();
                        _model.countyName = x.adnm;
                        _model.countyadcd = x.adcd;
                        //水库
                        var shuiku = f.Where(y => y.typename == "水库" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.shuiku = shuiku != null ? shuiku.Sum(y => y.nums) : 0;
                        //水闸
                        var shuizha = f.Where(y => y.typename == "水闸" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.shuizha = shuizha != null ? shuizha.Sum(y => y.nums) : 0;
                        //泵站
                        var bengzhan = f.Where(y => y.typename == "泵站" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.bengzhan = bengzhan != null ? bengzhan.Sum(y => y.nums) : 0;
                        //地下空间
                        var dixiakongjian = f.Where(y => y.typename == "地下空间" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.dixiakongjian = dixiakongjian != null ? dixiakongjian.Sum(y => y.nums) : 0;
                        //山塘
                        var shantang = f.Where(y => y.typename == "山塘" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.shantang = shantang != null ? shantang.Sum(y => y.nums) : 0;
                        //堤防
                        var difang = f.Where(y => y.typename == "堤防" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.difang = difang != null ? difang.Sum(y => y.nums) : 0;
                        //海塘
                        var haitang = f.Where(y => y.typename == "海塘" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.haitang = haitang != null ? haitang.Sum(y => y.nums) : 0;
                        //电站
                        var dianzhan = f.Where(y => y.typename == "电站" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.dianzhan = dianzhan != null ? dianzhan.Sum(y => y.nums) : 0;
                        //圩区
                        var weiqu = f.Where(y => y.typename == "圩区" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.weiqu = weiqu != null ? weiqu.Sum(y => y.nums) : 0;
                        //渡槽
                        var ducao = f.Where(y => y.typename == "渡槽" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.ducao = ducao != null ? ducao.Sum(y => y.nums) : 0;
                        //山洪灾害危险区
                        var shanhongweixian = f.Where(y => y.typename == "山洪灾害危险区" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.shanhongweixian = shanhongweixian != null ? shanhongweixian.Sum(y => y.nums) : 0;
                        //地质灾害点
                        var dizhizaihai = f.Where(y => y.typename == "地质灾害点" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.dizhizaihai = dizhizaihai != null ? dizhizaihai.Sum(y => y.nums) : 0;
                        //危房
                        var weifang = f.Where(y => y.typename == "危房" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.weifang = weifang != null ? weifang.Sum(y => y.nums) : 0;
                        //下沉式立交桥
                        var xiachengshi = f.Where(y => y.typename == "下沉式立交桥" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.xiachengshi = xiachengshi != null ? xiachengshi.Sum(y => y.nums) : 0;
                        //低洼易涝区
                        var diwayilao = f.Where(y => y.typename == "低洼易涝区" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.diwayilao = diwayilao != null ? diwayilao.Sum(y => y.nums) : 0;
                        //物资仓库
                        var wuzhichangku = f.Where(y => y.typename == "物资仓库" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.wuzhichangku = wuzhichangku != null ? wuzhichangku.Sum(y => y.nums) : 0;
                        //避灾安置场所
                        var bizaianzhi = f.Where(y => y.typename == "避灾安置场所" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.bizaianzhi = bizaianzhi != null ? bizaianzhi.Sum(y => y.nums) : 0;
                        //其它
                        var qitaNum = f.Where(y => y.typename == "其它" && y.adcd.StartsWith(x.adcd.Substring(0, 6)));
                        _model.qitaNum = qitaNum != null ? qitaNum.Sum(y => y.nums) : 0;

                        _list.Add(_model);
                        nums++;
                    });
                //合计

                _model = new StatisGridTypeAllViewModel();
                _model.countyName = "小计";
                _model.shuiku = _list.Sum(y => y.shuiku);
                _model.shuizha = _list.Sum(y => y.shuizha);
                _model.bengzhan = _list.Sum(y => y.bengzhan);
                _model.dixiakongjian = _list.Sum(y => y.dixiakongjian);
                _model.haitang = _list.Sum(y => y.haitang);
                _model.difang = _list.Sum(y => y.difang);
                _model.shantang = _list.Sum(y => y.shantang);
                _model.dianzhan = _list.Sum(y => y.dianzhan);
                _model.ducao = _list.Sum(y => y.ducao);
                _model.shanhongweixian = _list.Sum(y => y.shanhongweixian);
                _model.dizhizaihai = _list.Sum(y => y.dizhizaihai);
                _model.weifang = _list.Sum(y => y.weifang);
                _model.xiachengshi = _list.Sum(y => y.xiachengshi);
                _model.diwayilao = _list.Sum(y => y.diwayilao);
                _model.wuzhichangku = _list.Sum(y => y.wuzhichangku);
                _model.bizaianzhi = _list.Sum(y => y.bizaianzhi);
                _model.qitaNum = _list.Sum(y => y.qitaNum);
                _model.weiqu = _list.Sum(y => y.weiqu);
                if (num == 0) { _model.numstr = "0," + nums; numList.Add(nums); }
                else
                {
                    _model.numstr = numList[num - 1] + "," + nums; numList.Add(numList[num - 1] + nums);
                }
                num++;
                _list.Add(_model);

                #endregion
                return new BsTableDataSource<StatisGridTypeAllViewModel>() { rows = _list, total = _list.Count() };
            }
        }
        public BsTableDataSource<StatisticsByPostInfoViewModel> StatisticsCoutyByPostOne(StatisticsCoutyByPostOne request)
        {
            if (string.IsNullOrEmpty(request.typename)) throw new Exception("类型名不能为空");
            var _year = request.year == null ? DateTime.Now.Year : request.year;
            using (var db = DbFactory.Open())
            {
                var typename = request.typename;
                List<StatisticsByPostInfoViewModel> _list = null;
                var adcdlike = ""; var count = 0;
                var _adcd = adcd;
                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;

                switch (request.type)
                {
                    case "town":
                        var buildertown = db.From<TownPersonLiable>();
                        buildertown.LeftJoin<TownPersonLiable, ADCDInfo>((x, y) => x.adcd == y.adcd);

                        if (!string.IsNullOrEmpty(request.adcd))
                        {
                            _adcd = request.adcd;
                            buildertown.Where(w => w.adcd.StartsWith(_adcd.Substring(0, 9)));
                        }
                        else if (!string.IsNullOrEmpty(request.adcds))
                        {
                            _adcd = request.adcds;
                            var adcds = request.adcds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            var sql = " (";
                            foreach (string adcd in adcds)
                            {
                                sql += "TownPersonLiable.adcd like '" + adcd.Substring(0, 9) + "%' OR ";
                            }
                            var newql = sql.Remove(sql.LastIndexOf("OR"), 2);
                            buildertown.And(newql + ")");
                        }
                        else
                        {
                            _adcd = adcd;
                            buildertown.Where(w => w.adcd.StartsWith(_adcd.Substring(0, 6)));
                        }

                        buildertown.Where(w => w.Year == _year && w.Position == typename);
                        buildertown.Select("TownPersonLiable.Name,TownPersonLiable.Position Post,TownPersonLiable.Post Position,TownPersonLiable.Remark,TownPersonLiable.Mobile,ADCDInfo.adnm");
                        count = db.Select(buildertown).Count;
                        buildertown.Limit(PageIndex, PageSize);
                        _list = db.Select<StatisticsByPostInfoViewModel>(buildertown);
                        break;
                    case "village":
                        //_list = db.Select<StatisticsByPostInfoViewModel>("select COUNT(Post) nums,post,postlevel='village' from VillageWorkingGroup where Year=" + _year + " and VillageADCD like '" + _adcd.Substring(0, num) + "%' and Post='" + typename + "' group by Post");
                        var buildervillage = db.From<VillageWorkingGroup>();
                        buildervillage.LeftJoin<VillageWorkingGroup, ADCDInfo>((x, y) => x.VillageADCD == y.adcd);

                        //adcdlike = _adcd.Substring(0, 6);
                        if (!string.IsNullOrEmpty(request.adcd))
                        {
                            _adcd = request.adcd;
                            buildervillage.Where(w => w.VillageADCD.StartsWith(_adcd.Substring(0, 9)));
                        }
                        else if (!string.IsNullOrEmpty(request.adcds))
                        {
                            _adcd = request.adcds;
                            var adcds = request.adcds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            var sql = " (";
                            foreach (string adcd in adcds)
                            {
                                sql += "VillageWorkingGroup.VillageADCD like '" + adcd.Substring(0, 9) + "%' OR ";
                            }
                            var newql = sql.Remove(sql.LastIndexOf("OR"), 2);
                            buildervillage.And(newql + ")");
                        }
                        else
                        {
                            _adcd = adcd;
                            buildervillage.Where(w => w.VillageADCD.StartsWith(_adcd.Substring(0, 6)));
                        }

                        buildervillage.Where(w => w.Year == _year && w.Post == typename);
                        buildervillage.Select("VillageWorkingGroup.PersonLiable Name,VillageWorkingGroup.Position,VillageWorkingGroup.Post,VillageWorkingGroup.Remarks, VillageWorkingGroup.HandPhone Mobile,ADCDInfo.adnm");
                        count = db.Select(buildervillage).Count;
                        buildervillage.Limit(PageIndex, PageSize);
                        _list = db.Select<StatisticsByPostInfoViewModel>(buildervillage);
                        break;
                    case "villageposition":
                        //_list = db.Select<StatisticsByPostInfoViewModel>("select COUNT(Position) nums,Position post,postlevel='villageposition' from VillageGridPersonLiable where Year=" + _year + " and VillageADCD like '" + _adcd.Substring(0, num) + "%' and Position='" + typename + "' group by Position");
                        var buildervillageposition = db.From<VillageGridPersonLiable>();
                        buildervillageposition.LeftJoin<VillageGridPersonLiable, ADCDInfo>((x, y) => x.VillageADCD == y.adcd);

                        //adcdlike = _adcd.Substring(0, 6);
                        if (!string.IsNullOrEmpty(request.adcd))
                        {
                            _adcd = request.adcd;
                            buildervillageposition.Where(w => w.VillageADCD.StartsWith(_adcd.Substring(0, 9)));
                        }
                        else if (!string.IsNullOrEmpty(request.adcds))
                        {
                            _adcd = request.adcds;
                            var adcds = request.adcds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            var sql = " (";
                            foreach (string adcd in adcds)
                            {
                                sql += "VillageGridPersonLiable.VillageADCD like '" + adcd.Substring(0, 9) + "%' OR ";
                            }
                            var newql = sql.Remove(sql.LastIndexOf("OR"), 2);
                            buildervillageposition.And(newql + ")");
                        }
                        else
                        {
                            _adcd = adcd;
                            buildervillageposition.Where(w => w.VillageADCD.StartsWith(_adcd.Substring(0, 6)));
                        }

                        buildervillageposition.Where(w => w.Year == _year && w.Position == typename);
                        buildervillageposition.Select("VillageGridPersonLiable.PersonLiable name,VillageGridPersonLiable.Position,VillageGridPersonLiable.Post,VillageGridPersonLiable.HandPhone mobile,VillageGridPersonLiable.Remarks,ADCDInfo.adnm");
                        count = db.Select(buildervillageposition).Count;
                        buildervillageposition.Limit(PageIndex, PageSize);
                        _list = db.Select<StatisticsByPostInfoViewModel>(buildervillageposition);
                        break;
                    case "county":
                        //_list = db.Select<StatisticsByPostInfoViewModel>("select COUNT(Position) nums,Position post,postlevel='county' from CountryPerson where Year=" + _year + " and adcd like '" + _adcd.Substring(0, num) + "%' and Position='" + typename + "' group by Position");
                        var buildercounty = db.From<CountryPerson>();
                        buildercounty.LeftJoin<CountryPerson, ADCDInfo>((x, y) => x.adcd == y.adcd);

                        adcdlike = _adcd.Substring(0, 6);

                        buildercounty.Where(w => w.Year == _year && w.adcd.StartsWith(adcdlike) && w.Position == typename);
                        buildercounty.Select("CountryPerson.UserName,CountryPerson.Position Post,CountryPerson.Post Position,CountryPerson.Remark,CountryPerson.Phone mobile,ADCDInfo.adnm");
                        count = db.Select(buildercounty).Count;
                        buildercounty.Limit(PageIndex, PageSize);
                        _list = db.Select<StatisticsByPostInfoViewModel>(buildercounty);
                        break;
                }
                return new BsTableDataSource<StatisticsByPostInfoViewModel>() { total = count, rows = _list };
            }
        }

        public BsTableDataSource<VillageTransferPersonViewModel> GetStatisCountyAllTransferPerson(GetStatisCountyAllTransferPerson request)
        {
            using (var db = DbFactory.Open())
            {
                List<VillageTransferPersonViewModel> RList = null;
                var _adcd = string.IsNullOrEmpty(request.adcd) ? adcd : request.adcd;
                var builder = db.From<VillageTransferPerson>();
                builder.LeftJoin<VillageTransferPerson, ADCDInfo>((x, y) => x.adcd == y.adcd);

                builder.And(w => ((w.PersonLiableName != "" && w.PersonLiableName != null) || (w.WarnPersonLiableName != "" && w.WarnPersonLiableName != null) || (w.DisasterPreventionManager != "" && w.DisasterPreventionManager != null)));

                if (_adcd.Contains("000000000")) { builder.Where(w => w.adcd.StartsWith(_adcd.Substring(0, 6))); }
                else { builder.Where(w => w.adcd.StartsWith(_adcd.Substring(0, 9))); }
                builder.Select("VillageTransferPerson.*,ADCDInfo.adnm");
                var count = db.Select(builder).Count;
                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                builder.Limit(PageIndex, PageSize);
                RList = db.Select<VillageTransferPersonViewModel>(builder);

                return new BsTableDataSource<VillageTransferPersonViewModel>() { total = count, rows = RList };
            }
        }
        #endregion

        public BsTableDataSource<StatisAppPersonInPostViewModel> GetStatisAppPersonInPost(GetStatisAppPersonInPost request)
        {
            List<StatisAppPersonInPostViewModel> _list = new List<StatisAppPersonInPostViewModel>();
            StatisAppPersonInPostViewModel _model = null;
            List<ADCDInfo> f = null;
            using (var db = DbFactory.Open())
            {
                switch (RowID)
                {
                    case 5:
                        f = db.Select<ADCDInfo>(w => w.grade > 0).OrderBy(o => o.adcd).ToList();
                        break;
                    case 2:
                        f = db.Select<ADCDInfo>(w => w.adcd.StartsWith(adcd.Substring(0, 4)) && w.grade > 0).OrderBy(o => o.adcd).ToList();
                        break;
                }
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                //村责任人
                var villagePersonList = db.SqlList<AllPersonListViewModel>("EXEC GetVillagePersonByYear @year", new { year = _year });
                //到岗位人数
                var dgperson = db.Select<AllPersonListViewModel>("select distinct adcd from AppRecord");
                //市
                var cityList = f.Where(w => w.grade == 1).ToList();
                var num = 0;
                List<int> numList = new List<int>();
                cityList.ForEach(w =>
                {
                    //县
                    var countyList = f.Where(x => x.parentId == w.Id).ToList();
                    var nums = 1;
                    countyList.ForEach(x =>
                    {
                        _model = new StatisAppPersonInPostViewModel();
                        _model.cityName = w.adnm;
                        _model.countyName = x.adnm;
                        _model.countyadcd = x.adcd;
                        _model.townNum = f.Where(y => y.parentId == x.Id).Count();
                        _model.villageNum = f.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.grade == 4).Count();
                        _model.villagePersonNum = villagePersonList.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6))).Count();
                        _model.villagePersonInPostNum = dgperson.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6))).Count();
                        _list.Add(_model);
                        nums++;
                    });
                    //合计
                    _model = new StatisAppPersonInPostViewModel();
                    _model.cityName = w.adnm;
                    _model.countyName = "小计";
                    _model.townNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.townNum);
                    _model.villageNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.villageNum);
                    _model.villagePersonNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.villagePersonNum);
                    _model.villagePersonInPostNum = _list.Where(x => x.cityName == w.adnm).Sum(y => y.villagePersonInPostNum);
                    if (num == 0) { _model.numstr = "0," + nums; numList.Add(nums); }
                    else
                    {
                        _model.numstr = numList[num - 1] + "," + nums; numList.Add(numList[num - 1] + nums);
                    }
                    num++;
                    _list.Add(_model);
                });
                //if(cityList.Count >1)
                //{
                //    StatisAppPersonInPostViewModel almodel = new StatisAppPersonInPostViewModel();
                //    almodel.cityName = "全省";
                //    almodel.countyName = "汇总";
                //    _list.Add(almodel);
                //}
            }
            return new BsTableDataSource<StatisAppPersonInPostViewModel>() { total = _list.Count, rows = _list };
        }

        public BsTableDataSource<StatiscPersonInPost> StatisVillagePersonInPostByCountyAdcd(StatisVillagePersonInPostByCountyAdcd request)
        {
            if (string.IsNullOrEmpty(request.adcd)) throw new Exception("adcd不能为空！");
            var _year = request.year == null ? DateTime.Now.Year : request.year;
            var _adcd = "";
            if (!string.IsNullOrEmpty(request.villageadcd))
            {
                _adcd = request.villageadcd.Substring(0, 12);
            }
            else if (!string.IsNullOrEmpty(request.townadcd))
            {
                _adcd = request.townadcd.Substring(0, 9);
            }
            else
            {
                _adcd = request.adcd.Substring(0, 6);
            }
            using (var db = DbFactory.Open())
            {
                List<StatiscPersonInPost> _list = new List<StatiscPersonInPost>();
                //县级在人员列表
                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex - 1) * PageSize;
                var list = db.SqlList<StatiscPersonInPost>("EXEC AppVillagePersonInPostByCountyAdcd @adcd,@year,@ptype,@sindex,@eindex", new { adcd = _adcd, year = _year, ptype = 0,sindex= PageIndex, eindex= PageSize });
                var count= db.SqlList<StatiscPersonInPost>("EXEC AppVillagePersonInPostByCountyAdcd @adcd,@year,@ptype,@sindex,@eindex", new { adcd = _adcd, year = _year, ptype=999, sindex = 0, eindex = 0}).Select(w => w.adcd).Distinct().Count();
                //人员去重
                var personlist = list.Select(w => w.personLiable).Distinct().ToList();
                //履职情况
                var apprecodeList = db.Select<AppRecord>(w => w.adcd.StartsWith(request.adcd.Substring(0, 6))).OrderByDescending(o => o.addtime).ToList();
                //岗位
                var apppostlist = Enum.GetNames(typeof(Model.Enums.GrassrootsFloodCtrlEnums.AppPost)).Cast<string>().ToList();
                StatiscPersonInPost tplvm = null; StatiscPersonInPost personinfo = null;
                personlist.ForEach(w =>
                {
                    tplvm = new StatiscPersonInPost();
                    //岗位统计
                    var postlist = list.Where(x => x.personLiable == w).Select(x => x.post).Distinct().ToList();
                    //岗位校验
                    var listapp = new List<string>();
                    postlist.ForEach(x =>
                    {
                        if (apppostlist.Contains(x) && !listapp.Contains(x)) { listapp.Add(x); }
                        else if ((x == "预警员" || x == "监测员") && !listapp.Contains(Model.Enums.GrassrootsFloodCtrlEnums.AppPost.监测预警组.ToString())) { listapp.Add(Model.Enums.GrassrootsFloodCtrlEnums.AppPost.监测预警组.ToString()); }
                        else { }
                    });
                    tplvm.apppost = string.Join(",", listapp);
                    tplvm.apppostcount = listapp.Count();
                    //
                    personinfo = list.Find(y => y.personLiable == w);
                    tplvm.personLiable = personinfo.personLiable;
                    tplvm.adcd = personinfo.adcd;
                    tplvm.adnm = personinfo.adnm;
                    tplvm.handPhone = personinfo.handPhone;
                    tplvm.lat = personinfo.lat;
                    tplvm.lng = personinfo.lng;
                    var atime = apprecodeList.Find(x => x.adcd == personinfo.adcd).addtime;
                    tplvm.addtime = atime == null?"":atime.ToString();
                    _list.Add(tplvm);
                    //
                });
                return new BsTableDataSource<StatiscPersonInPost>() { rows = _list, total = count};
            }
        }

        /// <summary>
        /// 防汛任务统计
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<StatisPrevFloodViewModel> GetStatisPrevFlood(GetStatisPrevFlood request)
        {
            List<StatisPrevFloodViewModel> listFlood = new List<StatisPrevFloodViewModel>();
            StatisPrevFloodViewModel model = null;
            List<ADCDInfo> abcdmodel = null;  //行政区划信息
            var adcdvalue = request.adcd;
            using (var db = DbFactory.Open())
            {       
                
                switch (RowID)
                {                 
                    case 5://省级             
                        abcdmodel = db.Select<ADCDInfo>(t => t.grade > 0).OrderBy(t => t.adcd).ToList();
                        break;
                    case 2://市级
                        abcdmodel = db.Select<ADCDInfo>(t => t.adcd.StartsWith(adcd.Substring(0, 4)) && t.grade > 0).OrderBy(m => m.adcd).ToList();
                        break;
                    case 3://县级
                        abcdmodel = db.Select<ADCDInfo>(t => t.adcd.StartsWith(adcd.Substring(0, 6)) && t.grade > 0).OrderBy(m => m.adcd).ToList();
                        break;
                }
                if ( !string.IsNullOrEmpty(request.adcd)) //查询条件非空
                {   
                    //市
                    var cityListtmp = abcdmodel.Where(w => w.grade == 1).ToList();
                    //if (RowID == 3)//县级用户
                    //{
                    //    cityListtmp = abcdmodel.Where(w => w.grade == 2).ToList();
                    //}
                    var cityList = cityListtmp.Where(t => t.adcd == adcdvalue).ToList();
                    var num = 0;      
                    var yearDate = request.year == null ? DateTime.Now.Year : request.year;//年份        
                    var villagePersonList = db.SqlList<AllPersonListViewModel>("EXEC GetVillagePersonByYear @year", new { year = yearDate }); //村责任人           
                    var dgperson = db.Select<AllPersonListViewModel>("select distinct adcd from AppRecord");//到岗位人数
                    var floodIsHeavyOrLightList = db.Select<AllPersonListViewModel>("select adcd,FXFTRW from ADCDDisasterInfo");//防汛轻重
                    List<int> numList = new List<int>();
                    cityList.ForEach(w =>
                    {
                        //县
                        var countyList = abcdmodel.Where(x => x.parentId == w.Id).ToList();
                        var nums = 1;
                        countyList.ForEach(x =>
                        {
                            model = new StatisPrevFloodViewModel();
                            model.cityName = w.adnm;
                            model.countyName = x.adnm;
                            model.countyadcd = x.adcd;
                            model.townNum = abcdmodel.Where(y => y.parentId == x.Id).Count();
                            model.villageNum = abcdmodel.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.grade == 4).Count();
                            //防汛任务较轻
                            model.prevFloodTaskLight = floodIsHeavyOrLightList.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.FXFTRW == "较轻").Count();
                            //防汛任务较重
                            model.prevFloodTaskHeavy = floodIsHeavyOrLightList.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.FXFTRW == "较重").Count();
                            listFlood.Add(model);
                            nums++;
                        });
                        //合计
                        model = new StatisPrevFloodViewModel();
                        model.cityName = w.adnm;
                        model.countyName = "小计";
                        model.townNum = listFlood.Where(x => x.cityName == w.adnm).Sum(y => y.townNum);
                        model.villageNum = listFlood.Where(x => x.cityName == w.adnm).Sum(y => y.villageNum);
                        //防汛任务较轻
                        model.prevFloodTaskLight = listFlood.Where(x => x.cityName == w.adnm).Sum(y => y.prevFloodTaskLight);
                        //防汛任务较重
                        model.prevFloodTaskHeavy = listFlood.Where(x => x.cityName == w.adnm).Sum(y => y.prevFloodTaskHeavy);
                        if (num == 0) { model.numstr = "0," + nums; numList.Add(nums); }
                        else
                        {
                            model.numstr = numList[num - 1] + "," + nums; numList.Add(numList[num - 1] + nums);
                        }
                        num++;
                        listFlood.Add(model);
                    });
                }
                else //查询条件为空
                {
                    if(RowID == 2  || RowID == 5)
                    {
                        //市
                        var cityList = abcdmodel.Where(w => w.grade == 1).ToList();
                        var num = 0;
                        //年份
                        var yearDate = request.year == null ? DateTime.Now.Year : request.year;
                        //村责任人
                        var villagePersonList = db.SqlList<AllPersonListViewModel>("EXEC GetVillagePersonByYear @year", new { year = yearDate });
                        //到岗位人数
                        var dgperson = db.Select<AllPersonListViewModel>("select distinct adcd from AppRecord");
                        //防汛轻重
                        var floodIsHeavyOrLightList = db.Select<AllPersonListViewModel>("select adcd,FXFTRW from ADCDDisasterInfo");

                        List<int> numList = new List<int>();

                        cityList.ForEach(w =>
                        {
                            //县
                            var countyList = abcdmodel.Where(x => x.parentId == w.Id).ToList();
                            var nums = 1;
                            countyList.ForEach(x =>
                            {
                                model = new StatisPrevFloodViewModel();
                                model.cityName = w.adnm;
                                model.countyName = x.adnm;
                                model.countyadcd = x.adcd;
                                model.townNum = abcdmodel.Where(y => y.parentId == x.Id).Count();
                                model.villageNum = abcdmodel.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.grade == 4).Count();
                                //防汛任务较轻
                                model.prevFloodTaskLight = floodIsHeavyOrLightList.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.FXFTRW == "较轻").Count();
                                //防汛任务较重
                                model.prevFloodTaskHeavy = floodIsHeavyOrLightList.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.FXFTRW == "较重").Count();
                                listFlood.Add(model);
                                nums++;
                            });
                            //合计
                            model = new StatisPrevFloodViewModel();
                            model.cityName = w.adnm;
                            model.countyName = "小计";
                            model.townNum = listFlood.Where(x => x.cityName == w.adnm).Sum(y => y.townNum);
                            model.villageNum = listFlood.Where(x => x.cityName == w.adnm).Sum(y => y.villageNum);
                            //防汛任务较轻
                            model.prevFloodTaskLight = listFlood.Where(x => x.cityName == w.adnm).Sum(y => y.prevFloodTaskLight);
                            //防汛任务较重
                            model.prevFloodTaskHeavy = listFlood.Where(x => x.cityName == w.adnm).Sum(y => y.prevFloodTaskHeavy);
                            if (num == 0) { model.numstr = "0," + nums; numList.Add(nums); }
                            else
                            {
                                model.numstr = numList[num - 1] + "," + nums; numList.Add(numList[num - 1] + nums);
                            }
                            num++;
                            listFlood.Add(model);
                        });

                        if(cityList.Count > 1)
                        {
                            StatisPrevFloodViewModel almodel = new StatisPrevFloodViewModel();
                            almodel.cityName = "全省";
                            almodel.countyName = "汇总";
                            almodel.townNum = listFlood.Where(x => x.countyName == "小计").Sum(y => y.townNum);
                            almodel.villageNum = listFlood.Where(x => x.countyName == "小计").Sum(y => y.villageNum);
                            almodel.prevFloodTaskLight = listFlood.Where(x => x.countyName == "小计").Sum(y => y.prevFloodTaskLight);
                            almodel.prevFloodTaskHeavy = listFlood.Where(x => x.countyName == "小计").Sum(y => y.prevFloodTaskHeavy);
                            almodel.numstr = (numList[num - 1]) + "," + 1;
                            listFlood.Add(almodel);

                        }
                    }
                    
                    if(RowID == 3)
                    {
                        //防汛轻重
                        var floodIsHeavyOrLightList = db.Select<AllPersonListViewModel>("select adcd,FXFTRW from ADCDDisasterInfo");
                        List<int> numList = new List<int>();
                        var num = 0;
                        //县
                        var countyList = abcdmodel.Where(w => w.grade == 2).ToList();
                        countyList.ForEach(w =>
                        {
                            var townList = abcdmodel.Where(x => x.parentId == w.Id).ToList(); //镇的集合
                            var nums = 1;
                            townList.ForEach(x =>
                            {
                                model = new StatisPrevFloodViewModel();
                                model.countyName = w.adnm; //县的名字
                                model.countyadcd = w.adcd;//县的adcd
                                model.townName = x.adnm;  //镇的名字
                                model.townadcd = x.adcd;
                                model.id = x.Id; //镇的Id
                                //model.villageNum = abcdmodel.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 6)) && y.grade == 4).Count();
                                model.villageNum = abcdmodel.Where(y => y.parentId.Equals(x.Id) && y.grade == 4).Count(); //镇下面的村数量
                                //防汛任务较轻
                                model.prevFloodTaskLight = floodIsHeavyOrLightList.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 9)) && y.FXFTRW == "较轻").Count();
                                //防汛任务较重
                                model.prevFloodTaskHeavy = floodIsHeavyOrLightList.Where(y => y.adcd.StartsWith(x.adcd.Substring(0, 9)) && y.FXFTRW == "较重").Count();
                                listFlood.Add(model);
                                nums++;
                            });
                            //合计
                            model = new StatisPrevFloodViewModel();
                            model.countyName = w.adnm;
                            model.townName = "小计";
                            model.townNum = listFlood.Where(x => x.countyName == w.adnm).Sum(y => y.townNum);
                            model.villageNum = listFlood.Where(x => x.countyName == w.adnm).Sum(y => y.villageNum);
                            //防汛任务较轻
                            model.prevFloodTaskLight = listFlood.Where(x => x.countyName == w.adnm).Sum(y => y.prevFloodTaskLight);
                            //防汛任务较重
                            model.prevFloodTaskHeavy = listFlood.Where(x => x.countyName == w.adnm).Sum(y => y.prevFloodTaskHeavy);
                            if (num == 0) { model.numstr = "0," + nums; numList.Add(nums); }
                            else
                            {
                                model.numstr = numList[num - 1] + "," + nums; numList.Add(numList[num - 1] + nums);
                            }
                            num++;
                            listFlood.Add(model);

                        });
                    }

                }
            }
            return new BsTableDataSource<StatisPrevFloodViewModel>() { total = listFlood.Count, rows = listFlood };
        }

        /// <summary>
        /// 县级乡镇防汛详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<CountyFloodDetailViewModel> GetCountyFloodDetail(GetCountyFloodDetail request)
        {

            if(string.IsNullOrEmpty(request.countyadcd))
            {
                throw new Exception("adcd不能为空!");
            }
            int PageSize = request.PageSize == 0 ? 10 : request.PageSize;
            int PageIndex = request.PageIndex == 0 ? 0 : request.PageIndex;
            //var year = request.year == null? DateTime.Now.Year : request.year;
            List<CountyFloodDetailViewModel> list = new List<CountyFloodDetailViewModel>();
            CountyFloodDetailViewModel model = null;
            List<ADCDInfo> adcdModel = null;
            using (var db = DbFactory.Open())
            {
                List<AllPersonListViewModel> floodIsHeavyOrLightList = new List<AllPersonListViewModel>();
                //防汛轻重              
                if(request.floodTask == "1")
                {
                    floodIsHeavyOrLightList = db.Select<AllPersonListViewModel>(" select adcd,FXFTRW from ADCDDisasterInfo WHERE FXFTRW ='较轻' ");        
                    List<int> numList = new List<int>();
                    adcdModel = db.Select<ADCDInfo>(t => t.adcd.StartsWith(adcd.Substring(0, 6)) && t.grade > 0).OrderBy(m => m.adcd).ToList();
                    //县
                    var countyList = adcdModel.Where(w => w.grade == 2).ToList();
                    List<ADCDInfo> townList = new List<ADCDInfo>();
                    countyList.ForEach(w => {
                        if (string.IsNullOrEmpty(request.townadcd))
                        {
                            townList = adcdModel.Where(x => x.parentId == w.Id)
                            .OrderBy(m => m.adcd)
                            .ToList(); //镇的集合
                        }
                        else
                        {
                            townList = db.Select<ADCDInfo>(t => t.adcd.StartsWith(request.townadcd.Substring(0, 9)) && t.grade > 0)
                            .OrderBy(m => m.adcd)
                            .ToList();//筛选后的镇集合
                        }


                        townList.ForEach(x => {
                            List<ADCDInfo> villageList = new List<ADCDInfo>();
                            ADCDInfo e = new ADCDInfo();
                            floodIsHeavyOrLightList.ForEach(m => {
                                e = adcdModel.Where(k => k.parentId == x.Id && k.adcd.Equals(m.adcd)).FirstOrDefault();//村的集合
                                if(e != null)
                                {
                                    villageList.Add(e);
                                }
                              
                            }); 
                            var villageAssemble = villageList.OrderBy(m => m.adcd)
                                                  .ToList();
                            var nums = 1;
                            villageAssemble.ForEach(k => {
                                model = new CountyFloodDetailViewModel();
                                model.townName = x.adnm;//镇的名字
                                model.villageName = k.adnm; //村的名字
                                int floodcount = floodIsHeavyOrLightList.Where(y => y.adcd.Equals(k.adcd) && y.FXFTRW != null).Count();
                                if (floodcount > 0)
                                {

                                    model.prevFloodTask = floodIsHeavyOrLightList.Where(y => y.adcd.Equals(k.adcd)).FirstOrDefault().FXFTRW.ToString();
                                }
                                else
                                {
                                    model.prevFloodTask = "";
                                }
                                list.Add(model);
                                nums++;
                            });
                        });

                    });
                }
                else if(request.floodTask == "2")
                {
                    floodIsHeavyOrLightList = db.Select<AllPersonListViewModel>(" select adcd,FXFTRW from ADCDDisasterInfo WHERE FXFTRW ='较重' ");
                }
                else
                {
                    floodIsHeavyOrLightList = db.Select<AllPersonListViewModel>("select adcd,FXFTRW from ADCDDisasterInfo");
                    List<int> numList = new List<int>();
                    adcdModel = db.Select<ADCDInfo>(t => t.adcd.StartsWith(adcd.Substring(0, 6)) && t.grade > 0).OrderBy(m => m.CreateTime).ToList();
                    //县
                    var countyList = adcdModel.Where(w => w.grade == 2).ToList();
                    List<ADCDInfo> townList = new List<ADCDInfo>();
                    countyList.ForEach(w => {
                        if (string.IsNullOrEmpty(request.townadcd))
                        {
                            townList = adcdModel.Where(x => x.parentId == w.Id).ToList(); //镇的集合
                        }
                        else
                        {
                            townList = db.Select<ADCDInfo>(t => t.adcd.StartsWith(request.townadcd.Substring(0, 9)) && t.grade > 0).OrderBy(m => m.CreateTime).ToList();//筛选后的镇集合
                        }


                        townList.ForEach(x => {
                            var villageList = adcdModel.Where(k => k.parentId == x.Id).ToList(); //村的集合
                            var nums = 1;
                            villageList.ForEach(k => {
                                model = new CountyFloodDetailViewModel();
                                model.townName = x.adnm;//镇的名字
                                model.villageName = k.adnm; //村的名字
                                int floodcount = floodIsHeavyOrLightList.Where(y => y.adcd.Equals(k.adcd) && y.FXFTRW != null).Count();
                                if (floodcount > 0)
                                {

                                    model.prevFloodTask = floodIsHeavyOrLightList.Where(y => y.adcd.Equals(k.adcd)).FirstOrDefault().FXFTRW.ToString();
                                }
                                else
                                {
                                    model.prevFloodTask = "";
                                }
                                list.Add(model);
                                nums++;
                            });
                        });
                       
                        //List<ADCDInfo> villageAssemble = new List<ADCDInfo>();
                        //townList.ForEach(x => {
                        //    villageAssemble = adcdModel.Where(k => k.parentId == x.Id).ToList(); //村的集合
                        //});
                        //villageAssemble.ForEach
                    });
                }
             
            }
            //分页
            var resultlist = list.Skip(PageSize * (PageIndex - 1)).Take(PageSize).ToList();
            return new BsTableDataSource<CountyFloodDetailViewModel>() { total = list.Count, rows = resultlist };
           
        }
    }
}
