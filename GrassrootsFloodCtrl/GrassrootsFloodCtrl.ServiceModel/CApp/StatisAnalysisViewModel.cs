using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.CApp
{
   public class StatisAnalysisViewModel
    {
        //市级
        public string cityName { get; set; }
        //县级
        public string countyName { get; set; }
        //乡镇总数
        public int? townNum { get; set; }
        //县已审市已批
        public int? ApprovalStatus3 { get; set; }
        //县已审市未批 
        public int? ApprovalStatus2 { get; set; }
        //县未审 
        public int? ApprovalStatus1 { get; set; }
        //市审批不通过
        public int? ApprovalStatusN1 { get; set; }
        //县审批不通过 
        public int? ApprovalStatus0 { get; set; }
        //存在未提交审核乡镇
        public int? NoApprovalStatus { get; set; }
        //校核
        public int? ApprovalStatusNum { get; set; }
        //合并序号
        public string numstr { get; set; }

    }
}
