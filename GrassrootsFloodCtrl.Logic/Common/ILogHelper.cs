using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;

namespace GrassrootsFloodCtrl.Logic.Common
{
    public interface ILogHelper
    {
        //写日志
        bool WriteLog(string userNperation, OperationTypeEnums type);
    }
}
