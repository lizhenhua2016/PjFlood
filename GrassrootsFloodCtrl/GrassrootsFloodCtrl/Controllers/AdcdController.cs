using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.Logic.Adcd;

namespace GrassrootsFloodCtrl.Controllers
{
    [Authorize]
    public class AdcdController : ControllerBase
    {
        public IAdcdLogic adcdLogic { get; set; }
        // GET: Adcd
        public ActionResult Index()
        {
            if (RoleID != (int)GrassrootsFloodCtrlEnums.RoleEnums.乡镇用户)
            {
                return Redirect("/Account/Login");
                //return View("Error", model: "您访问的页面不存在。");
            }
            ViewData["auditnum"] = UserSession.AuditStatus == null ? 0 : UserSession.AuditStatus;
            return View();
        }

        public ActionResult AddAdcd(string id = "")
        {
            ViewData["auditnum"] = UserSession.AuditStatus == null ? 0 : UserSession.AuditStatus;
            ViewData["id"] = id;
            return View();
        }

        public JsonResult GetCityAdcd(string adcd)
        {
            var list = adcdLogic.GetCityAdcdSelectList(adcd);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCountryAdcd(string adcd)
        {
            var list = adcdLogic.GetCountryAdcdSelectList(adcd);
            return Json(list,JsonRequestBehavior.AllowGet);
        }
    }
}