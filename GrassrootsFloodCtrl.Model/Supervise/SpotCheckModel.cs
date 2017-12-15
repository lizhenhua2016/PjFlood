using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.Supervise
{
   public class SpotCheckModel
    {
        public int id { get; set; }

        public string adcd { get; set; }

        public string areas { get; set; }

        public int uid { get; set; }

        public string checkman { get; set; }

        public string checkmanrealname { get; set; }

        public DateTime checktime { get; set; }

        public string bycheckman { get; set; }

        public string bycheckphone { get; set; }

        public string checkitems { get; set; }

        public string noremarks { get; set; }

        public string remarks { get; set; }

        public string checkstatus { get; set; }

        public string operateLog { get; set; }

        public int year { get; set; }

        public int IdCount { get; set; }
    }
}
