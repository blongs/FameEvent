using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class SocketTestPanel : UIBase
{
    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)(ushort)TCPEvent.TcpRecMsgTest:
                Debug.Log("TcpRecMsgTest");

                break;
        }
    }

    private void Awake()
    {
        msgIds = new ushort[] {
        (ushort)TCPEvent.TcpRecMsgTest,
        };
        RegistSelf(this, msgIds);
        Test();
    }


    void Test()
    {
        string data = "018003";
        byte[] datas = Encoding.Default.GetBytes(data);
        //int data = 018003;
        // byte[] datas = BitConverter.GetBytes(data);
        string datastr = "";
        for (int i = 0; i < datas.Length; i++)
        {
            datastr = datastr + "," + datas[i];
        }
        Debug.Log("datastr = " + datastr);
        //Debug.Log("BitConverter.ToInt32(headByte,0) = " + BitConverter.ToInt32(datas, 0));
        Encoding.Default.GetString(datas);
        Debug.Log("Encoding.Default.GetString(datas)= " + Encoding.Default.GetString(datas));
    }



    // Use this for initialization
    void Start()
    {
        UIManager.Instance.GetGameObject("TcpConnectButton").GetComponent<UIBehaviour>().AddButtonListener(TcpConnectButtonClick);
        UIManager.Instance.GetGameObject("TcpSendMsgButton").GetComponent<UIBehaviour>().AddButtonListener(TcpSendMsgButtonClick);
    }


    private void TcpConnectButtonClick()
    {
        TCPConnectMsg msg = new TCPConnectMsg((ushort)TCPEvent.TcpConnect, "127.0.0.1", 8888);
        SendMsg(msg);
    }


    private void TcpSendMsgButtonClick()
    {
        string content = string.Format("{0:D6}", (ushort)TCPEvent.TcpRecMsgTest) + "asdfasdf";
        Debug.Log("content = " + content);
        byte[] data = Encoding.Default.GetBytes(content);
        string datastr = "";
        for (int i = 0; i < data.Length; i++)
        {
            datastr = datastr + "," + data[i];
        }
        Debug.Log("datastr = " + datastr);
        NetMsgBase ba = new NetMsgBase(data);
        TCPMsg msg = new TCPMsg((ushort)TCPEvent.TcpSendMsg, ba);
        SendMsg(msg);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
