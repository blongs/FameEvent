using UnityEngine;
using System.Collections;

public class JPushSDKHelper : MonoBehaviour {

    private static JPushSDKHelper _instance = null;
    private static string _gameObjectName = "JPushSDKHelper";
    private AndroidJavaObject _SDKHelperPlugin;
    public static JPushSDKHelper GetInstance()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject();
            if (obj != null)
            {
                _instance = obj.AddComponent<JPushSDKHelper>();
                obj.name = JPushSDKHelper._gameObjectName;
            }
        }

        return _instance;
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = GetComponent<JPushSDKHelper>();
            DontDestroyOnLoad(gameObject);
        }

#if UNITY_ANDROID
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        using (var pluginClass = new AndroidJavaClass(SDKDefine.AndroidJPushPluginName))
            if (pluginClass != null)
            {
                _SDKHelperPlugin = pluginClass.CallStatic<AndroidJavaObject>("Instance");
            }

        if (_SDKHelperPlugin == null)
        {
            Debug.LogError("JPushSDKHelper Init() _SDKHelperPlugin == null!!!!");
            return;
        }
#endif
    }

    public void InitSDK()
    {
#if UNITY_ANDROID
        _SDKHelperPlugin.Call("Init");
#endif
    }

    public void SetTags(string value)
    {
#if UNITY_ANDROID
        _SDKHelperPlugin.Call("SetTags", value);
#endif
    }

    public string GetJPushId()
    {
#if UNITY_ANDROID
        return _SDKHelperPlugin.Call<string>("GetJPushId");
#else
        return ""
#endif
    }


}
