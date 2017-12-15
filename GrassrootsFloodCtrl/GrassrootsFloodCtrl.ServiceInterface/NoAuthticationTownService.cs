
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.ServiceModel.NoTownRoute;
using GrassrootsFloodCtrl.Logic.NoAuthticationTown;

namespace GrassrootsFloodCtrl.ServiceInterface.NoAuthtication
{
    public class NoAuthticationTownService : ServiceBase
    {
        public NoAuthticationITownManager TownManager { get; set; }

        public BsTableDataSource<TownPersonLiableViewModel> Get(NoAuthticationGetTownList request)
        {
            return TownManager.NoAuthticationGetTownList(request);
        }
        public BsTableDataSource<TownPersonLiableViewModel> Get(NoAuthticationGetTownList1 request)
        {
            return TownManager.NoAuthticationGetTownList1(request);
        }
        public bool Post(NoAuthticationSaveTown request)
        {
            return TownManager.NoAuthticationSaveTown(request);
        }

        public bool Post(NoAuthticationDelTown request)
        {
            return TownManager.NoAuthticationDelTown(request.ids);
        }
    }
}
