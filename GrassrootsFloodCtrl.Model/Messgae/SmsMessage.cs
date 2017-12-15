using Dy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace GrassrootsFloodCtrl.Model.Messgae
{
    /// <summary>
    /// 短信
    /// </summary>
   public class SmsMessage
    {
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int id { get; set; }
        [field("手机号码", "string", null, null)]
        public string Mobile { get; set; }

        [field("短信内容", "string", null, null)]
        public string Content { get; set; }

        [field("发送人", "string", null, null)]
        public string name { get; set; }
        [field("行政区划", "string", null, null)]
        public string adcd { get; set; }
        [field("发送用户", "string", null, null)]
        public string UserName { get; set; }
        [field("发送时间", "DateTime", null, null)]
        public DateTime tm { get; set; }
    }
}
