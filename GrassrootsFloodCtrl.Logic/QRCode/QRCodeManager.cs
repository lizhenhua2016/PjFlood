using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.ServiceModel.Village;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.Model.DangerZone;

namespace GrassrootsFloodCtrl.Logic.QRCode
{
    public class QRCodeManager : ManagerBase,IQRCodeManager
    {
        public BsTableDataSource<VillageGridViewModel> GetVillageGrid(QRVillageGrid request)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<VillageGridPersonLiable>();
                builder.LeftJoin<VillageGridPersonLiable, ADCDInfo>((x, y) => x.VillageADCD == y.adcd);
                var _year = null != request.year ? request.year : System.DateTime.Now.Year;
                builder.Where<VillageGridPersonLiable>(x => x.Year == _year && x.VillageADCD == request.adcd);
                builder.Select(" VillageGridPersonLiable.*,ADCDInfo.adnm");
                var count = db.Count(builder);

                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(o => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                    builder.OrderByDescending(o => request.Sort);
                else
                    builder.OrderBy(o => o.ID);

                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                builder.Limit(PageIndex, PageSize);
                var RList = db.Select<VillageGridViewModel>(builder);

                return new BsTableDataSource<VillageGridViewModel>() { rows = RList, total = count };
            }
        }
        public BsTableDataSource<StaticsVillageGroup> QRGroupOne(QRGroupOne request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(request.adcd)) throw new Exception("参数异常！");
                var builder = db.From<VillageWorkingGroup>();
                builder.LeftJoin<VillageWorkingGroup, Model.Post.Post>((x, y) => x.Post == y.PostName);
                var _year = null != request.year ? request.year : System.DateTime.Now.Year;
                builder.Where<VillageWorkingGroup>(x => x.Year == _year && x.VillageADCD == request.adcd);
                builder.Where<Model.Post.Post>(w => w.PostType == "行政村防汛防台工作组");
                builder.Select("VillageWorkingGroup.*,Post.ID as Pid");
                var count = db.Count(builder);
                builder.OrderBy<Model.Post.Post>(o => o.orderId);

                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                builder.Limit(PageIndex, PageSize);
                var RList = db.Select<VillageWorkingGroupViewModel>(builder);
                var RList1 = RList.Select(w => w.Post).Distinct().ToList();
                try
                {
                    List<StaticsVillageGroup> lsvg = new List<StaticsVillageGroup>();
                    RList1.ForEach(w =>
                    {
                        StaticsVillageGroup svg = new StaticsVillageGroup();
                        svg.post = w;
                        //var fpid = RList.Where(x => x.Post == w);
                        var R = RList.Where(x => x.Post == w).Distinct().ToList();
                        svg.postid = R.FirstOrDefault().PId;
                        List<PersonLiabel> lpl = new List<PersonLiabel>();
                        R.ForEach(y =>
                        {
                            PersonLiabel p = new PersonLiabel();
                            p.adcd = y.VillageADCD;
                            p.name = y.PersonLiable;
                            p.position = y.Position;
                            p.mobile = y.HandPhone;
                            lpl.Add(p);
                        });
                        svg.datas = lpl;
                        lsvg.Add(svg);
                    });
                    return new BsTableDataSource<StaticsVillageGroup>() { rows = lsvg, total = count };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public BsTableDataSource<VillageTransferPersonViewModel> QRVillageTransferPerson(QRVillageTransferPerson request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(request.adcd)) throw new Exception("参数异常！");
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                var builder = db.From<ADCDInfo>();
                builder.LeftJoin<ADCDInfo, VillageTransferPerson>((x, y) => x.adcd == y.adcd);
                builder.LeftJoin<VillageTransferPerson, DangerZone>((x, y) => x.DangerZoneType == y.DangerZoneName);
                if (!string.IsNullOrEmpty(request.adcd)) builder.And(x => x.adcd == request.adcd);
                    builder.And<VillageTransferPerson>(y => y.Year == _year);
                builder.Select("VillageTransferPerson.*,ADCDInfo.adnm,DangerZone.Id as DId");
                var count = db.Count(builder);

                if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "asc")
                    builder.OrderBy(x => request.Sort);
                else if (!string.IsNullOrEmpty(request.Sort) && !string.IsNullOrEmpty(request.Order) && request.Order == "desc")
                    builder.OrderByDescending(x => request.Sort);
                else
                    builder.OrderBy(x => x.adcd);

                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : request.PageIndex * PageSize;
                builder.Limit(PageIndex, PageSize);
                var list = db.Select<VillageTransferPersonViewModel>(builder);

                return new BsTableDataSource<VillageTransferPersonViewModel>() { rows = list, total = count };
            }
        }

        public VillagePic GetVillagePicByAdcdAndYear(QRVillagePicByAdcdAndYear request)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<VillagePic>();
                var _year = request.year == null ? DateTime.Now.Year : request.year;
                if (string.IsNullOrEmpty(request.adcd) && request.adcd.Length == 15)
                    throw new Exception("行政区划编码为空或不规范");
                builder.Where(x => x.Year == _year && x.adcd == request.adcd);
                return db.Single(builder);
            }
        }
    }
}
