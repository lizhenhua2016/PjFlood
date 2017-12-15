using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Post
{
  public class PostViewModel
    {
        /// <summary>
        /// 岗位ID
        /// </summary>
        public int? ID { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        public string PostName { get; set; }
    }
}
