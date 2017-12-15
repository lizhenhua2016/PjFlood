using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrassrootsFloodCtrl.Controllers
{
    [Authorize]
    public class LogController : ControllerBase
    {
        public ActionResult LogCityIndex()
        {
            ViewData["s"] =  DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            ViewData["e"] =DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }
        public ActionResult LogCountyIndex(string id,string s,string e)
        {
            ViewData["s"] = string.IsNullOrEmpty(s) ? DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd") : s;
            ViewData["e"] = string.IsNullOrEmpty(e) ? DateTime.Now.ToString("yyyy-MM-dd") : e;
            ViewData["rid"] = RoleID;
            ViewData["adcd"] = id;
            return View();
        }
        public ActionResult LogTownIndex(string id, string s, string e)
        {
            var newid = string.IsNullOrEmpty(id) && RoleID == 3 ? adcd : id;
            ViewData["s"] = string.IsNullOrEmpty(s)? DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"):s;
            ViewData["e"] = string.IsNullOrEmpty(e)? DateTime.Now.ToString("yyyy-MM-dd") : e;
            ViewData["adcd"] = newid;
            ViewData["adcdparent"] = newid.Substring(0, 4) + "00000000000";
            ViewData["rid"] = RoleID;
            return View();
        }
        public ActionResult LogTownInfo(string id)
        {
            ViewData["adcd"] = id;
            return View();
        }
    }
}