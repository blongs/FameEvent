using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IABManifestLoader
{
    public AssetBundleManifest assetManifest;

    public string manifestPath;

    private bool isLoadFinish;

    public AssetBundle manifestLoader;

    public bool IsLoadFinish()
    {
        return isLoadFinish;
    }
    public void SetManifestPath(string path)
    {
        manifestPath = path;
    }

    public static IABManifestLoader instance = null;

    public static IABManifestLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new IABManifestLoader();
            }
            return instance;

        }
    }

    public IABManifestLoader()
    {
        assetManifest = null;
        manifestLoader = null;
        isLoadFinish = false;
        manifestPath = IPathTools.GetAssetBundlePath()+"/"+ IPathTools.GetPlatformFolderName();
    }

    public IEnumerator LoadManifest()
    {
        WWW manifest = new WWW(manifestPath);
        yield return manifest;
        if (!string.IsNullOrEmpty(manifest.error))
        {
            //baocuo xin xi 
        }
        else
        {
            //jiazai wancheng 
            if (manifest.progress >= 1.0f)
            {
                manifestLoader = manifest.assetBundle;

                assetManifest = manifestLoader.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                isLoadFinish = true;
            }
        }
    }

    public string[] GetDepences(string name)
    {
        return assetManifest.GetAllDependencies(name);
    }

    public void UnLoadManifest()
    {
        manifestLoader.Unload(true);
    }

}
