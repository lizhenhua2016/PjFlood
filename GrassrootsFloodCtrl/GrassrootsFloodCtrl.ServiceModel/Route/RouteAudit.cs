using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Audit;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Route("/Audit/GetAuditResult", "GET", Summary = "接口：审核结果")]
    [Api("审核")]
    public class GetAuditResult : IReturn<AuditViewModel>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "申请id")]
        public int? id { get; set; }
    }

    [Route("/Audit/AuditApplication", "POST", Summary = "接口：乡镇提交审核申请")]
    [Api("审核")]
    public class AuditApplication : IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "申请id")]
        public int? id { get; set; }
    }
    [Route("/Audit/GetAuditApplication", "GET", Summary = "接口：县市级别获取审核申请")]
    [Api("审核")]
    public class GetAuditApplication : PageQuery, IReturn<BsTableDataSource<AuditViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "等级id")]
        public int? level { get; set; }
    }
    
    [Route("/Audit/GetAuditApplicationCounty", "GET", Summary = "接口：县市级别获取审核申请")]
    [Api("审核")]
    public class GetAuditApplicationCounty : PageQuery, IReturn<BsTableDataSource<AuditCountyViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
    }
    [Route("/Audit/PostAudit", "POST", Summary = "接口：审核申请处理")]
    [Api("审核")]
    public class PostAudit : PageQuery, IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "县市级adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "被审核县,乡镇id")]
        public string ids { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "批注")]
        public string remarks { get; set; }

        [ApiMember(IsRequired = true, DataType = "int", Description = "审核类型0县级别，1市级")]
        public int? t { get; set; }

    }

    [Route("/Audit/GetTownVillage","Get",Summary ="接口:获取镇下村信息")]
    [Api("审核")]
    public class GetTownVillage : IReturn<List<ADCDDisasterViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "村名")]
        public string adnm { get; set; }
    }
    [Route("/Audit/AuditNo", "Post", Summary = "接口:审批不通过")]
    [Api("审核")]
    public class AuditNo : IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "申请id")]
        public string id { get; set; }

        [ApiMember(IsRequired = true, DataType = "int", Description = "类型0县1市")]
        public int? t { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "批示")]
        public string remarks { get; set; }
    }
    [Route("/Audit/GetAuditNo", "Post", Summary = "接口:审批不通过消息")]
    [Api("审核")]
    public class GetAuditNo : IReturn<List<AuditViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "申请id")]
        public int? id { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "申请次数")]
        public int? nums { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "类型：1县级,默认乡镇")]
        public int? typeid { get; set; }

    }
    [Route("/Audit/GetAreaList", "Post", Summary = "接口:获取区域")]
    [Api("审核")]
    public class GetAreaList : IReturn<List<ADCDDisasterViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "区域类型2市3县")]
        public int? tid { get; set; }
    }
    [Route("/Audit/GetAuditNumByADCD", "GET", Summary = "根据条件获取申请次数")]
    [Api("系统用户相关接口")]
    public class GetAuditNumByADCD : IReturn<AuditOtherViewModel>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
    }
}
