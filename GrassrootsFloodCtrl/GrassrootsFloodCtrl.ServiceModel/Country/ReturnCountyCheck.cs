using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.Country
{
    public class ReturnCountyCheck
    {
        public int CheckStatus { get; set; }

        public bool IsSuccess { get; set; }

        public string CheckSuggest { get; set; }
    }
}
