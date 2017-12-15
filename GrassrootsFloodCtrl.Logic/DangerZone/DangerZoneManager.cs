using System;
using System.Collections.Generic;
using Dy.Common;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.DangerZone;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.DangerZone;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.ServiceModel.Route;

namespace GrassrootsFloodCtrl.Logic
{
  public  class DangerZoneManager: ManagerBase,IDangerZoneManager
    {
      /// <summary>
      /// 获取危险点类型列表
      /// </summary>
      /// <param name="request"></param>
      /// <returns></returns>
      public BsTableDataSource<DangerZoneViewModel> GetDangerZoneList(GetDangerZoneList request)
      {
          using (var db=DbFactory.Open())
          {
                var builder = db.From<DangerZone>();
                builder.LeftJoin<DangerZone, ADCDInfo>((x, y) => x.adcd == y.adcd);
                if (!string.IsNullOrEmpty(request.adcd))
                  builder.And(x=>x.adcd==request.adcd);
                if (!string.IsNullOrEmpty(request.name))
                    builder.And(x => x.DangerZoneName.Contains(request.name));
                builder.Select<DangerZone, ADCDInfo>((x, y)=>new {Id=x.Id,adcd=x.adcd,adnm=y.adnm, operateLog=x.operateLog});
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
                var list = db.Select<DangerZoneViewModel>(builder);
                return new BsTableDataSource<DangerZoneViewModel>() { total = count, rows = list };
            }
      }

        /// <summary>
        /// 获取危险点类型列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<DangerZoneViewModel> GetDangerZone(GetDangerZone request)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<DangerZone>();
                builder.LeftJoin<DangerZone, ADCDInfo>((x, y) => x.adcd == y.adcd);
                if (!string.IsNullOrEmpty(request.adcd))
                    builder.And(x => x.adcd == request.adcd);
                if (!string.IsNullOrEmpty(request.name))
                    builder.And(x => x.DangerZoneName.Contains(request.name));

                return db.Select<DangerZoneViewModel>(builder);
               
            }
        }


        /// <summary>
        /// 获取危险点类型列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DangerZone GetDangerZoneById(int  id)
        {
            using (var db = DbFactory.Open())
            {
                return db.Single<DangerZone>(x=>x.Id==id);
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool SaveDangerZone(SaveDangerZone request)
      {
          using (var db=DbFactory.Open())
          {
              var info=new DangerZone();
              info.adcd = request.adcd;
              info.DangerZoneName = request.name;
              var log=new operateLog();
              log.userName = RealName;
              log.operateTime = DateTime.Now;
              var logList=new List<operateLog>();

              if (request.id != 0)
              {
                  var model = GetDangerZoneById(request.id);
                  if (model == null)
                      throw new Exception("该类型不存在");
                  info.Id = request.id;
                  log.operateMsg = "编辑id为：" + request.id + "原类型为:" + info.DangerZoneName + ",现类型为：" + request.name +
                                   "的危险点类型数据";
                  logList.Add(log);
                  info.operateLog = JsonTools.ObjectToJson(logList);
                  return db.Update(info)==1;
              }
              else
              {
                    log.operateMsg = "新增类型为：" + request.name + "的危险点类型数据";
                    logList.Add(log);
                    info.operateLog = JsonTools.ObjectToJson(logList);
                    return db.Insert(info) == 1;
              }
          }
      }


    }
}
