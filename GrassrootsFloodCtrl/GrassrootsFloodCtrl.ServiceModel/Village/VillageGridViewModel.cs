using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Village
{
  public class VillageGridViewModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 村adcd
        /// </summary>
        public string VillageADCD { get; set; }
        /// <summary>
        /// 村名
        /// </summary>
        public string adnm { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        public string PersonLiable { get; set; }

        /// <summary>
        /// 网格名
        /// </summary>
        public string GridName { get; set; }
        /// <summary>
        /// 村网格名
        /// </summary>
        public string VillageGridName { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string HandPhone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? AddTime { get; set; }
        /// <summary>
        /// 编辑时间
        /// </summary>
        public DateTime? EditTime { get; set; }
        /// <summary>
        /// 编辑日志
        /// </summary>
        public string operateLog { get; set; }
        /// <summary>
        /// 所属年份
        /// </summary>
        public string Year { get; set; }
    }
}
