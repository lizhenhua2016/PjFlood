using GrassrootsFloodCtrl.Model.AppApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppSendMessageModel
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int Total { get; set; }
        public int StateCode { get; set; }
        public string Message { get; set; }
        public int NoReadSumCount { get; set; }
        /// <summary>
        /// 消息列表
        /// </summary>
        public List<AppMessageModel> Rows { get; set; }
    }
}
