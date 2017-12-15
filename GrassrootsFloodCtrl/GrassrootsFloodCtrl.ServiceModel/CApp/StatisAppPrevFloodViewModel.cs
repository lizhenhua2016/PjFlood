using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.CApp
{
    public class StatisPrevFloodViewModel
    {
        //市级
        public string cityName { get; set; }
        //县级
        public string countyName { get; set; }
        //县级adcd
        public string countyadcd { get; set; }
        //镇级名称
        public string townName { get; set; }
        //镇级adcd
        public string townadcd { get; set; }
        //id
        public int id { get; set; }
        //父id
        public int parentId { get; set; }
        //乡镇总数
        public int? townNum { get; set; }
        //行政村总数
        public int? villageNum { get; set; }
        //防汛任务较轻
        public int? prevFloodTaskLight { get; set; }
        //防汛任务较重
        public int? prevFloodTaskHeavy { get; set; }
        public string numstr { get; set; }

    }
}
