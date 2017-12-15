using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Village;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.ServiceModel.NoAuditRoute;

namespace GrassrootsFloodCtrl.Logic.NoVerify
{
    public interface INoVerifyVillageWorkingGroupManange
    {
        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageWorkingGroupViewModel> NoVerifyGetGroup(NoVerifyGetGroup request);

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageViewModel> GetList(NoVerifyGetList request);
        /// <summary>
        /// 获取所有责任人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageWorkingGroupViewModel> GetPsersonLiableList(NoVerifyGetPersonLiableList request);
        #region 上传下载
        /// <summary>
        /// 工作组模板下载
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        UploadFileViewModel DownloadVWGFileModel(NoVerifyGetVWGFileModel request);
        /// <summary>
        /// 工作组上传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResult UploadWGFiles(NoVerifyUploadWGFiles request);
        /// <summary>
        /// 下载一个村的工作组人员信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResult DownLoadOneVillage(NoVerifyDownLoadOneVillage request);
        BaseResult DownLoadVillage(NoVerifyDownLoadVillage request);
        #endregion
    }
}
