using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Grid;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    [Route("/Grid/GetList", "GET", Summary = "接口：获取网格类型")]
    [Api("岗位")]
    public class GetGridList : PageQuery, IReturn<BsTableDataSource<GridViewModel>>
    {

    }
}
