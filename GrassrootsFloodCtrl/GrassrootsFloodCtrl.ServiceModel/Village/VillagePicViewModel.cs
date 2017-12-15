using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace GrassrootsFloodCtrl.ServiceModel.Village
{
    /// <summary>
    /// 行政村防汛防台形势图VM
    /// </summary>
    public class VillagePicViewModel
    {


        /// <summary>
        /// 自增ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 行政区划编码
        /// </summary>
        public string adcd { get; set; }

        /// <summary>
        /// 行政区划名称
        /// </summary>
        public string adnm { get; set; }
        /// <summary>
        /// 文件路径等json字符串 
        /// isSuccess是否上传成功 message上传结果反馈  fileName文件名称 flieSize文件大小 fileType文件类型 fileSrc文件路径 realName真实文件名
        /// </summary>

        public string path { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatTime { get; set; }

            /// <summary>
            /// 操作日志 记录json字符串：反序列化为 List<operateLog> : operateLog:userName,operateMsg,operateTime
            /// </summary>
           
            public string operateLog { get; set; }
        
    }
}
