using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ILoaderManager : MonoBehaviour
{

    public static ILoaderManager Instance;

    private void Awake()
    {
        Instance = this;
        //加载manifest
        StartCoroutine(IABManifestLoader.Instance.LoadManifest());

    }

   // IABScenceManager scenceManager;

    private Dictionary<string, IABScenceManager> loadManager = new Dictionary<string, IABScenceManager>();
    //2读取配置文件
    public void ReadConfiger(string scenceName)
    {
        if (!loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = new IABScenceManager(scenceName);

            tmpManager.ReadConfiger(scenceName);

            loadManager.Add(scenceName, tmpManager);
        }
    }

    public void LoadCallBack(string scenceName, string bundleName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];
            StartCoroutine(tmpManager.LoadAssetSys(bundleName));
        }
        else
        {

        }
    }
    //3提供加载功能
    public void LoadAsset(string scenceName, string bundleName, LoaderProgrecess progress)
    {
        if (!loadManager.ContainsKey(scenceName))
        {
            ReadConfiger(scenceName);
        }
        IABScenceManager tmpManager = loadManager[scenceName];
        tmpManager.LoadAsset(bundleName, progress, LoadCallBack);

    }


    #region

    public string GetBundleRetateName(string scenceName,string bundleName)
    {
        IABScenceManager tmpManager = loadManager[scenceName];
        if (tmpManager != null)
        {
            return tmpManager.GetBundleReateName(bundleName);
        }
        else
        {
            return null;
        }

    }
    public Object GetSingleResources(string scenceName, string bundleName, string resName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];
            return tmpManager.GetSingleResources(bundleName, resName);
        }
        else
        {
            return null;
        }
    }

    public Object[] GetMultResources(string scenceName, string bundleName, string resName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];
            return tmpManager.GetMultResources(bundleName, resName);
        }
        else
        {
            return null;
        }
    }

    public void UnLoadResObj(string scenceName, string bundleName, string resName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];
            tmpManager.DisposeResObj(bundleName, resName);
        }

    }

    public void UnLoadBundleResObj(string scenceName, string bundleName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];
            tmpManager.DisposeBundleRes(bundleName);
        }
    }

    public void UnLoadAllResObjs(string scenceName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];
            tmpManager.DisposeAllRes();
        }
    }

    public void UnLoadAssetBunle(string scenceName, string bundleName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];
            tmpManager.DisposeBundle(bundleName);
        }
    }

    public void UnLoadAllAssetBundle(string scenceName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];
            tmpManager.DiseposeAllbundle();
            System.GC.Collect();
        }
    }

    public void UnLoadAllAssetBundleAndObjs(string scenceName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];
            tmpManager.DisposeAllBundleAndRes();
            System.GC.Collect();
        }
    }

    private void OnDestroy()
    {
        loadManager.Clear();
        System.GC.Collect();
    }


    public void DebuggerAssetBundle(string scenceName)
    {
        if (loadManager.ContainsKey(scenceName))
        {
            IABScenceManager tmpManager = loadManager[scenceName];
            tmpManager.DebugAllAsset();

        }
    }
    #endregion

    public bool IsLoadingAssetBundleFinish(string scenceName, string bundleName)
    {
        bool tmpBool = loadManager.ContainsKey(scenceName);
        if (tmpBool)
        {
            IABScenceManager tmpManager = loadManager[scenceName];
            return tmpManager.IsLoadingFinish(bundleName);
        }
        return false;
    }

    public bool IsLoadingAssetBundle(string scenceName, string bundleName)
    {
        bool tmpBool = loadManager.ContainsKey(scenceName);
        if (tmpBool)
        {
            IABScenceManager tmpManager = loadManager[scenceName];
            return tmpManager.IsLoadingAssetBundle(bundleName);
        }
        return false;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
