using GrassrootsFloodCtrl.Model.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.Schema
{
   public interface ISchemaManager
    {
        /// <summary>
        /// 读取某表的结构信息。
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="schema">架构名称，可空。</param>
        /// <returns>基本表结构信息</returns>
        List<ColumnInfo> GetColumns(string tableName, string schema = "");

        /// <summary>
        /// 添加字段，如果不存在时。
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <param name="propName">属性名称</param>
        void AddColumnIfNotExist(Type modelType, string propName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <param name="tableName">真实表名</param>
        /// <param name="primaryKey">主键 </param>
        /// <param name="AutoIncrement">主键是否自增长 </param>
        void CreateNewTableIfNotExists(Type modelType, string tableName, string primaryKey, bool AutoIncrement);
    }
}
