using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

using System.Diagnostics;


public class QuickPublishHelper
{

    private static bool isAPK = false;

    /*---------------------------------------------------------------------------------------------------------------------
    *  Android sdk 混合
    *---------------------------------------------------------------------------------------------------------------------*/

    // 混合android sdk
    [MenuItem("SDK-Publish/混合SDK/易接版", false, 1)]
    public static void MergeSDKPluginYj()
    {
        BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        string symbolsStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        symbolsStr = string.IsNullOrEmpty(symbolsStr) ? symbolsStr : symbolsStr + ";";


        //先去掉旧的再在最后加上新的，这样处理比较简单
        if (symbolsStr.LastIndexOf("ONESDK_PUBLISH;") > -1)
        {
            symbolsStr = symbolsStr.Replace("ONESDK_PUBLISH;", "");
        }

        if (symbolsStr.LastIndexOf("ONESDK_PUBLISH") > -1)
        {
            symbolsStr = symbolsStr.Replace("ONESDK_PUBLISH", "");
        }

        symbolsStr = symbolsStr + "ONESDK_PUBLISH;";
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, symbolsStr);

#if UNITY_ANDROID
        GenerateSDKPluginYj();
#endif
        UnityEngine.Debug.Log("易接版SDK混合完成!");
    }

    [MenuItem("SDK-Publish/混合SDK/标准版", false, 1)]
    public static void MergeSDKPluginUnityNoRelace()
    {
        GenerateSDKPluginUnity();
    }



    public static void GenerateSDKPluginYj()
    {
        List<string> pluginList = new List<string>() { "ToolsAndroid", "YiJieAndroid", "ReYunAndroid" };
        isAPK = true;
        AndroidPluginEditor.GenerateSDKPlugin("(易接版)", "BaseManifest_YJ", pluginList);
    }

    public static void GenerateSDKPluginUnity()
    {
        List<string> pluginList = new List<string>() { "ToolsAndroid", "ReYunAndroid" };
        isAPK = false;
        AndroidPluginEditor.GenerateSDKPlugin("(标准版)", "BaseManifest_Unity", pluginList);

    }

    static public void StartBuildAPK(bool isApk)
    {
        //设置保存目录
        string path = GetApkSavePath(isApk);
        if (path == null || path == "")
        {
            return;
        }

#if UNITY_ANDROID
        //设置证书信息
        if (!SetAPKKeyStoreInfo())
        {
            return;
        }
#endif

        UnityEngine.Debug.Log("APK Path = " + path);
        if (isAPK)
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            BuildPipeline.BuildPlayer(GetBuildScenes(), path, target, BuildOptions.None);
        }
        else
        {
            BuildPipeline.BuildPlayer(GetBuildScenes(), path, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
        }
        if (File.Exists(path))
        {
            string msg = "APK包已生成,保存路径：" + path;
            EditorUtility.DisplayDialog("APK打包完成", msg, "关闭");
        }
    }

    static private string GetApkSavePath(bool isApk)
    {
        //设置保存目录
        System.DateTime nowTime = System.DateTime.Now;
        string apkName = "";

        apkName = string.Format("KYDW-{0:yyMMddHHmm}-develop", nowTime);

#if UNITY_ANDROID
        if (isApk)
        {
            apkName = apkName + ".apk";
        }

#endif

        string path = CreatBuildDefaultSaveDir(isApk);
        if (isApk)
        {
            //一键打包APK设置默认保存目录
            // path = CreatBuildDefaultSaveDir();
            if (path == null || path == "")
            {
                return "";
            }
            path = path + Path.DirectorySeparatorChar + apkName;
        }
        return path;
    }

    static private string CreatBuildDefaultSaveDir(bool isApk)
    {
        string dir = Application.dataPath;
        int index = dir.LastIndexOf(Path.DirectorySeparatorChar);
        if (index >= 0)
        {
            UnityEngine.Debug.Log("index = " + index);
            dir = dir.Substring(0, index);
        }
        dir = dir.Replace("/Assets", "");
        if (isApk)
        {
            dir = dir + Path.DirectorySeparatorChar + "ApkS";
        }
        else
        {
            dir = dir + Path.DirectorySeparatorChar + "AndroidProjects";
            if (Directory.Exists(dir))
            {
                DeletFolder(dir);
            }
        }
        if (!Directory.Exists(dir))
        {
            DirectoryInfo dirInfo = Directory.CreateDirectory(dir);
            if (dirInfo == null)
            {
                UnityEngine.Debug.LogError("Create Directory Failed! Directory:" + dir);
                return null;
            }
        }
        UnityEngine.Debug.Log("dir = " + dir);
        return dir;
    }

    private static void DeletFolder(string _dir)
    {
        try
        {
            DirectoryInfo dir = new DirectoryInfo(_dir);
            FileSystemInfo[] fileInfos = dir.GetFileSystemInfos();
            foreach (FileSystemInfo i in fileInfos)
            {
                if (i is DirectoryInfo)
                {
                    DirectoryInfo subDir = new DirectoryInfo(i.FullName);
                    subDir.Delete(true);
                }
                else
                {
                    File.Delete(i.FullName);
                }
            }
        }
        catch (Exception e)
        {

        }
    }

#if UNITY_ANDROID
    [MenuItem("SDK-Publish/设置打包签名", false, 2)]
    public static bool SetAPKKeyStoreInfo()
    {
        if (string.IsNullOrEmpty(PlayerSettings.Android.keystoreName))
        {
            EditorUtility.DisplayDialog("APK打包", "请先选择证书和设置证书信息", "关闭");
            UnityEngine.Debug.Log("设置打包签名失败!");
            return false;
        }

        PlayerSettings.Android.keystorePass = "123456";
        PlayerSettings.Android.keyaliasName = "zhizhundianwan";
        PlayerSettings.Android.keyaliasPass = "123456";
        UnityEngine.Debug.Log("设置打包签名完成!");
        return true;
    }


    [MenuItem("SDK-Publish/移除AndroidSDKPlugins目录", false, 3)]
    public static void ReMoveAndroidPlugins()
    {
        MoveAndroidSDKPlugins(true);
        AssetDatabase.Refresh();
    }


    [MenuItem("SDK-Publish/还原AndroidSDKPlugins目录", false, 5)]
    public static void AddAndroidPlugins()
    {
        MoveAndroidSDKPlugins(false);
        AssetDatabase.Refresh();
    }

#endif



    private static void MoveAndroidSDKPlugins(bool isGo)
    {
        string dir = Application.dataPath;
        int index = dir.LastIndexOf(Path.DirectorySeparatorChar);
        if (index > 0)
        {
            dir = dir.Substring(0, index);
        }
        dir = dir.Replace("/Assets", "");
        dir = dir + Path.DirectorySeparatorChar + "AndroidSDKPlugins";
                                                   
        string goDir = dir;
        string backDir = Application.dataPath + "/Plugins/AndroidSDKPlugins";
      
        if (isGo)
        {
            Directory.Move(@backDir, @goDir);
        }
        else
        {
            Directory.Move(@goDir, @backDir);
        }

    }

    [MenuItem("SDK-Publish/开始打包", false, 4)]
    public static void CreatProjects()
    {
        StartBuildAPK(isAPK);
    }


    static private string[] GetBuildScenes()
    {
        var names = new List<string>();
        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
            {
                continue;
            }

            if (e.enabled)
            {
                names.Add(e.path);
            }
        }
        return names.ToArray();
    }
}