using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.SumAppMessage
{
    public class SumAppWarnInfoModel
    {
        public string EventName { get; set; }
        public string Warninglevel { get; set; }
        public string SendMessageByUserName { get; set; }
        public DateTime ReceiveDateTime { get; set; }
        public string AppWarnInfoID { get; set; }
        public int ReadCount { get; set; }
        public int NoReadCount { get; set; }
    }
}
