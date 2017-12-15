using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.CApp
{
    public class AppStaticsViewModel
    {
        public string adcd { get; set; }
        public string name { get; set; }
        //维度
        public double? latitude { get; set; }
        //经度
        public double? longitude { get; set; }
        //数量
        public int? count { get; set; }
    }
}
