using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net;

public class ChannelSDKHelperBase : MonoBehaviour
{ 
    /// <summary>
    /// 当前SDK的状态
    /// </summary>
    public SDKPhase mCurSDKPhase = SDKPhase.None;

    protected static ChannelSDKHelperBase _instance;

    protected ChannelSDKHelperBase()
    {

    }

    // Use this for initialization
    void Awake()
    {
        
    }

#region 外部调用SDK功能统一接口
    /// <summary>
    /// 游戏启动以后初始化SDK
    /// </summary>
    public virtual void InitSDK(string customParams = "")
    {

    }

    /// <summary>
    /// 登录
    /// </summary>
    public virtual void Login(string customParams = "")
    {
        
    }

    /// <summary>
    /// 应用宝登录
    /// </summary>
    public virtual void LoginForTencent(string platform) { 
    
    }

    /// <summary>
    /// 登出
    /// </summary>
    public virtual void Logout(string customParams = "")
    {
        
    }

    /// <summary>
    /// 退出
    /// </summary>
    public virtual void Exit()
    {
        
    }

    /// <summary>
    ///  定额支付
    /// </summary>
    /// <param name="configId"> 充值配置ID </param>
    public virtual void Pay(PayInfo payInfo)
    {

    }

    /// <summary>
    /// 获取支付时需要的相关参数
    /// </summary>
    public virtual string GetPayInfo() {
        return "";
    }

    /// <summary>
    /// 非定额计费接口
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="unitPrice"></param>
    /// <param name="count"></param>
    /// <param name="callBackInfo"></param>
    public virtual void Charge(string itemName, int unitPrice, int count, string callBackInfo)
    {

    }
    
    /// <summary>
    /// 设置角色数据（登录成功后调用）
    /// </summary>
    /// <param name="roleId">角色唯一标识</param>
    /// <param name="roleName">角色名</param>
    /// <param name="roleLevel">角色等级</param>
    /// <param name="zoneId">区域唯一标识</param>
    /// <param name="zoneName">区域名称</param>
    public virtual void SetRoleData(ChannelUserInfo userInfo, long createTime)
    {

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
    public virtual void SendRoleData(SDKDefine.SendRoleDataType dataType, int roleId, string roleName, int roleLevel, int zoneId, string zoneName, ulong roleCTime, ulong roleLevelMTime,
        int balance = 0, int vip = 1, string partyName = "无帮派")
    {

    }

    /// <summary>
    /// IOS 爱思渠道需要显示用户中心
    /// <param name="data"> 传入的参数, json格式 </param>
    /// </summary>
    public virtual void EnterUserCenter(string data)
    {

    }

#endregion

#region SDK事件监听器
    private SDKDefine.SDKListener _initListener;
    public SDKDefine.SDKListener initListener
    {
        get
        {
            if (_initListener == null)
            {
                _initListener = new SDKDefine.SDKListener();
            }
            return _initListener;
        }
    }

    /// <summary>
    /// 登录监听
    /// </summary>
    private SDKDefine.SDKListener _loginListener;
    public SDKDefine.SDKListener loginListener
    {
        get
        {
            if (_loginListener == null)
            {
                _loginListener = new SDKDefine.SDKListener();
            }
            return _loginListener;
        }
    }

    /// <summary>
    /// 退出监听
    /// </summary>
    private SDKDefine.SDKListener _exitListener;
    public SDKDefine.SDKListener exitListener
    {
        get
        {
            if (_exitListener == null)
            {
                _exitListener = new SDKDefine.SDKListener();
            }
            return _exitListener;
        }
    }

    /// <summary>
    /// 计费监听
    /// </summary>
    private SDKDefine.SDKListener _payListener;
    public SDKDefine.SDKListener payListener
    {
        get
        {
            if (_payListener == null)
            {
                _payListener = new SDKDefine.SDKListener();
            }
            return _payListener;
        }
    }

   /**
   * SetInitListener方法用于设置初始化监听
   *  @param gameObject	游戏场景中的对象，SDK内部完成登录逻辑后，
   * 						并把登录结果通过Unity的
   * 						API(com.unity3d.player.UnityPlayer.UnitySendMessage(String gameObject,
   * 								StringruntimeScriptMethod,Stringargs)
   * 						通知到Unity，故游戏开发者需要指定一个游戏对象和该对象的运行脚本，用于侦听SDK的初始化结果
   * @param listener      初始化的监听函数，隶属于gameObject对象的运行时脚本的方法名称，该方法会在收到通知后触发
   */
    public virtual void SetInitListener(string gameObject, string listener)
    {
        initListener.gameObjectName = gameObject;
        initListener.listerFuncName = listener;
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
    public virtual void SetLoginListener(string gameObject, string listener)
    {
        loginListener.gameObjectName = gameObject;
        loginListener.listerFuncName = listener;
    }

    /**
    *	SetExitListener方法用于设置退出监听
    *  @param gameObject	游戏场景中的对象，SDK内部完成退出逻辑后，
    * 						并把登录结果通过Unity的
    * 						API(com.unity3d.player.UnityPlayer.UnitySendMessage(String gameObject,
    * 								StringruntimeScriptMethod,Stringargs)
    * 						通知到Unity，故游戏开发者需要指定一个游戏对象和该对象的运行脚本，用于侦听SDK的退出结果
    * @param listener      登录的监听函数，隶属于gameObject对象的运行时脚本的方法名称，该方法会在收到通知后触发
    */
    public virtual void SetExitListener(string gameObject, string listener)
    {
        exitListener.gameObjectName = gameObject;
        exitListener.listerFuncName = listener;
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
    public virtual void SetPayListener(string gameObject, string listener)
    {
        payListener.gameObjectName = gameObject;
        payListener.listerFuncName = listener;
    }
#endregion

#region SDK事件回调

    /// <summary>
    /// 初始化结果回调
    /// </summary>
    /// <param name="result"></param>
    public virtual void OnInitCallBack(string result)
    {

    }

    /// <summary>
    /// 登录、登出回调
    /// </summary>
    /// <param name="result"></param>
    public virtual void OnLoginCallBack(string result)
    {

    }

    /// <summary>
    /// 登录(for 应用宝)
    /// </summary>
    /// <param name="result"></param>
    public virtual void OnLoginForTencentCallBack(string result)
    { 
    
    }

    /// <summary>
    /// 退出监听函数
    /// </summary>
    /// <param name="result"></param>
    public virtual void OnExitCallBack(string result)
    {

    }

    /// <summary>
    /// 支付监听函数
    /// </summary>
    /// <param name="result"></param>
    public virtual void OnPayCallBack(string result)
    {
        
    }
        
#endregion

#region 成员变量
    /// <summary>
    /// SDK帐户数据信息
    /// </summary>
    public SDKOnlineUser mCurrentUser
    {
        get
        {
            return ChannelUserInfo.Instance.SDKUser;
        }
        set
        {
            ChannelUserInfo.Instance.SDKUser = value;
        }
    }

    /// <summary>
    /// 最后一次的定单号
    /// </summary>
    private string _lastOrderNo;

#endregion

}