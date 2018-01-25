using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 每一个UI界面管理类的父类，封装了注册和发送消息
/// </summary>
public  abstract class UIBase : MonoBase
{
    public ushort[] msgIds;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void RegistSelf(MonoBase mono,params ushort[] msgs)
    {
        UIManager.Instance.RegistMsg(mono,msgs);
    }

    public void UnRegistSelf(MonoBase mono, params ushort[] msgs)
    {
        UIManager.Instance.UnRegistMsg(mono, msgs);
    }

    public void SendMsg(MsgBase msg)
    {
        UIManager.Instance.SendMsg(msg);
    }

    private void OnDestroy()
    {
        if (msgIds != null)
        {
            UnRegistSelf(this,msgIds);
        }
    }
   
}
