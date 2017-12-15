using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.CApp
{
   public class CAppStaticsByPostViewModel
    {
        public List<CAppStaticsPLViewModel> DisasterPoint { get; set; }
        public int? CountyPLNums { get; set; }
        public int? TownPLNums { get; set; }
        public int? VillagePLNums { get; set; }
    }
    public class CAppStaticsPLViewModel
    {
        //岗位名
        public string post { get; set; }

        //责任人数量
        public int? nums { get; set; }

        //岗位等级
        public string postlevel { get; set; }
        
    }
}
