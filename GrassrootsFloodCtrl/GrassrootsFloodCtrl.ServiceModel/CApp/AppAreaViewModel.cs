using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.CApp
{
   public class AppAreaViewModel
    {
        public int id { get; set; }
        public int parentId { get; set; }
        public int grade { get; set; }
        public string adcd { get; set; }
        public string adnm { get; set; }
        public string padnm { get; set; }
        public string lng { get; set; }
        public string lat { get; set; }
        public int? inperson { get; set; }
        public int? noperson { get; set; }
        public List<AppAreaViewModelSon> SonList { get; set; }
    }
    public class AppAreaViewModelSon
    {
        public int id { get; set; }
        public int parentId { get; set; }
        public int grade { get; set; }
        public string adcd { get; set; }
        public string adnm { get; set; }
        public string lng { get; set; }
        public string lat { get; set; }
        public int? inperson { get; set; }
        public int? noperson { get; set; }
    }
}
