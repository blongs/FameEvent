using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using PKG;
using Google.Protobuf;

public class ProtoSocketTestPanel : UIBase
{
    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)TCPEvent.TcpSendMsgBack:
                Debug.Log("TcpSendMsgBack");
                break;
            case (ushort)TCPEvent.TcpBackLoginMsg:
               
                NetMsgBase tCPMsg = (NetMsgBase)tmpMsg;
                IMessage IMLogin = new Login();
                Login login = new Login();
                login = (Login)IMLogin.Descriptor.Parser.ParseFrom(tCPMsg.GetBodyBytes());
                Debug.Log("login.UserName = " + login.UserName);
                Debug.Log("login.PassWord = " + login.PassWord);
                break;
        }
    }

    private void Awake()
    {
        msgIds = new ushort[] {
        (ushort)TCPEvent.TcpSendMsgBack,
        (ushort)TCPEvent.TcpBackLoginMsg,
        };
        RegistSelf(this, msgIds);
        Test();
    }


    void Test()
    {

        byte[] datas = new byte[20];
        for (int i = 0; i < datas.Length; i++)
        {
            datas[i] = (byte)i;
        }

        byte[] tmpByte = new byte[3];
        //表示将剩下的字节 送入 tmpByte
        Buffer.BlockCopy(datas, 6, tmpByte, 0, 3);

        string strs = "";
        for (int i = 0; i < tmpByte.Length; i++)
        {
            datas[i] = (byte)i;
            strs = strs + tmpByte[i];
        }
        Debug.Log("strs = "+strs);
    }



    // Use this for initialization
    void Start()
    {
        UIManager.Instance.GetGameObject("TcpConnectButton").GetComponent<UIBehaviour>().AddButtonListener(TcpConnectButtonClick);
        UIManager.Instance.GetGameObject("TcpSendMsgButton").GetComponent<UIBehaviour>().AddButtonListener(TcpSendMsgButtonClick);
        UIManager.Instance.GetGameObject("TcpLoginMsgButton").GetComponent<UIBehaviour>().AddButtonListener(TcpLoginMsgButtonClick);
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

        NetMsgBase ba = new NetMsgBase(FrameTools.CombomBinaryArray(bodycountbytes, FrameTools.CombomBinaryArray(headbackMsgbytes, data)));
        TCPMsg msg = new TCPMsg((ushort)TCPEvent.TcpSendMsg, ba);
        SendMsg(msg);
    }


    private void TcpLoginMsgButtonClick()
    {
        Login login = new Login();
        login.UserName = "username";
        login.PassWord = "password";

        byte[] bodys = login.ToByteArray();

        string strs = "";
        for (int i = 0; i < bodys.Length; i++)
        {
            strs = strs + " " + bodys[i];
        }
        int bodycount = bodys.Length;
        byte[] bodycountbytes = BitConverter.GetBytes(bodycount);
        byte[] headbackMsgbytes = BitConverter.GetBytes((ushort)TCPEvent.TcpBackLoginMsg);

        NetMsgBase ba = new NetMsgBase(FrameTools.CombomBinaryArray(bodycountbytes, FrameTools.CombomBinaryArray(headbackMsgbytes, bodys)));
        TCPMsg msg = new TCPMsg((ushort)TCPEvent.TcpSendLoginMsg, ba);
        SendMsg(msg);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
