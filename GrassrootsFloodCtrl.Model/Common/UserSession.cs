using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace GrassrootsFloodCtrl.Model.Common
{
   public class UserSession: AuthUserSession
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        //public string UserName { get; set; }
        /// <summary>
        /// 真实名称
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 行政区划
        /// </summary>
        public string adcd { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public int? RoleId { get; set; }
        /// <summary>
        /// 审核申请当前次数
        /// </summary>
        public int? AuditNums { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int? AuditStatus { get; set; }
        
        /// <summary>
        /// 经度
        /// </summary>
        public string lng { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public string lat { get; set; }
    }
}
