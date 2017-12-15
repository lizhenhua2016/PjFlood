using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using ServiceStack;
using ServiceStackForLeafletjsResponse;
using GrassrootsFloodCtrl.ServiceModel.Route;

namespace GrassrootsFloodCtrl.ServiceModel.RouteNoVerify
{
    [Route("/NoVerifyZZTX/NoVerifyQRCodeList", "POST", Summary = "获取所有村及其二维码")]
    [Api("组织体系相关接口")]
    public class NoVerifyQRCodeList : PageQuery, IReturn<BsTableDataSource<ADCDQRCodeViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "村行政区名")]
        public string adnm { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "村行政区选中")]
        public string adcds { get; set; }


    }

    [Route("/NoVerifyZZTX/NoVerifyAppGetAdcdInfoByAdcd", "POST", Summary = "通过adcd获取对应的adcd的信息")]
    [Api("组织体系相关接口")]
    public class NoVerifyAppGetAdcdInfoByAdcd : IReturn<List<ResponseAdcdByUserAdcd>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "这个是输入的adcd")]
        public string UserAdcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "操作类型")]
        public int ActionType { get; set; }
    }


    [Route("/NoVerifyZZTX/NoVGetNextLevleAdcdInfoByAdcd", "post", Summary = "通过Adcd获取下一级的adcd的信息")]
    [Api("组织体系相关接口")]
    public class NoVGetNextLevleAdcdInfoByAdcd : IReturn<ResponseAdcdInfo>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
    }

    [Route("/NoVerifyZZTX/NoVerifyGetADCDInfo", "GET", Summary = "获取行政区划")]
    [Api("组织体系相关接口")]
    public class NoVGetADCDInfo : PageQuery, IReturn<BsTableDataSource<ADCDInfo>>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "行政区划级别(0：全部，1：省级，2：地级市，3：县级市（区），4：乡镇街道，5：行政村，6：自然村)")]
        public int levle { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "行政区划编码")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政区划名称")]
        public string adnm { get; set; }
    }


    [Route("/NoVerifyZZTX/NoVGetAdcdInfoForCounty", "GET", Summary = "获取行政区划")]
    [Api("组织体系相关接口")]
    public class NoVGetAdcdInfoForCounty : PageQuery, IReturn<BsTableDataSource<ADCDInfo>>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "行政区划级别(0：全部，1：省级，2：地级市，3：县级市（区），4：乡镇街道，5：行政村，6：自然村)")]
        public int levle { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "行政区划编码")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政区划名称")]
        public string adnm { get; set; }
    }
}
