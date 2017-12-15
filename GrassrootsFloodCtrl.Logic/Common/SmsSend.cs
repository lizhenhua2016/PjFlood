using System;
using OpenMas;
using System.Configuration;
using ImApiDotNet;
using System.Web.Services.Protocols;

namespace GrassrootsFloodCtrl.Logic.Common
{
    public class SmsSend
    {
        public static string tbServiceURL = ConfigurationManager.AppSettings["tbServiceURL"].ToString();
        public static string GetServiceURL = ConfigurationManager.AppSettings["GetServiceURL"].ToString();



        private ImApiDotNet.APIClient apiclient = new APIClient();
        private static String[] retunvalues = new String[] { "初始化成功", "连接数据库出错", "数据库关闭失败", "数据库插入错误", "数据库删除错误", "数据库查询错误", "参数错误", "API标识非法", "消息内容太长", "没有初始化或初始化失败", "API接口处于暂停（失效）状态", "短信网关未连接" };
        private static String[] initvalues = new String[] { "成功", "连接失败", "用户名或密码错误", "密码错误", "接口编码不存在" };

        #region 发送短信
        public static string SendSMS(string tbMobileList, string tbContent)
        {
            //string tbServiceURL = "http://127.0.0.1:9080/OpenMasService";
            //string tbContent = "";      //发送的短信内容
            //////
            string tbExtendCode = "16";   //扩展号
            string tbApplicationID = "jcfxft";//应用账号
            string tbPassword = "VhPw|YCDc27e";     //应用账号对应的密码
            string tbSendTime = "";     //发送时间

            string OpenMasUrl = tbServiceURL;
            Sms client = new Sms(OpenMasUrl);

            //获取页面的信息
            string[] MobileList = tbMobileList.Trim().Split(',');   //需要发送的手机列表,以","分割。
            string Content = tbContent;                        //发送的短信内容
            string ExtendCode = tbExtendCode;                  //扩展号
            string ApplicationID = tbApplicationID;            //应用账号
            string Password = tbPassword;                      //应用账号对应的密码
            string SendTime = tbSendTime;                      //普通短信,如果SendTime为空则立即发送，否则为定时发送

            string MessageID = "";

            #region 发送短信
            try
            {
                //普通短信,如果SendTime为空则立即发送，否则为定时发送
                if (string.IsNullOrEmpty(SendTime))
                    MessageID = client.SendMessage(MobileList, Content, ExtendCode, ApplicationID, Password);
                else
                    MessageID = client.SendMessage(MobileList, Content, ExtendCode, ApplicationID, Password, DateTime.Parse(SendTime));
                return MessageID;
                //this.Label8.Text = "提交成功，返回的MessageID为" + MessageID;
            #endregion
            }
            catch (Exception ex)
            {
                //this.Label8.Text = "提交失败，失败原因为" + ex.Message.ToString();
                return "0";
            }
        }
        #endregion
        [SoapDocumentMethodAttribute("urn:NotifySms", RequestNamespace = "http://openmas.chinamobile.com/pulgin", OneWay = true, Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public static SmsMessage NotifySms(string messageId)
        {
            try
            {
                //调用上行短信获取接口获取短消息
                string _SmsServiceUrl = GetServiceURL;
                OpenMas.Sms _Sms = new OpenMas.Sms(_SmsServiceUrl);
                SmsMessage message = _Sms.GetMessage(messageId);
                return message;
                //业务逻辑，短信内容可以从message中获取
                //......
            }
            catch (Exception ex)
            {
                //处理异常信息
                //.......
                return null;
            }
        }



        public string InitData()
        {
            string textIp = "";  //mas服务器IP
            string textUsername = "";       //接口创建时的名称
            string textPassword = "";       //接口创建时的密码1234qwer!
            string textCode = "";            //接口API编码名称
            string textDB = "mas";              //mas数据库名称。
            //if (apiclient == null)
            //{
            //    //InitData();
            //}
            //ImApiDotNet.APIClient apiclient = new ImApiDotNet.APIClient();

            int con;
            try
            {
                con = apiclient.init(textIp.Trim(), textUsername.Trim(), textPassword.Trim(), textCode.Trim(), textDB.Trim());
                //con = apiclient.init(textIp.Trim(), textUsername.Trim(), textPassword.Trim(), textCode.Trim());
                con = System.Math.Abs(con);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return initvalues[con];
        }


        public int sendSMS(string tel, string content)
        {
            // InitData();
            int mes = 1;
            long smID = 10;
            string[] MobileList = tel.Trim().Split(',');   //需要发送的手机列表,以","分割。
            mes = apiclient.sendSM(MobileList, content, smID);
            return mes;
        }


        /// <summary>
        /// 切换电信(联通)和移动使用
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        //public string smsALL(string tel, string content)
        //{
            //string phonePrefix = ConfigurationManager.AppSettings["phonePrefix"].ToString();//获取电信和联通的号段

            //Database db = DatabaseManager.CreateDatabase("DXLTMsgConnectionString", false);//电信联通短信数据库
            //string temp, result = "";
            //string[] tels = tel.Split(',');
            //string mobileList = "";
            //foreach (string te in tels)
            //{
            //    temp = te.Trim().Substring(0, 3);
            //    if (phonePrefix.IndexOf(temp) >= 0)//电信或联通
            //    {
            //        //电信或联通
            //        int MsgType = 7;//消息类型，1取消订阅2订阅请求3点播4订阅5交互式操作6查询，其他保留（默认“7”）
            //        int lenContent = content.Length;
            //        string strSql = "INSERT INTO SMSend_SXXSLJ(DestTermID,MsgLen,MsgContent,MsgType) VALUES ('" + te + "'," + lenContent + ",'" + content + "'," + MsgType + ")";
            //        bool success = db.ExecuteNonQuery(strSql);
            //        result = success ? success.ToString() : "0";
            //    }
            //    else//移动
            //    {
            //        //mobileList = te + ",";
            //        result = SendSMS(te, content);
            //    }
        //    }

        //    return result;
        //}
    }
}