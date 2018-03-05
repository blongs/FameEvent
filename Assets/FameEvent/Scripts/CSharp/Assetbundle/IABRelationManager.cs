using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IABRelationManager
{
    /// <summary>
    /// 
    /// </summary>
    List<string> dependenceBundle;

    /// <summary>
    /// 
    /// </summary>
    List<string> referBundle;

    IABLoader assetLoader;

    string theBundleName;

    LoaderProgrecess loaderProgrecess;

    public string GetBundleName()
    {
        return theBundleName;
    }

    public IABRelationManager()
    {
        dependenceBundle = new List<string>();

        referBundle = new List<string>();
    }

    public void AddRefference(string bundleName)
    {
        dependenceBundle.Add(bundleName);
    }

    public List<string> GetRefference()
    {
        return referBundle;
    }

    public bool RemoveRefference(string bundleName)
    {
        for (int i = 0; i < referBundle.Count; i++)
        {
            if (bundleName.Equals(referBundle[i]))
            {
                referBundle.RemoveAt(i);
            }
        }
        if (referBundle.Count <= 0)
        {
            DisPose();
            return true;
        }
        return false;
    }

    public void SetDependences(string[] dependence)
    {
        if (dependenceBundle.Count > 0)
        {
            dependenceBundle.AddRange(dependence);
        }
    }
    public List<string> GetDependence()
    {

        return dependenceBundle;
    }

    public void RemoveDependence(string bundleName)
    {
        for (int i = 0; i < dependenceBundle.Count; i++)
        {
            if (bundleName.Equals(dependenceBundle[i]))
            {
                dependenceBundle.RemoveAt(i);
            }
        }
    }
    bool IsLoadFinish;

    public void BundleLoadFinish(string bundleName)
    {
        IsLoadFinish = true;
    }

    public bool IsBundleLoadFinish()
    {
        return IsLoadFinish;
    }

    public void Initial(string bundleName, LoaderProgrecess progress)
    {
        IsLoadFinish = false;

        theBundleName = bundleName;

        loaderProgrecess = progress;
        assetLoader = new IABLoader(IPathTools.GetWWWAssetBundlePath() + "/" + theBundleName, IPathTools.GetAssetBundlePath() + "/" + theBundleName, theBundleName,progress, BundleLoadFinish);
    }


    public LoaderProgrecess GetProgress()
    {
        return loaderProgrecess;
    }


    #region 由下层提供API

    public IEnumerator LoadAssetBundle()
    {
        yield return assetLoader.CommonLoad();
    }

    public void LoadAssetBundleFromFile()
    {
        assetLoader.AssetBundleLoad();
    }
    public void DisPose()
    {
        if (assetLoader != null)
        {
            assetLoader.Dispose();
        }
    }

    public void UnLoadAssetResources(Object tmpObj)
    {
        if (assetLoader != null)
        {
            assetLoader.UnLoadAssetResources(tmpObj);
        }
    }
    public Object GetSingleResource(string bundleName)
    {
        return assetLoader.GetResources(bundleName);
    }

    public Object[] GetMutiResources(string bundleName)
    {
        return assetLoader.GetMutiResources(bundleName);
    }

    public void DebuugerAsset()
    {
        if (assetLoader != null)
        {
            assetLoader.DebugerLoader();
        }
    }

    #endregion
}
