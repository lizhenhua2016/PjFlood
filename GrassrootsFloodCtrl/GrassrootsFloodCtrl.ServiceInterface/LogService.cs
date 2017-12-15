using GrassrootsFloodCtrl.Logic.Log;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceInterface
{
   public class LogService : ServiceBase
    {
        public ILogMyManager LogMyManager { get; set; }
        public SuperviseModel POST(GetLogStatisList request)
        {
            return LogMyManager.GetLogStatisList(request);
        }
        public BsTableDataSource<LogInfoViewModel> GET(GetLogList request)
        {
            return LogMyManager.GetLogList(request);
        }
        public BaseResult GET(GetAuditDate request)
        {
            return LogMyManager.GetAuditDate(request);
        }
    }
}
