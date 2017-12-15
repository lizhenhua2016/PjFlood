using Dy.Common;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.Grid
{
   public class Grid
    {
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int ID { get; set; }

        [field("网格名", "string", null, null)]
        [StringLength(50)]
        public string GridName { get; set; }

        [field("网格类型", "string", null, null)]
        [StringLength(15)]
        public string GridType { get; set; }

        [field("所属地区", "string", null, null)]
        [StringLength(15)]
        public string ByADCD { get; set; }

        [field("操作日志", "string", null, null)]
        public string operateLog { get; set; }

        [field("职务名", "string", null, null)]
        public string PositionName { get; set; }
    }
}
