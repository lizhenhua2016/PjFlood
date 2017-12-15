using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model
{
    /// <summary>
    /// 用来绑定BootstrapTable控件的数据源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BsTableDataSource<T> where T : class
    {
        public long total { get; set; }

        public string other { get; set; }

        public List<T> rows { get; set; }
    }
}
