// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatisAnalysisController.cs" company="lizhenhua">
//   lizhenhua
// </copyright>
// <summary>
//   Defines the StatisAnalysisController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GrassrootsFloodCtrl.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// The statis analysis controller.
    /// </summary>
    [Authorize]
    public class StatisAnalysisController : ControllerBase
    {
        /// GET: StatisAnalysis
        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Index()
        {
            this.ViewData["adcd"] = adcd.Substring(0, 4);
            return this.View();
        }

        #region 类型统计
        public ActionResult StatisType()
        {
            if(RoleID == 3) ViewData["adcd"] = adcd;
            else ViewData["adcd"] = adcd.Substring(0, 4);
            return View();
        }
        #endregion

        #region 岗位统计
        public ActionResult StatisPost()
        {
            if (RoleID == 3) ViewData["adcd"] = adcd;
            else ViewData["adcd"] = adcd.Substring(0, 4);
            return View();
        }
        #endregion

        /// <summary>
        /// The statis type info.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisTypeInfo()
        {
            this.ViewData["roleid"] = RoleID;
            return this.View();
        }

        /// <summary>
        /// The statis type info one.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisTypeInfoOne()
        {
            return this.View();
        }

        /// <summary>
        /// The statis prev flood.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisPrevFlood()
        {
            this.ViewBag.RoleId = this.RoleID;
            return this.View();
        }

        /// <summary>
        /// The left.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult left()
        {
            var url = "";
            var urlkh = "";
            var urlkhjl = "";
            var urlapp = "";
            var urlPrevFlood = "";
            //审批统计
            url = "/StatisAnalysis";
            //类型统计
            urlkh = "/StatisAnalysis/StatisType";
            //岗位统计
            urlkhjl = "/StatisAnalysis/StatisPost";
            //到岗人员统计
            urlapp = "/StatisAnalysis/StatisAppPersonInPost";
            //防汛任务统计
            urlPrevFlood = "/StatisAnalysis/StatisPrevFlood";
            //switch (RoleID)
            //{
            //    case 5:
            //        url = "/StatisAnalysis";
            //        urlkh = "/StatisAnalysis/StatisType";
            //        urlkhjl = "/StatisAnalysis/StatisPost";
            //        urlapp = "/StatisAnalysis/StatisAppPersonInPost";
            //        break;
            //    case 2:
            //        url = "/StatisAnalysis";
            //        urlkh = "/StatisAnalysis/StatisType";
            //        urlkhjl = "/StatisAnalysis/StatisPost";
            //        urlapp = "/StatisAnalysis/StatisAppPersonInPost";
            //        break;
            //    case 3:
            //        url = "/StatisAnalysis";
            //        urlkh = "/StatisAnalysis/StatisType";
            //        urlkhjl = "/StatisAnalysis/StatisPost";
            //        urlapp = "/StatisAnalysis/StatisAppPersonInPost";
            //        break;                
            //}

            this.ViewData["url"] = url;
            this.ViewData["urlkh"] = urlkh;
            this.ViewData["urlkhjl"] = urlkhjl;
            this.ViewData["urlapp"] = urlapp;
            this.ViewData["urlPrevFlood"] = urlPrevFlood;
            this.ViewData["rid"] = this.RoleID;
            return View();
        }

        /// <summary>
        /// The statis county person.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisCountyPerson()
        {
            return this.View();
        }

        /// <summary>
        /// The statis county person one.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisCountyPersonOne()
        {
            return this.View();
        }

        /// <summary>
        /// The statis town person.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisTownPerson()
        {
            this.ViewData["rid"] = this.RoleID;
            return this.View();
        }

        /// <summary>
        /// The statis village person.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisVillagePerson()
        {
            this.ViewData["rid"] = this.RoleID;
            return this.View();
        }

        /// <summary>
        /// The statis transferperson.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisTransferperson()
        {
            return this.View();
        }

        /// <summary>
        /// The statis transfer person one.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisTransferPersonOne()
        {
            return this.View();
        }

        /// <summary>
        /// The statistics by post one.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisticsByPostOne()
        {
            return this.View();
        }


        /// <summary>
        /// The statis all grid.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisAllGrid()
        {
            this.ViewData["roid"] = this.RoleID;
            return this.View();
        }

        /// <summary>
        /// The statis all danger.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisAllDanger()
        {
            this.ViewData["roid"] = this.RoleID;
            return this.View();
        }

        /// <summary>
        /// The statis type info county.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisTypeInfoCounty()
        {
            return this.View();
        }

        /// <summary>
        /// The statis county all grid.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisCountyAllGrid()
        {
            return this.View();
        }

        /// <summary>
        /// The statis county all danger.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisCountyAllDanger()
        {
            return this.View();
        }

        /// <summary>
        /// The statistics couty by post one.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisticsCoutyByPostOne()
        {
            return this.View();
        }


        /// <summary>
        /// The statis county transfer person one.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisCountyTransferPersonOne()
        {
            return this.View();
        }


        /// <summary>
        /// The statis app person in post.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisAppPersonInPost()
        {
            return this.View();
        }

        /// <summary>
        /// The statis app person in post by county.
        /// </summary>
        /// <param name="_adcd">
        /// The _adcd.
        /// </param>
        /// <param name="year">
        /// The year.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisAppPersonInPostByCounty(string _adcd,string year)
        {
            this.ViewData["rid"] = this.RoleID;
            this.ViewData["adcd"] = _adcd;
            this.ViewData["year"] = year;
            return this.View();
        }

        /// <summary>
        /// The statis town flood detail.
        /// </summary>
        /// <param name="adcd">
        /// The adcd.
        /// </param>
        /// <param name="year">
        /// The year.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult StatisTownFloodDetail(string adcd,string year)
        {
            this.ViewData["rid"] = this.RoleID;
            this.ViewData["adcd"] = adcd;
            this.ViewData["year"] = year;
            return this.View();
        }
    }
}