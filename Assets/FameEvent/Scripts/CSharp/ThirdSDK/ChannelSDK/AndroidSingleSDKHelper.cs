using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Net;
using MiNiJSON;
using System.Collections.Generic;


public class AndroidSingleSDKHelper : ChannelSDKHelperBase
{
#if UNITY_ANDROID
    /// <summary>
    /// android插件
    /// </summary>
    private AndroidJavaObject _SDKHelperPlugin;
#endif

    protected AndroidSingleSDKHelper()
    {

    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = GetComponent<AndroidSingleSDKHelper>();
        }

        if (_instance == null)
        {
            Debug.LogError("AndroidSingleSDKHelper Awake(), _instance == null!!!");
        }

        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
#if UNITY_ANDROID
        using (var pluginClass = new AndroidJavaClass(SDKDefine.AndroidChannelPluginName))
            _SDKHelperPlugin = pluginClass.CallStatic<AndroidJavaObject>("instance");

        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("AndroidSingleSDKHelper init() _SDKHelperPlugin == null!!!!");
        }
#endif
    }

    #region 外部调用SDK功能统一接口
    /// <summary>
    /// 游戏启动以后初始化SDK
    /// </summary>
    public override void InitSDK(string customParams = "")
    {
        Debug.Log("AndroidSingleSDKHelper::InitSDK() Called!");
        if (Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("AndroidSingleSDKHelper::InitSDK() Application.platform != RuntimePlatform.Android return!!!");
            return;
        }

        //如果SDK已经初始化过就不需要再初始化了
        if (mCurSDKPhase >= SDKPhase.Init)
        {
            Debug.Log("AndroidSingleSDKHelper::InitSDK() isSDKInited == true return!!!");
            return;
        }

        mCurSDKPhase = SDKPhase.Init;

#if UNITY_ANDROID
        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("AndroidSingleSDKHelper::InitSDK() _SDKHelperPlugin == null!!!!!!!");
            return;
        }
        _SDKHelperPlugin.Call("InitSDK");
#endif

        mCurSDKPhase = SDKPhase.Inited;
    }

    /// <summary>
    /// 登录
    /// </summary>
    public override void Login(string customParams = "")
    {
        Debug.Log("AndroidSingleSDKHelper::Login() Called!");
        if (Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("AndroidSingleSDKHelper::Login() Application.platform != RuntimePlatform.Android, return!!!");
            return;
        }

        //如果SDK已经登录成功，就直接走登录成功的流程
        if (mCurSDKPhase == SDKPhase.LoginFinish)
        {
            AndroidChannelSDKMgr.OnLoginEvent(true);
            return;
        }

        //如果SDK还未初始化完成，直接返回
        if (mCurSDKPhase != SDKPhase.Inited)
        {
            Debug.Log("AndroidSingleSDKHelper::Login() mCurSDKState != SDKState.Inited, return!!!");
            return;
        }

        mCurSDKPhase = SDKPhase.Login;

#if UNITY_ANDROID
        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("AndroidSingleSDKHelper::Login() _SDKHelperPlugin == null!!!!!!!");
            return;
        }
        Debug.Log("AndroidSingleSDKHelper::Login->_SDKHelperPlugin.Call(Login)");
        _SDKHelperPlugin.Call("Login");
#endif
    }

    /// <summary>
    /// 应用宝登录
    /// </summary>
    /// <param name="platform"></param>
    public override void LoginForTencent(string platform)
    {
        Debug.Log("AndroidSingleSDKHelper::LoginForTencent() Called!"+"platform is :"+platform);
        if (Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("AndroidSingleSDKHelper::LoginForTencent() Application.platform != RuntimePlatform.Android, return!!!");
            return;
        }

#if UNITY_ANDROID
        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("AndroidSingleSDKHelper::LoginForTencent() _SDKHelperPlugin == null!!!!!!!");
            return;
        }
        Debug.Log("AndroidSingleSDKHelper::LoginForTencent->_SDKHelperPlugin.Call(OnLoginForTencent)");
        _SDKHelperPlugin.Call("OnLoginForTencent",platform);
