using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.DataShare
{
    public class DataShareReturnModel
    {
        public string UserName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 行政区划编码
        /// </summary>
        public string adcd { get; set; }
    }
}
