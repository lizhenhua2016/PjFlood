using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.DangerZone
{
   public  class DangerZoneViewModel
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 危险点类型名称
        /// </summary>
        public string DangerZoneName { get; set; }

        /// <summary>
        /// 所属地区编码
        /// </summary>

        public string adcd { get; set; }
        /// <summary>
        /// 所属地区名称
        /// </summary>

        public string adnm { get; set; }
        /// <summary>
        /// 操作日志
        /// </summary>
        public string operateLog { get; set; }
    }
}
