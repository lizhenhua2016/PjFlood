using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.Supervise
{
   public class SpotCheckOne
    {
        public string bycheckman { get; set; }
        public string year { get; set; }
        public string bycheckphone { get; set; }
        public string areas { get; set; }
        public List<SpotCheck> checkItems { get; set; }
    }
}
