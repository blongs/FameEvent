using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IABResourcesLoader:IDisposable
{
    private AssetBundle ABResources;

    public IABResourcesLoader(AssetBundle tmpBundle)
    {
        ABResources = tmpBundle;
    }

    public UnityEngine.Object this[string resName]
    {
        get
        {
            if (this.ABResources == null || !this.ABResources.Contains(resName))
            {
                Debug.Log("res not contain");
                return null;
            }
            return ABResources.LoadAsset(resName);
        }
    }

    public UnityEngine.Object[] loadResources(string resName)
    {
        if (this.ABResources == null || !this.ABResources.Contains(resName))
        {
            Debug.Log("res not contain");
            return null;
        }
        return this.ABResources.LoadAssetWithSubAssets(resName);
    }

    public void UnLoadResource(UnityEngine.Object resObj)
    {
        Resources.UnloadAsset(resObj);
    }

    public void Dispose()
    {
        if (this.ABResources != null)
        {
            ABResources.Unload(false);
        }
    }

    public void DebugAllResources()
    {
        string[] tmpAssetName = ABResources.GetAllAssetNames();
        for (int i = 0; i < tmpAssetName.Length; i++)
        {
            Debug.Log(tmpAssetName[i]);
        }
    }


}
