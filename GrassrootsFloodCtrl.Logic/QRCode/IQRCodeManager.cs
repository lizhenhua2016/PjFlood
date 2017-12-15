using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Village;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.QRCode
{
    public interface IQRCodeManager
    {
        BsTableDataSource<StaticsVillageGroup> QRGroupOne(QRGroupOne request);

        BsTableDataSource<VillageGridViewModel> GetVillageGrid(QRVillageGrid request);

        BsTableDataSource<VillageTransferPersonViewModel> QRVillageTransferPerson(QRVillageTransferPerson request);

        VillagePic GetVillagePicByAdcdAndYear(QRVillagePicByAdcdAndYear request);
    }
}
