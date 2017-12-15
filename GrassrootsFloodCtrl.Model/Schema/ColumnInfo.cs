using ServiceStack.DataAnnotations;

namespace GrassrootsFloodCtrl.Model.Schema
{
    /// <summary>
    /// sql表结构信息
    /// </summary>
    public class ColumnInfo
    {
        [Alias("TABLE_CATALOG")]
        public string TableCatalog { get; set; }

        [Alias("TABLE_SCHEMA")]
        public string TableSchema { get; set; }

        [Alias("TABLE_NAME")]
        public string TableName { get; set; }

        [Alias("COLUMN_NAME")]
        public string ColumnName { get; set; }

        [Alias("COLUMN_DEFAULT")]
        public string ColumnDefault { get; set; }

        [Alias("IS_NULLABLE")]
        public string Nullable { get; set; }

        [Alias("DATA_TYPE")]
        public string DataType { get; set; }
    }
}