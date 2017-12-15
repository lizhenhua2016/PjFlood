using GrassrootsFloodCtrl.Logic.Schema;
using GrassrootsFloodCtrl.ViewModels;
using ServiceStack;
using ServiceStack.Auth;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ServiceStack.Mvc;
using Dy.Common;
using GrassrootsFloodCtrl.Model.Common;
using GrassrootsFloodCtrl.Model.Enums;
using GrassrootsFloodCtrl.ServiceInterface;
using GrassrootsFloodCtrl.ServiceModel.Route;
using ServiceStack.Caching;

namespace GrassrootsFloodCtrl.Controllers
{
    public class AccountController : ServiceStackController
    {
        public ISchemaManager SchemaManager { get; set; }

        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string msg="")
        {
            ViewData["msg"] = msg;
            return View();
        }

        //public ActionResult png()
        //{
        //     //public void ShowAuthCode(Stream stream, out string code)
       
        //    //Random random = new Random();
        //    //code = random.Next(1000, 9999).ToString();

        //    //Bitmap bitmap = CreateAuthCode(code);
        //    //bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Gif);

        //    var iCount = 4;
        //    var number=0;
        //    var checkCode = String.Empty;
        //    var iSeed = DateTime.Now.Millisecond;
        //    var random = new Random(iSeed);
        //    for (int i = 0; i < iCount; i++)
        //    {
        //        number = random.Next(10);
        //        checkCode += number.ToString();
        //    }
        //    Session["CheckCode"] = checkCode;
        //    if (checkCode == null || checkCode.Trim() == String.Empty)
        //        return View();
        //    int iWordWidth = 15;
        //    int iImageWidth = checkCode.Length * iWordWidth;
        //    Bitmap image = new Bitmap(iImageWidth, 20);
        //    Graphics g = Graphics.FromImage(image);
        //    try
        //    {
        //        //生成随机生成器 
        //        random = new Random();
        //        //清空图片背景色 
        //        g.Clear(Color.White);

        //        //画图片的背景噪音点
        //        for (int i = 0; i < 20; i++)
        //        {
        //            int x1 = random.Next(image.Width);
        //            int x2 = random.Next(image.Width);
        //            int y1 = random.Next(image.Height);
        //            int y2 = random.Next(image.Height);
        //            g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
        //        }

        //        //画图片的背景噪音线 
        //        for (int i = 0; i < 2; i++)
        //        {
        //            int x1 = 0;
        //            int x2 = image.Width;
        //            int y1 = random.Next(image.Height);
        //            int y2 = random.Next(image.Height);
        //            if (i == 0)
        //            {
        //                g.DrawLine(new Pen(Color.Gray, 2), x1, y1, x2, y2);
        //            }
        //        }
        //        for (int i = 0; i < checkCode.Length; i++)
        //        {

        //            string Code = checkCode[i].ToString();
        //            int xLeft = iWordWidth * (i);
        //            random = new Random(xLeft);
        //            iSeed = DateTime.Now.Millisecond;
        //            int iValue = random.Next(iSeed) % 4;
        //            if (iValue == 0)
        //            {
        //                Font font = new Font("Arial", 13, (FontStyle.Bold | System.Drawing.FontStyle.Italic));
        //                Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
        //                LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Blue, Color.Red, 1.5f, true);
        //                g.DrawString(Code, font, brush, xLeft, 2);
        //            }
        //            else if (iValue == 1)
        //            {
        //                Font font = new System.Drawing.Font("楷体", 13, (FontStyle.Bold));
        //                Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
        //                LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Blue, Color.DarkRed, 1.3f, true);
        //                g.DrawString(Code, font, brush, xLeft, 2);
        //            }
        //            else if (iValue == 2)
        //            {
        //                Font font = new System.Drawing.Font("宋体", 13, (System.Drawing.FontStyle.Bold));
        //                Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
        //                LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Green, Color.Blue, 1.2f, true);
        //                g.DrawString(Code, font, brush, xLeft, 2);
        //            }
        //            else if (iValue == 3)
        //            {
        //                Font font = new System.Drawing.Font("黑体", 13, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Bold));
        //                Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
        //                LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Blue, Color.Green, 1.8f, true);
        //                g.DrawString(Code, font, brush, xLeft, 2);
        //            }
        //        }
        //        //////画图片的前景噪音点 
        //        //for (int i = 0; i < 8; i++)
        //        //{
        //        //    int x = random.Next(image.Width);
        //        //    int y = random.Next(image.Height);
        //        //    image.SetPixel(x, y, Color.FromArgb(random.Next()));
        //        //}
        //        //画图片的边框线 
        //        g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
        //        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //        image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        //        Response.ClearContent();
        //        Response.BinaryWrite(ms.ToArray());
        //    }
        //    finally
        //    {
        //        g.Dispose();
        //        image.Dispose();
        //    }



