using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GrassrootsFloodCtrl.Model.Enums;
using ServiceStack;
using GrassrootsFloodCtrl.ServiceInterface;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Village;
using System.Configuration;
using GrassrootsFloodCtrl.Logic.Common;
using System.IO;
using System.Web;

namespace GrassrootsFloodCtrl.Controllers
{
    [Authorize]
    public class VillageController : ControllerBase
    {
        // GET: Village
        public ActionResult Index()
        {
            if (RoleID != (int)GrassrootsFloodCtrlEnums.RoleEnums.乡镇用户)
            {
                return Redirect("/Account/Login");
                // return View("Error", model: "您访问的页面不存在。");
            }
            ViewData["useradcd"] = adcd;
            ViewData["username"] = UserName;
            ViewData["realname"] = UserSession.RealName;
            ViewData["nowyear"] = DateTime.Now.Year;
            ViewData["auditnum"] = UserSession.AuditStatus == null ? getAuditStatus() : UserSession.AuditStatus;
            //var userAuth = HostContext.Resolve<VillageService>().GET(new GetList() { adcd = adcd, year = 2017, status = 1 });
            ////已提交
            //var rlist1 = userAuth.rows.Where(w => w.VillageADCD != null).ToList();
            ////未提交
            //var rlist0 = userAuth.rows.Where(w => w.VillageADCD == null).ToList();
            //return View(rlist1);
            return View();
        }
        public ActionResult VillageGridPersonLiabel()
        {
            if (RoleID != (int)GrassrootsFloodCtrlEnums.RoleEnums.乡镇用户)
            {
                return Redirect("/Account/Login");
                //return View("Error", model: "您访问的页面不存在。");
            }
            ViewData["useradcd"] = adcd;
            ViewData["realname"] = UserSession.RealName;
            ViewData["nowyear"] = DateTime.Now.Year;
            ViewData["auditnum"] = UserSession.AuditStatus == null ? getAuditStatus() :UserSession.AuditStatus;
            return View();
        }
        // GET: Village/Details/5
        public ActionResult Details(string id)
        {
            ViewData["useradcd"] = id;
            //ViewData["adnm"] = adnm;
            ViewData["auditnum"] = UserSession.AuditStatus == null ? getAuditStatus() : UserSession.AuditStatus;
            var postlist = HostContext.Resolve<PostService>().Get(new GetPostList() { PageSize=100, typeId = 1 });
            var rlist = postlist.rows.ToList();
            return View(rlist);
            // return View();
        }
        public ActionResult VillageGridDetail(string id)
        {
            ViewData["useradcd"] = id;
            ViewData["auditnum"] = UserSession.AuditStatus == null ? getAuditStatus() : UserSession.AuditStatus;
            var postlist = HostContext.Resolve<GridService>().Get(new GetGridList() { PageSize = 100 });
            var rlist = postlist.rows.ToList();
            return View(rlist);
        }
        public ActionResult AddPersonLiable(int? id)
        {
            ViewData["useradcd"] = adcd;
            ViewData["id"] = id == null ? 0 : id;
            using (var service = HostContext.ResolveService<ZZTXService>())
            {
                var list = service.GET(new GetADCDInfo() { levle = 5, adcd = "", PageSize = int.MaxValue });
                if (list != null && list.rows.Count > 0)
                {
                    var items = new List<SelectListItem>();
                    list.rows.ForEach(x =>
                    {
                        items.Add(new SelectListItem() { Text = x.adnm, Value = x.adcd });
                    });

                    ViewData["adcdList"] = items;
                }
            }
			var postlist = HostContext.Resolve<PostService>().Get(new GetPostList() { PageSize = 100, typeId = 1});
            if (postlist != null && postlist.rows.Count > 0)
            {
                var items = new List<SelectListItem>();
                postlist.rows.ForEach(x =>
                {
                    items.Add(new SelectListItem() { Text = x.PostName, Value = x.PostName });
                });
                ViewData["postList"] = items;
            }
            return View();
        }
        public ActionResult AddGridPersonLiable(int? id)
        {
            ViewData["useradcd"] = adcd;
            ViewData["id"] = id == null ? 0 : id;
            using (var service = HostContext.ResolveService<ZZTXService>())
            {
                var list = service.GET(new GetADCDInfo() { levle = 5, adcd = "", PageSize = int.MaxValue });
                if (list != null && list.rows.Count > 0)
                {
                    var items = new List<SelectListItem>();
                    list.rows.ForEach(x =>
                    {
                        items.Add(new SelectListItem() { Text = x.adnm, Value = x.adcd });
                    });

                    ViewData["adcdList"] = items;
                }
            }
            var postlist = HostContext.Resolve<GridService>().Get(new GetGridList() { PageSize = 100 });
            ViewData["gridList"] = postlist.rows.ToList();
            #region 可注释部分
            if (postlist != null && postlist.rows.Count > 0)
            {
                var items = new List<SelectListItem>();
                postlist.rows.ForEach(x =>
                {
                    items.Add(new SelectListItem() { Text = x.GridName, Value = x.GridName });
                });
                ViewData["gridList"] = items;
            }
            var positionlist = HostContext.Resolve<PositionService>().Get(new GetPositionList() { PageSize = 100, typeId = 2 });
            if (positionlist != null && positionlist.rows.Count > 0)
            {
                var items = new List<SelectListItem>();
                positionlist.rows.ForEach(x =>
                {
                    items.Add(new SelectListItem() { Text = x.PositionName, Value = x.PositionName });
                });
                ViewData["positionList"] = items;
            }
            #endregion
            return View();
        }
        public ActionResult LoadVWGPL(int? id)
        {
            //0村级防汛防台责任人1网格责任人
            //if (RoleID != (int)GrassrootsFloodCtrlEnums.RoleEnums.乡镇用户)
            //{
            //    return Redirect("/Account/Login");
            //    // return View("Error", model: "您访问的页面不存在。");
            //}
            ViewData["typeid"] = null == id ? 0 : 1;
            return View();
        }
        public ActionResult DelFile(string filepath)
        {
            if (!string.IsNullOrEmpty(filepath))
            {
                var file = Server.MapPath("~/" + filepath);
                System.IO.File.Delete(file);
            }
           return View();
        }
        /// <summary>
        /// 行政村危险区转移人员
        /// </summary>
        /// <returns></returns>
        public ActionResult VillageTransferPerson()
        {
            if (RoleID != (int)GrassrootsFloodCtrlEnums.RoleEnums.乡镇用户)
            {
                return Redirect("/Account/Login");
                // return View("Error", model: "您访问的页面不存在。");
            }
            ViewData["auditnum"] = UserSession.AuditStatus == null ? getAuditStatus() : UserSession.AuditStatus;
            return View();
        }

