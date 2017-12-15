using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dy.Common;
using ServiceStack.DataAnnotations;

namespace GrassrootsFloodCtrl.Model.DangerZone
{
    /// <summary>
    /// 危险点（区）类型
    /// </summary>
   public class DangerZone
    {
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        [field("危险点类型名称", "string", null, null)]
        [StringLength(50)]
        public string DangerZoneName { get; set; }

        [field("所属地区编码", "string", null, null)]
        [StringLength(15)]
        public string adcd { get; set; }

        [field("操作日志", "string", null, null)]
        public string operateLog { get; set; }
    }
}
