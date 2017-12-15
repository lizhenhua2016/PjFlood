using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Logic.ZZTX;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using ServiceStack;
using ServiceStackForLeafletjsResponse;
using GrassrootsFloodCtrl.Logic.NoVerify;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    public class NoVerifyZZTXService:ServiceBase
    {
        public INoVerifyZZTXManager NoVerifyZZTXManager { get; set; }

        public BsTableDataSource<ADCDQRCodeViewModel> POST(NoVerifyQRCodeList request)
        {
            return NoVerifyZZTXManager.QRCodeList(request);
        }

        public List<ResponseAdcdByUserAdcd> post(NoVerifyAppGetAdcdInfoByAdcd request)
        {
            return NoVerifyZZTXManager.AppGetAdcdInfoByAdcd(request);
        }

        //通过Adcd获取下一级的adcd的信息
        public List<ResponseAdcdInfo> post(NoVGetNextLevleAdcdInfoByAdcd request)
        {
            return NoVerifyZZTXManager.GetNextLevleAdcdInfoByAdcd(request);
        }
        public BsTableDataSource<ADCDInfo> GET(NoVGetADCDInfo request)
        {
            return NoVerifyZZTXManager.GetADCDInfo(request);
        }
        public BsTableDataSource<ADCDInfo> GET(NoVGetAdcdInfoForCounty request)
        {
            return NoVerifyZZTXManager.GetADCDInfoForCounty(request);
        }
    }
}
