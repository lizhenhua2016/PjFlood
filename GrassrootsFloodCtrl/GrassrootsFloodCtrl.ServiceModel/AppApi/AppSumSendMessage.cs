using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppSumSendMessage
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int Total { get; set; }
        public int StateCode { get; set; }
        public string Message { get; set; }
        
        public List<AppSumSendContent> Rows { get; set; }
    
    }
}
