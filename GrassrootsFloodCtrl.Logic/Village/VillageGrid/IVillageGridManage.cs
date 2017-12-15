using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Village;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.Village.VillageGrid
{
    public interface IVillageGridManage
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageViewModel> GetVillageGridList(GetVillageGridList request);
        BsTableDataSource<StatiscPerson> GetVillageGrid1(GetVillageGrid1 request);
        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageGridViewModel> GetVillageGrid(GetVillageGrid request);
        /// <summary>
        /// 新增,编辑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResult SaveVillageGrid(SaveVillageGrid request);
      
        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResult DeleteVillageGrid(DeleteVillageGrid request);

        BaseResult DelVillageGrid(DelVillageGrid request);
        /// <summary>
        /// 获取所有责任人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageGridViewModel> GetGridPersonLiableList(GetGridPersonLiableList request);
        /// <summary>
        /// 获取一个责任人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageGridViewModel> GetGridPersonLiable(GetGridPersonLiable request);
        
        #region 上传下载
        /// <summary>
        /// 网格模板下载
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        UploadFileViewModel DownGridFileModel(DownGridFileModel request);
        /// <summary>
        /// 网格上传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResult UploadGridFiles(UploadGridFiles request);
        /// <summary>
        /// 下载一个村的网格人员信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResult DownLoadOneGrid(DownLoadOneGrid request);
        BaseResult DownLoadGrid(DownLoadGrid request);

        #endregion
    }
}
