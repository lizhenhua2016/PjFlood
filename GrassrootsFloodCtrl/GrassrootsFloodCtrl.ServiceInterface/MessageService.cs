using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Logic.Message;
using ServiceStack;
using GrassrootsFloodCtrl.ServiceModel.Route;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    [Authenticate]
   public  class MessageService
    {
        public IMessageManager MessageManager { get; set; }
        /// <summary>
        /// 保存短信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool Post(SaveSmsMessage request)
        {
            return MessageManager.SaveMessage(request);
        }
    }
}
