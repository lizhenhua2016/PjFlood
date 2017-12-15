using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using ServiceStackForLeafletjsResponse;

namespace GrassrootsFloodCtrl.Logic.ZZTX
{
    /// <summary>
    /// 组织责任
    /// </summary>
    public interface IZZTXManager
    {
        /// <summary>
        /// 根据id获取单个行区划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ADCDInfo GetADCDInfoById(int id);
        /// <summary>
        /// 根据行区划编码获取单个行区划信息
        /// </summary>
        /// <param name="adcd"></param>
        /// <returns></returns>
        ADCDInfo GetADCDInfoByADCD(string adcd);

        /// <summary>
        /// 保存行区划信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool SaveADCDInfo(SaveADCDInfo request);
        /// <summary>
        /// 获取行区划信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<ADCDInfo> GetADCDInfo(GetADCDInfo request);
        List<ADCDTree> GetADCDInfoTree(GetADCDInfoTree request);
        /// <summary>
        /// 根据乡镇、行政村名称获取受灾害影响的行政区划信息
        /// </summary>
        /// <param name="adnm"></param>
        /// <returns></returns>
        ADCDInfo GetAdcdInfoByADNM(GetAdcdInfoByADNM request);

        /// <summary>
        /// 获取受灾害影响的行政区划信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<ADCDDisasterViewModel> GetADCDDisasterInfo(GetADCDDisasterInfo request);
        /// <summary>
        /// 删除受灾害影响的行政区划信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DelADCDDisasterInfo(string ids);
        bool DelADCDInfo(DelADCDInfo request);
        /// <summary>
        /// 保存行政区划受灾害信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool SaveADCDDisasterInfo(SaveADCDDisasterInfo request);
        
        /// <summary>
        /// 导入当前年度信息(把上一年度的信息复制到今年)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool ImportCurrentInfo(ImportCurrentInfo request);

        /// <summary>
        /// 保存行政区划坐标点信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool SavePoint(SavePoint request);

        BsTableDataSource<ADCDQRCodeViewModel> QRCodeList(QRCodeList request);

        List<ADCDInfo> GetADCDInfoBySession(GetADCDInfoBySession request);


        BsTableDataSource<ADCDInfo> GetADCDInfoForCounty(GetAdcdInfoForCounty request);

        List<ResponseAdcdByUserAdcd> GetAdcdInfoByAdcdForTree(GetAdcdByUseradcd request);

        List<ResponseAdcdByUserAdcd> AppGetAdcdInfoByAdcd(AppGetAdcdInfoByAdcd request);


        //获取全部的市
        List<ResponseAdcdByUserAdcd> AppGetAllCity(AppGetAllCity reqest);
        //通过adcd获取全部的县
        List<ResponseAdcdByUserAdcd> AppGetAllCounty(AppGetAllCounty reqest);
        //通过adcd获取下一级的全部的镇
        List<ResponseAdcdByUserAdcd> AppGetAllTown(AppGetAllTown reqest);
        //通过adcd获取下一级全部的村
        List<ResponseAdcdByUserAdcd> AppGetAllVillage(AppGetAllVillage reqest);

        //通过adcd获取相对应的信息
        ResponseAdcdInfo GetAdcdInfoByAdcd(GetAdcdInfoByAdcd requst);

        List<ResponseAdcdInfo> GetNextLevleAdcdInfoByAdcd(GetNextLevleAdcdInfoByAdcd requst);

    }
}
