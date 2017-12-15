using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dy.Common;

namespace GrassrootsFloodCtrl.Model.Sys
{
    /// <summary>
    /// 用户角色关系表
    /// </summary>
   public class UserRoleInfo
    {
        [PrimaryKey]
        [AutoIncrement]
        [field("自增ID", "int", null, null)]
        public int Id { get; set; }

        [field("角色ID", "string", null, null)]
        public int RoleID { get; set; }

        [field("用户ID", "string", null, null)]
        public int UserID { get; set; }
    }
}