        public ActionResult AddVillageTransferPerson(int id=0,string adcd="")
        {
            using (var service = HostContext.ResolveService<ZZTXService>())
            {
               var list= service.GET(new GetADCDInfo() { levle = 5,adcd = adcd, PageSize = int.MaxValue });
                if (list != null && list.rows.Count > 0)
                {
                    var items = new List<SelectListItem>();
                    list.rows.ForEach(x =>
                    {
                        items.Add(new SelectListItem() { Text =x.adnm,Value = x.adcd});
                    });

                    ViewData["adcdList"] = items;
                }
                var dangerService = HostContext.ResolveService<DangerZoneService>();
                var dangerList = dangerService.Get(new GetDangerZone() {  });
                if (dangerList != null)
                {
                    var items = new List<SelectListItem>();
                    dangerList.ForEach(x =>
                    {
                        items.Add(new SelectListItem() { Text = x.DangerZoneName, Value = x.DangerZoneName });
                    });

                    ViewData["dangerZonelist"] = items;
                }

                ViewData["adcd"] = adcd;
                ViewData["id"] = id;
                return View();
            }
        }

        //public ActionResult VillageTransferPersonList()
        //{
        //    return View();
        //}
        public ActionResult InfoVillageTransferPerson(int id)
        {
            if (RoleID != (int)GrassrootsFloodCtrlEnums.RoleEnums.乡镇用户)
            {
                return Redirect("/Account/Login");
                // return View("Error", model: "您访问的页面不存在。");
            }
            ViewData["auditnum"] = UserSession.AuditStatus == null ? getAuditStatus() : UserSession.AuditStatus;
            using (var service = HostContext.ResolveService<VillageService>())
            {
                var info=new VillageTransferPersonViewModel();
               var list= service.Get(new GetVillageTransferPerson() { id =id }).rows;
                if (list != null && list.Count == 1)
                    info = list[0];
                return View(info);
            }
        }
        /// <summary>
        /// 行政村形势图
        /// </summary>
        /// <returns></returns>

