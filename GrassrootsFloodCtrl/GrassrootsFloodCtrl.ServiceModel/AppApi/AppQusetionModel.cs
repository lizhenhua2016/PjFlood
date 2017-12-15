using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.AppApi
{
    public class AppQusetionModel
    {
        public int StateCode { get; set; }
        public string Message { get; set; }
        public List<AppQuestionDescription> QusetionList { get; set; }
    }

    public class AppQuestionDescription
    {
        public string messageId { get; set; }
        public DateTime? EndTime { get; set; }
        public string GroupName { get; set; }
        public bool IsResumption { get; set; }

    }
}
