using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Grid;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack.OrmLite;

namespace GrassrootsFloodCtrl.Logic.Grid
{
    public class GridManager : ManagerBase, IGridManager
    {
        public BsTableDataSource<GridViewModel> GetGridList(GetGridList request)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<Model.Grid.Grid>();

                var count = db.Select(builder).Count;

                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(o => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                    builder.OrderByDescending(o => request.Sort);
                else
                    builder.OrderBy(o => o.ID);

                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex - 1) * PageSize;
                builder.Limit(PageIndex, PageSize);
                var RList = db.Select<GridViewModel>(builder);

                return new BsTableDataSource<GridViewModel>() { rows = RList, total = count };
            }
        }
    }
}
