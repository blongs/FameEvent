﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTestPanel : UIBase
{

    private void Awake()
    {
        msgIds = new ushort[] {
(ushort)AssetEvent.BackTest,
(ushort)UIEventAllen.Login,
(ushort)UIEventAllen.Load,
        };
        RegistSelf(this,msgIds);
    }
    // Use this for initialization
    void Start()
    {
        UIManager.Instance.GetGameObject("AssetBundleButton").GetComponent<UIBehaviour>().AddButtonListener(AssetBundleButtonClick);
        UIManager.Instance.GetGameObject("ReleaseButton").GetComponent<UIBehaviour>().AddButtonListener(ReleaseButtonClick);
        UIManager.Instance.GetGameObject("DownAssetBundleButton").GetComponent<UIBehaviour>().AddButtonListener(DownLoadAssetBundleButtonClick);

        UIManager.Instance.GetGameObject("CowboyAttack").GetComponent<UIBehaviour>().AddButtonListener(CowboyAttackButtonClick);


    }

    // Update is called once per frame
    void Update()
    {

    }

    void AssetBundleButtonClick()
    {
#if USE_ASSETBUNDLE
        StartCoroutine(IABManifestLoader.Instance.LoadManifest());
#endif
        HankAssetResource msg = new HankAssetResource(true, (ushort)AssetEvent.HankResource, "scencecowboy", "Prefabs", "Cowboy", (ushort)UIEventAllen.Login);
        SendMsg(msg);
    }


    void ReleaseButtonClick()
    {
        HankAssetResource msg = new HankAssetResource(true, (ushort)AssetEvent.ReleaseSingleObj, "scencecowboy", "Prefabs", "Cowboy", (ushort)UIEventAllen.Load);
        SendMsg(msg);
    }

    void DownLoadAssetBundleButtonClick()
    {
        string path = "http://192.168.13.107/Resourses/" + IPathTools.GetPlatformFolderName() + ".zip";
        UpdateHelper.Instance.DownResource(path, null);
    }

    void CowboyAttackButtonClick()
    {
        MsgBase msg = new MsgBase((ushort)NPCEvent.ZWalkAttack);
        SendMsg(msg);
    }


    

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)UIEventAllen.Login:
                Debug.Log("Login");
                HankAssetResourceBack temp = (HankAssetResourceBack)tmpMsg;
                Instantiate(temp.value[0]);
                break;
            case (ushort)UIEventAllen.Load:
               
                Debug.Log("Release");
                break;
        }
    }
}
