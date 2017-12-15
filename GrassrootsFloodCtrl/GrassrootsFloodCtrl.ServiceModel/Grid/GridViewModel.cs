using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Grid
{
   public class GridViewModel
    {
        /// <summary>
        /// 网格ID
        /// </summary>
        public int? ID { get; set; }

        /// <summary>
        /// 网格名称
        /// </summary>
        public string GridName { get; set; }

        /// <summary>
        /// 网格对应职务名
        /// </summary>
        public string PositionName { get; set; }
    }
}
