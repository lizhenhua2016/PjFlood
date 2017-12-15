using System;
using System.Collections.Generic;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.AppApi;
using GrassrootsFloodCtrl.ServiceModel.AppReport;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack.OrmLite;

namespace GrassrootsFloodCtrl.Logic.AppReport
{
    public class AppReportManage : ManagerBase, IAppReportManage
    {
        public BsTableDataSource<AppReportByVillageAdcdAndWarninfoid> AppReportByVillageAdcdAndWarninfoid(
            RouteAppReportByVillageAdcdAndWarninfoid request)
        {
            using (var db = DbFactory.Open())
            {
                var list = db.SqlList<AppReportByVillageAdcdAndWarninfoid>(
                    "exec AppReportByVillageAdcdAndWarninfoid @adcd,@warninfoid",
                    new {adcd = request.adcd, warninfoid = request.warninfoid});

                return new BsTableDataSource<AppReportByVillageAdcdAndWarninfoid> {rows = list, total = list.Count};
            }
        }

        public List<AppPostCode> AppGetPostCode(RouteAppDutyPost request)
        {
            using (var db = DbFactory.Open())
            {
                var list = db.SqlList<AppPostCode>("exec AppGetPostCode @messageid",
                    new {messageid = request.MessageId});
                return list;
            }
        }

        public List<AppVillageOnDuty> AppGetRecord(RouteAppRecordByMessageId request)
        {
            using (var db = DbFactory.Open())
            {
                var list = db.SqlList<AppVillageOnDuty>("exec AppGetRecord @messageid,@postcode",
                    new {messageid = request.MessageId,postcode= request.postcode});
                return list;
            }
        }

        public List<AppLocation> AppGetLocation(RouteAppLocation request)
        {
            using (var db = DbFactory.Open())
            {
                var list = db.SqlList<AppLocation>("exec AppGetlocationByMessageid @messageid",
                    new {messageid = request.messageid});
                return list;
            }
        }


        //public bool AppGetRegMobile(RouteGetRegMobile request)
        //{
        //    using (var db = DbFactory.Open())
        //    {
        //        db.Insert(new AppGetReg() { Id = Guid.NewGuid().ToString(), Mobile = request.mobile });
        //        return true;
        //    }
        //}
    }
}