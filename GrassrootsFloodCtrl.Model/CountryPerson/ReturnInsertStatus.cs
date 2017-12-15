using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Model.CountryPerson
{
    public class ReturnInsertStatus
    {
        public int Column { get; set; }

        public bool Status { get; set; }

        public string ColumnName { get; set; }
        public string Description { get; set; }
    }
}
