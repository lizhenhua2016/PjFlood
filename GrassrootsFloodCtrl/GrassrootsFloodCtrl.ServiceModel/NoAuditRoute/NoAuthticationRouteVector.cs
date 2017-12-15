using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack;

namespace GrassrootsFloodCtrl.ServiceModel.NoAuditRoute
{
    [Route("/NoAuthticationVectors/Vectors", "Post", Summary = "获取镇街责任人列表接口")]
    [Api("没有权限的镇街责任人相关接口")]
    public class NoAuthticationRouteVector: PageQuery,IReturn<BsTableDataSource<ResponseVectors>>
    {
        [ApiMember(IsRequired = true,DataType = "string", Description = "类型")]
       public int Distype { get; set; }
    }
}