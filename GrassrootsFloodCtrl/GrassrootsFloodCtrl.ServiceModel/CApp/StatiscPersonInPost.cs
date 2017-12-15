using GrassrootsFloodCtrl.Model.Supervise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.CApp
{
   public class StatiscPersonInPost : StatiscPerson
    {
        public string adnm { get; set; }
        public string token { get; set; }
        public string lng { get; set; }
        public string lat { get; set; }
    }
}
