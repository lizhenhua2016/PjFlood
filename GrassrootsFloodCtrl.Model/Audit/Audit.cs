using Dy.Common;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.Audit
{
   public class Audit
    {
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int? ID { get; set; }

        [field("年份", "int", null, null)]
        public int? Year { get; set; }

        [field("镇adcd", "string", null, null)]
        [StringLength(100)]
        public string TownADCD { get; set; }

        [field("提交时间", "datetime", null, null)]
        public DateTime? TownAddTime { get; set; }

        [field("县审核提交时间", "datetime", null, null)]
        public DateTime? CountyAuditTime { get; set; }

        [field("市审核提交时间", "datetime", null, null)]
        public DateTime? CityAuditTime { get; set; }

        [field("审核状态:0未提交,1县待审,2县已审,3市已批", "int", null, null)]
        public int? Status { get; set; }

        [field("乡镇申请次数", "int", null, null)]
        public int? AuditNums { get; set; }

        /// <summary>
        /// 操作日志 记录json字符串：反序列化为 List<operateLog> : operateLog:userName,operateMsg,operateTime
        /// </summary>
        [field("操作日志 记录json字符串：反序列化为 List<operateLog> : operateLog:userName,operateMsg,operateTime", "string", null, null)]
        public string operateLog { get; set; }
    }
}
