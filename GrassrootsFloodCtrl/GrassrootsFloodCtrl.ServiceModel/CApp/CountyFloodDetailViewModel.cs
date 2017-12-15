using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.CApp
{
    public class CountyFloodDetailViewModel
    {
        //乡镇(街道)
        public string townName { get; set; }
        //行政村
        public string villageName { get; set; }
        //防汛任务情况
        public string prevFloodTask { get; set; }
    }
}
