using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Route("/VillagePic/GetVillagePicAdcd", "Get", Summary = "获取行政村防汛防台形势图（已上报和未上报的村名）")]
    [Api("村防汛形势图相关接口")]
    public class GetVillagePicAdcd : PageQuery, IReturn<BsTableDataSource<VillageViewModel>>
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

    [Route("/VillagePic/SaveVillagePic", "Post", Summary = "保存行政村防汛防台形势图 ")]
    [Api("村防汛形势图相关接口")]
    public class SaveVillagePic : IReturn<bool>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }
       
        [ApiMember(IsRequired = true, DataType = "string", Description = "文件路径")]
        public string filePath { get; set; }

        [ApiMember(IsRequired = true, DataType = "int", Description = "年度")]
        public int Year { get; set; }
    }

    [Route("/VillagePic/DelVillagePic", "Post", Summary = "删除行政村防汛防台形势图 ")]
    [Api("村防汛形势图相关接口")]
    public class DelVillagePic : IReturn<bool>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "村adcd，多个以逗号隔开")]
        public string adcds { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "年份")]
        public int Year { get; set; }
    }

    [Route("/VillagePic/GetVillagePicByAdcdAndYear", "Get", Summary = "获取已上报的行政村防汛防台形势图")]
    [Api("村防汛形势图相关接口")]
    public class GetVillagePicByAdcdAndYear : IReturn<VillagePic>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "镇adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "年度")]
        public int year { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "审核次数")]
        public int? nums { get; set; }
    }

    [Route("/VillagePic/GetVillagePicList", "Get", Summary = "获取已上报的行政村防汛防台形势图列表")]
    [Api("村防汛形势图相关接口")]
    public class GetVillagePicList : PageQuery, IReturn<BsTableDataSource<VillagePicViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政村adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "年度")]
        public int year { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政村名称")]
        public string adnm { get; set; }
    }
}
