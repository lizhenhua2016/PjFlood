using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppLoginModel
    {
        public int StatusCode { get; set; }
        public bool ExistUser { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string ActionName { get; set; }
        public string Adcd { get; set; }
        public bool IsSend { get; set; }
        public List<postInfo> Postion { get; set; }
    }
}
