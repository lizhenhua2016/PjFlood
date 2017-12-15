using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppGpsVillageDisplay
    {
        public string postCode { get; set; }
        public string gridType { get; set; }

        public string step { get; set; }     

        public string location { get; set; }

        public string username { get; set; }
        public string ReceiveUserName { get; set; }
    }
}
