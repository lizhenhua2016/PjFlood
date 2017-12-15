using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.Model.Common;
using Dy.Common;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using GrassrootsFloodCtrl.ServiceModel.Post;
using System.Data;
using GrassrootsFloodCtrl.ServiceModel.Grid;
using System.Collections;
using GrassrootsFloodCtrl.Logic.Common;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;
using GrassrootsFloodCtrl.ServiceModel.Position;
using GrassrootsFloodCtrl.Model.Supervise;
using Aspose.Cells;

namespace GrassrootsFloodCtrl.Logic.Village.VillageGrid
{
    public class VillageGridManage : ManagerBase, IVillageGridManage
    {
        public ILogHelper _ILogHelper { get; set; }
        public BsTableDataSource<VillageViewModel> GetVillageGridList(GetVillageGridList request)
        {try
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
            }catch(Exception ex)
            {
                throw new Exception("系统异常,请刷新："+ex.Message);
            }
        }
        public BaseResult SaveVillageGrid(SaveVillageGrid request)
        {
            BaseResult br = new BaseResult();
            try
            {
                var _year = DateTime.Now.Year;
                using (var db = DbFactory.Open())
                {
                    if (string.IsNullOrEmpty(request.villageadcd)) throw new Exception("参数ADCD不能为空！");
                    if (string.IsNullOrEmpty(request.personLiable)) throw new Exception("参数责任人不能为空！");
                    if (string.IsNullOrEmpty(request.handphone) || (!ValidatorHelper.IsMobile(request.handphone) && !ValidatorHelper.IsTelephone(request.handphone))) throw new Exception("参数手机号无效！");
                    if (string.IsNullOrEmpty(request.gridname)) throw new Exception("参数网格类型不能为空！");
                    if (string.IsNullOrEmpty(request.villagegridname)) throw new Exception("参数网格名不能为空！");
                    
                    VillageGridPersonLiable _vgr = new VillageGridPersonLiable()
                    {
                        VillageADCD = request.villageadcd.Trim(),
                        PersonLiable = request.personLiable.Trim(),
                        HandPhone = request.handphone.Trim(),
                        GridName = request.gridname.Trim(),
                        VillageGridName=request.villagegridname.Trim(),
                        Position = string.IsNullOrEmpty(request.position) ? "" : request.position.Trim(),
                        AddTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                        Year = _year,
                        Remarks = string.IsNullOrEmpty(request.remark) ? "" : request.remark.Trim()
                    };
					// _ILogHelper.GetModel(typeof(VillageGridPersonLiable),_vgr);                    //新数据
                    List<VillageGridPersonLiable> _newdata = new List<VillageGridPersonLiable>();
                    _newdata.Add(_vgr);
                    //
                    var builder = db.From<VillageGridPersonLiable>();
                    if (null == request.id)
                    {
                        var f = db.Single<VillageGridPersonLiable>(w => w.VillageADCD == request.villageadcd && w.PersonLiable == request.personLiable && w.GridName == request.gridname && w.VillageGridName == request.villagegridname && w.Position == request.position && w.HandPhone == request.handphone);
                        if (null != f) throw new Exception("重复数据：同村同类型同网格名同责任人同手机！");
                        #region 旧数据，新数据
                        //写入更新记录
                        if (AuditNums != null)
                        {
                            _vgr.AuditNums = AuditNums.Value + 1;
                        }
                        #endregion
                        operateLog log = new operateLog();
                        log.userName = RealName;
                        log.operateTime = DateTime.Now;
                        log.operateMsg = request.villageadcd + "村{" + _year + "}新增了网格责任人{" + request.personLiable + "}的信息";
                        List<operateLog> listLog = new List<operateLog>();
                        listLog.Add(log);
                        _vgr.operateLog = JsonTools.ObjectToJson(listLog);
                        br.IsSuccess = db.Insert(_vgr) == 1 ? true : false;
                        #region 日志
                        StringBuilder sb = new StringBuilder();
						var adcdInfo = db.Single<ADCDInfo>(w => w.adcd == _vgr.VillageADCD);
                        sb.Append("在栏目{组织责任/行政村网格责任人}下,新增数据{");
                        sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                        sb.Append("责任人：" + _vgr.PersonLiable + ";");
                        sb.Append("手机：" + _vgr.HandPhone+";");
                        sb.Append("网格名：" + _vgr.GridName + ";");
                        sb.Append("村网格名：" + _vgr.VillageGridName + ";");
                        sb.Append("职位：" + _vgr.Position + ";");
                        sb.Append("添加时间：" + _vgr.AddTime + ";");
                        sb.Append("年份：" + _vgr.Year + ";");
                        sb.Append("备注：" + _vgr.Remarks + ";");
                        sb.Append("}");
                        _ILogHelper.WriteLog(sb.ToString(),OperationTypeEnums.新增);
                        #endregion
                    }
                    if (null != request.id && request.id.Value > 0)
                    {
                        operateLog log = new operateLog();
                        log.userName = RealName;
                        log.operateTime = DateTime.Now;
                        log.operateMsg = request.villageadcd + "村{" + _year + "}修改了网格责任人{" + request.personLiable + "}的信息";
                        List<operateLog> listLog = new List<operateLog>();
                        listLog.Add(log);
                        _vgr.operateLog = JsonTools.ObjectToJson(listLog);
                        _vgr.EditTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        #region 旧数据，新数据
                        //取出数据
                        //写入更新记录
                        var r = db.Single<VillageGridPersonLiable>(w => w.ID == request.id);
                        VillageGridPersonLiable _olddata = new VillageGridPersonLiable()
                        {
                            VillageADCD = r.VillageADCD,
                            PersonLiable = r.PersonLiable,
                            HandPhone = r.HandPhone,
                            Position = r.Position,
                            AddTime = r.AddTime,
                            Year = r.Year,
                            Remarks = r.Remarks,
                            EditTime = r.EditTime,
                            GridName = r.GridName,
                            VillageGridName = r.VillageGridName
                        };
                        if (AuditNums != null)
                        {
                            List<VillageGridPersonLiable> _listOldData = new List<VillageGridPersonLiable>();
                            _listOldData.Add(_olddata);
                            _vgr.AuditNums = AuditNums.Value + 1;
                            //旧数据写入实体
                            _vgr.OldData = JsonTools.ObjectToJson(_listOldData);
                            //新数据写入实体
                            _vgr.NewData = JsonTools.ObjectToJson(_newdata);
                        }
                        #endregion
                        #region 日志
                        StringBuilder sb = new StringBuilder();
						var adcdInfo = db.Single<ADCDInfo>(w => w.adcd == _olddata.VillageADCD);
                        sb.Append("在栏目{组织责任/行政村网格责任人}下,更新数据{");
                        sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                        sb.Append("责任人：" + _olddata.PersonLiable + ";");
                        sb.Append("手机：" + _olddata.HandPhone + ";");
                        sb.Append("网格名：" + _olddata.GridName + ";");
                        sb.Append("村网格名：" + _olddata.VillageGridName + ";");
                        sb.Append("职位：" + _olddata.Position + ";");
                        sb.Append("添加时间：" + _olddata.AddTime + ";");
                        sb.Append("年份：" + _olddata.Year + ";");
                        sb.Append("备注：" + _olddata.Remarks + ";");
                        sb.Append("} 为 {");
                        sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                        sb.Append("责任人：" + _vgr.PersonLiable + ";");
                        sb.Append("手机：" + _vgr.HandPhone + ";");
                        sb.Append("网格名：" + _vgr.GridName + ";");
                        sb.Append("村网格名：" + _vgr.VillageGridName + ";");
                        sb.Append("职位：" + _vgr.Position + ";");
                        sb.Append("添加时间：" + _vgr.AddTime + ";");
                        sb.Append("年份：" + _vgr.Year + ";");
                        sb.Append("备注：" + _vgr.Remarks + ";");
                        sb.Append("}");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
                        #endregion
                        br.IsSuccess = db.Update<VillageGridPersonLiable>(_vgr, x => x.ID == request.id) == 1 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return br;
        }

        public BsTableDataSource<VillageGridViewModel> GetGridPersonLiable(GetGridPersonLiable request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(adcd))
                { throw new Exception("请重新登录"); }
                //取镇下面的所有行政村
                var builder = db.From<VillageGridPersonLiable>();
                if (null == request.id && request.id <= 0)
                { throw new Exception("参数异常"); }
                //var _year = null != request.year && !string.IsNullOrEmpty(request.year.ToString()) ? request.year : System.DateTime.Now.Year;
                // builder.Where<VillageWorkingGroup>(y => y.VillageADCD != null && y.Year == _year && y.ID==request.id);
                builder.Where<VillageGridPersonLiable>(y => y.ID == request.id);
                var count = db.Select(builder).Count;

                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(o => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                    builder.OrderByDescending(o => request.Sort);
                else
                    builder.OrderBy(o => o.ID);

                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex - 1) * PageSize;
                builder.Limit(PageIndex, PageSize);
                var RList = db.Select<VillageGridViewModel>(builder);

                return new BsTableDataSource<VillageGridViewModel>() { rows = RList, total = count };
            }
        }
        public BsTableDataSource<VillageGridViewModel> GetGridPersonLiableList(GetGridPersonLiableList request)
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
                builder.Select<VillageGridPersonLiable, ADCDInfo>((x, y) => new {
                    id = x.ID,
                    VillageADCD=x.VillageADCD,
                    adnm=y.adnm,
                    PersonLiable=x.PersonLiable,
                    GridName=x.GridName,
                    VillageGridName=x.VillageGridName,
                    Position=x.Position,
                    HandPhone=x.HandPhone,
                    Remarks=x.Remarks,
                    AddTime=x.AddTime,
                    EditTime=x.EditTime,
                    Year=x.Year
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

        public BaseResult DeleteVillageGrid(DeleteVillageGrid request)
        {
            BaseResult br = new BaseResult();
            try
            {
                using (var db = DbFactory.Open())
                {
                    if (string.IsNullOrEmpty(request.id))
                        throw new Exception("参数异常！");
                    ArrayList arr = new ArrayList();
                    var ids = request.id.Split(',');
                    for(int i=0;i<ids.Count(); i++)
                    {
                        var id = int.Parse(ids[i]);
                        arr.Add(id);
                        #region 日志
                        var _vgr = db.Single<VillageGridPersonLiable>(w=>w.ID == id);
						var adcdInfo = db.Single<ADCDInfo>(w => w.adcd == _vgr.VillageADCD);
                        StringBuilder sb = new StringBuilder();
                        sb.Append("在栏目{组织责任/行政村网格责任人}下,删除了数据{");
                        sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                        sb.Append("责任人：" + _vgr.PersonLiable + ";");
                        sb.Append("手机：" + _vgr.HandPhone + ";");
                        sb.Append("网格名：" + _vgr.GridName + ";");
                        sb.Append("村网格名：" + _vgr.VillageGridName + ";");
                        sb.Append("职位：" + _vgr.Position + ";");
                        sb.Append("添加时间：" + _vgr.AddTime + ";");
                        sb.Append("年份：" + _vgr.Year + ";");
                        sb.Append("备注：" + _vgr.Remarks + ";");
                        sb.Append("}");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.删除);
                        #endregion
                    }

                    br.IsSuccess = db.Delete<VillageGridPersonLiable>(w=> Sql.In(w.ID,arr)) > 0;
                }
            }
            catch (Exception ex)
            {
                br.IsSuccess = false;
                br.ErrorMsg = ex.Message;
                throw new Exception(ex.Message);
            }
            return br;
        }

        public BaseResult DelVillageGrid(DelVillageGrid request)
        {
            BaseResult br = new BaseResult();
            try
            {
                using (var db = DbFactory.Open())
                {
                    if (string.IsNullOrEmpty(request.adcd))
                        throw new Exception("参数异常！");
                    var builder = db.From<VillageGridPersonLiable>();
                    var _year = null != request.year && !string.IsNullOrEmpty(request.year.ToString()) ? int.Parse(request.year) : DateTime.Now.Year;
                    #region 日志
                    StringBuilder sb = new StringBuilder();
					var adcdInfo = db.Single<ADCDInfo>(w => w.adcd == request.adcd);
                    sb.Append("在栏目{组织责任/行政村网格责任人}下,删除村{"+ adcdInfo.adnm + "}及其数据");
                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.删除);
                    #endregion
                    br.IsSuccess = db.Delete<VillageGridPersonLiable>(w => w.VillageADCD == request.adcd && w.Year == _year) == 1 ? true : false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return br;
        }
        /// <summary>
        /// 获取某一个村下的网购责任人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<VillageGridViewModel> GetVillageGrid(GetVillageGrid request)
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

        public BsTableDataSource<StatiscPerson> GetVillageGrid1(GetVillageGrid1 request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(adcd))
                { throw new Exception("请重新登录"); }
                var builder = db.From<VillageGridPersonLiable>();
                builder.LeftJoin<VillageGridPersonLiable, ADCDInfo>((x, y) => x.VillageADCD == y.adcd);
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                builder.Where<VillageGridPersonLiable>(x => x.Year == _year && x.VillageADCD == request.adcd);
               
                builder.Select(" VillageGridPersonLiable.*,ADCDInfo.adnm");
                var count = db.Count(builder);

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
                var newlist = RList.Select(w => w.PersonLiable).Distinct().ToList();
                newlist.Remove("");
                newlist.Remove(null);
                List<StatiscPerson> rlist = new List<StatiscPerson>();
                newlist.ForEach(w => {
                    var f = RList.Where(x => x.PersonLiable == w).ToList();
                    var newpost = ""; var phone = "";
                    f.ForEach(y => {
                        newpost += y.Position + ";";
                        phone += y.HandPhone + ';';
                    });
                    var fphones = phone.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    var fnewpost = newpost.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    StatiscPerson tplvm = new StatiscPerson()
                    {
                        personLiable = w,
                        post = string.Join(";",fnewpost),
                        handPhone = string.Join(";", fphones),
                        id = f.FirstOrDefault().ID,
                        adcd = f.FirstOrDefault().VillageADCD
                    };
                    rlist.Add(tplvm);
                });
                return new BsTableDataSource<StatiscPerson>() { rows = rlist, total = rlist.Count() };
            }
        }
        #region 网格责任人模板下载
        public UploadFileViewModel DownGridFileModel(DownGridFileModel request)
        {
            UploadFileViewModel _ufvm = new UploadFileViewModel();
            var _year = null == request.year || request.year == 0 ? DateTime.Now.Year : request.year;
            var name = RealName + _year + "“村级(社区)网格责任人”模板" + DateTime.Now.ToString("MMddhhmmss") + new Random(DateTime.Now.Second).Next(10000);
            var filesavepath = "Files/" + RealName + "/Grid";
            var _fileexits = System.Web.HttpContext.Current.Server.MapPath("~/" + filesavepath);
            if (!Directory.Exists(_fileexits))
            {
                Directory.CreateDirectory(_fileexits);
            }
            var path = System.Web.HttpContext.Current.Server.MapPath("~/" + filesavepath + "/" + name + ".xls");
            /*******************************/
            HSSFWorkbook workBook = new HSSFWorkbook();
            createSheet1(workBook, "村级(社区)网格责任人");
            //获取行政村名录
            var createxcel = new creatExcel();
            List<string> cunList = new List<string>();
            using (var db = DbFactory.Open())
            {
                cunList = db.Select<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd).Select(x => x.adnm).ToList();
            }
            createxcel.setSheet3(workBook, "行政村名录", cunList);
            using (FileStream file = new FileStream(path, FileMode.Create))
            {
                workBook.Write(file);//创建Excel文件。
                file.Close();
            }
            _ufvm.fileSrc = filesavepath + "/" + name + ".xls";
            /*******************************/
            return _ufvm;
        }
        private ISheet createSheet1(HSSFWorkbook workBook, string sheetName)
        {
            ISheet sheet = workBook.CreateSheet(sheetName);

            //合并第一行单元格
            //IRow RowHead = sheet.CreateRow(0);
            //sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));
            //string noitce = "注意： ";
            //noitce += "请不要修改<村级网格责任人>表的格式，如果在系统上新增了网格类型请重新下载数据模板";
            //RowHead.CreateCell(0).SetCellValue(noitce);
            ///*****单元格样式 start******/
            //var cellStyleHead = workBook.CreateCellStyle();
            //cellStyleHead.Alignment = HorizontalAlignment.Center;//居中显示
            //cellStyleHead.VerticalAlignment = VerticalAlignment.Top;//垂直居中
            //cellStyleHead.WrapText = true;
            ////高度
            //RowHead.HeightInPoints = 50; //2 * sheet.DefaultRowHeight / 10;
            ////字体设置,字体要调用CreateFont()
            //IFont fonthead = workBook.CreateFont();
            //fonthead.FontHeightInPoints = 10;
            //fonthead.Color = HSSFColor.OliveGreen.Red.Index;
            //cellStyleHead.SetFont(fonthead);
            ////这里调试出来的,样式一定要给到单元格才有效
            //RowHead.Cells[0].CellStyle = cellStyleHead;
            #region 标题
            var cellStyleHeadTitle = workBook.CreateCellStyle();
            cellStyleHeadTitle.Alignment = HorizontalAlignment.Center;//居中显示
            cellStyleHeadTitle.VerticalAlignment = VerticalAlignment.Top;//垂直居中
            ExcelHeadAttribute eha = new ExcelHeadAttribute()
            {
                rowIndex = 0,
                firstRow = 0,
                lastRow = 0,
                firstCol = 0,
                lastCol = 6,
                fontColor = 8,
                fontSize = 16,
                HeightInPoints = 50,
                name = "村级(社区)网格责任人"
            };
            CreateHead(workBook, sheet, cellStyleHeadTitle, eha);
            #endregion
            #region 提醒
            var cellStyleHeadNotice = workBook.CreateCellStyle();
            cellStyleHeadNotice.Alignment = HorizontalAlignment.Left;//居中显示
            cellStyleHeadNotice.VerticalAlignment = VerticalAlignment.Top;//垂直居中
            string noitce = "注意： ";
            noitce += "1、行政村请在“行政村名录工作表”中复制，否则无法导入。";
            noitce += "2、一人兼多岗，也请明确要逐条列出。   ";
            noitce += "3、加*列为必填项。   ";
            noitce += "4、网格类型、责任人职务请下拉选择。   ";
            noitce += "5、小灵通(格式:区号\" - \"号码)。";
            ExcelHeadAttribute ehanotice = new ExcelHeadAttribute()
            {
                rowIndex = 1,
                firstRow = 1,
                lastRow = 1,
                firstCol = 0,
                lastCol = 6,
                fontColor = 10,
                fontSize = 14,
                HeightInPoints = 40,
                name = noitce
            };
            CreateHead(workBook, sheet, cellStyleHeadNotice, ehanotice);
            #endregion
            #region 添加表头
            IRow RowBody = sheet.CreateRow(2);
            List<string> listtitle = new List<string>();
            listtitle.Add("*行政村"); listtitle.Add("*网格名称(最多25个汉字)"); listtitle.Add("*网格类型(仅限下拉内容)");
            listtitle.Add("*责任人姓名"); listtitle.Add("责任人职务"); listtitle.Add("*责任人手机"); listtitle.Add("备注");

            for (int iColumnIndex = 0; iColumnIndex < listtitle.Count(); iColumnIndex++)
            {
                RowBody.CreateCell(iColumnIndex).SetCellValue(listtitle[iColumnIndex].ToString());
                RowBody.Cells[iColumnIndex].Row.HeightInPoints = 20;
                //单元格样式
                ICellStyle cellStyle = workBook.CreateCellStyle();
                cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cellStyle.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                cellStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                //设置单元格上下左右边框线  
                cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                //文字水平和垂直对齐方式  
                cellStyle.Alignment = HorizontalAlignment.Center;
                cellStyle.VerticalAlignment = VerticalAlignment.Top;
                //是否换行
                cellStyle.WrapText = true;

                RowBody.Cells[iColumnIndex].CellStyle = cellStyle;
                //字体大小
                IFont cellfont = workBook.CreateFont();
                cellfont.FontHeightInPoints = 14;
                cellStyle.SetFont(cellfont);
                sheet.SetColumnWidth(iColumnIndex, 30 * 256);
            }
            #endregion
            //设置下拉框--网格类型
            setSheet2(workBook, sheet);
            //设置下拉框--责任人
            setSheet3(workBook, sheet);
            return sheet;
        }
        private void setSheet2(HSSFWorkbook workBook, ISheet sheet)
        {
            //创建表
            ISheet sheet2 = workBook.CreateSheet("网格类型");
            //隐藏
            workBook.SetSheetHidden(1, true);
            //取数据
            using (var db = DbFactory.Open())
            {
                var builder = db.From<Model.Grid.Grid>();
                var rlist = db.Select<GridViewModel>(builder);
                for (int iRowIndex = 0; iRowIndex < rlist.Count; iRowIndex++)
                {
                    sheet2.CreateRow(iRowIndex).CreateCell(0).SetCellValue(rlist[iRowIndex].GridName);
                }
            }
            //设计表名称
            IName range = workBook.CreateName();
            range.RefersToFormula = "网格类型!$A:$A";
            range.NameName = "PostDataName";
            //定义下拉框范围
            CellRangeAddressList regions = new CellRangeAddressList(3, 65535, 2, 2);
            //设置数据引用
            DVConstraint constraint = DVConstraint.CreateFormulaListConstraint("PostDataName");
            HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
            sheet.AddValidationData(dataValidate);
        }
        private void setSheet3(HSSFWorkbook workBook, ISheet sheet)
        {
            //创建表
            ISheet sheet2 = workBook.CreateSheet("网格责任人");
            //隐藏
            workBook.SetSheetHidden(2, true);
            //取数据
            using (var db = DbFactory.Open())
            {
                var builder = db.From<Model.Position.Position>();
                //builder.Where(w=>w.PositionType == )
                var rlist = db.Select<PositionViewModel>(builder);
                for (int iRowIndex = 0; iRowIndex < rlist.Count; iRowIndex++)
                {
                    sheet2.CreateRow(iRowIndex).CreateCell(0).SetCellValue(rlist[iRowIndex].PositionName);
                }
            }
            //设计表名称
            IName range = workBook.CreateName();
            range.RefersToFormula = "网格责任人!$A:$A";
            range.NameName = "PositionDataName";
            //定义下拉框范围
            CellRangeAddressList regions = new CellRangeAddressList(3, 65535, 4, 4);
            //设置数据引用
            DVConstraint constraint = DVConstraint.CreateFormulaListConstraint("PositionDataName");
            HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
            sheet.AddValidationData(dataValidate);
        }
        #endregion

