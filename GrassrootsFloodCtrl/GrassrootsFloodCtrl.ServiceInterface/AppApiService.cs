// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppApiService.cs" company="abc">
//   abc
// </copyright>
// <summary>
//   Defines the AppApiService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GrassrootsFloodCtrl.ServiceInterface
{
    using Logic.AppApi;
    using Model;
    using ServiceModel.AppApi;
    using ServiceModel.Common;
    using ServiceModel.Route;
    using System.Collections.Generic;
    

    /// <summary>
    /// The app api service.
    /// </summary>
    public class AppApiService : ServiceBase
    {
        /// <summary>
        /// Gets or sets the app api manager.
        /// </summary>
        public IAppApiManager AppApiManager { get; set; }

        // 获取验证码

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="BaseResult"/>.
        /// </returns>
        public BaseResult POST(AppGetLoginVCode request)
        {
            return this.AppApiManager.AppGetLoginVCode(request);
        }

        // 登陆

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="BaseResult"/>.
        /// </returns>
        public BaseResult POST(AppLogin request)
        {
            return this.AppApiManager.AppLogin(request);
        }

        // token获取用户信息

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="AppUserInfoViewModel"/>.
        /// </returns>
        public AppUserInfoViewModel POST(AppGetUserInfo request)
        {
            return this.AppApiManager.AppGetUserInfo(request);
        }

        // 通讯录

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<MailListViewModel> POST(AppGetMailList request)
        {
            return this.AppApiManager.AppGetMailList(request);
        }

        /// 问题提交
        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="BaseResult"/>.
        /// </returns>
        public BaseResult POST(AppPostSignAndFile request)
        {
            return this.AppApiManager.AppPostSignAndFile(request);
        }

        /// 问题补充
        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="BaseResult"/>.
        /// </returns>
        public BaseResult POST(AppPostFillIn request)
        {
            return this.AppApiManager.AppPostFillIn(request);
        }

        /// 获取提交过的问题
        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="BsTableDataSource"/>.
        /// </returns>
        public BsTableDataSource<AppRecordViewModel> POST(GetAppRecord request)
        {
            return this.AppApiManager.GetAppRecord(request);
        }

        /// 村级责任人，是否接到上级防汛通知，选项
        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<VillagePersonItemViewModel> Get(GetVillagePersonItem request)
        {
            return this.AppApiManager.GetVillagePersonItem(request);
        }

        /// <summary>
        /// 增加预警信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResult Post(RouteAddAppWarnEvent request)
        {
            return this.AppApiManager.AddAppWarnEvent(request);
        }

        /// <summary>
        /// 获取预警信息的列表
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// </returns>
        public AppWarnViewModel Get(RouteGetAppWarnList request)
        {
            return this.AppApiManager.GetWarntList(request);
        }

        /// <summary>
        /// 返回预警信息下拉框
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AppWarnViewModel Get(RouteGetAppWarnSelect request)
        {
            return this.AppApiManager.GetWarnSelectList(request);
        }

        /// <summary>
        /// 返回插入事件和消息添加情况
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResult Post(RouteAppWarnInfo request)
        {
            return this.AppApiManager.AddAppWarnInfoAndAppSendMessage(request);
        }

        /// <summary>
        /// 返回接收消息的列表
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        public AppSendMessageModel Get(RouteGetReceiveCountyAppSendMessage requset)
        {
            return this.AppApiManager.GetReceiveCountyAppSendMessage(requset);
        }

        /// <summary>
        /// 发送消息列表
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        public AppSendMessageModel Get(RouteGetSendCountyAppSendMessage requset)
        {
            return this.AppApiManager.GetSendCountyAppSendMessage(requset);
        }

        /// <summary>
        /// 更改消息状态
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        public BaseResult Get(RouteUpdateAppSendMessage requset)
        {
            return this.AppApiManager.UpdateAppSendMessage(requset);
        }

        /// <summary>
        /// 根据ID获取消息详情
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        public AppSendDetailsModel Get(RouteByIdAppSendMessageDetails requset)
        {
            return this.AppApiManager.GetByIdAppSendMessageDetail(requset);
        }

        /// <summary>
        /// 根据事件ID获取人员列表
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        public AppPersonModel Get(RouteGetByWarnInfoIdPerson requset)
        {
            return this.AppApiManager.GetByWarnInfoIdPerson(requset);
        }

        /// <summary>
        /// 关闭任务
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        public BaseResult Get(RouteGetByWarnInfoIdCloseEvent requset)
        {
            return this.AppApiManager.GetByWarnInfoIdCloseEvent(requset);
        }

        /// <summary>
        /// 获取等级列表
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        public AppWarnLevelListModel Get(RouteGetEventLevel requset)
        {
            return this.AppApiManager.GetAllWarnLevel(requset);
        }

        /// <summary>
        /// 第一次登录核对用户
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        public AppLoginModel Post(RouteCheckUser requset)
        {
            return this.AppApiManager.CheckUser(requset);
        }

        /// <summary>
        /// 创建GPS
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public AppUserGps Post(CreateGpsRecord request)
        {
            return this.AppApiManager.CreateGpsRecord(request);
        }

        /// <summary>
        /// 获取所有的人的GPS定位
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<AppUserGps> Post(GetGpsList request)
        {
            return this.AppApiManager.GetGpsList(request);
        }

        public List<AppUserGps> Post(GetGpsListByUserName request)
        {
            return this.AppApiManager.GetGpsListByUserName(request);
        }

        public AppSumSendMessage Get(RouteGetPCReceiveCountyAppSendMessage requset)
        {
            return AppApiManager.GetPCReceiveCountyAppSendMessage(requset);
        }

        public AppSumSendMessage Get(RouteGetPCSendCountyAppSendMessage requset)
        {
            return AppApiManager.GetPCSendCountyAppSendMessage(requset);
        }

        /// <summary>
        /// 模糊查询任务名字
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        public AppWarnEventMoel Get(RouteSearchWarnEventName requset)
        {
            return AppApiManager.GetSearchWarnEventName(requset);
        }

        /// <summary>
        /// 模糊查询事件
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        public AppWarnEventMoel Get(RouteSearchWarnInfoName requset)
        {
            return AppApiManager.GetSearchWarnInfoName(requset);
        }

        /// <summary>
        /// 返回未读的条数
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        public AppMessageRemind Get(RouteGetMessageNoReadCount requset)
        {
            return AppApiManager.GetMessageNoReadCount(requset);
        }
        /// <summary>
        /// 返回正在执行任务的列表
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        public AppGetByUserExecutingModel Get(RouteGetByUserExecutingList requset)
        {
            return AppApiManager.GetByUserExecutingList(requset);
        }


        public List<Model.AppApi.AppWarnInfo> Post(RouteGetWarnListByWarnId request)
        {
            return AppApiManager.GetAllWarn(request);

        }
        /// <summary>
        /// b/s人员列表
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        public AppBsPesonModel Get(RouteBsGetByWarnInfoIdPerson requset)
        {
            return AppApiManager.GetBsByWarnInfoIdPerson(requset);
        }
        /// <summary>
        /// 判断是否显示转发按钮
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResult Get(RouteGetForwardExist request)
        {
            return AppApiManager.GetForwardExist(request);
        }
        //Up2对接移动接口登录
        public AppLoginModel Get(RoutePostAppLoginInfo request)
        {
            return AppApiManager.GetAppLoginInfo(request);
        }

        public AppQusetionModel Get(RouteGetQuestionList requset)
        {
            return AppApiManager.GetQusertionList(requset);
        }


        public AppUserGpsViewModel post(AppNewGpsGuiJi request)
        {
            return AppApiManager.AppNewGpsGuiJi(request);
        }
        //获取小事件通过村adcd和大事件Id
        public List<AppXiaoshijian> post(RouteGetWarnEventNext request) {
            return AppApiManager.AppGetWarnEventNext(request);
        }
        //获取app的县级注册人数
        public int post(AppGetRegCount request) {
            return AppApiManager.AppGetRegCount(request);
        }
        //获取app的镇级注册人数
        public int post(AppGetRegCountByCountyAdcdForTown request) {
            return AppApiManager.AppGetRegCountByCountyAdcdForTown(request);
        }
        //镇级下拉事件
        public AppWarnViewModel Get(RouteTownGetAppWarnSelect request)
        {
            return AppApiManager.GetTownGetAppWarnSelect(request);
        }

        //获取app的镇级注册人数
        public int post(AppGetRegCountByCountyAdcdForVillage request)
        {
            return AppApiManager.AppGetRegCountByCountyAdcdForVillage(request);
        }

        public List<AppGpsVillageDisplay> post(AppGetGpsLocation request) {
            return AppApiManager.AppGpsVillageDisplay(request);
        }
        public BaseResult Get(RouteVillageLoaction request)
        {
            return AppApiManager.AddLoactionAndRecord(request);
        }


        //点击乡村名字获取大事件名称
        public List<AppWarnEventMoel2> post(RouteGetWarnListByVillageAdcd requst) {
            return AppApiManager.GetWarnListByVillageAdcd(requst);
        }
        //弹出层获取岗位信息
        public List<AppLvZhi> post(AppGetPostByUserName request) {
            return AppApiManager.GetPostByUserName(request);
        }

        public int post(AppCountySendMessage request) {
            return AppApiManager.CountySendMessage(request);
        }

        public AppWarnViewModel post(RouteTownGetAppWarnSelect request) {
            return AppApiManager.GetTownGetAppWarnSelect(request);
        }
        //履职地图界面上获取岗位
        public List<AppPostcode> post(AppGetPostcode request)
        {
            return AppApiManager.GetAppPostcodeOnLvZhi(request);
        }
        
        //履职地图界面上获取履职轨迹
        public List<AppLocation> post(AppGetLocationOnLvZhi request)
        {
            return AppApiManager.GetLocationOnLvZhi(request);
        }
        
        //履职地图界面上获取履职记录
        public List<AppTaskRecord> post(AppGetTaskRecordOnLvZhi request)
        {
            return AppApiManager.GetTaskRecordOnLvZhi(request);
        }
    }
}