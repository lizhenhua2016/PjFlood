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

namespace GrassrootsFloodCtrl.ServiceModel.Route
{

    [Route("/ZZTX/GetADCDInfoById", "Get", Summary = "根据行政区划编码获取单个行政区划")]
    [Api("组织体系相关接口")]
    public class GetADCDInfoById : IReturn<ADCDInfo>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "自增ID")]
        public int id { get; set; }
    }

    [Route("/ZZTX/GetADCDInfoByADCD", "GET", Summary = "根据行政区划编码获取单个行政区划")]
    [Api("组织体系相关接口")]
    public class GetADCDInfoByADCD : IReturn<ADCDInfo>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "行政区划编码")]
        public string adcd { get; set; }
    }

    [Route("/ZZTX/GetADCDInfo", "GET", Summary = "获取行政区划")]
    [Api("组织体系相关接口")]
    public  class GetADCDInfo:PageQuery,IReturn<BsTableDataSource<ADCDInfo>>
    {
        [ApiMember(IsRequired = true,DataType = "int",Description = "行政区划级别(0：全部，1：省级，2：地级市，3：县级市（区），4：乡镇街道，5：行政村，6：自然村)")]
        public int levle { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "行政区划编码")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政区划名称")]
        public string adnm { get; set; }
    }

    [Route("/ZZTX/GetADCDInfoTree", "POST", Summary = "获取行政区划")]
    [Api("组织体系相关接口")]
    public class GetADCDInfoTree : PageQuery, IReturn<List<ADCDTree>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政区划编码")]
        public string adcd { get; set; }
    }

    [Route("/ZZTX/SaveADCDInfo", "POST", Summary = "保存行政区划")]
    [Api("组织体系相关接口")]
    public class SaveADCDInfo : IReturn<bool>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "自增ID")]
        public int? id { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "行政区划编码")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "行政区划名称")]
        public string adnm { get; set; }
        [ApiMember(IsRequired = false, DataType = "double", Description = "经度")]
        public double? lng { get; set; }
        [ApiMember(IsRequired = false, DataType = "double", Description = "纬度")]
        public double? lat { get; set; }
    }

    [Route("/ZZTX/GetAdcdInfoByADNM", "GET", Summary = "根据乡镇和行政村名称获取行政村信息")]
    [Api("组织体系相关接口")]
    public class GetAdcdInfoByADNM : IReturn<ADCDInfo>
    {
        [ApiMember(IsRequired = true,DataType = "string",Description = "行政区划名称")]
        public string adnm { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "乡镇行政区划编码")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "年份")]
        public int Year { get; set; }
    }

    [Route("/ZZTX/GetADCDDisasterInfo", "GET", Summary = "获取受灾害影响的行政区划信息")]
    [Api("组织体系相关接口")]
    public class GetADCDDisasterInfo : PageQuery,IReturn<BsTableDataSource<ADCDDisasterViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "自增ID")]
        public int? id { get; set; }
        [ApiMember(IsRequired = false, DataType = "stirng", Description = "行政区划名称")]
        public string name { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年度")]
        public int? year { get; set; }
    }

    [Route("/ZZTX/SaveADCDDisasterInfo", "POST", Summary = "保存受灾害影响的行政区划信息")]
    [Api("组织体系相关接口")]
    public class SaveADCDDisasterInfo :  IReturn<bool>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "自增ID")]
        public int? id { get; set; }
        [ApiMember(IsRequired = false, DataType = "stirng", Description = "行政区划编码")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "行政区划名称")]
        public string adnm { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "受灾点数量")]
        public int? PointNum { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "总人口数")]
        public int? TotalNum { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "受灾害影响人数")]
        public int? PopulationNum { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "经度")]
        public double? lng { get; set; }
        [ApiMember(IsRequired = false, DataType = "double", Description = "纬度")]
        public double? lat { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年度")]
        public int Year { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "防汛防台任务轻重情况")]
        public string FXFTRW { get; set; }
        
    }

    [Route("/ZZTX/DelADCDDisasterInfo", "POST", Summary = "删除受灾害影响的行政区划信息")]
    [Api("组织体系相关接口")]
    public class DelADCDDisasterInfo : IReturn<bool>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "自增长的ID（多个ID以英文状态的逗号隔开）")]
        public string ids { get; set; }
    }
    [Route("/ZZTX/DelADCDInfo", "POST", Summary = "删除行政区划信息")]
    [Api("组织体系相关接口")]
    public class DelADCDInfo : IReturn<bool>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "自增的ID，多个以逗号隔开")]
        public string ids { get; set; }
    }
    [Route("/ZZTX/ImportCurrentInfo", "POST", Summary = "导入当前年度信息(把上一年度的信息复制到今年)")]
    [Api("组织体系相关接口")]
    public class ImportCurrentInfo : IReturn<bool>
    {
        [ApiMember(IsRequired = true,DataType = "int",Description = "信息类型（0：行政村信息，1：行政村防汛防台工作组，2：行政村网格责任人，3：行政村危险区转移人员清单，4：行政村防汛防台形势图，5：镇街防汛防台责任人）")]
        public int type { get; set; }
    }

    [Route("/ZZTX/SavePoint", "POST", Summary = "保存行政区划坐标点信息")]
    [Api("组织体系相关接口")]
    public class SavePoint : IReturn<bool>
    {
        [ApiMember(IsRequired = true,DataType = "double",Description = "经度")]
        public double lng { get; set; }
        [ApiMember(IsRequired = true, DataType = "double", Description = "纬度")]
        public double lat { get; set; }
        //[ApiMember(IsRequired = false, DataType = "int", Description = "自增ID")]
        //public int id { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政区划编码adcd")]
        public string adcd { get; set; }
    }


    [Route("/ZZTX/QRCodeList", "POST", Summary = "获取所有村及其二维码")]
    [Api("组织体系相关接口")]
    public class QRCodeList : PageQuery, IReturn<BsTableDataSource<ADCDQRCodeViewModel>>
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

    [Route("/ZZTX/GetADCDInfoBySession", "GET", Summary = "通过session的adcd获取行政区划")]
    [Api("组织体系相关接口")]
    public class GetADCDInfoBySession : IReturn<List<ADCDInfo>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "通过session的adcd获取当前的值")]
        public string adcd { get; set; }
    }

    [Route("/ZZTX/GetADCDInfoForCounty", "GET", Summary = "获取行政区划")]
    [Api("组织体系相关接口")]
    public class GetAdcdInfoForCounty : PageQuery, IReturn<BsTableDataSource<ADCDInfo>>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "行政区划级别(0：全部，1：省级，2：地级市，3：县级市（区），4：乡镇街道，5：行政村，6：自然村)")]
        public int levle { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "行政区划编码")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政区划名称")]
        public string adnm { get; set; }
    }


    [Route("/ZZTX/GetAdcdByUseradcd", "POST", Summary = "获取全部的")]
    [Api("组织体系相关接口")]
    public class GetAdcdByUseradcd : IReturn<List<ResponseAdcdByUserAdcd>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "这个是输入的adcd")]
        public string UserAdcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "父级Id")]
        public int parentId { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "这里是输入的等级（省1，市2，县3，镇4，村5）")]
        public int grade { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "地点名称")]
        public string adnm { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "获取类型")]
        public int actiontype { get; set; }
    }


    [Route("/ZZTX/AppGetAdcdInfoByAdcd", "POST", Summary = "通过adcd获取对应的adcd的信息")]
    [Api("组织体系相关接口")]
    public class AppGetAdcdInfoByAdcd : IReturn<List<ResponseAdcdByUserAdcd>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "这个是输入的adcd")]
        public string UserAdcd { get; set; } 
        [ApiMember(IsRequired = true,DataType ="int",Description ="操作类型")]
        public int ActionType { get; set; }
    }

    [Route("/AppEmergency/AppGetAllCity", "POST", Summary = "省级获取全部的市的信息")]
    [Api("应急管理地图展示")]
    public class AppGetAllCity : IReturn<List<ResponseAdcdByUserAdcd>>
    {
        [ApiMember(IsRequired =true,DataType ="string",Description ="省级adcd")]
        public string ProvAdcd { get; set; }
    }

    [Route("/AppEmergency/AppGetAllCounty", "POST", Summary = "通过市级adcd获取下一级的县级的信息")]
    [Api("应急管理地图展示")]
    public class AppGetAllCounty : IReturn<List<ResponseAdcdByUserAdcd>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "市级adcd")]
        public string CityAdcd { get; set; }
    }

    [Route("/AppEmergency/AppGetAllTown", "POST", Summary = "通过县级adcd获取下级镇的信息")]
    [Api("应急管理地图展示")]
    public class AppGetAllTown : IReturn<List<ResponseAdcdByUserAdcd>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "县级adcd")]
        public string CountyAdcd { get; set; }
    }

    [Route("/AppEmergency/AppGetAllVillage", "POST", Summary = "通过镇级adcd获取下级村的信息")]
    [Api("应急管理地图展示")]
    public class AppGetAllVillage : IReturn<List<ResponseAdcdByUserAdcd>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村级adcd")]
        public string TownAdcd { get; set; }
    }
    [Route("/ZZTX/GetAdcdInfoByAdcd","post",Summary ="通过adcd获取对应的adcd的信息")]
    [Api("组织体系相关接口")]
    public class GetAdcdInfoByAdcd : IReturn<ResponseAdcdInfo>
    {
        [ApiMember(IsRequired =true,DataType ="string",Description ="adcd")]
        public string adcd { get; set; }
    }
    [Route("/ZZTX/GetNextLevleAdcdInfoByAdcd", "post", Summary = "通过Adcd获取下一级的adcd的信息")]
    [Api("组织体系相关接口")]
    public class GetNextLevleAdcdInfoByAdcd : IReturn<ResponseAdcdInfo>
    {
        [ApiMember(IsRequired = true,DataType ="string",Description ="adcd")]
        public string adcd { get; set; }
    }
}
