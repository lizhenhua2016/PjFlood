using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dy.Common;

namespace GrassrootsFloodCtrl.Model.ZZTX
{
    /// <summary>
    /// 组织责任---行政区划信息
    /// </summary>
    public class ADCDInfo
    {
        [PrimaryKey]
        [AutoIncrement]
        [field("自增ID", "int", null, null)]
        public int Id { get; set; }

        [field("行政区划父id", "int", null, null)]
        public int parentId { get; set; }

        [field("行政区层级", "smallint", null, null)]
        public int grade { get; set; }
        
        [field("行政区划编码", "string", null, null)]
        [StringLength(15)]
        public string adcd { get; set; }

        [field("行政区划名称", "string", null, null)]
        [StringLength(50)]
        public string adnm { get; set; }

        [field("经度", "double", null, null)]
        [StringLength(15)]
        public double? lng { get; set; }

        [field("纬度", "double", null, null)]
        [StringLength(15)]
        public double? lat { get; set; }

        [field("创建时间", "DateTime", null, null)]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 操作日志 记录json字符串：反序列化为 List<operateLog> : operateLog:userName,operateMsg,operateTime
        /// </summary>
        [field("操作日志 记录json字符串：反序列化为 List<operateLog> : operateLog:userName,operateMsg,operateTime", "string", null, null)]
        public string operateLog { get; set; }
    }
}
