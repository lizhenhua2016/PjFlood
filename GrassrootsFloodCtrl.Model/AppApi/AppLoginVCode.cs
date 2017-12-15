using Dy.Common;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.AppApi
{
  public  class AppLoginVCode
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
        public string adddtime { get; set; }
    }
}
