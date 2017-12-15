using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Org
{
   public class ColumnManageViewModel
    {
        //栏目id
        public int ColumnID { get; set; }
        //栏目名
        public string ColumnName { get; set; }
        //图标ico
        public string Icon { get; set; }
        //地址
        public string LocalUrl { get; set; }
        //是否显示
        public bool IsVisible { get; set; }
        //父id
        public int ParentID { get; set; }
        //等级
        public string Level { get; set; }
        //操作权限
        public string Actions { get; set; }
        //排序
        public int Sort { get; set; }
    }
}
