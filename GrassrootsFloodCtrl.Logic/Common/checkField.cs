using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrassrootsFloodCtrl.Logic.Common
{
    public class checkField
    {
        /// <summary>
        /// 大写字母转成小写字母  对应ServiceStack 建表的字段
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static string LetterToLower(string propName)
        {
            var lowerLetter = propName.Substring(0, 1).ToLower();
            var last= propName.Substring(1);
            var a= last.Replace("A", "_a").Replace("B", "_b").Replace("C", "_c").Replace("D", "_d").
                Replace("E", "_e").Replace("F", "_f").Replace("G", "_g").Replace("H", "_h").
                Replace("I", "_i").Replace("J", "_j").Replace("K", "_k").Replace("L", "_l").
                Replace("M", "_m").Replace("N", "_n").Replace("O", "_o").Replace("P", "_p").
                Replace("Q", "_q").Replace("R", "_r").Replace("S", "_s").Replace("T", "_t").
                Replace("U", "_u").Replace("V", "_v").Replace("W", "_w").Replace("X", "_x").
                Replace("Y", "_y").Replace("Z", "_z");
            return lowerLetter + a;
        }

        /// <summary>
        /// 字段类型转换
        /// </summary>
        /// <param name="fieldType">字段类型</param>
        /// <returns></returns>
        public static string FieldType(string fieldType)
        {
            var FieldType = "";
            switch (fieldType)
            {
                case "Int32":
                    FieldType= "INT";
                    break;
                case "String":
                    FieldType = "TEXT";
                    break;
                case "DateTime":
                    FieldType = "timestamp with time zone";
                    break;
                default:
                    FieldType = "TEXT";
                    break;

            }
            return FieldType;
        }
    }
}