#endif
    }


    /// <summary>
    /// 登出
    /// </summary>
    public override void Logout(string customParams = "")
    {
        Debug.Log("AndroidSingleSDKHelper::Logout() Called!");
        if (Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("AndroidSingleSDKHelper::Logout() Application.platform != RuntimePlatform.Android return!!!");
            return;
        }

        if(mCurSDKPhase <= SDKPhase.Login)
        {
            Debug.Log("AndroidSingleSDKHelper::Logout() mCurSDKState <= SDKState.Login, return!!!");
            return;
        }

#if UNITY_ANDROID
        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("AndroidSingleSDKHelper::Logout() _SDKHelperPlugin == null!!!!!!!");
            return;
        }
        _SDKHelperPlugin.Call("Logout");
#endif
    }

    /// <summary>
    /// 退出
    /// </summary>
    public override void Exit()
    {
        Debug.Log("AndroidSingleSDKHelper::Exit() Called!!!");
        if (Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("AndroidSingleSDKHelper::Exit() Application.platform != RuntimePlatform.Android return!!!");
            return;
        }

        if (mCurSDKPhase <= SDKPhase.Login)
        {
            Debug.Log("AndroidSingleSDKHelper::Exit() mCurSDKState <= SDKState.Login, return!!!");
            return;
        }

#if UNITY_ANDROID
        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("AndroidSingleSDKHelper::Exit() _SDKHelperPlugin == null!!!!!!!");
            return;
        }
        _SDKHelperPlugin.Call("Exit", exitListener.gameObjectName, exitListener.listerFuncName);
#endif
    }

    /// <summary>
    /// 定额支付
    /// </summary>
    /// <param name="configId"> 充值配置ID </param>
    public override void Pay(PayInfo payInfo)
    {
        Debug.Log("AndroidSingleSDKHelper::Pay() Called!!!");
        if (mCurSDKPhase < SDKPhase.LoginFinish)
        {
            Debug.Log("AndroidSingleSDKHelper::Pay() mCurSDKState < SDKState.Logined, return!!!");
            return;
        }

#if UNITY_ANDROID
        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("AndroidSingleSDKHelper::RequestPayOrderInfo() _SDKHelperPlugin == null!!!!!!!");
            return;
        }
        _SDKHelperPlugin.Call("Pay", payInfo.GetPayInfoStr());
#endif
    }

    /// <summary>
    /// 获取支付时需要的参数
    /// </summary>
    /// <returns></returns>
    public override string GetPayInfo()
    {
        Debug.Log("AndroidSingleSDKHelper::GetPayInfo() Called!!!");
        if (Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("AndroidSingleSDKHelper::GetPayInfo() Application.platform != RuntimePlatform.Android return !!!");
            return "";
        }

#if UNITY_ANDROID
        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("GetPayInfo() _SDKHelperPlugin == null!!!!!!!");
            return "";
        }
        return _SDKHelperPlugin.Call<string>("GetPayInfo");
#else
        return "";