        //    //private Bitmap CreateAuthCode(string str)

        //    //Font fn = new Font("宋体", 12);
        //    //Brush forecolor = Brushes.Black;
        //    //Brush bgcolor = Brushes.White;
        //    //PointF pf = new PointF(5, 5);
        //    //Bitmap bitmap = new Bitmap(100, 25);
        //    //Rectangle rec = new Rectangle(0, 0, 100, 25);
        //    //Graphics gh = Graphics.FromImage(bitmap);
        //    //gh.FillRectangle(bgcolor, rec);
        //    //gh.DrawString(str, fn, forecolor, pf);
        //    //return bitmap;
       
        //    return View();
        //}

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var cutTM = DateTime.Now;
                    var checkCode= HostContext.AppHost.Resolve<ICacheClient>().Get<string>("checkCode");
                    var isCheck= Request.Form["isCheck"].Trim().ToLower();
                    var SendTime = Request.Form["SendTime"].Trim().ToLower();
                    var sendTime = !string.IsNullOrEmpty(SendTime) ? DateTime.Parse(SendTime) : cutTM.AddMonths(-1);
                    if (isCheck=="true" || !string.IsNullOrEmpty(checkCode))
                    {
                        var MessgaeValidTimeConfig = AppConfigUtil.getValue("MessgaeValidTime");
                        var MessgaeValidTime = !string.IsNullOrEmpty(MessgaeValidTimeConfig)
                            ? double.Parse(MessgaeValidTimeConfig) :5;
                        if (model.Code == null || model.Code != checkCode)
                        {
                            ModelState.AddModelError("", "验证失败，校验码错误。"); // +e.Message
                            HostContext.AppHost.Resolve<ICacheClient>().Remove("checkCode");
                            return View(model);
                        }
                        else if(!string.IsNullOrEmpty(model.Code)&&(cutTM - sendTime).TotalMinutes > MessgaeValidTime)
                        {
                            ModelState.AddModelError("", "验证失败，校验码无效，请重新获取校验码。"); // +e.Message
                            HostContext.AppHost.Resolve<ICacheClient>().Remove("checkCode");
                            return View(model);
                        }else { }
                    }
                    using (var authService = HostContext.ResolveService<AuthenticateService>(base.HttpContext))
                    {
                        #region 登陆
                        var response = authService.Authenticate(new Authenticate
                        {
                            UserName = model.UserName,
                            Password = model.Password,
                            RememberMe = true
                        });

                        //SchemaManager.CreateNewTableIfNotExists(typeof (LogInfo),
                        //"log_info_" + DateTime.Now.ToString("yyyyMM"), "id", true);

                        var authTicket = new FormsAuthenticationTicket(model.UserName, model.RememberMe, 120);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                            FormsAuthentication.Encrypt(authTicket));
                        cookie.HttpOnly = true;
                        cookie.Expires = model.RememberMe ? DateTime.Now.AddDays(2d) : DateTime.Now.AddMinutes(120);
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(Dy.Common.UrlHelper.Decode(returnUrl));
                        }
                        else if (model.Password != ((int)GrassrootsFloodCtrlEnums.InitialPasswordEnums.初始密码).ToString() || model.UserName == "dxz")
                        {
                            var userrole = HostContext.Resolve<SysService>().GET(new GetUserByIdOrUserName() { UserName = model.UserName });
                            if (null != userrole && userrole.RoleID == (int)GrassrootsFloodCtrlEnums.RoleEnums.县级用户)
                            {
                                return RedirectToAction("CountyIndex", "Audit");
                            }
                            else if (null != userrole && userrole.RoleID == (int)GrassrootsFloodCtrlEnums.RoleEnums.市级用户)
                            {
                                return RedirectToAction("CityIndex", "Audit");
                            }
                            else if(null != userrole && userrole.RoleID == (int)GrassrootsFloodCtrlEnums.RoleEnums.省级用户)
                            {
                                return RedirectToAction("Index", "ComprehensiveApp");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            return RedirectToAction("ModificationPsaaword", "Home");
                        }
                        #endregion
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "验证失败，用户名或密码错误。" );// +e.Message
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult LogOff(string name)
        {
            try
            {
                using (var service = HostContext.AppHost.Resolve<SysService>())
                {
                    var log = new operateLog();
                    var IP = WebUtils.Get_ClientIP();
                    log.operateMsg = "退出系统，IP地址为：" + IP;
                    log.operateTime = DateTime.Now;
                    log.userName = name;
                    var operation = JsonTools.ObjectToJson(log);
                    service.Post(new AddLog() { UserName = name, Operation = operation, OperationType = GrassrootsFloodCtrlEnums.OperationTypeEnums.退出 });

                    var cookies = Response.Cookies.Get(FormsAuthentication.FormsCookieName); 
                    if(cookies!=null)
                        Response.Cookies.Remove(FormsAuthentication.FormsCookieName);

                    //var authService = HostContext.ResolveService<AuthenticateService>();
                    //var session = (Model.Common.UserSession)authService.GetSession(false);
                
                    var yearCookie= Response.Cookies.Get("year");
                    if (yearCookie != null)
                        Response.Cookies.Remove("year");
                    HostContext.AppHost.Resolve<ICacheClient>().RemoveAll(HostContext.AppHost.Resolve<ICacheClient>().GetAllKeys());
                }
            }
            catch
            {
                return Redirect("/");
            }
            Response.SetCookie(new HttpCookie("ss-id") { Expires = DateTime.Now.AddMonths(-1) });
            Response.SetCookie(new HttpCookie("ss-opt") { Expires = DateTime.Now.AddMonths(-1) });
            Response.SetCookie(new HttpCookie("ss-pid") { Expires = DateTime.Now.AddMonths(-1) });
            Response.SetCookie(new HttpCookie("X-UAId") { Expires = DateTime.Now.AddMonths(-1) });
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult Login1(string returnUrl)
        {
            return View();
        }
        /// <summary>
        /// 登陆1
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login1()
        {
            try
            {
                var secretKey = objFunn.chkRequestValue("secretkey");
                if (string.IsNullOrEmpty(secretKey))
                {
                    ModelState.AddModelError("", "验证失败，用户名或密码错误。");
                    return View();
                }
                var model = new LoginViewModel();
                string[] StrArr = DESHelper.DESDecrypt(secretKey).Split(',');

                model.UserName = StrArr[0];
                model.Password = StrArr[1];
                //DateTime Time = Convert.ToDateTime(StrArr[2]);
                //if ((DateTime.Now - Time).TotalMinutes > 10)
                //{
                //    ModelState.AddModelError("", "登入已超时");
                //    return View();
                //}

                using (var authService = HostContext.ResolveService<AuthenticateService>(base.HttpContext))
                {
                    var response = authService.Authenticate(new Authenticate
                    {
                        UserName = model.UserName,
                        Password = model.Password,
                        RememberMe = true
                    });

                    var authTicket = new FormsAuthenticationTicket(model.UserName, model.RememberMe, 120);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                        FormsAuthentication.Encrypt(authTicket));
                    cookie.HttpOnly = true;
                    cookie.Expires = model.RememberMe ? DateTime.Now.AddDays(2d) : DateTime.Now.AddMinutes(120);
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("Index", "Home");//Redirect("/Home/Index");//
                    
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "验证失败，用户名或密码错误。" + e.Message);
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult NoLogin()
        {
            LoginViewModel model = new LoginViewModel();
            try
            {
                model.UserName = "13456949000";
                model.Password = "a123123";
                var cutTM = DateTime.Now;
                using (var authService = HostContext.ResolveService<AuthenticateService>(base.HttpContext))
                {
                    #region 登陆
                    var response = authService.Authenticate(new Authenticate
                    {
                        UserName = model.UserName,
                        Password = model.Password,
                        RememberMe = true
                    });

                    //SchemaManager.CreateNewTableIfNotExists(typeof (LogInfo),
                    //"log_info_" + DateTime.Now.ToString("yyyyMM"), "id", true);

                    var authTicket = new FormsAuthenticationTicket(model.UserName, model.RememberMe, 120);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                        FormsAuthentication.Encrypt(authTicket));
                    cookie.HttpOnly = true;
                    cookie.Expires = model.RememberMe ? DateTime.Now.AddDays(2d) : DateTime.Now.AddMinutes(120);
                    Response.Cookies.Add(cookie);

                    if (model.Password != ((int)GrassrootsFloodCtrlEnums.InitialPasswordEnums.初始密码).ToString())
                    {
                        var userrole = HostContext.Resolve<SysService>().GET(new GetUserByIdOrUserName() { UserName = model.UserName });
                        if (null != userrole && userrole.RoleID == (int)GrassrootsFloodCtrlEnums.RoleEnums.县级用户)
                        {
                            return RedirectToAction("CountyIndex", "Audit");
                        }
                        else if (null != userrole && userrole.RoleID == (int)GrassrootsFloodCtrlEnums.RoleEnums.市级用户)
                        {
                            return RedirectToAction("CityIndex", "Audit");
                        }
                        else if (null != userrole && userrole.RoleID == (int)GrassrootsFloodCtrlEnums.RoleEnums.省级用户)
                        {
                            return RedirectToAction("Index", "ComprehensiveApp");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        return RedirectToAction("ModificationPsaaword", "Home");
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "验证失败，用户名或密码错误。");// +e.Message
            }
            return View(model);
        }
        //[HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassWord()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassWord(LoginViewModel model, string returnUrl)
        {
            //return RedirectToAction("Login", "Account", new { msg = "forget" });
            var checkCode = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var cutTM = DateTime.Now;

                    checkCode = HostContext.AppHost.Resolve<ICacheClient>().Get<string>("checkCode");

                    var SendTime = Request.Form["SendTime"].Trim().ToLower();
                    var sendTime = !string.IsNullOrEmpty(SendTime) ? DateTime.Parse(SendTime) : cutTM.AddMonths(-1);
                    if (!string.IsNullOrEmpty(checkCode))
                    {
                        #region 校验码正常

                        var MessgaeValidTimeConfig = AppConfigUtil.getValue("MessgaeValidTime");
                        var MessgaeValidTime = !string.IsNullOrEmpty(MessgaeValidTimeConfig)
                            ? double.Parse(MessgaeValidTimeConfig)
                            : 5;
                        if (string.IsNullOrEmpty(model.Code) || model.Code != checkCode)
                            ModelState.AddModelError("", "验证失败，校验码错误。"); // +e.Message
                        else if (!string.IsNullOrEmpty(model.Code) && (cutTM - sendTime).TotalMinutes > MessgaeValidTime)
                            ModelState.AddModelError("", "验证失败，校验码无效，请重新获取校验码。"); // +e.Message
                        else
                        {
                            using (var service = HostContext.AppHost.Resolve<LoginService>())
                            {
                                #region 修改密码

                                if (service.Post(new forgetPassword(){UserName = model.UserName,Password = model.Password})){
                                    
                                    return RedirectToAction("Login", "Account",new { msg= "forget" });
                                }

                                #endregion
                            }
                        }

                        #endregion
                    }
                    else
                        ModelState.AddModelError("", "验证失败，校验码异常，请重新获取校验码。"); // +e.Message
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "验证失败。"); // +e.Message
                }
            }
            if(!string.IsNullOrEmpty(checkCode))
                HostContext.AppHost.Resolve<ICacheClient>().Remove("checkCode");
            return View(model);
        }
    }
}