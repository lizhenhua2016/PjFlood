using GrassrootsFloodCtrl.Logic.NoVerify;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Logic.Excel;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Excel;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    public class NoVerifyExcelService:ServiceBase
    {
        public INoVerifyExcelManager NoVerifyExcelManager { get; set; }

        public UploadFileViewModel Get(NoVerifyexportExcel request)
        {
            return NoVerifyExcelManager.exportExcel(request);
        }
        public UploadFileViewModel Get(NoVerifydownLoadExcelModel request)
        {
            return NoVerifyExcelManager.downLoadExcelModel(request);
        }

        public ExcelViewModel Post(NoVerifyImportExcel request)
        {
            //var files=Request.Files;

            return NoVerifyExcelManager.ImportExcel(request);
        }


    }
}
