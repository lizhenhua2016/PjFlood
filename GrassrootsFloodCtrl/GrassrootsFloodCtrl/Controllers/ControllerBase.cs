using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.Sys;
using GrassrootsFloodCtrl.ServiceInterface;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Mvc;
using ServiceStack.Logging;
using ServiceStack.Caching;

namespace GrassrootsFloodCtrl.Controllers
{
    [Authorize]
    public class ControllerBase : ServiceStackController<UserSession>
    {
        /// <summary>
        /// Api根目录
        /// </summary>
        public string ApplicationPath
        {
            get { return "/api/"; }
        }

        protected string UserName
        {
            get { return User.Identity.Name; }
        }

        private int _userId;

        protected int UserId
        {
            get
            {
                if (_userId > 0)
                    return _userId;

                if (UserSession != null && !string.IsNullOrEmpty(UserSession.UserAuthId))
                    _userId = UserSession.UserAuthId.ToInt(0);

                return _userId;
            }
            private set
            {
                _userId = value;
            }
        }

        private string _adcd;
        protected string adcd
        {
            get {
               return _adcd = string.IsNullOrEmpty(UserSession.adcd)?"":UserSession.adcd;
            }
            set {
                _adcd = value;
            }
        }
        private int? _roleid;
        protected int? RoleID
        {
            get { return _roleid= UserSession.RoleId; }
            set { _roleid = value; }
        }
        private int? _AuditNums;
        protected int? AuditNums
        {
            get { return _AuditNums = UserSession.AuditNums; }
            set { _AuditNums = value; }
        }

        private int? _AuditStatus;
        protected int? AuditStatus
        {
            get { return _AuditStatus = UserSession.AuditStatus; }
            set { _AuditStatus = value; }
        }
        private string _lng;
        protected string lng
        {
            get
            {
                return _lng = string.IsNullOrEmpty(UserSession.lng) ? "" : UserSession.lng;
            }
            set
            {
                _lng = value;
            }
        }

        private string _lat;
        protected string lat
        {
            get
            {
                return _lat = string.IsNullOrEmpty(UserSession.lat) ? "" : UserSession.lat;
            }
            set
            {
                _lat = value;
            }
        }
        protected ILog Log { get { return LogManager.LogFactory.GetLogger("Controller"); } }

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            try
            {
                //这里引用mvc的过滤器似乎没有起到什么作用。
                base.OnAuthentication(filterContext);
                //这里不应该用验证UserSession.UserAuthId是否为空，
                //因为采用的是接口授权的方式，servicestack的只对api的访问调用进行了授权访问，
                //而在原生态的mvc里面如果要验证就需要用另外的方式去操作，这里使用了servicestackcontrol来操作
                //if (User.Identity.IsAuthenticated && string.IsNullOrEmpty(UserSession.UserAuthId))
                //{
                //    Log.Debug("UserSession失效，自动登录。");

                //    ReCreateAuthSession();
                //}
                //新的验证是ControllerBase里面要初始化变量
                if (User.Identity.IsAuthenticated && string.IsNullOrEmpty(UserSession.UserName))
                {
                    Log.Debug("UserSession失效，自动登录。");
                    using (var authService = HostContext.ResolveService<AuthenticateService>(HttpContext))
                    {
                        var _session = (UserSession)authService.GetSession(false);
                        ReCreateAuthSessionNew(_session);
                        authService.SaveSession(_session, new TimeSpan(0,AppHost.AppConfig.SessionTimeout,0));
                    }
                }
                //else
                //{
                //    filterContext.Result = new RedirectResult("/Account/Login");
                //}
            }
            catch(Exception ex)
            {
                
            }
        }

