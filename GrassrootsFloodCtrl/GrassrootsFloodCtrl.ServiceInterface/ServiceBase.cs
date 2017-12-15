using GrassrootsFloodCtrl.Model.Common;
using ServiceStack;
using ServiceStack.Caching;
using System;


namespace GrassrootsFloodCtrl.ServiceInterface
{
    public class ServiceBase : Service
    {
        private int? _userid;
        private string _realName;
        private string _adcd;

        /// <summary>
        /// 用户ID
        /// </summary>
        public int? UserId
        {
            get
            {
                if (null != GetSession(false).UserAuthId) return _userid ?? Convert.ToInt32(GetSession(false).UserAuthId);
                else return _userid;
            }
            set
            {
                _userid = value;
            }
        }

        /// <summary>
        /// 真实名字
        /// </summary>
        public string RealName
        {
            get
            {
                var session = HostContext.AppHost.Resolve<ICacheClient>().SessionAs<UserSession>();
                return session.RealName;
            }
            set
            {
                _realName = value;
            }
        }
        /// <summary>
        ///用户所属行政区划编码
        /// </summary>
        public string adcd
        {
            get
            {
                var session = HostContext.AppHost.Resolve<ICacheClient>().SessionAs<UserSession>();
                return session.adcd;
            }
            set
            {
                _adcd = value;
            }
        }
    }
}