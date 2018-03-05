using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Net;

using MiNiJSON;

namespace OneSDK
{
    public class AndroidMultSDKHelper : ChannelSDKHelperBase
    {
        #region gangaOnlineUnityHelper

#if UNITY_ANDROID 
        /**
         * exit接口用于系统全局退出
         * @param context      上下文Activity
         * @param gameObject   游戏场景中的对象
         * @param listener     退出的监听函数，隶属于gameObject对象的运行时脚本的方法名称，该方法会在收到退出通知后触发
         * */
        [DllImport("gangaOnlineUnityHelper")]
        private static extern void exit(IntPtr context, string gameObject, string listener);

        /**
         * onCreate_listener接口用于初始化回调
         * @param context      上下文Activity
         * @param gameObject   游戏场景中的对象
         * @param listener     退出的监听函数，隶属于gameObject对象的运行时脚本的方法名称，该方法会在收到退出通知后触发
         * */
        [DllImport("gangaOnlineUnityHelper")]
        private static extern void onCreate_listener(IntPtr context, string gameObject, string listener);

        /**
         * login接口用于SDK登陆
         * @param context      上下文Activity
         * @param customParams 自定义参数
         * */
        [DllImport("gangaOnlineUnityHelper")]
        private static extern void login(IntPtr context, string customParams);

        /**
         * logout接口用于SDK登出
         * @param context      上下文Activity
         * @param customParams 自定义参数
         * */
        [DllImport("gangaOnlineUnityHelper")]
        private static extern void logout(IntPtr context, string customParams);

        /**
         * charge接口用于用户触发非定额计费
         * @param context      上下文Activity
         * @param gameObject   游戏场景中的对象
         * @param itemName     虚拟货币名称
         * @param unitPrice    游戏道具单位价格，单位-分
         * @param count        商品或道具数量
         * @param callBackInfo 由游戏开发者定义传入的字符串，会与支付结果一同发送给游戏服务器，
         *                     游戏服务器可通过该字段判断交易的详细内容（金额角色等）
         * @param callBackUrl  将支付结果通知给游戏服务器时的通知地址url，交易结束后，
         * 					   系统会向该url发送http请求，通知交易的结果金额callbackInfo等信息
         * @param payResultListener  支付监听接口，隶属于gameObject对象的运行时脚本的方法名称，该方法会在收到通知后触发
         * */
        [DllImport("gangaOnlineUnityHelper")]
        private static extern void charge(IntPtr context, string gameObject, string itemName, int unitPrice,
                int count, string callBackInfo, string callBackUrl, string payResultListener);

        /**
         * pay接口用于用户触发定额计费
         * @param context      上下文Activity
         * @param gameObject   游戏场景中的对象
         * @param unitPrice    游戏道具单位价格，单位-分
         * @param unitName     虚拟货币名称
         * @param count        商品或道具数量
         * @param callBackInfo 由游戏开发者定义传入的字符串，会与支付结果一同发送给游戏服务器，
         *                     游戏服务器可通过该字段判断交易的详细内容（金额角色等）
         * @param callBackUrl  将支付结果通知给游戏服务器时的通知地址url，交易结束后，
         * 					   系统会向该url发送http请求，通知交易的结果金额callbackInfo等信息
         * @param payResultListener  支付监听接口，隶属于gameObject对象的运行时脚本的方法名称，该方法会在收到通知后触发
         * */
        [DllImport("gangaOnlineUnityHelper")]
        private static extern void pay(IntPtr context, string gameObject, int unitPrice, string unitName,
                int count, string callBackInfo, string callBackUrl, string payResultListener);

        /**
         * payExtend接口用于用户触发定额计费
         * @param context      上下文Activity
         * @param gameObject   游戏场景中的对象
         * @param unitPrice    游戏道具单位价格，单位-分
         * @param unitName     虚拟货币名称
         * @param itemCode     商品ID
         * @param remain       商品自定义参数
         * @param count        商品或道具数量
         * @param callBackInfo 由游戏开发者定义传入的字符串，会与支付结果一同发送给游戏服务器，
         *                     游戏服务器可通过该字段判断交易的详细内容（金额角色等）
         * @param callBackUrl  将支付结果通知给游戏服务器时的通知地址url，交易结束后，
         * 					   系统会向该url发送http请求，通知交易的结果金额callbackInfo等信息
         * @param payResultListener  支付监听接口，隶属于gameObject对象的运行时脚本的方法名称，该方法会在收到通知后触发
         * */
        [DllImport("gangaOnlineUnityHelper")]
        private static extern void payExtend(IntPtr context, string gameObject, int unitPrice, string unitName,
            string itemCode, string remain, int count, string callBackInfo, string callBackUrl, string payResultListener);

