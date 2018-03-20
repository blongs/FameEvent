using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Text;
public class NetSocket
{

    public delegate void CallBackNormal(bool sucess, ErrorSocket error, string exception);

    //  public delegate void CallBackSend(bool sucess, ErrorSocket error, string exception);

    public delegate void CallBackRecv(bool sucess, ErrorSocket error, string exception, byte[] byteMessage, string strMessage);

    // public delegate void CallBackDisConnect(bool sucess, ErrorSocket error, string exception);

    private CallBackNormal callBackConnect;
    private CallBackNormal callBackSend;
    private CallBackNormal callBackDisConnect;

    private CallBackRecv callBackRecv;

    public enum ErrorSocket
    {
        Sucess = 0,
        TimeOut,
        SocketNull,
        SocketUnConnect,
        ConnectSucess,
        ConnectUnSucessKown,
        ConnectError,

        SendSucess,
        SendUnSucessUnKown,
        RecvUnSucessUnKown,

        DisConnectSucess,
        DisConnectUnKown,

    }

    private ErrorSocket errorSocket;

    private Socket clientSocket;

    // private string addressIP;

    // private ushort port;

    SocketBuffer socketBuffer;

    public NetSocket()
    {
        socketBuffer = new SocketBuffer(6, RecvMsgOver);
        recvBuf = new byte[1024];
    }
    public bool isConnected()
    {
        if (clientSocket != null && clientSocket.Connected)
        {
            return true;
        }
        return false;
    }


    #region Connect

    public void AsyncConnect(string _ip, ushort _port, CallBackNormal _connectBack, CallBackRecv _callBackRecv)
    {
        errorSocket = ErrorSocket.Sucess;
        callBackConnect = _connectBack;
        callBackRecv = _callBackRecv;

        if (clientSocket != null && clientSocket.Connected)
        {
            this.callBackConnect(false, ErrorSocket.ConnectError, "connect repeat");
        }
        else if (clientSocket == null || !clientSocket.Connected)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipAdress = IPAddress.Parse(_ip);

            IPEndPoint endPoint = new IPEndPoint(ipAdress, _port);

            IAsyncResult connect = clientSocket.BeginConnect(endPoint, ConnectCallBack, clientSocket);
            if (!WriteDot(connect))
            {
                callBackConnect(false, errorSocket, "链接超时");
            }


        }
    }
    private void ConnectCallBack(IAsyncResult asconnect)
    {
        try
        {
            clientSocket.EndConnect(asconnect);
            if (clientSocket.Connected == false)
            {
                errorSocket = ErrorSocket.ConnectUnSucessKown;
                this.callBackConnect(false, errorSocket, "链接超时");
                return;
            }
            else
            {
                errorSocket = ErrorSocket.ConnectSucess;
                this.callBackConnect(true, errorSocket, "sucess");
                Recive();
            }
        }
        catch (Exception e)
        {
            this.callBackConnect(false, errorSocket, e.ToString());
        }
    }

    #endregion

    #region TimeOut Check

    // true 表示可以写入 读取， false 表示 超时了
    bool WriteDot(IAsyncResult ar)
    {
        int i = 0;
        while (ar.IsCompleted == false)
        {
            i++;
            if (i > 20)
            {
                errorSocket = ErrorSocket.TimeOut;
                return false;
            }

            Thread.Sleep(200);
        }
        return true;
    }
    #endregion

    #region Recv

    byte[] recvBuf;
    public void Recive()
    {
        if (clientSocket != null && clientSocket.Connected)
        {
            // IAsyncResult recv = clientSocket.BeginReceive(recvBuf, 0, recvBuf.Length, SocketFlags.None, ReciveCallBack, clientSocket);
            clientSocket.BeginReceive(recvBuf, 0, recvBuf.Length, SocketFlags.None, ReciveCallBack, clientSocket);
            /*
            if (!WriteDot(recv))
            {
                callBackRecv(false, ErrorSocket.RecvUnSucessUnKown, "recive time out", null, "");
            }
            */

        }
    }

    private void ReciveCallBack(IAsyncResult _ar)
    {
        try
        {
            if (!clientSocket.Connected)
            {
                callBackRecv(false, ErrorSocket.RecvUnSucessUnKown, "链接失败", null, "");
                return;

            }
            int length = clientSocket.EndReceive(_ar);
            if (length == 0)
            {
                return;
            }
            socketBuffer.RecvByte(recvBuf, length);
        }
        catch (Exception e)
        {
            callBackRecv(false, ErrorSocket.RecvUnSucessUnKown, e.ToString(), null, "");
        }
        Recive();
    }
    #endregion

    #region RecvMsgOver

    public void RecvMsgOver(byte[] data)
    {
        callBackRecv(true, ErrorSocket.Sucess, "", data, "recv back sucess");
    }


    #endregion

    #region Send

    public void SendCallBack(IAsyncResult ar)
    {
        try
        {
            int byteSend = clientSocket.EndSend(ar);
            if (byteSend > 0)
            {
                callBackSend(true, ErrorSocket.SendSucess, "");
            }
        }
        catch (Exception e)
        {
            callBackSend(false, ErrorSocket.SendUnSucessUnKown, e.ToString());
        }
    }

    public void AsynSend(byte[] sendBuffer, CallBackNormal tmpSendBack)
    {
        errorSocket = ErrorSocket.Sucess;
        this.callBackSend = tmpSendBack;
        if (clientSocket == null)
        {
            callBackSend(false, ErrorSocket.SocketNull, "");
        }
        else if (!clientSocket.Connected)
        {
            callBackSend(false, ErrorSocket.SocketUnConnect, "");
        }
        else
        {
            IAsyncResult asySend = clientSocket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, SendCallBack, clientSocket);

            if (!WriteDot(asySend))
            {
                callBackSend(false, ErrorSocket.SendUnSucessUnKown, "send time out");

            }
        }
    }

    #endregion

    #region Disconnect

    public void DisConnectCallBack(IAsyncResult ar)
    {
        try
        {
            clientSocket.EndDisconnect(ar);
            clientSocket.Close();
            clientSocket = null;
            callBackDisConnect(true, ErrorSocket.DisConnectSucess, "");
        }
        catch (Exception e)
        {
            callBackDisConnect(false, ErrorSocket.DisConnectUnKown, e.ToString());
        }
    }

    public void AsyncDisConnect(CallBackNormal tmpConnectBack)
    {
        try
        {
            errorSocket = ErrorSocket.Sucess;
            this.callBackDisConnect = tmpConnectBack;

            if (clientSocket == null)
            {
                callBackDisConnect(false, ErrorSocket.DisConnectUnKown, "client is null");
            }
            else if (clientSocket.Connected == false)
            {
                callBackDisConnect(false, ErrorSocket.DisConnectUnKown, "client is unconnect");
            }
            else
            {
                IAsyncResult asynDisConnect = clientSocket.BeginDisconnect(false, DisConnectCallBack, clientSocket);
                if (!WriteDot(asynDisConnect))
                {
                    callBackDisConnect(false, ErrorSocket.SendUnSucessUnKown, "Disconnect time out");

                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("e ="+e.StackTrace);
        }
    }
    #endregion
}
