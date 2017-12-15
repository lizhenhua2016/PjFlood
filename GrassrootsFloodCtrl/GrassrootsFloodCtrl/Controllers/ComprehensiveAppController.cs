using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GrassrootsFloodCtrl.ServiceInterface;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack;

namespace GrassrootsFloodCtrl.Controllers
{
    [Authorize]
    public class ComprehensiveAppController : ControllerBase
    {
        // GET: ComprehensiveApp
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(adcd))
            {
                return View("Error", model: "登陆超时，请重新登陆。");
            }
            ViewData["adcd"] = adcd.Substring(0,4);

            using (var service = HostContext.ResolveService<ZZTXService>())
            {
                var abc = service.GET(new GetADCDInfoBySession() { adcd = adcd });
                foreach (var item in abc)
                {
                    ViewBag.level = item.grade;
                }
            }



            if (!string.IsNullOrEmpty(UserSession.adcd))
            {
                ViewBag.useradcd = UserSession.adcd;
            }
            return View();
        }
        public ActionResult AppIndex(string adcd,string post,string mobile,string name,string lng,string lat,string stime)
        {
            if (string.IsNullOrEmpty(adcd))
            {
                return View("Error", model: "登陆超时，请重新登陆。");
            }
            if(string.IsNullOrEmpty(adcd) || string.IsNullOrEmpty(post) || string.IsNullOrEmpty(mobile))
            {
                throw new Exception("参数异常！");
            }
            ViewData["adcd"] = adcd;
            ViewData["post"] = post;
            ViewData["mobile"] = mobile;
            ViewData["name"] = name;
            ViewData["lat"] = lat;
            ViewData["lng"] = lng;
            ViewData["stime"] = string.IsNullOrEmpty(stime) ? DateTime.Now.ToString("yyyy-MM-dd"):Convert.ToDateTime(stime).ToString("yyyy-MM-dd");
            return View();
        }
    }
}