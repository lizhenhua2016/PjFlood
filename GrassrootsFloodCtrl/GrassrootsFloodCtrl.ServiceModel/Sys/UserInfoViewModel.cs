using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Sys
{
    /// <summary>
    /// 用户信息VM
    /// </summary>
    public class UserInfoViewModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 行政区划编码
        /// </summary>
        public string adcd { get; set; }
        /// <summary>
        /// 所属市
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 所属县市区
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool isEnable { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public int? RoleID { get; set; }

        /// <summary>
        /// 人名
        /// </summary>
        public string UserRealName { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        ///验证码 
        /// </summary>
        public string VCode { get; set; }
    }
}
