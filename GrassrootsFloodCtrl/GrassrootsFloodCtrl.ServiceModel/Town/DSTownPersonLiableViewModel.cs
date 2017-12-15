using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Town
{
   public class DSTownPersonLiableViewModel
    {
        /// <summary>
        /// 镇街编码
        /// </summary>
        public string adcd { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string Post { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
