using System.Collections.Generic;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.AppReport;
using GrassrootsFloodCtrl.ServiceModel.Route;

namespace GrassrootsFloodCtrl.Logic.AppReport
{
    internal interface IAppReportManage
    {
        BsTableDataSource<AppReportByVillageAdcdAndWarninfoid> AppReportByVillageAdcdAndWarninfoid(RouteAppReportByVillageAdcdAndWarninfoid request);
        List<AppPostCode> AppGetPostCode(RouteAppDutyPost request);
        List<AppVillageOnDuty> AppGetRecord(RouteAppRecordByMessageId request);
        List<AppLocation> AppGetLocation(RouteAppLocation request);
        //bool AppGetRegMobile(RouteGetRegMobile request);
    }
}