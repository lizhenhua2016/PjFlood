using Dy.Common;
using Funq;
using GrassrootsFloodCtrl.App_Start;
using GrassrootsFloodCtrl.Logic;
using GrassrootsFloodCtrl.Logic.Adcd;
using GrassrootsFloodCtrl.Logic.AppApi;
using GrassrootsFloodCtrl.Logic.AppReport;
using GrassrootsFloodCtrl.Logic.Audit;
using GrassrootsFloodCtrl.Logic.CApp;
using GrassrootsFloodCtrl.Logic.Common;
using GrassrootsFloodCtrl.Logic.Country;
using GrassrootsFloodCtrl.Logic.DataShare;
using GrassrootsFloodCtrl.Logic.Excel;
using GrassrootsFloodCtrl.Logic.Grid;
using GrassrootsFloodCtrl.Logic.Log;
using GrassrootsFloodCtrl.Logic.Message;
using GrassrootsFloodCtrl.Logic.NoAuthticationTown;
using GrassrootsFloodCtrl.Logic.NoAuthVillageGrid;
using GrassrootsFloodCtrl.Logic.NoVerify;
using GrassrootsFloodCtrl.Logic.Org;
using GrassrootsFloodCtrl.Logic.Position;
using GrassrootsFloodCtrl.Logic.Post;
using GrassrootsFloodCtrl.Logic.QRCode;
using GrassrootsFloodCtrl.Logic.Schema;
using GrassrootsFloodCtrl.Logic.StatisAnalysis;
using GrassrootsFloodCtrl.Logic.SumAppMessage;
using GrassrootsFloodCtrl.Logic.SumAppUser;
using GrassrootsFloodCtrl.Logic.Supervise;
using GrassrootsFloodCtrl.Logic.Sys;
using GrassrootsFloodCtrl.Logic.Town;
using GrassrootsFloodCtrl.Logic.Village;
using GrassrootsFloodCtrl.Logic.Village.VillageGrid;
using GrassrootsFloodCtrl.Logic.Village.WorkingGroup;
using GrassrootsFloodCtrl.Logic.ZZTX;
using GrassrootsFloodCtrl.Model.AppApi;
using GrassrootsFloodCtrl.Model.Audit;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.CountryPerson;
using GrassrootsFloodCtrl.Model.DangerZone;
using GrassrootsFloodCtrl.Model.DataShare;
using GrassrootsFloodCtrl.Model.Grid;
using GrassrootsFloodCtrl.Model.Messgae;
using GrassrootsFloodCtrl.Model.Org;
using GrassrootsFloodCtrl.Model.Post;
using GrassrootsFloodCtrl.Model.Supervise;
using GrassrootsFloodCtrl.Model.Sys;
using GrassrootsFloodCtrl.Model.Town;
using GrassrootsFloodCtrl.Model.Village;
using GrassrootsFloodCtrl.Model.ZZTX;
using GrassrootsFloodCtrl.ServiceInterface;
using Microsoft.SqlServer.Types;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.Logging.Log4Net;
using ServiceStack.Mvc;
using ServiceStack.OrmLite;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace GrassrootsFloodCtrl
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// </summary>
        public AppHost()
            : base("GrassrootsFloodCtrl", typeof(SysService).Assembly) { }

        /// <summary>
        /// 配置环境、文件上传目录、session过期时间等等
        /// </summary>
        public static AppConfig AppConfig { get; private set; }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        public override void Configure(Container container)
        {
            try
            {
                AppConfig = new AppConfig(new AppSettings());
                container.Register(AppConfig);
                SetConfig(new HostConfig
                {
                    HandlerFactoryPath = "api",
                    GlobalResponseHeaders = {
                      { "Access-Control-Allow-Origin", "*" },
                      { "Access-Control-Allow-Methods", "GET, POST, OPTIONS" },
                      { "Access-Control-Allow-Headers", "Content-Type" },
                    },
                });

                //Swagger 插件
                var swaggerEnable = ConfigUtils.GetAppSetting("SwaggerEnable");
                if (!string.IsNullOrEmpty(swaggerEnable) && dyConverter.ToBoolean(swaggerEnable))
                    Plugins.Add(new SwaggerFeature() { });

                Plugins.Add(new AuthFeature(() => new UserSession(),
                   new IAuthProvider[] {
                    new CustomCredentialsAuthProvider(), //HTML Form post of UserName/Password credentials
                   }));
                Plugins.Add(new RegistrationFeature());
                //添加跨域访问
                Plugins.Add(new CorsFeature(allowedMethods: "GET, POST"));

                LogManager.LogFactory = new Log4NetFactory(configureLog4Net: true);

                ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;   //使用camelCase输出json字符串。

                //添加postgisGeometry置换器，可以直接读取空间字段
                SqlServerDialect.Provider.RegisterConverter<SqlGeometry>(new SqlGeometryConverter());
                var connectionString = ConfigurationManager.ConnectionStrings["GrassrootsFloodCtrl"].ConnectionString;
                var connFactory = new OrmLiteConnectionFactory(connectionString, SqlServerDialect.Provider);
                container.Register<IDbConnectionFactory>(c => connFactory);

                container.Register<ICacheClient>(new MemoryCacheClient());
                container.Register<ISessionFactory>(c => new SessionFactory(c.Resolve<ICacheClient>()));
                container.Register<IUserAuthRepository>(new OrmLiteAuthRepository(connFactory)
                {
                    UseDistinctRoleTables = true
                });

                //Config examples
                //this.Plugins.Add(new PostmanFeature());
                //this.Plugins.Add(new CorsFeature());

                //注册服务
                container.RegisterAs<SysManager, ISysManager>();
                container.RegisterAs<AppReportManage, AppReportManage>();
                container.RegisterAs<SchemaManager, ISchemaManager>();
                container.RegisterAs<ZZTXManager, IZZTXManager>();
                container.RegisterAs<VillageWorkingGroupManage, IVillageWorkingGroupManage>();
                container.RegisterAs<VillageTransferPersonManager, IVillageTransferPersonManager>();
                container.RegisterAs<PostManager, IPostManager>();
                container.RegisterAs<VillagePicManager, IVillagePicManager>();
                container.RegisterAs<TownManager, ITownManager>();
                container.RegisterAs<NoAuthticationTownManager, NoAuthticationITownManager>();
                container.RegisterAs<GridManager, IGridManager>();
                container.RegisterAs<VillageGridManage, IVillageGridManage>();
                container.RegisterAs<ExcelManager, IExcelManager>();
                container.RegisterAs<DangerZoneManager, IDangerZoneManager>();
                container.RegisterAs<ColumnManager, IColumnManager>();
                container.RegisterAs<CountryPersonManager, ICountryPersonManager>();
                container.RegisterAs<AuditManager, IAuditManager>();
                container.RegisterAs<Logic.NoAuthticationForAudit.NoAuthticationAuditManager, Logic.NoAuthticationForAudit.NoAuthticationIAuditManager>();
                container.RegisterAs<MessageManager, IMessageManager>();
                container.RegisterAs<Logic.Common.LogHelper, ILogHelper>();
                container.RegisterAs<PositionManager, IPositionManager>();
                container.RegisterAs<SuperviseManager, ISuperviseManager>();
                container.RegisterAs<CAppManager, ICAppManager>();
                container.RegisterAs<StatisAnalysisManager, IStatisAnalysisManager>();
                container.RegisterAs<QRCodeManager, IQRCodeManager>();
                container.RegisterAs<LogMyManager, ILogMyManager>();
                container.RegisterAs<AppApiManager, IAppApiManager>();
                container.RegisterAs<DataShareManager, IDataShareManager>();
                container.RegisterAs<NoVerifyVillageWorkingGroupManange, INoVerifyVillageWorkingGroupManange>();
                container.RegisterAs<NoVerifyVillageGridManage, INoVerifyVillageGridManage>();
                container.RegisterAs<NoVerifyVillageTransferPersonManager, INoVerifyVillageTransferPersonManager>();
                container.RegisterAs<NoVerifyVillagePicManager, INoVerifyVillagePicManager>();
                container.RegisterAs<NoVerifySuperviseManage,INoVerifySuperviseManage>();
                container.RegisterAs<NoVerifyExcelManager,INoVerifyExcelManager>();
                container.RegisterAs<NoVerifyTownManage, INoVerifyTownManage>();
                container.RegisterAs<NoAuthVillageGridManage,INoAuthVillageGridManage> ();
                container.RegisterAs<NoVerifyZZTXManager, INoVerifyZZTXManager>();
                container.RegisterAs<NoVerifyCAppManager, INoVerifyCAppManager>();
                container.RegisterAs<AdcdLogic, IAdcdLogic>();
                container.RegisterAs<NoVerifyCountryPersonManager, INoVerifyCountryPersonManager>();
                container.RegisterAs<SumAppMessageLogic, ISumAppMessageLogic>();
                container.RegisterAs<SunAppUserLogic, ISunAppUserLogic> ();
                //Set MVC to use the same Funq IOC as ServiceStack
                ControllerBuilder.Current.SetControllerFactory(new FunqControllerFactory(container));
                InitTables(container); 
            }
            catch (Exception ex)
            {
                throw new Exception("系统异常:" + ex.Message);
            }
        }

        private void InitTables(Container container)
        {
            var connFactory = container.Resolve<IDbConnectionFactory>();

            var schemaManager = container.Resolve<ISchemaManager>();

            using (var db = connFactory.Open())
            {
                //创建和用户表
                db.CreateTableIfNotExists<UserInfo>();
                //重新创建用户相关表
                //var authRepo = (OrmLiteAuthRepository)container.Resolve<IUserAuthRepository>();
                //authRepo.InitSchema();   //Create only the missing tables
                //authRepo.InitApiKeySchema();

                //创建表
                db.CreateTableIfNotExists<LogInfo>();//系统日志表
                db.CreateTableIfNotExists<UserInfo>();//用户信息表
                db.CreateTableIfNotExists<Role>();//角色信息表
                db.CreateTableIfNotExists<UserRoleInfo>();//用户角色关系表
                db.CreateTableIfNotExists<ADCDInfo>();//行政区划信息表
                db.CreateTableIfNotExists<ADCDDisasterInfo>();//受灾害影响的行政区划信息表
                db.CreateTableIfNotExists<VillageWorkingGroup>();//村防汛防台工作组
                db.CreateTableIfNotExists<VillageGridPersonLiable>();//村网格责任人
                db.CreateTableIfNotExists<VillageTransferPerson>();//村危险区转移人员
                db.CreateTableIfNotExists<Post>();//岗位管理
                db.CreateTableIfNotExists<TownPersonLiable>();//镇级防汛防台责任人
                db.CreateTableIfNotExists<VillagePic>();//村防汛防台形势图
                db.CreateTableIfNotExists<Grid>();
                db.CreateTableIfNotExists<DangerZone>();//危险区转移人员清单
                db.CreateTableIfNotExists<Column>();//栏目管理
                db.CreateTableIfNotExists<RoleDetail>();//角色权限
                db.CreateTableIfNotExists<CountryPerson>();//县级人员
                db.CreateTableIfNotExists<Audit>();//县市审核
                db.CreateTableIfNotExists<AuditCounty>();//县市审核
                db.CreateTableIfNotExists<AuditDetails>();
                db.CreateTableIfNotExists<AuditCountyDetails>();
                db.CreateTableIfNotExists<SmsMessage>();//短信
                db.CreateTableIfNotExists<Model.Position.Position>();
                db.CreateTableIfNotExists<SpotCheck>();
                db.CreateTableIfNotExists<ADCDQRCode>();//二维码
                db.CreateTableIfNotExists<AppLoginVCode>();//
                db.CreateTableIfNotExists<AppRecord>();//
                db.CreateTableIfNotExists<DatashareUser>();//数据共享
                //加字段
                schemaManager.AddColumnIfNotExist(typeof(ADCDDisasterInfo), "Year");//受灾害影响的行政区划信息表---年度字段
                schemaManager.AddColumnIfNotExist(typeof(ADCDDisasterInfo), "operateLog");
                schemaManager.AddColumnIfNotExist(typeof(ADCDDisasterInfo), "CreateTime");
                schemaManager.AddColumnIfNotExist(typeof(ADCDInfo), "operateLog");
                schemaManager.AddColumnIfNotExist(typeof(ADCDInfo), "CreateTime");
                schemaManager.AddColumnIfNotExist(typeof(VillageWorkingGroup), "EditTime");
                schemaManager.AddColumnIfNotExist(typeof(VillageTransferPerson), "Remark");//创建管理员角色及账号
                schemaManager.AddColumnIfNotExist(typeof(VillageGridPersonLiable), "EditTime");
                schemaManager.AddColumnIfNotExist(typeof(VillageGridPersonLiable), "VillageGridName");
                schemaManager.AddColumnIfNotExist(typeof(VillageGridPersonLiable), "GridName");
                schemaManager.AddColumnIfNotExist(typeof(UserInfo), "loginNum");
                schemaManager.AddColumnIfNotExist(typeof(UserInfo), "UserRealName");
                schemaManager.AddColumnIfNotExist(typeof(UserInfo), "Unit");
                schemaManager.AddColumnIfNotExist(typeof(UserInfo), "Position");
                schemaManager.AddColumnIfNotExist(typeof(Model.Audit.Audit), "AuditNums");
                schemaManager.AddColumnIfNotExist(typeof(AuditDetails), "AuditNums");
                schemaManager.AddColumnIfNotExist(typeof(VillageGridPersonLiable), "AuditNums");
                schemaManager.AddColumnIfNotExist(typeof(VillageGridPersonLiable), "OldData");
                schemaManager.AddColumnIfNotExist(typeof(VillageGridPersonLiable), "NewData");
                schemaManager.AddColumnIfNotExist(typeof(VillagePic), "AuditNums");
                schemaManager.AddColumnIfNotExist(typeof(VillagePic), "OldData");
                schemaManager.AddColumnIfNotExist(typeof(VillagePic), "NewData");
                schemaManager.AddColumnIfNotExist(typeof(VillageTransferPerson), "AuditNums");
                schemaManager.AddColumnIfNotExist(typeof(VillageTransferPerson), "OldData");
                schemaManager.AddColumnIfNotExist(typeof(VillageTransferPerson), "NewData");
                schemaManager.AddColumnIfNotExist(typeof(VillageTransferPerson), "IfTransfer");
                schemaManager.AddColumnIfNotExist(typeof(VillageWorkingGroup), "AuditNums");
                schemaManager.AddColumnIfNotExist(typeof(VillageWorkingGroup), "OldData");
                schemaManager.AddColumnIfNotExist(typeof(VillageWorkingGroup), "NewData");
                schemaManager.AddColumnIfNotExist(typeof(ADCDDisasterInfo), "FXFTRW");
                schemaManager.AddColumnIfNotExist(typeof(CountryPerson), "adcd");
                schemaManager.AddColumnIfNotExist(typeof(CountryPerson), "OldData");
                schemaManager.AddColumnIfNotExist(typeof(CountryPerson), "NewData");
                schemaManager.AddColumnIfNotExist(typeof(CountryPerson), "AuditNums");
                schemaManager.AddColumnIfNotExist(typeof(TownPersonLiable), "OldData");
                schemaManager.AddColumnIfNotExist(typeof(TownPersonLiable), "NewData");
                schemaManager.AddColumnIfNotExist(typeof(TownPersonLiable), "AuditNums");
                schemaManager.AddColumnIfNotExist(typeof(AuditCounty), "CityAuditTime");
                schemaManager.AddColumnIfNotExist(typeof(AppRecord), "adcd");
                schemaManager.AddColumnIfNotExist(typeof(AppRecord), "token");
                schemaManager.AddColumnIfNotExist(typeof(AppRecord), "postTime");
                schemaManager.AddColumnIfNotExist(typeof(AppRecord), "stepItem");
                schemaManager.AddColumnIfNotExist(typeof(AppRecord), "valuesItem");

                var role = db.Single<Role>(x => x.RoleName == "系统管理员");
                if (role == null)
                    db.Insert(new Role()
                    {
                        RoleName = "系统管理员"
                    });

                var info = db.Single<UserInfo>(x => x.UserName == "admin");
                if (info == null)
                {
                    var id = (int)db.Insert(new UserInfo()
                    {
                        UserName = "admin",
                        PassWord = DESHelper.DESEncrypt("abc123"),
                        RealName = "系统管理员",
                        adcd = "330000000000000",
                        isEnable = true
                    }, true);
                    var roleID = db.Single<Role>(x => x.RoleName == "系统管理员").Id;
                    db.Insert(new UserRoleInfo()
                    {
                        UserID = id,
                        RoleID = roleID
                    });
                }
                //
            }
        }
    }
}