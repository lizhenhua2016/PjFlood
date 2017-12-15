using System;
using GrassrootsFloodCtrl.Model.Sys;
using GrassrootsFloodCtrl.Model.Common;
using System.Collections.Generic;
using System.Web;
using Dy.Common;
using GrassrootsFloodCtrl.ServiceInterface;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Web;

namespace GrassrootsFloodCtrl.App_Start
{
    public class CustomCredentialsAuthProvider : CredentialsAuthProvider
    {
        public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
        {
            var authRepo = HostContext.Resolve<LoginService>();
            var user = authRepo.POST(new LoginUser() { UserName = userName, PassWord = password });
            if (user != null)
            {
                //这里没有对会话进行保存，同时这个返回true后会直接进入OnAuthenticated去验证，所以这里去掉
                //对会话的保存
                //var userSession = new UserSession();
                //SetSessionData(userSession, user);
                return true;
            }
            else
                return false;
        }

        public override IHttpResult OnAuthenticated(IServiceBase authService, IAuthSession session, IAuthTokens tokens, Dictionary<string, string> authInfo)
        {
            var authUser = HostContext.Resolve<SysService>().GET(new GetUser() { UserName = session.UserAuthName });
            if (authService != null)
            {
                var newSession = new UserSession();
                SetSessionData(newSession, authUser);
                //以前这个base.SessionExpiry是没有值的，这里重新定义一个时间
                // authService.SaveSession(session, base.SessionExpiry);
                authService.SaveSession(session, new TimeSpan(0, AppHost.AppConfig.SessionTimeout, 0));
            }
            return null;
        }

        public override bool IsAuthorized(IAuthSession session, IAuthTokens tokens, Authenticate request = null)
        {
            try
            {
                var pass = base.IsAuthorized(session, tokens, request);

                var user = HttpContext.Current.User;
                if (!pass && user.Identity.IsAuthenticated)
                {
                    if (request != null && user.Identity.Name != request.UserName)
                    {
                        return false;
                    }

                    if (session == null || string.IsNullOrEmpty(session.UserName))
                    {
                        var authUser =
                            HostContext.Resolve<SysService>().GET(new GetUser() { UserName = user.Identity.Name });
                        //这里是api接口访问授权严重，密码是否需要重新验证呢
                        //if (DESHelper.DESEncrypt(request.Password) != authUser.PassWord)
                        //    return false;
                        //else
                        //{
                        var newSession = new UserSession();
                        SetSessionData(newSession, authUser);

                        var authService = HostContext.ResolveService<AuthenticateService>();
                        //这里session保存没有添加时间   
                        // authService.SaveSession(newSession);
                        authService.SaveSession(newSession, new TimeSpan(0, AppHost.AppConfig.SessionTimeout, 0));
                        //}
                    }
                    pass = true;
                }

                return pass;
            }
            catch (Exception ex)
            {
                throw new Exception("系统异常，重新进入栏目!!!.");
            }
        }

        public static void SetSessionData(UserSession session, UserInfo user)
        {
            session.RealName = user.RealName;
            session.userId = user.Id;
            session.UserName = user.UserName;
            session.adcd = user.adcd;
            session.UserAuthId = user.Id.ToString();
            var userrole = HostContext.Resolve<SysService>().GET(new GetUserByIdOrUserName() { UserName = user.UserName });
            if (null != userrole)
            {
                session.RoleId = userrole.RoleID;
            }

            #region 经纬度
            
            var adcdinfo = HostContext.Resolve<ZZTXService>().GET(new GetADCDInfoByADCD() { adcd = user.adcd });
            if (adcdinfo != null && adcdinfo.lng != null && adcdinfo.lng != 0)
            {
                session.lng = adcdinfo.lng.ToString();
                session.lat = adcdinfo.lat.ToString();
            }
            else
            {
                adcdinfo = HostContext.Resolve<ZZTXService>().GET(new GetADCDInfoByADCD() { adcd = user.adcd.Substring(0, 6) + "000000000" });
                if (adcdinfo != null && adcdinfo.lng != null && adcdinfo.lng != 0)
                {
                    session.lng = adcdinfo.lng.ToString();
                    session.lat = adcdinfo.lat.ToString();
                }
                else
                {
                    adcdinfo = HostContext.Resolve<ZZTXService>().GET(new GetADCDInfoByADCD() { adcd = user.adcd.Substring(0, 4) + "00000000000" });
                    if (adcdinfo != null && adcdinfo.lng != null && adcdinfo.lng != 0)
                    {
                        session.lng = adcdinfo.lng.ToString();
                        session.lat = adcdinfo.lat.ToString();
                    }
                    else
                    {
                        session.lng = "120.15674";
                        session.lat = "30.27283";
                    }
                }
            }
            #endregion

            #region 审核
            using (var service = HostContext.ResolveService<AuditService>())
            {
                var info = service.GET(new GetAuditNumByADCD() { adcd = user.adcd });
                if (info != null)
                {
                    session.AuditStatus = info.Status;
                    session.AuditNums = info.AuditNums;
                }
            }
            #endregion
        }

    }
}