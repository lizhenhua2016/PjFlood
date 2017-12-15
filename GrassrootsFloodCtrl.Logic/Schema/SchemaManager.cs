using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model.Schema;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.Logic.Common;

namespace GrassrootsFloodCtrl.Logic.Schema
{
    public class SchemaManager : ManagerBase, ISchemaManager
    {
        public List<ColumnInfo> GetColumns(string tableName, string schema = "")
        {
            using (var db = DbFactory.Open())
            {
                return db.Select<ColumnInfo>(
                    string.Format("select * from INFORMATION_SCHEMA.columns where table_name = '{0}' {1}",
                        tableName.ToLower(), string.IsNullOrEmpty(schema) ? "" : " and table_schema = '" + schema + "'"));
            }
        }

        public void AddColumnIfNotExist(Type modelType, string propName)
        {
            using (var db = DbFactory.Open())
            {
                var modelDef = OrmLiteUtils.GetModelDefinition(modelType);
                var strategy = ((OrmLiteConnectionFactory) DbFactory).DialectProvider.NamingStrategy;
                var tableName = strategy.GetTableName(modelDef);
                var fieldDef = modelDef.GetFieldDefinition(propName);
                var fieldName = "";
                if (fieldDef != null)
                    fieldName = fieldDef.Alias;

                if (string.IsNullOrEmpty(fieldName))
                    fieldName = strategy.GetColumnName(propName);

                var columns = GetColumns(tableName);
                var colDic = columns.ToDictionary(x => x.ColumnName);
                if (!colDic.ContainsKey(fieldName))
                {
                    fieldDef.IsNullable = true;
                    db.AddColumn(modelType, fieldDef);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <param name="tableName">真实表名</param>
        /// <param name="primaryKey">主键 </param>
        /// <param name="AutoIncrement">主键是否自增长 </param>
        public void CreateNewTableIfNotExists(Type modelType, string tableName, string primaryKey, bool AutoIncrement)
        {
            using (var db = DbFactory.Open())
            {
                var tbName = !string.IsNullOrEmpty(tableName) ? tableName : checkField.LetterToLower(modelType.Name);
                if (!db.TableExists(tbName))
                {
                    PropertyInfo[] props = modelType.GetProperties();
                    if (props.Length > 0)
                    {
                        var createSql = "CREATE TABLE " + tbName
                                        + "(";
                        // 遍历属性
                        foreach (PropertyInfo prop in props)
                        {
                            if (checkField.LetterToLower(prop.Name) == primaryKey)
                                createSql += checkField.LetterToLower(prop.Name) + " " +
                                             (AutoIncrement ? "serial" : checkField.FieldType(prop.PropertyType.Name)) +
                                             " PRIMARY KEY,";
                            else
                                createSql += checkField.LetterToLower(prop.Name) + " " +
                                             checkField.FieldType(prop.PropertyType.Name) + " ,";
                        }
                        createSql = createSql.TrimEnd(',');

                        createSql += "); ";
                        db.ExecuteSql(createSql);
                    }
                }
            }
        }
    }
}
