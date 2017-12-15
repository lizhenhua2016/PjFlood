using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.Model.Common;
using Dy.Common;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using GrassrootsFloodCtrl.ServiceModel.Post;
using System.Data;
using GrassrootsFloodCtrl.ServiceModel.Grid;
using System.Collections;
using GrassrootsFloodCtrl.Logic.Common;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;
using GrassrootsFloodCtrl.ServiceModel.Position;
using GrassrootsFloodCtrl.Model.Supervise;
using Aspose.Cells;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;

namespace GrassrootsFloodCtrl.Logic.NoVerify
{
    public class NoVerifyVillageGridManage : ManagerBase, INoVerifyVillageGridManage
    {
        public BsTableDataSource<VillageGridViewModel> NoVerifyGetVillageGrid(NoVerifyGetVillageGrid request)
        {
            using (var db = DbFactory.Open())
            {
                //if (string.IsNullOrEmpty(adcd))
                //{ throw new Exception("请重新登录"); }
                var builder = db.From<VillageGridPersonLiable>();
                builder.LeftJoin<VillageGridPersonLiable, ADCDInfo>((x, y) => x.VillageADCD == y.adcd);
                var _year = null != request.year && !string.IsNullOrEmpty(request.year.ToString()) ? request.year : System.DateTime.Now.Year;
                builder.Where<VillageGridPersonLiable>(x => x.Year == _year && x.VillageADCD == request.adcd);
                if (request.nums != null && request.nums > 1) builder.Where<VillageGridPersonLiable>(w => w.AuditNums == request.nums);
                builder.Select(" VillageGridPersonLiable.*,ADCDInfo.adnm");
                var count = db.Count(builder);

                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(o => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                    builder.OrderByDescending(o => request.Sort);
                else
                    builder.OrderByDescending(o => o.ID);

                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                builder.Limit(PageIndex, PageSize);
                var RList = db.Select<VillageGridViewModel>(builder);

                return new BsTableDataSource<VillageGridViewModel>() { rows = RList, total = count };
            }
        }
    }
}
