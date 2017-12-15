using Dy.Common;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.Audit
{
   public class AuditCountyDetails
    {

        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int? ID { get; set; }

        [field("自增ID", "int", null, null)]
        public int? CountyID { get; set; }

        [field("审核者adcd", "string", null, null)]
        [StringLength(100)]
        public string AuditADCD { get; set; }

        [field("审核者adcd", "int", null, null)]
        public int? AuditRole { get; set; }

        [field("审核时间", "DateTime", null, null)]
        public DateTime? AuditTime { get; set; }

        [field("当前处理次数", "int", null, null)]
        public int? AuditNums { get; set; }

        [field("备注", "string", null, null)]
        public string Remarks { get; set; }
    }
}
