using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Position;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.Model.Enums;

namespace GrassrootsFloodCtrl.Logic.Position
{
    public class PositionManager : ManagerBase, IPositionManager
    {
        public BsTableDataSource<PositionViewModel> GetPositionList(GetPositionList request)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<Model.Position.Position>();
                switch (request.typeId)
                {
                    //0:行政村信息，1：行政村防汛防台工作组，2：行政村网格责任人，3：行政村危险区人员转移清单，4：镇级防汛防台责任人
                    case 0:
                        break;
                    case 1://行政村防汛防台工作组
                        builder.Where(x => x.PositionType == GrassrootsFloodCtrlEnums.ZZTXEnums.行政村防汛防台工作组.ToString());
                        break;
                    case 2://行政村网格责任人
                        builder.Where(x => x.PositionType == GrassrootsFloodCtrlEnums.ZZTXEnums.行政村网格责任人.ToString());
                        break;
                    case 3://行政村危险区人员转移清单
                        builder.Where(x => x.PositionType == GrassrootsFloodCtrlEnums.ZZTXEnums.行政村危险区人员转移清单.ToString());
                        break;
                    case 4://镇级防汛防台责任人
                        builder.Where(x => x.PositionType == GrassrootsFloodCtrlEnums.ZZTXEnums.镇级防汛防台责任人.ToString());
                        break;
                    case 5://镇级防汛防台责任人
                        builder.Where(x => x.PositionType == GrassrootsFloodCtrlEnums.ZZTXEnums.县级防汛防台责任人.ToString());
                        break;
                    default:
                        throw new Exception("类型不正确");
                }
                var count = db.Select(builder).Count;

                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(o => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                    builder.OrderByDescending(o => request.Sort);
                else
                    builder.OrderBy(o => o.orderId);

                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex - 1) * PageSize;
                builder.Limit(PageIndex, PageSize);
                var RList = db.Select<PositionViewModel>(builder);

                return new BsTableDataSource<PositionViewModel>() { rows = RList, total = count };
            }
        }
    }
}
