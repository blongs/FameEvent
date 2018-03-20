using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleManager : ManagerBase
{
    public static AssetBundleManager Instance;
    NativeRourcesLoader nativeRourcesLoader;
    private void Awake()
    {
        Instance = this;
        gameObject.AddComponent<ILoaderManager>();
        if (nativeRourcesLoader == null)
        {
            nativeRourcesLoader = gameObject.AddComponent<NativeRourcesLoader>();
        }
    }

    public void SendMsg(MsgBase msg)
    {
        if (msg.GetManager() == ManagerId.AssetManager)
        {
            ProcessEvent(msg);
        }
        else
        {
            MsgCenter.Instance.SendToMsg(msg);
        }
    }
}
