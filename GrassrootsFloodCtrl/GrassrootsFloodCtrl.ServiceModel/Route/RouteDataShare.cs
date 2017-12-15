using GrassrootsFloodCtrl.ServiceModel.Country;
using GrassrootsFloodCtrl.ServiceModel.DataShare;
using GrassrootsFloodCtrl.ServiceModel.Sys;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.ServiceModel.Village;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Route("/DataShare/CountyPersLiableList", "POST", Summary = "接口：县级责任人")]
    [Api("数据共享")]
    public class CountyPersLiableList : IReturn<List<DSCountryPersonServiceModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户名(手机号)")]
        public string username { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "密码")]
        public string password { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
    }

    [Route("/DataShare/TownPersLiableList", "POST", Summary = "接口：镇级责任人")]
    [Api("数据共享")]
    public class TownPersLiableList : IReturn<List<DSTownPersonLiableViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户名(手机号)")]
        public string username { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "密码")]
        public string password { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
    }

    [Route("/DataShare/VillageGroupPersLiableList", "POST", Summary = "接口：村级工作组")]
    [Api("数据共享")]
    public class VillageGroupPersLiableList : IReturn<List<DSVillageWorkingGroupViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户名(手机号)")]
        public string username { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "密码")]
        public string password { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
    }
    [Route("/DataShare/VillageGridPersLiableList", "POST", Summary = "接口：村级网格责任人")]
    [Api("数据共享")]
    public class VillageGridPersLiableList : IReturn<List<DSVillageGridViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户名(手机号)")]
        public string username { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "密码")]
        public string password { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
    }
    [Route("/DataShare/VillageTransferPersLiableList", "POST", Summary = "接口：村级人员转移清单")]
    [Api("数据共享")]
    public class VillageTransferPersLiableList : IReturn<List<DSVillageTransferPersonViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户名(手机号)")]
        public string username { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "密码")]
        public string password { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
    }
    [Route("/DataShare/ADCDList", "POST", Summary = "接口：行政区划经纬度")]
    [Api("数据共享")]
    public class ADCDList : IReturn<List<DSADCDDisasterViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户名(手机号)")]
        public string username { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "密码")]
        public string password { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
    }

    [Route("/DataShare/DSLogin", "POST", Summary = "接口：登陆")]
    [Api("数据共享")]
    public class DSLogin : IReturn<DataShareReturnModel>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户名(手机号)")]
        public string UserName { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "密码")]
        public string Password { get; set; }
    }
}
