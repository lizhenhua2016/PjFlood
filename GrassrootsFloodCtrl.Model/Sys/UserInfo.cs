using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dy.Common;
using ServiceStack.Auth;
using ServiceStack.DataAnnotations;

namespace GrassrootsFloodCtrl.Model.Sys
{
    /// <summary>
    /// 用户信息
    /// </summary>
   public class UserInfo
    {
        [PrimaryKey]
        [AutoIncrement]
        [field("自增ID", "int", null, null)]
        public int Id { get; set; }

        [field("用户名", "string", null, null)]
        [StringLength(50)]
        public string UserName { get; set; }

        [field("密码", "string", null, null)]
        [StringLength(50)]
        public string PassWord { get; set; }

        [field("真实名字", "string", null, null)]
        [StringLength(50)]
        public string RealName { get; set; }

        [field("管理员姓名", "string", null, null)]
        [StringLength(50)]
        public string UserRealName { get; set; }
        [field("管理员所在单位", "string", null, null)]
        [StringLength(100)]
        public string Unit { get; set; }
        [field("职务", "string", null, null)]
        [StringLength(100)]
        public string Position { get; set; }

        [field("行政区划", "string", null, null)]
        [StringLength(15)]
        public string adcd { get; set; }

        [field("手机号码", "string", null, null)]
        [StringLength(11)]
        public string Mobile { get; set; }

        [field("是否启用", "bool", null, null)]
        public bool isEnable { get; set; }
        [field("登陆次数", "int", null, null)]
        public int loginNum { get; set; }
    }
}
