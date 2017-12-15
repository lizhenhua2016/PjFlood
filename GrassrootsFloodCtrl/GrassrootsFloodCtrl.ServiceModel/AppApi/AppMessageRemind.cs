using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppMessageRemind
    {
        public int StateCode { get; set; }
        public string Message { get; set; }
        public int MessageCount { get; set; }
    }
}
