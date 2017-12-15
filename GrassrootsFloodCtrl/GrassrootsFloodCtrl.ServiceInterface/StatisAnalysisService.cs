using GrassrootsFloodCtrl.Logic.StatisAnalysis;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.CApp;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.ServiceModel.Village;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.ServiceInterface
{
    [Authenticate]
    public class StatisAnalysisService :ServiceBase
    {
       public IStatisAnalysisManager StatisAnalysisManager { get; set; }
       public BsTableDataSource<StatisAnalysisViewModel> Get(GetStatisAnalysisList request)
        {
            return StatisAnalysisManager.GetStatisAnalysisList(request);
        }

        public List<CAppStaticsViewModel> Post(Statistics request)
        {
            return StatisAnalysisManager.Statics(request);
        }
        public List<StaticsVillageTransferModel> Post(StaticsVillageTransfer request)
        {
            return StatisAnalysisManager.StaticsVillageTransfer(request);
        }

        public List<CAppStaticsByPostViewModel> Post(StatisticsByPost request)
        {
            return StatisAnalysisManager.StatisticsByPost(request);
        }

        public BsTableDataSource<VillageTransferPersonViewModel> Get(GetStatisTypeInfoOneWXQ request)
        {
            return StatisAnalysisManager.GetStatisTypeInfoOneWXQ(request);
        }

        public BsTableDataSource<VillageGridViewModel> Get(GetStatisTypeInfoOneWG request)
        {
            return StatisAnalysisManager.GetStatisTypeInfoOneWG(request);
        }

        public BsTableDataSource<StatisTypeInfo> Get(GetStatisTypeInfoList request)
        {
            return StatisAnalysisManager.GetStatisTypeInfoList(request);
        }

        public BsTableDataSource<StatisTypeInfo> Get(GetStatisCountyPerson request)
        {
            return StatisAnalysisManager.GetStatisCountyPerson(request);
        }

       public BsTableDataSource<TownPersonLiableViewModel> Get(GetStatisTownPerson request)
        {
            return StatisAnalysisManager.GetStatisTownPerson(request);
        }

       public  BsTableDataSource<StatisVillagePersonViewModel> Get(GetStatisVillagePerson request)
        {
            return StatisAnalysisManager.GetStatisVillagePerson(request);
        }

       public BsTableDataSource<StatisTransferPersonViewModel> Get(GetStatisTransferPerson request)
        {
            return StatisAnalysisManager.GetStatisTransferPerson(request);
        }
        public BsTableDataSource<StatisticsByPostInfoViewModel> Get(StatisticsByPostOne request)
        {
            return StatisAnalysisManager.StatisticsByPostOne(request);
        }

        public BsTableDataSource<StatisTransferPersonViewModel> Get(GetStatisDangerTypeAll request)
        {
            return StatisAnalysisManager.GetStatisDangerTypeAll(request);
        }

       public BsTableDataSource<StatisGridTypeAllViewModel> Get(GetStatisGridTypeAll request)
        {
            return StatisAnalysisManager.GetStatisGridTypeAll(request);
        }

        public BsTableDataSource<VillageGridViewModel> Get(GetStatisTypeInfoCountyWG request)
        {
            return StatisAnalysisManager.GetStatisTypeInfoCountyWG(request);
        }

        public BsTableDataSource<VillageTransferPersonViewModel> GET(GetStatisTypeInfoCountyWXQ request)
        {
            return StatisAnalysisManager.GetStatisTypeInfoCountyWXQ(request);
        }

        public BsTableDataSource<StatisTransferPersonViewModel> GET(GetStatisCountyDangerTypeAll request)
        {
            return StatisAnalysisManager.GetStatisCountyDangerTypeAll(request);
        }
        public BsTableDataSource<StatisGridTypeAllViewModel> GET(GetStatisCountyGridTypeAll request)
        {
            return StatisAnalysisManager.GetStatisCountyGridTypeAll(request);
        }

        public BsTableDataSource<StatisticsByPostInfoViewModel> Get(StatisticsCoutyByPostOne request)
        {
            return StatisAnalysisManager.StatisticsCoutyByPostOne(request);
        }
        public BsTableDataSource<VillageTransferPersonViewModel> Get(GetStatisCountyAllTransferPerson request)
        {
            return StatisAnalysisManager.GetStatisCountyAllTransferPerson(request);
        }
        public BsTableDataSource<StatisAppPersonInPostViewModel> Get(GetStatisAppPersonInPost request)
        {
            return StatisAnalysisManager.GetStatisAppPersonInPost(request);
        }
        public BsTableDataSource<StatiscPersonInPost> Get(StatisVillagePersonInPostByCountyAdcd request)
        {
            return StatisAnalysisManager.StatisVillagePersonInPostByCountyAdcd(request);
        }
        /// <summary>
        /// 防汛任务统计
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<StatisPrevFloodViewModel> Get(GetStatisPrevFlood request)
        {
            return StatisAnalysisManager.GetStatisPrevFlood(request);
        }
        /// <summary>
        /// 县级乡镇防汛详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<CountyFloodDetailViewModel> Get(GetCountyFloodDetail request)
        {
            return StatisAnalysisManager.GetCountyFloodDetail(request);
        }
    }
}
