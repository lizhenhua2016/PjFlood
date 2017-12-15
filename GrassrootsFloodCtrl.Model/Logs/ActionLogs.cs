using Dy.Common;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.Logs
{
   public class ActionLogs
    {
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int? LogID { get; set; }

        [field("栏目名", "string", null, null)]
        [StringLength(100)]
        public string ColumnName { get; set; }

        [field("操作年份", "int", null, null)]
        public int Year { get; set; }

        [field("当前申请次数", "int", null, null)]
        public int AuditNums { get; set; }

        [field("旧数据", "string", null, null)]
        public string OldData { get; set; }

        [field("新数据", "string", null, null)]
        public string NewData { get; set; }

        [field("操作人", "string", null, null)]
        [StringLength(50)]
        public string AddUser { get; set; }

        [field("添加时间", "datetime", null, null)]
        public DateTime? AddTime { get; set; }



    }
}
