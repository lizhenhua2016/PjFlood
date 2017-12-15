namespace GrassrootsFloodCtrl.Model.Enums
{
   public class GrassrootsFloodCtrlEnums
    {

        #region 系统日志的操作类型

        public enum OperationTypeEnums:int
        {
            None,
            新增,
            更新,
            删除,
            登陆,
            退出,
            导出,
            审批
        }
        #endregion

        public enum AreaCode
        {
            省级编码=330000
        }
        public enum InitialPasswordEnums
        {
            初始密码 = 123456
        }
        public enum ZZTXEnums
       {
            None,
            行政村信息,
            行政村防汛防台工作组,
            行政村网格责任人,
            行政村危险区人员转移清单,
            行政村防汛防台形势图,
            镇级防汛防台责任人,
            县级防汛防台责任人,
            行政村标绘
        }
        //栏目操作权限
        public enum Actions
        {
           查看=1, 新增=2,修改=3,删除=4,查询=5,导入=6,导出=7,下载=8
        }
        //
        public enum AuditStatusEnums
        {
            市审批不通过 = -1,
            县审批不通过 = 0,
            待审批 = 1,
            县已审市未批 = 2,
            县已审市已批 = 3
        }

       //暂定系统角色
       public enum RoleEnums
       {
            None,
            系统管理员,
            市级用户,
            县级用户,
            乡镇用户,
            省级用户,
            其他用户
            
        }
        //防汛防台任务轻重情况
        public enum FXFTRW
        {
            None,
            较轻,
            较重
        }
        //app端岗位统计
        public enum AppPost
        {
            村级主要负责人,
            监测预警组,
            人员转移组,
            抢险救援组,
            巡查员,
            管理员,
            联络员
        }
        //app端村级责任人，是否接到上级防汛通知，选项
        public enum AppVillagePersonItem
        {
            防汛值班=1,
            巡查检查=2,
            监测预警=3,
            人员转移=4,
            其它=5
        }
    }
}
