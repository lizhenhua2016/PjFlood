using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.CApp
{
   public class StaticsVillageTransferModel
    {
        /// <summary>
        ///  转移类型
        /// </summary>
        public string typename { get; set; }
        /// <summary>
        /// 影响人数
        /// </summary>
        public int nums { get; set; }
    }
}
