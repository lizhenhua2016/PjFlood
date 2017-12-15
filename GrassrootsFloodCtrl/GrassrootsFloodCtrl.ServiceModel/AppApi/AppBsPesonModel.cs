using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppBsPesonModel
    {
        public int StateCode { get; set; }
        public string Message { get; set; }
        public int Total { get; set; }
        public List<AppPersonListModel> Rows { get; set; }
    }
}
