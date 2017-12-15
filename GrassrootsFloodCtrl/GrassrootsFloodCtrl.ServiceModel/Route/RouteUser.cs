using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.Model.Sys;
using GrassrootsFloodCtrl.ServiceModel.Sys;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Route("/Sys/User", "GET", Summary="获取用户列表")]
    [Api("系统用户相关接口")]
    public class GetUserInfoList : PageQuery,IReturn<BsTableDataSource<UserInfoViewModel>>
    {
        [ApiMember(IsRequired = false,DataType = "stirng",Description = "姓名")]
       public string name { get; set; }
        [ApiMember(IsRequired = false, DataType = "stirng", Description = "用户名")]
        public string userName { get; set; }
        [ApiMember(IsRequired = false, DataType = "stirng", Description = "行政区划编码")]
        public string adcd { get; set; }
    }

    [Route("/Sys/SaveUserInfo", "POST", Summary = "保存用户")]
    [Api("系统用户相关接口")]
    public class SaveUserInfo : IReturn<bool>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "用户ID")]
        public int id { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户名")]
        public string UserName { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "真实姓名")]
        public string RealName { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "密码")]
        public string PassWord { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "行政区划")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "手机号码")]
        public string Mobile { get; set; }
        [ApiMember(IsRequired = true, DataType = "bool", Description = "启用、停用")]
        public bool isEnable { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "角色ID")]
        public int role { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "人名")]
        public string UserRealName { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "单位")]
        public string Unit { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "职务")]
        public string Position { get; set; }
    }

    [Route("/Sys/DelUser", "Post", Summary = "删除用户")]
    [Api("系统用户相关接口")]
    public class DelUser : IReturn<bool>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户ID,多个以逗号隔开")]
        public string ids { get; set; }
    }

    [Route("/Sys/Login", "POST", Summary = "登陆系统")]
    [Api("系统用户相关接口")]
    public class LoginUser : IReturn<UserInfo>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户名")]
        public string UserName { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "密码")]
        public string PassWord { get; set; }
    }

    [Route("/Sys/GetUser", "GET", Summary = "根据条件获取单个用户")]
    [Api("系统用户相关接口")]
    public class GetUser : IReturn<UserInfo>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "用户名")]
        public string UserName { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "用户ID")]
        public int? userID { get; set; }

    }
    [Route("/Sys/GetUserLoginNum", "Get", Summary = "根据条件获取单个用户")]
    [Api("系统用户相关接口")]
    public class GetUserLoginNum : IReturn<int>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "用户名")]
        public string name { get; set; }
    }
    [Route("/Sys/SendMessgae", "Get", Summary = "根据用户名发送短信")]
    [Api("系统用户相关接口")]
    public class SendMessgae : IReturn<bool>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "用户名")]
        public string name { get; set; }
    }


    [Route("/Sys/GetUserByIdOrUserName", "GET", Summary = "根据条件获取单个用户")]
    [Api("系统用户相关接口")]
    public class GetUserByIdOrUserName : IReturn<UserInfoViewModel>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "用户名")]
        public string UserName { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "用户ID")]
        public int? userID { get; set; }

    }
    
    [Route("/Sys/changePassword/{Id}", "POST", Summary = "更改密码")]
    [Api("系统用户相关接口")]
    public class changePassword : IReturn<bool>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "用户ID")]
        public int Id { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "原密码")]
        public string oldPassword { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "新密码")]
        public string newPassword { get; set; }
        

    }

    [Route("/Sys/forgetPassword", "Post", Summary = "重置密码")]
    [Api("系统用户相关接口")]
    public class forgetPassword : IReturn<bool>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户名")]
        public string UserName { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "新密码")]
        public string Password { get; set; }
    }

    [Route("/Sys/Role/GetRolesList", "Get", Summary = "获取角色列表")]
    [Api("系统用户相关接口")]
    public class GetRolesList : PageQuery, IReturn<BsTableDataSource<Role>>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "自增Id")]
        public int? Id { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "角色名称")]
        public string roleName { get; set; }
    }
    [Route("/Sys/Log/getLogList", "Get", Summary = "获取日志")]
    [Api("系统用户相关接口")]
    public class getLogList : PageQuery, IReturn<BsTableDataSource<LogInfo>>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "自增Id")]
        public int? Id { get; set; }
        [ApiMember(IsRequired = false, DataType = "DateTime", Description = "开始时间")]
        public DateTime? startTime { get; set; }
        [ApiMember(IsRequired = false, DataType = "DateTime", Description = "结束时间")]
        public DateTime? stopTime { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "日志操作（1新增,2更新,3删除,4登陆,5退出）")]
        public GrassrootsFloodCtrlEnums.OperationTypeEnums OperationType { get; set; }
    }

    [Route("/Sys/Log/AddLog", "Post", Summary = "新增日志")]
    [Api("系统用户相关接口")]
    public class AddLog : IReturn<bool>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "自增Id")]
        public int? Id { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "日志操作（1新增,2更新,3删除,4登陆,5退出）")]
        public GrassrootsFloodCtrlEnums.OperationTypeEnums OperationType { get; set; }

        [ApiMember(IsRequired = true, DataType = "DateTime", Description = "操作时间")]
        public DateTime tm { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政区划")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户名")]
        public string UserName { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "操作日志")]
        public string Operation { get; set; }
    }
}
