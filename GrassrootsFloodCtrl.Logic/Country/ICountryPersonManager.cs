using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.CountryPerson;
using GrassrootsFloodCtrl.ServiceModel.Country;
using GrassrootsFloodCtrl.ServiceModel.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.Country
{
    public interface ICountryPersonManager
    {
        // string AddChangePersons(AddChangePersons requset);
        //批量导入城防体系人员
       ReturnInsertStatus  InsertExeclCountryPerson(CountryImportExcel requset);
        //城防体人员列表
       BsTableDataSource<CountryPersonServiceModel> GetCountryPersonList(GetCountryPersonList request);
       BsTableDataSource<CountryPersonServiceModel> GetCountryPersonList1(GetCountryPersonList1 request);
        //下载当年的县级城防体系人员列表
       ExportExeclCountryPerson GetExportExecl(GetExportExecl requset);

        bool SaveCountryPerson(SaveCountryPerson requset);

        bool DelectCountryPerson(DeleteCountryPerson request);

        ReturnCountyCheck AddCheck(AddCountryCheck requset);

        ReturnCountyCheck CountyCheck(CountryStatus requset);

    }
}
