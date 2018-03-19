using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using PKG;
using Google.Protobuf;

public class TabToyTestPanel : UIBase
{
    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)TabToyEvent.TableTestBackSample:
                Debug.Log("TableTestBackSample");
                TableTestMsg temp = (TableTestMsg)tmpMsg;
                Debug.Log("temp.sampleDefine.Name = " + temp.sampleDefine.Name+ "temp.sampleDefine.ID =" + temp.sampleDefine.ID);
                break;
            case (ushort)TabToyEvent.TableTestBackBlongs:
                Debug.Log("TableTestBackBlongs");
                TableTestMsg temp1 = (TableTestMsg)tmpMsg;
                Debug.Log("temp.sampleDefine.Name = " + temp1.blongsDefine.NEWID + "temp.sampleDefine.ID =" + temp1.blongsDefine.NEWName);
                break;
            case (ushort)TabToyEvent.TableCharactorBack:
                Debug.Log("TableCharactorBack");
                TableCharactorMsg temp2 = (TableCharactorMsg)tmpMsg;
                Debug.Log("temp.sampleDefine.Name = " + temp2.charactorDefine.Name + "temp.sampleDefine.ID =" + temp2.charactorDefine.ID);
                break;
        }
    }

    private void Awake()
    {
        msgIds = new ushort[] {
        (ushort)TabToyEvent.TableTestBackSample,
        (ushort)TabToyEvent.TableTestBackBlongs,
        (ushort)TabToyEvent.TableCharactorBack,
        };
        RegistSelf(this, msgIds);
    }


    // Use this for initialization
    void Start()
    {
        UIManager.Instance.GetGameObject("GetSample").GetComponent<UIBehaviour>().AddButtonListener(GetSampleButtonClick);
        UIManager.Instance.GetGameObject("GetBlongs").GetComponent<UIBehaviour>().AddButtonListener(GetBlongsButtonClick);
        UIManager.Instance.GetGameObject("GetCharactor").GetComponent<UIBehaviour>().AddButtonListener(GetCharactorButtonClick);
    }


    private void GetBlongsButtonClick()
    {
        TableTestMsg msg = new TableTestMsg(0,null,1,null);
        msg.msgId = (ushort)TabToyEvent.TableTestGetBlongs;
        SendMsg(msg);
    }


    private void GetSampleButtonClick()
    {
        TableTestMsg msg = new TableTestMsg(101, null, 0, null);
        msg.msgId = (ushort)TabToyEvent.TableTestGetSample;
        SendMsg(msg);
    }


    private void GetCharactorButtonClick()
    {
        TableCharactorMsg msg = new TableCharactorMsg(101, null);
        msg.msgId = (ushort)TabToyEvent.TableCharactorGet;
        SendMsg(msg);
    }




    // Update is called once per frame
    void Update()
    {

    }
}
