using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class ReYunSDKHelper : MonoBehaviour
{
#if UNITY_ANDROID
    /// <summary>
    /// android插件
    /// </summary>
    private AndroidJavaObject _SDKHelperPlugin;

#elif UNITY_IOS

//    [DllImport("__Internal")]
//	private static extern void ReYun_Instance();

    [DllImport("__Internal")]
	private static extern void ReYun_Init(string channelId);

    [DllImport("__Internal")]
	private static extern void ReYun_FirstIn(string jsonStr);

    [DllImport("__Internal")]
	private static extern void ReYun_LoginSuccess(string jsonStr);

    [DllImport("__Internal")]
	private static extern void ReYun_StartPay(string jsonStr);

    [DllImport("__Internal")]
	private static extern void ReYun_PaySuccess(string jsonStr);

    [DllImport("__Internal")]
	private static extern void ReYun_Consume(string jsonStr);

    [DllImport("__Internal")]
	private static extern void ReYun_SetQuest(string jsonStr);

    [DllImport("__Internal")]
	private static extern void ReYun_SetCustomEvent(string jsonStr);

//    [DllImport("__Internal")]
//	private static extern void ReYun_Exit();    
#endif

    private static ReYunSDKHelper _instance = null;
    private static string _gameObjectName = "ReYunSDKHelper";

    public static ReYunSDKHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject();
                if (obj != null)
                {
                    _instance = obj.AddComponent<ReYunSDKHelper>();
                    obj.name = ReYunSDKHelper._gameObjectName;
                }
            }
            return _instance;
        }

    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = GetComponent<ReYunSDKHelper>();
            DontDestroyOnLoad(gameObject);
        }

#if UNITY_ANDROID
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        using (var pluginClass = new AndroidJavaClass(SDKDefine.AndroidReYunPluginName))
            if (pluginClass != null)
            {
                _SDKHelperPlugin = pluginClass.CallStatic<AndroidJavaObject>("Instance");
            }

        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("ReYunSDKHelper Init() _SDKHelperPlugin == null!!!!");
            return;
        }
#elif UNITY_IOS
//        if (Application.platform != RuntimePlatform.IPhonePlayer)
//        {
//            return;
//        }
//
//        ReYun_Instance();
#else

#endif
    }

    void Start()
    {
        // InitSDK();
    }

    public void Init()
    {

    }

    private Dictionary<string, string> eventDic = new Dictionary<string, string>();
    private string eventName = "";
    public int count;
    private Stack<string> keyNames = new Stack<string>();


    public void AddEventDic(string key, string value)
    {
        if (!eventDic.ContainsKey(key))
        {
            eventDic.Add(key, value);
            keyNames.Push(key);
            //DebugDic();
        }
    }

    public void DeletCurrentEvenDic()
    {
        string currentKey = keyNames.Pop();
        if (eventDic.ContainsKey(currentKey))
        {
            eventDic.Remove(currentKey);
            // DebugDic();
        }
    }

    private void DebugDic()
    {
        foreach (var item in eventDic)
        {
            Debug.LogError("key = " + item.Key + ",value = " + item.Value);
        }
    }

    public void SetEventName(string _eventName)
    {
        eventName = _eventName;
        // Debug.LogError("eventName = " + eventName);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void InitSDK(bool isNeedGame, string gameKey, bool isNeedTracking, string trackingKey, string channelId)
    {
#if UNITY_ANDROID
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        using (var pluginClass = new AndroidJavaClass(SDKDefine.AndroidReYunPluginName))
            if (pluginClass != null)
            {
                _SDKHelperPlugin = pluginClass.CallStatic<AndroidJavaObject>("Instance");
            }

        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("ReYunSDKHelper InitSDK() _SDKHelperPlugin == null!!!!");
            return;
        }

        _SDKHelperPlugin.Call("init",isNeedGame.ToString(), gameKey,isNeedTracking.ToString(), trackingKey, channelId);
        Debug.Log("热云SDK初始化成功...");
#elif UNITY_IOS
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            return;
        }

        ReYun_Init(Boot.GetInstance().SDKUser.getSubChannel());
#else

#endif
    }

    /// <summary>
    /// 首次进入服务器统计用户注册数据
    /// </summary>
    /// <param name="data"></param>
    public void FirstIn(string data)
    {
        if (data == null)
        {
            Debug.LogError("ReYunSDKHelper FirstIn() data == null!!!!");
            return;
        }
#if UNITY_ANDROID
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("ReYunSDKHelper FirstIn() _SDKHelperPlugin == null!!!!");
            return;
        }
        _SDKHelperPlugin.Call("firstIN", data);

#elif UNITY_IOS
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            return;
        }

		//ReYun_FirstIn(jsonStr);
#else

