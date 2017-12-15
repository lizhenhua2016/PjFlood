using System.Collections.Generic;
using GrassrootsFloodCtrl.Logic;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.DangerZone;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    [Authenticate]
    public class DangerZoneService:ServiceBase
    {
        public IDangerZoneManager DangerZoneManager { get; set; }

        public BsTableDataSource<DangerZoneViewModel> Get(GetDangerZoneList request)
        {
            return DangerZoneManager.GetDangerZoneList(request);
        }

        public List<DangerZoneViewModel> Get(GetDangerZone request)
        {
            return DangerZoneManager.GetDangerZone(request);
        }

        public bool Post(SaveDangerZone request)
        {
            return DangerZoneManager.SaveDangerZone(request);
        }
        
    }
}
