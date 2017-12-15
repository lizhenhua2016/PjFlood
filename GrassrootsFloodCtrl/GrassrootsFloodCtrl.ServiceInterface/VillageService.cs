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

namespace GrassrootsFloodCtrl.ServiceInterface
{
    [Authenticate]
    public class VillageService : ServiceBase
    {
        public IVillageWorkingGroupManage VillageWorkingGroupManage { get; set; }
        public IVillageTransferPersonManager VillageTransferPersonManager { get; set; }
        public IVillagePicManager VillagePicManager { get; set; }
        public IVillageGridManage VillageGridMange { get; set; }
        public List<VillageNumViewModel> Get(GetVillageReportNum request)
        {
            return VillageTransferPersonManager.GetVillageReportNum(request);
        }
        public List<VillageNumViewModel> Get(GetVillageReportNum1 request)
        {
            return VillageTransferPersonManager.GetVillageReportNum1(request);
        }

        #region 行政村防汛防台工作组

        public BsTableDataSource<VillageViewModel> GET(GetList request)
        {
            return VillageWorkingGroupManage.GetList(request);
        }
        public BsTableDataSource<VillageWorkingGroupViewModel> GET(GetGroup request)
        {
            return VillageWorkingGroupManage.GetGroup(request);
        }
        public BsTableDataSource<StatiscPerson> GET(GetGroup1 request)
        {
            return VillageWorkingGroupManage.GetGroup1(request);
        }
        public BsTableDataSource<StaticsVillageGroup> GET(GetGroupOne request)
        {
            return VillageWorkingGroupManage.GetGroupOne(request);
        }
        public BaseResult POST(SaveGroup request)
        {
            return VillageWorkingGroupManage.SaveGroup(request);
        }
        public BaseResult POST(DeleteGroup request)
        {
            return VillageWorkingGroupManage.DeleteGroup(request);
        }
        public BsTableDataSource<VillageWorkingGroupViewModel> GET(GetPersonLiableList request)
        {
            return VillageWorkingGroupManage.GetPsersonLiableList(request);
        }
        public BsTableDataSource<VillageWorkingGroupViewModel> GET(GetPersonLiable request)
        {
            return VillageWorkingGroupManage.GetPsersonLiable(request);
        }
        public BaseResult POST(DelVillageGroup request)
        {
            return VillageWorkingGroupManage.DelVillageGroup(request);
        }
        public UploadFileViewModel GET(GetVWGFileModel request)
        {
            return VillageWorkingGroupManage.DownloadVWGFileModel(request);
        }
        public BaseResult POST(UploadWGFiles request)
        {
            return VillageWorkingGroupManage.UploadWGFiles(request);
        }
        public BaseResult POST(DownLoadOneVillage request)
        {
            return VillageWorkingGroupManage.DownLoadOneVillage(request);
        }
        //BaseResult DownLoadVillage(DownLoadVillage request)
        public BaseResult POST(DownLoadVillage request)
        {
            return VillageWorkingGroupManage.DownLoadVillage(request);
        }
        #endregion

        #region 村网格责任人
        public BsTableDataSource<VillageViewModel> GET(GetVillageGridList request)
        {
            return VillageGridMange.GetVillageGridList(request);
        }
        public BaseResult POST(SaveVillageGrid request)
        {
            return VillageGridMange.SaveVillageGrid(request);
        }
        public BsTableDataSource<VillageGridViewModel> GET(GetGridPersonLiable request)
        {
           return VillageGridMange.GetGridPersonLiable(request);
        }
        public BsTableDataSource<VillageGridViewModel> GET(GetGridPersonLiableList request)
        {
            return VillageGridMange.GetGridPersonLiableList(request);
        }
        public BsTableDataSource<VillageGridViewModel> GET(GetVillageGrid request)
        {
            return VillageGridMange.GetVillageGrid(request);
        }
        public BsTableDataSource<StatiscPerson> GET(GetVillageGrid1 request)
        {
            return VillageGridMange.GetVillageGrid1(request);
        }
        public BaseResult POST(DeleteVillageGrid request)
        {
            return VillageGridMange.DeleteVillageGrid(request);
        }
        public BaseResult POST(DelVillageGrid request)
        {
            return VillageGridMange.DelVillageGrid(request);
        }
        public UploadFileViewModel GET(DownGridFileModel request)
        {
            return VillageGridMange.DownGridFileModel(request);
        }
        public BaseResult POST(UploadGridFiles request)
        {
            return VillageGridMange.UploadGridFiles(request);
        }
        public BaseResult POST(DownLoadOneGrid request)
        {
            return VillageGridMange.DownLoadOneGrid(request);
        }
        public BaseResult POST(DownLoadGrid request)
        {
            return VillageGridMange.DownLoadGrid(request);
        }
        #endregion

        #region 行政村危险区转移人员

        public BsTableDataSource<VillageViewModel> Get(GetVillageTransferPersonAdcd request)
        {
            return VillageTransferPersonManager.GetVillageTransferPersonAdcd(request);
        }
        

        public BsTableDataSource<VillageTransferPersonViewModel> Get(GetVillageTransferPerson request)
        {
            return VillageTransferPersonManager.GetVillageTransferPerson(request);
        }
        public BsTableDataSource<VillageTransferPersonViewModel> Get(GetVillageTransferPerson1 request)
        {
            return VillageTransferPersonManager.GetVillageTransferPerson1(request);
        }
        public BsTableDataSource<StatiscPerson> Get(GetVillageTransferPerson2 request)
        {
            return VillageTransferPersonManager.GetVillageTransferPerson2(request);
        }
        public bool POST(DelVillageTransferPerson request)
        {
            return VillageTransferPersonManager.DelVillageTransferPerson(request.ids);
        }

        public BaseResult POST(SaveVillageTransferPerson request)
        {
            return VillageTransferPersonManager.SaveVillageTransferPerson(request);
        }

        public bool POST(DelVillageTransferPersonByADCD request)
        {
            return VillageTransferPersonManager.DelVillageTransferPersonByADCD(request);
        }
        public bool POST(NoVillageTransferPerson request)
        {
            return VillageTransferPersonManager.NoVillageTransferPerson(request);
        }

        #endregion

        #region 行政村防汛形势图
        public BsTableDataSource<VillageViewModel> Get(GetVillagePicAdcd request)
        {
            return VillagePicManager.GetVillagePicAdcd(request);
        }


        public bool Post(SaveVillagePic request)
        {
            return VillagePicManager.SaveVillagePic(request);
        }

        public bool Post(DelVillagePic request)
        {
            return VillagePicManager.DelVillagePic(request);
        }

        public VillagePic Get(GetVillagePicByAdcdAndYear request)
        {
            return VillagePicManager.GetVillagePicByAdcdAndYear(request);
        }

        public BsTableDataSource<VillagePicViewModel>  Get(GetVillagePicList request)
        {
            return VillagePicManager.GetVillagePicList(request);
        }
        
        #endregion
    }
}
