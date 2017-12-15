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
    /// <summary>
    /// 县级人员管理
    /// </summary>
    [Authorize]
    public class CountryPersonController : ControllerBase
    {
        // GET: CountryPerson
        public ActionResult Index()
        {
            if (RoleID != (int)GrassrootsFloodCtrlEnums.RoleEnums.县级用户)
            {
                return Redirect("/Account/Login");
                // return View("Error", model: "您访问的页面不存在。");
            }
            ViewData["id"] = adcd;
            return View();
        }

        public ActionResult AddPerson(string id)
        {
            using (var service = HostContext.ResolveService<PostService>())
            {
                var itemList = new List<SelectListItem>();
                service.Get(new GetPostList()
                {
                    PageSize = int.MaxValue,
                    typeId = 5
                }).rows.ForEach(x =>
                {
                    itemList.Add(new SelectListItem() { Text = x.PostName, Value = x.ID.ToString() });

                });
                ViewData["itemList"] = itemList;
                ViewData["id"] = id;
                return View();
            }
        }
    }
}