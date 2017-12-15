using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.Common
{
  public class ExcelHeadAttribute
    {
        /// <summary>
        /// 合并的行号
        /// </summary>
        public int rowIndex { get; set; }
        /// <summary>
        /// 第一个行
        /// </summary>
        public int firstRow { get; set; }
        /// <summary>
        /// 最后一个行
        /// </summary>
        public int lastRow { get; set; }
        /// <summary>
        /// 第一个列
        /// </summary>
        public int firstCol { get; set; }
        /// <summary>
        /// 最后一个列
        /// </summary>
        public int lastCol { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 单元格高度
        /// </summary>
        public int HeightInPoints { get; set; }
        /// <summary>
        /// 文字大小
        /// </summary>
        public short fontSize { get; set; }
        /// <summary>
        /// 文字颜色：这里只能用NOPI特定的类型
        /// </summary>
        public short fontColor { get; set; }
    }
}
