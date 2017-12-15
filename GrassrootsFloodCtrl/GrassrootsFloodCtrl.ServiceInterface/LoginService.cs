using GrassrootsFloodCtrl.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Logic.Sys;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack;

namespace GrassrootsFloodCtrl.ServiceInterface
{
   public class LoginService: Service
    {
        public ISysManager SysManager { get; set; }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public UserInfo POST(LoginUser request)
        {
            return SysManager.Login(request.UserName, request.PassWord);
        }

        /// <summary>
        /// 根据用户名称获取登录次数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int Get(GetUserLoginNum request)
        {
            return SysManager.GetUserLoginNum(request);
        }

        public bool Get(SendMessgae request)
        {
            return SysManager.SendMessgae(request);
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool Post(forgetPassword request)
        {
            return SysManager.forgetPassword(request);
        }
    }
}
