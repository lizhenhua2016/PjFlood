using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Org;
using GrassrootsFloodCtrl.ServiceModel.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.Org
{
   public interface IColumnManager
    {
        BsTableDataSource<ColumnManageViewModel> GetColumnList(GetColumnList request);
        BaseResult ColumnSave(ColumnSave request);
        BaseResult ColumnDel(ColumnDel request);

        BsTableDataSource<RoleDetailViewModel> GetRoleDetaileList(GetRoleDetaileList request);
    }
}
