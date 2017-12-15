using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dy.Common;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.Model.Town;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.NoTownRoute;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack.OrmLite;
using System.Collections;
using GrassrootsFloodCtrl.Logic.Common;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.Logic.ZZTX;
using GrassrootsFloodCtrl.Logic.Village.WorkingGroup;
using GrassrootsFloodCtrl.Logic.Supervise;
using GrassrootsFloodCtrl.Logic.Country;
using GrassrootsFloodCtrl.Logic.NoAuthticationTown;

namespace GrassrootsFloodCtrl.Logic.Town
{
    public class NoAuthticationTownManager : ManagerBase, NoAuthticationITownManager
    {
        public ILogHelper _ILogHelper { get; set; }
    
        /// <summary>
        /// 获取镇街责任人列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<TownPersonLiableViewModel> NoAuthticationGetTownList(NoAuthticationGetTownList request)
        {
            using (var db = DbFactory.Open())
            {               
                if (request.year == null)
                    throw new Exception("年度异常");
                if (string.IsNullOrEmpty(request.adcd))
                {
                    var builder = db.From<TownPersonLiable>();
                    builder.LeftJoin<TownPersonLiable, ADCDInfo>((x, y) => x.adcd == y.adcd);

                    if (adcd.Length == 15 && adcd.IndexOf("000000") > 0)//登陆的是乡镇用户
                        builder.And(x => x.adcd.StartsWith(adcd.Substring(0, 9)));
                    else if (adcd.Length == 6 && adcd.IndexOf("00") < 0)//登陆的是县级用户
                        builder.And(x => x.adcd.StartsWith(adcd.Substring(0, 6)));
                    else if (adcd == GrassrootsFloodCtrlEnums.AreaCode.省级编码.ToString())//管理员
                    {

                    }
                    else
                        throw new Exception("登陆用户的所属行政区划编码不正确");
                    if (!string.IsNullOrEmpty(request.name))
                        builder.And(x => x.Name.Contains(request.name));
                    if (!string.IsNullOrEmpty(request.Position))
                        builder.And(x => x.Position.Contains(request.Position));
                    if (!string.IsNullOrEmpty(request.Post))
                        builder.And(x => x.Post.Contains(request.Post));
                    builder.And(x => x.Year == request.year);
                    if (request.Id != 0)
                        builder.And(x => x.Id == request.Id);
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
                else
                {
                    var builder = db.From<TownPersonLiable>();
                    builder.And(x => x.adcd == request.adcd);
                    builder.And(x => x.Year == request.year);
                    if (request.nums != null && request.nums.Value > 1) builder.And(x => x.AuditNums == request.nums);
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
        }
        public BsTableDataSource<TownPersonLiableViewModel> NoAuthticationGetTownList1(NoAuthticationGetTownList1 request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(adcd))
                    throw new Exception("请重新登录");
                var builder = db.From<TownPersonLiable>();
                builder.And(x => x.adcd == request.adcd);
                if (!string.IsNullOrEmpty(request.post))
                    builder.Where(w => w.Position.Contains(request.post));
                if (!string.IsNullOrEmpty(request.position))
                    builder.Where(w => w.Post.Contains(request.position));
                if (!string.IsNullOrEmpty(request.name))
                    builder.Where(w => w.Name.Contains(request.name));
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                builder.And(x => x.Year == _year);
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
                var newlist = list.Select(w => w.Name).Distinct().ToList();
                List<TownPersonLiableViewModel> rlist = new List<TownPersonLiableViewModel>();
                var _city = db.Single<ADCDInfo>(w => w.adcd == request.adcd.Substring(0, 4) + "00000000000");
                var _county = db.Single<ADCDInfo>(w => w.adcd == request.adcd.Substring(0, 6) + "000000000");
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
                    TownPersonLiableViewModel tplvm = new TownPersonLiableViewModel()
                    {
                        Name = w,
                        Post = newpost.TrimEnd(';'),
                        Mobile = string.Join(";", fphones),
                        Id = f.FirstOrDefault().Id,
                        adcd = f.FirstOrDefault().adcd,
                        adnmparent = _city.adnm + "_" + _county.adnm
                    };
                    var builder1 = db.From<SpotCheck>();
                    builder1.Where(y => y.adcd == request.adcd && y.year == _year && y.bycheckman == tplvm.Name && y.bycheckphone == tplvm.Mobile).OrderByDescending(o => o.checktime);
                    var fbycheckman = db.Single(builder1);
                    if (fbycheckman != null) { tplvm.checkresult = fbycheckman.checkstatus + "(" + Convert.ToDateTime(fbycheckman.checktime).ToString("yyyy-MM-dd HH:mm:ss") + ")"; }
                    else { tplvm.checkresult = "-"; }
                    rlist.Add(tplvm);
                });
                return new BsTableDataSource<TownPersonLiableViewModel>() { rows = rlist, total = rlist.Count() };
            }
        }
        /// <summary>
        /// 保存镇级责任人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// //request.name, request.Position,request.Post,request.Mobile,request.Remark,request.Id
        public TownPersonLiable NoAuthticationGetTownByName(string name, string post, string position, string mobile, string remark, string id)
        {
            using (var db = DbFactory.Open())
            {
                if (id != "")
                {
                    return db.Single<TownPersonLiable>(x => x.adcd == adcd && x.Name == name && x.Position == post && x.Post == position && x.Remark == remark && x.Mobile == mobile);
                }
                else
                {
                    return db.Single<TownPersonLiable>(x => x.adcd == adcd && x.Name == name && x.Position == post);
                }
            }
        }
        /// <summary>
        /// 保存镇级责任人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool NoAuthticationSaveTown(NoAuthticationSaveTown request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(adcd))
                    throw new Exception("请重新登录");
                if (!ValidatorHelper.IsMobile(request.Mobile))
                    throw new Exception("手机号码输入错误，请重新输入");
                var info = new TownPersonLiable();
                var log = new operateLog();
                log.userName = RealName;
                log.operateTime = DateTime.Now;

