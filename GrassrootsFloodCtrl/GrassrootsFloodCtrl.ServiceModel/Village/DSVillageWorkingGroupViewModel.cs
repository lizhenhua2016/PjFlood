using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Village
{
  public class DSVillageWorkingGroupViewModel
    {
        public string VillageADCD { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        public string PersonLiable { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        public string Post { get; set; }

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
    }
}
