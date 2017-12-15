using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.SumAppMessage
{
    public class SumReadModel
    {
        public string ReceiveUserName { get; set; }
        public string ReceiveUserPhone { get; set; }
        public string Position { get; set; }
        public DateTime ReceiveDateTime { get; set; }
        public string MessageId { get; set; }
        public string grade { get; set; }
    }
}
