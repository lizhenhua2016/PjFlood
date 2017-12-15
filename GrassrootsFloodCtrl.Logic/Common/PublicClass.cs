using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GrassrootsFloodCtrl.Model.Enums.GrassrootsFloodCtrlEnums;

namespace GrassrootsFloodCtrl.Logic.Common
{
   public static class PublicClass
    {
        public static string GetAudit(int? eid,string ename)
        {
            var r = "";
            try
            {
                if (null != eid)
                {
                  r = Enum.GetName(typeof(AuditStatusEnums), eid);
                }

                if (!string.IsNullOrEmpty(ename))
                {
                   var b =(int)Enum.Parse(typeof(AuditStatusEnums), ename);
                    r= b.ToString();
                }
            }
            catch(Exception ex)
            {
                r = ex.Message;
            }
            return r;
        }
    }
}