        /**
         * 部分渠道如UC渠道，要对游戏人物数据进行统计，而且为接入规范，调用时间：在游戏角色登录成功后调用
         *  public static void setRoleData(Context context, String roleId，
         *  	String roleName, String roleLevel, String zoneId, String zoneName)
         *  
         *  @param context   上下文Activity
         *  @param roleId    角色唯一标识
         *  @param roleName  角色名
         *  @param roleLevel 角色等级
         *  @param zoneId    区域唯一标识
         *  @param zoneName  区域名称
         * */
        //setRoleData接口用于部分渠道如UC渠道，要对游戏人物数据进行统计，接入规范：在游戏角色登录成功后
        [DllImport("gangaOnlineUnityHelper")]
        private static extern void setRoleData(IntPtr context, string roleId,
                string roleName, string roleLevel, string zoneId, string zoneName);

        //备用接口
        [DllImport("gangaOnlineUnityHelper")]
        private static extern void setData(IntPtr context, string key, string value);

        /**
         *	setLoginListener方法用于设置登陆监听
         * 初始化SDK
         *  @param context      上下文Activity
         *  @param gameObject	游戏场景中的对象，SDK内部完成计费逻辑后，
         * 						并把计费结果通过Unity的
         * 						API(com.unity3d.player.UnityPlayer.UnitySendMessage(String gameObject,
         * 								StringruntimeScriptMethod,Stringargs)
         * 						通知到Unity，故游戏开发者需要指定一个游戏对象和该对象的运行脚本，用于侦听SDK的计费结果
         * @param listener      登录的监听函数，隶属于gameObject对象的运行时脚本的方法名称，该方法会在收到通知后触发
         */
        [DllImport("gangaOnlineUnityHelper")]
        private static extern void setLoginListener(IntPtr context, string gameObject, string listener);

        /**
         *	extend扩展接口
         * 扩展接口
         *  @param context      上下文Activity
         * @param data          data
         *  @param gameObject	游戏场景中的对象，
         * @param listener      扩展的监听函数，隶属于gameObject对象的运行时脚本的方法名称，该方法会在收到通知后触发
         */
        [DllImport("gangaOnlineUnityHelper")]
        private static extern void extend(IntPtr context, string data, string gameObject, string listener);

        /**
         *	isMusicEnabled方法用于判断SDK是否需要打开游戏声音，目前只有移动基地需要此接口，
         *  游戏开发者需要根据该返回值，设定游戏背景音乐是否开启
         *
         *  @param context      上下文Activity
         */
        [DllImport("gangaOnlineUnityHelper")]
        private static extern bool isMusicEnabled(IntPtr context);
#endif

        #endregion

        protected AndroidMultSDKHelper()
        {

        }

        // Use this for initialization
        void Awake()
        {
            Debug.Log("AndroidMultSDKHelper Awake().....");
            if (_instance == null)
            {
                _instance = GetComponent<AndroidMultSDKHelper>();
            }

            if (_instance == null)
            {
                Debug.Log("AndroidMultSDKHelper Awake(), _instance == null!!!");
            }

            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {

        }

        #region 外部调用SDK功能统一接口
        /// <summary>
        /// 游戏启动以后初始化SDK
        /// </summary>
        public override void InitSDK(string customParams = "")
        {
            //如果SDK已经初始化过就不需要再初始化了
            if (mCurSDKPhase >= SDKPhase.Init)
            {
                Debug.Log("AndroidMultSDKHelper::InitSDK() isSDKInited == true return!!!");
                return;
            }

            mCurSDKPhase = SDKPhase.Inited;
        }

        /// <summary>
        /// 登录
        /// </summary>
        public override void Login(string customParams = "")
        {
            Debug.Log("AndroidMultSDKHelper::Login() Called!!!");
            if (Application.platform != RuntimePlatform.Android)
            {
                return;
            }

            //如果SDK已经登录成功，就直接走登录成功的流程
            if (mCurSDKPhase == SDKPhase.LoginFinish)
            {
                Debug.Log("AndroidMultSDKHelper::Login() mCurSDKPhase == SDKPhase.LoginFinish, return!!!");
                AndroidChannelSDKMgr.OnLoginEvent(true);
                return;
            }

            //如果SDK还未初始化完成，直接返回
            if (mCurSDKPhase != SDKPhase.Inited)
            {
                Debug.Log("AndroidMultSDKHelper::Login() mCurSDKState != SDKState.Inited, return!!!");
                return;
            }

            mCurSDKPhase = SDKPhase.Login;

#if UNITY_ANDROID
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    login(curActivity.GetRawObject(), "");
                }
            }
#endif
        }

