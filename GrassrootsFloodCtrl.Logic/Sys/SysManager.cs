using System;
using System.Linq;
using Dy.Common;
using GrassrootsFloodCtrl.Model;
using GrassrootsFloodCtrl.ServiceModel.Route;
using GrassrootsFloodCtrl.ServiceModel.Sys;
using ServiceStack.OrmLite;
using GrassrootsFloodCtrl.Model.Sys;
using GrassrootsFloodCtrl.Model.ZZTX;
using System.Collections;
using System.Net.Http;
using GrassrootsFloodCtrl.Logic.Common;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.Model.Messgae;
using ServiceStack;
using ServiceStack.Caching;
using GrassrootsFloodCtrl.Logic.Message;

namespace GrassrootsFloodCtrl.Logic.Sys
{
    /// <summary>
    /// 系统相关业务逻辑实现
    /// </summary>
    public class SysManager: ManagerBase,ISysManager
    {
        public IMessageManager MessageManager { get; set; }

        /// <summary>
       /// 获取用户列表  返回总数量total和列表row
       /// </summary>
       /// <param name="request"></param>
       /// <returns></returns>
        public BsTableDataSource<UserInfoViewModel> GetUserInfoList(GetUserInfoList request)
       {
           using (var db = DbFactory.Open())
           {
                var builder = db.From<UserInfo>();
                builder.LeftJoin<UserInfo, UserRoleInfo>((x, y) => x.Id == y.UserID);
                //builder.LeftJoin<UserRoleInfo, Role>((y, z) =>y.RoleID==z.Id);
                if (!string.IsNullOrEmpty(request.userName))
                    builder.Where(x => x.UserName.Contains(request.userName));
                if (!string.IsNullOrEmpty(request.name))
                    builder.Where(x => x.RealName.Contains(request.name));

                if (!string.IsNullOrEmpty(request.adcd) && request.adcd.IndexOf("00000000000") > 0)//地级市
                    builder.Where(x => x.adcd.StartsWith(request.adcd.Substring(0, 4)));
                else if (!string.IsNullOrEmpty(request.adcd) && request.adcd.IndexOf("000000000") > 0)//县级市区
                    builder.Where(x => x.adcd.StartsWith(request.adcd.Substring(0,6)));
                
                if (!string.IsNullOrEmpty(request.Sort))
                {
                    if (request.Order == "desc")
                        builder.OrderByFieldsDescending(request.Sort);
                    else
                        builder.OrderByFields(request.Sort);
                }
                else
                    builder.OrderBy(x => x.adcd);
                var count = db.Count(builder);
                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                builder.Limit(PageIndex, PageSize);
                builder.Select<UserInfo, UserRoleInfo>((x, y) => new
               {
                    Id=x.Id,
                    UserName=x.UserName,
                    RealName=x.RealName,
                    adcd=x.adcd,
                    Mobile=x.Mobile,
                    isEnable=x.isEnable,
                    RoleID=y.RoleID,
                    UserRealName=x.UserRealName,
                    Unit=x.Unit,
                    Position=x.Position
                });
                var list = db.Select<UserInfoViewModel>(builder);
               var roleList = db.Select<Role>();
                list.ForEach(x =>
                {
                    x.City = db.Single<ADCDInfo>(y => y.adcd == x.adcd.Substring(0, 6) + "000000000").adnm;
                    x.Role = roleList.Where(w => w.Id == x.RoleID).FirstOrDefault().RoleName;
                });
                return new BsTableDataSource<UserInfoViewModel>() {total = count, rows = list};
           }
       }