#endif
        
    }

    /// <summary>
    /// 非定额计费接口
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="unitPrice"></param>
    /// <param name="count"></param>
    /// <param name="callBackInfo"></param>
    public override void Charge(string itemName, int unitPrice, int count, string callBackInfo)
    {
        Debug.Log("AndroidSingleSDKHelper::Charge() Called!!!");
        if (Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("AndroidSingleSDKHelper::Charge() Application.platform != RuntimePlatform.Android return !!!");
            return;
        }

        if (mCurSDKPhase < SDKPhase.LoginFinish)
        {
            Debug.Log("AndroidSingleSDKHelper::Charge() mCurSDKState < SDKState.Logined, return !!!");
            return;
        }

//#if UNITY_ANDROID
//        if (_SDKHelperPlugin == null)
//        {
//            Debug.LogError("Charge() _SDKHelperPlugin == null!!!!!!!");
//            return;
//        }
//        _SDKHelperPlugin.Call("Charge", payListener.gameObjectName, itemName, unitPrice, count, callBackInfo, CP_PAY_SYNC_URL, payListener.listerFuncName);
//#endif
    }

    /// <summary>
    /// 设置角色数据（登录成功后调用）
    /// </summary>
    /// <param name="roleId">角色唯一标识</param>
    /// <param name="roleName">角色名</param>
    /// <param name="roleLevel">角色等级</param>
    /// <param name="zoneId">区域唯一标识</param>
    /// <param name="zoneName">区域名称</param>
    public override void SetRoleData(ChannelUserInfo userInfo, long createTime = 0)
    {
        Debug.Log("AndroidSingleSDKHelper::SetRoleData() Called!!!");
        if (Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("AndroidSingleSDKHelper::SetRoleData() Application.platform != RuntimePlatform.Android, return!!!");
            return;
        }
        if (mCurSDKPhase < SDKPhase.LoginFinish)
        {
            Debug.Log("AndroidSingleSDKHelper::SetRoleData() mCurSDKState < SDKState.Logined, return!!!");
            return;
        }
#if UNITY_ANDROID
        if (_SDKHelperPlugin == null)
        {
            Debug.LogError(" Login() _SDKHelperPlugin == null!!!!!!!");
            return;
        }
        _SDKHelperPlugin.Call("SetRoleData", userInfo.RoleId, userInfo.RoleName, userInfo.RoleLevel, userInfo.ZoneId, userInfo.ZoneName, createTime);
        JSONObject gameinfo = new JSONObject();
        gameinfo.AddField("roleId", userInfo.RoleId);
        gameinfo.AddField("roleName", userInfo.RoleName);
        gameinfo.AddField("roleLevel", userInfo.RoleLevel);
        gameinfo.AddField("zoneId", userInfo.ZoneId);
        gameinfo.AddField("zoneName", userInfo.ZoneName);
        _SDKHelperPlugin.Call("SetData", "gameinfo", gameinfo.ToString());
#endif
    }

    /// <summary>
    /// 发送角色数据
    /// </summary>
    /// <param name="dataType"></param>
    /// <param name="roleId"></param>
    /// <param name="roleName"></param>
    /// <param name="roleLevel"></param>
    /// <param name="zoneId"></param>
    /// <param name="zoneName"></param>
    /// <param name="balance"></param>
    /// <param name="vip"></param>
    /// <param name="partyName"></param>
    /// <param name="roleCTime"></param>
    /// <param name="roleLevelMTime"></param>
    public override void SendRoleData(SDKDefine.SendRoleDataType dataType, int roleId, string roleName, int roleLevel, int zoneId, string zoneName, ulong roleCTime, ulong roleLevelMTime,
        int balance = 0, int vip = 1, string partyName = "无帮派")
    {
        Debug.Log("AndroidSingleSDKHelper::SendRoleData() Called!!!");
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

#if UNITY_ANDROID
        if (_SDKHelperPlugin == null)
        {
            Debug.LogError(" Login() _SDKHelperPlugin == null!!!!!!!");
            return;
        }
#endif
    }
    #endregion

    #region SDK回调处理
    /// <summary>
    /// 初始化结果回调
    /// </summary>
    /// <param name="result"></param>
    public override void OnInitCallBack(string result)
    {
        Debug.Log("AndroidSingleSDKHelper::OnInitCallBack() Called, result = " + result);
        var resultDict = Json.Deserialize(result) as Dictionary<string, object>;
        if (resultDict == null)
        {
            return;
        }

        //SDK初始化成功
        object tempObj;
        if (resultDict.TryGetValue("userinfo", out tempObj))
        {
            //更新SDK相关角色信息
            var userInfo = tempObj as Dictionary<string, object>;
            mCurrentUser.UpdateData(userInfo);
        }
    }

    /// <summary>
    /// 登录、登出回调
    /// </summary>
    /// <param name="result"></param>
    public override void OnLoginCallBack(string result)
    {
        Debug.Log("AndroidSingleSDKHelper::OnLoginCallBack() Called, result = " + result);
        var loginData = Json.Deserialize(result) as Dictionary<string, object>;
        if (loginData == null)
        {
            Debug.LogError("AndroidSingleSDKHelper::OnLoginCallBack->miniJson == null!!!");
            OnLoginFailed();
            return;
        }

        string type = (string)loginData["result"];
        if (SDKDefine.LoginResult.LOGOUT == type)
        {
            //SDK登出
            OnLogout();
        }
        else if (SDKDefine.LoginResult.LOGIN_SUCCESS == type)
        {
            //SDK登录成功
            var userinfo = loginData["userinfo"] as Dictionary<string, object>;

            //登录成功以后再收到登录成功的回调要针对有些SDK做特殊处理,像掌纵这种SDK
            if (mCurSDKPhase == SDKPhase.LoginFinish /*&& newUser.getChannelUserId() != currentUser.getChannelUserId()*/)
            {
                //帐号不同就是切换帐号
                SwitchUser(userinfo, true);
            }
            else
            {
                OnLoginSuccess(userinfo);
            }
        }
        else if (SDKDefine.LoginResult.LOGIN_FAILED == type)
        {
            //SDK登录失败
            OnLoginFailed();
        }
        //应用宝的登出
        else if (SDKDefine.LoginResult.LOGOUT_TENCENT == type)
        {
            OnLogout(false);
        }
    }

    public override void OnLoginForTencentCallBack(string result)
    {
        Debug.Log("AndroidSingleSDKHelper::OnLoginForTencentCallBack() Called, result = " + result);
      
    }

    /// <summary>
    /// 退出监听函数
    /// </summary>
    /// <param name="result"></param>
    public override void OnExitCallBack(string result)
    {
        
        Debug.Log("AndroidSingleSDKHelper::OnExitCallBack-> result=" + result);
        JSONObject sfjson = new JSONObject(result);
        string type = sfjson.GetField("result").str;
        string data = sfjson.GetField("data").str;

        if (SDKDefine.ExitResult.SDKEXIT == type)
        {
            //SDK退出
            if (data.Equals("true"))
            {
                Application.Quit();
            }
            else if (data.Equals("false"))
            {

            }
        }
        else if (SDKDefine.ExitResult.SDKEXIT_NO_PROVIDE == type)
        {
            //游戏自带退出界面
            Application.Quit();
        }
         
    }

    /// <summary>
    /// 支付监听函数
    /// </summary>
    /// <param name="result"></param>
    public override void OnPayCallBack(string result)
    {
        
        Debug.Log("AndroidSingleSDKHelper::OnPayCallBack()-> result=" + result);
        JSONObject sfjson = new JSONObject(result);
        string payResult = sfjson.GetField("result").str;
        if (payResult.Equals(SDKDefine.PayResult.PAY_SUCCESS))
        {
            Debug.Log("AndroidSingleSDKHelper::OnPayCallBack->SDK支付提交成功！！");
        }
        else
        {
            Debug.Log("AndroidSingleSDKHelper::OnPayCallBack->SDK支付提交错误!  payResult = " + payResult);
        }
         
    }
    #endregion

    #region SDK登录结果处理以及登录验证
    /// <summary>
    /// 创建登录验证url,Url完整格式为: http://fzlogin.wywlwx.com.cn/login/index/id/{渠道Id}/token/{验证TokenId}
    /// </summary>
    /// <returns></returns>
    private string CreateLoginCheckURL(SDKOnlineUser userInfo)
    {
        if (mCurrentUser == null)
        {
            return null;
        }
        string url = "";
        if (true)//(GameManager.Instance.isTestServer)
        {
            url = "http://www.xupule.cn/nbf/index.php/login/check.html";
        }
        else
        {
            url = "http://web.fu-5.xyz/nbf/index.php/login/check.html";
        }
        if (string.IsNullOrEmpty(url))
        {
            return "";
        }
        StringBuilder builder = new StringBuilder(url);
        builder.Append("?gmid=");
        //builder.Append(GameManager.Instance.gameKey);
        builder.Append("&agentid=");
        //builder.Append(ConfigInfo.WBFlag.ToString());
        builder.Append("&uin=");
        builder.Append(mCurrentUser.getChannelUserId());
        builder.Append("&sess=");
        builder.Append(SDKUtils.ToUrlEncode(mCurrentUser.getToken()));
        builder.Append("&ext1=");
        builder.Append("&ext2=");
        return builder.ToString();
    }

    //登录验证完成后后期处理的代理
    public delegate void LoginCheckFinishDelegate();

    /// <summary>
    /// 登录验证
    /// </summary>
    private IEnumerator LoginCheck(SDKOnlineUser userInfo, LoginCheckFinishDelegate finishCheckDelegate)
    {
        Debug.Log("AndroidSingleSDKHelper::LoginCheck() Called!!");
        mCurSDKPhase = SDKPhase.LoginCheck;

        //如果SDK角色信息为空,直接走登录验证失败的流程
        if (userInfo == null)
        {
            Debug.LogError("AndroidSingleSDKHelper::LoginCheck-> userInfo == null!!");
          //  UICommonManager.GetInstance().bubbleLoopTipsControl.ShowTips("登录验证失败");
            OnLoginCheckFailed();
            yield break;
        }

        //如果SDK不需要登录验证,直接走登录验证成功的流程
        if (!userInfo.getNeedLoginCheck())
        {
            Debug.Log("AndroidSingleSDKHelper::LoginCheck-> user.getNeedLoginCheck == false");
            //更新当前用户信息
            mCurrentUser = userInfo;

            //调用代理
            finishCheckDelegate();
            yield break;
        }

        //生成登录验证Url地址
        string url = CreateLoginCheckURL();
        Debug.Log("AndroidSingleSDKHelper::LoginCheck-> url = " + url);
        //WWW用Post方式请求登录验证结果
        WWW www = new WWW(url);
        yield return www;

        string result = "";
        if (www.isDone && string.IsNullOrEmpty(www.error))
        {
            result = www.text;
        }
        Debug.Log("AndroidSingleSDKHelper::LoginCheck-> result = " + result);

        //解析登录验证返回的信息
        var respondDict = Json.Deserialize(result) as Dictionary<string, object>;
        if (respondDict == null)
        {
            //如果登录验证返回的信息为空,直接走登录验证失败的流程
            Debug.LogError("AndroidSingleSDKHelper::LoginCheck-> respondDict == null!!!");
          //  UICommonManager.GetInstance().bubbleLoopTipsControl.ShowTips("登录验证失败");
            OnLoginCheckFailed();
            //finishCheckDelegate();
            yield break;
        }

        string type = (string)respondDict["code"];
        if (type == null || !(type == "1"))
        {
            //如果登录验证返回的登录验证的标志不是SUCCESS,直接走登录验证失败的流程
            Debug.LogError("AndroidSingleSDKHelper::LoginCheck-> respondDict[result] != SUCCESS!!!");
           // UICommonManager.GetInstance().bubbleLoopTipsControl.ShowTips("登录验证失败");
            OnLoginCheckFailed();
            //finishCheckDelegate();
            yield break;
        }

        //如果登录验证返回用户信息是空,直接走登录验证失败的流程
        if (respondDict["info"] == null)
        {
            Debug.LogError("AndroidSingleSDKHelper::LoginCheck-> respondDict[userinfo] == null!!!");
           // UICommonManager.GetInstance().bubbleLoopTipsControl.ShowTips("登录验证失败");
            OnLoginCheckFailed();
            yield break;
        }
        
        //登录验证成功处理
        var userinfoDic = respondDict["info"] as Dictionary<string, object>;

        //更新当前用户信息
        //userInfo.UpdateData(userinfoDic);
       // mCurrentUser = userInfo;

        //调用代理
        finishCheckDelegate();
         
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private string CreateLoginCheckURL()
    {
        if (mCurrentUser == null)
        {
            return null;
        }
        string url = "";
        if (true)//(GameManager.Instance.isTestServer)
        {
            url = "http://www.xupule.cn/nbf/index.php/login/check.html";
        }
        else
        {
            url = "http://web.fu-5.xyz/nbf/index.php/login/check.html";
        }
        if (string.IsNullOrEmpty(url))
        {
            return "";
        }
        StringBuilder builder = new StringBuilder(url);
        builder.Append("?gmid=");
      //  builder.Append(GameManager.Instance.gameKey);
        builder.Append("&agentid=");
       // builder.Append(ConfigInfo.WBFlag.ToString());
        builder.Append("&uin=");
        builder.Append(mCurrentUser.getChannelUserId());
        builder.Append("&sess=");
        builder.Append(SDKUtils.ToUrlEncode(mCurrentUser.getToken()));
        builder.Append("&ext1=");
        builder.Append("&ext2=");
        return builder.ToString();
    }

    /// <summary>
    /// SDK登录成功后的处理
    /// </summary>
    /// <param name="userinfo"></param>
    private void OnLoginSuccess(Dictionary<string, object> userinfo)
    {
        Debug.Log("AndroidSingleSDKHelper::OnLoginSuccess() Called!!");
        if (userinfo == null)
        {
            OnLoginFailed();
            return;
        }

        //登录用户信息
        SDKOnlineUser newUser = mCurrentUser.Clone();
        newUser.UpdateData(userinfo);

        //登录验证
        StartCoroutine(LoginCheck(newUser, () =>
        {
            OnLoginCheckSuccess();
        }));
    }

    /// <summary>
    /// SDK登录失败后的处理
    /// </summary>
    private void OnLoginFailed()
    {
        Debug.Log("AndroidSingleSDKHelper::OnLoginFailed() Called!!");
        
        AndroidChannelSDKMgr.OnLoginEvent(false);

      
        /*
        UICommonManager.GetInstance().messageBoxControl.ShowMessage(Localization.instance.Get("SDKLoginFailedMsg"),
                (UIMessageBox mb) =>
                {
                    mCurrentUser.Reset();
                    mCurSDKPhase = SDKPhase.Inited;
                    Login();
                },
                (UIMessageBox mb) =>
                {
                    Application.Quit();
                });
         */ 
         
    }

    /// <summary>
    /// SDK登录验证成功后的处理
    /// </summary>
    /// <param name="userInfoDict"></param>
    private void OnLoginCheckSuccess()
    {
        Debug.Log("AndroidSingleSDKHelper::OnLoginCheckSuccess() Called!!");
        if (mCurSDKPhase != SDKPhase.LoginCheck)
        {
            Debug.Log("AndroidSingleSDKHelper::OnLoginCheckSuccess() mCurSDKPhase != SDKPhase.LoginCheck!!");
            OnLoginFailed();
            return;
        }

        mCurSDKPhase = SDKPhase.LoginFinish;
        mTryLoginCount = 0;
        Debug.Log("AndroidSingleSDKHelper::OnLoginCheckSuccess-> currentUser = " + mCurrentUser.ToString());
        AndroidChannelSDKMgr.OnLoginEvent(true);
    }

    /// <summary>
    /// SDK登录验证失败后的处理
    /// </summary>
    private void OnLoginCheckFailed()
    {
        Debug.Log("AndroidSingleSDKHelper::OnLoginCheckFailed() Called!!");
     
        AndroidChannelSDKMgr.OnLoginEvent(false);
        /*
     UICommonManager.GetInstance().messageBoxControl.ShowMessage(Localization.instance.Get("SDKLoginCheckFailedMsg"),
            (UIMessageBox mb) =>
            {
                mCurrentUser.Reset();
                mCurSDKPhase = SDKPhase.Inited;
                Login();
            },
            (UIMessageBox mb) =>
            {
                Application.Quit();
            });
      */ 
    }
    
    /// <summary>
    /// SDK登出处理
    /// </summary>
    private void OnLogout(bool needRelogin=true)
    {
        Debug.Log("AndroidSingleSDKHelper::OnLogout() Called!!");
        mCurrentUser.Reset();
        mCurSDKPhase = SDKPhase.Inited;
        AndroidChannelSDKMgr.OnLogOutEvent();
        if (needRelogin)
        {
            Login();
        }
    }

    private void SwitchUser(Dictionary<string, object> userinfo, bool needLogout)
    {
        //登录用户信息
        SDKOnlineUser newUser = mCurrentUser.Clone();
        newUser.UpdateData(userinfo);

        //登录验证
        StartCoroutine(LoginCheck(newUser, () =>
        {
            OnSwitchUserSuccess(needLogout);
        }));
    }
    
    private void OnSwitchUserSuccess(bool needLogout)
    {
        Debug.Log("AndroidSingleSDKHelper::OnSwitchUserSuccess() Called!!");
        mCurSDKPhase = SDKPhase.LoginFinish;
        AndroidChannelSDKMgr.OnSwitchUserEvent(needLogout);
    }

    #endregion

    #region 游戏暂定时SDK相关处理
    private bool mCurPauseStatus = false;
    /// <summary>
    /// U3D应用暂停事件处理
    /// </summary>
    /// <param name="pauseStatus"></param>
    void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("AndroidSingleSDKHelper::OnApplicationPause() Called!!  pauseStatus = " + pauseStatus);

        mCurPauseStatus = pauseStatus;
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        if (pauseStatus)
        {
            return;
        }
    }

    private int mTryLoginCount = 0;
    private IEnumerator CheckLoginState()
    {
#if UNITY_ANDROID
        //如果应用为非暂停状态，同时检测到SDK未登录就调用SDK的登录
        if (_SDKHelperPlugin == null)
        {
            yield break;
        }

        float secondTime = mCurrentUser.getLoginCheckTime();
        if(secondTime < 0.1f)
        {
            yield break;
        }

        yield return new WaitForSeconds(secondTime + mTryLoginCount*1.0f);

        //如果原来是暂停状态，并且没有登录SDK，当取消暂停时就调用SDK登录
        if (!mCurPauseStatus && mCurSDKPhase <= SDKPhase.Login)
        {
            Debug.LogError("OnApplicationPause==>mCurSDKPhase:" + (int)mCurSDKPhase);
            _SDKHelperPlugin.Call("Login");
        }
#else
        yield break;
#endif

    }
    #endregion
}