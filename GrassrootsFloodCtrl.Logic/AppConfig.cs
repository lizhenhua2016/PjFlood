using GrassrootsFloodCtrl.Logic.Common;
using ServiceStack.Configuration;


namespace GrassrootsFloodCtrl.Logic
{
    public class AppConfig
    {
        public AppConfig(IAppSettings appSettings)
        {
            Env = appSettings.Get<Env>("Env", Env.Dev);
            UploadSavePath = appSettings.Get<string>("UploadPath", "Uploads");
            VisitUrlPath = appSettings.Get<string>("VisitPath", "Uploads");
            SessionTimeout = appSettings.Get<int>("SessionTimeout", 60);
        }

        /// <summary>
        /// 当前开发环境
        /// </summary>
        public Env Env { get; private set; }

        /// <summary>
        /// Session过期时间
        /// </summary>
        public int SessionTimeout { get; private set; }

        /// <summary>
        /// 保存上传文件的跟目录，可以是本地目录，也可以是网络路径。
        /// </summary>
        public string UploadSavePath { get; set; }

        /// <summary>
        /// 访问上传文件的路径。可能由于CDN或分布式部署的原因，访问路径可能不一样。
        /// </summary>
        public string VisitUrlPath { get; set; }
    }
}