using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using GrassrootsFloodCtrl.ServiceModel.Sys;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Sys;

namespace GrassrootsFloodCtrl.Logic.Sys
{
    /// <summary>
    /// 系统相关接口
    /// </summary>
   public interface ISysManager 
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<UserInfoViewModel> GetUserInfoList(GetUserInfoList request);

        /// <summary>
        /// 登录前获取登录次数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        int GetUserLoginNum(GetUserLoginNum request);
        /// <summary>
        /// 根据用户名送短信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool SendMessgae(SendMessgae request);

        /// <summary>
        /// 根据用户名或行政区划编码获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="ADCD"></param>
        /// <returns></returns>
        UserInfo GetUserInfoByUserName(string userName,string ADCD=null);

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool SaveUserInfo(SaveUserInfo request);
        /// <summary>
        /// 根据条件获取单个用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        UserInfoViewModel GetUserByIdOrUserName(GetUserByIdOrUserName request);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DelUser(string ids);

        /// <summary>
        /// 根据条件获取单个用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        UserInfo GetUser(GetUser request);
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        UserInfo Login(string userName, string passWord);
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool forgetPassword(forgetPassword request);

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool changePassword(changePassword request);
        /// <summary>
        /// 新增日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool AddLog(AddLog request);

        /// <summary>
        /// 获取系统日志列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<LogInfo> getLogList(getLogList request);
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<Role> GetRolesList(GetRolesList request);
    }
}
