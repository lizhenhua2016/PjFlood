using Dy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace GrassrootsFloodCtrl.Model.Village
{
    /// <summary>
    /// 行政村防汛防台形势图
    /// </summary>
    public class VillagePic
    {
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [field("ADCD", "string", null, null)]
        [StringLength(50)]
        public string adcd { get; set; }
        /// <summary>
        /// 文件路径等json字符串 
        /// isSuccess是否上传成功 message上传结果反馈  fileName文件名称 flieSize文件大小 fileType文件类型 fileSrc文件路径 realName真实文件名
        /// </summary>
        [field("文件路径等json字符串", "string", null, null)]
        public string path { get; set; }

        [field("年度", "int", null, null)]
        public int Year { get; set; }

        [field("创建时间", "string", null, null)]
        public DateTime CreatTime { get; set; }
       
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
