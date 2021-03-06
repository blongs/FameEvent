﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消息处理中心，也是最先初始化的脚本，管理其他的Manager部分
/// </summary>
public class MsgCenter : MonoBase {

   
    public static MsgCenter Instance;
    // Use this for initialization
    private void Awake()
    {
        Instance = this;
        GameObject go = new GameObject("UIManager");
        go.AddComponent<UIManager>();
        go.transform.parent = transform;

        go = new GameObject("NPCManager");
        go.AddComponent<NPCManager>();

        go.transform.parent = transform;
        go = new GameObject("AssetBundleManager");

        go.AddComponent<AssetBundleManager>();
        go.transform.parent = transform;

        go = new GameObject("NetManager");
        go.AddComponent<NetManager>();

        go.transform.parent = transform;
        go = new GameObject("TabToyManager");

        go.AddComponent<TabToyManager>();
        go.transform.parent = transform;
    }
    void Start ()
    {
		
	}


    public override void ProcessEvent(MsgBase tmpMsg)
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SendToMsg(MsgBase tmpMsg)
    {
        AnasysisMsg(tmpMsg);
    }
    /// <summary>
    /// 处理所有模块的消息分发。
    /// </summary>
    /// <param name="tmpMsg"></param>
    private void AnasysisMsg(MsgBase tmpMsg)
    {
        ManagerId tmpId = tmpMsg.GetManager();
        switch (tmpId)
        {
            case ManagerId.AssetManager:
                AssetBundleManager.Instance.SendMsg(tmpMsg);
                break;
            case ManagerId.AudioManager:

                break;
            case ManagerId.CharacterManager:

                break;
            case ManagerId.GameManager:

                break;

            case ManagerId.NetManager:
                NetManager.Instance.SendMsg(tmpMsg);
                break;
            case ManagerId.NPCManager:
                NPCManager.Instance.SendMsg(tmpMsg);
                break;
            case ManagerId.UIManader:
                UIManager.Instance.SendMsg(tmpMsg);
                break;
            case ManagerId.TabToyManager:
                TabToyManager.Instance.SendMsg(tmpMsg);
                break;
            default:
                break;
        }
    }
}
