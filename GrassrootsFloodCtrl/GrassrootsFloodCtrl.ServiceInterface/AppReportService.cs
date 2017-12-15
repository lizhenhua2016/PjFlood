using System.Collections.Generic;
using GrassrootsFloodCtrl.Logic.AppReport;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.AppReport;
using GrassrootsFloodCtrl.ServiceModel.Route;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    public class AppReportService : ServiceBase
    {
        public AppReportManage appreportmanage { get; set; }

        public BsTableDataSource<AppReportByVillageAdcdAndWarninfoid> post(RouteAppReportByVillageAdcdAndWarninfoid request)
        {
            return appreportmanage.AppReportByVillageAdcdAndWarninfoid(request);
        }

        public List<AppPostCode> post(RouteAppDutyPost request)
        {
            return appreportmanage.AppGetPostCode(request);
        }

        public List<AppVillageOnDuty> post(RouteAppRecordByMessageId request)
        {
            return appreportmanage.AppGetRecord(request);
        }

        public List<AppLocation> post(RouteAppLocation request)
        {
            return appreportmanage.AppGetLocation(request);
        }

        //public bool post(RouteGetRegMobile request)
        //{
        //    return appreportmanage.AppGetRegMobile(request);
        //}
        
    }
}