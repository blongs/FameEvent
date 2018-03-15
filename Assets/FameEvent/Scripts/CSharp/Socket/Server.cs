using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Text;
using Google.Protobuf;
using PKG;


public class SocketState
{
    public Socket socket;

    public byte[] buffer;

    public SocketState(Socket tmpSocket)
    {
        socket = tmpSocket;
        buffer = new byte[1024];
    }
    #region recv
    void RecvCallBack(IAsyncResult ar)
    {
        /*
        string strs = "";
        for (int i = 0; i < buffer.Length; i++)
        {
            strs = strs + " " + buffer[i];
        }
        Debug.Log("strs= " + strs);
        */
        //返回值表示从客户端收到多少数据
        int length = socket.EndReceive(ar);
        short count = BitConverter.ToInt16(buffer, 0);
        /*
        Debug.Log("count = " + count);
        byte[] back = new byte[4];
        Debug.Log("---1---");
        for (int i = 4; i < 8; i++)
        {

            back[i - 4] = buffer[i];
            Debug.Log("back[i - 4] = " + back[i - 4]);
        }
        Debug.Log("---2---");
        short backId = BitConverter.ToInt16(back, 0);
        Debug.Log("backId = " + backId);
        */
        byte[] bodys = new byte[count];
        for (int i = 6; i < count + 6; i++)
        {
            bodys[i - 6] = buffer[i];
        }
        IMessage IMLogin = new Login();
        Login login = new Login();
        login = (Login)IMLogin.Descriptor.Parser.ParseFrom(bodys);
        Array.Resize(ref buffer, count + 6);
        BegainSend(buffer);

    }

    //处理客户端的数据
    public void BegainRecv()
    {
        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecvCallBack, this);
    }
    #endregion

    #region Send
    void SendCallBack(IAsyncResult ar)
    {
        int byteSend = socket.EndSend(ar);
        Debug.Log("byteSend =" + byteSend);

    }
    public void BegainSend(byte[] data)
    {
        Debug.Log("---BegainSend--- ");
        //Debug.Log("BegainSend tmpStr =" + tmpStr);
        // byte[] data = Encoding.Default.GetBytes(tmpStr);
        socket.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallBack, this);
    }


    #endregion
}

public class Server : MonoBehaviour
{
    private Socket serverSocket;


    #region accept
    /// <summary>
    /// 接收客户端的请求数据
    /// </summary>
    private void ListenRecv()
    {
        while (true)
        {
            try
            {
                //接收客户端的链接请求
                serverSocket.BeginAccept(new AsyncCallback(AsysAcceptCallBack), serverSocket);
            }
            catch (Exception e)
            {

            }
            Thread.Sleep(1000);
        }
    }

    /// <summary>
    ///处理接收数据的函数 
    /// </summary>
    /// <param name="ar"></param>

    void AsysAcceptCallBack(IAsyncResult ar)
    {
        Socket listener = (Socket)ar.AsyncState;
        //得到一个连接请求 杜塞德 直到客户端有请求过来
        Socket clientSocket = listener.EndAccept(ar);
        //
        socketArr.Add(new SocketState(clientSocket));
    }

    #endregion

    List<SocketState> socketArr;
    public void InitalSocket()
    {
        //表示和电脑端哪个网卡绑定 IPAddress.Any 表示使用最优的网卡 ，8888表示端口号
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 8888);
        //把socket和网卡还有端口号进行绑定
        serverSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        //socket和ip信息绑定
        serverSocket.Bind(endPoint);

        //存放的连接请求  可以接受100台机器的连接请求
        serverSocket.Listen(100);
        socketArr = new List<SocketState>();

        //开启一个子线程去接收客户端的数据
        Thread tmpThread = new Thread(ListenRecv);
        tmpThread.Start();
    }

    private void OnApplicationQuit()
    {
        serverSocket.Shutdown(SocketShutdown.Both);
    }


    private void Start()
    {
        InitalSocket();
    }
    private void Update()
    {
        if (socketArr.Count > 0)
        {
            for (int i = 0; i < socketArr.Count; i++)
            {
                socketArr[i].BegainRecv();
            }
        }
    }
}
