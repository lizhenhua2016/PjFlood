using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.ZZTX
{
   public class ResponseAdcdInfo
    {        
        public int Id { get; set; }

        
        public int parentId { get; set; }

        
        public int grade { get; set; }

       
      
        public string adcd { get; set; }

       
        public string adnm { get; set; }

      
        public double? lng { get; set; }

      
        public double? lat { get; set; }

       
        public DateTime? CreateTime { get; set; }
       
        public string operateLog { get; set; }
    }
}
