using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
   public class AppLoginVCodeViewModel
    {
        public int ID { get; set; }

        public string userName { get; set; }
  
        public string VerificationCode { get; set; }
 
        public string token { get; set; }

        public string adddtime { get; set; }

        public string adcd { get; set; }
    }
}
