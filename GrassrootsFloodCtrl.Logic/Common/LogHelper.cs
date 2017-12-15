using Dy.Common;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.Sys;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;

namespace GrassrootsFloodCtrl.Logic.Common
{
    public class LogHelper : ManagerBase, ILogHelper
    {
        public bool WriteLog(string userNperation, OperationTypeEnums type)
        {
            using (var db = DbFactory.Open())
            {
                var logInfo = new LogInfo();
                var operation= new operateLog();
                operation.operateMsg = userNperation;
                operation.operateTime = DateTime.Now;
                operation.userName = UserName;
                logInfo.adcd = adcd;
                logInfo.OperationType = type;
                logInfo.UserName = UserName;
                logInfo.tm = DateTime.Now;
                logInfo.RealName = RealName;
                logInfo.Operation = JsonTools.ObjectToJson(operation);
                return db.Insert<LogInfo>(logInfo) == 1;
            }
        }
    }
}
