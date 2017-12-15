using GrassrootsFloodCtrl.ServiceModel.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
   public class MailListViewModel
    {
        public string postname { get; set; }
        public List<PeerUser> userList { get; set; }
    }
    public class PeerUser
    {
        public string adcd { get; set; }
        public string RealName { get; set; }
        public string Mobile { get; set; }
    }
}
