using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Messgae;
using GrassrootsFloodCtrl.ServiceModel.Sys;
using ServiceStack;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{

    [Route("/Sys/SmsMessage", "GET", Summary = "根据条件获取单个用户")]
    [Api("短信相关接口")]
    public class GetSmsMessageList :PageQuery,IReturn<BsTableDataSource<SmsMessage>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "用户名")]
        public string UserName { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政区划")]
        public string adcd { get; set; }
    }


    [Route("/Sys/SmsMessage", "Post", Summary = "保存短信")]
    [Api("短信相关接口")]
    public class SaveSmsMessage : IReturn<bool>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "手机号码")]
        public string Mobile { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "短信内容")]
        public string Content { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "发送人")]
        public string name { get; set; }
        [ApiMember(IsRequired = false, DataType = "DateTime", Description = "发送时间")]
        public DateTime? tm { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政区划")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "发送用户名")]
        public string UserName { get; set; }
    }
}
