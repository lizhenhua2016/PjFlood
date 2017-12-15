using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrassrootsFloodCtrl.Controllers
{
    public class QRCodeVillageController : Controller
    {
        // GET: QRCodeVillage
        ///保持adcd的不过期，在页面切换的过程中将一直带着adcd
        
        /// <summary>
        /// 工作组
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ActionResult Index(string a)
        {
            ViewData["adcd"] = a;
            return View();
        }
        /// <summary>
        /// 网格
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ActionResult IndexWG(string a)
        {
            ViewData["adcd"] = a;
            return View();
        }
        /// <summary>
        /// 转移清单
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ActionResult IndexTrans(string a)
        {
            ViewData["adcd"] = a;
            return View();
        }
        /// <summary>
        /// 形势图
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ActionResult IndexPic(string a)
        {
            ViewData["adcd"] = a;
            return View();
        }
    }
}