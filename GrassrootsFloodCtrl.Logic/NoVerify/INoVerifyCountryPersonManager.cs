using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.CountryPerson;
using GrassrootsFloodCtrl.ServiceModel.Country;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.NoVerify
{
    public interface INoVerifyCountryPersonManager
    {
        // string AddChangePersons(AddChangePersons requset);
        //批量导入城防体系人员
        ReturnInsertStatus InsertExeclCountryPerson(NoVerifyCountryImportExcel requset);
        //城防体人员列表
        BsTableDataSource<CountryPersonServiceModel> GetCountryPersonList(NoVerifyGetCountryPersonList request);
        BsTableDataSource<CountryPersonServiceModel> GetCountryPersonList1(NoVerifyGetCountryPersonList1 request);
        //下载当年的县级城防体系人员列表
        ExportExeclCountryPerson GetExportExecl(NoVerifyGetExportExecl requset);

        bool SaveCountryPerson(NoVerifySaveCountryPerson requset);

        bool DelectCountryPerson(NoVerifyDeleteCountryPerson request);

        ReturnCountyCheck AddCheck(NoVerifyAddCountryCheck requset);

        ReturnCountyCheck CountyCheck(NoVerifyCountryStatus requset);
    }
}