#endif
    }

    /// <summary>
    /// 每次进入服务器统计用户登陆数据
    /// </summary>
    /// <param name="data"></param>
    public void LoginSuccess(string data)
    {
        if (data == null)
        {
            Debug.LogError("ReYunSDKHelper LoginSuccess() data == null!!!!");
            return;
        }

#if UNITY_ANDROID
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("ReYunSDKHelper LoginSuccess() _SDKHelperPlugin == null!!!!");
            return;
        }
        _SDKHelperPlugin.Call("loginSuccess", data);

#elif UNITY_IOS
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            return;
        }

      //  ReYun_LoginSuccess(jsonStr);
#else

#endif
    }

    /// <summary>
    /// 统计用户开始充值数据
    /// </summary>
    /// <param name="data"></param>
    public void StartPay(PayInfo payInfo)
    {
        if (payInfo == null)
        {
            Debug.LogError("ReYunSDKHelper StartPay() data == null!!!!");
            return;
        }

       // string jsonStr = MiNiJSON.Json.Serialize(data);
#if UNITY_ANDROID
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("ReYunSDKHelper StartPay() _SDKHelperPlugin == null!!!!");
            return;
        }
        _SDKHelperPlugin.Call("startPay", payInfo.GetReYunPayStartStr());

#elif UNITY_IOS
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            return;
        }

       // ReYun_StartPay(jsonStr);
#else

#endif
    }

    /// <summary>
    /// 统计用户的成功充值数据
    /// </summary>
    /// <param name="data"></param>
    public void PaySucceess(PayInfo payInfo)
    {
        if (payInfo.GetPayInfoStr() == null)
        {
            Debug.LogError("ReYunSDKHelper PaySucceess() data == null!!!!");
            return;
        }

#if UNITY_ANDROID
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("ReYunSDKHelper PaySucceess() _SDKHelperPlugin == null!!!!");
            return;
        }
        _SDKHelperPlugin.Call("paySuccess", payInfo.GetReYunPayStartStr());

#elif UNITY_IOS
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            return;
        }

       // ReYun_PaySuccess(jsonStr);
#else

#endif
    }

    /// <summary>
    /// 统计用户在应用内的虚拟交易数据
    /// </summary>
    /// <param name="data"></param>
    public void Consume(Dictionary<string, string> data)
    {
        if (data == null)
        {
            Debug.LogError("ReYunSDKHelper Consume() data == null!!!!");
            return;
        }
        string jsonStr = MiNiJSON.Json.Serialize(data);

#if UNITY_ANDROID
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("ReYunSDKHelper Consume() _SDKHelperPlugin == null!!!!");
            return;
        }
        _SDKHelperPlugin.Call("cost", jsonStr);

#elif UNITY_IOS
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            return;
        }

        ReYun_Consume(jsonStr);
#else

#endif
    }

    /// <summary>
    /// 统计用户的任务、副本数据
    /// </summary>
    /// <param name="data"></param>
    public void SetQuest(Dictionary<string, string> data)
    {
        if (data == null)
        {
            Debug.LogError("ReYunSDKHelper SetQuest() data == null!!!!");
            return;
        }
        string jsonStr = MiNiJSON.Json.Serialize(data);

#if UNITY_ANDROID
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("ReYunSDKHelper SetQuest() _SDKHelperPlugin == null!!!!");
            return;
        }
        _SDKHelperPlugin.Call("setQuest", jsonStr);

#elif UNITY_IOS
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            return;
        }

        ReYun_SetQuest(jsonStr);
#else

#endif
    }

    /// <summary>
    /// 统计用户的自定义事件
    /// </summary>
    /// <param name="data"></param>
    public void SetCustomEvent()
    {
        AddEventDic("Count", count + "");
        DebugDic();
        string jsonStr = MiNiJSON.Json.Serialize(eventDic);
        Debug.LogError("ReYunSDKHelper SetCustomEvent() eventName = "+eventName+" ,MiNiJSON =" + jsonStr);
#if UNITY_ANDROID



        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("ReYunSDKHelper SetCustomEvent() _SDKHelperPlugin == null!!!!");
            return;
        }


        _SDKHelperPlugin.Call("setCustomEvent", eventName, jsonStr);
#elif UNITY_IOS
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			return;
		}
		
		ReYun_SetCustomEvent(jsonStr);
#else
#endif
    }

    /// <summary>
    /// 退出sdk
    /// </summary>
    public void ExitSDK()
    {
#if UNITY_ANDROID
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("ReYunSDKHelper FirstIn() _SDKHelperPlugin == null!!!!");
            return;
        }

        _SDKHelperPlugin.Call("exitSDK");
#endif
    }

    public void OnApplicationQuit()
    {
        ExitSDK();
    }
}
