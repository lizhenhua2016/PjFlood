using GrassrootsFloodCtrl.ServiceInterface;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;
namespace GrassrootsFloodCtrl.Controllers
{
    [Authorize]
    public class OrgController : ControllerBase
    {
        // GET: Org
        public ActionResult UserList()
        {
            using (var service= HostContext.ResolveService<ZZTXService>())
            {
                var itemList = new List<SelectListItem>();
                service.GET(new GetADCDInfo()
                {
                    levle = 2,
                    PageSize = int.MaxValue
                }).rows.ForEach(x =>
                {
                    itemList.Add(new SelectListItem() { Text = x.adnm, Value = x.adcd });

                });

                ViewData["itemList"] = itemList;
                return View();
            }
        }

        public ActionResult OrgResponsibilities()
        {
            return View();
        }

        public ActionResult AddUser(string id="")
        {
            using (var service = HostContext.ResolveService<ZZTXService>())
            {
                var itemList = new List<SelectListItem>();
                service.GET(new GetADCDInfo()
                {
                    levle = 2,PageSize = int.MaxValue
                }).rows.ForEach(x =>
                {
                    itemList.Add(new SelectListItem() {Text = x.adnm, Value = x.adcd});
                });
                var roleList = new List<SelectListItem>();
                var sysService= HostContext.ResolveService<SysService>();
               sysService.GetRolesList(new GetRolesList() {PageSize = int.MaxValue}).rows.ForEach(x =>
               {
                   roleList.Add(new SelectListItem() { Text = x.RoleName, Value = x.Id.ToString() });
               });
                ViewData["itemList"] = itemList;
                ViewData["roleList"] = roleList;
                ViewData["id"] = id;
                return View();
            }
        }

        #region 栏目管理
        [HttpGet]
        public ActionResult ColumnManage()
        {
            ViewData["username"] = UserName;
            ViewData["nowyear"]= DateTime.Now.Year;
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult ColumnAdd(int? id)
        {
            ViewData["id"] = null != id ? id.Value : 0;
            ViewData["username"] = UserName;
            var columnList = HostContext.Resolve<OrgService>().Get(new GetColumnList() { PageSize = 999, username = UserName, id = 0 });
            if (columnList != null && columnList.rows.Count > 0)
            {
                var items = new List<SelectListItem>();
                columnList.rows.ForEach(x =>
                {
                    var value = x.ColumnID.ToString()+"|"+  x.Level;
                    items.Add(new SelectListItem() { Text = x.ColumnName, Value = value  });
                });
                ViewData["columnList"] = items;
            }
            List<SelectListItem> actionList = new List<SelectListItem>();
            string[] Names = Enum.GetNames(typeof(Actions));
            foreach (string Name in Names)
            {
                actionList.Add(new SelectListItem { Text = Name, Value = ((Actions)Enum.Parse(typeof(Actions), Name)).ToString("d") });
            }
            ViewData["actionsList"] = actionList;
            return View();
        } 
        public JsonResult ShowActions(string actions)
        {
            string r = "";
            string[] Actions = actions.Split(',');
            string[] Names = Enum.GetNames(typeof(Actions));
            foreach (string Name in Names)
            {
                var value = ((Actions)Enum.Parse(typeof(Actions), Name)).ToString("d");
                foreach (string aName in Actions)
                {
                    if(aName == value)
                    {
                        r += Name + ",";
                    }
                }
            }
            var result = new
            {
                msg = r
            };
            return new JsonResult() { Data= result, JsonRequestBehavior=JsonRequestBehavior.AllowGet };
        }
        #endregion

        #region 角色
        [HttpGet]
        public ActionResult Role()
        {
            ViewData["username"] = UserName;
            return View();
        }
        [HttpGet]
        public ActionResult RoleAdd(int? rid)
        {
            ViewData["username"] = UserName;
            StringBuilder sb = new StringBuilder(); 
            var columnList = HostContext.Resolve<OrgService>().Get(new GetColumnList() { PageSize = 999, username = UserName, id = 0 });
            if (null != rid && rid > 0)
            {
                //编辑
            }else
            {
                //加载
                if (columnList != null && columnList.rows.Count > 0)
                {
                    var actionList = new List<int>();
                    columnList.rows.ForEach(x =>
                    {
                        sb.Append("<fieldset class=\"fieldset\">");
                        sb.Append("<legend class=\"legend\">"+x.ColumnName+"</legend>");
                        sb.Append("<label class=\"checkbox-inline\">");
                        sb.Append("<input type=\"checkbox\" name=\"actionsAll\" class=\"checkboxSize\" />");
                        sb.Append("</label>");

                        sb.Append("</fieldset>");
                    });
                }
            }
            return View();
        }
        
        #endregion
    }
}