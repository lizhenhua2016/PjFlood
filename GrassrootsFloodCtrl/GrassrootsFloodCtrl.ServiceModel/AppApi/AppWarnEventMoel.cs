using GrassrootsFloodCtrl.Model.AppApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppWarnEventMoel
    {
        public int Total { get; set; }
        public int StateCode { get; set; }
        public string Message { get; set; }
        public dynamic Rows { get; set; }
    }
}