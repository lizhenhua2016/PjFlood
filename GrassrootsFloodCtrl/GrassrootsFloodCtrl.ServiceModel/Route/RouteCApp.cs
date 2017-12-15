using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.ServiceModel.AppApi;
using GrassrootsFloodCtrl.ServiceModel.CApp;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.ServiceModel.Country;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    #region 综合应用
    [Route("/CApp/GetTownInfo", "Post", Summary = "综合应用镇级概括")]
    [Api("综合应用相关接口")]
    public class GetTownInfo : IReturn<List<TownInfoAppViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/CApp/GetTownPerson", "Post", Summary = "综合应用镇级责任人")]
    [Api("综合应用相关接口")]
    public class GetTownPerson : IReturn<List<TownPersonAppViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/CApp/GetTownGridMan", "Post", Summary = "综合应用镇网格责任人")]
    [Api("综合应用相关接口")]
    public class GetTownGridMan : IReturn<List<TownGridPersonAppViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/CApp/GetCunDot", "Post", Summary = "综合应用村概括")]
    [Api("综合应用相关接口")]
    public class GetCunDot : IReturn<VillageInfoAppViewModel>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/CApp/GetKeys", "Post", Summary = "综合应用关键字检索")]
    [Api("综合应用相关接口")]
    public class GetKeys : IReturn<List<AppKeyViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "关键字")]
        public string name { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/CApp/GetKeysInfo", "Post", Summary = "综合应用关键字内容查询")]
    [Api("综合应用相关接口")]
    public class GetKeysInfo : IReturn<List<AppKeyInfoViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "stirng", Description = "关键字")]
        public string name { get; set; }

        [ApiMember(IsRequired = false, DataType = "stirng", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = true, DataType = "int", Description = "adcd类型")]
        public int? ctype { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/CApp/GetCountyInfo", "Post", Summary = "综合应用县级概括")]
    [Api("综合应用相关接口")]
    public class GetCountyInfo : IReturn<List<CountyInfoAppViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    #region app统计
    [Route("/CApp/GetAppStatics", "Post", Summary = "app端数据聚合")]
    [Api("综合应用相关接口")]
    public class GetAppStatics : IReturn<List<AppStaticsViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = true, DataType = "string", Description = "层级")]
        public string scale { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "起始时间")]
        public string starttime { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "结束时间")]
        public string endtime { get; set; }
    }

    [Route("/CApp/GetAppArea", "Post", Summary = "app端数据聚合")]
    [Api("综合应用相关接口")]
    public class GetAppArea : IReturn<List<AppAreaViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "父id")]
        public int parentid { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "等级id")]
        public int grade { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "起始时间")]
        public string starttime { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "结束时间")]
        public string endtime { get; set; }
    }

    [Route("/CApp/CCKHVillageApp", "Get", Summary = "村人员及其履职")]
    [Api("综合应用相关接口")]
    public class CCKHVillageApp : IReturn<List<StatiscPerson>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "姓名")]
        public string name { get; set; }

        [ApiMember(IsRequired = false, Description = "范围类型", DataType = "int")]
        public int? fid { get; set; }
    }

    [Route("/CApp/GetPersonAppInfo", "Get", Summary = "村责任人履职详情")]
    [Api("综合应用相关接口")]
    public class GetPersonAppInfo : IReturn<List<AppRecordAndUserViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "手机")]
        public string mobile { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "岗位")]
        public string post { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "起始时间")]
        public string starttime { get; set; }

        [ApiMember(IsRequired = false, DataType = "string", Description = "结束时间")]
        public string endtime { get; set; }
    }
    #endregion

    #endregion
}
