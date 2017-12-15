using System.Collections.Generic;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.Town;


namespace GrassrootsFloodCtrl.ServiceModel.Country
{
    public class CountyInfoAppViewModel
    {
        public string CityName { get; set; }//市名称
        public string CountyName { get; set; }//县名称
        public double LGT { get; set; }//经度
        public double LTT { get; set; }//纬度
        public int ZhenBenji { get; set; }//镇领导
        public int CountyBenJi { get; set; }//县责任人
        public int ZDPoint { get; set; }//危险点
        public int ZDManNums { get; set; }//影响人数
        public int TownCount { get; set; }//镇数量
        public List<GridModel> rows { get; set; }//网格
        
        public List<ADCDInfo> Towns { get; set; }//镇adcd列表
    }
}