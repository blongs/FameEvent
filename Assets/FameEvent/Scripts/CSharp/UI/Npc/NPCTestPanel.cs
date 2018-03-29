using System;
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
        RegistSelf(this, msgIds);
    }
    // Use this for initialization
    void Start()
    {
        UIManager.Instance.GetGameObject("AssetBundleButton").GetComponent<UIBehaviour>().AddButtonListener(AssetBundleButtonClick);
        UIManager.Instance.GetGameObject("ReleaseButton").GetComponent<UIBehaviour>().AddButtonListener(ReleaseButtonClick);
        UIManager.Instance.GetGameObject("DownAssetBundleButton").GetComponent<UIBehaviour>().AddButtonListener(DownLoadAssetBundleButtonClick);

        UIManager.Instance.GetGameObject("CowboyAttack").GetComponent<UIBehaviour>().AddButtonListener(CowboyAttackButtonClick);


    }

    bool run = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            if (run == false)
            {
                run = true;
                MsgBase msg = new MsgBase((ushort)NPCEvent.Run);
                SendMsg(msg);

            }

        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            if (run == true)
            {
                run = false;
                MsgBase msg = new MsgBase((ushort)NPCEvent.Idle);
                SendMsg(msg);
            }
        }
    }

    void AssetBundleButtonClick()
    {
#if USE_ASSETBUNDLE
        StartCoroutine(IABManifestLoader.Instance.LoadManifest());
#endif
        HankAssetResource msg = new HankAssetResource(true, (ushort)AssetEvent.HankResource, "scenceactor", "Prefabs", "Mesh", (ushort)UIEventAllen.Login);
        SendMsg(msg);
    }


    void ReleaseButtonClick()
    {
        HankAssetResource msg = new HankAssetResource(true, (ushort)AssetEvent.ReleaseSingleObj, "scenceactor", "Prefabs", "Mesh", (ushort)UIEventAllen.Load);
        SendMsg(msg);
    }

    void DownLoadAssetBundleButtonClick()
    {
        // string path = "http://192.168.13.107/Resourses/" + IPathTools.GetPlatformFolderName() + ".zip";
        // UpdateHelper.Instance.DownResource(path, null);
        HankAssetResource msg = new HankAssetResource(true, (ushort)AssetEvent.HankResource, "scenceactor", "Prefabs", "difu", (ushort)UIEventAllen.Login);
        SendMsg(msg);
    }

    void CowboyAttackButtonClick()
    {
        MsgBase msg = new MsgBase((ushort)NPCEvent.Attack);
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
