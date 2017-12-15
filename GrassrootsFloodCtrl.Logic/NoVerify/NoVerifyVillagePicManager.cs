using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dy.Common;
using GrassrootsFloodCtrl.Logic.ZZTX;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack.OrmLite;
using ServiceStack.Redis.Pipeline;
using GrassrootsFloodCtrl.Logic.Common;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;

namespace GrassrootsFloodCtrl.Logic.NoVerify
{
    public class NoVerifyVillagePicManager : ManagerBase, INoVerifyVillagePicManager
    {
        public BsTableDataSource<VillageViewModel> GetVillagePicAdcd(NoVerifyGetVillagePicAdcd request)
        {
            using (var db = DbFactory.Open())
            {
                var year = DateTime.Now.Year;
                if (request.year != 0)
                    year = request.year;
                var builder = db.From<ADCDInfo>();
                builder.LeftJoin<ADCDInfo, VillagePic>((x, y) => x.adcd == y.adcd);
                builder.Where(x => x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd);

                if (!string.IsNullOrEmpty(request.adnm))
                    builder.And(x => x.adnm.Contains(request.adnm));
                if (!string.IsNullOrEmpty(request.adcd))
                    builder.And(x => x.adcd == request.adcd);
                builder.SelectDistinct(x => new { x.Id, x.adcd, x.adnm });
                if (request.type == 0)//0：未上报，1：已上报
                    builder.And<VillagePic>(y => y.path == string.Empty || y.path == null);
                else if (request.type == 1)
                    builder.And<VillagePic>(y => y.path != "" && y.Year == year);
                else if (request.type == 2)
                {
                }
                else
                    throw new Exception("上报状态不正确");

                var count = db.Select(builder).Count;
                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(x => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) &&
                         request.Order == "desc")
                    builder.OrderByDescending(x => request.Sort);
                else
                    builder.OrderBy(x => x.adcd);

                var rows = request.PageSize == 0 ? 10 : request.PageSize;
                var skip = request.PageIndex == 0 ? 0 : (request.PageIndex - 1) * rows;

                builder.Limit(skip, rows);
                var list = db.Select<VillageViewModel>(builder);
                return new BsTableDataSource<VillageViewModel>() { total = count, rows = list };

            }
        }

        /// <summary>
        /// 获取已上报的行政村防汛防台形势图列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<VillagePicViewModel> GetVillagePicList(NoVerifyGetVillagePicList request)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<VillagePic>();
                if (request.year == 0)
                    throw new Exception("年度不正确");

                builder.LeftJoin<VillagePic, ADCDInfo>((x, y) => x.adcd == y.adcd);
                builder.Where(x => x.Year == request.year && x.adcd.StartsWith(adcd.Substring(0, 9)) && x.adcd != adcd);
                if (!string.IsNullOrEmpty(request.adnm))
                    builder.And<ADCDInfo>(y => y.adnm.Contains(request.adnm));
                builder.Select<VillagePic, ADCDInfo>((x, y) => new { Id = x.Id, adcd = x.adcd, adnm = y.adnm, path = x.path, Year = x.Year, CreatTime = x.CreatTime, operateLog = x.operateLog });
                var count = db.Select(builder).Count;
                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(x => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) &&
                         request.Order == "desc")
                    builder.OrderByDescending(x => request.Sort);
                else
                    builder.OrderBy(x => x.adcd);

                var rows = request.PageSize == 0 ? 10 : request.PageSize;
                var skip = request.PageIndex == 0 ? 0 : (request.PageIndex - 1) * rows;

                builder.Limit(skip, rows);
                var list = db.Select<VillagePicViewModel>(builder);
                return new BsTableDataSource<VillagePicViewModel>() { total = count, rows = list };
            }
        }

        public VillagePic NoVerifyGetVillagePicByAdcdAndYear(NoVerifyGetVillagePicByAdcdAndYear request)
        {         
                using (var db = DbFactory.Open())
                {
                    var builder = db.From<VillagePic>();
                    if (request.year == 0)
                        throw new Exception("年度不正确");
                    if (string.IsNullOrEmpty(request.adcd) && request.adcd.Length == 15)
                        throw new Exception("行政区划编码为空或不规范");
                    builder.Where(x => x.Year == request.year && x.adcd == request.adcd);
                    if (request.nums != null && request.nums > 1) builder.Where(x => x.AuditNums == request.nums);
                    return db.Single(builder);
                }
      
        }
    }
}