        /// <summary>
        /// 登出
        /// </summary>
        public override void Logout(string customParams = "")
        {
            Debug.Log("AndroidMultSDKHelper::Logout() Called!");
            if (Application.platform != RuntimePlatform.Android)
            {
                return;
            }

            if (mCurSDKPhase <= SDKPhase.Login)
            {
                Debug.Log("AndroidMultSDKHelper::Logout() mCurSDKState <= SDKState.Login, return!!!");
                return;
            }

#if UNITY_ANDROID
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    logout(curActivity.GetRawObject(), "");
                }
            }
#endif
        }

        /// <summary>
        /// 退出
        /// </summary>
        public override void Exit()
        {

            Debug.Log("AndroidMultSDKHelper::Exit() Called!!!");
            if (Application.platform != RuntimePlatform.Android)
            {
                return;
            }

            if (mCurSDKPhase <= SDKPhase.Login)
            {
                Debug.Log("AndroidMultSDKHelper::Exit() mCurSDKState <= SDKState.Login, return!!!");
                return;
            }

#if UNITY_ANDROID
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    exit(curActivity.GetRawObject(), exitListener.gameObjectName, exitListener.listerFuncName);
                }
            }
#endif

        }

        /// <summary>
        /// 定额支付
        /// </summary>
        /// <param name="configId"> 充值配置ID </param>
        public override void Pay(PayInfo payInfo)
        {

            Debug.Log("AndroidMultSDKHelper::Pay() Called!!!");
            if (Application.platform != RuntimePlatform.Android)
            {
                return;
            }

            if (mCurSDKPhase < SDKPhase.LoginFinish)
            {
                Debug.Log("AndroidMultSDKHelper::Pay() mCurSDKState < SDKState.Logined, return!!!");
                return;
            }

#if UNITY_ANDROID
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    pay(curActivity.GetRawObject(), payListener.gameObjectName, payInfo.GetPrice(), payInfo.GetProductName(), payInfo.GetCount(), payInfo.GetOrderId(), payInfo.GetNotify_Url(), payListener.listerFuncName);
                }
            }
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
            
            Debug.Log("AndroidMultSDKHelper::Charge() Called!!!");
            if (Application.platform != RuntimePlatform.Android)
            {
                Debug.Log("AndroidMultSDKHelper::Charge() Application.platform != RuntimePlatform.Android return !!!");
                return;
            }

            if (mCurSDKPhase < SDKPhase.LoginFinish)
            {
                Debug.Log("AndroidMultSDKHelper::Charge() mCurSDKState < SDKState.Logined, return !!!");
                return;
            }

#if UNITY_ANDROID
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    string paySyncUrl = mCurrentUser.getPaySyncUrl();
                    charge(curActivity.GetRawObject(), payListener.gameObjectName, itemName, unitPrice, count, callBackInfo, paySyncUrl, payListener.listerFuncName);
                }
            }
#endif
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
            
            Debug.Log("AndroidMultSDKHelper::SetRoleData() Called!!!");
            if (Application.platform != RuntimePlatform.Android)
            {
                Debug.Log("AndroidMultSDKHelper::SetRoleData() Application.platform != RuntimePlatform.Android, return!!!");
                return;
            }

            if (mCurSDKPhase < SDKPhase.LoginFinish)
            {
                Debug.Log("AndroidMultSDKHelper::SetRoleData() mCurSDKState < SDKState.Logined, return!!!");
                return;
            }

