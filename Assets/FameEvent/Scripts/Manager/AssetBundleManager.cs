using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleManager : ManagerBase
{
    public static AssetBundleManager Instance;
    private void Awake()
    {
        Instance = this;
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
