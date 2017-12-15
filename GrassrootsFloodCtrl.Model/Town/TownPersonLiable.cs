using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dy.Common;
using ServiceStack.DataAnnotations;

namespace GrassrootsFloodCtrl.Model.Town
{
    /// <summary>
    /// 镇街责任人
    /// </summary>
    public class TownPersonLiable
    {
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [field("ADCD", "string", null, null)]
        [StringLength(50)]
        public string adcd { get; set; }
        [field("岗位", "string", null, null)]
        [StringLength(50)]
        public string Position { get; set; }
         [field("姓名", "string", null, null)]
        [StringLength(50)]
        public string Name { get; set; }
        [field("职务", "string", null, null)]
        [StringLength(50)]
        public string Post { get; set; }
        [field("手机", "string", null, null)]
        [StringLength(11)]
        public string Mobile { get; set; }
        [field("备注", "string", null, null)]
        [StringLength(500)]
        public string Remark { get; set; }
        [field("年度", "int", null, null)]
        public int Year { get; set; }

        [field("创建时间", "DateTime", null, null)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 操作日志 记录json字符串：反序列化为 List<operateLog> : operateLog:userName,operateMsg,operateTime
        /// </summary>
        [field("操作日志 记录json字符串：反序列化为 List<operateLog> : operateLog:userName,operateMsg,operateTime", "string", null, null)]
        public string operateLog { get; set; }

        #region 申报更新记录
        [field("审核申请修改次数", "int", null, null)]
        public int? AuditNums { get; set; }

        [field("旧数据", "string", null, null)]
        public string OldData { get; set; }

        [field("新数据", "string", null, null)]
        public string NewData { get; set; }
        #endregion
    }
}
