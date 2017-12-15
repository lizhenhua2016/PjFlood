using GrassrootsFloodCtrl.Logic.Audit;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.Audit;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    [Authenticate]
    public class AuditService:ServiceBase
    {
        public IAuditManager AuditManager { get; set; }
        public BaseResult POST(AuditApplication request)
        {
           return AuditManager.AuditApplication(request);
        }
        public AuditViewModel GET(GetAuditResult request)
        {
            return AuditManager.GetAuditResult(request);
        }
        public BsTableDataSource<AuditViewModel> GET(GetAuditApplication request)
        {
            return AuditManager.GetAuditApplication(request);
        }

        public BaseResult POST(PostAudit request)
        {
            return AuditManager.PostAudit(request);
        }

        public List<ADCDDisasterViewModel> GET(GetTownVillage request)
        {
            return AuditManager.GetTownVillage(request);
        }

        public BaseResult POST(AuditNo request)
        {
            return AuditManager.AuditNo(request);
        }
        public List<AuditViewModel> GET(GetAuditNo request)
        {
            return AuditManager.GetAuditNo(request);
        }
        public List<ADCDInfo> POST(GetAreaList request)
        {
            return AuditManager.GetAreaList(request);
        }
        public AuditOtherViewModel GET(GetAuditNumByADCD rquest)
        {
            return AuditManager.GetAuditNumByADCD(rquest);
        }
        public BsTableDataSource<AuditCountyViewModel> GET(GetAuditApplicationCounty request)
        {
            return AuditManager.GetAuditApplicationCounty(request);
        }
    }
}
