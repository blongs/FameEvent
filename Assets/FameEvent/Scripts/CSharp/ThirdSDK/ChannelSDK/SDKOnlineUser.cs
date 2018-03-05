using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;

public class SDKOnlineUser:ICloneable
{
    private string channelId = "";          //渠道Id
    private int channelShortId = 1;         //渠道数字型Id,此Id由服务器下发
    private string channelName = "";        //渠道名
    private string subChannel = "";         //子渠道,主要用于各个SDK渠道买量时分渠道统计

    public string deviceTypeId = "1";       //操作系统类型, 1:安卓, 2:IOS, 3:IOS越狱
    public bool haveChannelSDK = false;     //是否存在渠道sdk
    public bool useChannelSDKExit = false;  //是否使用SDK的退出

    private string loginCheckUrl = "";      //登录验证url
    private string payOrderUrl = "";        //充值订单号生成Url
    private string paySyncUrl = "";         //充值异步回调url

    //下面的字段在登录后获得
    public string  id = "";
    private string channelUserId = "";      //渠道用户Id
    private string channelUserName = "";    //渠道用户名

    private string token = "";              //玩家账号、登录验证的Token值
    private string productCode = "";

    private bool   needLoginCheck = true;   //是否需要登录验证
    private bool   needCpOrder = true;      //充值时是否需要请求Cp订单号，默认是需要的，只有应用宝等特殊渠道需要特殊处理

    private bool  canLogout = false;        //是否可以注销
    private float loginCheckTime = 0.0f;    //App从暂停状态恢复以后检测SDK登录时间间隔

    private bool needRecharge = false;      //登录后是否需要通知服务器验证上次充值是否到账，针对应用宝等特殊渠道的处理
    private bool haveUserCenter = false;    //是否有用户中心,针对特殊渠道（IOS越狱渠道:爱思）

    private string param1 = "";//备用参数1
    private string param2 = "";//备用参数2
    private string param3 = "";//备用参数3

    /// <summary>
    /// 华为专用属性
    /// </summary>
    private string ts = "";
    
    object ICloneable.Clone()
    {
        return this.Clone();
    }

    public SDKOnlineUser Clone()
    {
        return (SDKOnlineUser)this.MemberwiseClone();
    }
    
    public SDKOnlineUser()
    {
        Init();
    }

    private void Init()
    {
        this.payOrderUrl = "http://fzpay.wywlwx.com.cn/order/corder";
#if UNITY_EDITOR
        this.channelId = "{windows}";
        this.subChannel = "";

        this.haveChannelSDK = false;
        this.useChannelSDKExit = false;
        this.deviceTypeId = "1";
        
#elif UNITY_ANDROID
        this.deviceTypeId = "1";
#elif UNITY_IPHONE
        this.deviceTypeId = "2";
#endif
    }

    /// <summary>
    /// 角色信息重置
    /// </summary>
    public void Reset()
    {
        this.id = "";
        this.channelUserId = "";        
        this.channelUserName = "";

        this.token = "";
        this.productCode = "";

        this.needLoginCheck = true;
        this.needCpOrder = true;
        this.canLogout = false;
        this.needRecharge = false;

        this.param1 = "";
        this.param2 = "";
        this.param3 = "";
    }

    public void UpdateData(Dictionary<string, object> userinfo)
    {
        if (userinfo == null)
        {
            return;
        }

        object jsonObj = null;
        if (userinfo.TryGetValue("id",out jsonObj))
        {
            string str = jsonObj.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                this.id = str;
            }
        }

