using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.CApp
{
   public class StatisticsByPostInfoViewModel
    {
        public string adcd { get; set; }
        /// <summary>
        /// 镇街名称
        /// </summary>
        public string adnm { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        public string position { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string post { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string mobile { get; set; }
        public string remarks { get; set; }
    }
}
