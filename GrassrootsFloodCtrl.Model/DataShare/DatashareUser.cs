
using Dy.Common;
using ServiceStack.DataAnnotations;
namespace GrassrootsFloodCtrl.Model.DataShare
{
    public class DatashareUser
    {
        [PrimaryKey]
        [AutoIncrement]
        [field("自增ID", "int", null, null)]
        public int Id { get; set; }

        [field("用户名", "string", null, null)]
        [StringLength(50)]
        public string UserName { get; set; }

        [field("密码", "string", null, null)]
        [StringLength(50)]
        public string PassWord { get; set; }

        [field("行政区划", "string", null, null)]
        [StringLength(15)]
        public string adcd { get; set; }
    }
}
