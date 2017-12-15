using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.ZZTX
{
   public class DSADCDDisasterViewModel
    {
        /// <summary>
        /// 行政区划编码
        /// </summary>
        public string adcd { get; set; }

        /// <summary>
        /// 行政区划名称
        /// </summary>
        public string adnm { get; set; }
        /// <summary>
        /// 总人口数
        /// </summary>

        public double? TotalNum { get; set; }

        /// <summary>
        /// 行政区划经度
        /// </summary>
        public double? Lng { get; set; }

        /// <summary>
        /// 行政区划纬度
        /// </summary>
        public double? Lat { get; set; }

        /// <summary>
        /// 防汛防台任务轻重情况
        /// </summary>
        public string FXFTRW { get; set; }
    }
}
