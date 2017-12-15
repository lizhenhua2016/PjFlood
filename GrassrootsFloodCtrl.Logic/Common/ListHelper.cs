using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace GrassrootsFloodCtrl.Logic.Common
{
    public static class ListHelper
    {
        /// <summary>
        /// 将泛类型集合List类转换成DataTable
        /// </summary>
        /// <param name="list">泛类型集合</param>
        /// param name="arr">不需要的列 从0开始 </param>
        /// param name="dicList">ture fasle需要转成的中文集合 按照顺序从第0列开始匹配</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> entitys,ArrayList arr=null, List<Dictionary<bool, string>> dicList = null)
        {
            
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                throw new Exception("需转换的集合为空");
            }
            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                if (arr != null)
                {
                    if (!arr.Contains(i))
                        dt.Columns.Add(entityProperties[i].Name);
                }
                else
                    dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                var len = entityProperties.Length;
                if (arr != null)
                    len = len - arr.Count;
                object[] entityValues = new object[len];
                var j = 0;
                
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    var k = -1;
                    if (arr != null)
                    {
                        if (!arr.Contains(i))
                        {
                            if (dicList!=null&&dicList.Count > 0)
                            {
                                var value = entityProperties[i].GetValue(entity, null);
                                if (value!=null&&(value.ToString().ToLower() == "true" || value.ToString().ToLower() == "false"))
                                {
                                    for (var m=0;m< dicList.Count;m++)
                                    {
                                        if (m > k)
                                        {
                                            var dic = dicList[m];
                                            foreach (var d in dic)
                                            {
                                                if (value.ToString().ToLower() == d.Key.ToString().ToLower())
                                                {
                                                    k = m;
                                                    value = d.Value;
                                                    break;
                                                    //dicList.Remove(dic);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                entityValues[j] = value;
                            }
                            else
                            {
                                entityValues[j] = entityProperties[i].GetValue(entity, null);
                            }
                            j++;
                        }
                    }
                    else
                        entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }
    }
}
