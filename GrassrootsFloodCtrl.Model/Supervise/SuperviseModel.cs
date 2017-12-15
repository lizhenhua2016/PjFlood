using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.Supervise
{
  public class SuperviseModel
    {
        /// <summary>
        /// 所有乡镇
        /// </summary>
        public int? TownAll { get; set; }
        /// <summary>
        /// 省级统计已上报数
        /// </summary>
        public int? HasReportedAll { get; set; }
        /// <summary>
        /// 省级统计未上报数
        /// </summary>
        public int? NoReportedAll { get; set; }
        public int? isSupervise { get; set; }
        /// <summary>
        /// 市县级统计
        /// </summary>
        public List<NextLevelStatics> CCStatics { get; set; }
        /// <summary>
        /// 县级责任人
        /// </summary>
        public List<NextLevelStatics> CStattics { get; set; }
        //
        public List<LogStatisItem> LSStatics { get; set; }
        public List<LogTownStatisItem> LSTownStatics { get; set; }
        public int? HasReportedAllCounty { get; set; }
        public int? NoReportedAllCounty { get; set; }
        public int? VillageAll { get; set; }
        public int? CountyAll { get; set; }
        /// <summary>
        /// 市县层级
        /// </summary>
        public int? Level { get; set; }
        /// <summary>
        /// 省级评价
        /// </summary>
        public string GradeName { get; set; }
        /// <summary>
        /// 县级评价
        /// </summary>
        public string GradeNameCounty { get; set; }
        public string ADNM { set; get; }
        public string PADNM { set; get; }
        public string PPADNM { set; get; }

        public string TownADCD { get; set; }
        public string adcd { get; set;}
        public string adnm { get; set; }
        public List<CountyVillage> CountyVillage { get; set; }
    }
    public class CountyVillage
    {
        public string adnm { get; set; }
        public int villagecount { get; set; }
    }
    public class NextLevelStatics
    {
        /// <summary>
        /// 市,县级adcd
        /// </summary>
        public string ADCD { get; set; }
        /// <summary>
        /// 市,县级名称
        /// </summary>
        public string ADNM { get; set; }
        /// <summary>
        /// 已填报
        /// </summary>
        public int? HasReported { get; set; }
        /// <summary>
        /// 未填报
        /// </summary>
        public int? NoReported { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public double? Grade { get; set; }
        /// <summary>
        /// 评价
        /// </summary>
        public string GradeName { get; set; }
        #region 审核
        public int countyUnAudited { get; set; }
        public int countyAudited { get; set; }
        public int townAudited { get; set; }
        public int townUnAudited { get; set; }
        #endregion
    }
    public class LogTownStatisItem
    {
        /// <summary>
        /// 市,县级adcd
        /// </summary>
        public string ADCD { get; set; }
        /// <summary>
        /// 市,县级名称
        /// </summary>
        public string ADNM { get; set; }
        public int adcdount { get; set; }
        //镇更新数量
        public int townUpdateNum { get; set; }
        //镇数量
        public int townAllNum { get; set; }
        //县级是否更新
        public int countyIfUpdate { get; set; }
        //镇相关信息
        public int? townPersonNum{get;set;}
        public int? workgroupPersonNum { get; set; }
        public int? gridPersonNum { get; set; }
        public int? transferPersonNum { get; set; }
        public int? picPersonNum { get; set; }
    }
    public class LogStatisItem
    {
        public string adcd { get; set; }
        //adcd 出现次数
        public int adcdount { get; set; }
    }
}
