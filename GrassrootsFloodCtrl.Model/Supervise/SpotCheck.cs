using Dy.Common;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.Supervise
{
   public class SpotCheck
    {
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int id { get; set; }
        
        [field("adcd", "string", null, null)]
        [StringLength(50)]
        public string adcd { get; set; }
        
        [field("被抽查地区", "string", null, null)]
        [StringLength(50)]
        public string areas { get; set; }

        [field("抽查人id", "int", null, null)]
        public int uid { get; set; }

        [field("抽查人", "string", null, null)]
        [StringLength(50)]
        public string checkman { get; set; }

        [field("抽查人realname", "string", null, null)]
        [StringLength(50)]
        public string checkmanrealname { get; set; }
        

        [field("抽查时间", "string", null, null)]
        public DateTime checktime { get; set; }

        [field("被抽查人", "string", null, null)]
        [StringLength(50)]
        public string bycheckman { get; set; }

        [field("被抽查人手机", "string", null, null)]
        [StringLength(50)]
        public string bycheckphone { get; set; }

        [field("不合格项", "string", null, null)]
        [StringLength(50)]
        public string checkitems { get; set; }

        [field("不合格项其他描述", "string", null, null)]
        [StringLength(500)]
        public string noremarks { get; set; }

        [field("备注", "string", null, null)]
        [StringLength(500)]
        public string remarks { get; set; }

        [field("合格不合格", "string", null, null)]
        [StringLength(50)]
        public string checkstatus { get; set; }

        [field("日志", "string", null, null)]
        public string operateLog { get; set; }

        [field("抽查年份", "string", null, null)]
        [StringLength(50)]
        public int year { get; set; }
    }
}
