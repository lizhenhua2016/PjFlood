using GrassrootsFloodCtrl.ServiceModel.Town;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Village
{
   public class VillageInfoAppViewModel
    {
         public string ADCD { get; set; }
        public string Name { get; set; }
        public string ZhenName { get; set; }
        public string ShiName { get; set; }
        //经度
        public string LGT { get; set; }
        public string LTT { get; set; }
        public int? AllManNums { get; set; }
        public int? DisasterPoint { get; set; }
        public int? DisasterManNums { get; set; }
        //村责任人数
        public int? villagePersonNums { get; set; }
        //村到岗人数
        public int? villageInPostNums { get; set; }
        public string qrpath { get; set; }
        public List<GridModel> rows { get; set; }
    }
}
