using GrassrootsFloodCtrl.Model.AppApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppGetByUserExecutingModel
    {
        public int StateCode { get; set; }
        public string Message { get; set; }
        public List<AppMessageModel> ExecutingList { get; set; }
        public List<AppMessageModel> NoReadList { get; set; }

    }
}