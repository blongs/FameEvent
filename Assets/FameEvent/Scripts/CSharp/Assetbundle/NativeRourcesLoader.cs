using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public delegate void NativeResourceCallBack(NativeResourceCallBackNode tmpNode);
public class NativeResourceCallBackNode
{
    public string scenceName;

    public string bundleName;

    public string resourceName;

    public ushort backMsgId;

    public bool isSingle;

    public NativeResourceCallBack callBack;
    public NativeResourceCallBackNode nextValue;
    public NativeResourceCallBackNode(bool tmpSingle, string tmpSenceName, string tmpBundle, string tmpRes, ushort tmpBackId, NativeResourceCallBack tmpCallBack, NativeResourceCallBackNode tmpNode)
    {
        this.isSingle = tmpSingle;
        this.scenceName = tmpSenceName;
        this.bundleName = tmpBundle;
        this.resourceName = tmpRes;
        this.backMsgId = tmpBackId;
        this.callBack = tmpCallBack;
        this.nextValue = tmpNode;
    }

    public void Dispose()
    {
        callBack = null;
        nextValue = null;
        this.scenceName = null;
        this.bundleName = null;
        this.resourceName = null;

    }
}

public class NativeResourceCallBackManager
{
    Dictionary<string, NativeResourceCallBackNode> manager = null;

    public NativeResourceCallBackManager()
    {
        manager = new Dictionary<string, NativeResourceCallBackNode>();
    }

    public void AddBundle(string bundle, NativeResourceCallBackNode currentNode)
    {
        if (manager.ContainsKey(bundle))
        {
            NativeResourceCallBackNode tmpNode = manager[bundle];
            while (tmpNode.nextValue != null)
            {
                tmpNode = tmpNode.nextValue;
            }

            tmpNode.nextValue = currentNode;
        }
        else
        {
            manager.Add(bundle, currentNode);
        }
    }

    public void Dispose(string bundle)
    {
        if (manager.ContainsKey(bundle))
        {
            NativeResourceCallBackNode tmpNode = manager[bundle];

            while (tmpNode.nextValue != null)
            {
                NativeResourceCallBackNode curNode = tmpNode;

                tmpNode = tmpNode.nextValue;
                curNode.Dispose();
            }

            tmpNode.Dispose();

            manager.Remove(bundle);
        }
    }

    public void CallBackResource(string bundle)
    {
        if (manager.ContainsKey(bundle))
        {
            NativeResourceCallBackNode tmpNode = manager[bundle];
            do
            {
                tmpNode.callBack(tmpNode);
                tmpNode = tmpNode.nextValue;
            }
            while (tmpNode != null);
        }
        else
        {

        }
    }
}

public class NativeRourcesLoader : AssetBase
{
    public override void ProcessEvent(MsgBase recMsg)
    {
        switch (recMsg.msgId)
        {
            case (ushort)AssetEvent.HankResource:
                {
                    HankAssetResource tmpMsg = (HankAssetResource)recMsg;
#if USE_ASSETBUNDLE
                    GetResources(tmpMsg.scenceName, tmpMsg.bundleName, tmpMsg.resourceName, tmpMsg.isSingle, tmpMsg.backMsgId);
#else
                    if (tmpMsg.isSingle)
                    {
                        UnityEngine.Object temp = ResourcesManager.Instance.GetSceneResources(tmpMsg.scenceName, tmpMsg.bundleName, tmpMsg.resourceName);
                        NativeResourceCallBackNode tmpResourceNode = new NativeResourceCallBackNode(tmpMsg.isSingle, tmpMsg.scenceName, tmpMsg.bundleName, tmpMsg.resourceName, tmpMsg.backMsgId, SendToBackMsg, null);
                        CallBack.AddBundle(tmpMsg.bundleName, tmpResourceNode);
                        this.ReleaseBack.Changer(tmpMsg.backMsgId, temp);
                        SendMsg(ReleaseBack);
                    }
                    else
                    {
                        Debug.LogError("Resource.Load 暂时不需要这个功能");
                    }
#endif
                }
                break;

            case (ushort)AssetEvent.ReleaseSingleObj:
                {
                  
                    HankAssetResource tmpMsg = (HankAssetResource)recMsg;
#if USE_ASSETBUNDLE
                    ILoaderManager.Instance.UnLoadResObj(tmpMsg.scenceName, tmpMsg.bundleName, tmpMsg.resourceName);
#else
                    ResourcesManager.Instance.ReleaseSingleSceneObject(tmpMsg.scenceName, tmpMsg.bundleName, tmpMsg.resourceName);
#endif
                }
                break;
            case (ushort)AssetEvent.ReleaseBundleObj:
                {
                    HankAssetResource tmpMsg = (HankAssetResource)recMsg;
#if USE_ASSETBUNDLE
                    ILoaderManager.Instance.UnLoadBundleResObj(tmpMsg.scenceName, tmpMsg.bundleName);
#else
                    ResourcesManager.Instance.ReleaseSceneTypeObject(tmpMsg.scenceName, tmpMsg.bundleName);
#endif
                }
                break;
            case (ushort)AssetEvent.ReleaseScenceObj:
                {

                    HankAssetResource tmpMsg = (HankAssetResource)recMsg;
#if USE_ASSETBUNDLE
                    ILoaderManager.Instance.UnLoadAllResObjs(tmpMsg.scenceName);
#else
                    ResourcesManager.Instance.ReleaseAllSceneObject(tmpMsg.scenceName);
#endif
                }
                break;
            case (ushort)AssetEvent.ReleaseSingleBundle:
                {
                    HankAssetResource tmpMsg = (HankAssetResource)recMsg;
                    ILoaderManager.Instance.UnLoadAssetBunle(tmpMsg.scenceName, tmpMsg.bundleName);
                }
                break;
            case (ushort)AssetEvent.ReleaseScenceBundle:
                {
                    HankAssetResource tmpMsg = (HankAssetResource)recMsg;
                    ILoaderManager.Instance.UnLoadAllAssetBundle(tmpMsg.scenceName);
                }
                break;
            case (ushort)AssetEvent.ReleaseAll:
                {
                    HankAssetResource tmpMsg = (HankAssetResource)recMsg;
                    ILoaderManager.Instance.UnLoadAllAssetBundleAndObjs(tmpMsg.scenceName);
                }
                break;
        }

    }

