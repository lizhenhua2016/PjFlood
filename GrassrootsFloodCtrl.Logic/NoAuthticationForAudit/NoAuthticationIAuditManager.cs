using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.Audit;
using GrassrootsFloodCtrl.ServiceModel.Common;

using GrassrootsFloodCtrl.ServiceModel.NoAuditRoute;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.NoAuthticationForAudit
{
    public interface NoAuthticationIAuditManager
    {
        AuditViewModel NoAuthticationGetAuditResult(NoAuthticationGetAuditResult request);
        BaseResult NoAuthticationAuditApplication(NoAuthticationAuditApplication request);
        BsTableDataSource<AuditViewModel> NoAuthticationGetAuditApplication(NoAuthticationGetAuditApplication request);
        BaseResult NoAuthticationPostAudit(NoAuthticationPostAudit request);
        List<ADCDDisasterViewModel> NoAuthticationGetTownVillage(NoAuthticationGetTownVillage request);
        BaseResult NoAuthticationAuditNo(NoAuthticationAuditNo request);
        List<AuditViewModel> NoAuthticationGetAuditNo(NoAuthticationGetAuditNo request);
        List<ADCDInfo> NoAuthticationGetAreaList(NoAuthticationGetAreaList request);
        AuditOtherViewModel NoAuthticationGetAuditNumByADCD(NoAuthticationGetAuditNumByADCD request);
        BsTableDataSource<AuditCountyViewModel> NoAuthticationGetAuditApplicationCounty(NoAuthticationGetAuditApplicationCounty request);
    }
}
