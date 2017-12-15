using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppMessageModel
    {
        public string Id { get; set; }
        public string Warninglevel { get; set; }
        public string SendMessage { get; set; }
        public bool IsReaded { get; set; }
        public string AppWarnInfoID { get; set; }
        public string SendAdcd { get; set; }
        public string SendMessageByUserName { get; set; }
        public DateTime? Time { get; set; }
        public int SendCount { get; set; }
        public int NoReadCount { get; set; }
        public bool IsClosed { get; set; }
        public string AppWarnEventName { get; set; }
        public string ReceiveUserName { get; set; }
        public int WarninglevelId { get; set; }
        public int AppWarnEventId { get; set; }
        public string Remark { get; set; }
        public string Percent { get; set; }
        public string adnm { get; set; }
    }
}
