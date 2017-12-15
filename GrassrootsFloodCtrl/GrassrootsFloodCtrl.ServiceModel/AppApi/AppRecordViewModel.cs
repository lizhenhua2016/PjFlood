using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
   public class AppRecordViewModel
    {
      
        public string token { get; set; }

        //[field("岗位类型", "string", null, null)]
        public string postTypecode { get; set; }

        //[field("岗位名", "string", null, null)]
        public string postCode { get; set; }

        //[field("网格名", "string", null, null)]
        public string gridType { get; set; }

       // [field("接到任务步骤", "string", null, null)]
        public string step { get; set; }
        
        //[field("对应步骤操作结果", "string", null, null)]
        public string values { get; set; }

        //接到任务步骤选项(主要针对村级责任人)
        public string stepItem { get; set; }
        public string valuesItem { get; set; }

        // [field("附件名", "string", null, null)]
        public string fileName { get; set; }

       // [field("拍照等附件", "string", null, null)]
        public string file { get; set; }

        //[field("新增时间", "datetime", null, null)]
        public DateTime addtime { get; set; }

        //[field("是否不填", "bit", null, null)]
        public bool ifFillIn { get; set; }
    }
}