        public ActionResult VillagePic()
        {
            if (RoleID != (int)GrassrootsFloodCtrlEnums.RoleEnums.乡镇用户)
            {
                return Redirect("/Account/Login");
                // return View("Error", model: "您访问的页面不存在。");
            }
            ViewData["auditnum"] = UserSession.AuditStatus == null ? getAuditStatus() : UserSession.AuditStatus;
            ViewData["adcd"] = adcd;
            return View();
        }
        
        public ActionResult InfoVillagePic(string id)
        {
            ViewData["id"] = id;
            return View();
        }

        public Action downLoadPic(string id,string fileName)
        {
            WebClient my = new WebClient();
            my.DownloadFile(id,"D://"+ fileName);   //保存
            return null;
        }
        [HttpGet]
        public ActionResult AddPic(string adcd,string adnm)
        {
            if (string.IsNullOrEmpty(adcd)) throw new Exception("参数异常");
            ViewData["adcd"] = adcd;
            return View();
        }
        [HttpGet]
        public ActionResult getVillageInfo(string a)
        {
            ViewData["adcd"] = a;
            return View();
        }
        [HttpGet]
       // [ValidateInput(true)]
        public ActionResult downQRCode()
        {
            //获取当前请求上下文
            HttpContextBase context = HttpContext;
            //转码成路径
            string str = Server.UrlDecode(Request["url"].ToString());
            string fname = Request["name"].ToString()+str.Substring(str.IndexOf("."),str.Length-str.IndexOf("."));
            //设置报文头，下载而非打开图片
            context.Response.AddHeader("Content-Disposition", string.Format("attachment;filename=\"{0}\"", HttpUtility.UrlEncode(fname)));
            //下载文件，参数2指定格式
            return File(str, "image/jpeg");

            //var  filePath = Server.MapPath("~/Files/"+UserSession.RealName + "/QRPic");
            //FileStream fs = new FileStream(filePath, FileMode.Open);
            //byte[] bytes = new byte[(int)fs.Length];
            //fs.Read(bytes, 0, bytes.Length);
            //fs.Close();
            //Response.Charset = "UTF-8";
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            //Response.ContentType = "application/octet-stream";


            //Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName));
            //Response.BinaryWrite(bytes);
            //Response.Flush();
            //Response.End();
            //return new EmptyResult();

            //return File(new FileStream(filePath, FileMode.Open), "application/octet-stream", Server.UrlEncode(fileName));
        }
        public int? getAuditStatus()
        {
            var useraudtinum = HostContext.Resolve<AuditService>().GET(new GetAuditNumByADCD());
            if (null != useraudtinum)
            {
                //UserSession.AuditNums = useraudtinum.AuditNums;
                 return useraudtinum.Status;
            }
            else
            {
                return 0;
            }
        }
    }
}
