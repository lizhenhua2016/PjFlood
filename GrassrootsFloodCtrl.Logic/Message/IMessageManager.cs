using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Logic.Common;
using GrassrootsFloodCtrl.ServiceModel.Route;

namespace GrassrootsFloodCtrl.Logic.Message
{
   public interface IMessageManager
   {
        /// <summary>
        /// 保存短信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
       bool SaveMessage(SaveSmsMessage request);
   }
}
