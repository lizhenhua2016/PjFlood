using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppSendDetailsModel
    {
        public string Id { get; set; }
        public string SendMessagePhone { get; set; }
        public string SendMessageByUserName { get; set; }
        public string AppWarnEventName { get; set; }
        public string Warninglevel { get; set; }
        public string SendMessage { get; set; }
        public string ReceiveUserPhone { get; set; }
        public string ReceiveUserName { get; set; }
        public bool IsReaded { get; set; }
        public DateTime? UserReadTime { get; set; }
        public int AppWarnInfoID { get; set; }
        public DateTime? ReceiveDateTime { get; set; }
        public bool IsClosed { get; set; }
        public string Remark { get; set; }
        public string VillageMessage { get; set; }
        public string SendAdcd { get; set; }
    }
}
