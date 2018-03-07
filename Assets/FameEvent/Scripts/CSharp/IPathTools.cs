using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
/*
   ----------------------------------------------------
   Unity3D中目录说明：
   streamingAssetsPath （只读目录）
        pc     可以通过www和stream读取，注意win平台www协议需要三个/即file://，但试验中貌似两个//也行。
        android    只能通过www读取，因为该文件夹在jar包内部，协议是jar:file:// ，Application.streamingAssetsPath返回的路径会自动添加该协议。

   persistentDataPath（读写目录）   

        pc     可通过www读取，stream读写，注意win平台www协议需要三个/即file:///，但试验中貌似两个//也行。
        android     www读取，stream读写，注意，www时Application.persistentDataPath返回的路径不会自动添加协议，协议是file://而不是jar:// 因为导出包是选择SD卡，该目录会在SD卡中暴露出来，无需root权限，属于本地文件，因此需要本地文件协议file://读取。
   ----------------------------------------------------
*/

/// <summary>
/// 
/// </summary>
public class IPathTools
{
    static string StreamingAssetsPath
    {
        get
        {
            string path = "";
#if UNITY_EDITOR
            path = Application.dataPath + "/StreamingAssets";
#elif UNITY_ANDROID && !UNITY_EDITOR
            path = "jar:file://" + Application.dataPath + "!/assets";
#elif UNITY_IPHONE && !UNITY_EDITOR
            path =  Application.dataPath + "/Raw";
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
            path = Application.dataPath + "/StreamingAssets";
#endif
            return path;
        }
    }

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
#if UNITY_EDITOR
#if USE_ASSETBUNDLE
            tmpPath = Application.persistentDataPath;
#else 
        tmpPath = StreamingAssetsPath;
#endif
#endif

#if !UNITY_EDITOR && UNITY_ANDROID
#if BUNDLE_PUBLISH
            tmpPath = Application.persistentDataPath;
#else
        tmpPath = StreamingAssetsPath;
#endif
#endif
        return tmpPath;
    }
    public static string GetAssetBundlePath()
    {
        string platFolder = GetPlatformFolderName();

        string allPath = GetAppFilePath() + "/AssetBundle/" + platFolder;

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
#if UNITY_EDITOR
        tmpStr = "file://" + GetAssetBundlePath();
#elif UNITY_ANDROID && !UNITY_EDITOR
        string tmpPath = GetAssetBundlePath();
#if USE_ASSETBUNDLE
            tmpStr = "file://" + tmpPath;   
#else
            tmpStr = tmpPath;
#endif
#endif
        return tmpStr;
    }

}
