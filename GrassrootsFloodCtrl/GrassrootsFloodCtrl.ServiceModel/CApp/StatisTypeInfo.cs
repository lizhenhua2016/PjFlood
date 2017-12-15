using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.CApp
{
   public class StatisTypeInfo
    {
        public string adcd { get; set; }
        public string countyadcd { get; set; }
        public string cityName { get;set;}
        public string countyName { get; set; }
        public int? townnum { get; set; }
        public string typeName { get; set; }
        public int? typeNum { get; set; }
        public int? householderNum { get; set; }
        public int? personLiableNum { get; set; }
        public int? warnPersonLiableNum { get; set; }
        public int? disasterPreventionManagerNum { get; set; }
        public string numstr { get; set; }
    }
}
