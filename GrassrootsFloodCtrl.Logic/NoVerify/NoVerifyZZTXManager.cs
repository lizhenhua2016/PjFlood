using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Logic.ZZTX;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using ServiceStack.OrmLite;
using System.Data;
using Dy.Common;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.Town;
using GrassrootsFloodCtrl.Model.Village;
using ServiceStack;
using GrassrootsFloodCtrl.Logic.Common;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;
using GrassrootsFloodCtrl.Model.Enums;
using System.Configuration;
using System.IO;
using GrassrootsFloodCtrl.ServiceModel.NoAuditRoute;
using ServiceStackForLeafletjsResponse;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;
using GrassrootsFloodCtrl.ServiceModel.Village;

namespace GrassrootsFloodCtrl.Logic.NoVerify
{
    public class NoVerifyZZTXManager : ManagerBase, INoVerifyZZTXManager
    {
        public List<ResponseAdcdByUserAdcd> AppGetAdcdInfoByAdcd(NoVerifyAppGetAdcdInfoByAdcd request)
        {
            using (var db = DbFactory.Open())
            {
                return db.SqlList<ResponseAdcdByUserAdcd>("exec AppGetAdcdInfoByAdcd @adcd,@type", new { adcd = request.UserAdcd, type = request.ActionType });
            }
        }

        /// <summary>
        /// 获取行政区划信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<ADCDInfo> GetADCDInfo(NoVGetADCDInfo request)
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

        public BsTableDataSource<ADCDInfo> GetADCDInfoForCounty(NoVGetAdcdInfoForCounty request)
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
        //获取矢量化
        public BsTableDataSource<ResponseVectors> GetAllVectors(NoAuthticationRouteVector request)
        {
            using (var db= DbFactory.Open())
            {
                var list = db.SqlList<Vectors>("exec GetAllVectors @disType",new{ disType= request.Distype});
                
                List<ResponseVectors> abc=new List<ResponseVectors>();
                abc.Add(new ResponseVectors(){Type = request.Distype, Vectorses = list});
                
                return new BsTableDataSource<ResponseVectors>() { rows = abc, total = list.Count };
            }
        }

        public List<ResponseAdcdInfo> GetNextLevleAdcdInfoByAdcd(NoVGetNextLevleAdcdInfoByAdcd requst)
        {
            using (var db = DbFactory.Open())
            {
                var list = db.SqlList<ResponseAdcdInfo>("exec GetNextLevleAdcdInfoByAdcd @adcd", new { adcd = requst.adcd });
                return list;
            }
        }

        public BsTableDataSource<ADCDQRCodeViewModel> QRCodeList(NoVerifyQRCodeList request)
        {
            //if (string.IsNullOrEmpty(adcd)) throw new Exception("adcd无效");
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


    }
}
