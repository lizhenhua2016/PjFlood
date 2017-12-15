using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Country
{
    //县级城防体系人员输出列表模板
    public  class CountryPersonServiceModel
    {
        public int Id { get; set; }

        public string Position { get; set; }

        public string UserName { get; set; }

        public string Post { get; set; }

        public string Phone { get; set; }

        public string Remark { get; set; }
        public string adcd { get; set; }
        public string adnm { get; set; }
        public string checkresult { get; set; }
        public string adnmparent { get; set; }
        public DateTime? CreateTime { get; set; }
        public int Gwid { get; set; }
    }
}
