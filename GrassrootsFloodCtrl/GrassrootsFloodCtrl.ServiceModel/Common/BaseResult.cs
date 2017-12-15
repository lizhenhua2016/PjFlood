using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Common
{
    public class BaseResult
    {
        public bool IsSuccess { set; get; }
        public string ErrorMsg { set; get; }
        public string vcode { get; set; }
        public string token { get; set; }
        public int? AddID { set; get; }
        public string Others { set; get; }
        public string filepath { get; set; }
        public List<ErrList> ErrorList { get; set; }
        public string ActionName { get; set; }
        public string Adcd { get; set; }
        public string QusetionEndTime { get; set; }
    }
   public class ErrList {
        public int rowid { get; set; }
        public string msg { get; set; }
    }
    public class VillagePerson
    {
        public string adcd { get; set; }
        public string name { get; set; }
        public string Post { get; set; }
        public string gridname { get; set; }
        public string HandPhone { get; set; }
        public string Position { get; set; }
    }
}
