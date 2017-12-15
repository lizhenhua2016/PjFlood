using GrassrootsFloodCtrl.Logic.Org;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Org;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    [Authenticate]
   public class OrgService: ServiceBase
    {
        public IColumnManager ColumnManager { get; set; }
        public BsTableDataSource<ColumnManageViewModel> Get(GetColumnList request)
        {
            return ColumnManager.GetColumnList(request);
        }
        public BaseResult POST(ColumnSave request)
        {
            return ColumnManager.ColumnSave(request);
        }
        public BaseResult POST(ColumnDel request)
        {
            return ColumnManager.ColumnDel(request);
        }
        public BsTableDataSource<RoleDetailViewModel> Get(GetRoleDetaileList request)
        {
            return ColumnManager.GetRoleDetaileList(request);
        }
    }
}
