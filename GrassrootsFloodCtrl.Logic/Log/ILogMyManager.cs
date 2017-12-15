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

namespace GrassrootsFloodCtrl.Logic.Log
{
    public interface ILogMyManager
    {
        SuperviseModel GetLogStatisList(GetLogStatisList request);

        BsTableDataSource<LogInfoViewModel> GetLogList(GetLogList request);

        BaseResult GetAuditDate(GetAuditDate request);
    }
}
