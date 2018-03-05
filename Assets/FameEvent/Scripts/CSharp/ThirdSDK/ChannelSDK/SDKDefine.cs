//SDK状态
public enum SDKPhase
{
    None,           //
    Init,           //初始化状态
    Inited,         //初始化完成
    Login,          //登录状态
    LoginCheck,     //登录验证状态
    LoginFinish     //登录完成状态
};

public class SDKDefine
{
    //易接默认的 channelid
    public static AndroidMetaInfo OneSDKDefaultChannelIDMeta = new AndroidMetaInfo("com.snowfish.channelid", "{4ff036a1-3254eafe}");

    //自己对接的SDK的默认的channelid
    public static AndroidMetaInfo MHJSDKDefaultChannelIDMeta = new AndroidMetaInfo("com.androidplugins.channelid", "{0000-0000}");

    //安卓子渠道
    public static AndroidMetaInfo AndroidSubChannelMeta = new AndroidMetaInfo("com.reyun.CKEY", "default");

    //安卓游戏退出方式
    public static AndroidMetaInfo AndroidExitTypeMeta = new AndroidMetaInfo("exittype", "game");

    //安卓辅助工具插件的Java类名
    public const string AndroidUtilsPluginName = "com.androidplugins.functions.AndroidPluginHelper";

    //安卓渠道SDK插件的Java类名
    public const string AndroidChannelPluginName = "com.androidplugins.functions.SDKHelper";

    //安卓极光推送插件的Java类名
    public const string AndroidJPushPluginName = "com.androidplugins.functions.JPushHelper";

    //热云Android SDK插件的Java类名
    public const string AndroidReYunPluginName = "com.androidplugins.functions.RYSDKHelper";

    //SharedAndroid SDK插件的Java类名
   // public const string AndroidSharedPluginName = "com.androidplugins.functions.MainActivity";

    protected SDKDefine()
    {

    }

    /// <summary>
    /// 安卓渠道meta-data数据保存结构
    /// </summary>
    public class AndroidMetaInfo
    {
        public AndroidMetaInfo(string k, string v)
        {
            key = k;
            value = v;
        }

        public string key;
        public string value;
    }

    /**
        * Result of paying.
    */
    public class PayResult
    {
        /** pay success */
        public const string PAY_SUCCESS = "0";

        /** pay failure*/
        public const string PAY_FAILURE = "1";

        /** pay orderNo*/
        public const string PAY_ORDER_NO = "2";
    }

    public class LoginResult
    {
        /** logout*/
        public const string LOGOUT = "0";

        /** login success */
        public const string LOGIN_SUCCESS = "1";

        /** login failed*/
        public const string LOGIN_FAILED = "2";

        public const string LOGOUT_TENCENT = "3";
    }

    public class ExitResult
    {
        /** exit success*/
        public const string SDKEXIT = "0";

        /**No Exiter Provide*/
        public const string SDKEXIT_NO_PROVIDE = "1";
    }

    public class SDKListener
    {
        public SDKListener()
        {
            gameObjectName = "";
            listerFuncName = "";
        }

        public string gameObjectName;
        public string listerFuncName;
    }

    /// <summary>
    /// 发送数据类型
    /// </summary>
    public enum SendRoleDataType
    {
        Create,
        LevelUp,
        EnterServer
    }
}