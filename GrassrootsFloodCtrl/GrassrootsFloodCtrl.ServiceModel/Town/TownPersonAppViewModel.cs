using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Town
{
   public class TownPersonAppViewModel
    {
        public string GWName { get; set; }
        public int GWID { get; set; }
        public List<TownPersonLiableViewModel> Datas { get; set; }
    }
}
