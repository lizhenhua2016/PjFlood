using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model.Messgae;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack.OrmLite;

namespace GrassrootsFloodCtrl.Logic.Message
{
    public class MessageManager:ManagerBase,IMessageManager
    {
        public bool SaveMessage(SaveSmsMessage request)
        {
            using (var db=DbFactory.Open())
            {
                var info = new SmsMessage();
                if(string.IsNullOrEmpty(request.Mobile))
                    throw  new Exception("手机号码不能为空");
                if (string.IsNullOrEmpty(request.Content))
                    throw new Exception("短信内容不能为空");
                info.name = !string.IsNullOrEmpty(request.name) ? request.name : RealName;
                info.tm = request.tm!=null ? request.tm.Value : DateTime.Now;
                info.Mobile = request.Mobile;
                info.Content = request.Content;
                info.adcd = !string.IsNullOrEmpty(request.adcd) ? request.adcd : adcd;
                info.UserName = !string.IsNullOrEmpty(request.UserName) ? request.UserName : UserName;
                return db.Insert(info)==1;
            }
        }
    }
}
