using Dy.Common;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;

namespace GrassrootsFloodCtrl.Model.Sys
{

    /// <summary>
    /// 系统操作日志记录(增、删、改)
    /// </summary>
    public class LogInfo
    {
        [PrimaryKey]
        [AutoIncrement]
        [field("自增Id", "int",null,null)]
        public int Id { get; set; }

        [field("操作类型", "enums", null, null)]
        public OperationTypeEnums OperationType { get; set; }

        [field("操作时间", "DateTime",null,null)]
        public DateTime tm { get; set; }
        [field("行政区划", "string", null, null)]
        [StringLength(20)]
        public string adcd { get; set; }
        [field("用户名", "string",null,null)]
        [StringLength(50)]
        public string UserName { get; set; }

        [field("真实姓名", "string",null,null)]
        [StringLength(50)]
        public string RealName { get; set; }

        /// <summary>
        /// 操作日志 记录json字符串：operateLog : operateLog:userName,operateMsg,operateTime
        /// </summary>
        [field("操作日志 记录json字符串:operateLog : operateLog:userName,operateMsg,operateTime", "string",null,null)]
        public string Operation { get; set; }
    }
}
