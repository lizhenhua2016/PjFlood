using GrassrootsFloodCtrl.Model.Audit;
using GrassrootsFloodCtrl.ServiceInterface;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GrassrootsFloodCtrl.Model.Enums;

namespace GrassrootsFloodCtrl.Controllers
{
    [Authorize]
    public class AuditController : ControllerBase
    {
        [HttpGet]
        public ActionResult AuditApplication(int? year)
        {
            if (string.IsNullOrEmpty(adcd))
            {
                return View("Error", model: "登陆到期,请重新登陆！");
            }
            ViewData["year"] = year;
            ViewData["adcd"] = adcd;
            return View();
        }
        /// <summary>
        /// 县级
        /// </summary>
        /// <returns></returns>
        public ActionResult CountyIndex()
        {
            if(RoleID != (int)GrassrootsFloodCtrlEnums.RoleEnums.县级用户)
            {
                return Redirect("/Account/Login");
                //return View("Error",model:"您访问的页面不存在。");
            }
            ViewData["adcd"] = adcd;
            ViewData["realname"] = UserSession.RealName;
            return View();
        }
        /// <summary>
        /// 市级
        /// </summary>
        /// <returns></returns>
        public ActionResult CityIndex()
        {
            if (RoleID != (int)GrassrootsFloodCtrlEnums.RoleEnums.市级用户)
            {
                return View("Error", model: "您访问的页面不存在。");
            }
            ViewData["adcd"] = adcd;
            ViewData["realname"] = UserSession.RealName;
           //
            return View();
        }
        public ActionResult CityCountyIndex()
        {
            if (RoleID != (int)GrassrootsFloodCtrlEnums.RoleEnums.市级用户)
            {
                return View("Error", model: "您访问的页面不存在。");
            }
            ViewData["adcd"] = adcd;
            ViewData["realname"] = UserSession.RealName;
            return View();
        }
        public ActionResult CountyPerson(string adcd,string name,int? s,int? id,int? nums)
        {
            try
            {
                ViewData["adcd"] = adcd;
                ViewData["name"] = name;
                ViewData["nums"] = nums == null ? 0 : nums.Value;
                ViewData["s"] = s.Value;
                //县审核不通过
                var rlist = HostContext.Resolve<AuditService>().GET(new GetAuditNo { id = id.Value, typeid=1, nums = nums });
                if (null != rlist && rlist.Count > 0)
                {
                    //找到最新的数据
                    var fcity = rlist.Where(w => w.AuditRole == 2).OrderByDescending(w => w.AuditTime);
                    if (fcity.Count() > 0) { ViewData["auditmsg"] = "市级审批不通过意见：" + fcity.FirstNonDefault().Remarks; }
                    else { ViewData["auditmsg"] = "暂无"; }
                }
                else
                {
                    ViewData["auditmsg"] = "暂无";
                }
                //市审核不通过
            }
            catch (Exception ex)
            {
                return View("Error", model: ex.Message);
            }
            return View();
        }
        public ActionResult CountyPersonAll(string id)
        {
            ViewData["adcd"] = id;
            return View();
        }
        public ActionResult TownInfo(string id,int? ids,string name,int? s,int t,int? nums)
        {
            try
            {
                ViewData["adcd"] = id; ViewData["name"] = name;
                ViewData["id"] = ids.Value; ViewData["s"] = s.Value;
                ViewData["rid"] = RoleID; ViewData["nums"] = nums;
                switch (t) {
                    case 3: ViewData["url"] = "/Audit/CountyIndex"; break;
                    case 2: ViewData["url"] = "/Audit/CityIndex"; break;
                    default: ViewData["url"] = "javascript:void(0);"; break;
                }
                //县审核不通过
                var rlist = HostContext.Resolve<AuditService>().GET(new GetAuditNo { id=ids.Value,nums= nums });
                if(null != rlist && rlist.Count > 0)
                {
                    //找到最新的数据
                    var fcity = rlist.Where(w=>w.AuditRole == 2).OrderByDescending(w=>w.AuditTime);
                    var fcounty = rlist.Where(w => w.AuditRole == 3).OrderByDescending(w => w.AuditTime);
                    if (fcity.Count() > 0) { ViewData["auditmsg"] = fcity.FirstNonDefault().Remarks; }
                    else if(fcounty.Count() > 0) { ViewData["auditmsg"] = fcounty.FirstNonDefault().Remarks; }
                    else { ViewData["auditmsg"] = "暂无"; }
                }else
                {
                    ViewData["auditmsg"] = "暂无";
                }
                //市审核不通过
            }
            catch(Exception ex)
            {
                return View("Error",model:ex.Message);
            }
            return View();
        }
        public ActionResult AuditNO(string id,int? t)
        {
            ViewData["id"] = id;
            ViewData["t"] = null == t?0:t;
            return View();
        }
      
    }
}