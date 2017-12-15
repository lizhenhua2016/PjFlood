using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.Common
{
   public class ConverterHelper
    {
       public static string ObjectToString(object obj)
       {
           if (obj != null)
           {
                if (obj.ToString().Trim().ToLower()=="null")
                    return "";
                else
                    return obj.ToString().Trim();
            }
           else
               return "";
       }
    }
}
