using GrassrootsFloodCtrl.Logic.Country;
using GrassrootsFloodCtrl.Logic.NoVerify;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.CountryPerson;
using GrassrootsFloodCtrl.ServiceModel.Country;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    public class NoVerifyCountryPersonService : ServiceBase
    {
        public INoVerifyCountryPersonManager CountryPerson { get; set; }
        //public string Post(AddChangePersons requset)
        //{
        //    return CountryPerson.AddChangePersons(requset);
        //}

        public ReturnInsertStatus Post(NoVerifyCountryImportExcel requset)
        {
            return CountryPerson.InsertExeclCountryPerson(requset);
        }

        public BsTableDataSource<CountryPersonServiceModel> Get(NoVerifyGetCountryPersonList request)
        {
            return CountryPerson.GetCountryPersonList(request);
        }
        public BsTableDataSource<CountryPersonServiceModel> Get(NoVerifyGetCountryPersonList1 request)
        {
            return CountryPerson.GetCountryPersonList1(request);
        }
        public ExportExeclCountryPerson Get(NoVerifyGetExportExecl requset)
        {
            return CountryPerson.GetExportExecl(requset);
        }

        public bool Post(NoVerifySaveCountryPerson request)
        {
            return CountryPerson.SaveCountryPerson(request);
        }

        public bool Post(NoVerifyDeleteCountryPerson request)
        {
            return CountryPerson.DelectCountryPerson(request);
        }

        public ReturnCountyCheck Get(NoVerifyAddCountryCheck requset)
        {
            return CountryPerson.AddCheck(requset);
        }

        public ReturnCountyCheck Get(NoVerifyCountryStatus requset)
        {
            return CountryPerson.CountyCheck(requset);
        }

    }
}
