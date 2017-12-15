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

namespace GrassrootsFloodCtrl.Logic.Village.WorkingGroup
{
    public interface IVillageWorkingGroupManage
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageViewModel> GetList(GetList request);
        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageWorkingGroupViewModel> GetGroup(GetGroup request);
        BsTableDataSource<StatiscPerson> GetGroup1(GetGroup1 request);
        BsTableDataSource<StaticsVillageGroup> GetGroupOne(GetGroupOne request);
        /// <summary>
        /// 新增,编辑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResult SaveGroup(SaveGroup request);
        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResult DeleteGroup(DeleteGroup request);
        /// <summary>
        /// 删除一个村下面所有的责任人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResult DelVillageGroup(DelVillageGroup request);
        /// <summary>
        /// 获取所有责任人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageWorkingGroupViewModel> GetPsersonLiableList(GetPersonLiableList request);
        /// <summary>
        /// 获取一个责任人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<VillageWorkingGroupViewModel> GetPsersonLiable(GetPersonLiable request);
        #region 上传下载
        /// <summary>
        /// 工作组模板下载
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        UploadFileViewModel DownloadVWGFileModel(GetVWGFileModel request);
        /// <summary>
        /// 工作组上传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResult UploadWGFiles(UploadWGFiles request);
        /// <summary>
        /// 下载一个村的工作组人员信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResult DownLoadOneVillage(DownLoadOneVillage request);
        BaseResult DownLoadVillage(DownLoadVillage request);
        #endregion
    }
}
