/**
 * Created by Administrator on 2017/9/22.
 */


/****************************************
 * 函数名称：IsDate
 * 功能说明：构造函数
 * 参    数：sDate:日期字符串
 * 调用示列：
 *           string sDate="2008-10-28";
 *           IsDate(sDate);
 *****************************************/
/// <summary>  
/// 判断是否是日期  
/// </summary>  
/// <param name="sDate">日期字符串</param>  
/// <returns>返回是否(bool)</returns>  
function IsDate(sDate) {
    var sRegex = /^(\d{4})-(\d{2})-(\d{2})$/;
    var bResult = sDate.match(reg);
    if (bResult == null) {
        return false;
    }
    else {
        return true;
    }
}

/****************************************
 * 函数名称：IsNullEmpty
 * 功能说明：判断字符串是否为空
 * 参    数：str:空字符串
 * 调用示列：
 *           string str="";
 *           IsNullEmpty(str);
 *****************************************/
/// <summary>  
///  判断字符串是否为空  
/// </summary>  
/// <param name="sNullOrEmpty">空字符串</param>  
/// <returns>返回是否(bool)</returns>  
function IsNullEmpty(sNullOrEmpty) {
    if (sNullOrEmpty.length == '' || sNullOrEmpty.length <= 0) {
        return false;
    }
    else {
        return true;
    }
}

/****************************************
 * 函数名称：IsCurrent
 * 功能说明：判断是否是货币
 * 参    数：sCurrent:货币字符串
 * 调用示列：
 *           string sCurrent="88888.00";
 *           IsCurrent(sCurrent);
 *****************************************/
/// <summary>  
/// 判断是否是货币  
/// </summary>  
/// <param name="sCurrent">货币字符串</param>  
/// <returns>返回是否(bool)</returns>  
function IsCurrent(sCurrent) {
    var bResult1 = sCurrent.match("[^0-9.-]");
    var bResult2 = sCurrent.match("[[0-9]*[.][0-9]*[.][0-9]*");
    var bResult3 = sCurrent.match("[[0-9]*[-][0-9]*[-][0-9]");
    var bResult4 = sCurrent.match("(^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$)|(^([-]|[0-9])[0-9]*$)");
    if (bResult1 != null || bResult2 != null || bResult3 != null || bResult4 == null) {
        return false;
    }
    else {
        return true;
    }
}

/****************************************
 * 函数名称：IsNumeric
 * 功能说明：判断是否是数字
 * 参    数：sNum:数字字符串
 * 调用示列：
 *           string sNum="88888";
 *           IsNumeric(sNum);
 *****************************************/
/// <summary>  
/// 判断是否是数字  
/// </summary>  
/// <param name="sNum">数字字符串</param>  
/// <returns>返回是否(bool)</returns>  
function IsNumeric(sNum) {
    var bResult = sNum.match("^(-|\\+)?\\d+(\\.\\d+)?$");
    if (bResult == null) {
        return false;
    }
    else {
        return true;
    }
}

/****************************************
 * 函数名称：IsUrl
 * 功能说明：判断是否是URL
 * 参    数：sUrl:URL字符串
 * 调用示列：
 *           string sUrl="http:\\www.sina.com.cn";
 *           IsUrl(sUrl);
 *****************************************/
/// <summary>  
/// 判断是否是URL  
/// </summary>  
/// <param name="sUrl">URL字符串</param>  
/// <returns>返回是否(bool)</returns>  
function IsUrl(sUrl) {
    var bResult = sUrl.match("http(s)?://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?");
    if (bResult == null) {
        return false;
    }
    else {
        return true;
    }
}

/****************************************
 * 函数名称：IsMail
 * 功能说明：判断是否是MAILL
 * 参    数：sMail:MAIL字符串
 * 调用示列：
 *           string sMail="olivier@hdtworld.com";
 *           IsMail(sMail);
 *****************************************/
