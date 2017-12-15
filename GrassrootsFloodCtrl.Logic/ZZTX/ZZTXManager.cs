using Dy.Common;
using GrassrootsFloodCtrl.Logic.Common;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.Model.Town;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStackForLeafletjsResponse;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;

namespace GrassrootsFloodCtrl.Logic.ZZTX
{
    /// <summary>
    /// 组织责任
    /// </summary>
    public class ZZTXManager : ManagerBase, IZZTXManager
    {
        public ILogHelper _ILogHelper { get; set; }

        /// <summary>
        /// 根据id获取单个行区划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ADCDInfo GetADCDInfoById(int id)
        {
            using (var db = DbFactory.Open())
            {
                return db.Single<ADCDInfo>(x => x.Id == id);
            }
        }

        /// <summary>
        /// 根据行区划编码获取单个行区划信息
        /// </summary>
        /// <param name="adcd"></param>
        /// <returns></returns>
        public ADCDInfo GetADCDInfoByADCD(string _adcd)
        {
            var newadcd = adcd != _adcd ? adcd : _adcd;
            using (var db = DbFactory.Open())
            {
                return db.Single<ADCDInfo>(x => x.adcd == newadcd);
            }
        }

        /// <summary>
        /// 保存行区划信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool SaveADCDInfo(SaveADCDInfo request)
        {
            using (var db = DbFactory.Open())
            {
                var info = new ADCDInfo();
                //info.adcd = request.adcd;
                info.adnm = request.adnm;
                if (request.lng != null)
                    info.lng = request.lng;
                if (request.lat != null)
                    info.lat = request.lat;
                try
                {
                    info.parentId = db.Single<ADCDInfo>(w => w.adcd == adcd).Id;
                    info.grade = 4;
                }
                catch (Exception ex) { }
                operateLog log = new operateLog();
                log.userName = RealName;
                log.operateTime = DateTime.Now;
                if (string.IsNullOrEmpty(request.adnm)) throw new Exception("行政村名不能为空！");

                if (request.id != null)
                {
                    var f = db.Single<ADCDInfo>(w => w.adnm == request.adnm && w.adcd == adcd && w.lat == request.lat && w.lng == w.lng);
                    if (null != f && f.lat == request.lat && f.lng == request.lng) throw new Exception("行政村名已存在！");
                    info.Id = request.id.Value;
                    info.adcd = request.adcd;
                    log.operateMsg = "更新" + request.adnm + "信息";
                    List<operateLog> listLog = new List<operateLog>();
                    listLog.Add(log);
                    info.operateLog = JsonTools.ObjectToJson(listLog);
                    #region 日志
                    var _f = db.Single<ADCDInfo>(w => w.Id == request.id);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("在栏目{行政区划}下,修改了数据{");
                    sb.Append("行政区划名称：" + _f.adnm + ";");
                    sb.Append("经度：" + _f.lng + ";");
                    sb.Append("纬度：" + _f.lat + ";");
                    sb.Append("}为{");
                    sb.Append("行政区划名称：" + info.adnm + ";");
                    sb.Append("经度：" + info.lng + ";");
                    sb.Append("纬度：" + info.lat + ";");
                    sb.Append("}");
                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
                    #endregion 日志
                    return db.Update(info) == 1;
                }
                else
                {
                    var f = db.Single<ADCDInfo>(w => w.adnm == request.adnm && w.adcd == adcd);
                    if (null != f) throw new Exception("行政村名已存在！");
                    info.CreateTime = DateTime.Now;
                    var list = db.Select<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9))).OrderByDescending(x => x.adcd).ToList();
                    if (list.Count != 0)
                        info.adcd = (long.Parse(list[0].adcd.Substring(0, 12)) + 1) + list[0].adcd.Substring(12, 3);
                    else
                        info.adcd = adcd.Substring(0, 9) + "001000";
                    log.operateMsg = "新增" + request.adnm + "信息";
                    List<operateLog> listLog = new List<operateLog>();
                    listLog.Add(log);
                    info.operateLog = JsonTools.ObjectToJson(listLog);
                    #region 日志
                    StringBuilder sb = new StringBuilder();
                    sb.Append("在栏目{行政区划}下,新增了数据{");
                    sb.Append("行政区划名称：" + info.adnm + ";");
                    sb.Append("经度：" + info.lng + ";");
                    sb.Append("纬度：" + info.lat + ";");
                    sb.Append("}");
                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
                    #endregion 日志
                    var success = db.Insert(info) == 1;
                    if (success)
                    {
                        var adcdDisasterInfo = new ADCDDisasterInfo();
                        adcdDisasterInfo.adcd = info.adcd;
                        adcdDisasterInfo.Year = DateTime.Now.Year;
                        adcdDisasterInfo.CreateTime = DateTime.Now;
                        db.Insert(adcdDisasterInfo);
                    }
                    return success;
                }
            }
        }

        /// <summary>
        /// 获取行区划信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<ADCDInfo> GetADCDInfo(GetADCDInfo request)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<ADCDInfo>();
                switch (request.levle)
                {
                    case 0://全部
                        break;

                    case 1://省
                        builder.Where(x => x.adcd == "330000000000000");
                        break;

                    case 2://市
                        switch (RowID)
                        {
                            case 2:
                                builder.Where("adcd ='" + adcd + "'");//x => x.adcd != "330000000000000" && x.adcd.EndsWith("00"));
                                break;

                            case 5:
                                builder.Where("adcd like '33__00000000000' and adcd !='330000000000000'");//x => x.adcd != "330000000000000" && x.adcd.EndsWith("00"));
                                break;
                        }
                        //builder.And("len(adcd)=6");
                        break;

                    case 3://县（区）
                        builder.Where("adcd like '33____000000000'  and adcd not like '33__00000000000'");
                        //builder.Where(x => x.adcd != "330000"&&!x.adcd.EndsWith("00"));
                        //builder.And("len(adcd)=6");
                        if (!string.IsNullOrEmpty(request.adcd) && request.adcd.Length == 15 && request.adcd.IndexOf("000000000") > 0)
                            builder.And("adcd like '" + request.adcd.Substring(0, 4) + "__000000000' and adcd !='" + request.adcd + "'");
                        break;

                    case 4://镇街
                        builder.Where("adcd like '%000000' and adcd not like '%000000000'");
                        //builder.Where(x => x.adcd.EndsWith("000000"));
                        if (!string.IsNullOrEmpty(request.adcd))
                            builder.And(x => x.adcd.StartsWith(request.adcd.Substring(0, 6)) && x.adcd != request.adcd);
                        break;

                    case 5://行政村
                        builder.Where(x => !x.adcd.EndsWith("000000") && x.adcd.EndsWith("000"));
                        builder.And("len(adcd)=15");
                        if (!string.IsNullOrEmpty(request.adcd))
                            builder.And(x => x.adcd == request.adcd);
                        else
                            builder.And(x => x.adcd.Contains(adcd.Substring(0, 9)));
                        break;

                    case 51://行政村1
                        builder.Where(x => x.adcd.Contains(request.adcd.Substring(0, 9)) && x.adcd.Substring(10, 3) != "000");
                        break;
                        //case 6://自然村
                        //    builder.Where(x => x.adcd.Length == 15 && x.adcd.Substring(12, 3) != "000");
                        //    break;
                }
                if (!string.IsNullOrEmpty(request.adnm))
                    builder.And(x => x.adnm.Contains(request.adnm));
                var count = db.Count(builder);

                var rows = request.PageSize == 0 ? 10 : request.PageSize;
                var skip = request.PageIndex == 0 ? 0 : request.PageIndex * rows;

                builder.Limit(skip, rows);
                builder.OrderBy(x => x.adcd);
                var list = db.Select(builder);
                return new BsTableDataSource<ADCDInfo>() { total = count, rows = list };
            }
        }

        /// <summary>
        /// 抽查考核
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<ADCDTree> GetADCDInfoTree(GetADCDInfoTree request)
        {
            List<ADCDTree> ladcdall = new List<ADCDTree>();
            using (var db = DbFactory.Open())
            {
                var _adcd = string.IsNullOrEmpty(request.adcd) ? adcd : request.adcd;
                ADCDTree adcdt = null;
                switch (RowID)
                {
                    case (int)GrassrootsFloodCtrlEnums.RoleEnums.省级用户://省
                                                                      // var city = db.Select<ADCDInfo>("select * from ADCDInfo where adcd like '33__00000000000' and adcd !='330000000000000'");
                                                                      //   var county = db.Select<ADCDInfo>("select * from ADCDInfo where adcd like '33____000000000'  and adcd not like '33__00000000000'");
                                                                      //var town = db.Select<ADCDInfo>("select * from ADCDInfo where adcd like '%000000' and adcd not like '%000000000'");
                                                                      //
                        var provice = db.Select<ADCDInfo>(w => w.parentId > 0).OrderBy(w => w.adcd).ToList();
                        provice.ForEach(w =>
                        {
                            adcdt = new ADCDTree()
                            {
                                adcd = w.adcd,
                                id = w.Id,
                                level = w.grade,
                                name = w.adnm,
                                pId = w.parentId
                            };

                            //switch (w.grade)
                            //{
                            //    case 2:
                            //        var f = city.Single(x => x.adcd == w.adcd.Substring(0, 4) + "00000000000");
                            //        adcdt.areapath = f.adnm;
                            //        break;
                            //    case 3:
                            //        var fcity = city.Single(x => x.adcd == w.adcd.Substring(0, 4) + "00000000000");
                            //        var fcounty = county.Single(x => x.adcd == w.adcd.Substring(0, 6) + "000000000");
                            //        adcdt.areapath = fcity.adnm + "_" + fcounty.adnm;
                            //        break;
                            //    case 4:
                            //        var fcityv = city.Single(x => x.adcd == w.adcd.Substring(0, 4) + "00000000000");
                            //        var fcountyv = county.Single(x => x.adcd == w.adcd.Substring(0, 6) + "000000000");
                            //        var vftown = town.Single(x => x.adcd == w.adcd.Substring(0, 9) + "000000");
                            //        adcdt.areapath = fcityv.adnm + "_" + fcountyv.adnm + "_" + vftown.adnm;
                            //        break;
                            //}
                            ladcdall.Add(adcdt);
                        });
                        #region
                        //var provice = db.Single<ADCDInfo>(w => w.adcd == _adcd);
                        ////省的父id
                        //db.UpdateOnly<ADCDInfo>(() => new ADCDInfo { parentId = 0 }, w => w.adcd == _adcd);
                        //var city = db.Select<ADCDInfo>("select * from ADCDInfo where adcd like '33__00000000000' and adcd !='330000000000000'");
                        //var county = db.Select<ADCDInfo>("select * from ADCDInfo where adcd like '33____000000000'  and adcd not like '33__00000000000'");
                        //var town = db.Select<ADCDInfo>("select * from ADCDInfo where adcd like '%000000' and adcd not like '%000000000'");
                        //var village = db.Select<ADCDInfo>("select * from ADCDInfo where CONVERT(int,SUBSTRING(adcd,10,3)) > 0 and  adcd like '%000'");
                        //city.ForEach(w =>
                        //{
                        //    adcdt = new ADCDTree()
                        //    {
                        //        adcd = w.adcd,
                        //        id = w.Id,
                        //        level = 1,
                        //        name = w.adnm,
                        //        pId = provice.Id
                        //        // areapath = provice.adnm
                        //    };
                        //    ladcdall.Add(adcdt);
                        //    //更新市级的父id
                        //    db.UpdateOnly<ADCDInfo>(() => new ADCDInfo { parentId = provice.Id,grade = 1 }, x => x.adcd == w.adcd);
                        //    //
                        //});
                        //county.ForEach(w =>
                        //{
                        //    var a = w.adcd.Substring(0, 4) + "00000000000";
                        //    var f = city.Single<ADCDInfo>(x => x.adcd == w.adcd.Substring(0, 4) + "00000000000");
                        //    adcdt = new ADCDTree()
                        //    {
                        //        adcd = w.adcd,
                        //        id = w.Id,
                        //        level = 2,
                        //        name = w.adnm,
                        //        pId = f.Id,
                        //        areapath = f.adnm
                        //        // areapath = provice.adnm + "_" + f.adnm
                        //    };
                        //    ladcdall.Add(adcdt);
                        //    //更新县级的父id
                        //    db.UpdateOnly<ADCDInfo>(() => new ADCDInfo { parentId = f.Id,grade = 2 }, x => x.adcd == w.adcd);
                        //    //
                        //});
                        //town.ForEach(w =>
                        //{
                        //    var fcity = city.Single<ADCDInfo>(x => x.adcd == w.adcd.Substring(0, 4) + "00000000000");
                        //    var fcounty = county.Single<ADCDInfo>(x => x.adcd == w.adcd.Substring(0, 6) + "000000000");
                        //    adcdt = new ADCDTree()
                        //    {
                        //        adcd = w.adcd,
                        //        id = w.Id,
                        //        level = 3,
                        //        name = w.adnm,
                        //        pId = fcounty.Id,
                        //        //areapath = provice.adnm + "_" + fcity.adnm + "_" + fcounty.adnm
                        //        areapath = fcity.adnm + "_" + fcounty.adnm
                        //    };
                        //    ladcdall.Add(adcdt);
                        //    //更新镇级的父id
                        //    db.UpdateOnly<ADCDInfo>(() => new ADCDInfo { parentId = fcounty.Id,grade = 3 }, x => x.adcd == w.adcd);
                        //    //
                        //});
                        //village.ForEach(w =>
                        //{
                        //    var fcity = city.Single<ADCDInfo>(x => x.adcd == w.adcd.Substring(0, 4) + "00000000000");
                        //    var fcounty = county.Single<ADCDInfo>(x => x.adcd == w.adcd.Substring(0, 6) + "000000000");
                        //    var vftown = town.Single<ADCDInfo>(x => x.adcd == w.adcd.Substring(0, 9) + "000000");
                        //    adcdt = new ADCDTree()
                        //    {
                        //        adcd = w.adcd,
                        //        id = w.Id,
                        //        level = 4,
                        //        name = w.adnm,
                        //        pId = vftown.Id,
                        //        //areapath = provice.adnm + "_" + fcity.adnm + "_" + fcounty.adnm + "_" + vftown.adnm
                        //        areapath = fcity.adnm + "_" + fcounty.adnm + "_" + vftown.adnm
                        //    };
                        //    ladcdall.Add(adcdt);
                        //    //更新村级的父id
                        //    db.UpdateOnly<ADCDInfo>(() => new ADCDInfo { parentId = vftown.Id,grade = 4 }, x => x.adcd == w.adcd);
                        //    //
                        //});

                        #endregion
                        break;

                    case (int)GrassrootsFloodCtrlEnums.RoleEnums.市级用户://市
                        var fcitys = db.Single<ADCDInfo>(w => w.adcd == _adcd);
                        adcdt = new ADCDTree()
                        {
                            adcd = fcitys.adcd,
                            id = fcitys.Id,
                            level = 0,
                            name = fcitys.adnm,
                            pId = 0,
                            open = true
                        };
                        ladcdall.Add(adcdt);
                        var countys = db.Select<ADCDInfo>(w => w.adcd.Contains(_adcd.Substring(0, 4)) && w.adcd.Contains("000000000") && w.adcd != _adcd);
                        var towns = db.Select<ADCDInfo>("select * from ADCDInfo where CONVERT(int,SUBSTRING(adcd,7,3)) > 0 and SUBSTRING(adcd,1,4) = '" + _adcd.Substring(0, 4) + "' and  adcd like '%000000'");
                        var villages = db.Select<ADCDInfo>("select * from ADCDInfo where CONVERT(int,SUBSTRING(adcd,10,3)) > 0 and SUBSTRING(adcd,1,4) = '" + _adcd.Substring(0, 4) + "' and  adcd like '%000'");
                        countys.ForEach(w =>
                        {
                            var a = w.adcd.Substring(0, 4) + "00000000000";
                            adcdt = new ADCDTree()
                            {
                                adcd = w.adcd,
                                id = w.Id,
                                level = 2,
                                name = w.adnm,
                                pId = fcitys.Id,
                                areapath = fcitys.adnm
                            };
                            ladcdall.Add(adcdt);
                        });
                        towns.ForEach(w =>
                        {
                            var fcounty = countys.Single<ADCDInfo>(x => x.adcd == w.adcd.Substring(0, 6) + "000000000");
                            adcdt = new ADCDTree()
                            {
                                adcd = w.adcd,
                                id = w.Id,
                                level = 3,
                                name = w.adnm,
                                pId = fcounty.Id,
                                areapath = fcitys.adnm + "_" + fcounty.adnm
                            };
                            ladcdall.Add(adcdt);
                        });
                        villages.ForEach(w =>
                        {
                            var fcounty = countys.Single<ADCDInfo>(x => x.adcd == w.adcd.Substring(0, 6) + "000000000");
                            var vftown = towns.Single<ADCDInfo>(x => x.adcd == w.adcd.Substring(0, 9) + "000000");
                            adcdt = new ADCDTree()
                            {
                                adcd = w.adcd,
                                id = w.Id,
                                level = 4,
                                name = w.adnm,
                                pId = vftown.Id,
                                areapath = fcitys.adnm + "_" + fcounty.adnm + "_" + vftown.adnm
                            };
                            ladcdall.Add(adcdt);
                        });
                        break;

                    case (int)GrassrootsFloodCtrlEnums.RoleEnums.县级用户://县（区）
                        var townss = db.Select<ADCDInfo>(w => w.adcd.Contains(_adcd.Substring(0, 6)) && w.adcd.Contains("000000") && w.adcd != _adcd);
                        var villagess = db.Select<ADCDInfo>("select * from ADCDInfo where CONVERT(int,SUBSTRING(adcd,10,3)) > 0 and SUBSTRING(adcd,0,6) = '" + _adcd.Substring(0, 6) + "' and  adcd like '%000'");
                        break;
                }
                return ladcdall;
            }
        }

        /// <summary>
        /// 获取受灾害影响的行政区划信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<ADCDDisasterViewModel> GetADCDDisasterInfo(GetADCDDisasterInfo request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(adcd))
                    throw new Exception("请重新登录");
                var builder = db.From<ADCDDisasterInfo>();
                builder.LeftJoin<ADCDDisasterInfo, ADCDInfo>((x, y) => x.adcd == y.adcd);
                builder.LeftJoin<ADCDDisasterInfo, VillageTransferPerson>((x, y) => x.adcd == y.adcd);
                if (adcd.Length == 15 && adcd.IndexOf("000000") > 0)//登陆的是乡镇用户
                    builder.Where<ADCDInfo>(y => y.adcd.StartsWith(adcd.Substring(0, 9)));
                else if (adcd.Length == 6 && adcd.IndexOf("00") < 0)//登陆的是县级用户
                    builder.Where<ADCDInfo>(y => y.adcd.StartsWith(adcd.Substring(0, 6)));
                else if (adcd.Length == 6 && adcd.IndexOf("0000") < 0 && adcd.IndexOf("00") > 0)//登陆的是市级用户
                    builder.Where<ADCDInfo>(y => y.adcd.StartsWith(adcd.Substring(0, 4)));
                else if (adcd == "330000000000000")//管理员
                {
                }
                else
                    throw new Exception("登陆用户的所属行政区划编码不正确");
                if (!string.IsNullOrEmpty(request.name))
                    builder.And<ADCDInfo>(y => y.adnm.Contains(request.name));
                if (request.year != null)
                    builder.And<ADCDDisasterInfo>(x => x.Year == request.year);
                if (request.id != null)
                    builder.And(x => x.Id == request.id);
                //builder.Select<ADCDDisasterInfo, ADCDInfo>((x, y) => new
                //{
                //    Id = x.Id,
                //    adcd = x.adcd,
                //    adnm = y.adnm,
                //    lng = y.lng,
                //    lat = y.lat,
                //    TotalNum = x.TotalNum,
                //    //PointNum = x.PointNum,
                //    //PopulationNum = x.PopulationNum,
                //    FXFTRW = x.FXFTRW,
                //    Year = x.Year
                //});
                builder.Select("ADCDDisasterInfo.Id,ADCDDisasterInfo.adcd,ADCDInfo.adnm,ADCDInfo.lng,ADCDInfo.lat,ADCDDisasterInfo.TotalNum,ADCDDisasterInfo.FXFTRW,ADCDDisasterInfo.Year ,SUM(VillageTransferPerson.HouseholderNum) PopulationNum,COUNT(VillageTransferPerson.HouseholderNum) PointNum").//,VillageTransferPerson.IfTransfer
                    GroupBy("ADCDDisasterInfo.Id,ADCDDisasterInfo.adcd,ADCDInfo.adnm,ADCDInfo.lng,ADCDInfo.lat,ADCDDisasterInfo.TotalNum,ADCDDisasterInfo.PointNum,ADCDDisasterInfo.PopulationNum,ADCDDisasterInfo.FXFTRW,ADCDDisasterInfo.Year");//,VillageTransferPerson.IfTransfer

                var count = db.Count(builder);
                //if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                //    builder.OrderBy(x => request.Sort);
                //else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) &&
                //         request.Order == "desc")
                //    builder.OrderByDescending(x => request.Sort);
                //else
                //    builder.OrderBy(x=>x.adcd);

                //默认按照未标绘的村和Adcd排序，未标绘的在前面
                builder.OrderBy(x => x.adcd);
                builder.OrderBy<ADCDInfo>(x => x.lng);
                var rows = request.PageSize == 0 ? 10 : request.PageSize;
                var skip = request.PageIndex == 0 ? 0 : (request.PageIndex - 1) * rows;

                builder.Limit(skip, rows);
                var list = db.Select<ADCDDisasterViewModel>(builder);
                return new BsTableDataSource<ADCDDisasterViewModel>() { total = count, rows = list };
            }
        }

        public int GetPointNum(IDbConnection db, string villageadcd, int type)
        {
            var c = 0;
            try
            {
                var builder = db.From<VillageTransferPerson>();
                builder.Where(w => w.adcd == villageadcd);
                if (type == 1)
                {
                    builder.Select("SUM(HouseholderNum) HouseholderNum");
                }
                else
                {
                    builder.Select("COUNT(HouseholderNum) HouseholderNum");
                }
                c = db.Single(builder).HouseholderNum;
            }
            catch (Exception ex)
            {
            }
            return c;
        }

        /// <summary>
        /// 根据乡镇、行政村名称获取受灾害影响的行政区划信息
        /// </summary>
        /// <param name="adnm"></param>
        /// <returns></returns>
        public ADCDInfo GetAdcdInfoByADNM(GetAdcdInfoByADNM request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(request.adcd) || request.adcd.Length != 15 || request.adcd.Replace("000", "").Length != 9)
                    throw new Exception("乡镇编码不正确");
                var builder = db.From<ADCDInfo>();
                builder.LeftJoin<ADCDInfo, ADCDDisasterInfo>((x, y) => x.adcd == y.adcd);
                builder.Where<ADCDInfo>(x => x.adnm == request.adnm);
                builder.And<ADCDDisasterInfo>(y => y.Year == request.Year && y.adcd.StartsWith(request.adcd.Substring(0, 9)));
                return db.Single(builder);
            }
        }

        /// <summary>
        /// 保存行政区划受灾害信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool SaveADCDDisasterInfo(SaveADCDDisasterInfo request)
        {
            using (var db = DbFactory.Open())
            {
                var info = new ADCDDisasterInfo();

                info.PointNum = request.PointNum;
                info.TotalNum = request.TotalNum;
                info.PopulationNum = request.PopulationNum;
                info.Year = request.Year == 0 ? DateTime.Now.Year : request.Year;
                info.FXFTRW = request.FXFTRW;
                operateLog log = new operateLog();
                log.userName = RealName;
                log.operateTime = DateTime.Now;

                if (request.id != null)
                {
                    info.Id = dyConverter.ToInt32(request.id);
                    info.adcd = request.adcd;
                    log.operateMsg = "更新" + request.adnm + "受灾信息";
                    List<operateLog> listLog = new List<operateLog>();
                    listLog.Add(log);
                    info.operateLog = JsonTools.ObjectToJson(listLog);
                    #region 日志
                    var _f = db.Single<ADCDDisasterInfo>(w => w.Id == request.id);
                    var adcdInfo = GetADCDInfoByADCD(request.adcd);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("在栏目{行政区划}下,修改了数据{");
                    sb.Append("村adcd：" + adcdInfo.adnm + ";");
                    sb.Append("防汛任务轻重：" + _f.FXFTRW + ";");
                    sb.Append("}为{");
                    sb.Append("村adcd：" + adcdInfo.adnm + ";");
                    sb.Append("防汛任务轻重：" + info.FXFTRW + ";");
                    sb.Append("}");
                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
                    #endregion
                    return db.Update(info) == 1;
                }
                else
                {
                    var flag = true;
                    var list = db.Select<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9))).OrderByDescending(x => x.adcd).ToList();
                    var result = list.Where(x => x.adnm == request.adnm).ToList();
                    if (result.Count == 1)
                    {
                        info.adcd = result[0].adcd;
                        var adcdDisasterInfo = db.Single<ADCDDisasterInfo>(x => x.adcd == result[0].adcd && x.Year == request.Year);
                        if (adcdDisasterInfo != null)
                            throw new Exception("行政村已存在，请勿重复添加");
                        else
                            flag = false;
                    }
                    else
                        flag = true;

                    info.CreateTime = DateTime.Now;
                    info.Year = DateTime.Now.Year;

                    if (flag)
                    {
                        info.adcd = (long.Parse(list[0].adcd.Substring(0, 12)) + 1) + list[0].adcd.Substring(12, 3);
                        #region 新增行政村信息

                        var model = new SaveADCDInfo();
                        model.adcd = info.adcd;
                        model.adnm = request.adnm;
                        if (request.lng != null)
                            model.lng = request.lng;
                        if (request.lat != null)
                            model.lat = request.lat;
                        SaveADCDInfo(model);

                        #endregion
                    }
                    log.operateMsg = "新增" + request.adnm + "受灾信息";
                    List<operateLog> listLog = new List<operateLog>();
                    listLog.Add(log);
                    info.operateLog = JsonTools.ObjectToJson(listLog);
                    return db.Insert(info) == 1L;
                }
            }
        }

        /// <summary>
        /// 删除受灾害影响的行政区划信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DelADCDDisasterInfo(string ids)
        {
            using (var db = DbFactory.Open())
            {
                ArrayList arr = new ArrayList();
                string[] arrs = ids.Split(',');
                for (int i = 0; i < arrs.Length; i++)
                {
                    var id = int.Parse(arrs[i]);
                    arr.Add(id);
                }
                return db.Delete<ADCDDisasterInfo>(x => Sql.In(x.Id, arr)) > 0;
            }
        }

        public bool DelADCDInfo(DelADCDInfo request)
        {
            if (string.IsNullOrEmpty(request.ids))
            {
                throw new Exception("参数异常！");
            }

            using (var db = DbFactory.Open())
            {
                ArrayList arr = new ArrayList();
                ArrayList arrAdcd = new ArrayList();
                string[] arrs = request.ids.Split(',');
                for (int i = 0; i < arrs.Length; i++)
                {
                    var id = int.Parse(arrs[i]);
                    arr.Add(id);
                    var info = GetADCDInfoById(id);
                    #region 日志
                    StringBuilder sb = new StringBuilder();
                    var datainfo = db.Single<ADCDInfo>(w => w.adcd == info.adcd);
                    sb.Append("在栏目{行政区划}下,删除了村{" + datainfo.adnm + "}及其数据");
                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.删除);
                    #endregion
                    if (info != null)
                        arrAdcd.Add(info.adcd);
                }
                var r = db.Delete<ADCDInfo>(x => Sql.In(x.Id, arr)) > 0;

                if (r)
                {
                    r = db.Delete<ADCDDisasterInfo>(x => Sql.In(x.adcd, arrAdcd)) > 0;
                    r = db.Delete<VillageWorkingGroup>(x => Sql.In(x.VillageADCD, arrAdcd)) > 0;
                    r = db.Delete<VillageGridPersonLiable>(x => Sql.In(x.VillageADCD, arrAdcd)) > 0;
                    r = db.Delete<VillageTransferPerson>(x => Sql.In(x.adcd, arrAdcd)) > 0;
                    r = db.Delete<VillagePic>(x => Sql.In(x.adcd, arrAdcd)) > 0;
                }
                return r;
            }
        }

        /// <summary>
        /// 导入当前年度信息(把上一年度的信息复制到今年)  还要补充
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool ImportCurrentInfo(ImportCurrentInfo request)
        {
            using (var db = DbFactory.Open())
            {
                var resultCount = 0;
                var year = DateTime.Now.Year;

                operateLog log = new operateLog();
                log.userName = RealName;
                log.operateTime = DateTime.Now;

                StringBuilder sb = new StringBuilder();
                //0：行政村信息，1：行政村防汛防台工作组，2：行政村网格责任人，3：行政村危险区转移人员清单，4：行政村防汛防台形势图，5：镇街防汛防台责任人
                switch (request.type)
                {
                    case 0://行政村信息
                        var result = new List<ADCDDisasterInfo>();
                        var list = db.Select<ADCDDisasterInfo>(x => x.Year == (DateTime.Now.Year - 1) && x.adcd == adcd);

                        foreach (var info in list)
                        {
                            info.Year = year;
                            log.operateMsg = "拷贝" + (year - 1) + "年度" + GetADCDInfoByADCD(info.adcd).adnm + "的行政村信息到" + year + "年度";
                            List<operateLog> listLog = new List<operateLog>();
                            listLog.Add(log);
                            info.operateLog = JsonTools.ObjectToJson(listLog);
                            result.Add(info);
                        }
                        resultCount = db.SaveAll(result);
                        #region 日志
                        sb.Append("在栏目{行政村信息}下,拷贝{" + (year - 1) + "}年度的行政村信息{" + resultCount + "}条到{" + year + "}年度");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
                        #endregion
                        break;

                    case 1://行政村防汛防台工作组
                        var VillageWorkingGroupResult = new List<VillageWorkingGroup>();
                        var VillageWorkingGroupList = db.Select<VillageWorkingGroup>(x => x.Year == (year - 1) && x.VillageADCD.StartsWith(adcd.Substring(0, 9)) && x.VillageADCD != adcd);

                        foreach (var info in VillageWorkingGroupList)
                        {
                            info.Year = year;
                            log.operateMsg = "拷贝" + (year - 1) + "年度" + GetADCDInfoByADCD(info.VillageADCD).adnm + info.PersonLiable + "的行政村防汛防台工作组信息到" + year + "年度";
                            List<operateLog> listLog = new List<operateLog>();
                            listLog.Add(log);
                            info.operateLog = JsonTools.ObjectToJson(listLog);
                            VillageWorkingGroupResult.Add(info);
                        }
                        resultCount = db.SaveAll(VillageWorkingGroupResult);
                        #region 日志
                        sb.Append("在栏目{行政村信息}下,拷贝{" + (year - 1) + "}年度的行政村防汛防台工作组信息{" + resultCount + "}条到{" + year + "}年度");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
                        #endregion
                        break;

                    case 2://行政村网格责任人
                        var VillageGridPersonLiableResult = new List<VillageGridPersonLiable>();
                        var VillageGridPersonLiableList = db.Select<VillageGridPersonLiable>(x => x.Year == (year - 1) && x.VillageADCD.StartsWith(adcd.Substring(0, 9)) && x.VillageADCD != adcd);

                        foreach (var info in VillageGridPersonLiableList)
                        {
                            info.Year = year;
                            log.operateMsg = "拷贝" + (year - 1) + "年度" + GetADCDInfoByADCD(info.VillageADCD).adnm + info.PersonLiable + "的行政村网格责任人到" + year + "年度";
                            List<operateLog> listLog = new List<operateLog>();
                            listLog.Add(log);
                            info.operateLog = JsonTools.ObjectToJson(listLog);
                            VillageGridPersonLiableResult.Add(info);
                        }
                        resultCount = db.SaveAll(VillageGridPersonLiableResult);
                        #region 日志
                        sb.Append("在栏目{行政村信息}下,拷贝{" + (year - 1) + "}年度的行政村网格责任人信息{" + resultCount + "}条到{" + year + "}年度");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
                        #endregion
                        break;

                    case 3://行政村危险区转移人员清单
                        var VillageTransferPersonResult = new List<VillageTransferPerson>();
                        var VillageTransferPersonList = db.Select<VillageTransferPerson>(x => x.Year == (year - 1) && x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd);

                        foreach (var info in VillageTransferPersonList)
                        {
                            info.Year = year;
                            log.operateMsg = "拷贝" + (year - 1) + "年度" + GetADCDInfoByADCD(info.adcd).adnm + info.DangerZoneName + "的行政村危险区(点)转移人员清单到" + year + "年度";
                            List<operateLog> listLog = new List<operateLog>();
                            listLog.Add(log);
                            info.operateLog = JsonTools.ObjectToJson(listLog);
                            VillageTransferPersonResult.Add(info);
                        }
                        resultCount = db.SaveAll(VillageTransferPersonResult);
                        #region 日志
                        sb.Append("在栏目{行政村信息}下,拷贝{" + (year - 1) + "}年度的行政村危险区(点)转移人员清单信息{" + resultCount + "}条到{" + year + "}年度");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
                        #endregion
                        break;

                    case 4://行政村防汛防台形势图
                        var VillagePicResult = new List<VillagePic>();
                        var VillagePicList = db.Select<VillagePic>(x => x.Year == (year - 1) && x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd);

                        foreach (var info in VillagePicList)
                        {
                            info.Year = year;
                            log.operateMsg = "拷贝" + (year - 1) + "年度" + GetADCDInfoByADCD(info.adcd).adnm + "的防汛防台形势图到" + year + "年度";
                            List<operateLog> listLog = new List<operateLog>();
                            listLog.Add(log);
                            info.operateLog = JsonTools.ObjectToJson(listLog);
                            VillagePicResult.Add(info);
                        }
                        resultCount = db.SaveAll(VillagePicResult);
                        #region 日志
                        sb.Append("在栏目{行政村信息}下,拷贝{" + (year - 1) + "}年度的防汛防台形势图信息{" + resultCount + "}条到{" + year + "}年度");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
                        #endregion
                        break;

                    case 5://镇街防汛防台责任人
                        var TownPersonLiableResult = new List<TownPersonLiable>();
                        var TownPersonLiableList = db.Select<TownPersonLiable>(x => x.Year == (year - 1) && x.adcd == adcd);

                        foreach (var info in TownPersonLiableList)
                        {
                            info.Year = year;
                            log.operateMsg = "拷贝" + (year - 1) + "年度" + GetADCDInfoByADCD(info.adcd).adnm + info.Name + "的镇级防汛防台责任人信息到" + year + "年度";
                            List<operateLog> listLog = new List<operateLog>();
                            listLog.Add(log);
                            info.operateLog = JsonTools.ObjectToJson(listLog);
                            TownPersonLiableResult.Add(info);
                        }
                        resultCount = db.SaveAll(TownPersonLiableResult);
                        #region 日志
                        sb.Append("在栏目{行政村信息}下,拷贝{" + (year - 1) + "}年度的镇级防汛防台责任人信息{" + resultCount + "}条到{" + year + "}年度");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
                        #endregion
                        break;

                    default:
                        throw new Exception("参数不正确");
                }
                return resultCount > 0;
            }
        }

        /// <summary>
        /// 保存行政区划坐标点信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool SavePoint(SavePoint request)
        {
            using (var db = DbFactory.Open())
            {
                var info = GetADCDInfoByADCD(request.adcd);
                if (info == null)
                    throw new Exception("行政村不存在");
                operateLog log = new operateLog();
                log.userName = RealName;
                log.operateTime = DateTime.Now;
                log.operateMsg = "修改行政村坐标点信息";
                List<operateLog> listLog = new List<operateLog>();
                listLog.Add(log);
                //var oldLog=JsonToolps.JsonToObject(info.operateLog, new List<operateLog>()) as List<operateLog>;
                //oldLog.Add(log);
                #region 日志
                var _r = db.Single<ADCDInfo>(w => w.adcd == request.adcd);
                StringBuilder sb = new StringBuilder();
                sb.Append("在栏目{行政村信息}下,修改了村{" + _r.adnm + "}坐标{" + _r.lng + "," + _r.lat + "}为{" + request.lng + "," + request.lat + "}");
                _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
                #endregion
                return db.UpdateOnly(new ADCDInfo() { lng = request.lng, lat = request.lat, operateLog = JsonTools.ObjectToJson(listLog) },
                    onlyFields: x => new { x.lng, x.lat, x.operateLog },
                    where: x => x.adcd == request.adcd) == 1;
            }
        }

        public BsTableDataSource<ADCDQRCodeViewModel> QRCodeList(QRCodeList request)
        {
            if (string.IsNullOrEmpty(adcd)) throw new Exception("adcd无效");
            var _adcd = string.IsNullOrEmpty(request.adcd) ? adcd : request.adcd;
            var _year = request.year == null ? DateTime.Now.Year : request.year;
            using (var db = DbFactory.Open())
            {
                var builder = db.From<ADCDInfo>();
                builder.LeftJoin<ADCDInfo, ADCDQRCode>((x, y) => x.adcd == y.adcd);
                builder.Where(w => w.adcd.Contains(_adcd.Substring(0, 9)) && w.adcd != _adcd);
                if (!string.IsNullOrEmpty(request.adnm)) builder.Where(w => w.adnm.Contains(request.adnm));
                builder.Select(" ADCDQRCode.id,ADCDQRCode.qrpath,ADCDQRCode.qrname,ADCDInfo.adcd,ADCDInfo.adnm");

                var count = db.Count(builder);

                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(o => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                    builder.OrderByDescending(o => request.Sort);
                else
                    builder.OrderBy(o => o.adcd);

                //var rows = request.PageSize == 0 ? 10 : request.PageSize;
                //var skip = request.PageIndex == 0 ? 0 : request.PageIndex * rows;

                //builder.Limit(skip, rows);
                var RList = db.Select<ADCDQRCodeViewModel>(builder);
                if (!string.IsNullOrEmpty(request.adcds))
                {
                    List<ADCDQRCodeViewModel> lstqrcode = new List<ADCDQRCodeViewModel>();
                    var A = request.adcds.Split(',');
                    for (var i = 0; i < A.Length; i++)
                    {
                        var f = RList.Single(w => w.adcd == A[i]);
                        if (f != null)
                        {
                            ADCDQRCode qrcode = new ADCDQRCode();
                            qrcode.adcd = f.adcd;
                            var filename = f.adnm + f.adcd.Substring(9, 3);
                            if (string.IsNullOrEmpty(f.qrpath))
                            {
                                //新增
                                string str = ConfigurationManager.AppSettings["QRCodeUrl"].ToString() + "?a=" + f.adcd + "";
                                qrcode.qrpath = QRCodeHelper.GetQRCode(str, RealName, filename);
                                qrcode.qrname = filename;
                                db.Insert(qrcode);
                            }
                            else
                            {
                                //删除
                                var filepath = System.Web.HttpContext.Current.Server.MapPath("~/" + f.qrpath);
                                File.Delete(filepath);
                                string str = ConfigurationManager.AppSettings["QRCodeUrl"].ToString() + "?a=" + f.adcd + "";
                                qrcode.qrpath = QRCodeHelper.GetQRCode(str, RealName, filename);
                                qrcode.qrname = filename;
                                db.Update<ADCDQRCode>(qrcode, w => w.adcd == f.adcd);
                            }
                            ADCDQRCodeViewModel qrv = new ADCDQRCodeViewModel()
                            {
                                adcd = f.adcd,
                                adnm = f.adnm,
                                qrname = f.qrname,
                                qrpath = qrcode.qrpath
                            };
                            lstqrcode.Add(qrv);
                        }
                    }
                    return new BsTableDataSource<ADCDQRCodeViewModel>() { rows = lstqrcode, total = lstqrcode.Count() };
                }
                return new BsTableDataSource<ADCDQRCodeViewModel>() { rows = RList, total = count };
            }
        }

        /// <summary>
        /// 获取session中的adcd
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<ADCDInfo> GetADCDInfoBySession(GetADCDInfoBySession request)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<ADCDInfo>();
                builder.Where(x => x.adcd == request.adcd);
                builder.OrderBy(x => x.adcd);
                var list = db.Select(builder);
                return list;
            }
        }

        public BsTableDataSource<ADCDInfo> GetADCDInfoForCounty(GetAdcdInfoForCounty request)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<ADCDInfo>();
                switch (request.levle)
                {
                    case 0: //全部
                        break;

                    case 1: //省
                        builder.Where(x => x.adcd == "330000000000000");
                        break;

                    case 2: //市
                        switch (RowID)
                        {
                            case 2:
                                builder.Where("adcd ='" + adcd +
                                              "'"); //x => x.adcd != "330000000000000" && x.adcd.EndsWith("00"));
                                break;

                            case 5:
                                builder.Where(
                                    "adcd like '33__00000000000' and adcd !='330000000000000'"); //x => x.adcd != "330000000000000" && x.adcd.EndsWith("00"));
                                break;
                        }
                        //builder.And("len(adcd)=6");
                        break;

                    case 3: //县（区）
                        //                        builder.Where("adcd like '33____000000000'  and adcd not like '33__00000000000'");
                        builder.Where(x => x.adcd == request.adcd);
                        //builder.Where(x => x.adcd != "330000"&&!x.adcd.EndsWith("00"));
                        //builder.And("len(adcd)=6");
                        //                        if (!string.IsNullOrEmpty(request.adcd) && request.adcd.Length == 15 && request.adcd.IndexOf("000000000") > 0)
                        //                            builder.And("adcd like '" + request.adcd.Substring(0, 4) + "__000000000' and adcd !='" + request.adcd + "'");
                        break;

                    case 4: //镇街
                        builder.Where("adcd like '%000000' and adcd not like '%000000000'");
                        //builder.Where(x => x.adcd.EndsWith("000000"));
                        if (!string.IsNullOrEmpty(request.adcd))
                            builder.And(x => x.adcd.StartsWith(request.adcd.Substring(0, 6)) && x.adcd != request.adcd);
                        break;

                    case 5: //行政村
                        builder.Where(x => !x.adcd.EndsWith("000000") && x.adcd.EndsWith("000"));
                        builder.And("len(adcd)=15");
                        if (!string.IsNullOrEmpty(request.adcd))
                            builder.And(x => x.adcd == request.adcd);
                        else
                            builder.And(x => x.adcd.Contains(adcd.Substring(0, 9)));
                        break;

                    case 51: //行政村1
                        builder.Where(x => x.adcd.Contains(request.adcd.Substring(0, 9)) &&
                                           x.adcd.Substring(10, 3) != "000");
                        break;
                        //case 6://自然村
                        //    builder.Where(x => x.adcd.Length == 15 && x.adcd.Substring(12, 3) != "000");
                        //    break;
                }
                if (!string.IsNullOrEmpty(request.adnm))
                    builder.And(x => x.adnm.Contains(request.adnm));
                var count = db.Count(builder);

                var rows = request.PageSize == 0 ? 10 : request.PageSize;
                var skip = request.PageIndex == 0 ? 0 : request.PageIndex * rows;

                builder.Limit(skip, rows);
                builder.OrderBy(x => x.adcd);
                var list = db.Select(builder);
                return new BsTableDataSource<ADCDInfo>() { total = count, rows = list };
            }
        }

        /// <summary>
        /// 根据adcd获取下一层的所有信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<ResponseAdcdByUserAdcd> GetAdcdInfoByAdcdForTree(GetAdcdByUseradcd request)
        {
            if (string.IsNullOrEmpty(request.UserAdcd))
            {
                return null;
            }
            else
            {
                using (var db = DbFactory.Open())
                {
                    return db.SqlList<ResponseAdcdByUserAdcd>("exec GetAdcdInfoByAdcdForTree @adcd,@type", new { adcd = request.UserAdcd, type = request.actiontype });
                }
            }
        }

        public List<ResponseAdcdByUserAdcd> AppGetAdcdInfoByAdcd(AppGetAdcdInfoByAdcd request)
        {
            using (var db = DbFactory.Open())
            {
                return db.SqlList<ResponseAdcdByUserAdcd>("exec AppGetAdcdInfoByAdcd @adcd,@type", new { adcd = request.UserAdcd, type = request.ActionType });
            }
        }

        //通过adcd获取下一级全部的市
        public List<ResponseAdcdByUserAdcd> AppGetAllCity(AppGetAllCity reqest)
        {
            using (var db = DbFactory.Open())
            {
                return db.SqlList<ResponseAdcdByUserAdcd>("exec AppGetAllNextInfoByAdcd @adcd", new { adcd = reqest.ProvAdcd });
            }
        }

        //通过adcd获取下一级全部的县
        public List<ResponseAdcdByUserAdcd> AppGetAllCounty(AppGetAllCounty reqest)
        {
            using (var db = DbFactory.Open())
            {
                return db.SqlList<ResponseAdcdByUserAdcd>("exec AppGetAllNextInfoByAdcd @adcd", new { adcd = reqest.CityAdcd });
            }
        }

        //通过adcd获取下一级全部的镇
        public List<ResponseAdcdByUserAdcd> AppGetAllTown(AppGetAllTown reqest)
        {
            using (var db = DbFactory.Open())
            {
                return db.SqlList<ResponseAdcdByUserAdcd>("exec AppGetAllNextInfoByAdcd @adcd", new { adcd = reqest.CountyAdcd });
            }
        }

        //通过adcd获取下一级全部的村
        public List<ResponseAdcdByUserAdcd> AppGetAllVillage(AppGetAllVillage reqest)
        {
            using (var db = DbFactory.Open())
            {
                return db.SqlList<ResponseAdcdByUserAdcd>("exec AppGetAllNextInfoByAdcd @adcd", new { adcd = reqest.TownAdcd });
            }
        }

        //通过adcd获取相对应的信息
        public ResponseAdcdInfo GetAdcdInfoByAdcd(GetAdcdInfoByAdcd request)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<ADCDInfo>();
                builder.Where(x => x.adcd == request.adcd);
                return db.Single<ResponseAdcdInfo>(builder);
            }
        }

        public List<ResponseAdcdInfo> GetNextLevleAdcdInfoByAdcd(GetNextLevleAdcdInfoByAdcd requst)
        {
            using (var db = DbFactory.Open())
            {
                var list = db.SqlList<ResponseAdcdInfo>("exec GetNextLevleAdcdInfoByAdcd @adcd",new { adcd = requst.adcd });
                return list;
            }
        }
    }
}