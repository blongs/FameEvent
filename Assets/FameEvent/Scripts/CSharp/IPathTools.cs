using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IPathTools
{


    public static string GetPlatformFolderName()
    {
        string platform = "";

#if UNITY_ANDROID 
        platform = "Android";
#elif UNITY_IPHONE
            platform = "IOS";
#elif UNITY_STANDALONE_WIN
        platform = "WIN";
#endif
        return platform;
    }

    public static string GetAppFilePath()
    {
        string tmpPath = null;
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
#if BUNDLE_PUBLISH
            tmpPath = Application.persistentDataPath;
#else
            tmpPath = Application.streamingAssetsPath;
#endif
        }
        else
        {
            tmpPath = Application.persistentDataPath;
        }
        return tmpPath;
    }
    public static string GetAssetBundlePath()
    {
        string platFolder = GetPlatformFolderName();

        string allPath = GetAppFilePath()+ "/AssetBundle/" + platFolder;

        return allPath;
    }

    public static string DownLoadAssetBundlePath
    {
        get
        {
            string path = "";
#if !UNITY_EDITOR

#if UNITY_ANDROID
            path = string.Format("{0}", Application.persistentDataPath);
#elif UNITY_IPHONE
            path = string.Format("{0}", Application.temporaryCachePath);
#else
            path = string.Format("{0}", Application.persistentDataPath);
#endif

#else
            path = string.Format("{0}", Application.persistentDataPath);
#endif
            return path + "/AssetBundle";
        }
    }

    public static string GetWWWAssetBundlePath()
    {
        string tmpStr = "";
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            tmpStr = "file://" + GetAssetBundlePath();
        }
        else
        {
            string tmpPath = GetAssetBundlePath();
#if UNITY_ANDROID
            tmpStr = "jar:file://"+tmpPath;
#elif UNITY_STANDALONE_WIN
            tmpStr = "file://" + tmpPath;
#else
            tmpStr = "file://"+tmpPath;
#endif
        }
        return tmpStr;
    }
	
}
