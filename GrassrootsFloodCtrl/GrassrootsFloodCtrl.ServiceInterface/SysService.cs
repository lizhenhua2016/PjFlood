using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.ServiceModel.Sys;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.Logic.Sys;
using GrassrootsFloodCtrl.Model.Sys;
using ServiceStack;
using ServiceStack.Auth;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    /// <summary>
    /// 系统相关服务
    /// </summary>
    [Authenticate]
    public class SysService: ServiceBase
    {
        public ISysManager SysManager { get; set; }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<UserInfoViewModel> GET(GetUserInfoList request)
        {
            return SysManager.GetUserInfoList(request);
        }
        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool POST(SaveUserInfo request)
        {
            return SysManager.SaveUserInfo(request);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool Post(DelUser request)
        {
            return SysManager.DelUser(request.ids);
        }

        /// <summary>
        /// 根据添加查询单个用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public UserInfoViewModel GET(GetUserByIdOrUserName request)
        {
            return SysManager.GetUserByIdOrUserName(request);
        }
        /// <summary>
        /// 根据添加查询单个用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public UserInfo GET(GetUser request)
        {
            return SysManager.GetUser(request);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool POST(changePassword request)
         {
             return SysManager.changePassword(request);
         }
       

        
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<Role> GetRolesList(GetRolesList request)
        {
            return SysManager.GetRolesList(request);
        }

        /// <summary>
        /// 获取系统日志列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public BsTableDataSource<LogInfo> Get(getLogList request)
        {
            return SysManager.getLogList(request);
        }


        public bool Post(AddLog request)
        {
            return SysManager.AddLog(request);
        }
    }
}
