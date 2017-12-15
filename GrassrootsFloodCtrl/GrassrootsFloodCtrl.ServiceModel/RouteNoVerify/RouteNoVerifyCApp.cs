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

namespace GrassrootsFloodCtrl.ServiceModel.RouteNoVerify
{
    #region 综合应用
    [Route("/NoVerifyCApp/NoVerifyGetTownInfo", "Post", Summary = "综合应用镇级概括")]
    [Api("综合应用相关接口")]
    public class NoVerifyGetTownInfo : IReturn<List<TownInfoAppViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/NoVerifyCApp/NoVerifyGetTownPerson", "Post", Summary = "综合应用镇级责任人")]
    [Api("综合应用相关接口")]
    public class NoVerifyGetTownPerson : IReturn<List<TownPersonAppViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/NoVerifyCApp/NoVerifyGetTownGridMan", "Post", Summary = "综合应用镇网格责任人")]
    [Api("综合应用相关接口")]
    public class NoVerifyGetTownGridMan : IReturn<List<TownGridPersonAppViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/NoVerifyCApp/NoVerifyGetCunDot", "Post", Summary = "综合应用村概括")]
    [Api("综合应用相关接口")]
    public class NoVerifyGetCunDot : IReturn<VillageInfoAppViewModel>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/NoVerifyCApp/NoVerifyGetKeys", "Post", Summary = "综合应用关键字检索")]
    [Api("综合应用相关接口")]
    public class NoVerifyGetKeys : IReturn<List<AppKeyViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "关键字")]
        public string name { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    [Route("/NoVerifyCApp/NoVerifyGetKeysInfo", "Post", Summary = "综合应用关键字内容查询")]
    [Api("综合应用相关接口")]
    public class NoVerifyGetKeysInfo : IReturn<List<AppKeyInfoViewModel>>
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

    [Route("/NoVerifyCApp/NoVerifyGetCountyInfo", "Post", Summary = "综合应用县级概括")]
    [Api("综合应用相关接口")]
    public class NoVerifyGetCountyInfo : IReturn<List<CountyInfoAppViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "stirng", Description = "镇adcd")]
        public string adcd { get; set; }

        [ApiMember(IsRequired = false, DataType = "int", Description = "年份")]
        public int? year { get; set; }
    }

    #region app统计
    [Route("/NoVerifyCApp/NoVerifyGetAppStatics", "Post", Summary = "app端数据聚合")]
    [Api("综合应用相关接口")]
    public class NoVerifyGetAppStatics : IReturn<List<AppStaticsViewModel>>
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

    [Route("/NoVerifyCApp/NoVerifyGetAppArea", "Post", Summary = "app端数据聚合")]
    [Api("综合应用相关接口")]
    public class NoVerifyGetAppArea : IReturn<List<AppAreaViewModel>>
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

    [Route("/NoVerifyCApp/NoVerifyCCKHVillageApp", "Get", Summary = "村人员及其履职")]
    [Api("综合应用相关接口")]
    public class NoVerifyCCKHVillageApp : IReturn<List<StatiscPerson>>
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

    [Route("/NoVerifyCApp/NoVerifyGetPersonAppInfo", "Get", Summary = "村责任人履职详情")]
    [Api("综合应用相关接口")]
    public class NoVerifyGetPersonAppInfo : IReturn<List<AppRecordAndUserViewModel>>
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
