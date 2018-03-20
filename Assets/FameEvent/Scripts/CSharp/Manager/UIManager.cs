using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI的管理类，每一个需要交互的UI控件都注册到sonMembers中，然后每一个UIBase通过名字去查找，然后在通过UIBehaviour去封装监听。利于Lua热更代码。
/// 也是UI消息的发送中心
/// </summary>
public class UIManager : ManagerBase {

    public static UIManager Instance = null;

    void Awake()
    {
        Debug.Log("---UIManager--- Awake");
        Instance = this;
    }

    public void SendMsg(MsgBase msg)
    {
        if (msg.GetManager() == ManagerId.UIManader)
        {
            ProcessEvent(msg);
        }
        else
        {
            MsgCenter.Instance.SendToMsg(msg);
        }
    }


    private Dictionary<string, GameObject> sonMembers = new Dictionary<string, GameObject>();


    public void RegistGameObject(string name,GameObject obj)
    {
        if (!sonMembers.ContainsKey(name))
        {
            sonMembers.Add(name,obj);
        }
    }

    public void UnRegistGameObject(string name)
    {
        if (!sonMembers.ContainsKey(name))
        {
            sonMembers.Remove(name);
        }
    }

    public GameObject GetGameObject(string name)
    {
        if (sonMembers.ContainsKey(name))
        {
            return sonMembers[name];
        }
        return null;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
