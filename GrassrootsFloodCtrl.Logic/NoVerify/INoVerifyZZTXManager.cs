using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.NoAuditRoute;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using ServiceStackForLeafletjsResponse;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;
using GrassrootsFloodCtrl.ServiceModel.Village;

namespace GrassrootsFloodCtrl.Logic.NoVerify
{
    public interface INoVerifyZZTXManager
    {

        BsTableDataSource<ADCDQRCodeViewModel> QRCodeList(NoVerifyQRCodeList request);

        List<ResponseAdcdByUserAdcd> AppGetAdcdInfoByAdcd(NoVerifyAppGetAdcdInfoByAdcd request);

        List<ResponseAdcdInfo> GetNextLevleAdcdInfoByAdcd(NoVGetNextLevleAdcdInfoByAdcd requst);

        /// <summary>
        /// 获取行区划信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<ADCDInfo> GetADCDInfo(NoVGetADCDInfo request);

        BsTableDataSource<ADCDInfo> GetADCDInfoForCounty(NoVGetAdcdInfoForCounty request);
        
        BsTableDataSource<ResponseVectors> GetAllVectors(NoAuthticationRouteVector request);

    }
}
