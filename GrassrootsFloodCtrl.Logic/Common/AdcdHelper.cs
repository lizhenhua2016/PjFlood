using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GrassrootsFloodCtrl.Logic.Common
{
    public static class AdcdHelper
    {
        /// <summary>
        /// 手机端根据adcd判断属于那个权限
        /// </summary>
        /// <param name="adcd"></param>
        /// <returns></returns>
        public static string GetByAdcdRole(string adcd)
        {
            if (adcd.Substring(2, 13) == "0000000000000")
            {
                return "省级";
            }
            else if (adcd.Substring(4, 11) == "00000000000")
            {
                return "市级";
            }
            else if (adcd.Substring(6, 9) == "000000000")
            {
                return "县级";
            }
            else if (adcd.Substring(9, 6) == "000000")
            {
                return "镇级";
            }
            else
            {
                return "村级";
            }

        }

    }
}