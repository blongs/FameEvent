using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class NetWorkToServer
{
    private Queue<NetMsgBase> recvMsgPool = null;

    private Queue<NetMsgBase> sendMsgPool = null;

    NetSocket clientSocket;

    Thread sendThread;



    public NetWorkToServer(string ip, ushort port)
    {
        recvMsgPool = new Queue<NetMsgBase>();
        sendMsgPool = new Queue<NetMsgBase>();
        clientSocket = new NetSocket();

        clientSocket.AsyncConnect(ip, port, AsysnCoonectCallBack, AsysnReciveCallBack);

    }

    void AsysnCoonectCallBack(bool sucess, NetSocket.ErrorSocket tmpError, string exception)
    {
        Debug.LogError("---AsysnCoonectCallBack---- sucess = "+ sucess);
        if (sucess)
        {
            sendThread = new Thread(LoopSendMsg);
            sendThread.Start();
        }
    }


    #region Send

    public void PutSendMsgToPool(NetMsgBase msg)
    {
        lock (sendMsgPool)
        {
            sendMsgPool.Enqueue(msg);
        }
    }
    void CallBackSend(bool sucess, NetSocket.ErrorSocket tmpError, string exception)
    {
       // Debug.LogError("---CallBackSend---- sucess = "+ sucess+ ",tmpError = "+ tmpError+ ",exception ="+ exception);
        if (sucess)
        {

        }
        else
        {

        }
    }

    void LoopSendMsg()
    {
        while (clientSocket != null && clientSocket.isConnected())
        {
            lock (sendMsgPool)
            {
                while (sendMsgPool.Count > 0)
                {
                    NetMsgBase tmpBody = sendMsgPool.Dequeue();
                    clientSocket.AsynSend(tmpBody.GetNetBytes(), CallBackSend);
                }
            }
            Thread.Sleep(100);

        }
    }

    #endregion

    #region Recive
    void AsysnReciveCallBack(bool sucess, NetSocket.ErrorSocket error, string exception, byte[] byteMessage, string strMessage)
    {
        Debug.LogError("---AsysnReciveCallBack---- sucess = "+ sucess+ ",exception = "+ exception+ ",strMessage = "+ strMessage);
        if (sucess)
        {
            PutRecvMsgToPool(byteMessage);
        }
        else
        {

        }
    }

    void PutRecvMsgToPool(byte[] recvMsg)
    {
        NetMsgBase tmp = new NetMsgBase(recvMsg);
        recvMsgPool.Enqueue(tmp);
    }
    public void Update()
    {
        if (recvMsgPool != null)
        {
            while (recvMsgPool.Count > 0)
            {
                NetMsgBase tmp = recvMsgPool.Dequeue();
                AnalyseData(tmp);
            }
        }
        //clientSocket.Recive();

    }

    void AnalyseData(NetMsgBase msg)
    {
        MsgCenter.Instance.SendToMsg(msg);
    }
    #endregion

    #region Disconnect

    void CallBackDisconnect(bool sucess, NetSocket.ErrorSocket tmpError, string exception)
    {
        if (sucess)
        {
            sendThread.Abort();
        }
        else
        {

        }
    }

    public void Disconnect()
    {
        if (clientSocket != null && clientSocket.isConnected())
        {
            clientSocket.AsyncDisConnect(CallBackDisconnect);
        }

    }
    #endregion



}