#if UNITY_ANDROID
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    setRoleData(curActivity.GetRawObject(), userInfo.RoleId, userInfo.RoleName, userInfo.RoleLevel.ToString(), userInfo.ZoneId, userInfo.ZoneName);

                    JSONObject gameinfo = new JSONObject();
                    gameinfo.AddField("roleId", userInfo.RoleId);
                    gameinfo.AddField("roleName", userInfo.RoleName);
                    gameinfo.AddField("roleLevel", userInfo.RoleLevel.ToString());
                    gameinfo.AddField("zoneId", userInfo.ZoneId);
                    gameinfo.AddField("zoneName", userInfo.ZoneName);
                    setData(curActivity.GetRawObject(), "gameinfo", gameinfo.ToString());
                }
            }
           
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
            if (Application.platform != RuntimePlatform.Android)
            {
                return;
            }

            //            if (instance.channelType == ChannelType.uc)
            //            {       // uc渠道特殊要求
            //#if UNITY_ANDROID
            //                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            //                {
            //                    using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            //                    {
            //                        string key = "";
            //                        switch (dataType)
            //                        {
            //                            case SendRoleDataType.Create:
            //                                key = "createrole";
            //                                break;
            //                            case SendRoleDataType.LevelUp:
            //                                key = "levelup";
            //                                break;
            //                            case SendRoleDataType.EnterServer:
            //                                key = "enterServer";
            //                                break;
            //                            default: break;
            //                        }
            //                        SFJSONObject roleInfo = new SFJSONObject();
            //                        roleInfo.put("roleId", roleId);
            //                        roleInfo.put("roleName", roleName);
            //                        roleInfo.put("roleLevel", roleLevel);
            //                        roleInfo.put("zoneId", zoneId);
            //                        roleInfo.put("zoneName", zoneName);
            //                        //roleInfo.put("balance", balance);
            //                        //roleInfo.put("vip", vip);
            //                        //roleInfo.put("partyName", partyName);
            //                        roleInfo.put("roleCTime", roleCTime);
            //                        roleInfo.put("roleLevelMTime", roleLevelMTime);
            //                        roleInfo.put("os", "android");

            //                        setData(curActivity.GetRawObject(), key, roleInfo.ToString());
            //                    }
            //                }
            //#endif
            //            }
        }

        #endregion

        #region SDK事件监听器
        /// <summary>
        /// 登录监听
        /// </summary>
        public override void SetLoginListener(string gameObject, string listener)
        {
            
            if (Application.platform != RuntimePlatform.Android)
            {
                return;
            }

            base.SetLoginListener(gameObject, listener);

#if UNITY_ANDROID
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    setLoginListener(curActivity.GetRawObject(), gameObject, listener);
                }
            }
