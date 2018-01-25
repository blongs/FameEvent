using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterPanel:UIBase
{
    private void Awake()
    {
        msgIds = new ushort[]
       {
           (ushort)UIEventAllen.Login,
           (ushort)UIEventAllen.Register,
       };
        RegistSelf(this, msgIds);
        UIManager.Instance.GetGameObject("LoginBtn").GetComponent<UIBehaviour>().AddButtonListener(LoginBtnClick);
        UIManager.Instance.GetGameObject("RegisterBtn").GetComponent<UIBehaviour>().AddButtonListener(RegisterBtnClick);
    }
    void Start()
    {

    }

    private void LoginBtnClick()
    {
        Debug.unityLogger.Log("LoginBtn Click");
        MsgBase tmpBase = new MsgBase((ushort)UIEventAllen.Login);
        SendMsg(tmpBase);
    }

    private void RegisterBtnClick()
    {
        Debug.unityLogger.Log("RegisterBtn Click");
        MsgBase tmpBase = new MsgBase((ushort)UIEventAllen.Register);
       // SendMsg(tmpBase);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)UIEventAllen.Load:
                Debug.Log("---Received Load ---");
                break;
            case (ushort)UIEventAllen.Login:
                Debug.Log("---Received Login ---");
                break;
            default:
                break;
        }
    }
}
