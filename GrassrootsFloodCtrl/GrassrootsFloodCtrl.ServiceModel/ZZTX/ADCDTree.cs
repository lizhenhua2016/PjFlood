using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.ZZTX
{
   public class ADCDTree
    {
        public int id { get; set; }
        public int pId { get; set; }
        public string adcd { get; set; }
        public string name { get; set; }
        public int level { get; set; }
        public bool open { get; set; }
        public string areapath { get; set; }
    }
}
