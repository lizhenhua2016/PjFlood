using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.Village
{
   public class StaticsVillageGroup
    {
        public string post { get; set; }
        public int? postid { get; set; }
        public List<PersonLiabel> datas { get; set; }
    }
    public class PersonLiabel
    {
        public string adcd { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string position { get; set; }
    }
}
