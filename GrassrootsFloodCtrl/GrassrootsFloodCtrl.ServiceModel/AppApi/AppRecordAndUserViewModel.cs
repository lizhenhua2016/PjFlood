using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
   public class AppRecordAndUserViewModel : AppRecordViewModel
    {
        public string username { get; set; }
        private string _location = "";
        public string location {
            get { return string.IsNullOrEmpty(_location)?"-":_location; }
            set { _location = value; }
        }
    }
}