        #region 网格责任人批量导入
        public BaseResult UploadGridFiles(UploadGridFiles request)
        {
            BaseResult _br = new BaseResult();
            if (string.IsNullOrEmpty(request.fpath)) throw new Exception("参数异常！");
            var _year = null == request.year ? DateTime.Now.Year : request.year;
            var newpath = System.Web.HttpContext.Current.Server.MapPath(request.fpath);
            try
            {
                //导入数据
                //var dt = Logic.Common.ExcelHelper.GetDataTable(newpath);
                ////移出表头
                //dt.Rows.RemoveAt(0);
                //dt.Rows.RemoveAt(0);
                Workbook workbook = new Workbook();
                workbook.Open(newpath);
                Cells cells = workbook.Worksheets[0].Cells;
                var dt = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, true);
                //移出表头
                dt.Rows.RemoveAt(0);
                dt.Rows.RemoveAt(0);
                #region 基础数据集
                //取出岗位
                List<GridViewModel> gridList = null;
                using (var dbgrid = DbFactory.Open())
                {
                    var buildergrid = dbgrid.From<Model.Grid.Grid>();
                    gridList = dbgrid.Select<GridViewModel>(buildergrid);
                }
                //取出adcd
                List<VillageViewModel> adcdList = null;
                using (var dbadcd = DbFactory.Open())
                {
                    var builderadcd = dbadcd.From<ADCDInfo>();
                    if (adcd.Length == 15 && adcd.IndexOf("000000") > 0)//登陆的是乡镇用户
                        builderadcd.Where<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd.ToString());
                    else if (adcd.Length == 6 && adcd.IndexOf("00") < 0)//登陆的是县级用户
                        builderadcd.Where<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 6)));
                    else if (adcd == GrassrootsFloodCtrlEnums.AreaCode.省级编码.ToString())//管理员
                    { }
                    else
                    {
                        throw new Exception("登陆用户的所属行政区划编码不正确");
                    }
                    adcdList = dbadcd.Select<VillageViewModel>(builderadcd);
                }
                //取出责任人
                List<VillageGridViewModel> vlist = null;
                using (var db = DbFactory.Open())
                {
                    var builder = db.From<ADCDInfo>();
                    builder.LeftJoin<ADCDInfo, VillageGridPersonLiable>((x, y) => x.adcd == y.VillageADCD);
                    if (adcd.Length == 15 && adcd.IndexOf("000000") > 0)//登陆的是乡镇用户
                    {
                        builder.Where<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd.ToString());
                    }
                    else
                    {
                        throw new Exception("登陆用户的所属行政区划编码不正确");
                    }
                    builder.Where<VillageGridPersonLiable>(y => y.VillageADCD != null && y.Year == _year);
                    builder.Select(" VillageGridPersonLiable.*,ADCDInfo.adnm");
                    vlist = db.Select<VillageGridViewModel>(builder);
                }
                #endregion
                #region 校验
                int i = 4;
                List<ErrList> _erlist = new List<ErrList>();
                foreach (DataRow dr in dt.Rows)
                {
                    ErrList _el = new ErrList();
                    _el.rowid = i;
                    bool _c = false;
                    //村名匹配
                    var villageName = dr[0].ToString().Trim();
                    if (string.IsNullOrEmpty(villageName))
                    {
                        _c = false;
                        _el.msg = "第  " + i + "  行村名为空！";
                    }
                    else
                    {
                        var A = adcdList.Where<VillageViewModel>(w => w.adnm == villageName).FirstOrDefault();
                        if (null == A || string.IsNullOrEmpty(A.adcd))
                        {
                            _c = false;
                            _el.msg = "第 " + i + " 行村名和系统里标注的村名不匹配！新增村名导入前请先标注<br/>";
                        }
                        else
                        {
                            _c = true;
                        }
                    }
                    //网格类型匹配
                    var gridName = dr[2].ToString().Trim();
                    if (string.IsNullOrEmpty(gridName))
                    {
                        _c = false;
                        _el.msg = "第  " + i + " 行网格类型名为空！";
                    }
                    else
                    {
                        var B = gridList.Where<GridViewModel>(w => w.GridName == gridName).FirstOrDefault();
                        if (null == B || string.IsNullOrEmpty(B.GridName))
                        {
                            _c = false;
                            _el.msg = "第 " + i + " 行网格类型名称和系统里的网格类型名不匹配！新增网格请先到“网格类型管理”栏目设置后,重新下载模板";
                        }
                        else
                        {
                            _c = true;
                        }
                    }
                    //责任人
                    var personName = dr[3].ToString().Trim();
                    if (string.IsNullOrEmpty(personName))
                    {
                        _c = false;
                        _el.msg = "第  " + i + " 行责任人为空！";
                    }
                    else
                    {
                        _c = true;
                    }
                    var handphone = dr[5].ToString().Trim();
                    if (string.IsNullOrEmpty(handphone))
                    {
                        _el.msg = "第  " + i + " 行责任人联系方式为空！";
                    }
                    else if (!ValidatorHelper.IsMobile(handphone) && !ValidatorHelper.IsTelephone(handphone))
                    {
                        _el.msg = "第  " + i + " 行责任人联系方式格式错误！请填写手机或座机(如：0571-8687133(2))";
                    }
                    else { }
                    //同村,同岗,同责任人验证
                    if (_c)
                    {
                        var checkPerson = vlist.Where<VillageGridViewModel>(w => w.adnm == villageName && w.GridName == gridName && w.VillageGridName == dr[1].ToString() && w.PersonLiable == personName && w.Year == _year.ToString()).FirstOrDefault();
                        if (null != checkPerson)
                        {
                            _el.msg = "第  " + i + " 行有重复数据！";
                        }
                    }
                    if (!string.IsNullOrEmpty(_el.msg))
                    {
                        _erlist.Add(_el);
                    }
                    i++;
                }
                _br.ErrorList = _erlist;
                if (_br.ErrorList.Count > 0)
                {
                    _br.IsSuccess = false;
                    _br.ErrorMsg = "数据异常,请查看数据异常提醒！";
                    return _br;
                }
                #endregion
                #region 取出数据写入实体类
                foreach (DataRow dr in dt.Rows)
                {
                    //实例化责任人对象
                    VillageGridPersonLiable item = new VillageGridPersonLiable();
                    //获取组code
                    var A = adcdList.Where<VillageViewModel>(w => w.adnm == dr[0].ToString().Trim()).FirstOrDefault();
                    item.VillageADCD = A.adcd;
                    item.VillageGridName = string.IsNullOrEmpty(dr[1].ToString()) ? "" : dr[1].ToString().Trim();
                    var B = gridList.Where<GridViewModel>(w => w.GridName == dr[2].ToString().Trim()).FirstOrDefault();
                    item.GridName = B.GridName;
                    item.PersonLiable = string.IsNullOrEmpty(dr[3].ToString()) ? "" : dr[3].ToString().Trim();
                    item.Position = string.IsNullOrEmpty(dr[4].ToString()) ? "" : dr[4].ToString().Trim();
                    item.HandPhone = string.IsNullOrEmpty(dr[5].ToString()) ? "" : dr[5].ToString().Trim();
                    item.Remarks = string.IsNullOrEmpty(dr[6].ToString()) ? "" : dr[6].ToString().Trim();
                    item.Year = _year;
                    item.AddTime = DateTime.Now;
                    #region 单条日志
                    operateLog log = new operateLog();
                    log.userName = RealName;
                    log.operateTime = DateTime.Now;
                    log.operateMsg = A.adcd + "村{" + _year + "}导入新增了网格责任人{" + item.PersonLiable + "}的信息";
                    List<operateLog> listLog = new List<operateLog>();
                    listLog.Add(log);
                    item.operateLog = JsonTools.ObjectToJson(listLog);
                    #endregion
                    if (null != AuditNums && AuditNums.Value > 1) item.AuditNums = AuditNums.Value + 1;
                    //写入数据库
                    using (var dbvwg = DbFactory.Open())
                    {
                        var builservwg = dbvwg.From<VillageGridPersonLiable>();
                        if (dbvwg.Insert(item) == 1)
                        {
                            _br.IsSuccess = true;
                        }
                        else
                        {
                            throw new Exception("写入失败");
                        }
                    }
                }
                _br.IsSuccess = true;
                _br.ErrorMsg = "";
                #endregion
                #region 日志
                StringBuilder sb = new StringBuilder();
                sb.Append("在栏目{组织责任/行政村网格责任人}下,导入数据{" + dt.Rows.Count + "}条");
                _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
                #endregion
            }
            catch (Exception ex)
            {
                _br.IsSuccess = false;
                _br.ErrorMsg = "数据导入异常:" + ex.Message;
            }
            finally
            {
                File.Delete(newpath);
            }
            return _br;
        }
        #endregion

        #region 某个村网格责任人人员下载
        public BaseResult DownLoadOneGrid(DownLoadOneGrid request)
        {
            var _year = null != request.year && !string.IsNullOrEmpty(request.year.ToString()) ? request.year : System.DateTime.Now.Year;
            var name = request.adcdname + _year + " 网格责任人" + DateTime.Now.ToString("MMddhhmmss") + new Random(DateTime.Now.Second).Next(10000);
            List<VillageGridViewModel> RList = null;
            using (var db = DbFactory.Open())
            {
                var builder = db.From<VillageGridPersonLiable>();
                builder.LeftJoin<VillageGridPersonLiable, ADCDInfo>((x, y) => x.VillageADCD == y.adcd);
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
                builder.Where<VillageGridPersonLiable>(x => x.Year == _year && x.VillageADCD == request.adcd);
                builder.Select(" ADCDInfo.adnm,VillageGridPersonLiable.VillageGridName,VillageGridPersonLiable.GridName,VillageGridPersonLiable.PersonLiable,VillageGridPersonLiable.Position,VillageGridPersonLiable.HandPhone,VillageGridPersonLiable.Remarks");
                RList = db.Select<VillageGridViewModel>(builder);
            }
            return DownLoadBase(name, _year, RList);
        }
        public BaseResult DownLoadGrid(DownLoadGrid request)
        {
            var _year = null != request.year && !string.IsNullOrEmpty(request.year.ToString()) ? request.year : System.DateTime.Now.Year;
            var name = request.adcdname + _year + " 网格责任人" + DateTime.Now.ToString("MMddhhmmss") + new Random(DateTime.Now.Second).Next(10000);
            List<VillageGridViewModel> RList = null;
            using (var db = DbFactory.Open())
            {
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

                builder.Where<VillageGridPersonLiable>(y => y.VillageADCD != null && y.Year == _year);

                builder.Select(" ADCDInfo.adnm,VillageGridPersonLiable.VillageGridName,VillageGridPersonLiable.GridName,VillageGridPersonLiable.PersonLiable,VillageGridPersonLiable.Position,VillageGridPersonLiable.HandPhone,VillageGridPersonLiable.Remarks");
                RList = db.Select<VillageGridViewModel>(builder);
            }
            return DownLoadBase(name, _year, RList);
        }
        #endregion

        #region 导入导出公共方法
        //导出
        public BaseResult DownLoadBase(string name, int? _year, List<VillageGridViewModel> RList)
        {
            BaseResult _br = new BaseResult();

            var filesavepath = "Files/" + RealName + "/Grid";
            var _fileexits = System.Web.HttpContext.Current.Server.MapPath("~/" + filesavepath);
            if (!Directory.Exists(_fileexits))
            {
                Directory.CreateDirectory(_fileexits);
            }

            var path = System.Web.HttpContext.Current.Server.MapPath("~/" + filesavepath + "/" + name + ".xls");
            var dt = new System.Data.DataTable();
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet("村级（社区）网格责任人");
            //合并第一行单元格
            //IRow RowHead = sheet.CreateRow(0);
            //sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));
            //string noitce = "注意： ";
            //noitce += "请不要修改<村级网格责任人>表的格式，如果在系统上新增了岗位请重新下载数据模板";
            //RowHead.CreateCell(0).SetCellValue(noitce);
            #region 标题
            var cellStyleHeadTitle = hssfworkbook.CreateCellStyle();
            cellStyleHeadTitle.Alignment = HorizontalAlignment.Center;//居中显示
            cellStyleHeadTitle.VerticalAlignment = VerticalAlignment.Top;//垂直居中
            ExcelHeadAttribute eha = new ExcelHeadAttribute()
            {
                rowIndex = 0,
                firstRow = 0,
                lastRow = 0,
                firstCol = 0,
                lastCol = 6,
                fontColor = 8,
                fontSize = 16,
                HeightInPoints = 50,
                name = "村级(社区)网格责任人"
            };
            CreateHead(hssfworkbook, sheet, cellStyleHeadTitle, eha);
            #endregion
            #region 提醒
            var cellStyleHeadNotice = hssfworkbook.CreateCellStyle();
            cellStyleHeadNotice.Alignment = HorizontalAlignment.Left;//居中显示
            cellStyleHeadNotice.VerticalAlignment = VerticalAlignment.Top;//垂直居中
            string noitce = "注意： ";
            noitce += "请不要修改<村级(社区)网格责任人>表的格式，如果在系统上新增了网格类型请重新下载数据模板";
            ExcelHeadAttribute ehanotice = new ExcelHeadAttribute()
            {
                rowIndex = 1,
                firstRow = 1,
                lastRow = 1,
                firstCol = 0,
                lastCol = 6,
                fontColor = 10,
                fontSize = 10,
                HeightInPoints = 20,
                name = noitce
            };
            CreateHead(hssfworkbook, sheet, cellStyleHeadNotice, ehanotice);
            #endregion
            //表头
            IRow RowBody = sheet.CreateRow(2);
            List<string> listtitle = new List<string>();
            listtitle.Add("行政村名称"); listtitle.Add("网格名称"); listtitle.Add("网格类型"); listtitle.Add("责任人姓名");
            listtitle.Add("责任人职务"); listtitle.Add("责任人手机"); listtitle.Add("备注");

            for (int iColumnIndex = 0; iColumnIndex < listtitle.Count; iColumnIndex++)
            {
                RowBody.CreateCell(iColumnIndex).SetCellValue(listtitle[iColumnIndex].ToString());
                RowBody.Cells[iColumnIndex].Row.HeightInPoints = 20;
                //单元格样式
                ICellStyle cellStyle = hssfworkbook.CreateCellStyle();
                cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cellStyle.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                cellStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                //设置单元格上下左右边框线  
                cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                //文字水平和垂直对齐方式  
                cellStyle.Alignment = HorizontalAlignment.Center;
                cellStyle.VerticalAlignment = VerticalAlignment.Top;
                //是否换行
                cellStyle.WrapText = true;

                RowBody.Cells[iColumnIndex].CellStyle = cellStyle;
                //字体大小
                IFont cellfont = hssfworkbook.CreateFont();
                cellfont.FontHeightInPoints = 14;
                cellStyle.SetFont(cellfont);
                sheet.SetColumnWidth(iColumnIndex, 20 * 256);
            }
            /****内容****/
            try
            {
                int i = 0;
                RList.ForEach(w => {
                    IRow row1 = sheet.CreateRow(i + 3);
                    for (int j = 0; j < 7; j++)
                    {
                        ICell cell = row1.CreateCell(j);
                        switch (j)
                        {
                            case 0: cell.SetCellValue(RList[i].adnm.ToString()); break;
                            case 1: cell.SetCellValue(RList[i].VillageGridName.ToString()); break;
                            case 2: cell.SetCellValue(RList[i].GridName.ToString()); break;
                            case 3: cell.SetCellValue(RList[i].PersonLiable.ToString()); break;
                            case 4: cell.SetCellValue(RList[i].Position.ToString()); break;
                            case 5: cell.SetCellValue(RList[i].HandPhone.ToString()); break;
                            case 6: cell.SetCellValue(RList[i].Remarks.ToString()); break;
                        }
                    }
                    i++;
                });
                _br.IsSuccess = true;
                #region 日志
                StringBuilder sb = new StringBuilder();
                sb.Append("在栏目{组织责任/行政村网格责任人}下,导出数据{" + RList.Count + "}条");
                _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.导出);
                #endregion
            }
            catch (Exception ex)
            {
                _br.IsSuccess = false;
                throw new Exception(ex.Message);
            }
            //转为字节数组
            MemoryStream stream = new MemoryStream();
            hssfworkbook.Write(stream);
            var buf = stream.ToArray();

            //保存为Excel文件
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
            }
            _br.filepath = filesavepath + "/" + name + ".xls";
            /*******************************/
            return _br;
        }

        //表头制作
        public void CreateHead(IWorkbook workBook, ISheet sheet, ICellStyle cellStyleHeadTitle, ExcelHeadAttribute eha)
        {
            IRow RowHeadTitle = sheet.CreateRow(eha.rowIndex);
            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(eha.firstRow, eha.lastRow, eha.firstCol, eha.lastCol));
            RowHeadTitle.CreateCell(0).SetCellValue(eha.name);
            /*****单元格样式 start******/
            cellStyleHeadTitle.WrapText = true;
            //高度
            RowHeadTitle.HeightInPoints = eha.HeightInPoints; //2 * sheet.DefaultRowHeight / 10;
            //字体设置,字体要调用CreateFont()
            IFont fontheadTitle = workBook.CreateFont();
            fontheadTitle.FontHeightInPoints = eha.fontSize;
            //HSSFColor.OliveGreen.Black.Index
            fontheadTitle.Color = eha.fontColor;
            cellStyleHeadTitle.SetFont(fontheadTitle);
            //这里调试出来的,样式一定要给到单元格才有效
            RowHeadTitle.Cells[0].CellStyle = cellStyleHeadTitle;
        }
        #endregion
    }
}
