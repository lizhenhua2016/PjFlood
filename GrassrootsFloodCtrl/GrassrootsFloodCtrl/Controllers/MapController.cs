using GrassrootsFloodCtrl.ServiceInterface;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack;
using System.Web.Mvc;
using GrassrootsFloodCtrl.ServiceModel.Village;

namespace GrassrootsFloodCtrl.Controllers
{
    public class MapController : ControllerBase
    {
        [HttpGet]
        public ActionResult Index(string useradcd)
        {
            ViewBag.loginadcd = useradcd;
            using (var service = HostContext.ResolveService<CAppService>())
            {
                var abc = service.Post(new GetCunDot() { adcd = useradcd });
                ViewBag.overviewforcun = abc;
                
                var persondaogang = service.Get(new CCKHVillageApp(){adcd = useradcd});
                ViewBag.duty = persondaogang;



            }

            using (var service = HostContext.ResolveService<VillageService>())
            {
                var pic = service.Get(new GetVillagePicByAdcdAndYear(){adcd = useradcd,year = 2017});
                ViewBag.VillagePic = pic;

                var transferpeple = service.Get(new GetVillageTransferPerson1() {adcd = useradcd});
                ViewBag.transferpeple = transferpeple.rows;

                var gride = service.GET(new GetVillageGrid() { adcd = useradcd });
                ViewBag.gride = gride.rows;
                
                var villagepic = service.Get(new GetVillagePicByAdcdAndYear() { adcd = useradcd,year=2007});
                ViewBag.villagepic = villagepic;

                var villageGroup = service.GET(new GetGroupOne() { adcd = useradcd });
                ViewBag.villageGroup = villageGroup.rows;
                
            }
            
            return View();
        }

        public ActionResult ShowTown(string useradcd)
        {
            ViewBag.townadcd = useradcd;
            return View();
        }
    }
}