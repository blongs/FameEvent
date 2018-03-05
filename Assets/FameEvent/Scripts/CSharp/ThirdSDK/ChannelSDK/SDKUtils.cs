using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Net;
using System.Text;

public class SDKUtils : MonoBehaviour
{
#if UNITY_IOS
	/**
        *SDK初始化，有些渠道要显式调用SDK的初始化方法
        * @param context      未用到的参数，为与android的接口保持一致，可以传固定的值
        * @param customParams 自定义参数，要求json格式字符串
	*/
	[DllImport("__Internal")]
	private static extern string readApplicationPlistData(string key);

    /*剪切板*/
    [DllImport ("__Internal")]
    private static extern void _copyTextToClipboard(string text);
#endif

    private const string _gameObjectName = "SDKUtils";
    protected static SDKUtils _instance;
    public static SDKUtils instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject(_gameObjectName);
                _instance = obj.AddComponent<SDKUtils>();
            }
            return _instance;
        }
    }

    protected SDKUtils()
    {

    }
    
#if UNITY_ANDROID
    /// <summary>
    /// android插件
    /// </summary>
    private AndroidJavaObject _AndroidPlugin;
#endif

    void Awake()
    {
        if (_instance == null)
        {
            _instance = GetComponent<SDKUtils>();
            gameObject.name = _gameObjectName;
        }

        DontDestroyOnLoad(gameObject);
        init();
    }

    private void init()
    {
#if UNITY_ANDROID
        using (var pluginClass = new AndroidJavaClass(SDKDefine.AndroidUtilsPluginName))
            _AndroidPlugin = pluginClass.CallStatic<AndroidJavaObject>("instance");

        if (_AndroidPlugin == null)
        {
            Debug.LogError("SDKUtils init() _mhjPlugin == null!!!!");
        }
#endif
    }

    /// <summary>
    /// 获取meta data数据
    /// </summary>
    /// <param name="metaKey"></param>
    /// <returns></returns>
    public static object GetMetaData(string metaKey)
    {
        object metaValue = null;
#if UNITY_ANDROID
        if (instance._AndroidPlugin == null)
        {
            Debug.LogError("GetMetaData() instance._mhjPlugin == null!!!!!!!");
            return metaValue;
        }
        else
        {
            metaValue = instance._AndroidPlugin.Call<string>("readApplictionMetaData", metaKey);
        }

#elif UNITY_IOS
		string jsonStr = readApplicationPlistData(metaKey);
		if(string.IsNullOrEmpty(jsonStr))
		{
			return metaValue;
		}

		Dictionary<string, object> jsonDict = MiNiJSON.Json.Deserialize(jsonStr) as Dictionary<string, object>;
		if(jsonDict != null)
		{
			jsonDict.TryGetValue(metaKey, out metaValue);
		}
#else

#endif
        return metaValue;
    }

    public static void DownloadApk(string url)
    {
#if UNITY_ANDROID
        instance._AndroidPlugin.Call("UpdateGame", url);
#endif
    }

    /// <summary>
    /// 拷贝字符到剪切板   
    /// </summary>
    /// <param name="text"></param>
    public static void Str2Clip(string text)
    {
#if UNITY_EDITOR
        TextEditor te = new TextEditor();
        te.content = new GUIContent(text);
        te.OnFocus();
        te.SelectAll();
        te.Copy();
#elif UNITY_ANDROID
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        if (activity==null)
        {
            return;
        }
        instance._AndroidPlugin.Call("copyTextToClipboard",activity, text);
#elif UNITY_IOS
        _copyTextToClipboard(text);
#endif
    }
 

    /// <summary>
    /// 转url地址，base64编码   
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    public static string ToUrlEncode(string strCode)
    {
        StringBuilder sb = new StringBuilder();
        byte[] byStr = System.Text.Encoding.UTF8.GetBytes(strCode); //默认是System.Text.Encoding.Default.GetBytes(str) 
        System.Text.RegularExpressions.Regex regKey = new System.Text.RegularExpressions.Regex("^[A-Za-z0-9]+$");
        for (int i = 0; i < byStr.Length; i++)
        {
            string strBy = Convert.ToChar(byStr[i]).ToString();
            if (regKey.IsMatch(strBy))
            {
                //是字母或者数字则不进行转换  
                sb.Append(strBy);
            }
            else
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }
        }
        return (sb.ToString());
    }
    /// <summary>
    /// 判断是否为模拟器 
    /// </summary>
    /// <returns></returns>
    public static bool isEmulator()
    {
        bool isEmulator = true;
        #if UNITY_EDITOR
            isEmulator = true;
        #elif UNITY_ANDROID
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            if (activity == null)
            {
                isEmulator = true;
            }
            else
            {
                isEmulator = instance._AndroidPlugin.Call<bool>("isEmulator",activity);
            }
#elif UNITY_IOS
            isEmulator = false;
#endif

            return isEmulator;
    }

    public static string GetVersionInfo(string packageName)
    {
#if UNITY_ANDROID
        return instance._AndroidPlugin.Call<string>("GetVersionInfo");
#endif
        return "";
    }

    public static string GetPackagerName()
    {
#if UNITY_ANDROID
        return instance._AndroidPlugin.Call<string>("GetPackagerName");
#endif
        return "";
    }
}