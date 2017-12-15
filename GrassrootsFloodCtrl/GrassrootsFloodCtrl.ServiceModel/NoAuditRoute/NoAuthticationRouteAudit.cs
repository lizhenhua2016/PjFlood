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

namespace GrassrootsFloodCtrl.ServiceModel.NoAuditRoute
{
    [Route("/NoAuthticationAudit/GetAuditResult", "GET", Summary = "接口：审核结果")]
    [Api("不需要权限的")]
    public class NoAuthticationGetAuditResult : IReturn<AuditViewModel>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "申请id")]
        public int? id { get; set; }
    }

    [Route("/NoAuthticationAudit/AuditApplication", "POST", Summary = "接口：乡镇提交审核申请")]
    [Api("不需要权限的")]
    public class NoAuthticationAuditApplication : IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "申请id")]
        public int? id { get; set; }
    }
    [Route("/NoAuthticationAudit/GetAuditApplication", "GET", Summary = "接口：县市级别获取审核申请")]
    [Api("不需要权限的")]
    public class NoAuthticationGetAuditApplication : PageQuery, IReturn<BsTableDataSource<AuditViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "等级id")]
        public int? level { get; set; }
    }

    [Route("/NoAuthticationAudit/GetAuditApplicationCounty", "GET", Summary = "接口：县市级别获取审核申请")]
    [Api("不需要权限的")]
    public class NoAuthticationGetAuditApplicationCounty : PageQuery, IReturn<BsTableDataSource<AuditCountyViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
    }
    [Route("/NoAuthticationAudit/PostAudit", "POST", Summary = "接口：审核申请处理")]
    [Api("不需要权限的")]
    public class NoAuthticationPostAudit : PageQuery, IReturn<BaseResult>
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

    [Route("/NoAuthticationAudit/GetTownVillage", "Get",Summary ="接口:获取镇下村信息")]
    [Api("不需要权限的")]
    public class NoAuthticationGetTownVillage : IReturn<List<ADCDDisasterViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "村名")]
        public string adnm { get; set; }
    }
    [Route("/NoAuthticationAudit/AuditNo", "Post", Summary = "接口:审批不通过")]
    [Api("不需要权限的")]
    public class NoAuthticationAuditNo : IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "申请id")]
        public string id { get; set; }

        [ApiMember(IsRequired = true, DataType = "int", Description = "类型0县1市")]
        public int? t { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "批示")]
        public string remarks { get; set; }
    }
    [Route("/NoAuthticationAudit/GetAuditNo", "Post", Summary = "接口:审批不通过消息")]
    [Api("不需要权限的")]
    public class NoAuthticationGetAuditNo : IReturn<List<AuditViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "申请id")]
        public int? id { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "申请次数")]
        public int? nums { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "类型：1县级,默认乡镇")]
        public int? typeid { get; set; }

    }
    [Route("/NoAuthticationAudit/GetAreaList", "Post", Summary = "接口:获取区域")]
    [Api("不需要权限的")]
    public class NoAuthticationGetAreaList : IReturn<List<ADCDDisasterViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "区域类型2市3县")]
        public int? tid { get; set; }
    }
    [Route("/NoAuthticationAudit/GetAuditNumByADCD", "GET", Summary = "根据条件获取申请次数")]
    [Api("不需要权限的")]
    public class NoAuthticationGetAuditNumByADCD : IReturn<AuditOtherViewModel>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
    }
}
