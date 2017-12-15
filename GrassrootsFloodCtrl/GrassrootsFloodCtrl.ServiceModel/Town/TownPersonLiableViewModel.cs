using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Town
{
    /// <summary>
    /// 乡镇责任人VM
    /// </summary>
   public class TownPersonLiableViewModel
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public int Id { get; set; }

      /// <summary>
      /// 镇街编码
      /// </summary>
        public string adcd { get; set; }
        /// <summary>
        /// 镇街名称
        /// </summary>
        public string adnm { get; set; }

        /// <summary>
      /// 岗位
      /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string Post { get; set; }
    /// <summary>
    /// 手机号码
    /// </summary>
        public string Mobile { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
        public string Remark { get; set; }
     /// <summary>
     /// 年度
     /// </summary>
        public int Year { get; set; }

        public string adnmparent { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
        public DateTime? CreateTime { get; set; }
        public string checkresult { get; set; }
        /// <summary>
        /// 操作日志 记录json字符串：反序列化为 List<operateLog> : operateLog:userName,operateMsg,operateTime
        /// </summary>
        public string operateLog { get; set; }
    }
}
