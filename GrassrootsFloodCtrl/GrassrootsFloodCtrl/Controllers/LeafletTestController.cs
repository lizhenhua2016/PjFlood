using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrassrootsFloodCtrl.Controllers
{
    public class LeafletTestController : ControllerBase
    {
        // GET: LeafletTest
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Emergencytest()
        {
            ViewBag.roleid = RoleID;
            ViewBag.username = UserName;
            ViewBag.adcd = adcd;
            return View();
        }

        public ActionResult SelfMaptest()
        {
            ViewBag.roleid = RoleID;
            ViewBag.username = UserName;
            ViewBag.adcd = adcd;
            return View();
        }
    }
}