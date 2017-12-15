using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.CApp;
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
    [Route("/Supervise/SPersonLiable", "Post", Summary = "接口：责任考核")]
    [Api("监督考核")]
    public class SPersonLiable : IReturn<SuperviseModel>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "市县adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "行政层级id")]
        public int? grid { get; set; }
    }
    [Route("/Supervise/SPersonLiable1", "Post", Summary = "接口：责任考核")]
    [Api("监督考核")]
    public class SPersonLiable1 : IReturn<SuperviseModel>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "市县adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "行政层级id")]
        public int? grid { get; set; }
    }
    
    [Route("/Supervise/GetPersonLiabelList", "Post", Summary = "接口：镇级责任人汇总")]
    [Api("监督考核")]
    public class GetPersonLiabelList : PageQuery,IReturn<BsTableDataSource<ADCDDisasterViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "类型id")]
        public int? pltype { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "职务")]
        public string position { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "岗位")]
        public string post { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "姓名")]
        public string key { get; set; }
    }

    [Route("/Supervise/CCKHVillage", "Get", Summary = "接口：村级责任人汇总")]
    [Api("监督考核")]
    public class CCKHVillage : PageQuery, IReturn<BsTableDataSource<StatiscPerson>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "职务")]
        public string position { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "岗位")]
        public string post { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "姓名")]
        public string name { get; set; }

        [ApiMember(IsRequired = false, Description = "范围类型", DataType = "int")]
        public int? fid { get; set; }
    }

    [Route("/Supervise/SetCCKH", "Post", Summary = "接口：抽查考核")]
    [Api("监督考核")]
    public class SetCCKH : PageQuery, IReturn<BaseResult>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
        
        [ApiMember(IsRequired = true, DataType = "string", Description = "抽查人")]
        public string checkman { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "抽查人真实姓名")]
        public string checkmanrealname { get; set; }
        
        [ApiMember(IsRequired = true, DataType = "string", Description = "抽查时间")]
        public string checktime { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "被抽查人")]
        public string bycheckman { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "被抽查人手机")]
        public string bycheckphone { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "抽查时间")]
        public string checkitems { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "不合格其他描述")]
        public string noremarks { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "备注")]
        public string remarks { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "抽查状态")]
        public string checkstatus { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "抽查人id")]
        public int uid { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "被抽查人地区")]
        public string areas { get; set; }
        
    }
    
    [Route("/Supervise/GetCCJLTable", "Post", Summary = "接口：抽查记录查询")]
    [Api("监督考核")]
    public class GetCCJLTable : PageQuery, IReturn<BsTableDataSource<SpotCheckModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "被抽查人")]
        public string name { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "抽查人")]
        public string checkname { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年")]
        public int? year { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "等级")]
        public int? level { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "抽查结果")]
        public string checkStatus { get; set; }
        
    }

    [Route("/Supervise/GetCCJLTableOne", "Post", Summary = "接口：查看单个的记录")]
    [Api("监督考核")]
    public class GetCCJLTableOne : PageQuery, IReturn<BsTableDataSource<SpotCheckOne>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "姓名")]
        public string key { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "电话")]
        public string phone { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年")]
        public int? year { get; set; }
    }
    //
    [Route("/Supervise/GetCityAppInPostList", "POST", Summary = "市统计")]
    [Api("更新日志")]
    public class GetCityAppInPostList : IReturn<List<AppAreaViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "起始时间")]
        public string starttime { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "结束时间")]
        public string endtime { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "id")]
        public int id { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "pid")]
        public int pid { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd等级")]
        public int grade { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }
}
