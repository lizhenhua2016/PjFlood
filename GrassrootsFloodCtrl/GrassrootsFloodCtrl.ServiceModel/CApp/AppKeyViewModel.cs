using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceModel.CApp
{
   public class AppKeyViewModel
    {
        public string ADCD { get; set; }
        public string Name { get; set; }
        public int ctype { get; set; }
    }
    public class TransferPersonViewModel
    {
        public string PersonLiableName { get; set; }
        public string WarnPersonLiableName { get; set; }
        public string DisasterPreventionManager { get; set; }
        public string ADCD { get; set; }
    }
}
