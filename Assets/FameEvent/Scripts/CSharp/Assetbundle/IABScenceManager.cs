using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IABScenceManager
{

    IABManager abManager;
    public IABScenceManager(string scenceName)
    {
        abManager = new IABManager(scenceName);

    }

    private Dictionary<string, string> allAsset = new Dictionary<string, string>();


    public void ReadConfiger(string filePath)
    {
        string textFileName = "Record.txt";
        string path = IPathTools.GetAssetBundlePath() + "/" + filePath + textFileName;
        ReadConfig(path);

    }

    private void ReadConfig(string path)
    {
        Debug.LogError("path = "+ path);
        FileStream fs = new FileStream(path, FileMode.Open);

        StreamReader br = new StreamReader(fs);

        string line = br.ReadLine();

        int allCount = int.Parse(line);

        for (int i = 0; i < allCount; i++)
        {
            string tmpStr = br.ReadLine();
            string[] temArr = tmpStr.Split("".ToCharArray());
            allAsset.Add(temArr[0], temArr[1]);
        }
        br.Close();
        fs.Close();
    }

    public void LoadConfig(string path)
    {
        WWW config = new WWW(path);
        if (!string.IsNullOrEmpty(config.error))
        {
            Debug.LogError("config.error = " + config.error);
        }
        else
        {
            if (config.progress >= 1.0f)
            {
                string tmpStr = config.text;
                Debug.LogError("tmpStr = "+ tmpStr);
               // tmpStr.ToLower();
            }
        }
    }

    public void LoadAsset(string bundleName, LoaderProgrecess progress, LoadAssetBundleCallBack callBack)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            string tmpValue = allAsset[bundleName];
            abManager.LoadAssetBundle(tmpValue, progress, callBack);
        }
        else
        {
            Debug.LogError("没有"+ bundleName+"的资源");
        }
    }


    #region 
    public string GetBundleReateName(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            return allAsset[bundleName];
        }
        else
        {
            return null;
        }
    }


    public IEnumerator LoadAssetSys(string bundleName)
    {
        yield return abManager.LoadAssetBundles(bundleName);
    }

    public Object GetSingleResources(string bundleName, string resName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            return abManager.GetSingleResources(allAsset[bundleName], resName);
        }

        else
        {
            Debug.LogError("");
            return null;
        }
    }

    public Object[] GetMultResources(string bundleName, string resName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            return abManager.GetMultResources(allAsset[bundleName], resName);
        }

        else
        {
            Debug.LogError("");
            return null;
        }
    }

    public void DisposeResObj(string bundleName, string resName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            abManager.DisposeResoucesObj(allAsset[bundleName], resName);
        }

        else
        {
            Debug.LogError("");

        }
    }


    public void DisposeBundleRes(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            abManager.DisposeResoucesObj(allAsset[bundleName]);
        }

        else
        {
            Debug.LogError("");

        }
    }


    public void DisposeAllRes()
    {
        abManager.DisposeAllObj();
    }

    public void DisposeBundle(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            
            abManager.DisposeBundle(allAsset[bundleName]);
        }

        else
        {
            Debug.LogError("");

        }
    }

    public void DiseposeAllbundle()
    {
        abManager.DisposeAllBundle();
        allAsset.Clear();
    }

    public void DisposeAllBundleAndRes()
    {
        abManager.DisposeAllBundleAndRes();
        allAsset.Clear();
    }

    public void DebugAllAsset()
    {
        List<string> keys = new List<string>();

        keys.AddRange(allAsset.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            abManager.DebugAssetBundle(allAsset[keys[i]]);
        }
    }

    public bool IsLoadingFinish(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            return abManager.IsLoadingFinish(allAsset[bundleName]);
        }
        else
        {
            Debug.LogError("");
            return false;
        }
    }

    public bool IsLoadingAssetBundle(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            return abManager.IsLoadingAssetBundle(allAsset[bundleName]);
        }
        else
        {
            Debug.LogError("");
            return false;
        }
    }
    #endregion

}
