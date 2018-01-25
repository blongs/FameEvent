using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void LoaderProgrecess(string bunleName, float progress);
public delegate void LoadFinish(string bunleName);


public class IABLoader
{
    private string bundleName;

    private string commomBundlePath;

    private string commomBundleFilePath;

    private WWW commomLoader;

    private float commonResourceLoaderProcess;

    private LoaderProgrecess loadProgress;
    private LoadFinish loadFinish;

    private IABResourcesLoader abloader;

    
    public IABLoader(string _commomBundlePath,string _commomBundleFilePath,string _bundleName,LoaderProgrecess loadProgress = null,LoadFinish loadFinish= null)
    {
        bundleName = _bundleName;
        commomBundlePath = _commomBundlePath;
        commomBundleFilePath = _commomBundleFilePath;
        commonResourceLoaderProcess = 0;
        this.loadProgress = loadProgress;
        this.loadFinish = loadFinish;
        abloader = null;
    }
    public IEnumerator CommonLoad()
    {
        commomLoader = new WWW(commomBundlePath);
        while (!commomLoader.isDone)
        {
            commonResourceLoaderProcess = commomLoader.progress;
            if (loadProgress != null)
            {
                loadProgress(bundleName, commonResourceLoaderProcess);
            }
            yield return commomLoader.progress;
        }

        if (commomLoader.progress >= 1.0)
        {
            abloader = new IABResourcesLoader(commomLoader.assetBundle);
           // abloader.DebugAllResources();
            if (loadProgress != null)
            {
                loadProgress(bundleName, commomLoader.progress);
            }

            if (loadFinish != null)
            {
                loadFinish(bundleName);
            }
        }
        else
        {
            Debug.LogError("load bundle error");
        }
        commomLoader = null;
    }

    /// <summary>
    /// 直接读取文件的方式加载AB包
    /// </summary>
    public void AssetBundleLoad()
    {
        AssetBundle assetBundle = AssetBundle.LoadFromFile(commomBundleFilePath);
        abloader = new IABResourcesLoader(assetBundle);
        if (loadFinish != null)
        {
            loadFinish(bundleName);
        }

        if (loadProgress != null)
        {
            loadProgress(bundleName,1);
        }
    }

    public void DebugerLoader()
    {
        if (commomLoader != null)
        {
            abloader.DebugAllResources();
        }

    }

    #region   下层提供
    //获取单个资源
    public Object GetResources(string name)
    {
        if (abloader != null)
        {
            return abloader[name];
        }
        else
        {
            return null;
        }
    }


    public Object[] GetMutiResources(string name)
    {
        if (abloader != null)
        {
            return abloader.loadResources(name);
        }
        else
        {
            return null;
        }
    }

    public void Dispose()
    {
        if (abloader != null)
        {
            abloader.Dispose();
            abloader = null;
        }
    }

    public void UnLoadAssetResources(Object tmpObj)
    {
        if (abloader != null)
        {
            abloader.UnLoadResource(tmpObj);
        }
    }


    #endregion
}
