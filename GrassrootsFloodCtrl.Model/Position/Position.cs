using Dy.Common;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.Position
{
   public class Position
    {
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int ID { get; set; }

        [field("职位名", "string", null, null)]
        [StringLength(50)]
        public string PositionName { get; set; }

        [field("职位类型", "string", null, null)]
        [StringLength(50)]
        public string PositionType { get; set; }

        [field("所属地区", "string", null, null)]
        [StringLength(15)]
        public string ByADCD { get; set; }

        [field("操作日志", "string", null, null)]
        public string operateLog { get; set; }

        [field("序号", "int", null, null)]
        public int? orderId { get; set; }
        
    }
}
