using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppWarnEventNextModel
    {
        public string Id { get; set; }
        public int AppWarnInfoid { get; set; }
        public int AppWarnEventId { get; set; }

        public string Warninglevel { get; set; }

        public string SendMessage { get; set; }
        public string SendAdcd { get; set; }
        public string SendMessageByUserName { get; set; }
        public string ReceiveDateTime { get; set; }
        public string EventName { get; set; }
    }
}