    HankAssetResourceBack resBackMsg = null;
    HankAssetResourceBack ReleaseBack
    {
        get
        {
            if (resBackMsg == null)
            {
                resBackMsg = new HankAssetResourceBack();
            }
            return resBackMsg;
        }
    }

    NativeResourceCallBackManager callBack = null;

    NativeResourceCallBackManager CallBack
    {
        get
        {
            if (callBack == null)
            {
                callBack = new NativeResourceCallBackManager();
            }
            return callBack;
        }
    }
    private void Awake()
    {
        msgIds = new ushort[]
        {
            (ushort)AssetEvent.HankResource,
            (ushort) AssetEvent.ReleaseSingleObj,
            (ushort)AssetEvent.ReleaseBundleObj,
            (ushort)AssetEvent.ReleaseScenceObj,
            (ushort)AssetEvent.ReleaseSingleBundle,
            (ushort)AssetEvent.ReleaseScenceBundle,
            (ushort) AssetEvent.ReleaseAll,
        };

        RegistSelf(this, msgIds);
    }

    public void SendToBackMsg(NativeResourceCallBackNode tmpNode)
    {
        if (tmpNode.isSingle)
        {
            UnityEngine.Object tmpObj = ILoaderManager.Instance.GetSingleResources(tmpNode.scenceName, tmpNode.bundleName, tmpNode.resourceName);
            this.ReleaseBack.Changer(tmpNode.backMsgId, tmpObj);
            SendMsg(ReleaseBack);

        }
        else
        {
            UnityEngine.Object[] tmpObj = ILoaderManager.Instance.GetMultResources(tmpNode.scenceName, tmpNode.bundleName, tmpNode.resourceName);
            this.ReleaseBack.Changer(tmpNode.backMsgId, tmpObj);
            SendMsg(ReleaseBack);
        }
    }


    void LoadProgrecess(string bundleName, float progress)
    {
        if (progress >= 1)
        {
            CallBack.CallBackResource(bundleName);
            CallBack.Dispose(bundleName);
        }
    }
    public void GetResources(string sceneName, string bundleName, string res, bool isSingle, ushort backId)
    {
#if !USE_ASSETBUNDLE
       
#else

        if (!ILoaderManager.Instance.IsLoadingAssetBundle(sceneName, bundleName))
        {
            ILoaderManager.Instance.LoadAsset(sceneName, bundleName, LoadProgrecess);
            string bundleFullName = ILoaderManager.Instance.GetBundleRetateName(sceneName, bundleName);
            if (bundleFullName != null)
            {
                NativeResourceCallBackNode tmpNode = new NativeResourceCallBackNode(isSingle, sceneName, bundleName, res, backId, SendToBackMsg, null);
                CallBack.AddBundle(bundleFullName, tmpNode);
            }
            else
            {
                //error
            }
        }
        else
        {
            if (ILoaderManager.Instance.IsLoadingAssetBundleFinish(sceneName, bundleName))
            {
                if (isSingle)
                {
                    UnityEngine.Object tmpObj = ILoaderManager.Instance.GetSingleResources(sceneName, bundleName, res);
                    this.ReleaseBack.Changer(backId, tmpObj);
                    SendMsg(ReleaseBack);
                }
                else
                {
                    UnityEngine.Object[] tmpObj = ILoaderManager.Instance.GetMultResources(sceneName, bundleName, res);
                    this.ReleaseBack.Changer(backId, tmpObj);
                    SendMsg(ReleaseBack);
                }
            }
            else
            {
                string bundleFullName = ILoaderManager.Instance.GetBundleRetateName(sceneName,bundleName);
                if (bundleFullName != null)
                {
                    NativeResourceCallBackNode tmpNode = new NativeResourceCallBackNode(isSingle, sceneName, bundleName, res, backId, SendToBackMsg, null);
                    CallBack.AddBundle(bundleFullName, tmpNode);
                }
            }
        }
#endif
    }

}
