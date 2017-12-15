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

namespace GrassrootsFloodCtrl.ServiceInterface
{
    /// <summary>
    /// 组织责任（体系）
    /// </summary>
    [Authenticate]
    public class ZZTXService: ServiceBase
    {
        public IZZTXManager ZZTXRManager { get; set; }
        public ADCDInfo GET(GetADCDInfoByADCD request)
        {
            return ZZTXRManager.GetADCDInfoByADCD(request.adcd);
        }

        public ADCDInfo Get(GetADCDInfoById request)
        {
           return ZZTXRManager.GetADCDInfoById(request.id);
        }

        public BsTableDataSource<ADCDInfo> GET(GetADCDInfo request)
       {
           return ZZTXRManager.GetADCDInfo(request);
       }
       public List<ADCDTree> POST(GetADCDInfoTree request)
        {
            return ZZTXRManager.GetADCDInfoTree(request);
        }
        public ADCDInfo GET(GetAdcdInfoByADNM request)
        {
            return ZZTXRManager.GetAdcdInfoByADNM(request);
        }

        public BsTableDataSource<ADCDDisasterViewModel> GET(GetADCDDisasterInfo request)
        {
            return ZZTXRManager.GetADCDDisasterInfo(request);
        }

        public bool POST(SaveADCDDisasterInfo request)
        {
            return ZZTXRManager.SaveADCDDisasterInfo(request);
        }

        public bool POST(DelADCDDisasterInfo request)
        {
            return ZZTXRManager.DelADCDDisasterInfo(request.ids);
        }
        public bool POST(SaveADCDInfo request)
        {
            return ZZTXRManager.SaveADCDInfo(request);
        }
        public bool POST(DelADCDInfo request)
        {
            return ZZTXRManager.DelADCDInfo(request);
        }

        public bool POST(ImportCurrentInfo request)
        {
            return ZZTXRManager.ImportCurrentInfo(request);
        }

        public bool POST(SavePoint request)
        {
            return ZZTXRManager.SavePoint(request);
        }
        public BsTableDataSource<ADCDQRCodeViewModel> POST(QRCodeList request)
        {
            return ZZTXRManager.QRCodeList(request);
        }

        public List<ADCDInfo> GET(GetADCDInfoBySession request)
        {
            return ZZTXRManager.GetADCDInfoBySession(request);
        }

        public BsTableDataSource<ADCDInfo> GET(GetAdcdInfoForCounty request)
        {
            return ZZTXRManager.GetADCDInfoForCounty(request);
        }

        public List<ResponseAdcdByUserAdcd> post(GetAdcdByUseradcd request)
        {
            return ZZTXRManager.GetAdcdInfoByAdcdForTree(request);
        }

        public List<ResponseAdcdByUserAdcd> post(AppGetAdcdInfoByAdcd request)
        {
            return ZZTXRManager.AppGetAdcdInfoByAdcd(request);
        }

        //获取全部的市
        public List<ResponseAdcdByUserAdcd> post(AppGetAllCity request)
        {
            return ZZTXRManager.AppGetAllCity(request);
        }
        //根据adcd获取下一级全部的县
        public List<ResponseAdcdByUserAdcd> post(AppGetAllCounty request)
        {
            return ZZTXRManager.AppGetAllCounty(request);
        }

        //根据adcd获取下一级全部的镇
        public List<ResponseAdcdByUserAdcd> post(AppGetAllTown request)
        {
            return ZZTXRManager.AppGetAllTown(request);
        }
        //根据adcd获取下一级全部的村
        public List<ResponseAdcdByUserAdcd> post(AppGetAllVillage request)
        {
            return ZZTXRManager.AppGetAllVillage(request);
        }
        //通过adcd获取对应的信息
        public ResponseAdcdInfo post(GetAdcdInfoByAdcd request) {
            return ZZTXRManager.GetAdcdInfoByAdcd(request);
        }

        //通过Adcd获取下一级的adcd的信息
        public List<ResponseAdcdInfo> post(GetNextLevleAdcdInfoByAdcd request)
        {
            return ZZTXRManager.GetNextLevleAdcdInfoByAdcd(request);
        }
    }
}
