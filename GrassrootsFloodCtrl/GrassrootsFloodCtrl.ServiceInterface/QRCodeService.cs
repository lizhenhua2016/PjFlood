using GrassrootsFloodCtrl.Logic.QRCode;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Village;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceInterface
{
   public class QRCodeService : ServiceBase
    {
        public IQRCodeManager QRCodeManager { get; set; }
        public BsTableDataSource<StaticsVillageGroup> GET(QRGroupOne request)
        {
            return QRCodeManager.QRGroupOne(request);
        }

        public BsTableDataSource<VillageGridViewModel> GET(QRVillageGrid request)
        {
            return QRCodeManager.GetVillageGrid(request);
        }

        public BsTableDataSource<VillageTransferPersonViewModel> Get(QRVillageTransferPerson request)
        {
            return QRCodeManager.QRVillageTransferPerson(request);
        }

        public VillagePic Get(QRVillagePicByAdcdAndYear request)
        {
            return QRCodeManager.GetVillagePicByAdcdAndYear(request);
        }

    }
}
