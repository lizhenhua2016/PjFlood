using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dy.Common;

namespace GrassrootsFloodCtrl.Model.AppApi
{
    public class AppMobileLogin
    {
        /// <summary>
        /// 登陆验证码绑定
        /// </summary>
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int ID { get; set; }
        [field("用户账号", "string", null, null)]
        public string userName { get; set; }
        [field("验证码", "string", null, null)]
        public string VerificationCode { get; set; }
        [field("用户唯一标识token", "string", null, null)]
        public string token { get; set; }
        [field("生成时间", "datetime", null, null)]
        public DateTime adddtime { get; set; }
        [field("adcd", "string", null, null)]
        public string adcd { get; set; }
        [field("adcdId", "int", null, null)]
        public int adcdId { get; set; }
        [field("真实姓名", "int", null, null)]
        public string realName { get; set; }
    }
}
