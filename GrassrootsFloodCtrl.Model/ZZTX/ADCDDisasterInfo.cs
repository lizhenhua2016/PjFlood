using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dy.Common;
using ServiceStack.DataAnnotations;

namespace GrassrootsFloodCtrl.Model.ZZTX
{
    /// <summary>
    /// 受灾害影响行政区划的信息
    /// </summary>
    public class ADCDDisasterInfo
    {
        [PrimaryKey]
        [AutoIncrement]
        [field("自增ID", "int", null, null)]
        public int Id { get; set; }

        [field("行政区划编码", "string", null, null)]
        [StringLength(15)]
        public string adcd { get; set; }

        [field("总人口数", "double", null, null)]
        public double? TotalNum { get; set; }

        [field("受灾点", "double", null, null)]
        public double? PointNum { get; set; }

        [field("受灾害影响人口", "double", null, null)]
        public double? PopulationNum { get; set; }

        [field("年度", "int", null, null)]
        public int? Year { get; set; }

        [field("防汛防台任务轻重情况", "string", null, null)]
        [StringLength(50)]
        public string FXFTRW { get; set; }

        [field("创建时间", "DateTime", null, null)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 操作日志 记录json字符串：反序列化为 List<operateLog> : operateLog:userName,operateMsg,operateTime
        /// </summary>
        [field("操作日志 记录json字符串：反序列化为 List<operateLog> : operateLog:userName,operateMsg,operateTime", "string", null, null)]
        public string operateLog { get; set; }
    }
}
