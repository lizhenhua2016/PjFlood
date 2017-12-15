using System.Collections.Generic;
using GrassrootsFloodCtrl.Logic.Village;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack;
using GrassrootsFloodCtrl.ServiceModel.Common;
using GrassrootsFloodCtrl.Logic.Village.WorkingGroup;
using GrassrootsFloodCtrl.Logic.Village.VillageGrid;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.Logic.NoVerify;
using GrassrootsFloodCtrl.ServiceModel.NoAuditRoute;
using GrassrootsFloodCtrl.ServiceModel.RouteNoVerify;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    public class NoVerifyVillageService:ServiceBase
    {
        public INoVerifyVillageWorkingGroupManange NoVerifyVillageWorkingGroupManage { get; set; }

        public INoVerifyVillageGridManage NoVerifyVillageGridManage { get; set; }

        public INoVerifyVillageTransferPersonManager NoVerifyVillageTransferPersonManager { get; set; }

        public INoVerifyVillagePicManager NoVerifyVillagePicManager { get; set; }


        public List<VillageNumViewModel> Get(NoVerifyGetVillageReportNum request)
        {
            return NoVerifyVillageTransferPersonManager.GetVillageReportNum(request);
        }
        #region 行政村防汛防台工作组
        public BsTableDataSource<VillageWorkingGroupViewModel> GET(NoVerifyGetGroup request)
        {
            return NoVerifyVillageWorkingGroupManage.NoVerifyGetGroup(request);
        }
        public BsTableDataSource<VillageWorkingGroupViewModel> GET(NoVerifyGetPersonLiableList request)
        {
            return NoVerifyVillageWorkingGroupManage.GetPsersonLiableList(request);
        }
        public BsTableDataSource<VillageViewModel> GET(NoVerifyGetList request)
        {
            return NoVerifyVillageWorkingGroupManage.GetList(request);
        }

        public UploadFileViewModel GET(NoVerifyGetVWGFileModel request)
        {
            return NoVerifyVillageWorkingGroupManage.DownloadVWGFileModel(request);
        }
        public BaseResult POST(NoVerifyUploadWGFiles request)
        {
            return NoVerifyVillageWorkingGroupManage.UploadWGFiles(request);
        }
        public BaseResult POST(NoVerifyDownLoadOneVillage request)
        {
            return NoVerifyVillageWorkingGroupManage.DownLoadOneVillage(request);
        }
        //BaseResult DownLoadVillage(DownLoadVillage request)
        public BaseResult POST(NoVerifyDownLoadVillage request)
        {
            return NoVerifyVillageWorkingGroupManage.DownLoadVillage(request);
        }

        #endregion

        #region 村网络责任人
        public BsTableDataSource<VillageGridViewModel> GET(NoVerifyGetVillageGrid request)
        {
            return NoVerifyVillageGridManage.NoVerifyGetVillageGrid(request);
        }
        #endregion

        #region 行政村危险区人员转移清单
        public BsTableDataSource<VillageTransferPersonViewModel> Get(NoVerifyGetVillageTransferPerson request)
        {
            return NoVerifyVillageTransferPersonManager.NoVerifyGetVillageTransferPerson(request);
        }
        public BsTableDataSource<VillageViewModel> Get(NoVerifyGetVillageTransferPersonAdcd request)
        {
            return NoVerifyVillageTransferPersonManager.GetVillageTransferPersonAdcd(request);
        }
        #endregion

        #region 行政村防汛防台形势图

        public VillagePic Get(NoVerifyGetVillagePicByAdcdAndYear request)
        {
            return NoVerifyVillagePicManager.NoVerifyGetVillagePicByAdcdAndYear(request);
        }

        public BsTableDataSource<VillagePicViewModel> Get(NoVerifyGetVillagePicList request)
        {
            return NoVerifyVillagePicManager.GetVillagePicList(request);
        }

        public BsTableDataSource<VillageViewModel> Get(NoVerifyGetVillagePicAdcd request)
        {
            return NoVerifyVillagePicManager.GetVillagePicAdcd(request);
        }

        #endregion



    }
}
