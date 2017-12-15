using Dy.Common;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.ZZTX
{
  public  class ADCDQRCode
    {
        [PrimaryKey]
        [AutoIncrement]
        [field("自增ID", "int", null, null)]
        public int ID { get; set; }

        [field("行政区划编码", "string", null, null)]
        [StringLength(50)]
        public string adcd { get; set; }

        [field("二维码图片路径", "string", null, null)]
        [StringLength(500)]
        public string qrpath { get; set; }

        [field("二维码名称", "string", null, null)]
        [StringLength(500)]
        public string qrname { get; set; }
    }
}
