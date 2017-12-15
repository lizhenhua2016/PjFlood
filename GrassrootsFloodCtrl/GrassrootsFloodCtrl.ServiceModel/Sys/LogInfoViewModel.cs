using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Sys
{
   public class LogInfoViewModel
    {
        public int Id { get; set; }
        
        public string OperationType { get; set; }
        
        public DateTime tm { get; set; }
 
        public string adcd { get; set; }
     
        public string adnm { get; set; }
        public string UserName { get; set; }
        
        public string RealName { get; set; }

        public string Operation { get; set; }
    }
}
