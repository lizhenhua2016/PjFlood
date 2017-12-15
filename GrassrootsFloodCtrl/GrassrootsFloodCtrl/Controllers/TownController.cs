using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GrassrootsFloodCtrl.ServiceInterface;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.Enums;

namespace GrassrootsFloodCtrl.Controllers
{
    [Authorize]
    public class TownController : ControllerBase
    {
        // GET: Town
        public ActionResult Index()
        {
            if (RoleID != (int)GrassrootsFloodCtrlEnums.RoleEnums.乡镇用户)
            {
                return Redirect("/Account/Login");
                //return View("Error", model: "您访问的页面不存在。");
            }
            ViewData["realname"] = UserSession.RealName;
            ViewData["auditnum"] = UserSession.AuditStatus == null ? 0 :UserSession.AuditStatus;
            return View();
        }

        public ActionResult AddTown(string id="")
        {
            using (var service=HostContext.ResolveService<PostService>())
            {
                var itemList = new List<SelectListItem>(); 
               service.Get(new GetPostList()
                { 
                    PageSize = int.MaxValue,typeId = 4
                }).rows.ForEach(x =>
                {
                    itemList.Add(new SelectListItem() {Text = x.PostName,Value = x.ID.ToString()});

                });
                ViewData["itemList"] = itemList;
                ViewData["id"] = id;
                return View();
            }
        }
    }
}