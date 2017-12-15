using Dy.Common;
using GrassrootsFloodCtrl.Model.DataShare;
using GrassrootsFloodCtrl.Model.Sys;
using GrassrootsFloodCtrl.Model.Town;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceModel.Country;
using GrassrootsFloodCtrl.ServiceModel.DataShare;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Town;
using GrassrootsFloodCtrl.ServiceModel.Village;
using GrassrootsFloodCtrl.ServiceModel.ZZTX;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrassrootsFloodCtrl.Logic.DataShare
{
    public class DataShareManager : ManagerBase, IDataShareManager
    {
        /// <summary>
        /// 县级责任人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<DSCountryPersonServiceModel> CountyPersLiableList(CountyPersLiableList request)
        {
            List<DSCountryPersonServiceModel> _list = null;
            List<DataShareReturnModel> list = new List<DataShareReturnModel>();
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(request.username.Trim()) || string.IsNullOrEmpty(request.password.Trim()) || string.IsNullOrEmpty(request.adcd.Trim())) throw new Exception("账号,密码,adcd不能为空！");
                //var f_user = db.Single<UserInfo>(w => w.UserName == request.username.Trim() && w.PassWord == DESHelper.DESEncrypt(request.password.Trim()) && w.adcd == request.adcd.Trim() && w.isEnable == true);
                list = db.SqlList<DataShareReturnModel>("EXEC DataShareGetUser @username,@password", new { UserName = request.username, Password = request.password });
                if (list != null)
                {
                    if (list[0].adcd.Contains("00000000000"))
                    {
                        //市
                        _list = db.SqlList<DSCountryPersonServiceModel>("EXEC CountyList @adcd", new { adcd = list[0].adcd.Substring(0, 4) });
                    }
                    else if (list[0].adcd.Contains("000000000"))
                    {
                        //县
                        _list = db.SqlList<DSCountryPersonServiceModel>("EXEC CountyList @adcd", new { adcd = list[0].adcd });
                    }
                    else { }
                }
                return _list;
            }
        }

        /// <summary>
        /// 镇级责任人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<DSTownPersonLiableViewModel> TownPersLiableList(TownPersLiableList request)
        {
            List<DSTownPersonLiableViewModel> _list = new List<DSTownPersonLiableViewModel>();
            List<DataShareReturnModel> list = new List<DataShareReturnModel>();
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(request.username.Trim()) || string.IsNullOrEmpty(request.password.Trim()) || string.IsNullOrEmpty(request.adcd.Trim())) throw new Exception("账号,密码,adcd不能为空！");
                //var f_user = db.Single<UserInfo>(w => w.UserName == request.username.Trim() && w.PassWord == DESHelper.DESEncrypt(request.password.Trim()) && w.adcd == request.adcd.Trim() && w.isEnable == true);
                list = db.SqlList<DataShareReturnModel>("EXEC DataShareGetUser @username,@password", new { UserName = request.username, Password = request.password });
                if (list != null)
                {
                    var builder = db.From<TownPersonLiable>();
                    if (list[0].adcd.Contains("00000000000"))
                    {
                        //市
                        //builder.Where(w=> w.adcd.StartsWith(f_user.adcd.Substring(0, 4)));
                        //builder.Select("adcd,Name,Post,Position,Mobile,Remark");
                        //_list = db.Select<TownPersonLiableViewModel>(builder);
                        _list = db.SqlList<DSTownPersonLiableViewModel>("EXEC TownList @adcd", new { adcd = list[0].adcd.Substring(0, 4) });
                    }
                    else if (list[0].adcd.Contains("000000000"))
                    {
                        //县
                        //builder.Where(w => w.adcd.StartsWith(f_user.adcd.Substring(0, 6)));
                        //builder.Select("adcd,Name,Post,Position,Mobile,Remark");
                        //_list = db.Select<TownPersonLiableViewModel>(builder);
                        _list = db.SqlList<DSTownPersonLiableViewModel>("EXEC TownList @adcd", new { adcd = list[0].adcd.Substring(0, 6) });
                    }
                    else { }
                }
                return _list;
            }
        }

        /// <summary>
        /// 村级工作组
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<DSVillageWorkingGroupViewModel> VillageGroupPersLiableList(VillageGroupPersLiableList request)
        {
            List<DSVillageWorkingGroupViewModel> _list = null;
            List<DataShareReturnModel> list = new List<DataShareReturnModel>();
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(request.username.Trim()) || string.IsNullOrEmpty(request.password.Trim()) || string.IsNullOrEmpty(request.adcd.Trim())) throw new Exception("账号,密码,adcd不能为空！");
                //var f_user = db.Single<UserInfo>(w => w.UserName == request.username.Trim() && w.PassWord == DESHelper.DESEncrypt(request.password.Trim()) && w.adcd == request.adcd.Trim() && w.isEnable == true);
                list = db.SqlList<DataShareReturnModel>("EXEC DataShareGetUser @username,@password", new { UserName = request.username, Password = request.password });
                if (list != null)
                {
                    var builder = db.From<VillageWorkingGroup>();
                    if (list[0].adcd.Contains("00000000000"))
                    {
                        //市
                        _list = db.SqlList<DSVillageWorkingGroupViewModel>("EXEC VillageWorkingGroupList @adcd", new { adcd = list[0].adcd.Substring(0, 4) });
                    }
                    else if (list[0].adcd.Contains("000000000"))
                    {
                        //县
                        _list = db.SqlList<DSVillageWorkingGroupViewModel>("EXEC VillageWorkingGroupList @adcd", new { adcd = list[0].adcd.Substring(0, 6) });
                    }
                    else { }
                }
                return _list;
            }
        }

        /// <summary>
        /// 村级网格责任人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<DSVillageGridViewModel> VillageGridPersLiableList(VillageGridPersLiableList request)
        {
            List<DSVillageGridViewModel> _list = new List<DSVillageGridViewModel>();
            List<DataShareReturnModel> list = new List<DataShareReturnModel>();
            
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(request.username.Trim()) || string.IsNullOrEmpty(request.password.Trim()) || string.IsNullOrEmpty(request.adcd.Trim())) throw new Exception("账号,密码,adcd不能为空！");
                //var f_user = db.Single<UserInfo>(w => w.UserName == request.username.Trim() && w.PassWord == DESHelper.DESEncrypt(request.password.Trim()) && w.adcd == request.adcd.Trim() && w.isEnable == true);
                list = db.SqlList<DataShareReturnModel>("EXEC DataShareGetUser @username,@password", new { UserName = request.username, Password = request.password });
                if (list != null)
                {
                    var builder = db.From<VillageGridPersonLiable>();
                    if (list[0].adcd.Contains("00000000000"))
                    {
                        //市
                        _list = db.SqlList<DSVillageGridViewModel>("EXEC VillageGridList @adcd", new { adcd = list[0].adcd.Substring(0, 4) });
                    }
                    else if (list[0].adcd.Contains("000000000"))
                    {
                        //县
                        _list = db.SqlList<DSVillageGridViewModel>("EXEC VillageGridList @adcd", new { adcd = list[0].adcd.Substring(0, 6) });
                    }
                    else { }
                }
                return _list;
            }
        }

        /// <summary>
        /// 村级人员转移清单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<DSVillageTransferPersonViewModel> VillageTransferPersLiableList(VillageTransferPersLiableList request)
        {
            //List<DSVillageTransferPersonViewModel> _list = new List<DSVillageTransferPersonViewModel>();
            List<DataShareReturnModel> list = new List<DataShareReturnModel>();
            List<DSVillageTransferPersonViewModel> _list = new List<DSVillageTransferPersonViewModel>();
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(request.username.Trim()) || string.IsNullOrEmpty(request.password.Trim()) || string.IsNullOrEmpty(request.adcd.Trim())) throw new Exception("账号,密码,adcd不能为空！");
                //var builder = db.From<DatashareUser>();
                list = db.SqlList<DataShareReturnModel>("EXEC DataShareGetUser @username,@password", new { UserName = request.username, Password = request.password });
                //return list;
                
                if (list != null)
                {
                    var builder = db.From<VillageTransferPerson>();
                    if (list[0].adcd.Contains("00000000000"))
                    {
                        //市
                        _list = db.SqlList<DSVillageTransferPersonViewModel>("EXEC VillageTransferPersonList @adcd", new { adcd = list[0].adcd.Substring(0, 4) });
                    }
                    else if (list[0].adcd.Contains("000000000"))
                    {
                        //县
                        _list = db.SqlList<DSVillageTransferPersonViewModel>("EXEC VillageTransferPersonList @adcd", new { adcd = list[0].adcd.Substring(0, 6) });
                    }
                    else { }
                }
                
            }
            return _list;
        }

        /// <summary>
        /// 行政区划经纬度
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<DSADCDDisasterViewModel> ADCDList(ADCDList request)
        {
            List<DSADCDDisasterViewModel> _list = new List<DSADCDDisasterViewModel>();
            List<DataShareReturnModel> list = new List<DataShareReturnModel>();
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(request.username.Trim()) || string.IsNullOrEmpty(request.password.Trim()) || string.IsNullOrEmpty(request.adcd.Trim())) throw new Exception("账号,密码,adcd不能为空！");
                list = db.SqlList<DataShareReturnModel>("EXEC DataShareGetUser @username,@password", new { UserName = request.username, Password = request.password });

                if (list != null)
                {
                    var builder = db.From<ADCDInfo>();
                    if (list[0].adcd.Trim().Contains("00000000000"))
                    {
                        //市
                        _list = db.SqlList<DSADCDDisasterViewModel>("EXEC ADCDList @adcd", new { adcd = list[0].adcd.Trim().Substring(0, 4) });
                    }
                    else if (list[0].adcd.Trim().Contains("000000000"))
                    {
                        //县
                        _list = db.SqlList<DSADCDDisasterViewModel>("EXEC ADCDList @adcd", new { adcd = list[0].adcd.Trim().Substring(0, 6) });
                    }
                    else { }
                }
                return _list;
            }
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<DataShareReturnModel> DSLogin(DSLogin request)
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password)) throw new Exception("用户名或密码为空！");
            List<DataShareReturnModel> _list = new List<DataShareReturnModel>();
            using (var db = DbFactory.Open())
            {
                //var builder = db.From<DatashareUser>();
                //builder.LeftJoin<UserInfo, UserRoleInfo>((x,y)=>x.Id==y.UserID);
                //builder.Where(w => w.UserName == request.UserName.Trim() && w.PassWord == DESHelper.DESEncrypt(request.Password.Trim()));
                ////builder.Select("UserInfo.UserName,UserInfo.RealName,UserInfo.adcd,UserInfo.isEnable,UserRoleInfo.RoleID");
                //_model = db.Single<DatashareUser>(builder);

                _list = db.SqlList<DataShareReturnModel>("EXEC DataShareGetUser @username,@password", new { UserName = request.UserName, Password = request.Password });
                return _list;
            }
        }
    }
}