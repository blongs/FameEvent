﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TabToyEvent
{
    TableTest = ManagerId.TabToyManager + 1,
}

public class TabToyManager : ManagerBase
{

    public static TabToyManager Instance = null;

    private void Awake()
    {
        Instance = this;
    }

    public void SendMsg(MsgBase msg)
    {
        if (msg.GetManager() == ManagerId.TabToyManager)
        {
            ProcessEvent(msg);
        }
        else
        {
            MsgCenter.Instance.SendToMsg(msg);
        }
    }


    private Dictionary<string, GameObject> sonMembers = new Dictionary<string, GameObject>();


    public void RegistGameObject(string name, GameObject obj)
    {
        if (!sonMembers.ContainsKey(name))
        {
            sonMembers.Add(name, obj);
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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
