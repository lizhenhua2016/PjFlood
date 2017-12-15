using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dy.Common;

namespace GrassrootsFloodCtrl.Model.Sys
{
    /// <summary>
    /// 角色表
    /// </summary>
  public  class Role
    {
        [PrimaryKey]
        [AutoIncrement]
        [field("自增ID", "int", null, null)]
        public int Id { get; set; }

        [field("角色名称", "string", null, null)]
        [StringLength(50)]
        public string RoleName { get; set; }

        //[field("行政区划", "string", null, null)]
        //[StringLength(15)]
        //public string adcd { get; set; }

        /// <summary>
        /// 操作日志 记录json字符串：反序列化为 List<operateLog> : operateLog:userName,operateMsg,operateTime
        /// </summary>
        [field("操作日志 记录json字符串：反序列化为 List<operateLog> : operateLog:userName,operateMsg,operateTime", "string", null, null)]
        public string LastOeration { get; set; }
    }
}
