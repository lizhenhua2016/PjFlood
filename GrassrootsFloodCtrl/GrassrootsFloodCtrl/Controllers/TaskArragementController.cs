using GrassrootsFloodCtrl.Logic.AppApi;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.ServiceModel.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrassrootsFloodCtrl.Controllers
{
    public class TaskArragementController : ControllerBase
    {

        public IAppApiManager AppApiManager { get; set; }

        public ActionResult left()
        {
            if (RoleID == null) { return View("Error", model: "登陆已到期，请重新登陆。"); }
            var url = "";
            var urlkh = "";
            var urlkhjl = "";
            var urllog = "";
            var urlapp = "";
            var urltask = "";
            switch (RoleID)
            {
                case 5:
                    url = "/Supervise";
                    urlkh = "/Supervise/CCKH";
                    urlkhjl = "/Supervise/CCHKJL";
                    urllog = "/Log/LogCityIndex";
                    urlapp = "/Supervise/AppInPostCityIndex";
                    break;
                case 2:
                    url = "/Supervise/County";
                    urlkh = "/Supervise/CCKH";
                    urlkhjl = "/Supervise/CCHKJL";
                    urllog = "/Log/LogCountyIndex";
                    urlapp = "/Supervise/AppInPostCountyIndex";
                    break;
                case 3:
                    url = "/Supervise/Towns";
                    urlkh = "";
                    urlkhjl = "";
                    urllog = "/Log/LogTownIndex";
                    urlapp = "/Supervise/AppInPostTownIndex";
                    urltask = "/TaskArragement/sendmessage";
                    break;
            }
            ViewData["url"] = url;
            ViewData["urlkh"] = urlkh;
            ViewData["urlkhjl"] = urlkhjl;
            ViewData["rid"] = RoleID;
            ViewData["urllog"] = urllog;
            ViewData["urlapp"] = urlapp;
            ViewData["urltask"] = urltask;
            return View();
        }

        // GET: TaskArragement
        //public ActionResult Index()
        //{
        //    return View();
        //}
        /// <summary>
        /// 消息发送
        /// </summary>
        /// <returns></returns>
        public ActionResult TaskIndex()
        {
            //var newid = "331081000000000";
            //ViewData["s"] = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            //ViewData["e"] = DateTime.Now.ToString("yyyy-MM-dd");
            //ViewData["adcd"] = newid;
            //ViewData["adcdparent"] = newid.Substring(0, 4) + "00000000000";
            //ViewData["rid"] = RoleID;
            //return View();
            ViewData["adcd"] = adcd;
            ViewData["year"] = 2017;
            //ViewData["adcd"] = "331081000000000";
            ViewData["rid"] = RoleID;
            ViewData["startTime"] = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            ViewData["endTime"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            return View();
        }
        public ActionResult FloatMenu()
        {
            ViewData["rid"] = RoleID;
            return View();
        }
        /// <summary>
        /// 履职查看
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkState()
        {
            ViewData["year"] = 2017;
            ViewData["adcd"] = adcd;
            ViewData["rid"] = 3;
            ViewData["startTime"] = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            ViewData["endTime"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            return View();
        }
        /// <summary>
        /// 消息发送
        /// </summary>
        /// <returns></returns>
        public ActionResult SendMessage()
        {
            ViewBag.Title = "发送消息";
            var evevtModel = new RouteGetAppWarnSelect();
            evevtModel.adcd = adcd;
            var evnetList = AppApiManager.GetWarnSelectList(evevtModel);
            var itemList = new List<SelectListItem>();
            foreach (var item in evnetList.WarnList)
            {
                itemList.Add(new SelectListItem() { Text = item.EventName, Value =item.Id.ToString() });
            }

            ViewData["itemList"] = itemList;
            ViewData["adcd"] = adcd;
            ViewBag.adcd = adcd;
            ViewBag.username = Session["username"];
            return View();
        }

        public ActionResult AddSendMessage()
        {
            string evnetId = Request.Form["EventId"].ToString();
            string leverId = Request.Form["LevelId"].ToString();
            string sendContent = Request.Form["SendContent"].ToString();
            string remark = Request.Form["Remark"].ToString();
            var model = new RouteAppWarnInfo();
            model.adcd = adcd;
            model.AppWarnEventId = int.Parse(evnetId);
            model.Remark = remark;
            model.userName = UserName;
            model.WarnLevel = int.Parse(leverId);
            model.WarnMessage = sendContent;
            var json = AppApiManager.AddAppWarnInfoAndAppSendMessage(model);
            return Json(json,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 事件追踪
        /// </summary>
        /// <returns></returns>
        public ActionResult EventTrail()
        {
            ViewData["year"] = 2017;
            ViewData["adcd"] = "331081000000000";
            ViewData["rid"] = 3;
            ViewData["startTime"] = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            ViewData["endTime"] = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        public ActionResult AddEvnet()
        {
            string eventName = Request.Form["EventName"].ToString();
            var model = new RouteAddAppWarnEvent();
            model.userName = UserName;
            model.EventName = eventName;
            var resurlt = AppApiManager.AddAppWarnEvent(model);
            return Json(resurlt,JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReceivePersonDetail(string appWarnInfoID)
        {
            this.ViewData["appInfoID"] = appWarnInfoID;
            this.ViewData["rid"] = this.RoleID;
            return View();
        }

        public ActionResult guiji(string receiveUserName)
        {
            this.ViewBag.userName2 = receiveUserName;
            return View();
        }
        public ActionResult lvzhi(string resevusername, string xiaoshijian,string dashijian)
        {
            ViewBag.resevusername = resevusername;
            ViewBag.xiaoshijian = xiaoshijian;
            ViewBag.dashijian = dashijian;
            return View();
        }
        
        public ActionResult GpsDisplay()
        {
            return this.View();
        }

        public ActionResult Emergency()
        {
            ViewBag.roleid = RoleID;
            ViewBag.username = UserName;
            ViewBag.adcd = adcd;
            return this.View();
        }

        public ActionResult SendMessageList()
        {
            return this.View();
        }

        public ActionResult VilliageDuty(string resiveadcd, int warninfoid)
        {
            ViewBag.resiveadcd = resiveadcd;
            ViewBag.warninfoid = warninfoid;
            return View();
        }

        //public ActionResult RegCount()
        //{
        //    return View();
        //}

    }
}