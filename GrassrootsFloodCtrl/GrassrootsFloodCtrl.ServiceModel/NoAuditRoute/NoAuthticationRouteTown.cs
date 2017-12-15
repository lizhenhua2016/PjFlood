using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Town;
using ServiceStack;
using GrassrootsFloodCtrl.ServiceModel.Village;
using GrassrootsFloodCtrl.ServiceModel.NoAuditRoute;

namespace GrassrootsFloodCtrl.ServiceModel.NoTownRoute
{
    [Route("/NoAuthticationTown/GetTownList", "Get", Summary = "获取镇街责任人列表接口")]
    [Api("没有权限的镇街责任人相关接口")]
    public class NoAuthticationGetTownList : PageQuery,IReturn<BsTableDataSource<TownPersonLiableViewModel>>
    {
        [ApiMember(IsRequired = false, Description = "自增Id", DataType = "int")]
        public int Id { get; set; }
        [ApiMember(IsRequired =false,Description = "岗位",DataType = "string")]
        public string Position { get; set; }
        [ApiMember(IsRequired = false, Description = "职务", DataType = "string")]
        public string Post { get; set; }
        [ApiMember(IsRequired = false, Description = "姓名", DataType = "string")]
        public string name { get; set; }
        [ApiMember(IsRequired = true, Description = "年度", DataType = "int")]
        public int? year { get; set; }
        [ApiMember(IsRequired = true, Description = "镇adcd", DataType = "string")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, Description = "当前提交次数", DataType = "int")]
        public int? nums { get; set; }
    }
    [Route("/NoAuthticationTown/GetTownList1", "Get", Summary = "同人多岗位获取")]
    [Api("没有权限的镇街责任人相关接口")]
    public class NoAuthticationGetTownList1 : PageQuery, IReturn<BsTableDataSource<TownPersonLiableViewModel>>
    {
        [ApiMember(IsRequired = true, Description = "镇adcd", DataType = "string")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, Description = "岗位", DataType = "string")]
        public string post { get; set; }

        [ApiMember(IsRequired = false, Description = "职位", DataType = "string")]
        public string position { get; set; }

        [ApiMember(IsRequired = false, Description = "姓名", DataType = "string")]
        public string name { get; set; }

        [ApiMember(IsRequired = false, Description = "年", DataType = "int")]
        public int? year { get; set; }

        [ApiMember(IsRequired = false, Description = "范围类型", DataType = "int")]
        public int? fid { get; set; }
    }

    [Route("/NoAuthticationTown/SaveTown", "Post", Summary = "保存镇街责任人接口")]
    [Api("没有权限的镇街责任人相关接口")]
    public class NoAuthticationSaveTown : IReturn<bool>
    {
        [ApiMember(IsRequired = false, Description = "自增ID", DataType = "int")]
        public int Id { get; set; }
        [ApiMember(IsRequired = false, Description = "岗位", DataType = "string")]
        public string Position { get; set; }
        [ApiMember(IsRequired = false, Description = "职务", DataType = "string")]
        public string Post { get; set; }
        [ApiMember(IsRequired = true, Description = "姓名", DataType = "string")]
        public string name { get; set; }
        [ApiMember(IsRequired = false, Description = "手机号码", DataType = "string")]
        public string Mobile { get; set; }
        [ApiMember(IsRequired = false, Description = "备注", DataType = "string")]
        public string Remark { get; set; }
    }

    [Route("/NoAuthticationTown/DelTown", "Post", Summary = "删除镇街责任人接口")]
    [Api("没有权限的镇街责任人相关接口")]
    public class NoAuthticationDelTown : IReturn<bool>
    {
        [ApiMember(IsRequired = true,DataType = "stirng",Description = "多个ID以逗号隔开")]
        public string ids { get; set; }
    }
    
}
