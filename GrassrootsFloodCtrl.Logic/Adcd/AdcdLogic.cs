using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model.Adcd;
using GrassrootsFloodCtrl.Logic.Common;
using ServiceStack;
using ServiceStack.OrmLite;

namespace GrassrootsFloodCtrl.Logic.Adcd
{
    public class AdcdLogic : ManagerBase, IAdcdLogic
    {
        public List<AdcdSelectModel> GetCityAdcdSelectList(string adcd)
        {
            using (var db = DbFactory.Open())
            {
                List<AdcdSelectModel> list = new List<AdcdSelectModel>();
                if (AdcdHelper.GetByAdcdRole(adcd) == "省级")
                {
                    list = db.SqlList<AdcdSelectModel>("select adcd,adnm from ADCDInfo where parentId in (select Id from ADCDInfo where adcd='"+adcd+"')");
                }
                if (AdcdHelper.GetByAdcdRole(adcd) == "市级")
                {
                    list = db.SqlList<AdcdSelectModel>("select adcd,adnm from ADCDInfo where adcd='"+adcd+"'");
                }
                return list;
            }

        }

        public List<AdcdSelectModel> GetCountryAdcdSelectList(string adcd)
        {
            using (var db = DbFactory.Open())
            {
                List<AdcdSelectModel> list = new List<AdcdSelectModel>();

                if (AdcdHelper.GetByAdcdRole(adcd) == "市级")
                {
                    list = db.SqlList<AdcdSelectModel>("select adcd,adnm from ADCDInfo where parentId in (select Id from ADCDInfo where adcd='" + adcd + "')");
                }
                if (AdcdHelper.GetByAdcdRole(adcd) == "县级")
                {
                    list = db.SqlList<AdcdSelectModel>("select adcd,adnm from ADCDInfo where adcd='" + adcd + "'");
                }
                return list;
            }
        }
    }
}
