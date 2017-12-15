using Dy.Common;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.Org
{
   public class Column
    {
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int? ColumnID { get; set; }

        [field("栏目名", "string", null, null)]
        [StringLength(100)]
        public string ColumnName { get; set; }
        
        [field("图标样式", "string",null,null)]
        [StringLength(50)]
        public string Icon { get; set; }

        [field("栏目地址", "string", null, null)]
        [StringLength(200)]
        public string LocalUrl { get; set; }
        
        [field("是否显示", "bit", null, null)]
        public bool IsVisible { get; set; }

        [field("父id", "int", null, null)]
        public int? ParentID { get; set; }

        [field("等级", "string", null, null)]
        [StringLength(100)]
        public string Level { get; set; }

        [field("操作权限", "string", null, null)]
        [StringLength(100)]
        public string Actions { get; set; }

        [field("序号", "int", null, null)]
        public int? Sort { get; set; }
    }
}
