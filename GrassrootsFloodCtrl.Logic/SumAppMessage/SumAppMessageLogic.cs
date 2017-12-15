using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.Model.SumAppMessage;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.Logic.Common;
using GrassrootsFloodCtrl.Model.AppApi;
using GrassrootsFloodCtrl.Model.ZZTX;

namespace GrassrootsFloodCtrl.Logic.SumAppMessage
{
    public class SumAppMessageLogic : ManagerBase, ISumAppMessageLogic
    {
        /// <summary>
        /// 获取大事件的列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<AppSumEventModel> GetSumEventUI(RouteSearchEventModel request)
        {
            using (var db = DbFactory.Open())
            {
                string role = AdcdHelper.GetByAdcdRole(adcd);
                var whereLamda = db.From<AppWarnEvent>();
                whereLamda.LeftJoin<AppWarnEvent, ADCDInfo>((x,y)=>x.adcd==y.adcd);
                if (role == "省级")
                {
                    if (request.CityAdcd == null)
                        whereLamda.And(x => x.adcd.StartsWith("33"));
                    else
                        whereLamda.And(x => x.adcd.StartsWith(request.CityAdcd.Substring(0, 4)));

                }
                if (role == "市级")
                {
                    whereLamda.And(x => x.adcd.StartsWith(adcd.Substring(0, 4)));
                }
                if (role == "县级")
                {
                    whereLamda.And(x => x.adcd.StartsWith(adcd.Substring(0, 6)));
                }
                if (role == "镇级")
                {
                    whereLamda.And(x => x.adcd==adcd);
                }
                if (request.CountryAdcd != null)
                {
                    whereLamda.And(x => x.adcd == request.CountryAdcd);
                }
                whereLamda.And(x => x.StartTime>=Convert.ToDateTime(DateTime.Now.ToString("yyyy-01-01")));
                if (!string.IsNullOrEmpty(request.EventName))
                {
                    whereLamda.And(x => x.EventName.Contains(request.EventName));
                }
                
                whereLamda.OrderByDescending(x => x.StartTime);
                var total = db.Count(whereLamda);
                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                whereLamda.Limit(PageIndex, PageSize);
                var list = db.Select<AppSumEventModel>(whereLamda);
                return new BsTableDataSource<AppSumEventModel> {total=total,rows=list };
            }
        }

        public BsTableDataSource<SumReadModel> GetSumReadUI(RouteSearcReadModel request)
        {
            using (var db = DbFactory.Open())
            {
                List<SumReadModel> list = new List<SumReadModel>();
                if (request.IsRead)
                {
                    list = db.SqlList<SumReadModel>("select a.ReceiveUserName,a.ReceiveUserPhone,a.Position,a.ReceiveDateTime,a.Id as MessageId,c.grade  from AppSendMessage a inner join AppMobileLogin b on b.userName=a.ReceiveUserPhone left  join ADCDInfo c on c.adcd = a.ReciveAdcd   where AppWarnInfoID = '" + request.WarnInfoId + "' and ReceiveUserName != SendMessageByUserName and a.IsReaded = 1 and a.Position!='驻村干部'");
                }
                else
                {
                    list = db.SqlList<SumReadModel>("select a.ReceiveUserName,a.ReceiveUserPhone,a.Position,a.ReceiveDateTime,a.Id as MessageId,c.grade  from AppSendMessage a inner join AppMobileLogin b on b.userName=a.ReceiveUserPhone left  join ADCDInfo c on c.adcd = a.ReciveAdcd   where AppWarnInfoID = '" + request.WarnInfoId + "' and ReceiveUserName != SendMessageByUserName and a.IsReaded = 0 and a.Position!='驻村干部'");
                }
                var pageList = list.Skip(request.PageSize * (request.PageIndex - 1))
                   .Take(request.PageSize).OrderByDescending(x => x.ReceiveDateTime).ToList();
                return new BsTableDataSource<SumReadModel> { total = list.Count(), rows = pageList };

            }
        }

        public BsTableDataSource<SumAppWarnInfoModel> GetSumWarnInfoUI(RouteSearcWarnInfoModel request)
        {
            using (var db = DbFactory.Open())
            {
                var list = db.SqlList<SumAppWarnInfoModel>("select a.SendMessage as eventName,b.Warninglevel,a.SendMessageByUserName+'('+c.adnm+')' as SendMessageByUserName,a.ReceiveDateTime, a.AppWarnInfoID  from AppSendMessage a left  join AppWarnLevel b on a.Warninglevel = b.Id left  join ADCDInfo c on c.adcd = a.ReciveAdcd  inner join AppMobileLogin d on d.userName = a.ReceiveUserPhone  where a.AppWarnEventId = '" + request.EventId+ "' and  a.ReceiveUserName=a.SendMessageByUserName and Remark='' and VillageMessage is null ");
                var townList= db.SqlList<SumAppWarnInfoModel>("select a.Remark as eventName,b.Warninglevel,a.SendMessageByUserName+'('+c.adnm+')' as SendMessageByUserName,a.ReceiveDateTime, a.AppWarnInfoID  from AppSendMessage a left  join AppWarnLevel b on a.Warninglevel = b.Id left  join ADCDInfo c on c.adcd = a.ReciveAdcd  inner join AppMobileLogin d on d.userName = a.ReceiveUserPhone  where a.AppWarnEventId = '" + request.EventId + "' and  a.ReceiveUserName=a.SendMessageByUserName and Remark!='' and VillageMessage is null ");
                var  vilageList = db.SqlList<SumAppWarnInfoModel>("select a.VillageMessage as eventName,b.Warninglevel,a.SendMessageByUserName+'('+c.adnm+')' as SendMessageByUserName,a.ReceiveDateTime, a.AppWarnInfoID  from AppSendMessage a left  join AppWarnLevel b on a.Warninglevel = b.Id left  join ADCDInfo c on c.adcd = a.ReciveAdcd  inner join AppMobileLogin d on d.userName = a.ReceiveUserPhone  where a.AppWarnEventId = '" + request.EventId + "' and  a.ReceiveUserName=a.SendMessageByUserName and Remark!='' and VillageMessage is not null ");
                list.AddRange(townList);
                list.AddRange(vilageList);
                foreach (var item in list)
                {
                    var appSendMessageList = db.SqlList<AppSendMessage>("select a.* from AppSendMessage a inner join AppMobileLogin b on b.userName=a.ReceiveUserPhone where AppWarnInfoID = '"+item.AppWarnInfoID+ "' and ReceiveUserName != SendMessageByUserName and a.Position!='驻村干部' ");
                    item.ReadCount = appSendMessageList.Count(x => x.IsReaded==true);
                    item.NoReadCount = appSendMessageList.Count(x=>x.IsReaded==false);
                }

                var pageList=list.Skip(request.PageSize * (request.PageIndex - 1))
                    .Take(request.PageSize).OrderByDescending(x=>x.ReceiveDateTime).ToList();
                return new BsTableDataSource<SumAppWarnInfoModel> { total = list.Count(),rows=pageList };
            }

        }
    }
}
