using ServiceStack;
using System.Web.Mvc;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.ServiceInterface;
using GrassrootsFloodCtrl.ServiceModel.Route;

namespace GrassrootsFloodCtrl.Controllers
{
    [Authorize]
    public class HomeController : ControllerBase
    {
        
        public ActionResult Index()
        {
            ViewData["lng"] = lng;
            ViewData["lat"] = lat;
            ViewData["status"] = AuditStatus;
            ViewBag.UserAdcd = UserSession.adcd;
            
            if (RoleID != (int)GrassrootsFloodCtrlEnums.RoleEnums.乡镇用户)
            {
                return Redirect("/Account/Login");
                // return View("Error", model: "您访问的页面不存在。");
            }
            return View();
        }

        public ActionResult EditPassword()
        {
            ViewData["id"] = UserId;
            return View();
        }
        public ActionResult ModificationPsaaword()
        {
            ViewData["id"] = UserId;
            ViewData["name"] = UserName;
            return View();
        }
        
        public ActionResult AddADCDDisasterInfo(int id=0)
        {
            ViewData["id"] = id;
            return View();
        }
    }
}