        /// <summary>
        /// 重新创建会话
        /// </summary>
        private void ReCreateAuthSessionNew(UserSession session)
        {
                var userAuth =  HostContext.Resolve<SysService>().GET(new GetUser() { UserName = UserName });
                if (userAuth != null)
                {
                    session.RealName = userAuth.RealName;
                    session.IsAuthenticated = true;
                    session.UserName = userAuth.UserName;
                    session.userId= userAuth.Id;
                    session.adcd = userAuth.adcd;
                    UserId = userAuth.Id;
                    #region 经纬度

                    using (var service= HostContext.Resolve<SysService>())
                    {
                        var userrole=service.GET(new GetUserByIdOrUserName() { UserName = UserName });
                        if (null != userrole)
                        {
                            session.RoleId = userrole.RoleID;
                        }
                        //
                        if (userrole.RoleID == 3 || userrole.RoleID == 4)
                        {
                            var useraudtinum = HostContext.Resolve<AuditService>().GET(new GetAuditNumByADCD());
                            if (null != useraudtinum)
                            {
                                session.AuditNums = useraudtinum.AuditNums;
                                session.AuditStatus = useraudtinum.Status;
                            }
                        }
                        var adcdinfo = HostContext.Resolve<ZZTXService>().GET(new GetADCDInfoByADCD() { adcd = userAuth.adcd });
                        if (adcdinfo != null && adcdinfo.lng != null && adcdinfo.lng != 0)
                        {
                            session.lng = adcdinfo.lng.ToString();
                            session.lat = adcdinfo.lat.ToString();
                        }
                        else
                        {
                            adcdinfo = HostContext.Resolve<ZZTXService>().GET(new GetADCDInfoByADCD() { adcd = userAuth.adcd.Substring(0, 6) + "000000000" });
                            if (adcdinfo != null && adcdinfo.lng != null && adcdinfo.lng != 0)
                            {
                                session.lng = adcdinfo.lng.ToString();
                                session.lat = adcdinfo.lat.ToString();
                            }
                            else
                            {
                                adcdinfo = HostContext.Resolve<ZZTXService>().GET(new GetADCDInfoByADCD() { adcd = userAuth.adcd.Substring(0, 4) + "00000000000" });
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
                    }
                    #endregion
                }
        }
        /// <summary>
        /// 重新创建会话
        /// </summary>
        private void ReCreateAuthSession()
        {
            using (var authService = HostContext.ResolveService<AuthenticateService>(HttpContext))
            {
                var userAuth =
                       HostContext.Resolve<SysService>().GET(new GetUser() { UserName = UserName });
                if (userAuth != null)
                {
                    var session = (UserSession)authService.GetSession(false);
                    session.RealName = userAuth.RealName;
                    session.IsAuthenticated = true;
                    session.UserName = userAuth.UserName;
                    session.userId = userAuth.Id;
                    session.adcd = userAuth.adcd;
                    UserId = userAuth.Id;
                    #region 经纬度

                    using (var service = HostContext.Resolve<SysService>())
                    {
                        var userrole = service.GET(new GetUserByIdOrUserName() { UserName = UserName });
                        if (null != userrole)
                        {
                            session.RoleId = userrole.RoleID;
                        }
                        //
                        if (userrole.RoleID == 3 || userrole.RoleID == 4)
                        {
                            var useraudtinum = HostContext.Resolve<AuditService>().GET(new GetAuditNumByADCD());
                            if (null != useraudtinum)
                            {
                                session.AuditNums = useraudtinum.AuditNums;
                                session.AuditStatus = useraudtinum.Status;
                            }
                        }
                        var adcdinfo = HostContext.Resolve<ZZTXService>().GET(new GetADCDInfoByADCD() { adcd = userAuth.adcd });
                        if (adcdinfo != null && adcdinfo.lng != null && adcdinfo.lng != 0)
                        {
                            session.lng = adcdinfo.lng.ToString();
                            session.lat = adcdinfo.lat.ToString();
                        }
                        else
                        {
                            adcdinfo = HostContext.Resolve<ZZTXService>().GET(new GetADCDInfoByADCD() { adcd = userAuth.adcd.Substring(0, 6) + "000000000" });
                            if (adcdinfo != null && adcdinfo.lng != null && adcdinfo.lng != 0)
                            {
                                session.lng = adcdinfo.lng.ToString();
                                session.lat = adcdinfo.lat.ToString();
                            }
                            else
                            {
                                adcdinfo = HostContext.Resolve<ZZTXService>().GET(new GetADCDInfoByADCD() { adcd = userAuth.adcd.Substring(0, 4) + "00000000000" });
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
                    }

                    #endregion

                    authService.SaveSession(session, new TimeSpan(0, AppHost.AppConfig.SessionTimeout, 0));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                ViewBag.RealName = UserSession.RealName;
                ViewBag.UserName = UserSession.UserName;
                ViewBag.RoleId = UserSession.RoleId;
            }
            base.OnResultExecuting(filterContext);
        }
    }
}