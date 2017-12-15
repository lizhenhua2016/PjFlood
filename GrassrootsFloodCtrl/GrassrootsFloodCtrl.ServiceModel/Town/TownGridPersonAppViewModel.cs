using GrassrootsFloodCtrl.ServiceModel.Village;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Town
{
  public class TownGridPersonAppViewModel
    {
        public string CunADCD { get; set; }
        public string CunName { get; set; }
       public List<VillageWorkingGroupViewModel> Rows { get; set; }
    }
}
