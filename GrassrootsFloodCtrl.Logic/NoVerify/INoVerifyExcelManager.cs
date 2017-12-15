using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Excel;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack.Web;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;

namespace GrassrootsFloodCtrl.Logic.NoVerify
{
    public interface INoVerifyExcelManager
    {
        /// <summary>
        /// 下载模板
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        UploadFileViewModel downLoadExcelModel(NoVerifydownLoadExcelModel request);
        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="request"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        ExcelViewModel ImportExcel(NoVerifyImportExcel request);
        /// <summary>
        /// 导出信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        UploadFileViewModel exportExcel(NoVerifyexportExcel request);
    }
}
