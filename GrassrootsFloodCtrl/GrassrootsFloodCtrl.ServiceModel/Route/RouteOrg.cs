using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Org;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    #region 栏目管理
    [Route("/Org/GetColumnList", "GET", Summary = "接口：栏目列表")]
    [Api("栏目管理")]
    public class GetColumnList : PageQuery, IReturn<BsTableDataSource<ColumnManageViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户名")]
        public string username { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "父id")]
        public int? id { get; set; }

    }
    [Route("/Org/ColumnSave", "POST", Summary = "接口：新增栏目")]
    [Api("栏目管理")]
    public class ColumnSave : IReturn<BaseResult>
    {
        [ApiMember(IsRequired = false, DataType = "int", Description = "id")]
        public int? cid { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "父栏目id")]
        public int? pid { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "栏目名")]
        public string cname { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "操作权限")]
        public string actions { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "栏目地址")]
        public string url { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "图标样式")]
        public string ico { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "是否启用")]
        public int? visible { get; set; }
        [ApiMember(IsRequired = false, DataType = "int", Description = "排序")]
        public int? csort { get; set; }
        [ApiMember(IsRequired = false, DataType = "string", Description = "等级")]
        public string level { get; set; }
    }
    [Route("/Org/ColumnDel", "POST", Summary = "接口:删除栏目")]
    [Api("栏目删除")]
    public class ColumnDel : IReturn<BaseResult>{
        [ApiMember(IsRequired = true, DataType = "string", Description = "栏目id")]
        public string id { get; set; }
    }
    #endregion
    #region 角色权限管理
    [Route("/Org/GetRoleDetaileList", "GET", Summary = "接口：栏目列表")]
    [Api("栏目管理")]
    public class GetRoleDetaileList : PageQuery, IReturn<BsTableDataSource<RoleDetailViewModel>>
    {
        [ApiMember(IsRequired = true, DataType = "int", Description = "角色id")]
        public int? rid { get; set; }

    }

    //[Route("/Org/RoleDetaileSave", "POST", Summary = "接口：新增栏目")]
    //[Api("栏目管理")]
    //public class RoleDetaileSave : IReturn<BaseResult>
    //{
    //    [ApiMember(IsRequired = false, DataType = "int", Description = "id")]
    //    public int? cid { get; set; }
    //    [ApiMember(IsRequired = true, DataType = "int", Description = "父栏目id")]
    //    public int? pid { get; set; }
    //    [ApiMember(IsRequired = true, DataType = "string", Description = "栏目名")]
    //    public string cname { get; set; }
    //    [ApiMember(IsRequired = true, DataType = "string", Description = "操作权限")]
    //    public string actions { get; set; }
    //    [ApiMember(IsRequired = true, DataType = "string", Description = "栏目地址")]
    //    public string url { get; set; }
    //    [ApiMember(IsRequired = false, DataType = "string", Description = "图标样式")]
    //    public string ico { get; set; }
    //    [ApiMember(IsRequired = true, DataType = "int", Description = "是否启用")]
    //    public int? visible { get; set; }
    //    [ApiMember(IsRequired = false, DataType = "int", Description = "排序")]
    //    public int? csort { get; set; }
    //    [ApiMember(IsRequired = false, DataType = "string", Description = "等级")]
    //    public string level { get; set; }
    //}
    //[Route("/Org/RoleDetaileDel", "POST", Summary = "接口:删除栏目")]
    //[Api("栏目删除")]
    //public class RoleDetaile : IReturn<BaseResult>
    //{
    //    [ApiMember(IsRequired = true, DataType = "string", Description = "栏目id")]
    //    public string id { get; set; }
    //}
    #endregion
}
