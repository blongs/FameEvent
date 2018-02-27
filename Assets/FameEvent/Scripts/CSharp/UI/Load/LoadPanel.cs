using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : UIBase
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
    }
    void Start()
    {
       
    }

    private void LoginBtnClick()
    {
       // Debug.unityLogger.Log("LoginBtn Click");
    }

    // Update is called once per frame
    void Update ()
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
