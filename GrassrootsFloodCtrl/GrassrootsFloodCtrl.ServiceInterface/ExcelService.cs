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
    [Authenticate]
    public class ExcelService:ServiceBase
    {
       public IExcelManager ExcelManager { get; set; }

       public UploadFileViewModel Get(downLoadExcelModel request)
       {
           return ExcelManager.downLoadExcelModel(request);
       }

       public ExcelViewModel Post(ImportExcel request)
       {
            //var files=Request.Files;
            
            return ExcelManager.ImportExcel(request);
        }

        public UploadFileViewModel Get(exportExcel request)
        {
            return ExcelManager.exportExcel(request);
        }

    }
}