#endif
             
        }
        #endregion

        #region SDK回调处理
        /// <summary>
        /// 登录、登出回调
        /// </summary>
        /// <param name="result"></param>
        public override void OnLoginCallBack(string result)
        {
            
            Debug.Log("AndroidMultSDKHelper::OnLoginResult-> result = " + result);
            var loginData = Json.Deserialize(result) as Dictionary<string, object>;
            if (loginData == null)
            {
                Debug.LogError("AndroidMultSDKHelper::OnLoginCallBack->miniJson == null!!!");
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
                if (mCurSDKPhase == SDKPhase.LoginFinish)
                {
                    //帐号不同就是切换帐号
                    SwitchUser(userinfo, true);
                }
                else
                {
                    OnLoginSuccess(userinfo);
                }
            }
            else if (SDKDefine.LoginResult.LOGIN_SUCCESS == type)
            {
                //SDK登录成功
                var userinfo = loginData["userinfo"] as Dictionary<string, object>;
                OnLoginSuccess(userinfo);
            }
            else if (SDKDefine.LoginResult.LOGIN_FAILED == type)
            {
                //SDK登录失败
                OnLoginFailed();
            }
    
        }

        /// <summary>
        /// 退出监听函数
        /// </summary>
        /// <param name="result"></param>
        public override void OnExitCallBack(string result)
        {
            
            Debug.Log("AndroidMultSDKHelper::OnExitCallBack-> result=" + result);
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
            Debug.Log("AndroidMultSDKHelper::OnPayCallBack-> result=" + result);
            //SFJSONObject sfjson = new SFJSONObject(result);
            //string type = (string)sfjson.get("result");
            //string data = (string)sfjson.get("data");

            //if (SDKDefine.PayResult.PAY_SUCCESS == type)
            //{
            //    //str = "pay result = pay success " + data;
            //    AndroidChannelSDKMgr.OnPayEvent(true, _lastOrderNo);
            //}
            //else if (SDKDefine.PayResult.PAY_FAILURE == type)
            //{
            //    //str = "pay result = pay failure" + data;
            //    AndroidChannelSDKMgr.OnPayEvent(false, _lastOrderNo);
            //}
            //else if (SDKDefine.PayResult.PAY_ORDER_NO == type)
            //{
            //    //str = "pay result = pay order No" + data;
            //    _lastOrderNo = data;
            //}
        }

        //帐号切换
        private void SwitchUser(Dictionary<string, object> userinfo, bool needLogout)
        {

            //登录用户信息更新
            mCurrentUser.UpdateData(userinfo);

            //不需要验证直接调用切换成功
            OnSwitchUserSuccess(needLogout);
        }

        //帐号切换成功的回调
        private void OnSwitchUserSuccess(bool needLogout)
        {
            Debug.Log("AndroidSingleSDKHelper::OnSwitchUserSuccess() Called!!");
            mCurSDKPhase = SDKPhase.LoginFinish;
            AndroidChannelSDKMgr.OnSwitchUserEvent(needLogout);
        }

        #endregion

        #region SDK登录结果处理以及登录验证
        /// <summary>
        /// 登录验证
        /// </summary>
        private IEnumerator LoginCheck()
        {

            Debug.Log("AndroidMultSDKHelper::LoginCheck() Called!!");
            mCurSDKPhase = SDKPhase.LoginCheck;

            //生成登录验证Url地址
            string url = CreateLoginCheckURL();

            //如果登录验证的url为空,直接走登录验证失败的流程
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("AndroidMultSDKHelper::LoginCheck-> url = null !!");
               // UICommonManager.GetInstance().bubbleLoopTipsControl.ShowTips("登录验证失败");
                OnLoginCheckFailed();
                yield break;
            }
            Debug.Log("AndroidMultSDKHelper::LoginCheck-> url = " + url);

            //WWW请求登录验证结果
            WWW www = new WWW(url);
            yield return www;

            string result = "";
            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                result = www.text;
            }
            Debug.Log("AndroidMultSDKHelper::LoginCheck-> result = " + result);

            //解析登录验证返回的信息
            var respondDict = Json.Deserialize(result) as Dictionary<string, object>;
            if (respondDict == null)
            {
                //如果登录验证返回的信息为空,直接走登录验证失败的流程
                Debug.LogError("AndroidMultSDKHelper::LoginCheck-> respondDict == null!!!");
               // UICommonManager.GetInstance().bubbleLoopTipsControl.ShowTips("登录验证失败");
                OnLoginCheckFailed();
               // OnLoginCheckSuccess(null); 
                yield break;
            }

            string type = (string)respondDict["code"];
            if (type == null || !(type == "1"))
            {
                //如果登录验证返回的登录验证的标志不是SUCCESS,直接走登录验证失败的流程
                Debug.LogError("AndroidMultSDKHelper::LoginCheck-> respondDict[result] != SUCCESS!!!");
               // UICommonManager.GetInstance().bubbleLoopTipsControl.ShowTips("登录验证失败");
               OnLoginCheckFailed();
               // OnLoginCheckSuccess(null);
                yield break;
            }

            //登录验证成功处理
            OnLoginCheckSuccess(null);

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
            if(true) //(GameManager.Instance.isTestServer)
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
            
            if (userinfo == null)
            {
                OnLoginFailed();
                return;
            }

            mCurrentUser.UpdateData(userinfo);
            Debug.Log("AndroidMultSDKHelper::OnLoginSuccess-> SFOnlineUser = " + mCurrentUser.ToString());

            //登录验证
            StartCoroutine(LoginCheck());
            
        }

        /// <summary>
        /// SDK登录失败后的处理
        /// </summary>
        private void OnLoginFailed()
        {
            mCurrentUser.Reset();
            mCurSDKPhase = SDKPhase.Inited;
            AndroidChannelSDKMgr.OnLoginEvent(false);
            Login();
        }

        /// <summary>
        /// SDK登录验证成功后的处理
        /// </summary>
        /// <param name="userInfoDict"></param>
        private void OnLoginCheckSuccess(Dictionary<string, object> userInfoDict)
        {
            
            if (mCurSDKPhase != SDKPhase.LoginCheck)
            {
                OnLoginFailed();
                return;
            }

            mCurSDKPhase = SDKPhase.LoginFinish;
            mCurrentUser.UpdateData(userInfoDict);
            Debug.Log("AndroidMultSDKHelper::OnLoginCheckSuccess-> user = " + mCurrentUser.ToString());

            AndroidChannelSDKMgr.OnLoginEvent(true);
             
        }

        /// <summary>
        /// SDK登录验证失败后的处理
        /// </summary>
        private void OnLoginCheckFailed()
        {
            mCurrentUser.Reset();
            mCurSDKPhase = SDKPhase.Inited;
            AndroidChannelSDKMgr.OnLoginEvent(false);
            Login();
        }

        /// <summary>
        /// SDK登出处理
        /// </summary>
        private void OnLogout()
        {
            mCurrentUser.Reset();
            mCurSDKPhase = SDKPhase.Inited;
            AndroidChannelSDKMgr.OnLogOutEvent();
            Login();
        }
        #endregion
    }
}