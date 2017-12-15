using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Sys
{
   public class DSUserInfoViewModel
    {
        public string UserName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        
        /// <summary>
        /// 行政区划编码
        /// </summary>
        public string adcd { get; set; }
      
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool isEnable { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public int? RoleID { get; set; }
    }
}
