using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.CApp;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Route("/StatisAnalysis/GetStatisAnalysisList", "Get", Summary = "审批信息统计")]
    [Api("统计分析")]
    public class GetStatisAnalysisList : IReturn<BsTableDataSource<StatisAnalysisViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "日期")]
        public int? year { get; set; }
    }
    [Route("/StatisAnalysis/GetStatisAppPersonInPost", "Get", Summary = "App履职人员统计")]
    [Api("统计分析")]
    public class GetStatisAppPersonInPost : IReturn<BsTableDataSource<StatisAppPersonInPostViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "日期")]
        public int? year { get; set; }
    }
    /// <summary>
    /// 防汛任务统计
    /// </summary>
    [Route("/StatisAnalysis/GetStatisPrevFlood","Get",Summary ="防汛任务统计")]
    [Api("统计分析")]
    public class GetStatisPrevFlood :IReturn<BsTableDataSource<StatisPrevFloodViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired =true,DataType ="string",Description ="日期")]
        public int? year { get; set; }
    }
    /// <summary>
    /// 县级乡镇防汛详情
    /// </summary>
    [Route("/StatisAnalysis/GetCountyFloodDetail","Get",Summary ="县级乡镇防汛详情")]
    public class GetCountyFloodDetail : PageQuery, IReturn<BsTableDataSource<CountyFloodDetailViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "adcd")]
        public string countyadcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd")]
        public string townadcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false,DataType ="string",Description ="防汛任务情况")]
        public string floodTask { get; set; }
    }
    [Route("/StatisAnalysis/Statistics", "Post", Summary = "按网格类型统计")]
    [Api("综合应用相关接口")]
    public class Statistics : IReturn<List<CAppStaticsViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "adcd类型")]
        public int? adcdtype { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/StatisAnalysis/StatisticsByPost", "Post", Summary = "按岗位统计")]
    [Api("综合应用相关接口")]
    public class StatisticsByPost : IReturn<List<CAppStaticsByPostViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "岗位名")]
        public string typename { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "岗位层级名")]
        public string typelevel { get; set; }

        [ApiMember(IsRequired = false, DataType = "stirng", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "adcd类型")]
        public int? adcdtype { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/StatisAnalysis/StaticsVillageTransfer", "Post", Summary = "统计")]
    [Api("综合应用相关接口")]
    public class StaticsVillageTransfer : IReturn<List<StaticsVillageTransferModel>>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "adcd")]
        public string items { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/StatisAnalysis/GetStatisTypeInfoList", "Get", Summary = "类型统计类型详情汇总")]
    [Api("综合应用相关接口")]
    public class GetStatisTypeInfoList : PageQuery, IReturn<BsTableDataSource<StatisTypeInfo>>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "类型名")]
        public string typename { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "类型")]
        public string type { get; set; }

        [ApiMember(IsRequired = false, DataType = "stirng", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "stirng", Description = "市")]
        public string cityname { get; set; }

        [ApiMember(IsRequired = false, DataType = "stirng", Description = "县")]
        public string countyname { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/StatisAnalysis/GetStatisTypeInfoOneWXQ", "Get", Summary = "类型统计危险区类型详情")]
    [Api("综合应用相关接口")]
    public class GetStatisTypeInfoOneWXQ : PageQuery, IReturn<BsTableDataSource<VillageTransferPersonViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "类型名")]
        public string typename { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "类型")]
        public string type { get; set; }

        [ApiMember(IsRequired = false, DataType = "stirng", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/StatisAnalysis/GetStatisTypeInfoOneWG", "Get", Summary = "类型统计网格类型详情")]
    [Api("综合应用相关接口")]
    public class GetStatisTypeInfoOneWG : PageQuery, IReturn<BsTableDataSource<VillageGridViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "类型名")]
        public string typename { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "类型")]
        public string type { get; set; }

        [ApiMember(IsRequired = false, DataType = "stirng", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    //
    [Route("/StatisAnalysis/GetStatisCountyPerson", "Get", Summary = "类型统计县级责任人")]
    [Api("综合应用相关接口")]
    public class GetStatisCountyPerson : PageQuery, IReturn<BsTableDataSource<StatisTypeInfo>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "市adcd")]
        public string cityadcd { get; set; }
        
        [ApiMember(IsRequired = false, DataType = "stirng", Description = "adcd集合")]
        public string adcds { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    //
    [Route("/StatisAnalysis/GetStatisTownPerson", "Get", Summary = "类型统计乡镇级责任人")]
    [Api("综合应用相关接口")]
    public class GetStatisTownPerson : PageQuery, IReturn<BsTableDataSource<TownPersonLiableViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd集合")]
        public string adcds { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "乡镇名")]
        public string townname { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/StatisAnalysis/GetStatisVillagePerson", "Get", Summary = "类型统计村级责任人")]
    [Api("综合应用相关接口")]
    public class GetStatisVillagePerson : PageQuery, IReturn<BsTableDataSource<StatisVillagePersonViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd集合")]
        public string adcds { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "乡镇名")]
        public string villagename { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/StatisAnalysis/GetStatisTransferPerson", "Get", Summary = "类型统计需转移责任人")]
    [Api("综合应用相关接口")]
    public class GetStatisTransferPerson :  IReturn<BsTableDataSource<StatisTransferPersonViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }
        
        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/StatisAnalysis/StatisticsByPostOne", "Get", Summary = "岗位统计岗位详情汇总")]
    [Api("综合应用相关接口")]
    public class StatisticsByPostOne : PageQuery,IReturn<BsTableDataSource<StatisticsByPostInfoViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "类型")]
        public string type { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "类型名")]
        public string typename { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/StatisAnalysis/GetStatisGridTypeAll", "Get", Summary = "类型统计需转移责任人")]
    [Api("综合应用相关接口")]
    public class GetStatisGridTypeAll : IReturn<BsTableDataSource<StatisTransferPersonViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/StatisAnalysis/GetStatisDangerTypeAll", "Get", Summary = "危险点统计需转移责任人")]
    [Api("综合应用相关接口")]
    public class GetStatisDangerTypeAll : IReturn<BsTableDataSource<StatisTransferPersonViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }
    
    [Route("/StatisAnalysis/GetStatisTypeInfoCountyWG", "Get", Summary = "县级获取网格类型个数")]
    [Api("综合应用相关接口")]
    public class GetStatisTypeInfoCountyWG : PageQuery, IReturn<BsTableDataSource<VillageGridViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "类型名")]
        public string typename { get; set; }
        
        [ApiMember(IsRequired = false, DataType = "stirng", Description = "单选的adcd")]
        public string adcdradio { get; set; }

        [ApiMember(IsRequired = false, DataType = "stirng", Description = "复选的adcd")]
        public string adcdchecks { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/StatisAnalysis/GetStatisTypeInfoCountyWXQ", "Get", Summary = "县级获取危险点个数")]
    [Api("综合应用相关接口")]
    public class GetStatisTypeInfoCountyWXQ : PageQuery, IReturn<BsTableDataSource<VillageTransferPersonViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "类型名")]
        public string typename { get; set; }

        [ApiMember(IsRequired = false, DataType = "stirng", Description = "单选的adcd")]
        public string adcdradio { get; set; }

        [ApiMember(IsRequired = false, DataType = "stirng", Description = "复选的adcd")]
        public string adcdchecks { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }
    
    [Route("/StatisAnalysis/GetStatisCountyGridTypeAll", "Get", Summary = "县级全部网格类型分布统计")]
    [Api("综合应用相关接口")]
    public class GetStatisCountyGridTypeAll : IReturn<BsTableDataSource<StatisTransferPersonViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/StatisAnalysis/GetStatisCountyDangerTypeAll", "Get", Summary = "县级全部危险点类型分布统计")]
    [Api("综合应用相关接口")]
    public class GetStatisCountyDangerTypeAll : IReturn<BsTableDataSource<StatisTransferPersonViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/StatisAnalysis/StatisticsCoutyByPostOne", "Get", Summary = "乡镇--岗位统计岗位详情汇总")]
    [Api("综合应用相关接口")]
    public class StatisticsCoutyByPostOne : PageQuery, IReturn<BsTableDataSource<StatisticsByPostInfoViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "选中的adcd")]
        public string adcds { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "类型")]
        public string type { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "类型名")]
        public string typename { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    //
    [Route("/StatisAnalysis/GetStatisCountyAllTransferPerson", "Get", Summary = "类型统计危险区类型详情")]
    [Api("综合应用相关接口")]
    public class GetStatisCountyAllTransferPerson : PageQuery, IReturn<BsTableDataSource<VillageTransferPersonViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "stirng", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/StatisAnalysis/StatisVillagePersonInPostByCountyAdcd", "Get", Summary = "类型统计危险区类型详情")]
    [Api("综合应用相关接口")]
    public class StatisVillagePersonInPostByCountyAdcd : PageQuery, IReturn<BsTableDataSource<StatiscPersonInPost>>
    {
        [ApiMember(IsRequired = false, DataType = "stirng", Description = "县级adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "stirng", Description = "乡镇adcd")]
        public string townadcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "stirng", Description = "村adcd")]
        public string villageadcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }
}