                if (request.Id != 0)
                {
                    var model = NoAuthticationGetTownByName(request.name, request.Position, request.Post, request.Mobile, request.Remark, request.Id.ToString());
                    if (model != null)
                        throw new Exception("已存在");
                    info.Id = request.Id;
                    log.operateMsg = "更新" + request.name + "的镇级责任人信息";
                }
                else
                {
                    var model = NoAuthticationGetTownByName(request.name, request.Position, "", "", "", "");
                    if (model != null)
                        throw new Exception("已存在");
                    log.operateMsg = "新增" + request.name + "的镇级责任人信息";
                }

                info.adcd = adcd;
                info.Name = request.name;
                info.CreateTime = DateTime.Now;
                info.Mobile = request.Mobile;
                info.Position = request.Position;
                info.Post = request.Post;
                info.Year = DateTime.Now.Year;
                info.Remark = request.Remark;
                //新数据
                List<TownPersonLiable> _newdata = new List<TownPersonLiable>();
                _newdata.Add(info);
                //
                var listLog = new List<operateLog>();
                listLog.Add(log);
                info.operateLog = JsonTools.ObjectToJson(listLog);
                if (request.Id != 0)
                {
                    #region 旧数据，新数据
                    //取出数据
                    //写入更新记录
                    var r = db.Single<TownPersonLiable>(w => w.Id == request.Id);
                    TownPersonLiable _olddata = new TownPersonLiable()
                    {
                        adcd = r.adcd,
                        Name = r.Name,
                        CreateTime = r.CreateTime,
                        Post = r.Post,
                        Position = r.Position,
                        Mobile = r.Mobile,
                        Year = r.Year,
                        Remark = r.Remark
                    };
                    if (AuditNums != null)
                    {
                        List<TownPersonLiable> _listOldData = new List<TownPersonLiable>();
                        _listOldData.Add(_olddata);
                        info.AuditNums = AuditNums.Value + 1;
                        //旧数据写入实体
                        info.OldData = JsonTools.ObjectToJson(_listOldData);
                        //新数据写入实体
                        info.NewData = JsonTools.ObjectToJson(_newdata);
                    }
                    #endregion
                    #region 日志
                    StringBuilder sb = new StringBuilder();
                    var adcdInfo = db.Single<ADCDInfo>(w => w.adcd == _olddata.adcd);
                    sb.Append("在栏目{组织责任/乡(镇、街道)防汛防台责任人}下,更新数据{");
                    sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                    sb.Append("姓名：" + _olddata.Name + ";");
                    sb.Append("创建时间：" + _olddata.CreateTime + ";");
                    sb.Append("手机：" + _olddata.Mobile + ";");
                    sb.Append("职务：" + _olddata.Position + ";");
                    sb.Append("岗位：" + _olddata.Post + ";");
                    sb.Append("年度：" + _olddata.Year + ";");
                    sb.Append("备注：" + _olddata.Remark + ";");
                    sb.Append("}为{");
                    sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                    sb.Append("姓名：" + info.Name + ";");
                    sb.Append("创建时间：" + info.CreateTime + ";");
                    sb.Append("手机：" + info.Mobile + ";");
                    sb.Append("职务：" + info.Position + ";");
                    sb.Append("岗位：" + info.Post + ";");
                    sb.Append("年度：" + info.Year + ";");
                    sb.Append("备注：" + info.Remark + ";");
                    sb.Append("}");
                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
                    #endregion
                    return db.Update(info) == 1;
                }
                else
                {
                    #region 旧数据，新数据
                    //写入更新记录
                    if (AuditNums != null)
                    {
                        info.AuditNums = AuditNums.Value + 1;
                    }
                    #endregion
                    #region 日志
                    StringBuilder sb = new StringBuilder();
                    var adcdInfo = db.Single<ADCDInfo>(w => w.adcd == info.adcd);
                    sb.Append("在栏目{组织责任/乡(镇、街道)防汛防台责任人}下,新增数据{");
                    sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                    sb.Append("姓名：" + info.Name + ";");
                    sb.Append("创建时间：" + info.CreateTime + ";");
                    sb.Append("手机：" + info.Mobile + ";");
                    sb.Append("职务：" + info.Position + ";");
                    sb.Append("岗位：" + info.Post + ";");
                    sb.Append("年度：" + info.Year + ";");
                    sb.Append("备注：" + info.Remark + ";");
                    sb.Append("}");
                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
                    #endregion
                    return db.Insert(info) == 1;
                }
            }
        }

        /// <summary>
        /// 删除镇级防汛防台责任人
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool NoAuthticationDelTown(string ids)
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
                    var _f = db.Single<TownPersonLiable>(w => w.Id == id);
                    var adcdInfo = db.Single<ADCDInfo>(w => w.adcd == _f.adcd);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("在栏目{组织责任/乡(镇、街道)防汛防台责任人}下,删除了数据{");
                    sb.Append("村ADCD：" + adcdInfo.adnm + ";");
                    sb.Append("姓名：" + _f.Name + ";");
                    sb.Append("创建时间：" + _f.CreateTime + ";");
                    sb.Append("手机：" + _f.Mobile + ";");
                    sb.Append("职务：" + _f.Position + ";");
                    sb.Append("岗位：" + _f.Post + ";");
                    sb.Append("年度：" + _f.Year + ";");
                    sb.Append("备注：" + _f.Remark + ";");
                    sb.Append("}");
                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.删除);
                    #endregion
                }
                return db.Delete<TownPersonLiable>(x => Sql.In(x.Id, arr)) > 0;
            }
        }
       
    }
}
