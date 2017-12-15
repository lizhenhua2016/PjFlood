using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.SumAppUser;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Route("/sumAppUser/list", "Get", Summary = "获取APP")]
    [Api("统计APP用户")]
    public class RouteSumAppUser:IReturn<List<SelectSumAppUserList>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "adcdName")]
        public string adcdName { get; set; }
    }
    [Route("/sumAppUser/list2", "Get", Summary = "获取APP")]
    [Api("统计APP用户")]
    public class RouteSumAppUser2 : IReturn<List<SelectSumAppUserList>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "adcdName")]
        public string adcdName { get; set; }
    }
    [Route("/sumAppUser/noInfo", "Get", Summary = "获取未注册人的信息")]
    public class RouteNoAppUser : PageQuery,
        IReturn<BsTableDataSource<Model.SumAppUser.SumAppUser>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
    }
}
