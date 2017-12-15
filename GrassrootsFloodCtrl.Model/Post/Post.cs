using Dy.Common;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.Post
{
   public class Post
    {
        [field("自增ID", "int", null, null)]
        [PrimaryKey]
        [AutoIncrement]
        public int ID { get; set; }

        [field("岗位名", "string", null, null)]
        [StringLength(50)]
        public string PostName { get; set; }

        [field("岗位类型", "string", null, null)]
        [StringLength(15)]
        public string PostType { get; set; }

        [field("所属地区", "string", null, null)]
        [StringLength(15)]
        public string ByADCD { get; set; }

        [field("操作日志", "string", null, null)]
        public string operateLog { get; set; }
        [field("序号", "int", null, null)]
        public int? orderId { get; set; }
    }
}
