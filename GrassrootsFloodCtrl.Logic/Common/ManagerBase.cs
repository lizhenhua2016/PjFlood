using ServiceStack;
using ServiceStack.Caching;
using System;
using System.Data;
using GrassrootsFloodCtrl.Model.Common;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace GrassrootsFloodCtrl.Logic
{
    public class ManagerBase
    {
        private int? _userid;
        private string _adcd;
        private string _realName;
        private string _userName;
        private int? _rowid;
        private int? _auditnums;
        /// <summary>
        /// 用户ID
        /// </summary>
        public int? UserId
        {
            get
            {
                var session = HostContext.AppHost.Resolve<ICacheClient>().SessionAs<UserSession>();
                if (null != session && session.userId > 0) return _userid = Convert.ToInt32(session.userId);
                else return _userid; 
                    //return _userid ?? Convert.ToInt32(session.userId);    
            }
            set
            {
                _userid = value;
            }
        }
        /// <summary>
        /// 当前用户所属的行政区划编码
        /// </summary>
        public string adcd
        {
            get
            {
                var session = HostContext.AppHost.Resolve<ICacheClient>().SessionAs<UserSession>();
                if (null != session && session.adcd != "")
                {
                    return session.adcd;
                }
                else
                {
                    return "";
                }
                //TODO:浦江县待修改
                //return "331081101000000";//测试值 浦江县adcd
            }
            set
            {
                _adcd = value;
            }
        }
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string RealName
        {
            get
            {
                var session = HostContext.AppHost.Resolve<ICacheClient>().SessionAs<UserSession>();
                if (null != session && session.RealName != "") return session.RealName;
                return "";
            }
            set
            {
                _realName = value;
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get
            {
                var session = HostContext.AppHost.Resolve<ICacheClient>().SessionAs<UserSession>();
                return session.UserName;
            }
            set
            {
                _userName = value;
            }
        }
        public int? RowID
        {
            get {
                var session = HostContext.AppHost.Resolve<ICacheClient>().SessionAs<UserSession>();
                return session.RoleId;
            }
            set { _rowid = value; }
        }

        public int? AuditNums
        {
            get
            {
                var session = HostContext.AppHost.Resolve<ICacheClient>().SessionAs<UserSession>();
                return session.AuditNums;
            }
            set { _auditnums = value; }
        }

        public IDbConnectionFactory DbFactory { get; set; }

        public AppConfig AppConfig { get; set; }

        protected IDbConnection GetConnection()
        {
            return DbFactory.Open();
        }

    }
}