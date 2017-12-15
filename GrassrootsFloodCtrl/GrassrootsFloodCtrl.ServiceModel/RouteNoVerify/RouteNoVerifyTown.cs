using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Town;
using ServiceStack;
using GrassrootsFloodCtrl.ServiceModel.Village;
using GrassrootsFloodCtrl.ServiceModel.Route;

namespace GrassrootsFloodCtrl.ServiceModel.RouteNoVerify
{
    [Route("/NoVerifyTown/NoVerifyGetTownList", "Get", Summary = "获取镇街责任人列表接口")]
    [Api("镇街责任人相关接口")]
    public class NoVerifyGetTownList : PageQuery, IReturn<BsTableDataSource<TownPersonLiableViewModel>>
    {
        [ApiMember(IsRequired = false, Description = "自增Id", DataType = "int")]
        public int Id { get; set; }
        [ApiMember(IsRequired = false, Description = "岗位", DataType = "string")]
        public string Position { get; set; }
        [ApiMember(IsRequired = false, Description = "职务", DataType = "string")]
        public string Post { get; set; }
        [ApiMember(IsRequired = false, Description = "姓名", DataType = "string")]
        public string name { get; set; }
        [ApiMember(IsRequired = true, Description = "年度", DataType = "int")]
        public int? year { get; set; }
        [ApiMember(IsRequired = false, Description = "镇adcd", DataType = "string")]
        public string adcd { get; set; }
        [ApiMember(IsRequired = false, Description = "当前提交次数", DataType = "int")]
        public int? nums { get; set; }
    }
}
