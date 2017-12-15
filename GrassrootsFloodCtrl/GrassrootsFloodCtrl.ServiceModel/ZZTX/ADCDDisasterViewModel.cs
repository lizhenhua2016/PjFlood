using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.ZZTX
{
    /// <summary>
    /// 受灾害影响的行政区划信息VM
    /// </summary>
   public class ADCDDisasterViewModel
    {

        /// <summary>
        /// 受灾行政村自增ID
        /// </summary>
        
        public int Id { get; set; }
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
        /// 受灾点
        /// </summary>
    
        public double? PointNum { get; set; }

        /// <summary>
        /// 受灾害影响人口
        /// </summary>
        public double? PopulationNum { get; set; }

        /// <summary>
        /// 行政区划经度
        /// </summary>
        public double? Lng { get; set; }

        /// <summary>
        /// 行政区划纬度
        /// </summary>
        public double? Lat { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// 防汛防台任务轻重情况
        /// </summary>
        public string FXFTRW { get; set; }

        //public string IfTransfer { get; set; }

        public int? onperson { get; set; }
    }
}
