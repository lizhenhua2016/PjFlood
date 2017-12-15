using System;
using System.ComponentModel.DataAnnotations;

namespace GrassrootsFloodCtrl.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {3} 个字符。", MinimumLength = 2)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        //[Display(Name = "是否需要验证码")]
        //public bool isCheck { get; set; }

        [Display(Name = "校验码")]
        public string Code { get; set; }

        //[Display(Name = "校验码发送时间")]
        //public DateTime SendTime { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }
}