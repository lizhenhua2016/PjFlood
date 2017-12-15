using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.ServiceModel.Audit;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.Model.Common;
using Dy.Common;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using GrassrootsFloodCtrl.Model.Audit;
using GrassrootsFloodCtrl.Logic.Common;
using GrassrootsFloodCtrl.Model.Town;
using GrassrootsFloodCtrl.Model.Village;

namespace GrassrootsFloodCtrl.Logic.Audit
{
    public class AuditManager : ManagerBase, IAuditManager
    {
        public ILogHelper _ILogHelper { get; set; }
        #region 乡镇提交申请
        /// <summary>
        /// 审核申请提交
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResult AuditApplication(AuditApplication request)
        {
            BaseResult bs = new BaseResult();
            var _year = System.DateTime.Now.Year;
            var addtime = DateTime.Now;
            try
            {
                using (var db = DbFactory.Open())
                {
                    var builder = db.From<Model.Audit.Audit>();
                    if (request.adcd != adcd)
                    {
                        bs.IsSuccess = false; bs.ErrorMsg = "操作账号异常"; return bs;
                    }
                    if (request.year.Value != _year)
                    {
                        bs.IsSuccess = false; bs.ErrorMsg = "年份时间异常,申报年份仅限本年度"; return bs;
                    }
                    #region 更新数据检测
                    var nnum = AuditNums + 1;
                    var townperson = db.Select<TownPersonLiable>(w=>w.Year == request.year && w.adcd.StartsWith(request.adcd.Substring(0,9)) && w.AuditNums == nnum);
                    var workgroup = db.Select<VillageWorkingGroup>(w=>w.Year == request.year && w.VillageADCD.StartsWith(request.adcd.Substring(0, 9)) && w.AuditNums == nnum);
                    var workgrid = db.Select<VillageGridPersonLiable>(w=>w.Year == request.year && w.VillageADCD.StartsWith(request.adcd.Substring(0, 9)) && w.AuditNums == nnum);
                    var worktransfer = db.Select<VillageTransferPerson>(w=>w.Year == request.year && w.adcd.StartsWith(request.adcd.Substring(0, 9)) && w.AuditNums == nnum);
                    var villagepic = db.Select<VillagePic>(w=>w.Year == request.year && w.adcd.StartsWith(request.adcd.Substring(0, 9)) && w.AuditNums == nnum);
                    if (AuditNums != null)
                    {
                        if (townperson.Count == 0 && workgroup.Count == 0 && workgrid.Count == 0 && worktransfer.Count == 0 && villagepic.Count == 0)
                        {
                            bs.IsSuccess = false; bs.ErrorMsg = "系统检测到，您没有更新过任何数据，所以不需提交审核！"; return bs;
                        }
                    }
                    #endregion
                    GrassrootsFloodCtrl.Model.Audit.Audit _audit = new Model.Audit.Audit();
                    _audit.TownADCD = request.adcd;
                    _audit.Year = request.year;
                    _audit.TownAddTime = addtime;
                    _audit.Status = int.Parse(PublicClass.GetAudit(null, AuditStatusEnums.待审批.ToString()));
                    //
                    var log = new operateLog();
                    log.userName = RealName;
                    log.operateTime = addtime;
                    if (null == request.id) log.operateMsg = RealName + "提交了审核申请";
                    else log.operateMsg = RealName + "修改后提交审核申请";
                    var listLog = new List<operateLog>();
                    listLog.Add(log);
                    _audit.operateLog = JsonTools.ObjectToJson(listLog);
                    //
                    if (null != request.id && request.id.Value > 0)
                    {
                        _audit.ID = request.id;
                        bs.IsSuccess = db.Update(_audit) == 1 ? true : false;
                        #region 日志
                        StringBuilder sb = new StringBuilder();
                        sb.Append("在栏目{组织责任/提交审核}下," + RealName + "提交了审批第"+(AuditNums + 1)+"次申请");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
                        #endregion
                    }
                    else
                    {
                        //写入前判断,在当前一年是否已经存在
                        var b = db.From<Model.Audit.Audit>();
                        b.Where(w => w.TownADCD == request.adcd && w.Year == _year);
                        b.OrderByDescending(w => w.TownAddTime);
                        var r = db.Single<AuditViewModel>(b);
                        if (null != r)
                        {
                            _audit.AuditNums = r.AuditNums + 1;
                            _audit.ID = r.ID;
                            bs.IsSuccess = db.Update(_audit) == 1 ? true : false;
                            #region 日志
                            StringBuilder sb = new StringBuilder();
                            sb.Append("在栏目{组织责任/提交审核}下,{" + RealName + "}第{" + _audit.AuditNums + "}次提交了审批申请");
                            _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
                            #endregion
                        }
                        else
                        {
                            _audit.AuditNums = 1;
                            bs.IsSuccess = db.Insert(_audit) == 1 ? true : false;
                            #region 日志
                            StringBuilder sb = new StringBuilder();
                            sb.Append("在栏目{组织责任/提交审核}下,{" + RealName + "}第{" + _audit.AuditNums + "}次提交了审批申请");
                            _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.新增);
                            #endregion
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                bs.IsSuccess = false;
                bs.ErrorMsg = ex.Message;
            }
            return bs;
        }
        /// <summary>
        /// 获取审核申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AuditViewModel GetAuditResult(GetAuditResult request)
        {
            var _year = System.DateTime.Now.Year;
            AuditViewModel _auditview = new AuditViewModel();
            if (request.adcd != adcd) throw new Exception("操作账号异常");
            if (request.year.Value != _year) throw new Exception("年份时间异常,申报年份仅限本年度");
            using (var db = DbFactory.Open())
            {
                var builder = db.From<Model.Audit.Audit>();
                builder.LeftJoin<Model.Audit.Audit, AuditDetails>((x, y) => x.ID == y.AuditID);
                builder.Where(x => x.Year == request.year && x.TownADCD == request.adcd);
                builder.OrderByDescending<AuditDetails>(w => w.AuditTime);
                _auditview = db.Single<AuditViewModel>(builder);
                if (_auditview != null) _auditview.statusname = PublicClass.GetAudit(_auditview.Status, "");
            }
            return _auditview;
        }
        #endregion
        #region 县市处理
        /// <summary>
        /// 市县级获取审核申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<AuditViewModel> GetAuditApplication(GetAuditApplication request)
        {
            var _year = System.DateTime.Now.Year;
            try
            {
                using (var db = DbFactory.Open())
                {
                    #region county
                    var _adcd = string.IsNullOrEmpty(request.adcd) ? adcd : request.adcd;
                    if (request.level.Value == 3)
                    {
                        List<AuditViewModel> RList1 = null;
                        List<AuditViewModel> RList0 = null;
                        //统计
                        //待审批
                        var lable1 = getCount(1, 6, _adcd);
                        //已审未批
                        var lable2 = getCount(2, 6, _adcd);
                        //已批
                        var lable3 = getCount(3, 6, _adcd);
                        //1
                        var builder = db.From<Model.Audit.Audit>();
                        builder.LeftJoin<Model.Audit.Audit, ADCDInfo>((x, y) => x.TownADCD == y.adcd);
                        builder.Where(x => x.TownADCD.StartsWith(_adcd.Substring(0, 6)));
                        builder.Select(" ADCDInfo.adnm,Audit.*");
                        RList1 = db.Select<AuditViewModel>(builder);
                        //0
                        var sql = "";
                        if (RList1.Count == 0)
                        {
                            sql = "SELECT ADCDInfo.adcd,ADCDInfo.adnm FROM ADCDInfo WHERE SUBSTRING(ADCDInfo.adcd, 10, 6) = '000000' AND ADCDInfo.adcd != '" + _adcd + "' AND ADCDInfo.adcd like '%" + _adcd.Substring(0, 6) + "%'";
                        }
                        else
                        {
                            //sql = "SELECT distinct(ADCDInfo.adcd),ADCDInfo.adnm FROM Audit,ADCDInfo " +
                            //       "WHERE ADCDInfo.adcd != Audit.TownADCD AND SUBSTRING(ADCDInfo.adcd, 10, 6) = '000000' AND Audit.Year = " + _year + " AND ADCDInfo.adcd != '" + _adcd + "' AND ADCDInfo.adcd like '%" + _adcd.Substring(0, 6) + "%'";
                            sql = "select a.adcd,a.adnm from (SELECT ADCDInfo.adcd, ADCDInfo.adnm FROM ADCDInfo WHERE SUBSTRING(ADCDInfo.adcd, 10, 6) = '000000'" +
                              "AND ADCDInfo.adcd != '" + _adcd + "' AND ADCDInfo.adcd like '%" + _adcd.Substring(0, 6) + "%') as a where a.adcd not in(select TownADCD as adcd from Audit where Audit.Year = " + _year + " and Audit.TownADCD like '" + _adcd.Substring(0, 6) + "%')";
                        }

                        RList0 = db.Select<AuditViewModel>(sql);
                        RList1.AddRange(RList0);
                        return new BsTableDataSource<AuditViewModel>() { rows = RList1, total = RList1.Count + RList0.Count, other = RList0.Count.ToString() + "|" + lable1 + "|" + lable2 + "|" + lable3 };
                    }
                    #endregion
                    #region city
                    if (request.level.Value == 2)
                    {
                        List<AuditViewModel> RList1 = null;
                        List<AuditViewModel> RList0 = null;
                        //统计
                        //var leng = string.IsNullOrEmpty(request.adcd)
                        //待审批
                        var lable1 = getCount(1, 4, _adcd);
                        //已审未批
                        var lable2 = getCount(2, 4, _adcd);
                        //已批
                        var lable3 = getCount(3, 4, _adcd);
                        //1
                        var builder = db.From<Model.Audit.Audit>();
                        builder.LeftJoin<Model.Audit.Audit, ADCDInfo>((x, y) => x.TownADCD == y.adcd);
                        builder.Where(x => x.TownADCD.StartsWith(_adcd.Substring(0, 4)));
                        builder.Select(" ADCDInfo.adnm,Audit.*");
                        RList1 = db.Select<AuditViewModel>(builder);
                        //0
                        var sql = "";
                        if (RList1.Count == 0)
                        {
                            sql = "SELECT ADCDInfo.adcd,ADCDInfo.adnm FROM ADCDInfo WHERE SUBSTRING(ADCDInfo.adcd, 10, 6) = '000000' AND ADCDInfo.adcd not like '%000000000' AND SUBSTRING(ADCDInfo.adcd, 7, 9) != '000000000'  AND ADCDInfo.adcd != '" + _adcd + "' AND ADCDInfo.adcd like '%" + _adcd.Substring(0, 4) + "%'";
                        }
                        else
                        {
                            // sql = "SELECT distinct(ADCDInfo.adcd),ADCDInfo.adnm FROM Audit,ADCDInfo " +
                            //          "WHERE ADCDInfo.adcd != Audit.TownADCD AND SUBSTRING(ADCDInfo.adcd, 10, 6) = '000000' AND Audit.Year = " + _year + " AND ADCDInfo.adcd != '" + _adcd + "' AND ADCDInfo.adcd like '%" + _adcd.Substring(0, 4) + "%'";
                            sql = "select a.adcd,a.adnm from (SELECT ADCDInfo.adcd, ADCDInfo.adnm FROM ADCDInfo WHERE SUBSTRING(ADCDInfo.adcd, 10, 6) = '000000' AND ADCDInfo.adcd not like '%000000000'" +
                             "AND ADCDInfo.adcd != '" + _adcd + "' AND ADCDInfo.adcd like '%" + _adcd.Substring(0, 4) + "%') as a where a.adcd not in(select TownADCD as adcd from Audit where Audit.Year = " + _year + " and Audit.TownADCD like '" + _adcd.Substring(0, 4) + "%')";
                        }
                        RList0 = db.Select<AuditViewModel>(sql);
                        RList1.AddRange(RList0);
                        return new BsTableDataSource<AuditViewModel>() { rows = RList1, total = RList1.Count + RList0.Count, other = RList0.Count.ToString() + "|" + lable1 + "|" + lable2 + "|" + lable3 };
                    }
                    #endregion
                    throw new Exception("权限异常");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public BsTableDataSource<AuditCountyViewModel> GetAuditApplicationCounty(GetAuditApplicationCounty request)
        {
            var _year = System.DateTime.Now.Year;
            try
            {
                using (var db = DbFactory.Open())
                {
                    var _adcd = string.IsNullOrEmpty(request.adcd) ? adcd : request.adcd;
                    List<AuditCountyViewModel> RList1 = null;
                    List<AuditCountyViewModel> RList0 = null;
                    //统计
                    //var leng = string.IsNullOrEmpty(request.adcd)
                    //待审批
                    var lable1 = getCountCounty(1, 4, _adcd);
                    //已审批
                    var lable2 = getCountCounty(3, 4, _adcd);
                    //1
                    var builder = db.From<AuditCounty>();
                    builder.LeftJoin<AuditCounty, ADCDInfo>((x, y) => x.CountyADCD == y.adcd);
                    builder.Where(x => x.CountyADCD.StartsWith(_adcd.Substring(0, 4)));
                    builder.Select(" ADCDInfo.adnm,AuditCounty.*");
                    RList1 = db.Select<AuditCountyViewModel>(builder);
                    //0
                    var sql = "";
                    if (RList1.Count == 0)
                    {
                        sql = "SELECT ADCDInfo.adcd,ADCDInfo.adnm FROM ADCDInfo WHERE SUBSTRING(ADCDInfo.adcd, 7, 9) = '000000000' AND ADCDInfo.adcd != '" + _adcd + "' AND ADCDInfo.adcd like '%" + _adcd.Substring(0, 4) + "%'";
                    }
                    else
                    {
                        //sql = "SELECT distinct(ADCDInfo.adcd),ADCDInfo.adnm FROM AuditCounty,ADCDInfo " +
                        //          "WHERE ADCDInfo.adcd != AuditCounty.CountyADCD AND SUBSTRING(ADCDInfo.adcd, 7, 9) = '000000000' AND AuditCounty.Year = " + _year + " AND ADCDInfo.adcd != '" + _adcd + "' AND ADCDInfo.adcd like '%" + _adcd.Substring(0, 4) + "%'";
                        sql = "select a.adcd,a.adnm from (SELECT ADCDInfo.adcd, ADCDInfo.adnm FROM ADCDInfo WHERE SUBSTRING(ADCDInfo.adcd, 7, 9) = '000000000'" +
                              "AND ADCDInfo.adcd != '" + _adcd + "' AND ADCDInfo.adcd like '%" + _adcd.Substring(0, 4) + "%') as a where a.adcd not in(select CountyADCD as adcd from AuditCounty where AuditCounty.Year = " + _year + " and AuditCounty.CountyADCD like '" + _adcd.Substring(0, 4) + "%')";
                    }
                    RList0 = db.Select<AuditCountyViewModel>(sql);
                    RList1.AddRange(RList0);
                    return new BsTableDataSource<AuditCountyViewModel>() { rows = RList1, total = RList1.Count + RList0.Count, other = RList0.Count.ToString() + "|" + lable1 + "|" + lable2 };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 统计数量公共方法
        /// </summary>
        /// <param name="_status"></param>
        /// <returns></returns>
        public int getCount(int _status, int len, string _adcd)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<Model.Audit.Audit>().Where(w => w.Status == _status && w.TownADCD.StartsWith(_adcd.Substring(0, len)));
                var r = db.Select<AuditViewModel>(builder);
                return r.Count;
            }
        }
        public int getCountCounty(int _status, int len, string _adcd)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<AuditCounty>().Where(w => w.Status == _status && w.CountyADCD.StartsWith(_adcd.Substring(0, len)));
                var r = db.Select<AuditCountyViewModel>(builder);
                return r.Count;
            }
        }
    
        #endregion
        #region 镇级详情
        public List<ADCDDisasterViewModel> GetTownVillage(GetTownVillage request)
        {
            if (string.IsNullOrEmpty(request.adcd)) throw new Exception("adcd不能为空");
            using (var db = DbFactory.Open())
            {
                //var builder = db.From<ADCDInfo>();
                var sql = "";
                if (string.IsNullOrEmpty(request.adnm))
                    // builder.Join<ADCDInfo, ADCDDisasterInfo>((x,y)=> x.adcd.Contains(request.adcd.Substring(0, 9)) && x.adcd != request.adcd && x.adcd == y.adcd);
                    sql = "select ADCDInfo.id,ADCDInfo.adcd,ADCDInfo.adnm, ADCDDisasterInfo.TotalNum,c.householderNum as PopulationNum,ADCDDisasterInfo.FXFTRW "
                        + "from ADCDInfo left join ADCDDisasterInfo on ADCDInfo.adcd = ADCDDisasterInfo.adcd left join (select sum(householderNum) as householderNum, adcd  from VillageTransferPerson group by adcd) as C on ADCDInfo.adcd = c.adcd "
                        + "where ADCDInfo.adcd LIKE '%" + request.adcd.Substring(0, 9) + "%' AND ADCDInfo.adcd != '" + request.adcd + "' ";

                if (!string.IsNullOrEmpty(request.adnm))
                    //sql = "select ADCDInfo.id,ADCDInfo.adcd,ADCDInfo.adnm, ADCDDisasterInfo.TotalNum,c.householderNum as PopulationNum,ADCDDisasterInfo.FXFTRW "
                    //    + "from ADCDInfo, ADCDDisasterInfo,(select sum(householderNum) as householderNum, adcd  from VillageTransferPerson group by adcd) as C "
                    //    + "where ADCDInfo.adcd LIKE '%" + request.adcd.Substring(0, 9) + "%' AND ADCDInfo.adnm like '%" + request.adnm + "%' AND ADCDInfo.adcd != '" + request.adcd + "' AND ADCDInfo.adcd = ADCDDisasterInfo.adcd AND ADCDInfo.adcd = c.adcd";

                sql = "select ADCDInfo.id,ADCDInfo.adcd,ADCDInfo.adnm, ADCDDisasterInfo.TotalNum,c.householderNum as PopulationNum,ADCDDisasterInfo.FXFTRW "
                      + " from ADCDInfo left join ADCDDisasterInfo on ADCDInfo.adcd = ADCDDisasterInfo.adcd left join (select sum(householderNum) as householderNum, adcd  from VillageTransferPerson group by adcd) as C on ADCDInfo.adcd = c.adcd"
                      + " where ADCDInfo.adcd LIKE '%" + request.adcd.Substring(0, 9) + "%' AND ADCDInfo.adnm like '%" + request.adnm + "%' AND ADCDInfo.adcd != '" + request.adcd + "' ";

                var list = db.Select<ADCDDisasterViewModel>(sql);
                return list;
            }
        }
        #endregion
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResult PostAudit(PostAudit request)
        {
            BaseResult br = new BaseResult();
            if (string.IsNullOrEmpty(request.ids)) { br.IsSuccess = false; br.ErrorMsg = "权限异常"; return br; }

            using (var db = DbFactory.Open())
            {
                var addtimd = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                if (request.t.Value == 0)
                {
                    //县级
                    if (RowID == 3)
                    {
                        var s = int.Parse(PublicClass.GetAudit(null, AuditStatusEnums.县已审市未批.ToString()));
                        br.IsSuccess = db.ExecuteNonQuery("UPDATE Audit SET Status=" + s + ",CountyAuditTime='"+addtimd+"' WHERE ID IN(" + request.ids + ") ") > 0 ? true : false;
                        #region 日志
                        StringBuilder sb = new StringBuilder();
                        var adcdInfo = db.Single<ADCDInfo>(w => w.adcd == request.adcd);
                        sb.Append("在栏目{县级审核}下,{" + RealName + "}处理了镇{" + adcdInfo.adnm + "}的审批申请,处理结果:县已审市未批");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
                        #endregion
                    }
                    //市级
                    if (RowID == 2)
                    {
                        var s = int.Parse(PublicClass.GetAudit(null, AuditStatusEnums.县已审市已批.ToString()));
                        br.IsSuccess = db.ExecuteNonQuery("UPDATE Audit SET Status=" + s + ",CityAuditTime='"+addtimd+"' WHERE ID IN(" + request.ids + ") ") > 0 ? true : false;
                        #region 日志
                        StringBuilder sb = new StringBuilder();
                        var adcdInfo = db.Single<ADCDInfo>(w => w.adcd == request.adcd);
                        sb.Append("在栏目{市级审核}下,{" + RealName + "}处理了镇{" + adcdInfo.adnm + "}的审批申请,处理结果:县已审市已批");
                        _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
                        #endregion
                    }
                }
                else
                {
                    //县级责任人审核
                    var s = int.Parse(PublicClass.GetAudit(null, AuditStatusEnums.县已审市已批.ToString()));
                    br.IsSuccess = db.ExecuteNonQuery("UPDATE AuditCounty SET Status=" + s + ",CityAuditTime='" + addtimd + "' WHERE ID IN(" + request.ids + ") ") > 0 ? true : false;
                    #region 日志
                    StringBuilder sb = new StringBuilder();
                    var adcdInfo = db.Single<ADCDInfo>(w => w.adcd == request.adcd);
                    sb.Append("在栏目{市级审核}下,{" + RealName + "}处理了县{" + adcdInfo.adnm + "}的责任人审批申请,处理结果:县已审市已批");
                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
                    #endregion
                }
            }
            return br;
        }

        #region 审批不通过
        public BaseResult AuditNo(AuditNo request)
        {
            BaseResult br = new BaseResult();
            if (null == request.id) throw new Exception("参数异常");
            try
            {
                using (var db = DbFactory.Open())
                {
                    var addtimd = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if(request.t.Value == 0)
                    {
                        var idd = request.id.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var id in idd)
                        {
                            var _id = int.Parse(id);
                            AuditDetails _AuditDetails = new AuditDetails();
                            _AuditDetails.AuditID = _id;
                            _AuditDetails.AuditADCD = adcd;
                            _AuditDetails.AuditRole = RowID;
                            _AuditDetails.AuditTime = addtimd;
                            _AuditDetails.Remarks = request.remarks;
                            var getanum = db.Single<Model.Audit.Audit>(w => w.ID == _id);
                            if (getanum != null)
                            {
                                _AuditDetails.AuditNums = getanum.AuditNums;
                            }
                            // _AuditDetails.AuditNums = 
                            var r = db.Insert<AuditDetails>(_AuditDetails) == 1 ? true : false;
                            if (r)
                            {
                                if (RowID == 3)
                                {
                                    var s = PublicClass.GetAudit(null, AuditStatusEnums.县审批不通过.ToString());
                                    br.IsSuccess = db.ExecuteNonQuery("UPDATE Audit SET Status=" + s + " WHERE ID =" + _id + " ") > 0 ? true : false;
                                    #region 日志
                                    var f = db.Single<Model.Audit.Audit>(w=>w.ID == _id);
                                    StringBuilder sb = new StringBuilder();
									var adcdInfo = db.Single<ADCDInfo>(w=>w.adcd==f.TownADCD);
                                    sb.Append("在栏目{县级审核}下,{" + RealName + "}处理了乡镇{"+adcdInfo.adnm+ "}的审批申请,处理结果：县审批不通过");
                                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
                                    #endregion
                                }
                                if (RowID == 2)
                                {
                                    var s = PublicClass.GetAudit(null, AuditStatusEnums.市审批不通过.ToString());
                                    br.IsSuccess = db.ExecuteNonQuery("UPDATE Audit SET Status=" + s + " WHERE ID =" + _id + " ") > 0 ? true : false;
                                    #region 日志
                                    var f = db.Single<Model.Audit.Audit>(w => w.ID == _id);
                                    var adcdInfo = db.Single<ADCDInfo>(w => w.adcd == f.TownADCD);
                                    StringBuilder sb = new StringBuilder();
                                    sb.Append("在栏目{市级审核}下,{" + RealName + "}处理了乡镇{" + adcdInfo.adnm + "}的审批申请,处理结果：市审批不通过");
                                    _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
                                    #endregion
                                }
                            }
                        }
                    }
                    if (request.t.Value == 1)
                    {
                        var idd = request.id.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var id in idd)
                        {
                            var _id = int.Parse(id);
                            AuditCountyDetails _AuditCountyDetails = new AuditCountyDetails();
                            _AuditCountyDetails.CountyID = _id;
                            _AuditCountyDetails.AuditADCD = adcd;
                            _AuditCountyDetails.AuditRole = RowID;
                            _AuditCountyDetails.AuditTime = addtimd;
                            _AuditCountyDetails.Remarks = request.remarks;
                            var getanum = db.Single<AuditCounty>(w => w.ID == _id);
                            if (getanum != null)
                            {
                                _AuditCountyDetails.AuditNums = getanum.AuditNums;
                            }
                            var r = db.Insert<AuditCountyDetails>(_AuditCountyDetails) == 1 ? true : false;
                            if (r)
                            {
                                var s = PublicClass.GetAudit(null, AuditStatusEnums.市审批不通过.ToString());
                                br.IsSuccess = db.ExecuteNonQuery("UPDATE AuditCounty SET Status=" + s + " WHERE ID =" + _id + " ") > 0 ? true : false;
                                #region 日志
                                var f = db.Single<AuditCounty>(w => w.ID == _id);
                                StringBuilder sb = new StringBuilder();
								var adcdInfo = db.Single<ADCDInfo>(w=>w.adcd == f.CountyADCD);
                                sb.Append("在栏目{市级审核}下,{" + RealName + "}处理了乡镇{" + adcdInfo.adnm + "}的责任人审批申请,处理结果：市审批不通过");
                                _ILogHelper.WriteLog(sb.ToString(), OperationTypeEnums.更新);
                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                br.IsSuccess = false; br.ErrorMsg = ex.Message;
            }
            return br;
        }
        public List<AuditViewModel> GetAuditNo(GetAuditNo request)
        {
            if (null == request.id) throw new Exception("参数异常");
            var _year = System.DateTime.Now.Year;
            using (var db = DbFactory.Open())
            {
                if(request.typeid == null)
                {
                    var builder = db.From<Model.Audit.Audit>();
                    builder.LeftJoin<Model.Audit.Audit, AuditDetails>((x, y) => x.ID == y.AuditID && y.AuditNums == request.nums.Value);
                    builder.Where(x => x.Year == _year && x.ID == request.id);
                    builder.OrderByDescending(w => w.TownAddTime);
                    var rlist = db.Select<AuditViewModel>(builder);
                    return rlist;
                }
                else
                {
                    var builder = db.From<AuditCounty>();
                    builder.LeftJoin<AuditCounty, AuditCountyDetails>((x, y) => x.ID == y.CountyID && y.AuditNums == request.nums.Value);
                    builder.Where(x => x.Year == _year && x.ID == request.id);
                    builder.OrderByDescending(w => w.CountyAddTime);
                    var rlist = db.Select<AuditViewModel>(builder);
                    return rlist;

                }
            }
        }
        #endregion
        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<ADCDInfo> GetAreaList(GetAreaList request)
        {
            if (string.IsNullOrEmpty(request.adcd) || null == request.tid) throw new Exception("参数不能为空！");
            try
            {
                using (var db = DbFactory.Open())
                {
                    List<ADCDInfo> list = new List<ADCDInfo>();
                    switch (request.tid)
                    {
                        case 3:
                            list = db.Select<ADCDInfo>(w => w.adcd.StartsWith(request.adcd.Substring(0, 4)) && w.adcd.EndsWith(request.adcd.Substring(6, 9)) && w.adcd != request.adcd);
                            break;
                        case 4:
                            list = db.Select<ADCDInfo>(w => w.adcd.StartsWith(request.adcd.Substring(0, 6)) && w.adcd.EndsWith(request.adcd.Substring(10, 6)) && w.adcd != request.adcd);
                            break;
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public AuditOtherViewModel GetAuditNumByADCD(GetAuditNumByADCD request)
        {
            try
            {
                using (var db = DbFactory.Open())
                {
                    AuditOtherViewModel r = new AuditOtherViewModel();
                    if (RowID == 4)
                    {
                        var builder = db.From<Model.Audit.Audit>();
                        builder.Where(w => w.TownADCD == adcd).OrderByDescending(w => w.TownAddTime);
                        r= db.Single<AuditOtherViewModel>(builder);
                    }
                    else if(RowID == 3)
                    {
                        var builder = db.From<AuditCounty>();
                        builder.Where(w => w.CountyADCD == adcd).OrderByDescending(w => w.CountyAddTime);
                       r= db.Single<AuditOtherViewModel>(builder);
                    }else
                    {

                    }
                    return r;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
