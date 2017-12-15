using GrassrootsFloodCtrl.Model.AppApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
   public class AppSendMessageAndPostModel : AppSendMessage
    {
        public List<string> Posts { get; set; }
    }
}
