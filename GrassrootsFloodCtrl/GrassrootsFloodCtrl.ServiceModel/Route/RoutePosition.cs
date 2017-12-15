using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Position;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Route("/Position/GetPositionList", "GET", Summary = "接口：获取岗位类型")]
    [Api("岗位")]
    public class GetPositionList : PageQuery, IReturn<BsTableDataSource<PositionViewModel>>
    {

        [ApiMember(IsRequired = true, DataType = "int", Description = "组织体系类型（0:行政村信息，1：行政村防汛防台工作组，2：行政村网格责任人，3：行政村危险区人员转移清单，4：镇级防汛防台责任人,5：防汛防台责任人）")]
        public int typeId { get; set; }
    }
}
