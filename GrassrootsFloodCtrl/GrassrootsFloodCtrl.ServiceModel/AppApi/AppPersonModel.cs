using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppPersonModel
    {
        public int StateCode { get; set; }
        public string Message { get; set; }
        public List<AppPersonListModel> NoReadPerson { get; set; }
        public List<AppPersonListModel> ReadPerson { get; set; }
    }
}
