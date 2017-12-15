using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.CApp
{
   public class StatisAppPersonInPostViewModel
    {
        //市级
        public string cityName { get; set; }
        //县级
        public string countyName { get; set; }
        //县级adcd
        public string countyadcd { get; set; }
        //乡镇总数
        public int? townNum { get; set; }
        //行政村总数
        public int? villageNum { get; set; }
        //行政村责任人总数
        public int? villagePersonNum { get; set; }
        //行政村责任人到岗总数
        public int? villagePersonInPostNum { get; set; }
        public string numstr { get; set; }
    }
}
