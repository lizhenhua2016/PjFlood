using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.Audit;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.Audit
{
   public interface IAuditManager
    {
        AuditViewModel GetAuditResult(GetAuditResult request);
        BaseResult AuditApplication(AuditApplication request);
        BsTableDataSource<AuditViewModel> GetAuditApplication(GetAuditApplication request);
        BaseResult PostAudit(PostAudit request);
        List<ADCDDisasterViewModel> GetTownVillage(GetTownVillage request);
        BaseResult AuditNo(AuditNo request);
        List<AuditViewModel> GetAuditNo(GetAuditNo request);
        List<ADCDInfo> GetAreaList(GetAreaList request);

        AuditOtherViewModel GetAuditNumByADCD(GetAuditNumByADCD request);
        BsTableDataSource<AuditCountyViewModel> GetAuditApplicationCounty(GetAuditApplicationCounty request);
    }
}