        if (userinfo.TryGetValue("uid", out jsonObj))
        {
            string str = jsonObj.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                this.id = str;
            }
        }

        if (userinfo.TryGetValue("channeluserid", out jsonObj))
        {
            string str = jsonObj.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                this.channelUserId = str;
            }
        }

        if (userinfo.TryGetValue("token", out jsonObj))
        {
            string str = jsonObj.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                this.token = str;
            }
        }

        if (userinfo.TryGetValue("username", out jsonObj))
        {
            string str = jsonObj.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                this.channelUserName = str;
            }
        }

        if (userinfo.TryGetValue("productcode", out jsonObj))
        {
            string str = jsonObj.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                this.productCode = str;
            }
        }

        //备用参数
        if (userinfo.TryGetValue("param1", out jsonObj))
        {
            string str = jsonObj.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                this.param1 = str;
            }
        }

        if (userinfo.TryGetValue("param2", out jsonObj))
        {
            string str = jsonObj.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                this.param2 = str;
            }
        }

        if (userinfo.TryGetValue("param3", out jsonObj))
        {
            string str = jsonObj.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                this.param3 = str;
            }
        }

        if (userinfo.TryGetValue("needcheck", out jsonObj))
        {
            string str = jsonObj.ToString();
            if (str.Equals("1") || str.Equals("true"))
            {
                this.needLoginCheck = true;
            }
            else if(str.Equals("0") || str.Equals("false"))
            {
                this.needLoginCheck = false;
            }
            else
            {
                this.needLoginCheck = (bool)jsonObj;
            }
        }

        if(userinfo.TryGetValue("needcporder", out jsonObj))
        {
            string str = jsonObj.ToString();
            if (str.Equals("1") || str.Equals("true"))
            {
                this.needCpOrder = true;
            }
            else if(str.Equals("0") || str.Equals("false"))
            {
                this.needCpOrder = false;
            }
            else
            {
                this.needCpOrder  = (bool)jsonObj;
            }
        }

        if (userinfo.TryGetValue("canLogout",out jsonObj))
        {
            string str = jsonObj.ToString();
            if (str.Equals("1") || str.Equals("true"))
            {
                this.canLogout = true;
            }
            else if(str.Equals("0") || str.Equals("false"))
            {
                this.canLogout = false;
            }
            else
            {
                this.canLogout = (bool)jsonObj;
            }
        }

        if (userinfo.TryGetValue("needRecharge", out jsonObj))
        {
            string str = jsonObj.ToString();
            if (str.Equals("1") || str.Equals("true"))
            {
                this.needRecharge = true;
            }
            else if (str.Equals("0") || str.Equals("false"))
            {
                this.needRecharge = false;
            }
            else
            {
                this.needRecharge = (bool)jsonObj;
            }
        }

        if (userinfo.TryGetValue("ts", out jsonObj))
        {
            string str = jsonObj.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                this.ts = str;
            }
        }

        if (userinfo.TryGetValue("loginchecktime",out jsonObj))
        {
            string str = jsonObj.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                this.loginCheckTime = float.Parse(str);
            }
        }

        if (userinfo.TryGetValue("haveusercenter",out jsonObj))
        {
            string str = jsonObj.ToString();
            if (str.Equals("1") || str.Equals("true"))
            {
                this.haveUserCenter = true;
            }
            else if (str.Equals("0") || str.Equals("false"))
            {
                this.haveUserCenter = false;
            }
            else
            {
                this.haveUserCenter = (bool)jsonObj;
            }
        }
    }   
    
    /// <summary>
    /// 获取渠道ID
    /// </summary>
    /// <param name="haveBracket"></param>
    /// <returns></returns>
    public string getChannelId(bool haveBracket = true)
    {
        if (!haveBracket)
        {
            return channelId.Trim(new char[] { '{', '}' });
        }

        return channelId;
    }

    /// <summary>
    /// 设置渠道ID
    /// </summary>
    /// <param name="channelId"></param>
    public void setChannelId(string channelId)
    {
        this.channelId = channelId;
    }

    /// <summary>
    /// 获取渠道编号
    /// </summary>
    /// <returns></returns>
    public int getChannleShortId()
    {
        return this.channelShortId;
    }

    /// <summary>
    /// 设置渠道编号
    /// </summary>
    /// <param name="id"></param>
    public void setChannelShortId(int id)
    {
        this.channelShortId = id;
    }

    public string getChannelName()
    {
        return this.channelName;
    }

    public void setChannelName(string name)
    {
        if(string.IsNullOrEmpty(name))
        {
            Debug.LogError("SDKOnlineUser::setChannelName-> name = null");
            return;
        }
        this.channelName = name;
    }

    public void setSubChannel(string value)
    {
        this.subChannel = value;
    }

    public string getSubChannel()
    {
        return this.subChannel;
    }

    public string getDeviceTypeId()
    {
        return this.deviceTypeId;
    }

    public void setDeviceTypeId(string platform)
    {
        this.deviceTypeId = platform;
    }

    public string getId()
    {
        return id;
    }

    public void setId(string id)
    {
        this.id = id;
    }

    public string getChannelUserId()
    {
        return channelUserId;
    }

    public void setChannelUserId(string channelUserId)
    {
        this.channelUserId = channelUserId;
    }

    public string getUserName()
    {
        return channelUserName;
    }

    public void setUserName(string userName)
    {
        this.channelUserName = userName;
    }

    public string getToken()
    {
        return token;
    }

    public void setToken(string token)
    {
        this.token = token;
    }

    public string getTs()
    {
        return ts;
    }

    public void setTs(string ts)
    {
        this.ts = ts;
    }

    public string getProductCode()
    {
        return productCode;
    }

    public void setProductCode(string productCode)
    {
        this.productCode = productCode;
    }

    public string getParam1()
    {
        return param1;
    }

    public void setParam1(string param1)
    {
        this.param1 = param1;
    }

    public string getParam2()
    {
        return param2;
    }

    public void setParam2(string param2)
    {
        this.param2 = param2;
    }

    public string getParam3()
    {
        return param3;
    }

    public void setParam3(string param3)
    {
        this.param3 = param3;
    }

    public bool getNeedLoginCheck()
    {
        return needLoginCheck;
    }

    public void setNeedLoginCheck(bool value)
    {
        needLoginCheck = value;
    }

    public bool getNeedCpOrder()
    {
        return needCpOrder;
    }

    public void setNeedCpOrder(bool value)
    {
        needCpOrder = value;
    }

    public bool getCanLogout() {
        return canLogout;
    }

    public void setcanLogout(bool value) {
        canLogout = value;
    }

    public bool getNeedRecharge() {
        return needRecharge;
    }

    public void setneedRecharge(bool value) {
        needRecharge = value;
    }

    public void setLoginCheckUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("SDKOnlineUser::setLoginCheckUrl-> url = null");
            return;
        }
        this.loginCheckUrl = url;
    }

    public string getLoginCheckUrl()
    {
        //登录验证URL的地址格式为：http://fzlogin.wywlwx.com.cn/login/index
        while(loginCheckUrl.EndsWith("/"))
        {
            loginCheckUrl = loginCheckUrl.Substring(0, loginCheckUrl.Length - 1);
        }
        return loginCheckUrl;
    }

    public void setPayOrderUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("SDKOnlineUser::setPayOrderUrl-> url = null");
            return;
        }
        this.payOrderUrl = url;
    }

    public string getPayOrderUrl()
    {
        while (payOrderUrl.EndsWith("/"))
        {
            payOrderUrl = payOrderUrl.Substring(0, payOrderUrl.Length - 1);
        }
        return this.payOrderUrl;
    }

    public void setPaySyncUrl(string url)
    {
        this.paySyncUrl = url;
    }

    public string getPaySyncUrl()
    {
        return this.paySyncUrl;
    }

    public float getLoginCheckTime()
    {
        return this.loginCheckTime;
    }

    public void setLoginCheckTime(float seconds)
    {
        this.loginCheckTime = seconds;
    }

    public bool getHaveUserCenter()
    {
        return this.haveChannelSDK;
    }

    public void setHaveUserCenter(bool val)
    {
        this.haveChannelSDK = val;
    }

    public override string ToString()
    {
        string str = "SFOnlineUser()-> "; 
        str = str + " haveChannelSDK = " + haveChannelSDK;
        str = str + " useChannelSDKExit = " + useChannelSDKExit;
        str = str + " channelId = \"" + channelId + "\"";
        str = str + " id = \"" + id + "\"";
        str = str + " channelUserId = \"" + channelUserId + "\"";
        str = str + " channelUserName = \"" + channelUserName + "\"";
        str = str + " token = \"" + token + "\"";
        str = str + " productCode =  \"" + productCode + "\"";
        str = str + " needLoginCheck =  \"" + needLoginCheck + "\"";
        str = str + " needCpOrder =  \"" + needCpOrder + "\"";
        str = str + " loginCheckUrl =  \"" + loginCheckUrl + "\"";
        str = str + " payOrderUrl =  \"" + payOrderUrl + "\"";
        str = str + " paySyncUrl =  \"" + paySyncUrl + "\"";
        str = str + " canLogout =  \"" + canLogout + "\"";
        str = str + " loginCheckTime =  \"" + loginCheckTime + "\"";
        str = str + " needRecharge =  \"" + needRecharge + "\"";
        str = str + " param1 =  \"" + param1 + "\"";
        str = str + " param2 =  \"" + param2 + "\"";
        str = str + " param3 =  \"" + param3 + "\"";
        return str;
    }
}

