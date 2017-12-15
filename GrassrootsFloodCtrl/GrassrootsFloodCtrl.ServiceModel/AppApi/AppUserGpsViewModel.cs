using Dy.Common;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppUserGpsViewModel
    {
        [field("用户adcd", "string", null, null)]
        public string adcd { get; set; }

        [field("用户账号", "string", null, null)]
        public string username { get; set; }

        [field("app所在经纬度轨迹", "string", null, null)]
        public string location { get; set; }
    }
}