using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Position
{
   public class PositionViewModel
    {
        /// <summary>
        /// 职位ID
        /// </summary>
        public int? ID { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>
        public string PositionName { get; set; }
    }
}
