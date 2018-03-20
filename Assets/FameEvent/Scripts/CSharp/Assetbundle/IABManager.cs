using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public delegate void LoadAssetBundleCallBack(string scenceName, string bundleName);

public class AssetResObj
{
    public List<Object> objs;
    public AssetResObj(params Object[] tmpObj)
    {
        objs = new List<Object>();
        objs.AddRange(tmpObj);
    }

    public void ReleaseObj()
    {
        for (int i = 0; i < objs.Count; i++)
        {
#if USE_ASSETBUNDLE
            Resources.UnloadAsset(objs[i]);
#else
            GameObject.Destroy(objs[i]);
#endif
        }
    }
}
public class AssetResObjs
{
    Dictionary<string, AssetResObj> resObjs;
    public AssetResObjs(string name ,AssetResObj tmp)
    {
        resObjs = new Dictionary<string, AssetResObj>();
        resObjs.Add(name,tmp);
    }

    public void AddResObj(string name, AssetResObj tmpObj)
    {
        resObjs.Add(name,tmpObj);
    }

    public void ReleaseAllResObj()
    {
        List<string> keys = new List<string>();
        foreach (string key in resObjs.Keys)
        {
            keys.Add(key);
        }
        for (int i = 0; i < keys.Count; i++)
        {
            ReleaseResObj(keys[i]);
        }
    }

    public void ReleaseResObj(string name)
    {
        if (resObjs.ContainsKey(name))
        {
            AssetResObj tmpObj = resObjs[name];
            tmpObj.ReleaseObj();
        }
        else
        {

        }
    }

    public List<Object> GetResObj(string name)
    {
        if (resObjs.ContainsKey(name))
        {
            AssetResObj tmpObj = resObjs[name];
            return tmpObj.objs;
        }
        else
        {
            return null;
        }
    }

}

public class IABManager
{
    Dictionary<string, IABRelationManager> loadHelper = new Dictionary<string, IABRelationManager>();


    Dictionary<string, AssetResObjs> loadObjs = new Dictionary<string, AssetResObjs>();

    string scenceName;


    public IABManager(string _scenceName)
    {
        scenceName = _scenceName;
    }

    public bool IsLoadingAssetBundle(string bundleName)
    {
        if (loadHelper.ContainsKey(bundleName))
        {
            return true;
        }
        return false;
    }

    public void DisposeResoucesObj(string bundleName, string resName)
    {
        if (loadObjs.ContainsKey(bundleName))
        {
            AssetResObjs tmpObj = loadObjs[bundleName];
            tmpObj.ReleaseResObj(resName);
        }
    }


    public void DisposeResoucesObj(string bundleName)
    {
        if (loadObjs.ContainsKey(bundleName))
        {
            AssetResObjs tmpObj = loadObjs[bundleName];
            tmpObj.ReleaseAllResObj();
        }
        Resources.UnloadUnusedAssets();
    }

    public void DisposeAllObj()
    {
        List<string> keys = new List<string>();
        keys.AddRange(loadObjs.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            DisposeResoucesObj(keys[i]);
        }
        loadObjs.Clear();
    }

    public void DisposeBundle(string bundleName)
    {
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];

            List<string> depences = loader.GetDependence();

            for (int i = 0; i < depences.Count; i++)
            {
                if (loadHelper.ContainsKey(depences[i]))
                {
                    IABRelationManager depedences = loadHelper[depences[i]];

                    if (depedences.RemoveRefference(bundleName))
                    {
                        DisposeBundle(depedences.GetBundleName());
                    }
                }
            }

