// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAppApiManager.cs" company="abc">
//   abc
// </copyright>
// <summary>
//   Defines the IAppApiManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GrassrootsFloodCtrl.Logic.AppApi
{
    using System.Collections.Generic;
    using GrassrootsFloodCtrl.Model;
    using GrassrootsFloodCtrl.Model.AppApi;
    using GrassrootsFloodCtrl.ServiceModel.AppApi;
    using GrassrootsFloodCtrl.ServiceModel.Common;
    using GrassrootsFloodCtrl.ServiceModel.Route;
    using GrassrootsFloodCtrl.Logic.Factory;

    /// <summary>
    /// The AppApiManager interface.
    /// </summary>
    public interface IAppApiManager
    {
        // 获取验证码

        /// <summary>
        /// The app get login v code.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="BaseResult"/>.
        /// </returns>
        BaseResult AppGetLoginVCode(AppGetLoginVCode request);

        // 登陆

        /// <summary>
        /// The app login.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="BaseResult"/>.
        /// </returns>
        BaseResult AppLogin(AppLogin request);

        // token获取用户信息

        /// <summary>
        /// The app get user info.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="AppUserInfoViewModel"/>.
        /// </returns>
        AppUserInfoViewModel AppGetUserInfo(AppGetUserInfo request);

        // 问题提交

        /// <summary>
        /// The app post sign and file.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="BaseResult"/>.
        /// </returns>
        BaseResult AppPostSignAndFile(AppPostSignAndFile request);

        // 问题补提

        /// <summary>
        /// The app post fill in.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="BaseResult"/>.
        /// </returns>
        BaseResult AppPostFillIn(AppPostFillIn request);

        // 通讯录

        /// <summary>
        /// The app get mail list.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        List<MailListViewModel> AppGetMailList(AppGetMailList request);

        // 获取提交过的问题

        /// <summary>
        /// The get app record.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>BsTableDataSource</cref>
        ///     </see>
        ///     .
        /// </returns>
        BsTableDataSource<AppRecordViewModel> GetAppRecord(GetAppRecord request);

        // 村级责任人，是否接到上级防汛通知，选项

        /// <summary>
        /// The get village person item.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        List<VillagePersonItemViewModel> GetVillagePersonItem(GetVillagePersonItem request);

        // 根据是否预警的状态返回该县的列表预警列表

        /// <summary>
        /// The get warn select list.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="AppWarnViewModel"/>.
        /// </returns>
        AppWarnViewModel GetWarnSelectList(RouteGetAppWarnSelect request);

        // 根据事件名称进行查询事件空某人为所有

        /// <summary>
        /// The get warnt list.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="AppWarnViewModel"/>.
        /// </returns>
        AppWarnViewModel GetWarntList(RouteGetAppWarnList request);

        /// <summary>
        /// 增加预警信息默认为空
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResult AddAppWarnEvent(RouteAddAppWarnEvent request);

        /// <summary>
        /// 增加发送预警和消息
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        BaseResult AddAppWarnInfoAndAppSendMessage(RouteAppWarnInfo requset);

        /// <summary>
        /// 增加发送预警信息
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="userAdcd">
        /// The user Adcd.
        /// </param>
        /// <returns>
        /// </returns>
        bool AddAppWarnInfo(AppWarnInfo model, string userAdcd);

        /// <summary>
        /// 根据权限不同插入到发送消息表
        /// </summary>
        /// <param name="addList">
        /// The add List.
        /// </param>
        /// <returns>
        /// </returns>
        int AddAppSendMessage(string sql);

        /// <summary>
        /// 根据权限生成发送消息的列表
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="byRoleIdUserList">
        /// The by Role Id User List.
        /// </param>
        /// <param name="userPhone">
        /// The user Phone.
        /// </param>
        /// <param name="userName">
        /// The user Name.
        /// </param>
        /// <returns>
        /// </returns>
        string GetAppSendMessageInserSql(AppSendMessage model, List<AppSendMessage> byRoleIdUserList, string userPhone,
            string userName);

        string GetValiageAppSendMessageInserSql(AppSendMessage model, List<AppSendMessageAndPostModel> byRoleIdUserList,
            string userPhone, string userName);

        /// <summary>
        /// 根据RoleId获取发送消息的人员;默认全体发送
        /// </summary>
        /// <param name="requset">
        /// The requset.
        /// </param>
        /// <returns>
        /// </returns>
        List<AppSendMessage> GetByRoleIdUserList(RouteAppWarnInfo requset);

        /// <summary>
        /// 获取发送消息列表
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        AppSendMessageModel GetSendCountyAppSendMessage(RouteGetSendCountyAppSendMessage requset);

        /// <summary>
        /// 获取接收消息的列表
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        AppSendMessageModel GetReceiveCountyAppSendMessage(RouteGetReceiveCountyAppSendMessage requset);

        /// <summary>
        /// 更改消息的状态
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        BaseResult UpdateAppSendMessage(RouteUpdateAppSendMessage requset);

        /// <summary>
        /// 根据ID获取
        /// </summary>
        /// <param name="requset">
        /// The requset.
        /// </param>
        /// <returns>
        /// </returns>
        AppSendMessage GetByIdAppSendMessageDetails(RouteByIdAppSendMessageDetails requset);


        /// <summary>
        /// The get by id app send message detail.
        /// </summary>
        /// <param name="requset">
        /// The requset.
        /// </param>
        /// <returns>
        /// The <see cref="AppSendDetailsModel"/>.
        /// </returns>
        AppSendDetailsModel GetByIdAppSendMessageDetail(RouteByIdAppSendMessageDetails requset);

        /// <summary>
        /// 获取人员列表
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        AppPersonModel GetByWarnInfoIdPerson(RouteGetByWarnInfoIdPerson requset);

        AppBsPesonModel GetBsByWarnInfoIdPerson(RouteBsGetByWarnInfoIdPerson requset);

        /// <summary>
        /// 关闭整个任务列表
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        BaseResult GetByWarnInfoIdCloseEvent(RouteGetByWarnInfoIdCloseEvent requset);

        /// <summary>
        /// 获取所有的等级列表
        /// </summary>
        /// <param name="requset">
        /// The requset.
        /// </param>
        /// <returns>
        /// </returns>
        AppWarnLevelListModel GetAllWarnLevel(RouteGetEventLevel requset);

        /// <summary>
        /// 核对用户
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        AppLoginModel CheckUser(RouteCheckUser requset);

        /// <summary>
        /// The create gps record.
        /// </summary>
        /// <param name="appgps">
        /// The appgps.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        GrassrootsFloodCtrl.ServiceModel.AppApi.AppUserGps CreateGpsRecord(CreateGpsRecord appgps);


        BsTableDataSource<GrassrootsFloodCtrl.ServiceModel.AppApi.AppUserGps> GetGpsList(GetGpsList request);

        //List<GrassrootsFloodCtrl.ServiceModel.AppApi.AppUserGps> GetGpsListByUserName(GetGpsListByUserName request);
        /// <summary>
        /// b/s端查询接收的信息的接口
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        AppSumSendMessage GetPCReceiveCountyAppSendMessage(RouteGetPCReceiveCountyAppSendMessage requset);

        /// <summary>
        /// b/s端查询发送的信息的接口
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        AppSumSendMessage GetPCSendCountyAppSendMessage(RouteGetPCSendCountyAppSendMessage requset);

        /// <summary>
        /// 模糊查询任务名字
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        AppWarnEventMoel GetSearchWarnEventName(RouteSearchWarnEventName requset);

        /// <summary>
        /// 模糊查询事件名字
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        AppWarnEventMoel GetSearchWarnInfoName(RouteSearchWarnInfoName requset);

        /// <summary>
        /// 获取消息未读的条数
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        AppMessageRemind GetMessageNoReadCount(RouteGetMessageNoReadCount requset);

        /// <summary>
        /// 获取最大的正在执行的列表
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        AppGetByUserExecutingModel GetByUserExecutingList(RouteGetByUserExecutingList requset);


        List<GrassrootsFloodCtrl.Model.AppApi.AppWarnInfo> GetAllWarn(RouteGetWarnListByWarnId reqeust);


        List<GrassrootsFloodCtrl.ServiceModel.AppApi.AppUserGps> GetGpsListByUserName(GetGpsListByUserName request);

        /// <summary>
        /// 镇级是否显示转发按钮
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseResult GetForwardExist(RouteGetForwardExist request);

        int UserExistCount(List<AppSendMessage> list);

        List<AppPersonListModel> UserExist(List<AppPersonListModel> list);

        //用户登录APP返回信息
        AppLoginModel GetAppLoginInfo(RoutePostAppLoginInfo request);

        //判断用户是否登录过
        bool CheckUserFirstLoginExist(string userName);

        bool InserUserInApp(AppMobileLogin model);
        AppMobileLogin CheckUserExist(RoutePostAppLoginInfo request);

        IGradelevelFactory CreateGradelevelFactory(string adcd);

        List<AppSendMessageAndPostModel> GetVillageList(RouteAppWarnInfo requset);

        AppQusetionModel GetQusertionList(RouteGetQuestionList requset);

        AppUserGpsViewModel AppNewGpsGuiJi(AppNewGpsGuiJi request);

        List<AppXiaoshijian> AppGetWarnEventNext(RouteGetWarnEventNext request);

        //通过adcd获取注册人数
        int AppGetRegCount(AppGetRegCount request);

        //通过县级Adcd获取APP镇级的注册人数
        int AppGetRegCountByCountyAdcdForTown(AppGetRegCountByCountyAdcdForTown request);

        //通过县级Adcd来获取村的注册人数
        int AppGetRegCountByCountyAdcdForVillage(AppGetRegCountByCountyAdcdForVillage request);

        List<AppGpsVillageDisplay> AppGpsVillageDisplay(AppGetGpsLocation request);

        List<AppWarnEventMoel2> GetWarnListByVillageAdcd(RouteGetWarnListByVillageAdcd request);

        AppWarnViewModel GetTownGetAppWarnSelect(RouteTownGetAppWarnSelect request);

        BaseResult AddLoactionAndRecord(RouteVillageLoaction request);
        List<AppLvZhi> GetPostByUserName(AppGetPostByUserName request);

        int CountySendMessage(AppCountySendMessage request);

        //在履职地图上显示岗位信息
        List<AppPostcode> GetAppPostcodeOnLvZhi(AppGetPostcode request);
        //在履职地图上显示岗位信息
        List<AppLocation> GetLocationOnLvZhi(AppGetLocationOnLvZhi request);
        //在履职地图上显示记录信息
        List<AppTaskRecord> GetTaskRecordOnLvZhi(AppGetTaskRecordOnLvZhi request);
    }
}