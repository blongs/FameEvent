using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TCPEvent
{
    TcpConnect = ManagerId.NetManager + 1,
    TcpSendMsg,
    TcpSendMsgBack,
    TcpSendLoginMsg,
    TcpBackLoginMsg,
    MaxValue
}

public class TCPConnectMsg : MsgBase
{
    public string ip;
    public ushort port;
    public TCPConnectMsg(ushort tmpId, string ip, ushort port)
    {
        this.msgId = tmpId;
        this.ip = ip;
        this.port = port;
    }
}

public class TCPMsg : MsgBase
{
    public NetMsgBase netMsg;

    public TCPMsg(ushort tmpId,NetMsgBase tmpBase)
    {
        this.msgId = tmpId;
        this.netMsg = tmpBase;
    }
}
public class TCPSocket : NetBase
{
    NetWorkToServer socket = null;

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        //throw new System.NotImplementedException();
        switch (tmpMsg.msgId)
        {
            case (ushort)TCPEvent.TcpConnect:
                {
                    TCPConnectMsg connectMsg = (TCPConnectMsg)tmpMsg;
                    socket = new NetWorkToServer(connectMsg.ip, connectMsg.port);
                }
                break;
            case (ushort)TCPEvent.TcpSendMsg:
                {
                    TCPMsg sendMsg = (TCPMsg)tmpMsg;
                    socket.PutSendMsgToPool(sendMsg.netMsg);
                }
                break;
            case (ushort)TCPEvent.TcpSendLoginMsg:
                {
                    TCPMsg sendMsg = (TCPMsg)tmpMsg;
                    socket.PutSendMsgToPool(sendMsg.netMsg);
                }
                break;
        }
    }

    private void Awake()
    {
        msgIds = new ushort[]
            {
                (ushort) TCPEvent.TcpConnect,
                (ushort) TCPEvent.TcpSendMsg,
                (ushort) TCPEvent.TcpSendLoginMsg,


            };

        RegistSelf(this, this.msgIds);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (socket != null)
        {
            socket.Update();
        }
    }
}
