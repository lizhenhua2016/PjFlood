using GrassrootsFloodCtrl.Logic.NoAuthticationForAudit;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.Audit;
using GrassrootsFloodCtrl.ServiceModel.Common;

using GrassrootsFloodCtrl.ServiceModel.NoAuditRoute;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceInterface.NoAuthtication
{    
    public class AuditService:ServiceBase
    {
        public NoAuthticationIAuditManager NoAuthticationAuditManager { get; set; }
        public BaseResult POST(NoAuthticationAuditApplication request)
        {
            return NoAuthticationAuditManager.NoAuthticationAuditApplication(request);
        }
        public AuditViewModel GET(NoAuthticationGetAuditResult request)
        {
            return NoAuthticationAuditManager.NoAuthticationGetAuditResult(request);
        }
        public BsTableDataSource<AuditViewModel> GET(NoAuthticationGetAuditApplication request)
        {
            return NoAuthticationAuditManager.NoAuthticationGetAuditApplication(request);
        }

        public BaseResult POST(NoAuthticationPostAudit request)
        {
            return NoAuthticationAuditManager.NoAuthticationPostAudit(request);
        }

        public List<ADCDDisasterViewModel> GET(NoAuthticationGetTownVillage request)
        {
            return NoAuthticationAuditManager.NoAuthticationGetTownVillage(request);
        }

        public BaseResult POST(NoAuthticationAuditNo request)
        {
            return NoAuthticationAuditManager.NoAuthticationAuditNo(request);
        }
        public List<AuditViewModel> GET(NoAuthticationGetAuditNo request)
        {
            return NoAuthticationAuditManager.NoAuthticationGetAuditNo(request);
        }
        public List<ADCDInfo> POST(NoAuthticationGetAreaList request)
        {
            return NoAuthticationAuditManager.NoAuthticationGetAreaList(request);
        }
        public AuditOtherViewModel GET(NoAuthticationGetAuditNumByADCD rquest)
        {
            return NoAuthticationAuditManager.NoAuthticationGetAuditNumByADCD(rquest);
        }
        public BsTableDataSource<AuditCountyViewModel> GET(NoAuthticationGetAuditApplicationCounty request)
        {
            return NoAuthticationAuditManager.NoAuthticationGetAuditApplicationCounty(request);
        }
    }
}
