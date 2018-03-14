using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Text;

public class Client : MonoBehaviour
{
    Socket clientSocket;

    byte[] buffer = new byte[1024];

    void Initail()
    {
        IPAddress ipAdderss = IPAddress.Parse("127.0.0.1");
        //服务器的地址和端口
        IPEndPoint ipEnd = new IPEndPoint(ipAdderss, 8888);
        // 网卡地址协议  字节流  tcp 协议  
        clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        //三次握手
        clientSocket.BeginConnect(ipEnd, ConnectCallBack,clientSocket);
    }

    #region Connect Back

    public void ConnectCallBack(IAsyncResult ar)
    {
        clientSocket.EndConnect(ar);
    }
    #endregion


    #region recv
    void RecvCallBack(IAsyncResult ar)
    {
        Debug.LogError("----RecvCallBack----");
        int length = clientSocket.EndReceive(ar);
        string tmpStr = Encoding.Default.GetString(buffer, 0, length);
        Debug.Log("tmpStr =" + tmpStr);
        //  BegainSend(tmpStr);
    }

    public void BegainRecv()
    {
        clientSocket.BeginReceive(buffer, 0, 1024, SocketFlags.None, RecvCallBack, this);
    }
    #endregion

    #region Send
    void SendCallBack(IAsyncResult ar)
    {
        int byteSend = clientSocket.EndSend(ar);
        Debug.Log("byteSend =" + byteSend);

    }
    public void BegainSend(string tmpStr)
    {
        byte[] data = Encoding.Default.GetBytes(tmpStr);
        clientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallBack, this);
    }


    #endregion


    public void RecvMsgOver(byte[] allByte)
    {

    }
    // Use this for initialization
    void Start()
    {
        Initail();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BegainSend("1234");
        }

        BegainRecv();
    }
}
