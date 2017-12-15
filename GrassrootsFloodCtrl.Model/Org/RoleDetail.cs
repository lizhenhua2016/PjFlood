using Dy.Common;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.Org
{
   public class RoleDetail
    {
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int? RoleDetailID { get; set; }

        [field("角色id", "int", null, null)]
        public int? RoleID { get; set; }

        [field("栏目id","int",null,null)]
        public int? ColumnID { get; set; }

        [field("权限","string",null,null)]
        [StringLength(500)]
        public string Actions { get; set; }
    }
}
