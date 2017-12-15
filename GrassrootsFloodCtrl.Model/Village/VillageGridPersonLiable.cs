using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Auth;
using ServiceStack.DataAnnotations;
using Dy.Common;

namespace GrassrootsFloodCtrl.Model.Village
{
    /// <summary>
    /// 村:网格责任人表结构
    /// </summary>
   public class VillageGridPersonLiable
    {
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int ID { get; set; }

        [field("村ADCD", "string", null, null)]
        [StringLength(50)]
        public string VillageADCD { get; set; }

        [field("村网格名", "string", null, null)]
        [StringLength(50)]
        public string VillageGridName { get; set; }

        [field("网格名", "string", null, null)]
        [StringLength(50)]
        public string GridName { get; set; }
        
        [field("责任人", "string", null, null)]
        [StringLength(50)]
        public string PersonLiable { get; set; }
        
        [field("职位", "string", null, null)]
        [StringLength(50)]
        public string Position { get; set; }

        [field("手机", "string", null, null)]
        [StringLength(15)]
        public string HandPhone { get; set; }

        [field("备注", "string", null, null)]
        [StringLength(500)]
        public string Remarks { get; set; }

        [field("添加时间", "datetime", null, null)]
        public DateTime? AddTime { get; set; }

        [field("编辑时间", "datetime", null, null)]
        public DateTime? EditTime { get; set; }

        [field("操作日志", "string", null, null)]
        public string operateLog { get; set; }

        [field("所属年份", "int", null, null)]
        public int? Year { get; set; }

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
