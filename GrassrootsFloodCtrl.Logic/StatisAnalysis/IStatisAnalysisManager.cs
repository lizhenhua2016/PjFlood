using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.CApp;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.ServiceModel.Village;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.StatisAnalysis
{
    public interface IStatisAnalysisManager
    {
        BsTableDataSource<StatisAnalysisViewModel> GetStatisAnalysisList(GetStatisAnalysisList request);

        List<CAppStaticsViewModel> Statics(Statistics request);
        List<CAppStaticsByPostViewModel> StatisticsByPost(StatisticsByPost request);

        List<StaticsVillageTransferModel> StaticsVillageTransfer(StaticsVillageTransfer request);

        BsTableDataSource<StatisTypeInfo> GetStatisTypeInfoList(GetStatisTypeInfoList request);
        BsTableDataSource<StatisTypeInfo> GetStatisCountyPerson(GetStatisCountyPerson request);
        BsTableDataSource<VillageTransferPersonViewModel> GetStatisTypeInfoOneWXQ(GetStatisTypeInfoOneWXQ request);
        BsTableDataSource<VillageGridViewModel> GetStatisTypeInfoOneWG(GetStatisTypeInfoOneWG request);
        BsTableDataSource<VillageGridViewModel> GetStatisTypeInfoCountyWG(GetStatisTypeInfoCountyWG request);

        BsTableDataSource<TownPersonLiableViewModel> GetStatisTownPerson(GetStatisTownPerson request);
        BsTableDataSource<StatisVillagePersonViewModel> GetStatisVillagePerson(GetStatisVillagePerson request);

        BsTableDataSource<StatisTransferPersonViewModel> GetStatisTransferPerson(GetStatisTransferPerson request);
     
        BsTableDataSource<StatisticsByPostInfoViewModel> StatisticsByPostOne(StatisticsByPostOne request);

        BsTableDataSource<StatisTransferPersonViewModel> GetStatisDangerTypeAll(GetStatisDangerTypeAll request);
        BsTableDataSource<StatisGridTypeAllViewModel> GetStatisGridTypeAll(GetStatisGridTypeAll request);
        BsTableDataSource<StatisTransferPersonViewModel> GetStatisCountyDangerTypeAll(GetStatisCountyDangerTypeAll request);
        BsTableDataSource<StatisGridTypeAllViewModel> GetStatisCountyGridTypeAll(GetStatisCountyGridTypeAll request);

        BsTableDataSource<VillageTransferPersonViewModel> GetStatisTypeInfoCountyWXQ(GetStatisTypeInfoCountyWXQ request);

        BsTableDataSource<StatisticsByPostInfoViewModel> StatisticsCoutyByPostOne(StatisticsCoutyByPostOne request);

        BsTableDataSource<VillageTransferPersonViewModel> GetStatisCountyAllTransferPerson(GetStatisCountyAllTransferPerson request);
        BsTableDataSource<StatisAppPersonInPostViewModel> GetStatisAppPersonInPost(GetStatisAppPersonInPost request);

        BsTableDataSource<StatiscPersonInPost> StatisVillagePersonInPostByCountyAdcd(StatisVillagePersonInPostByCountyAdcd request);
        /// <summary>
        /// 防汛任务统计
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<StatisPrevFloodViewModel> GetStatisPrevFlood (GetStatisPrevFlood request);
        /// <summary>
        /// 县级乡镇防汛详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BsTableDataSource<CountyFloodDetailViewModel> GetCountyFloodDetail(GetCountyFloodDetail request);
    }
}
