// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteAppApi.cs" company="abc">
//   abc
// </copyright>
// <summary>
//   Defines the AppGetLoginVCode type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Dy.Common;

namespace GrassrootsFloodCtrl.ServiceModel.Route
{
    using System.Collections.Generic;
    using System.Web;

    using GrassrootsFloodCtrl.Model;
    using GrassrootsFloodCtrl.ServiceModel.AppApi;
    using GrassrootsFloodCtrl.ServiceModel.Common;

    using ServiceStack;
    using System;

    /// <summary>
    /// The app get login v code.
    /// </summary>
    [Route("/AppApi/AppGetLoginVCode", "POST", Summary = "接口：获取验证码")]
    [Api("移动端")]
    public class AppGetLoginVCode : IReturn<BaseResult>
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户名(手机号)")]
        public string UserName { get; set; }
    }

    /// <summary>
    /// The app login.
    /// </summary>
    [Route("/AppApi/AppLogin", "POST", Summary = "接口：登陆")]
    [Api("移动端")]
    public class AppLogin : IReturn<BaseResult>
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户名(手机号)")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the v code.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "验证码")]
        public string VCode { get; set; }
    }

    /// <summary>
    /// The app get user info.
    /// </summary>
    [Route("/AppApi/AppGetUserInfo", "POST", Summary = "接口：获取用户信息")]
    [Api("移动端")]
    public class AppGetUserInfo : IReturn<AppUserInfoViewModel>
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户token")]
        public string token { get; set; }
    }

    /// <summary>
    /// The app grid type.
    /// </summary>
    [Route("/AppApi/AppGridType", "GET", Summary = "接口：网格类型")]
    [Api("移动端")]
    public class AppGridType : IReturn<Model.Grid.Grid>
    {
    }

    /// <summary>
    /// The app post type.
    /// </summary>
    [Route("/AppApi/AppPostType", "GET", Summary = "接口：岗位类型")]
    [Api("移动端")]
    public class AppPostType : IReturn<Model.Post.Post>
    {
    }

    /// <summary>
    /// The app post sign and file.
    /// </summary>
    [Route("/AppApi/AppPostSignAndFile", "POST", Summary = "接口：问题提交")]
    [Api("移动端")]
    public class AppPostSignAndFile : IReturn<BaseResult>
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "消息Id")]
        public string messageId { get; set; }

        /// <summary>
        /// Gets or sets the adcd.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户adcd")]
        public string adcd { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户账号")]
        public string username { get; set; }

        /// <summary>
        /// Gets or sets the post typecode.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "岗位类型")]
        public string postTypecode { get; set; }

        /// <summary>
        /// Gets or sets the post code.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "岗位职责")]
        public string postCode { get; set; }

        /// <summary>
        /// Gets or sets the grid type.
        /// </summary>
        [ApiMember(IsRequired = false, DataType = "string", Description = "网格类型")]
        public string gridType { get; set; }

        /// <summary>
        /// Gets or sets the step.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "第几步骤")]
        public string step { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "内容描述")]
        public string values { get; set; }

        /// <summary>
        /// Gets or sets the step item.
        /// </summary>
        [ApiMember(IsRequired = false, DataType = "string", Description = "村级责任人，是否接到上级防汛通知，选项")]
        public string stepItem { get; set; }

        /// <summary>
        /// Gets or sets the values item.
        /// </summary>
        [ApiMember(IsRequired = false, DataType = "string", Description = "是否接到上级防汛通知，其他选项内容描述")]
        public string valuesItem { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        [ApiMember(IsRequired = false, DataType = "string", Description = "按 “经度1，纬度1；经度2,纬度2....”格式，单点多点格式雷同，经纬度逗号分隔，两个坐标分号分隔")]
        public string location { get; set; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        [ApiMember(IsRequired = false, DataType = "string", Description = "如果有拍照等附件，附件的文件名")]
        public string fileName { get; set; }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        [ApiMember(IsRequired = false, DataType = "file", Description = "附带照片zip文件")]
        public HttpPostedFile file { get; set; }
    }

    #region
    //[Route("/AppApi/AppPostQuestion", "POST", Summary = "接口：带问题的提交---是否问题(包括：村级负责人、监测预警组(包含预警员和监测员)、抢险救援组)")]
    //[Api("移动端")]
    //public class AppPostQuestion : IReturn<BaseResult>
    //{
    //    [ApiMember(IsRequired = true, DataType = "string", Description = "用户账号")]
    //    public string username { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "岗位类型")]
    //    public string postTypecode { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "岗位职责")]
    //    public string postCode { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "网格类型")]
    //    public string gridType { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "第几步骤")]
    //    public string step { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "是或否，true，false选项中的一个")]
    //    public string values { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "按 “经度1，纬度1；经度2,纬度2”格式，单点多点格式雷同，经纬度逗号分隔，两个坐标分号分隔")]
    //    public string location { get; set; }
    //}

    //[Route("/AppApi/AppPostSign", "POST", Summary = "接口：带问题的提交---签到带轨迹(包括:管理员)")]
    //[Api("移动端")]
    //public class AppPostSign : IReturn<BaseResult>
    //{
    //    [ApiMember(IsRequired = true, DataType = "string", Description = "用户账号")]
    //    public string username { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "岗位类型")]
    //    public string postTypecode { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "岗位职责")]
    //    public string postCode { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "网格类型")]
    //    public string gridType { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "第几步骤")]
    //    public string step { get; set; }

    //    [ApiMember(IsRequired = false, DataType = "string", Description = "填空值")]
    //    public string values { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "按 “经度1，纬度1；经度2,纬度2....”格式，单点多点格式雷同，经纬度逗号分隔，两个坐标分号分隔")]
    //    public string location { get; set; }
    //}

    //[Route("/AppApi/AppPostTxt", "POST", Summary = "接口：带问题的提交---输入文本，转移人数(包括：人员转移组)")]
    //[Api("移动端")]
    //public class AppPostTxt : IReturn<BaseResult>
    //{
    //    [ApiMember(IsRequired = true, DataType = "string", Description = "用户账号")]
    //    public string username { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "岗位类型")]
    //    public string postTypecode { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "岗位职责")]
    //    public string postCode { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "网格类型")]
    //    public string gridType { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "第几步骤")]
    //    public string step { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "转移人数文本")]
    //    public string values { get; set; }

    //    [ApiMember(IsRequired = true, DataType = "string", Description = "按 “经度1，纬度1；经度2,纬度2....”格式，单点多点格式雷同，经纬度逗号分隔，两个坐标分号分隔")]
    //    public string location { get; set; }
    //}
    #endregion

    /// <summary>
    /// The app post fill in.
    /// </summary>
    [Route("/AppApi/AppPostFillIn", "POST", Summary = "接口：问题提交补填")]
    [Api("移动端")]
    public class AppPostFillIn : IReturn<BaseResult>
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "Guid", Description = "问题id")]
        public string qusetionId { get; set; }

        /// <summary>
        /// Gets or sets the adcd.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户adcd")]
        public string adcd { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户账号")]
        public string username { get; set; }

        /// <summary>
        /// Gets or sets the post typecode.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "岗位类型")]
        public string postTypecode { get; set; }

        /// <summary>
        /// Gets or sets the post code.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "岗位职责")]
        public string postCode { get; set; }

        /// <summary>
        /// Gets or sets the post time.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "补填时间")]
        public string postTime { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "补填说明")]
        public string values { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        [ApiMember(IsRequired = false, DataType = "string", Description = "按 “经度1，纬度1；经度2,纬度2....”格式，单点多点格式雷同，经纬度逗号分隔，两个坐标分号分隔")]
        public string location { get; set; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        [ApiMember(IsRequired = false, DataType = "string", Description = "如果有拍照等附件，附件的文件名")]
        public string fileName { get; set; }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        [ApiMember(IsRequired = false, DataType = "file", Description = "如果有拍照等附件，添加")]
        public HttpPostedFile file { get; set; }
    }

    /// <summary>
    /// The app get mail list.
    /// </summary>
    [Route("/AppApi/AppGetMailList", "POST", Summary = "接口：获取通讯录")]
    [Api("移动端")]
    public class AppGetMailList : IReturn<List<MailListViewModel>>
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户token")]
        public string token { get; set; }
    }

    /// <summary>
    /// The get app record.
    /// </summary>
    [Route("/AppApi/GetAppRecord", "POST", Summary = "接口：获取提交过的问题")]
    [Api("移动端")]
    public class GetAppRecord : PageQuery, IReturn<BsTableDataSource<AppRecordViewModel>>
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户token")]
        public string token { get; set; }

        /// <summary>
        /// Gets or sets the post typecode.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "岗位类型")]
        public string postTypecode { get; set; }

        /// <summary>
        /// Gets or sets the post code.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "岗位职责")]
        public string postCode { get; set; }
    }

    /// <summary>
    /// The create record.
    /// </summary>
    [Route("/AppApi/CreatGps", "POST", Summary = "接口：获取gps显示的数据")]
    [Api("移动端")]
    public class CreateGpsRecord : PageQuery, IReturn<BsTableDataSource<AppUserGps>>
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户手机号")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the lng.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "纬度")]
        public string Lng { get; set; }

        /// <summary>
        /// Gets or sets the lat.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "经度")]
        public string Lat { get; set; }

        /// <summary>
        /// Gets or sets the user adcd.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "adcd")]
        public string UserAdcd { get; set; }
    }

    /// <summary>
    /// The get gps list.
    /// </summary>
    [Route("/AppApi/GetGpsList", "POST", Summary = "获取所有的gps的点")]
    public class GetGpsList : PageQuery, IReturn<BsTableDataSource<AppUserGps>>
    {
        /// <summary>
        /// Gets or sets the user adcd.
        /// </summary>
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户ADCD")]
        public int AppWarnEventId { get; set; }
    }

    [Route("/AppApi/GetGpsListByUserName", "POST", Summary = "获取用户名的所有的gps的点")]
    public class GetGpsListByUserName : IReturn<List<AppUserGps>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户名")]
        public string UserName { get; set; }

    }
    /// <summary>
    /// The get village person item.
    /// </summary>
    [Route("/AppApi/GetVillagePersonItem", "Get", Summary = "接口：村级责任人，是否接到上级防汛通知，选项")]
    [Api("移动端")]
    public class GetVillagePersonItem : IReturn<List<VillagePersonItemViewModel>>
    {}

    [Route("/AppApi/AppNewGpsGuiJi", "post", Summary = "获取轨迹通过用户名")]
    [Api("移动端")]
    public class AppNewGpsGuiJi : IReturn<AppUserGpsViewModel> {
        [ApiMember(IsRequired =true,DataType ="string",Description ="用户名")]
        public string UserName { get; set; }
    }

    [Route("/AppApi/AppGetRegCount", "post", Summary = "通过adcd获取县注册人数")]
    [Api("移动端")]
    public class AppGetRegCount : IReturn<Int32> {
        [ApiMember(IsRequired =true,DataType ="string",Description ="县级adcd")]
        public string adcd { get; set; }
        [ApiMember(IsRequired =true,DataType ="int",Description ="等级：省：0，市：1，县：2，镇：3，村：4")]
        public int grad { get; set; }
    }
    [Route("/AppApi/AppGetRegCountByCountyAdcdForTown", "post", Summary = "通过adcd获取镇注册人数")]
    [Api("移动端")]
    public class AppGetRegCountByCountyAdcdForTown : IReturn<Int32>
    {
        [ApiMember(IsRequired =true,DataType ="string",Description ="县级adcd")]
        public string adcd { get; set; }
    }

    [Route("/AppApi/AppGetRegCountByCountyAdcdForVillage", "post", Summary = "通过adcd获取村注册人数")]
    [Api("移动端")]
    public class AppGetRegCountByCountyAdcdForVillage : IReturn<Int32>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "县级adcd")]
        public string adcd { get; set; }
    }

    [Route("/AppApi/AppGetGpsLocation", "post", Summary = "获取gps的定位")]
    [Api("移动端")]
    public class AppGetGpsLocation : IReturn<List<AppGpsVillageDisplay>> {
        [ApiMember(IsRequired = true,DataType ="string",Description ="村adcd")]
        public string villageadcd { get; set; }
        [ApiMember(IsRequired =true,DataType ="int",Description ="小事件id")]
        public int warninfoid { get; set; }
    }
    //履职岗位等信息
    [Route("/AppApi/AppGetPostByUserName", "post", Summary = "通过手机号获取岗位信息")]
    [Api("移动端")]
    public class AppGetPostByUserName : IReturn<AppLvZhi>
    {
        [ApiMember(IsRequired =true,DataType ="string",Description ="手机号码")]
        public string username { get; set; }
        [ApiMember(IsRequired =true,DataType ="string",Description ="小事件")]
        public string xiaoshijian { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "大事件")]
        public string dashijian { get; set; }
    }

    [Route("/AppApi/AppGetPostByUserName2", "post", Summary = "履职地图上显示岗位信息")]
    [Api("移动端")]
    public class AppGetPostcode : IReturn<List<AppPostcode>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户手机号")]
        public string username { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "小事件ID")]
        public int warninfoid { get; set; }
    }
    
    [Route("/AppApi/AppGetLocationOnLvZhi", "post", Summary = "履职地图上显示履职轨迹")]
    [Api("移动端")]
    public class AppGetLocationOnLvZhi : IReturn<List<AppLocation>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户手机号")]
        public string username { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "小事件ID")]
        public int warninfoid { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "岗位名称")]
        public string postcode { get; set; }
    }
    
    [Route("/AppApi/AppGetTaskRecordOnLvZhi", "post", Summary = "履职地图上显示履职记录")]
    [Api("移动端")]
    public class AppGetTaskRecordOnLvZhi : IReturn<List<AppTaskRecord>>
    {
        [ApiMember(IsRequired = true, DataType = "string", Description = "用户手机号")]
        public string username { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "小事件ID")]
        public int warninfoid { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "岗位名称")]
        public string postcode { get; set; }
    }
        
    
    //县级发送
    [Route("/AppApi/AppCountySendMessage", "post", Summary = "县级创建事件后发送信息")]
    [Api("移动端")]
    public class AppCountySendMessage : IReturn<Int32> {
        [ApiMember(IsRequired =true,DataType ="int",Description ="大事件id")]
        public string AppWarnEventId { get; set; }
        [ApiMember(IsRequired = true, DataType = "int", Description = "事件等级")]
        public string WarnLevel { get; set; }
        [ApiMember(IsRequired = true, DataType = "string", Description = "消息内容")]
        public string WarnMessage { get; set; } 
        [ApiMember(IsRequired = true, DataType = "string", Description = "备注")]
        public string Remark { get; set; }
    }
}
