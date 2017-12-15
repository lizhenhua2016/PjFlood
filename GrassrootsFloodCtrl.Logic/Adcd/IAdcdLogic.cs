using GrassrootsFloodCtrl.Model.Adcd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.Adcd
{
    public interface IAdcdLogic
    {
        List<AdcdSelectModel> GetCountryAdcdSelectList(string adcd);
        List<AdcdSelectModel> GetCityAdcdSelectList(string adcd);
    }
}
