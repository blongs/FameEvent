using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Net;


public class AndroidChannelSDKMgr : MonoBehaviour
{
    private const string _gameObjectName = "AndroidChannelSDKMgr";

    protected static AndroidChannelSDKMgr _instance;
    public static AndroidChannelSDKMgr instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject(_gameObjectName);
                obj.name = _gameObjectName;
                _instance = obj.AddComponent<AndroidChannelSDKMgr>();
            }
            return _instance;
        }
    }

    protected static ChannelSDKHelperBase _SDKHelper;
    public static ChannelSDKHelperBase SDKHelper
    {
        get
        {
            if (_SDKHelper == null)
            {
                string channelId = "";
                bool haveChannelSDK = false;
                string sdkHelperName = DeterminSDKHelper(out channelId, out haveChannelSDK);
                if (sdkHelperName != null && sdkHelperName != "")
                {
                    _SDKHelper = instance.gameObject.GetComponent(sdkHelperName) as ChannelSDKHelperBase;
                }

                if (_SDKHelper == null)
                {
                    _SDKHelper = instance.gameObject.AddComponent<ChannelSDKHelperBase>();
                }
            }

            return _SDKHelper;
        }
    }

    protected AndroidChannelSDKMgr()
    {

    }

    private static string DeterminSDKHelper(out string channelId, out bool haveChannelSDK)
    {
        string SDKHelperName = "";

        object metaValue = SDKUtils.GetMetaData(SDKDefine.OneSDKDefaultChannelIDMeta.key);
        channelId = metaValue != null ? metaValue.ToString() : "";
        channelId = channelId.Trim();
        if (channelId != "")
        {
            //如果易接的channelid 不为空就表示使用的易接的SDK
            SDKHelperName = "AndroidMultSDKHelper";
            haveChannelSDK = true;
        }
        else
        {
            metaValue = SDKUtils.GetMetaData(SDKDefine.MHJSDKDefaultChannelIDMeta.key);
            channelId = metaValue != null ? metaValue.ToString() : "";
            channelId = channelId.Trim();
            Debug.Log("channelId = " + channelId.ToString());
            if (channelId == SDKDefine.MHJSDKDefaultChannelIDMeta.value && channelId != "")
            {
                //如果我们自己接的渠道的 channelid 不为空且不是默认值,表示自己接入的渠道
                SDKHelperName = "AndroidSingleSDKHelper";
                haveChannelSDK = true;
            }
            else
            {
                SDKHelperName = "ChannelSDKHelperBase";
                haveChannelSDK = false;
            }
        }

        Debug.Log("AndroidChannelSDKMgr::DeterminSDKHelper->SDKHelperName = " + SDKHelperName);
        return SDKHelperName;
    }

    // Use this for initialization
    void Awake()
    {
        Debug.Log("allen----Awake----");
        string channelId = "";
        bool haveChannelSDK = false;
        string sdkHelperName = DeterminSDKHelper(out channelId, out haveChannelSDK);

        //设置SDKUser 渠道等信息
        // user.haveChannelSDK = haveChannelSDK;
        // user.setChannelId(channelId);

        if (_instance == null)
        {
            _instance = GetComponent<AndroidChannelSDKMgr>();
            if (sdkHelperName != null && sdkHelperName != null)
            {
                _SDKHelper = instance.gameObject.AddComponent<ChannelSDKHelperBase>();
            }

            DontDestroyOnLoad(gameObject);
        }

        //初始化
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        //获取子渠道
        object metaValue = SDKUtils.GetMetaData(SDKDefine.AndroidSubChannelMeta.key);
        string subChannelValue = metaValue != null ? metaValue.ToString() : "";
        if (string.IsNullOrEmpty(subChannelValue))
        {
            subChannelValue = SDKDefine.AndroidSubChannelMeta.value;
        }
        Debug.Log("AndroidChannelSDKMgr::GetMetaData-> SubChannel = " + subChannelValue);
        user.setSubChannel(subChannelValue.Trim());

        //判断是否使用sdk退出方式
        if (user.haveChannelSDK)
        {
            metaValue = SDKUtils.GetMetaData(SDKDefine.AndroidExitTypeMeta.key);
            string exitType = metaValue != null ? metaValue.ToString() : "";
            Debug.Log("AndroidChannelSDKMgr::GetMetaData-> exittype = " + exitType);
            if (exitType == SDKDefine.AndroidExitTypeMeta.value) // game：游戏的退出方式；sdk：sdk的退出方式
            {
                // user.useChannelSDKExit = false;
            }
            else
            {
                // user.useChannelSDKExit = true;
            }
        }
        else
        {
            // user.useChannelSDKExit = false;
        }

        //设置监听器
        SetInitListener();
        SetLoginListener();
        SetExitListener();
        SetPayListener();
    }

    protected void Start()
    {

    }

    protected void Update()
    {

    }

    #region 外部调用SDK功能统一接口

    public static void InitSDK()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.InitSDK();
    }

    /// <summary>
    /// 登录
    /// </summary>
    public static void Login()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.Login();
    }

    /// <summary>
    /// 应用宝登录
    /// </summary>
    public static void LoginForTencent(string platform)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.LoginForTencent(platform);
    }

    /// <summary>
    /// 登出
    /// </summary>
    public static void Logout(string customParams = "")
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.Logout(customParams);
    }

    /// <summary>
    /// 退出
    /// </summary>
    public static void Exit()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.Exit();
    }

    /// <summary>
    /// 设置角色数据（登录成功后调用）
    /// </summary>
    /// <param name="roleId">角色唯一标识</param>
    /// <param name="roleName">角色名</param>
    /// <param name="roleLevel">角色等级</param>
    /// <param name="zoneId">区域唯一标识</param>
    /// <param name="zoneName">区域名称</param>
    public static void SetRoleData(ChannelUserInfo userInfo, long createTime = 0)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.SetRoleData(userInfo, createTime);
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
    public static void SendRoleData(SDKDefine.SendRoleDataType dataType, int roleId, string roleName, int roleLevel, int zoneId, string zoneName, ulong roleCTime, ulong roleLevelMTime,
        int balance = 0, int vip = 1, string partyName = "无帮派")
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.SendRoleData(dataType, roleId, roleName, roleLevel, zoneId, zoneName, roleCTime, roleLevelMTime, balance, vip, partyName);
    }

    /// <summary>
    /// 定额支付
    /// </summary>
    /// <param name="configId">充值配置ID</param>
    public static void Pay(PayInfo payInfo)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.Pay(payInfo);
    }

    /// <summary>
    /// 获取支付时需要的参数
    /// </summary>
    /// <returns></returns>
    public static string GetPayInfo()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return "";
        }

        return SDKHelper.GetPayInfo();
    }

    /// <summary>
    /// 非定额计费接口
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="unitPrice"></param>
    /// <param name="count"></param>
    /// <param name="callBackInfo"></param>
    public static void Charge(string itemName, int unitPrice, int count, string callBackInfo)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.Charge(itemName, unitPrice, count, callBackInfo);
    }

    /// <summary>
    /// 进入用户中心，针对特殊渠道
    /// </summary>
    /// <param name="strParams"> 传入的参数, json格式 </param>
    public static void EnterUserCenter(string data)
    {
        Debug.Log("IOSChannelSDKMgr::EnterUserCenter called!!");
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.EnterUserCenter(data);
    }

    #endregion

    #region SDK回调统一接口
    /// <summary>
    /// SDK初始化回调
    /// </summary>
    public void OnSDKInitResult(string result)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.OnInitCallBack(result);
    }

    /// <summary>
    /// 登录、登出回调
    /// </summary>
    /// <param name="result"></param>
    public void OnLoginResult(string result)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.OnLoginCallBack(result);
    }

    /// <summary>
    /// 给应用宝登录回调的
    /// </summary>
    public void OnLoginForTencent(string result)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.OnLoginForTencentCallBack(result);
    }

    /// <summary>
    /// 退出监听函数
    /// </summary>
    /// <param name="result"></param>
    public void OnExitResult(string result)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.OnExitCallBack(result);
    }

    /// <summary>
    /// 支付监听函数
    /// </summary>
    /// <param name="result"></param>
    public void OnPayResult(string result)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.OnPayCallBack(result);
    }
    #endregion

    #region 内部方法-设置监听
    /**
    * SetInitListener方法用于设置初始化监听
    *  @param gameObject	游戏场景中的对象，SDK内部完成初始化逻辑后，
    * 						并把登录结果通过Unity的
    * 						API(com.unity3d.player.UnityPlayer.UnitySendMessage(String gameObject,
    * 								StringruntimeScriptMethod,Stringargs)
    * 						通知到Unity，故游戏开发者需要指定一个游戏对象和该对象的运行脚本，用于侦听SDK的初始化结果
    * @param listener      初始化的监听函数，隶属于gameObject对象的运行时脚本的方法名称，该方法会在收到通知后触发
    */
    private void SetInitListener()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.SetInitListener(_gameObjectName, "OnSDKInitResult");
    }

    /**
    *	SetLoginListener方法用于设置登陆监听
    *  @param gameObject	游戏场景中的对象，SDK内部完成登录逻辑后，
    * 						并把登录结果通过Unity的
    * 						API(com.unity3d.player.UnityPlayer.UnitySendMessage(String gameObject,
    * 								StringruntimeScriptMethod,Stringargs)
    * 						通知到Unity，故游戏开发者需要指定一个游戏对象和该对象的运行脚本，用于侦听SDK的登录结果
    * @param listener      登录的监听函数，隶属于gameObject对象的运行时脚本的方法名称，该方法会在收到通知后触发
    */
    private void SetLoginListener()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.SetLoginListener(_gameObjectName, "OnLoginResult");
    }

    /**
    *	SetLoginListener方法用于设置退出监听
    *  @param gameObject	游戏场景中的对象，SDK内部完成退出逻辑后，
    * 						并把登录结果通过Unity的
    * 						API(com.unity3d.player.UnityPlayer.UnitySendMessage(String gameObject,
    * 								StringruntimeScriptMethod,Stringargs)
    * 						通知到Unity，故游戏开发者需要指定一个游戏对象和该对象的运行脚本，用于侦听SDK的退出结果
    * @param listener      登录的监听函数，隶属于gameObject对象的运行时脚本的方法名称，该方法会在收到通知后触发
    */
    private void SetExitListener()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.SetExitListener(_gameObjectName, "OnExitResult");
    }

    /**
    *	SetPayListener方法用于设置计费监听
    *  @param gameObject	游戏场景中的对象，SDK内部完成计费逻辑后，
    * 						并把计费结果通过Unity的
    * 						API(com.unity3d.player.UnityPlayer.UnitySendMessage(String gameObject,
    * 								StringruntimeScriptMethod,Stringargs)
    * 						通知到Unity，故游戏开发者需要指定一个游戏对象和该对象的运行脚本，用于侦听SDK的计费结果
    * @param listener       计费的监听函数，隶属于gameObject对象的运行时脚本的方法名称，该方法会在收到通知后触发
    */
    private void SetPayListener()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        SDKHelper.SetPayListener(_gameObjectName, "OnPayResult");
    }

    #endregion

    #region 游戏内的事件代理
    public delegate void LoginDelegate(bool isSuccess);
    public delegate void LogoutDelegate();
    public delegate void PayDelegate(bool isSuccess, string orderNo);
    public delegate void SwitchUserDelegate(bool needLogout);

    /// <summary>
    /// 登录事件
    /// </summary>
    public static event LoginDelegate loginEvent;
    /// <summary>
    /// 登出事件
    /// </summary>
    public static event LogoutDelegate logoutEvent;
    /// <summary>
    /// 支付回调。orderNo是订单号
    /// </summary>
    public static event PayDelegate payEvent;
    /// <summary>
    /// 切换帐号事件
    /// </summary>
    public static event SwitchUserDelegate switchUserEvent;

    /// <summary>
    /// 登录事件处理
    /// </summary>
    public static void OnLoginEvent(bool isSuccess)
    {
        Debug.Log("AndroidChannelSDKMgr::OnLoginEvent() called!! ");
        if (loginEvent == null)
        {
            Debug.LogError("AndroidChannelSDKMgr::OnLoginEvent-> loginEvent == null, return!!!");
            return;
        }

        loginEvent(isSuccess);
    }

    /// <summary>
    /// 登出事件处理
    /// </summary>
    public static void OnLogOutEvent()
    {
        if (logoutEvent != null)
        {
            logoutEvent();
        }
    }

    /// <summary>
    /// 支付事件处理
    /// </summary>L
    public static void OnPayEvent(bool isSuccess, string orderNo)
    {
        if (payEvent != null)
        {
            payEvent(isSuccess, orderNo);
        }
    }

    /// <summary>
    /// 切换用户事件处理
    /// </summary>
    /// <param name="needLogout"></param>
    public static void OnSwitchUserEvent(bool needLogout)
    {
        if (switchUserEvent != null)
        {
            switchUserEvent(needLogout);
        }
    }

    #endregion

    #region 辅助方法
    /// <summary>
    /// SDK帐户数据信息
    /// </summary>
    public SDKOnlineUser user
    {
        get
        {
            return ChannelUserInfo.Instance.SDKUser;
        }
    }

    public static bool HasLogin()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return false;
        }

        if (SDKHelper == null)
        {
            return false;
        }

        return SDKHelper.mCurSDKPhase > SDKPhase.Login;
    }

    #endregion

}