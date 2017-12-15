using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.CApp
{
   public class CAppStaticsViewModel
    {
        public List<DisasterPointItem> DisasterPoint { get; set; }
        public int? CountyPLNums { get; set; }
        public int? TownPLNums { get; set; }
        public int? VillagePLNums { get; set; }
        public int? TransferPersonNums { get; set; }
    }
    public class DisasterPointItem
    {
        public int? nums { get; set; }
        public string typename { get; set; }
        public int? typeid { get; set; }
        public string adcd { get; set; }
    }
    public class NameADCD
    {
        public string  name { get; set; }
        public string adcd { get; set; }
    }
}
