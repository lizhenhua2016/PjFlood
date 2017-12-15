using GrassrootsFloodCtrl.Logic.Country;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.CountryPerson;
using GrassrootsFloodCtrl.ServiceModel.Country;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    [Authenticate]
    public class CountryPersonService : ServiceBase
    {
        public ICountryPersonManager CountryPerson { get; set; }
        //public string Post(AddChangePersons requset)
        //{
        //    return CountryPerson.AddChangePersons(requset);
        //}

        public ReturnInsertStatus Post(CountryImportExcel requset)
        {
            return CountryPerson.InsertExeclCountryPerson(requset);
        }

        public BsTableDataSource<CountryPersonServiceModel> Get(GetCountryPersonList request)
        {
            return CountryPerson.GetCountryPersonList(request);
        }
        public BsTableDataSource<CountryPersonServiceModel> Get(GetCountryPersonList1 request)
        {
            return CountryPerson.GetCountryPersonList1(request);
        }
        public ExportExeclCountryPerson Get(GetExportExecl requset)
        {
            return CountryPerson.GetExportExecl(requset);
        }

        public bool Post(SaveCountryPerson request)
        {
            return CountryPerson.SaveCountryPerson(request);
        }

        public bool Post(DeleteCountryPerson request)
        {
            return CountryPerson.DelectCountryPerson(request);
        }

        public ReturnCountyCheck Get(AddCountryCheck requset)
        {
            return CountryPerson.AddCheck(requset);
        }

        public ReturnCountyCheck Get(CountryStatus requset)
        {
            return CountryPerson.CountyCheck(requset);
        }
            
    }
}
