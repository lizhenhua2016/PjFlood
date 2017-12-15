using System;

namespace GrassrootsFloodCtrl.ServiceModel.Village
{
    public class VillageTransferPersonViewModel
    {
       /// <summary>
       /// 自增ID
       /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 行政区划编码
        /// </summary>
        public string adcd { get; set; }
        /// <summary>
        /// 行政区划名称
        /// </summary>
        public string adnm { get; set; }

        /// <summary>
        /// 危险区（点）名称
        /// </summary>
        public string DangerZoneName { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string DangerZoneType { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// 户主姓名
        /// </summary>
        public string HouseholderName { get; set; }

        /// <summary>
        /// 居住人数
        /// </summary>
        public int HouseholderNum { get; set; }

        /// <summary>
        /// 户主手机
        /// </summary>
        public string HouseholderMobile { get; set; }

        /// <summary>
        /// 责任人姓名
        /// </summary>
        public string PersonLiableName { get; set; }

        /// <summary>
        /// 责任人职务
        /// </summary>
        public string PersonLiablePost { get; set; }

        /// <summary>
        /// 责任人手机
        /// </summary>
        public string PersonLiableMobile { get; set; }

        /// <summary>
        /// 预警责任人姓名
        /// </summary>
        public string WarnPersonLiableName { get; set; }

        /// <summary>
        /// 预警责任人职务
        /// </summary>
        public string WarnPersonLiablePost { get; set; }

        /// <summary>
        /// 预警责任人手机
        /// </summary>
        public string WarnPersonLiableMobile { get; set; }

        /// <summary>
        /// 避灾场所名称
        /// </summary>
        public string DisasterPreventionName { get; set; }

        /// <summary>
        /// 有无安全鉴定
        /// </summary>
        public bool SafetyIdentification { get; set; }

        /// <summary>
        /// 避灾场所管理员
        /// </summary>
        public string DisasterPreventionManager { get; set; }

        /// <summary>
        /// 避灾场所管理员手机
        /// </summary>
        public string DisasterPreventionManagerMobile { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 操作日志 记录json字符串：反序列化为 List<operateLog> : operateLog:userName,operateMsg,operateTime
        /// </summary>
        public string operateLog { get; set; }

        public int? DId { get; set; }
        public int? IfTransfer { get; set; }
    }
}
