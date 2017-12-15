using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.Model.Village;
using System.Collections;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.Model.Common;
using Dy.Common;
using GrassrootsFloodCtrl.Model.Post;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using GrassrootsFloodCtrl.ServiceModel.Post;
using System.Data;
using GrassrootsFloodCtrl.Logic.Common;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;
using GrassrootsFloodCtrl.Model.Supervise;
using Aspose.Cells;

namespace GrassrootsFloodCtrl.Logic.Village.WorkingGroup
{
    public class VillageWorkingGroupManage : ManagerBase,IVillageWorkingGroupManage
    {
        public ILogHelper _ILogHelper { get; set; }
        /// <summary>
        /// 统计村 已上传，未上传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<VillageViewModel> GetList(GetList request)
        {
            try
            {
                using (var db = DbFactory.Open())
                {
                    if (string.IsNullOrEmpty(adcd))
                    { throw new Exception("请重新登录"); }
                    //取镇下面的所有行政村
                    var builder = db.From<ADCDInfo>();
                    builder.LeftJoin<ADCDInfo, VillageWorkingGroup>((x, y) => x.adcd == y.VillageADCD);
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
                        builder.Where<VillageWorkingGroup>(y => y.VillageADCD == null);
                    }
                    else if (null != request.status && request.status.Value == 1)
                    {
                        builder.Where<VillageWorkingGroup>(y => y.VillageADCD != null && y.Year == _year);
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

        /// <summary>
        /// 获取村工作组成员信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<VillageWorkingGroupViewModel> GetGroup(GetGroup request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(adcd))
                { throw new Exception("请重新登录"); }
                var builder = db.From<VillageWorkingGroup>();
                builder.LeftJoin<VillageWorkingGroup, ADCDInfo>((x,y) => x.VillageADCD == y.adcd);
                var _year = null != request.year && !string.IsNullOrEmpty(request.year.ToString()) ? request.year : System.DateTime.Now.Year;
                builder.Where<VillageWorkingGroup>(x=>x.Year == _year && x.VillageADCD == request.adcd);
                if (request.nums != null && request.nums > 1) builder.Where<VillageWorkingGroup>(w=>w.AuditNums == request.nums);
                builder.Select(" VillageWorkingGroup.*,ADCDInfo.adnm");
                var count = db.Count(builder);
              
                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(o => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                    builder.OrderByDescending(o => request.Sort);
                else
                    builder.OrderByDescending(o=>o.ID);

                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                builder.Limit(PageIndex, PageSize);
                var RList=db.Select<VillageWorkingGroupViewModel>(builder);
                
                return new BsTableDataSource<VillageWorkingGroupViewModel>() { rows = RList, total = count };
            }
        }

        public BsTableDataSource<StatiscPerson> GetGroup1(GetGroup1 request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(adcd))
                { throw new Exception("请重新登录"); }
                var builder = db.From<VillageWorkingGroup>();
                builder.LeftJoin<VillageWorkingGroup, ADCDInfo>((x, y) => x.VillageADCD == y.adcd);
                var _year = null != request.year && !string.IsNullOrEmpty(request.year.ToString()) ? request.year : System.DateTime.Now.Year;
                builder.Where<VillageWorkingGroup>(x => x.Year == _year && x.VillageADCD == request.adcd);
                
                builder.Select(" VillageWorkingGroup.*,ADCDInfo.adnm");
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
                var RList = db.Select<VillageWorkingGroupViewModel>(builder);
                var newlist = RList.Select(w => w.PersonLiable).Distinct().ToList();
                newlist.Remove("");
                newlist.Remove(null);
                List<StatiscPerson> rlist = new List<StatiscPerson>();
                newlist.ForEach(w=> {
                    var userpost = ""; var phones = "";
                    var f = RList.Where(y => y.PersonLiable == w).ToList();
                    f.ForEach(x=> {
                        userpost += x.Post + "_" + x.Position + ";";
                        phones += x.HandPhone + ';';
                    });
                    var rphones=phones.Split(new[] { ';' },StringSplitOptions.RemoveEmptyEntries).Distinct();
                    StatiscPerson vwgv = new StatiscPerson() {
                        personLiable = w,
                        post = userpost.TrimEnd(';'),
                        handPhone = string.Join(";",rphones), //f.FirstOrDefault().HandPhone,
                        id = f.FirstOrDefault().ID,
                        adcd=f.FirstOrDefault().VillageADCD
                    };
                    rlist.Add(vwgv);
                });
                return new BsTableDataSource<StatiscPerson>() { rows = rlist, total = rlist.Count() };
            }
        }

