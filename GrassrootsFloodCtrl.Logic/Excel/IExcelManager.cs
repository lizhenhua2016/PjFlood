using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Excel;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack.Web;

namespace GrassrootsFloodCtrl.Logic.Excel
{
  public  interface IExcelManager
    {

        /// <summary>
        /// 下载模板
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        UploadFileViewModel downLoadExcelModel(downLoadExcelModel request);
        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="request"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        ExcelViewModel ImportExcel(ImportExcel request);
        /// <summary>
        /// 导出信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        UploadFileViewModel exportExcel(exportExcel request);
    }
}