/// <summary>  
/// 判断是否是MAIL  
/// </summary>  
/// <param name="sMail">MAIL字符串</param>  
/// <returns>返回是否(bool)</returns>  
function IsMail(sMail) {
    var bResult = sMail.match("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
    if (bResult == null) {
        return false;
    }
    else {
        return true;
    }
}

/****************************************
 * 函数名称：IsPostCode
 * 功能说明：判断是否是邮编
 * 参    数：sPostCode:邮编字符串
 * 调用示列：
 *           string sPostCode="200001";
 *           IsPostCode(sPostCode);
 *****************************************/
/// <summary>  
/// 判断是否是邮编  
/// </summary>  
/// <param name="sPostCode">邮编字符串</param>  
/// <returns>返回是否(bool)</returns>  
function IsPostCode(sPostCode) {
    var bResult = sPostCode.match("^\\d{6}$");
    if (bResult == null) {
        return false;
    }
    else {
        return true;
    }
}

/****************************************
 * 函数名称：IsTelephone
 * 功能说明：判断是否是电话号码
 * 参    数：sTelephone:电话号码字符串
 * 调用示列：
 *           string sTelephone="66660000";
 *           IsTelephone(sTelephone);
 *****************************************/
/// <summary>  
/// 判断是否是电话号码  
/// </summary>  
/// <param name="sTelephone">电话号码字符串</param>  
/// <returns>返回是否(bool)</returns>  
function IsTelephone(sTelephone) {
    var bResult = sTelephone.match("^(\\(\\d{3}\\)|\\d{3}-)?\\d{8}$");
    if (bResult == null) {
        return false;
    }
    else {
        return true;
    }
}

/****************************************
 * 函数名称：IsMobile
 * 功能说明：判断是否是手机号码
 * 参    数：sMobile:手机号码字符串
 * 调用示列：
 *           string sMobile="1381101101101";
 *           IsMobile(sMobile);
 *****************************************/
/// <summary>  
/// 判断是否是手机号码  
/// </summary>  
/// <param name="sMobile">手机号码字符串</param>  
/// <returns>返回是否(bool)</returns>  
function IsMobile(sMobile) {
    var bResult = sMobile.match("^\\d{11}$");
    if (bResult == null) {
        return false;
    }
    else {
        return true;
    }
}

/****************************************
 * 函数名称：IsIDCard
 * 功能说明：判断是否身份证
 * 参    数：sIDCard:身份证字符串
 * 调用示列：
 *           string sIDCard="310106198210054xxx";
 *           IsIDCard(sIDCard);
 *****************************************/
/// <summary>  
/// 判断是否是数字  
/// </summary>  
/// <param name="sSimNum">数字字符串</param>  
/// <returns>返回是否(bool)</returns>  
function IsIDCard(sIDCard) {
    var bResult = sIDCard.match("^\\d{15}|\\d{18}$");
    if (bResult == null) {
        return false;
    }
    else {
        return true;
    }
}

/****************************************
 * 函数名称：IsCE
 * 功能说明：判断是中英表达式
 * 参    数：sCE:中英文表达式字符串
 * 调用示列：
 *           string sCE="HDT互动通";
 *           IsCE(sCE);
 *****************************************/
/// <summary>  
/// 判断是中英表达式  
/// </summary>  
/// <param name="sCE">中英文表达式字符串</param>  
/// <returns>返回是否(bool)</returns>  
function IsCE(sCE) {
    var bResult = sCE.match("^[a-zA-Z\\u4E00-\\u9FA5\\uF900-\\uFA2D]+$");
    if (bResult == null) {
        return false;
    }
    else {
        return true;
    }
}

/// <summary>  
/// 密码强度等级  
/// </summary>  
var pwdLevel;
/// <summary>  
/// 密码中是否有字母  
/// </summary>  
var hasLetter;
/// <summary>  
/// 密码中是否有大小写字母  
/// </summary>  
var hasULLetter;
/// <summary>  
/// 密码中是否有数字  
/// </summary>  
var hasNumeric;
/// <summary>  
/// 密码中是否有符号  
/// </summary>  
var hasSymbol;


/****************************************
 * 函数名称：IsPasswordLevel
 * 功能说明：判断密码强度
 * 参    数：sPassword:密码字符串
 * 调用示列：
 *           string sPassword="abc123-_";
 *           IsPasswordLevel(sPassword);
 *****************************************/
/// <summary>  
/// 判断密码强度  
/// </summary>  
/// <param name="sPassword">密码字符串</param>  
/// <returns>返回强度等级(string)</returns>  
function IsPasswordLevel(sPassword) {
    pwdLevel = 0;
    if (sPassword == "" || sPassword == null) {
        return "空密码";
    }
    else {
        //判断密码长度  
        JugePwdLength(sPassword);
        //判断字母  
        JugePwdLetter(sPassword);
        //判断数字  
        JugePwdNumeric(sPassword);
        //判断符号  
        JugeSymbol(sPassword);
        //判断奖励  
        JugeAward();
        //判断密码级别  
        //>= 90: 非常安全   
        //>= 80: 安全（Secure）   
        //>= 70: 非常强   
        //>= 60: 强（Strong）   
        //>= 50: 一般（Average）   
        //>= 25: 弱（Weak）   
        //>= 0: 非常弱   
        if (pwdLevel > 0) {
            if (pwdLevel > 25) {
                if (pwdLevel > 50) {
                    if (pwdLevel > 60) {
                        if (pwdLevel > 70) {
                            if (pwdLevel > 80) {
                                if (pwdLevel > 90) {
                                    return "非常安全";
                                }
                                else {
                                    return "安全";
                                }
                            }
                            else {
                                return "非常强";
                            }
                        }
                        else {
                            return "强";
                        }
                    }
                    else {
                        return "一般";
                    }
                }
                else {
                    return "弱";
                }
            }
            else {
                return "非常弱";
            }
        }
        return "极其弱";
    }
}

/****************************************
 * 函数名称：JugePwdlength
 * 功能说明：判断密码字符串长度
 * 参    数：str:字符串
 * 调用示列：
 *           string str="abc123-_";
 *           JugePwdlength(str);
 *****************************************/
/// <summary>  
/// 判断密码字符串长度  
/// </summary>  
/// <param name="slength">密码字符串</param>  
function JugePwdLength(sLength) {
    var length = sLength.length;
    if (length <= 4) {
        pwdLevel += 5;
    }
    else {
        if (length <= 7) {
            pwdLevel += 10;
        }
        else {
            pwdLevel += 20;
        }
    }
}

/****************************************
 * 函数名称：JugePwdLetter
 * 功能说明：判断密码强度是否有字符
 * 参    数：str:字符串
 * 调用示列：
 *           string str="abc123-_";
 *           JugePwdLetter(str);
 *****************************************/
/// <summary>  
/// 判断密码强度是否有字符  
/// </summary>  
/// <param name="sLetter">密码字符串</param>  
function JugePwdLetter(sLetter) {
    //0 分: 没有字母   
    //10 分: 全都是小（大）写字母   
    //20 分: 大小写混合字母   
    //判断是否有字母  
    var count = 0;
    var othercount = 0;
    var bLower = false, bUpper = false;
    for (var i = 0; i <= sLetter.length - 1; i++) {
        //大小写字母的KEYCODE 65-90  
        if ((sLetter.charCodeAt(i) >= 65) && (sLetter.charCodeAt(0) <= 90)) {
            count += 1;
        }
        //判断字符是否有大小写  
        if (sLetter.substr(i, 1).match("[A-Z]")) {
            bUpper = true;
        }
        //判断字符是否有大小写  
        if (sLetter.substr(i, 1).match("[a-z]")) {
            bLower = true;
        }
    }
    if (count == 0) {
        pwdLevel += 0;
    }
    else {
        hasLetter = true;
        if (bLower && bUpper) {
            pwdLevel += 20;
        }
        else {
            pwdLevel += 10;
        }
    }
    ;
}

/****************************************
 * 函数名称：JugePwdNumeric
 * 功能说明：判断密码强度是否有数字
 * 参    数：str:密码字符串
 * 调用示列：
 *           string str="abc123-_";
 *           JugePwdNumeric(str);
 *****************************************/
/// <summary>  
/// 判断密码强度是否有数字  
/// </summary>  
/// <param name="str">密码字符串</param>  
function JugePwdNumeric(sNum) {
    //三、数字:  
    //0 分: 没有数字   
    //10 分: 1 个数字   
    //20 分: 大于等于 3 个数字   
    var count = 0;


    for (var i = 0; i <= sNum.length - 1; i++) {
        //数字的KEYCODE 96-105  
        if ((sNum.charCodeAt(i) >= 96) && (sNum.charCodeAt(0) <= 105)) {
            count += 1;
        }
    }
    if (count == 0) {
        pwdLevel += 0;
    }
    else {
        hasNumeric = true;
        if (count < 3) {
            pwdLevel += 10;
        }
        else {
            pwdLevel += 20;
        }
    }
}

/****************************************
 * 函数名称：JugeAward
 * 功能说明：判断密码强度奖励
 * 参    数：
 * 调用示列：
 *           JugeAward();
 *****************************************/
/// <summary>  
/// 判断密码强度奖励  
function JugeAward() {


    //五、奖励:  
    //2 分: 字母和数字   
    //3 分: 字母、数字和符号   
    //5 分: 大小写字母、数字和符号   
    if (hasLetter && hasNumeric) {
        if (hasSymbol) {
            if (hasULLetter) {
                pwdLevel += 5;
            }
            else {
                pwdLevel += 3;
            }
        }
        else {
            pwdLevel += 2;
        }
    }
}


/****************************************
 * 函数名称：JugeAward
 * 功能说明：判断特定的符号
 * 参    数：str:密码字符串
 * 调用示列：
 *           string str="abc123-_";
 *           IsSymbol(str);
 *****************************************/
/// <summary>  
/// 判断特定的符号  
/// </summary>  
/// <param name="str">密码字符串</param>  
/// <returns>返回是否(bool)</returns>  
function IsSymbol(str) {
    var bResult = str.match("[_]|[-]|[#]");
    if (bResult == null) {
        return false;
    }
    else {
        return true;
    }
}


/****************************************
 * 函数名称：JugeSymbol
 * 功能说明：判断是密码强度否有符号
 * 参    数：str:密码字符串
 * 调用示列：
 *           string str="abc123-_";
 *           JugeSymbol(str);
 *****************************************/
/// <summary>  
/// 判断是密码强度否有符号  
/// </summary>  
/// <param name="str">密码字符串</param>  
function JugeSymbol(sSymbol) {
    //四、符号:  
    //0 分: 没有符号   
    //10 分: 1 个符号   
    //25 分: 大于 1 个符号   


    var count = 0;
    var tmpstr = "";
    for (var i = 0; i <= sSymbol.length - 1; i++) {
        tmpstr = sSymbol.substr(i, 1);
        if (IsSymbol(tmpstr)) {
            count += 1;
        }
    }
    if (count == 0) {
        pwdLevel += 0;
    }
    else {
        hasSymbol = true;
        if (count > 1) {
            pwdLevel += 25;
        }
        else {
            pwdLevel += 10;
        }
    }
}
