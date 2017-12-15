using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using ServiceStack;
using GrassrootsFloodCtrl.ServiceModel.Village;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.ServiceModel.Common;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Route("/VillageTransferPerson/GetVillageReportNum", "Get", Summary = "获取行政村上报和未上报统计信息")]
    [Api("村危险区转移人员相关接口")]
    public class GetVillageReportNum : IReturn<List<VillageNumViewModel>>
    {
        [ApiMember(IsRequired = false,DataType = "int",Description = "年度（如果不传值，默认为当前年度）")]
        public int Year { get; set; }
    }
    [Route("/VillageTransferPerson/GetVillageReportNum1", "Get", Summary = "获取行政村上报和未上报统计信息")]
    [Api("村危险区转移人员相关接口")]
    public class GetVillageReportNum1 : IReturn<List<VillageNumViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "年度（如果不传值，默认为当前年度）")]
        public int Year { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "adcd")]
        public string adcd { get; set; }
    }


    [Route("/VillageTransferPerson/GetVillageTransferPersonAdcd", "Get", Summary = "获取危险区转移人员行政村 （已上报和未上报的村名）")]
    [Api("村危险区转移人员相关接口")]
    public class GetVillageTransferPersonAdcd : PageQuery, IReturn<BsTableDataSource<VillageViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政村名称")]
        public string adnm { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "是否已上报（0：未上报，1：已上报,2:全部）")]
        public int type { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年度")]
        public int year { get; set; }
    }

    [Route("/VillageTransferPerson/GetVillageTransferPerson", "Get", Summary = "获取村危险区转移人员,通过乡镇adcd")]
    [Api("村危险区转移人员相关接口")]
    public class GetVillageTransferPerson : PageQuery, IReturn<BsTableDataSource<VillageTransferPersonViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "adcdInfo表的自增id")]
        public int adcdId { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "危险区转移人员表自增id")]
        public int id { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "危险区（点）名称")]
        public string name { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政村名称")]
        public string adnm { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年度")]
        public int? year { get; set; }
    }

    [Route("/VillageTransferPerson/GetVillageTransferPerson1", "Get", Summary = "获取村危险区转移人员,通过乡镇adcd")]
    [Api("村危险区转移人员相关接口")]
    public class GetVillageTransferPerson1 : PageQuery, IReturn<BsTableDataSource<VillageTransferPersonViewModel>>
    {
      
        [ApiMember(IsRequired = false, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }
      
        [ApiMember(IsRequired = false, DataType = "int", Description = "年度")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "审核次数")]
        public int? nums { get; set; }
    }
    [Route("/VillageTransferPerson/GetVillageTransferPerson2", "Get", Summary = "同名同岗位责任人")]
    [Api("村危险区转移人员相关接口")]
    public class GetVillageTransferPerson2 : PageQuery, IReturn<BsTableDataSource<StatiscPerson>>
    {

        [ApiMember(IsRequired = false, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年度")]
        public int? year { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "姓名")]
        public string name { get; set; }

        [ApiMember(IsRequired = false, Description = "范围类型", DataType = "int")]
        public int? fid { get; set; }

    }
    [Route("/VillageTransferPerson/SaveVillageTransferPerson", "POST", Summary = "保存村危险区转移人员")]
    [Api("村危险区转移人员相关接口")]
    public class SaveVillageTransferPerson :  IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "自增Id")]
        public int Id { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "危险区（点）名称")]
        public string DangerZoneName { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "类别")]
        public string DangerZoneType { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "位置")]
        public string Position { get; set; }

        [ApiMember(IsRequired = false, DataType = "double", Description = "经度")]
        public double Lng { get; set; }

        [ApiMember(IsRequired = false, DataType = "double", Description = "纬度")]
        public double Lat { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "户主姓名")]
        public string HouseholderName { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "居住人数")]
        public int HouseholderNum { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "户主手机")]
        public string HouseholderMobile { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "责任人姓名")]
        public string PersonLiableName { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "责任人职务")]
        public string PersonLiablePost { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "责任人手机")]
        public string PersonLiableMobile { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "预警责任人姓名")]
        public string WarnPersonLiableName { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "预警责任人职务")]
        public string WarnPersonLiablePost { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "预警责任人手机")]
        public string WarnPersonLiableMobile { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "避灾场所名称")]
        public string DisasterPreventionName { get; set; }

        [ApiMember(IsRequired = false, DataType = "bool", Description = "有无安全鉴定")]
        public bool SafetyIdentification { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "避灾场所管理员")]
        public string DisasterPreventionManager { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "避灾场所管理员手机")]
        public string DisasterPreventionManagerMobile { get; set; }

        [ApiMember(IsRequired = true, DataType = "int", Description = "年度")]
        public int Year { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "备注")]
        public string Remark { get; set; }
        
    }

    [Route("/VillageTransferPerson/DelVillageTransferPerson", "POST", Summary = "根据人员ID删除村危险区转移人员")]
    [Api("村危险区转移人员相关接口")]
    public class DelVillageTransferPerson : IReturn<bool>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "自增长的ID（多个ID以英文状态的逗号隔开）")]
        public string ids { get; set; }
    }
    [Route("/VillageTransferPerson/DelVillageTransferPersonByADCD", "POST", Summary = "根据行政村ADCD删除村危险区转移人员")]
    [Api("村危险区转移人员相关接口")]
    public class DelVillageTransferPersonByADCD : IReturn<bool>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "行政村ADCD")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年度")]
        public int Year { get; set; }
    }
    [Route("/VillageTransferPerson/NoVillageTransferPerson", "POST", Summary = "村无危险区可转移人员申请提交")]
    [Api("村危险区转移人员相关接口")]
    public class NoVillageTransferPerson : IReturn<bool>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "行政村ADCD")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年度")]
        public int Year { get; set; }
    }
    
}