        /// <summary>
        /// 根据用户名或行政区划编码获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByUserName(string userName,string ADCD=null)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(userName))
                    throw new Exception("用户名不能为空");
                var builder = db.From<UserInfo>();
                builder.Where(x => x.UserName == userName);
                if(!string.IsNullOrEmpty(ADCD))
                    builder.And(x => x.adcd == ADCD);
                var info= db.Single(builder);
                if (info != null)
                    info.PassWord = DESHelper.DESDecrypt(info.PassWord);
                return info;
            }
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool SaveUserInfo(SaveUserInfo request)
        {
            using (var db = DbFactory.Open())
            {
                var info=new UserInfo();
                info.adcd = request.adcd;
                info.RealName = request.RealName;
                info.UserName = request.UserName;
                info.PassWord =DESHelper.DESEncrypt(request.PassWord);
                //if(!ValidatorHelper.IsMobile(request.Mobile))
                //    throw new Exception("手机号码错误");
                //info.Mobile = request.Mobile;
                info.isEnable = request.isEnable;
                info.UserRealName = request.UserRealName;
                info.Unit = request.Unit;
                info.Position = request.Position;
                var result = 0;
                if (request.id != 0)
                {
                    info.Id = request.id;
                    result = db.Update(info);
                    if (result >= 1)
                    {
                        var urInfo = new UserRoleInfo();
                        urInfo.UserID = request.id;
                        urInfo.RoleID = request.role;
                        urInfo.Id= db.Single<UserRoleInfo>(x => x.UserID == request.id).Id;
                        db.Update(urInfo);
                    }
                }
                else
                {
                    var modle = GetUserInfoByUserName(request.UserName,request.adcd);
                    if (modle == null)
                    {
                        result = (int) db.Insert(info, true);
                        if (result >= 1)
                        {
                            var urInfo = new UserRoleInfo();
                            urInfo.UserID = result;
                            urInfo.RoleID = request.role;
                            db.Insert(urInfo);
                        }
                    }
                    else
                        throw new Exception("用户名已存在");
                }
                return result >= 1;
            }
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public UserInfo Login(string userName,string passWord)
        {
            using (var db=DbFactory.Open())
            {
                if(string.IsNullOrEmpty(userName)) 
                    throw  new Exception("用户名不能为空");
                if (string.IsNullOrEmpty(passWord))
                    throw new Exception("密码不能为空");
                var builder = db.From<UserInfo>();
                builder.Where(x => x.UserName == userName.Trim() && x.PassWord == DESHelper.DESEncrypt(passWord.Trim()) &&x.isEnable);
                var list=db.Select(builder);
                if (list.Count == 1)
                {
                    var info = list[0];
                    db.UpdateOnly(new UserInfo { loginNum = info.loginNum+1},
                   onlyFields: x => x.loginNum,
                   where: x => x.Id == info.Id);
                    var log = new operateLog();
                    var IP=WebUtils.Get_ClientIP();
                    log.operateMsg = "登录系统，登录IP地址为："+ IP;
                    log.operateTime=DateTime.Now;
                    log.userName = userName.Trim();
                    var operation = JsonTools.ObjectToJson(log);
                    AddLog(new AddLog() {adcd=info.adcd,UserName = userName ,Operation = operation ,OperationType = GrassrootsFloodCtrlEnums.OperationTypeEnums.登陆});

                    return info;
                }
                else
                    return null;
            }
        }
        /// <summary>
        /// 根据条件获取单个用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public UserInfo GetUser(GetUser request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(request.UserName) && request.userID == null)
                    throw new Exception("至少输入一个条件");

                var builder = db.From<UserInfo>();
                if (!string.IsNullOrEmpty(request.UserName))
                    builder.Where(x => x.UserName == request.UserName);
                if (request.userID != null)
                    builder.Where(x => x.Id == request.userID);
                var info = db.Single(builder);
                if (info != null)
                    info.PassWord = DESHelper.DESDecrypt(info.PassWord);
                return info;
            }

        }

        /// <summary>
        /// 根据用户名称获取登录次数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int  GetUserLoginNum(GetUserLoginNum request)
        {
            using (var db=DbFactory.Open())
            {
                if (string.IsNullOrEmpty(request.name))
                    throw new Exception("用户名为空");

                var builder = db.From<UserInfo>();
                if (!string.IsNullOrEmpty(request.name))
                    builder.Where(x => x.UserName == request.name);
               
                var info = db.Single(builder);

                return info != null?info.loginNum:0;
            }
        }

        /// <summary>
        /// 根据用户名送短信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool SendMessgae(SendMessgae request)
        {
            using (var db=DbFactory.Open())
            {
                if (string.IsNullOrEmpty(request.name))
                    throw new Exception("用户名不能为空");

                var builder = db.From<UserInfo>();
                if (!string.IsNullOrEmpty(request.name))
                    builder.Where(x => x.UserName == request.name);
                var checkCode=HostContext.AppHost.Resolve<ICacheClient>().Get<string>("checkCode");
                if(!string.IsNullOrEmpty(checkCode))
                    HostContext.AppHost.Resolve<ICacheClient>().Remove("checkCode");
                var info = db.Single(builder);
                if (info != null)
                {
                    if (!ValidatorHelper.IsMobile(info.UserName)&&string.IsNullOrEmpty(info.Mobile))//|| ValidatorHelper.IsMobile(info.Mobile)
                        throw new Exception("该用户没有手机号码");
                    Random rd = new Random();
                    int num = rd.Next(100000, 1000000);
                    
                    var content = "您正在登陆《浙江省基层防汛防台体系信息管理系统》，为防止他人登陆系统篡改信息千万不要告诉他人验证码 "+num.ToString()+"[五分钟内有效]。如不是您自己操作，请忽略。";
                    var msg=SmsSend.SendSMS(info.UserName, content);
                    if (msg != "" && msg.IndexOf("-") > 0)
                    {
                        var message=new SaveSmsMessage();
                        message.adcd = info.adcd;
                        message.Content = content;
                        message.Mobile = info.UserName;
                        message.UserName = info.UserName;
                        message.name = info.RealName;
                        message.tm=DateTime.Now;
                        MessageManager.SaveMessage(message);

                        HostContext.AppHost.Resolve<ICacheClient>().Add("checkCode", num.ToString());
                        return true;
                    }
                    else return false;
                }
                else
                    throw new Exception("用户名不存在");

            }
        }

        /// <summary>
        /// 根据条件获取单个用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public UserInfoViewModel GetUserByIdOrUserName(GetUserByIdOrUserName request)
        {
            using (var db = DbFactory.Open())
            {
                if (string.IsNullOrEmpty(request.UserName) && request.userID == null)
                    throw new Exception("至少输入一个条件");

                var builder = db.From<UserInfo>();
                builder.LeftJoin<UserInfo, UserRoleInfo>((x,y)=>x.Id==y.UserID);
                if (!string.IsNullOrEmpty(request.UserName))
                    builder.Where(x => x.UserName == request.UserName);
                if (request.userID != null)
                    builder.Where(x => x.Id == request.userID);
                builder.Select("UserInfo.*,UserRoleInfo.RoleID");
                var info= db.Single<UserInfoViewModel>(builder);
                if (info != null)
                {
                    info.PassWord = DESHelper.DESDecrypt(info.PassWord);
                    info.Country = db.Single<ADCDInfo>(x => x.adcd == info.adcd.Substring(0, 4) + "00000000000").adnm;
                    info.City = db.Single<ADCDInfo>(x => x.adcd==info.adcd.Substring(0, 6)+"000000000").adnm;
                }
                return info;
            }

        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DelUser(string ids)
        {
            using (var db = DbFactory.Open())
            {
                ArrayList arr = new ArrayList();
                string[] arrs = ids.Split(',');
                for (int i = 0; i < arrs.Length; i++)
                {
                    var id = int.Parse(arrs[i]);
                    arr.Add(id);
                }
                return db.Delete<UserInfo>(x => Sql.In(x.Id, arr)) > 0;
            }
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool changePassword(changePassword request)
        {
            using (var db=DbFactory.Open())
            {
                var info = db.Single<UserInfo>(x => x.Id == request.Id);
                if (info == null)
                    throw new Exception("用户不存在");
                var model=Login(info.UserName, request.oldPassword);
                if(model==null)
                    throw new Exception("原密码输入错误.");
               return db.UpdateOnly(new UserInfo { PassWord =DESHelper.DESEncrypt(request.newPassword) }, 
                   onlyFields:x=>x.PassWord,
                   where:x=>x.Id==info.Id)==1;
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool forgetPassword(forgetPassword request)
        {
            using (var db=DbFactory.Open())
            {
                var info = db.Single<UserInfo>(x => x.UserName == request.UserName);
                if (info == null)
                    throw new Exception("用户不存在");
               
                var success= db.UpdateOnly(new UserInfo { PassWord = DESHelper.DESEncrypt(request.Password) },
                    onlyFields: x => x.PassWord,
                    where: x => x.Id == info.Id) == 1;
                if (success)
                {
                    var logInfo = new LogInfo();
                    logInfo.UserName = request.UserName;
                    logInfo.tm = DateTime.Now;
                    logInfo.adcd = info.adcd;
                    var log=new operateLog();
                    log.operateMsg = "重置密码成功";
                    log.operateTime= DateTime.Now;
                    log.userName = request.UserName;

                    logInfo.Operation = JsonTools.ObjectToJson(log);
                    logInfo.RealName = info.RealName;
                    logInfo.OperationType = GrassrootsFloodCtrlEnums.OperationTypeEnums.更新;
                    db.Insert(logInfo);

                }
                return success;
            }
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<Role> GetRolesList(GetRolesList request)
        {
            using (var db = DbFactory.Open())
            {
                var builder = db.From<Role>();
                if (!string.IsNullOrEmpty(request.roleName))
                    builder.Where(x => x.RoleName.Contains(request.roleName));
                if (request.Id!=null)
                    builder.Where(x => x.Id==request.Id.Value);

                if (!string.IsNullOrEmpty(request.Sort))
                {
                    if (request.Order == "desc")
                        builder.OrderByFieldsDescending(request.Sort);
                    else
                        builder.OrderByFields(request.Sort);
                }
                else
                    builder.OrderBy(x => x.Id);
                var count = db.Count(builder);
                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : request.PageIndex * PageSize;
                builder.Limit(PageIndex, PageSize);
                var list = db.Select<Role>(builder);
                
                return new BsTableDataSource<Role>() { total = count, rows = list };
            }
        }

        /// <summary>
        /// 新增日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool AddLog(AddLog request)
        {
            using (var db=DbFactory.Open())
            {
                var info=new LogInfo();
                info.UserName = request.UserName;
                info.tm = DateTime.Now;
                info.adcd = !string.IsNullOrEmpty(request.adcd)? request.adcd:adcd;
                info.Operation = request.Operation;
                info.RealName = GetUserByIdOrUserName(new GetUserByIdOrUserName(){ UserName=request.UserName}).RealName;
                info.OperationType = request.OperationType;
                if (request.Id != null)
                {
                    info.Id = request.Id.Value;
                   return db.Update(info)==1;
                }else
                    return db.Insert(info) == 1;
            }
        }

        /// <summary>
        /// 获取系统日志列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BsTableDataSource<LogInfo> getLogList(getLogList request)
        {
            using (var db=DbFactory.Open())
            {
                var builder = db.From<LogInfo>();
                if (adcd.IndexOf("0000000000000") > 0) //省级用户
                {
                }else if (adcd.IndexOf("00000000000") > 0) //市级用户
                    builder.Where(x => x.adcd.StartsWith(adcd.Substring(0,4)));
                else if (adcd.IndexOf("000000000") > 0) //县级用户
                    builder.Where(x => x.adcd.StartsWith(adcd.Substring(0, 6)));
                else if (adcd.IndexOf("000000") > 0) //乡镇用户
                    builder.Where(x => x.adcd.StartsWith(adcd.Substring(0, 9)));
                else
                    builder.Where(x => x.UserName == UserName);

                if (!string.IsNullOrEmpty(request.OperationType.ToString())&& request.OperationType.ToString()!="None")
                    builder.And(x => x.OperationType == request.OperationType);
                if (request.startTime!=null)
                    builder.And(x => x.tm >= request.startTime.Value);
                if (request.stopTime != null)
                    builder.And(x => x.tm <= request.stopTime.Value.AddDays(1));
                if (!string.IsNullOrEmpty(request.Sort))
                {
                    if (request.Order == "desc")
                        builder.OrderByFieldsDescending(request.Sort);
                    else
                        builder.OrderByFields(request.Sort);
                }
                else
                    builder.OrderByDescending(x => x.tm);
                var count = db.Count(builder);
                var PageSize = request.PageSize == 0 ? 15 : request.PageSize;
                var PageIndex = request.PageIndex == 0 ? 0 : (request.PageIndex) * PageSize;
                builder.Limit(PageIndex, PageSize);
               
                var list = db.Select(builder);
               
                return new BsTableDataSource<LogInfo>() { total = count, rows = list };
            }
        }
    }
}
