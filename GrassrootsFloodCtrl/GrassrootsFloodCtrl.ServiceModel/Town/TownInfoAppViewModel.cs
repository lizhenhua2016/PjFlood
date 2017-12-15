using GrassrootsFloodCtrl.Model.ZZTX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Town
{
    public class TownInfoAppViewModel
    {
       
        public string ZhenName { get; set; }
        public string ShiName { get; set; }
        public double LGT { get; set; }
        public double LTT { get; set; }
        public int ZhenBenji { get; set; }
        public int CunBenji { get; set; }
        public int ZDPoint { get; set; }
        public int ZDManNums { get; set; }
        public List<GridModel> rows { get; set; }
        public int cuncount { get; set; }
        public List<ADCDInfo> cuns { get; set; }
    }
    public class GridModel{
        public string wanggeName { get; set; }
        public int wanggeCount { get; set; }
    }
}