        public BsTableDataSource<StaticsVillageGroup> GetGroupOne(GetGroupOne request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(adcd)) throw new Exception("请重新登录");
                if(string.IsNullOrEmpty(request.adcd)) throw new Exception("参数异常！"); 
                var builder = db.From<VillageWorkingGroup>();
                builder.LeftJoin<VillageWorkingGroup, Model.Post.Post>((x,y)=>x.Post == y.PostName);
                var _year = null != request.year && !string.IsNullOrEmpty(request.year.ToString()) ? request.year : System.DateTime.Now.Year;
                builder.Where<VillageWorkingGroup>(x => x.Year == _year && x.VillageADCD == request.adcd);
                builder.Where<Model.Post.Post>(w=>w.PostType == "行政村防汛防台工作组");
                builder.Select("VillageWorkingGroup.*,Post.ID as Pid");
                var count = db.Count(builder);
                builder.OrderBy<Model.Post.Post>(o => o.orderId);

                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                builder.Limit(PageIndex, PageSize);
                var RList = db.Select<VillageWorkingGroupViewModel>(builder);
                var RList1 = RList.Select(w=>w.Post).Distinct().ToList();
                try
                {
                    List<StaticsVillageGroup> lsvg = new List<StaticsVillageGroup>();
                    RList1.ForEach(w =>
                    {
                        StaticsVillageGroup svg = new StaticsVillageGroup();
                        svg.post = w;
                        //var fpid = RList.Where(x => x.Post == w);
                        var R = RList.Where(x => x.Post == w).Distinct().ToList();
                        svg.postid = R.FirstOrDefault().PId;
                        List<PersonLiabel> lpl = new List<PersonLiabel>();
                        R.ForEach(y =>
                        {
                            PersonLiabel p = new PersonLiabel();
                            p.adcd = y.VillageADCD;
                            p.name = y.PersonLiable;
                            p.position = y.Position;
                            p.mobile = y.HandPhone;
                            lpl.Add(p);
                        });
                        svg.datas = lpl;
                        lsvg.Add(svg);
                    });
                return new BsTableDataSource<StaticsVillageGroup>() { rows = lsvg, total = count };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public BaseResult SaveGroup(SaveGroup request)
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
                    if (string.IsNullOrEmpty(request.post)) throw new Exception("参数岗位不能为空！");
                   
                    VillageWorkingGroup _vgr = new VillageWorkingGroup()
                    {
                        VillageADCD = request.villageadcd.Trim(),
                        PersonLiable = request.personLiable.Trim(),
                        HandPhone = request.handphone.Trim(),
                        Post = request.post,
                        Position = string.IsNullOrEmpty(request.position)?"":request.position.Trim(),
                        AddTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                        Year = _year,
                        Remarks = string.IsNullOrEmpty(request.remark) ? "" : request.remark.Trim()
                    };
                    //新数据
                    List<VillageWorkingGroup> _newdata = new List<VillageWorkingGroup>();
                    _newdata.Add(_vgr);
                    //
                    var builder = db.From<VillageWorkingGroup>();
                    if (null == request.id)
                    {
                        var f = db.Single<VillageWorkingGroup>(w => w.VillageADCD == request.villageadcd && w.PersonLiable == request.personLiable && w.Post == request.post && w.HandPhone == request.handphone);
                        if (null != f) throw new Exception("重复数据：同村同岗位同责任人同手机！");
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
                        log.operateMsg = request.villageadcd + "村{" + _year + "}新增了防汛防台工作组责任人{" + request.personLiable + "}的信息";
                        List<operateLog> listLog = new List<operateLog>();
                        listLog.Add(log);
                        _vgr.operateLog = JsonTools.ObjectToJson(listLog);
                        br.IsSuccess = db.Insert(_vgr) == 1 ? true : false;
                        
                        #region 日志
                        StringBuilder sb = new StringBuilder();
						var adcdInfo = db.Single<ADCDInfo>(w => w.adcd == _vgr.VillageADCD); 
                        sb.Append("在栏目{组织责任/行政村防汛防台工作组}下,新增了数据{");
                        sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                        sb.Append("责任人：" + _vgr.PersonLiable + ";");
                        sb.Append("手机：" + _vgr.HandPhone + ";");
                        sb.Append("岗位：" + _vgr.Post + ";");
                        sb.Append("职位：" + _vgr.Position + ";");
                        sb.Append("添加时间：" + _vgr.AddTime + ";");
                        sb.Append("年份：" + _vgr.Year + ";");
                        sb.Append("备注：" + _vgr.Remarks + ";");
                        sb.Append("}");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
                        #endregion
                    }
                    if (null != request.id && request.id.Value > 0)
                    {
                        operateLog log = new operateLog();
                        log.userName = RealName;
                        log.operateTime = DateTime.Now;
                        log.operateMsg = request.villageadcd + "村{" + _year + "}修改了防汛防台工作组责任人{" + request.personLiable + "}的信息";
                        List<operateLog> listLog = new List<operateLog>();
                        listLog.Add(log);
                        _vgr.operateLog = JsonTools.ObjectToJson(listLog);
                        _vgr.EditTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        #region 旧数据，新数据
                        //取出数据
                        //写入更新记录
                        var r = db.Single<VillageWorkingGroup>(w => w.ID == request.id);
                        VillageWorkingGroup _olddata = new VillageWorkingGroup()
                        {
                            VillageADCD = r.VillageADCD,
                            PersonLiable = r.PersonLiable,
                            HandPhone = r.HandPhone,
                            Post = r.Post,
                            Position = r.Position,
                            AddTime = r.AddTime,
                            Year = r.Year,
                            Remarks = r.Remarks,
                            EditTime = r.EditTime
                        };
                        if (AuditNums != null)
                        {
                            List<VillageWorkingGroup> _listOldData = new List<VillageWorkingGroup>();
                            _listOldData.Add(_olddata);
                            _vgr.AuditNums = AuditNums.Value + 1;
                            //旧数据写入实体
                            _vgr.OldData = JsonTools.ObjectToJson(_listOldData);
                            //新数据写入实体
                            _vgr.NewData = JsonTools.ObjectToJson(_newdata);
                        }
                        #endregion
                        br.IsSuccess = db.Update<VillageWorkingGroup>(_vgr,x=>x.ID == request.id) == 1 ? true : false;
                        #region 日志
                        StringBuilder sb = new StringBuilder();
						var adcdInfo = db.Single<ADCDInfo>(w => w.adcd == _olddata.VillageADCD); 
                        sb.Append("在栏目{组织责任/行政村防汛防台工作组}下,修改数据{");
                        sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                        sb.Append("责任人：" + _olddata.PersonLiable + ";");
                        sb.Append("手机：" + _olddata.HandPhone + ";");
                        sb.Append("岗位：" + _olddata.Post + ";");
                        sb.Append("职位：" + _olddata.Position + ";");
                        sb.Append("添加时间：" + _olddata.AddTime + ";");
                        sb.Append("年份：" + _olddata.Year + ";");
                        sb.Append("备注：" + _olddata.Remarks + ";");
                        sb.Append("}为{");
                        sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                        sb.Append("责任人：" + _vgr.PersonLiable + ";");
                        sb.Append("手机：" + _vgr.HandPhone + ";");
                        sb.Append("岗位：" + _vgr.Post + ";");
                        sb.Append("职位：" + _vgr.Position + ";");
                        sb.Append("添加时间：" + _vgr.AddTime + ";");
                        sb.Append("年份：" + _vgr.Year + ";");
                        sb.Append("备注：" + _vgr.Remarks + ";");
                        sb.Append("}");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
                        #endregion
                    }
                }
            }
             catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return br;
        }

        public BaseResult DeleteGroup(DeleteGroup request)
        {
            BaseResult br = new BaseResult();
            try
            {
                using (var db=DbFactory.Open())
                {
                    if (string.IsNullOrEmpty(request.id))
                        throw new Exception("参数异常！");
                    ArrayList arr = new ArrayList();
                    string[] arrs = request.id.Split(',');
                    for (int i = 0; i < arrs.Length; i++)
                    {
                        var id = int.Parse(arrs[i]);
                        arr.Add(id);
                        #region 日志
                        var _vgr = db.Single<VillageWorkingGroup>(w => w.ID == id);
                        StringBuilder sb = new StringBuilder();
						var adcdInfo = db.Single<ADCDInfo>(w => w.adcd == _vgr.VillageADCD); 
                        sb.Append("在栏目{组织责任/行政村防汛防台工作组}下,删除了数据{");
                        sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                        sb.Append("责任人：" + _vgr.PersonLiable + ";");
                        sb.Append("手机：" + _vgr.HandPhone + ";");
                        sb.Append("岗位：" + _vgr.Post + ";");
                        sb.Append("职位：" + _vgr.Position + ";");
                        sb.Append("添加时间：" + _vgr.AddTime + ";");
                        sb.Append("年份：" + _vgr.Year + ";");
                        sb.Append("备注：" + _vgr.Remarks + ";");
                        sb.Append("}");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.删除);
                        #endregion
                    }
                    br.IsSuccess=db.Delete<VillageWorkingGroup>(x => Sql.In(x.ID, arr)) > 0;
                }
            }
            catch (Exception ex) {
                br.IsSuccess = false;
                br.ErrorMsg = ex.Message;
                throw new Exception(ex.Message);
            }
            return br;
        }

        public VillageWorkingGroupViewModel DownloadVillageGroup(GetGroup request)
        {
            throw new NotImplementedException();
        }

        public BsTableDataSource<VillageWorkingGroupViewModel> GetPsersonLiableList(GetPersonLiableList request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(adcd))
                { throw new Exception("请重新登录"); }
                //取镇下面的所有行政村
                var builder = db.From<VillageWorkingGroup>();
                builder.LeftJoin<VillageWorkingGroup, ADCDInfo>((x, y) => x.VillageADCD == y.adcd);
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
                builder.Where<VillageWorkingGroup>(y => y.VillageADCD != null && y.Year == _year);
                builder.Select<VillageWorkingGroup, ADCDInfo>((x, y) => new
                {
                    adcd = x.VillageADCD,
                    adnm = y.adnm,
                    id = x.ID,
                    PersonLiable = x.PersonLiable,
                    Position = x.Position,
                    Post = x.Post,
                    HandPhone = x.HandPhone,
                    Remarks = x.Remarks,
                    AddTime = x.AddTime,
                    Year = x.Year
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
                var RList = db.Select<VillageWorkingGroupViewModel>(builder);

                return new BsTableDataSource<VillageWorkingGroupViewModel>() { rows = RList, total = count };
            }
        }
        public BsTableDataSource<VillageWorkingGroupViewModel> GetPsersonLiable(GetPersonLiable request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(adcd))
                { throw new Exception("请重新登录"); }
                //取镇下面的所有行政村
                var builder = db.From<VillageWorkingGroup>();
                if(null == request.id && request.id <= 0)
                { throw new Exception("参数异常"); }
                //var _year = null != request.year && !string.IsNullOrEmpty(request.year.ToString()) ? request.year : System.DateTime.Now.Year;
                // builder.Where<VillageWorkingGroup>(y => y.VillageADCD != null && y.Year == _year && y.ID==request.id);
                builder.Where<VillageWorkingGroup>(y=>y.ID == request.id);
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
                var RList = db.Select<VillageWorkingGroupViewModel>(builder);

                return new BsTableDataSource<VillageWorkingGroupViewModel>() { rows = RList, total = count };
            }
        }

        public BaseResult DelVillageGroup(DelVillageGroup request)
        {
            BaseResult br = new BaseResult();
            try
            {
                using (var db = DbFactory.Open())
                {
                    if (string.IsNullOrEmpty(request.adcd))
                        throw new Exception("参数异常！");
                    var builder = db.From<VillageWorkingGroup>();
                    var _year = null != request.year && !string.IsNullOrEmpty(request.year.ToString()) ? int.Parse(request.year) :DateTime.Now.Year;
                    #region 日志
                    StringBuilder sb = new StringBuilder();
					var adcdInfo = db.Single<ADCDInfo>(w => w.adcd == request.adcd); 
                    sb.Append("在栏目{组织责任/行政村防汛防台工作组}下,删除了村{" + adcdInfo.adnm + "}及其数据");
                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.删除);
                    #endregion
                    br.IsSuccess = db.Delete<VillageWorkingGroup>(w => w.VillageADCD == request.adcd && w.Year == _year) == 1 ? true : false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return br;
        }
       
        #region 工作组模板下载
        public UploadFileViewModel DownloadVWGFileModel(GetVWGFileModel request)
        {
            UploadFileViewModel _ufvm = new UploadFileViewModel();
            var _year = null == request.year || request.year == 0 ? DateTime.Now.Year : request.year;
            var name = RealName + _year+ "“村级防汛防台工作组”模板"+DateTime.Now.ToString("MMddhhmmss") + new Random(DateTime.Now.Second).Next(10000);
            var filesavepath = "Files/"+RealName + "/WorkingGroup";
            var _fileExits = System.Web.HttpContext.Current.Server.MapPath("~/"+filesavepath);
            if (!Directory.Exists(_fileExits))
            {
                Directory.CreateDirectory(_fileExits);
            }
            var path = System.Web.HttpContext.Current.Server.MapPath("~/"+ filesavepath + "/" + name + ".xls");
            /*******************************/
            HSSFWorkbook workBook = new HSSFWorkbook();
            createSheet1(workBook, "村级防汛防台工作组");
            //获取行政村名录
            var createxcel = new creatExcel();
            List<string> cunList = new List<string>();
            using (var db=DbFactory.Open())
            {
                cunList = db.Select<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd).Select(x => x.adnm).ToList();
            }
            createxcel.setSheet3(workBook, "行政村名录", cunList);

            using (FileStream file = new FileStream(path, FileMode.Create))
            {
                workBook.Write(file);//创建Excel文件。
                file.Close();
            }
            _ufvm.fileSrc = filesavepath+"/" + name + ".xls";
            /*******************************/
            return _ufvm;
        }
        private ISheet createSheet1(HSSFWorkbook workBook, string sheetName)
        {
            ISheet sheet = workBook.CreateSheet(sheetName);
            #region 标题
            var cellStyleHeadTitle = workBook.CreateCellStyle();
            cellStyleHeadTitle.Alignment = HorizontalAlignment.Center;//居中显示
            cellStyleHeadTitle.VerticalAlignment = VerticalAlignment.Top;//垂直居中
            ExcelHeadAttribute eha = new ExcelHeadAttribute() {
                rowIndex = 0,
                firstRow = 0,
                lastRow = 0,
                firstCol = 0,
                lastCol = 5,
                fontColor = 8, fontSize = 16, HeightInPoints = 50, name = "村级防汛防台工作组"
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
            noitce += "4、岗位请下拉选择。   ";
            noitce += "5、小灵通(格式:区号\" - \"号码)。";
            ExcelHeadAttribute ehanotice = new ExcelHeadAttribute()
            {
                rowIndex = 1,
                firstRow = 1,
                lastRow = 1,
                firstCol = 0,
                lastCol = 5,
                fontColor = 10,
                fontSize = 14,
                HeightInPoints = 40,
                name = noitce
            };
            CreateHead(workBook, sheet, cellStyleHeadNotice, ehanotice);
            #endregion
            /*********添加表头 s********/
            IRow RowBody = sheet.CreateRow(2);
            List<string> listtitle = new List<string>();
            listtitle.Add("*行政村"); listtitle.Add("*岗位(仅限下拉内容)"); listtitle.Add("*责任人姓名");
            listtitle.Add("责任人职务"); listtitle.Add("*责任人手机"); listtitle.Add("备注");

            for (int iColumnIndex = 0; iColumnIndex < listtitle.Count(); iColumnIndex++)
            {
                //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
                //if(iColumnIndex >=2 && iColumnIndex <= 4)
                //{
                //    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 2, 4));
                //    RowBody.CreateCell(iColumnIndex).SetCellValue("责任人");
                //    RowBody.CreateCell(iColumnIndex).SetCellValue(listtitle[iColumnIndex].ToString());
                //}
                //else
                //{
                //    sheet.AddMergedRegion(new CellRangeAddress(1, 2, iColumnIndex, iColumnIndex));
                //    RowBody.CreateCell(iColumnIndex).SetCellValue(listtitle[iColumnIndex].ToString());
                //}
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
                sheet.SetColumnWidth(iColumnIndex, 25 * 256);
            }
            /*********添加表头 e********/
            //设置下拉框
            setSheet2(workBook, sheet);
            return sheet;
        }
        private void setSheet2(HSSFWorkbook workBook, ISheet sheet)
        {
            //创建表
            ISheet sheet2 = workBook.CreateSheet("岗位数据");
            //隐藏
            workBook.SetSheetHidden(1, true);
            //取数据
            using (var db=DbFactory.Open())
            {
                var builder = db.From<Model.Post.Post>().Where(w=>w.PostType== ZZTXEnums.行政村防汛防台工作组.ToString());
                var rlist = db.Select<PostViewModel>(builder);
                for (int iRowIndex = 0; iRowIndex < rlist.Count; iRowIndex++)
                {
                    sheet2.CreateRow(iRowIndex).CreateCell(0).SetCellValue(rlist[iRowIndex].PostName);
                }
            }
            //设计表名称
            IName range = workBook.CreateName();
            range.RefersToFormula = "岗位数据!$A:$A";
            range.NameName = "PostDataName";
            //定义下拉框范围
            CellRangeAddressList regions = new CellRangeAddressList(3, 65535, 1, 1);
            //设置数据引用
            DVConstraint constraint = DVConstraint.CreateFormulaListConstraint("PostDataName");
            HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
            sheet.AddValidationData(dataValidate);
        }
        #endregion
        #region 工作组批量导入
        public BaseResult UploadWGFiles(UploadWGFiles request)
        {
            BaseResult _br = new BaseResult();
            if (string.IsNullOrEmpty(request.fpath)) throw new Exception("参数异常！");
            var _year = null == request.year ? DateTime.Now.Year : request.year;
            var newpath = System.Web.HttpContext.Current.Server.MapPath(request.fpath);
            try
            { 
                //导入数据
               // var dt = Logic.Common.ExcelHelper.GetDataTable(newpath);
                Workbook workbook = new Workbook();
                workbook.Open(newpath);
                Cells cells = workbook.Worksheets[0].Cells;
                var dt= cells.ExportDataTable(0, 0, cells.MaxDataRow+1, cells.MaxColumn+1, true);
                //移出表头
                dt.Rows.RemoveAt(0);
                dt.Rows.RemoveAt(0);
                #region 基础数据集
                //取出岗位
                List<PostViewModel> postList = null;
                using (var dbpost = DbFactory.Open())
                {
                    var builderpost = dbpost.From<Model.Post.Post>();
                    postList = dbpost.Select<PostViewModel>(builderpost);
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
                List<VillageWorkingGroupViewModel> vlist = null;
                using (var db = DbFactory.Open())
                {
                    var builder = db.From<ADCDInfo>();
                    builder.LeftJoin<ADCDInfo, VillageWorkingGroup>((x, y) => x.adcd == y.VillageADCD);
                    if (adcd.Length == 15 && adcd.IndexOf("000000") > 0)//登陆的是乡镇用户
                    {
                        builder.Where<ADCDInfo>(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd.ToString());
                    }
                    else
                    {
                        throw new Exception("登陆用户的所属行政区划编码不正确");
                    }
                    builder.Where<VillageWorkingGroup>(y => y.VillageADCD != null && y.Year == _year);
                    builder.Select(" VillageWorkingGroup.*,ADCDInfo.adnm");
                    vlist = db.Select<VillageWorkingGroupViewModel>(builder);
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
                        }else
                        {
                            _c = true;
                        }
                    }
                    //岗位匹配
                    var postName = dr[1].ToString().Trim();
                    if (string.IsNullOrEmpty(postName))
                    {
                        _c = false;
                        _el.msg = "第  " + i + "  行岗位名为空！";
                    }
                    else
                    {   
                        var B = postList.Where<PostViewModel>(w => w.PostName == postName).FirstOrDefault();
                        if (null == B || string.IsNullOrEmpty(B.PostName))
                        {
                            _c = false;
                            _el.msg = "第 " + i + " 行岗位名称和系统里的岗位名称不匹配！新增岗位请先到“岗位管理”栏目设置后,重新下载模板";
                        }else
                        {
                            _c = true;
                        }
                    }
                    //责任人
                    var personName = dr[2].ToString();
                    if (string.IsNullOrEmpty(personName)) {
                        _c = false;
                        _el.msg = "第  " + i + "  行责任人为空！";
                    }
                    else
                    {
                        _c = true;
                        personName = dr[2].ToString().Trim();
                    }
                    //手机
                    var handphone = dr[4].ToString();
                    if (string.IsNullOrEmpty(handphone))
                    {
                        _el.msg = "第  " + i + "  行责任人联系方式为空！";
                    }else if(!ValidatorHelper.IsMobile(handphone) && !ValidatorHelper.IsTelephone(handphone))
                    {
                        _el.msg = "第  " + i + "  行责任人联系方式格式错误！手机或小灵通(格式:区号'-'号码)";
                    }else { }
                    //同村,同岗,同责任人验证
                    if (_c)
                    {
                        var checkPerson = vlist.Where<VillageWorkingGroupViewModel>(w=>w.adnm == villageName && w.Post == postName && w.PersonLiable == personName && w.Year==_year.ToString()).FirstOrDefault();
                        if(null != checkPerson)
                        {
                            _el.msg = "第  " + i + " 行有同村,同岗,同责任人的重复数据！";
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
                    VillageWorkingGroup item = new VillageWorkingGroup();
                    //获取组code
                    var A= adcdList.Where<VillageViewModel>(w=>w.adnm == dr[0].ToString().Trim()).FirstOrDefault();
                    item.VillageADCD = A.adcd;
                    var B = postList.Where<PostViewModel>(w=>w.PostName == dr[1].ToString().Trim()).FirstOrDefault();
                    item.Post = B.PostName;
                    item.PersonLiable = string.IsNullOrEmpty(dr[2].ToString())?"": dr[2].ToString().Trim();
                    item.Position = string.IsNullOrEmpty(dr[3].ToString()) ? "" : dr[3].ToString().Trim();
                    item.HandPhone = string.IsNullOrEmpty(dr[4].ToString()) ? "" : dr[4].ToString().Trim();
                    item.Remarks = string.IsNullOrEmpty(dr[5].ToString()) ? "" : dr[5].ToString().Trim();
                    item.Year = _year;
                    item.AddTime = DateTime.Now;
                    #region 单条日志
                    operateLog log = new operateLog();
                    log.userName = RealName;
                    log.operateTime = DateTime.Now;
                    log.operateMsg = A.adcd + "村{" + _year + "}导入新增了工作组责任人{" + item.PersonLiable + "}的信息";
                    List<operateLog> listLog = new List<operateLog>();
                    listLog.Add(log);
                    item.operateLog = JsonTools.ObjectToJson(listLog);
                    #endregion
                    if (null != AuditNums && AuditNums.Value > 1) item.AuditNums = AuditNums.Value + 1;
                    //写入数据库
                    using (var dbvwg = DbFactory.Open())
                    {
                        var builservwg = dbvwg.From<VillageWorkingGroup>();
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
                #region 日志
                StringBuilder sb = new StringBuilder();
                sb.Append("在栏目{组织责任/行政村防汛防台工作组}下,导入数据{" + dt.Rows.Count + "}条");
                _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
                #endregion
                _br.IsSuccess = true;
                _br.ErrorMsg = "";
                #endregion
            }
            catch (Exception ex)
            {
                _br.IsSuccess = false;
                _br.ErrorMsg = "数据导入异常:" + ex.Message;
                File.Delete(newpath);
            }
            finally
            {
                File.Delete(newpath);
            }
            return _br;
        }
        #endregion
        #region 工作组人员下载
        public BaseResult DownLoadOneVillage(DownLoadOneVillage request)
        {
            if (string.IsNullOrEmpty(adcd))
            {
                throw new Exception("请重新登录");
            }
            var _year = null != request.year && !string.IsNullOrEmpty(request.year.ToString()) ? request.year : System.DateTime.Now.Year;
            var name = request.adcdname + _year + "防汛防台工作组" + DateTime.Now.ToString("MMddhhmmss") + new Random(DateTime.Now.Second).Next(10000);
            List< VillageWorkingGroupViewModel > RList = null;
            using (var db = DbFactory.Open())
            {
                var builder = db.From<VillageWorkingGroup>();
                builder.LeftJoin<VillageWorkingGroup, ADCDInfo>((x, y) => x.VillageADCD == y.adcd);
                builder.Where<VillageWorkingGroup>(x => x.Year == _year && x.VillageADCD == request.adcd);
                builder.Select(" ADCDInfo.adnm,VillageWorkingGroup.PersonLiable,VillageWorkingGroup.Post,VillageWorkingGroup.Position,VillageWorkingGroup.HandPhone,VillageWorkingGroup.Remarks");
                RList = db.Select<VillageWorkingGroupViewModel>(builder);
            }
            return DownLoadBase(name,_year, RList);
        }
        public BaseResult DownLoadVillage(DownLoadVillage request)
        {
            if (string.IsNullOrEmpty(adcd))
            {
                throw new Exception("请重新登录");
            }
            List<VillageWorkingGroupViewModel> RList = null;
            var _year = null != request.year && !string.IsNullOrEmpty(request.year.ToString()) ? request.year : System.DateTime.Now.Year;
            var name = request.adcdname + _year + "防汛防台工作组" + DateTime.Now.ToString("MMddhhmmss") + new Random(DateTime.Now.Second).Next(10000);
            using (var db = DbFactory.Open())
            {
                //取镇下面的所有行政村
                var builder = db.From<ADCDInfo>();
                builder.LeftJoin<ADCDInfo, VillageWorkingGroup>((x, y) => x.adcd == y.VillageADCD);
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
                builder.Where<VillageWorkingGroup>(y => y.VillageADCD != null && y.Year == _year);
                RList = db.Select<VillageWorkingGroupViewModel>(builder);
            }
           return DownLoadBase( name, _year,RList);
        }
        #endregion
        #region 导入导出公共方法
        //导出
        public BaseResult DownLoadBase(string name, int? _year, List<VillageWorkingGroupViewModel> RList)
        {
            BaseResult _br = new BaseResult();
            var _filesavepath = "Files/" + RealName + "/WorkingGroup";
            var _fileexits = System.Web.HttpContext.Current.Server.MapPath("~/" + _filesavepath);
            if (!Directory.Exists(_fileexits))
            {
                Directory.CreateDirectory(_fileexits);
            }

            var path = System.Web.HttpContext.Current.Server.MapPath("~/" + _filesavepath + "/" + name + ".xls");
            var dt = new System.Data.DataTable();
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet("村级防汛防台工作组");
            //合并第一行单元格
            //IRow RowHead = sheet.CreateRow(0);
            //sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 5));

            //string noitce = "注意： ";
            //noitce += "请不要修改<村级防汛防台工作组>表的格式，如果在系统上新增了岗位请重新下载数据模板";

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
                lastCol = 5,
                fontColor = 8,
                fontSize = 16,
                HeightInPoints = 50,
                name = "村级防汛防台工作组"
            };
            CreateHead(hssfworkbook, sheet, cellStyleHeadTitle, eha);
            #endregion
            #region 提醒
            var cellStyleHeadNotice = hssfworkbook.CreateCellStyle();
            cellStyleHeadNotice.Alignment = HorizontalAlignment.Left;//居中显示
            cellStyleHeadNotice.VerticalAlignment = VerticalAlignment.Top;//垂直居中
            string noitce = "注意： ";
            noitce += "请不要修改<村级防汛防台工作组>表的格式，如果在系统上新增了岗位请重新下载数据模板";
            ExcelHeadAttribute ehanotice = new ExcelHeadAttribute()
            {
                rowIndex = 1,
                firstRow = 1,
                lastRow = 1,
                firstCol = 0,
                lastCol = 5,
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
            listtitle.Add("行政村名称"); listtitle.Add("岗位"); listtitle.Add("责任人姓名");
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
                    for (int j = 0; j < 6; j++)
                    {
                        ICell cell = row1.CreateCell(j);
                        switch (j)
                        {
                            case 0: cell.SetCellValue(RList[i].adnm.ToString()); break;
                            case 1: cell.SetCellValue(RList[i].Post.ToString()); break;
                            case 2: cell.SetCellValue(RList[i].PersonLiable.ToString()); break;
                            case 3: cell.SetCellValue(RList[i].Position.ToString()); break;
                            case 4: cell.SetCellValue(RList[i].HandPhone.ToString()); break;
                            case 5: cell.SetCellValue(RList[i].Remarks.ToString()); break;
                        }
                    }
                    i++;
                });
                _br.IsSuccess = true;
                #region 日志
                StringBuilder sb = new StringBuilder();
                sb.Append("在栏目{组织责任/行政村防汛防台工作组}下,导出数据{" + RList.Count + "}条");
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
            _br.filepath = _filesavepath + "/" + name + ".xls";
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
