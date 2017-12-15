using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.SumAppMessage
{
    public class AppSumEventModel
    {
        public string adnm { get; set; }
        public string UserName { get; set; }
        public DateTime StartTime { get; set; }
        public string EventName { get; set; }
        public int Id { get; set; }
    }
}
