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
            case (ushort)TCPEvent.TcpSendMsgBack:
                Debug.Log("TcpSendMsgBack");

                break;
        }
    }

    private void Awake()
    {
        msgIds = new ushort[] {
        (ushort)TCPEvent.TcpSendMsgBack,
        };
        RegistSelf(this, msgIds);
        Test();
    }


    void Test()
    {
        //string data = "018003";
        // byte[] datas = Encoding.Default.GetBytes(data);
        int data = 999999999;
        byte[] datas = BitConverter.GetBytes(data);
        string datastr = "";
        for (int i = 0; i < datas.Length; i++)
        {
            datastr = datastr + " " + datas[i];
        }
        Debug.Log("datastr = " + datastr);
        Debug.Log("BitConverter.ToInt32(headByte,0) = " + BitConverter.ToInt32(datas, 0));
        Encoding.Default.GetString(datas);
        //Debug.Log("Encoding.Default.GetString(datas)= " + Encoding.Default.GetString(datas));
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
        string body = "body content = asdfasdf";
        byte[] data = Encoding.Default.GetBytes(body);
        int bodycount = body.Length;
        byte[] bodycountbytes = BitConverter.GetBytes(bodycount);
        byte[] headbackMsgbytes = BitConverter.GetBytes((ushort)TCPEvent.TcpSendMsgBack);

        NetMsgBase ba = new NetMsgBase(CombomBinaryArray(bodycountbytes, CombomBinaryArray(headbackMsgbytes, data)));
        TCPMsg msg = new TCPMsg((ushort)TCPEvent.TcpSendMsg, ba);
        SendMsg(msg);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private byte[] CombomBinaryArray(byte[] srcArray1, byte[] srcArray2)
    {
        //根据要合并的两个数组元素总数新建一个数组
        byte[] newArray = new byte[srcArray1.Length + srcArray2.Length];

        //把第一个数组复制到新建数组
        Array.Copy(srcArray1, 0, newArray, 0, srcArray1.Length);

        //把第二个数组复制到新建数组
        Array.Copy(srcArray2, 0, newArray, srcArray1.Length, srcArray2.Length);

        return newArray;
    }
}
