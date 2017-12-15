using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppUserInfoViewModel
    {
        public string UserName { get; set; }

        public string RealName { get; set; }

        public string adcd { get; set; }

        public List<postInfo> Posts { get; set; }
    }
    public class postInfo
    {
        //岗位类型
        public string postTypecode { get; set; }
        //岗位
        public string postCode { get; set; }
        //网格类型
        public string gridType { get; set; }
        //转移人员
        public int? transferNum { get; set; }
    }

}
