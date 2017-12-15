using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dy.Common;
using ServiceStack.DataAnnotations;

namespace GrassrootsFloodCtrl.Model.Village
{
    /// <summary>
    /// 行政村危险区转移人员
    /// </summary>
    public class VillageTransferPerson
    {
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int ID { get; set; }

        [field("adcd", "string", null, null)]
        [StringLength(15)]
        public string adcd { get; set; }

        [field("危险区（点）名称", "string", null, null)]
        [StringLength(200)]
        public string DangerZoneName { get; set; }

        [field("类别", "string", null, null)]
        [StringLength(50)]
        public string DangerZoneType { get; set; }

        [field("位置", "string", null, null)]
        [StringLength(200)]
        public string Position { get; set; }

        [field("经度", "double", null, null)]
        public double Lng { get; set; }

        [field("纬度", "double", null, null)]
        public double Lat { get; set; }

        [field("户主姓名", "string", null, null)]
        [StringLength(50)]
        public string HouseholderName { get; set; }

        [field("居住人数", "int", null, null)]
        public int HouseholderNum { get; set; }

        [field("户主手机", "string", null, null)]
        [StringLength(15)]
        public string HouseholderMobile { get; set; }

        [field("责任人姓名", "string", null, null)]
        [StringLength(50)]
        public string PersonLiableName { get; set; }

        [field("责任人职务", "string", null, null)]
        [StringLength(20)]
        public string PersonLiablePost{ get; set; }

        [field("责任人手机", "string", null, null)]
        [StringLength(15)]
        public string PersonLiableMobile { get; set; }

        [field("预警责任人姓名", "string", null, null)]
        [StringLength(11)]
        public string WarnPersonLiableName { get; set; }

        [field("预警责任人职务", "string", null, null)]
        [StringLength(20)]
        public string WarnPersonLiablePost { get; set; }

        [field("预警责任人手机", "string", null, null)]
        [StringLength(15)]
        public string WarnPersonLiableMobile { get; set; }

        [field("避灾场所名称", "string", null, null)]
        [StringLength(50)]
        public string DisasterPreventionName { get; set; }

        [field("有无安全鉴定", "bool", null, null)]
        public bool SafetyIdentification { get; set; }

        [field("避灾场所管理员", "bool", null, null)]
        [StringLength(20)]
        public string DisasterPreventionManager{ get; set; }

        [field("避灾场所管理员手机", "string", null, null)]
        [StringLength(15)]
        public string DisasterPreventionManagerMobile { get; set; }

        [field("年度", "int", null, null)]
        public int Year { get; set; }

        [field("备注", "string", null, null)]
        [StringLength(500)]
        public string Remark { get; set; }

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
        #region 无转移人员状态
        [field("无转移人员状态", "int", null, null)]
        public int? IfTransfer { get; set; }
        #endregion
    }
}
