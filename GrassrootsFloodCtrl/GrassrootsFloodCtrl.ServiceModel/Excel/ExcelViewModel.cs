using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Excel
{
    public class ExcelViewModel
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 成功的数量
        /// </summary>
        public int SuccessNum { get; set; }
        /// <summary>
        /// 失败的数量
        /// </summary>
        public int failNum { get; set; }
        //失败的问题
        public List<Dictionary<string,string>> ErrorList { get; set; }
    }
}
