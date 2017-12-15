using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    public class PageQuery
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        [ApiMember(IsRequired = false, DataType = "int", Description = "每页数量")]
        public int PageSize { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        [ApiMember(IsRequired = false, DataType = "int", Description = "第几页")]
        public int PageIndex { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        [ApiMember(IsRequired = false, DataType = "string", Description = "排序字段")]
        public string Sort { get; set; }

        /// <summary>
        /// asc  或者 desc
        /// </summary>
        [ApiMember(IsRequired = false, DataType = "string", Description = "升序 降序 asc desc")]
        public string Order { get; set; }
       
    }
}