            if (loader.GetRefference().Count <= 0)
            {
                loader.DisPose();
                loadHelper.Remove(bundleName);
            }


        }
    }


    public void DisposeAllBundle()
    {
        List<string> keys = new List<string>();

        keys.AddRange(loadHelper.Keys);
        for (int i = 0; i < loadHelper.Count; i++)
        {
            IABRelationManager loader = loadHelper[keys[i]];
            loader.DisPose();
        }
        loadHelper.Clear();
    }

    public void DisposeAllBundleAndRes()
    {
        DisposeAllObj();
        DisposeAllBundle();
    }

    public void LoadAssetBundle(string bundleName,LoaderProgrecess progress, LoadAssetBundleCallBack callBack)
    {
        if (!loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = new IABRelationManager();
            loader.Initial(bundleName, progress);

            loadHelper.Add(bundleName, loader);
            callBack(scenceName, bundleName);
        }
        else
        {

        }
    }

    string[] GetDepences(string bundleName)
    {
        return IABManifestLoader.Instance.GetDepences(bundleName);
    }

    public IEnumerator LoadAssetBundleDependences(string bundleName,string refName,LoaderProgrecess progress)
    {
        if (!loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = new IABRelationManager();
            loader.Initial(bundleName, progress);
            if (refName != null)
            {
                loader.AddRefference(refName);
            }
            loadHelper.Add(bundleName, loader);

            yield return LoadAssetBundles(bundleName);
        }
        else
        {
            if (refName != null)
            {
                IABRelationManager loader = loadHelper[bundleName];
                loader.AddRefference(refName);
            }
        }
    }

    /// <summary>
    /// 加载bundle必须先加载manifest
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public IEnumerator LoadAssetBundles(string bundleName)
    {
        while (!IABManifestLoader.Instance.IsLoadFinish())
        {
            yield return null;

        }

        IABRelationManager loader = loadHelper[bundleName];

        string[] depences = GetDepences(bundleName);

        loader.SetDependences(depences);
        for (int i = 0; i < depences.Length; i++)
        {
            yield return LoadAssetBundleDependences(depences[i],bundleName,loader.GetProgress());
        }

        // yield return loader.LoadAssetBundle();
        loader.LoadAssetBundleFromFile();
    }

#region 由下层提供API
    public void DebugAssetBundle(string bundleName)
    {
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            loader.DebuugerAsset();
        }
    }


    public bool IsLoadingFinish(string bundleName)
    {
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            return loader.IsBundleLoadFinish();
        }
        else
        {
            return false;
        }
    }

    public Object GetSingleResources(string bundleName, string resName)
    {
        if (loadObjs.ContainsKey(bundleName))
        {
            AssetResObjs tmpRes = loadObjs[bundleName];
            List<Object> tmpObj = tmpRes.GetResObj(resName);
            if (tmpObj != null)
            {
                return tmpObj[0];
            }
            else
            {

            }
        }
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            Object tmpObj = loader.GetSingleResource(resName);
            AssetResObj tempAssetObj = new AssetResObj(tmpObj);
            if (loadObjs.ContainsKey(bundleName))
            {
                AssetResObjs tmpRes = loadObjs[bundleName];
                tmpRes.AddResObj(resName, tempAssetObj);
            }
            else
            {
                AssetResObjs tmpRes = new AssetResObjs(resName, tempAssetObj);
                loadObjs.Add(bundleName, tmpRes);
            }
            return tmpObj;
        }
        else
        {
            return null;
        }
    }



    public Object[] GetMultResources(string bundleName, string resName)
    {
        if (loadObjs.ContainsKey(bundleName))
        {
            AssetResObjs tmpRes = loadObjs[bundleName];
            List<Object> tmpObj = tmpRes.GetResObj(resName);
            if (tmpObj != null)
            {
                return tmpObj.ToArray();
            }
            else
            {

            }
        }
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            Object[] tmpObj = loader.GetMutiResources(resName);
            AssetResObj tempAssetObj = new AssetResObj(tmpObj);
            if (loadObjs.ContainsKey(bundleName))
            {
                AssetResObjs tmpRes = loadObjs[bundleName];
                tmpRes.AddResObj(resName, tempAssetObj);
            }
            else
            {
                AssetResObjs tmpRes = new AssetResObjs(resName, tempAssetObj);
                loadObjs.Add(bundleName, tmpRes);
            }
            return tmpObj;
        }
        else
        {
            return null;
        }
    }
#endregion

}
