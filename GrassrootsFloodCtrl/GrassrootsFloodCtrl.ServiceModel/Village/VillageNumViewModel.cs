using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model.Enums;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;

namespace GrassrootsFloodCtrl.ServiceModel.Village
{
    /// <summary>
    /// 
    /// </summary>
    public  class VillageNumViewModel
    {
        /// <summary>
        /// 责任体系组别
        /// </summary>
        public ZZTXEnums ZZTX { get; set; }
        /// <summary>
        /// 已上报数
        /// </summary>
        public int? HasReported { get; set; }
        /// <summary>
        /// 未上报数
        /// </summary>
        public int? NoReported { get; set; }
    }
}
