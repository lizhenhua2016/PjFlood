using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Audit
{
   public class AuditViewModel
    {
        #region audit
        public int? ID { get; set; }

        public int? Year { get; set; }

        public string TownADCD { get; set; }

        public DateTime? TownAddTime { get; set; }

        public int? Status { get; set; }

        public string operateLog { get; set; }

        public int? AuditID { get; set; }

        public string AuditADCD { get; set; }

        public int? AuditRole { get; set; }

        public DateTime? AuditTime { get; set; }

        public string Remarks { get; set; }

        public int? AuditNums { get; set; }
        #endregion
        #region adcdinfo
        public string adnm { get; set; }
       public string adcd { get; set; }
        #endregion
        public string statusname { get; set; }

    }
}
