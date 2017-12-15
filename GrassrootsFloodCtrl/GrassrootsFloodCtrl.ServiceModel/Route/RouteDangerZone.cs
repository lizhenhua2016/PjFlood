using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.DangerZone;
using ServiceStack;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Route("/DangerZone/GetDangerZoneList", "Get", Summary = "获取危险点类型列表(含分页)接口")]
    [Api("危险点类型相关接口")]
    public  class GetDangerZoneList: PageQuery, IReturn<BsTableDataSource<DangerZoneViewModel>>
    {
        [ApiMember(IsRequired = false,DataType = "string",Description = "行政区划编码")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "危险点（区）名称")]
        public string name { get; set; }
    }

    [Route("/DangerZone/GetDangerZone", "Get", Summary = "获取危险点类型列表接口")]
    [Api("危险点类型相关接口")]
    public class GetDangerZone :  IReturn<List<DangerZoneViewModel>>
    {
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政区划编码")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "危险点（区）类型名称")]
        public string name { get; set; }
    }
    [Route("/DangerZone/SaveDangerZone", "Post", Summary = "保存危险点类型接口")]
    [Api("危险点类型相关接口")]
    public class SaveDangerZone : IReturn<bool>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "自增ID")]
        public int id { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "行政区划编码")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "危险点（区）类型名称")]
        public string name { get; set; }
        
    }
}
