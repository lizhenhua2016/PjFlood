using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.CApp
{
   public class StatisTransferPersonViewModel
    {
        public string adcd { get; set; }
        public string countyadcd { get; set; }
        public string cityName { get; set; }
        public string countyName { get; set; }
        public int? weifangNumWXQ { get; set; }
        public int? shanhongNumWXQ { get; set; }
        public int? dizhizaihaiNumWXQ { get; set; }
        public int? diwayilaoNumWXQ { get; set; }
        public int? wudingshantangNumWXQ { get; set; }
        public int? difangxianduanNumWXQ { get; set; }
        public int? haitangxianduanNumWXQ { get; set; }
        public int? qitaNumWXQ { get; set; }
        public string numstr { get; set; }
    }
    public class StatisTransferADCD
    {
        public string adcd { get; set; }
        public string countyadcd { get; set; }
        public string adnm { get; set; }
        public string countyname { get; set; }
    }
    public class StatisFloodADCD
    {
        public int? Id { get; set; }
        public int? parentId { get; set; }
        public int? grade { get; set; }
        public string adcd { get; set; }
        public string countyadcd { get; set; }
        public string adnm { get; set; }
        public string countyname { get; set; }
    }
}
