using Dy.Common;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.CountryPerson
{
    /// <summary>
    ///县级人员体系类
    /// </summary>
    public class CountryPerson
    {
        [PrimaryKey]
        [AutoIncrement]
        [field("自增ID", "int", null, null)]
        public int Id { get; set; }

        [field("岗位", "string", null, null)]
        [StringLength(50)]
        public string Position { get; set; }

        [field("姓名", "string", null, null)]
        [StringLength(50)]
        public string UserName { get; set; }

        [field("职务", "string", null, null)]
        [StringLength(50)]
        public string Post { get; set; }

        [field("电话", "string", null, null)]
        [StringLength(50)]
        public string Phone { get; set; }

        [field("标记", "string", null, null)]
        [StringLength(500)]
        public string Remark { get; set; }

        [field("导入的年份", "int", null, null)]
        public int Year { get; set; }

        [field("创建的时间", "DateTime", null, null)]
        public DateTime CreateTime { get; set; }

        [field("创建的人", "string", null, null)]
        [StringLength(24)]
        public string CreateName { get; set; }

        [field("更改的时间", "DateTime", null, null)]
        public DateTime UpdateTime { get; set; }

        [field("更改人的名字", "string", null, null)]
        [StringLength(50)]
        public string UpdateName { get; set; }


        [field("县的名字", "string", null, null)]
        [StringLength(50)]
        public string Country { get; set; }

        [field("县级编码", "string", null, null)]
        [StringLength(50)]
        public string adcd { get; set; }

        [field("上一次得数据", "string", null, null)]
        public string OldData { get; set; }

        [field("新的得数据", "string", null, null)]
        public string NewData { get; set; }

        [field("次数", "string", null, null)]
        public int AuditNums { get; set; }

    }
}
