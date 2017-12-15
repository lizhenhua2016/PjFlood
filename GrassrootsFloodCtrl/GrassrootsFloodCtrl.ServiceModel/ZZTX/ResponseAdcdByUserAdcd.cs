using System;
using Dy.Common;
using ServiceStack.DataAnnotations;

namespace ServiceStackForLeafletjsResponse
{
   public class ResponseAdcdByUserAdcd
    {
        [PrimaryKey]
        [AutoIncrement]
        [field("自增ID", "int", null, null)]
        public int id { get; set; }
        [field("上级id", "int", null, null)]
        public int parentId { get; set; }
        [field("层级", "int", null, null)]
        public int grade { get; set; }
        [field("代码", "string", null, null)]
        public string adcd { get; set; }
        [field("名称", "string", null, null)]
        public string adnm { get; set; }
        [field("经度", "float", null, null)]
        public float lng { get; set; }
        [field("纬度", "float", null, null)]
        public float lat { get; set; }
        [field("操作日志", "string", null, null)]
        public string operateLog { get; set; }
        [field("添加时间", "DateTime", null, null)]
        public DateTime CreateTime { get; set; }
    }
}
