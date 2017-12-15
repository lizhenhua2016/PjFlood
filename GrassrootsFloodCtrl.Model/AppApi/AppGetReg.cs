using Dy.Common;
using ServiceStack.DataAnnotations;

namespace GrassrootsFloodCtrl.Model.AppApi
{
    public class AppGetReg
    {
        [field("主键", "string", null, null)]
        [PrimaryKey]
        public string Id { get; set; }
        [field("注册手机","string",null,null)]
        public string Mobile { get; set; }
    }
}