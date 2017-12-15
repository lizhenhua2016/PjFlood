using GrassrootsFloodCtrl.Model.AppApi;
using GrassrootsFloodCtrl.ServiceModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppWarnViewModel
    {
        public int StateCode { get; set; }
        public string  Message { get; set; }
        /// <summary>
        /// 下拉框列表
        /// </summary>
        public List<AppWarnEvent> WarnList { get; set; }
    }
}
