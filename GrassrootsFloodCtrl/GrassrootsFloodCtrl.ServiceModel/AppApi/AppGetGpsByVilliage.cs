using Dy.Common;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppGetGpsByVilliage
    {
        [field("自增ID", "Guid", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public string Id { get; set; }

        [field("消息ID", "Guid", null, null)]
        public string MessageId { get; set; }

        [field("用户adcd", "string", null, null)]
        public string adcd { get; set; }

        [field("用户账号", "string", null, null)]
        public string username { get; set; }

        [field("岗位类型", "string", null, null)]
        public string postTypecode { get; set; }

        [field("补填时间", "string", null, null)]
        public string postTime { get; set; }

        [field("岗位名", "string", null, null)]
        public string postCode { get; set; }

        [field("网格名", "string", null, null)]
        public string gridType { get; set; }

        [field("接到任务步骤", "string", null, null)]
        public string step { get; set; }

        [field("对应步骤操作结果", "string", null, null)]
        public string values { get; set; }

        [field("接到任务步骤选项(主要针对村级责任人)", "string", null, null)]
        public string stepItem { get; set; }

        [field("接到任务步骤选项(主要针对村级责任人),其他选项内容", "string", null, null)]
        public string valuesItem { get; set; }

        [field("app所在经纬度轨迹", "string", null, null)]
        public string location { get; set; }

        [field("附件名", "string", null, null)]
        public string fileName { get; set; }

        [field("拍照等附件", "string", null, null)]
        public string file { get; set; }

        [field("新增时间", "datetime", null, null)]
        public DateTime addtime { get; set; }

        [field("是否补填", "bit", null, null)]
        public bool ifFillIn { get; set; }

        [field("是否履职", "bool", null, null)]
        public bool IsResumption { get; set; }
    }
}

