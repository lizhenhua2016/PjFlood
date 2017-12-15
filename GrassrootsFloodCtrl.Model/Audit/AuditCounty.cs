using Dy.Common;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.Audit
{
   public class AuditCounty
    {
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int? ID { get; set; }

        [field("年份", "int", null, null)]
        public int Year { get; set; }

        [field("县adcd", "string", null, null)]
        [StringLength(100)]
        public string CountyADCD { get; set; }

        [field("县提交时间", "datetime", null, null)]
        public DateTime? CountyAddTime { get; set; }

        [field("市审核提交时间", "datetime", null, null)]
        public DateTime? CityAuditTime { get; set; }

        [field("审核状态:0审批未过,1待审,2已审", "int", null, null)]
        public int Status { get; set; }

        [field("申请次数", "int", null, null)]
        public int? AuditNums { get; set; }

        /// <summary>
        /// 操作日志 记录json字符串：反序列化为 List<operateLog> : operateLog:userName,operateMsg,operateTime
        /// </summary>
        [field("操作日志 记录json字符串：反序列化为 List<operateLog> : operateLog:userName,operateMsg,operateTime", "string", null, null)]
        public string operateLog { get; set; }
    }
}
