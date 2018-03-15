using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class SocketBuffer
{
    //定义消息头
    private byte[] headByte;

    private byte headLength = 6;

    //接受到的数据
    private byte[] allRecvData;

    //当前接收到的数据长度
    private int curRecvLength;

    //总共接收的数据长度
    private int allDataLength;

    public SocketBuffer(byte tmpHeadLength,CallBackRecvOver temOver)
    {
        headLength = tmpHeadLength;
        headByte = new byte[headLength];
        callBackRecvOver = temOver;

    }

    public void RecvByte(byte[] recvByte,int realLength)
    {
        if (realLength == 0)
        {
            return;
        }
        //当前接收的数据小于头的长度
        if (curRecvLength < headByte.Length)
        {
            RecvHead(recvByte, realLength);
        }
        else
        {
            //接收的总长度
            int tmpLength = curRecvLength + realLength;
            if (tmpLength == allDataLength)
            {
                //刚好想等
                RecvOneAll(recvByte, realLength);
            }
            else if (tmpLength > allDataLength)
            {
                RecvLarger(recvByte, realLength);
            }
            else
            {
                RecvSmall(recvByte, realLength);
            }
        }
    }
    private void RecvLarger(byte[] recvByte, int realLength)
    {
        int tmpLength = allDataLength - curRecvLength;
        Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, tmpLength);
        curRecvLength += tmpLength;
        RecvOneMsgOver();

        int remainLength = realLength - tmpLength;
        byte[] remainByte = new byte[remainLength];
        Buffer.BlockCopy(recvByte, tmpLength, remainByte, 0, remainLength);

        //看成从socket里面取出来
        RecvByte(remainByte,remainLength);

    }

    private void RecvSmall(byte[] recvByte, int realLength)
    {
        Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, realLength);
        curRecvLength += realLength;
    }

    private void RecvOneAll(byte[] recvByte,int realLength)
    {
        Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, realLength);
        curRecvLength += realLength;
        RecvOneMsgOver();

    }

    private void RecvHead(byte[] recvByte, int realLength)
    {
        //差多少个字节，才能组成一个头
        int tmpReal = headByte.Length - curRecvLength;
        //现在接收的，和已经接收的总长度
        int tmpLength = curRecvLength + realLength;
        //总长度小于头部
        if (tmpLength < headByte.Length)
        {
            Buffer.BlockCopy(recvByte, 0, headByte, curRecvLength, realLength);
            curRecvLength += realLength;
        }
        else
        {
            Buffer.BlockCopy(recvByte, 0, headByte, curRecvLength, tmpReal);
            curRecvLength += tmpReal;
            //string headBytestr = "";
            /*
            for (int i = 0; i < headByte.Length; i++)
            {
                headBytestr = headBytestr +","+ headByte[i];
            }
            */
           // UnityEngine.Debug.Log("headBytestr = " + headBytestr);
            //取出四个字节，转化int   取出的字节表示整个消息的长度
          //  UnityEngine.Debug.Log("BitConverter.ToInt32(headByte,0) = " + BitConverter.ToInt32(headByte, 0));
            UnityEngine.Debug.Log("headLength = " + headLength);
            allDataLength = BitConverter.ToInt32(headByte,0) + headLength;
            // body+head
            allRecvData = new byte[allDataLength];
            //已经包含了投不了
            Buffer.BlockCopy(headByte, 0, allRecvData, 0, headLength);

            int tmpRemin = realLength - tmpReal;
            //表示recvByte 是否还有数据
            if (tmpRemin > 0)
            {
                byte[] tmpByte = new byte[tmpRemin];
                //表示将剩下的字节 送入 tmpByte
                Buffer.BlockCopy(recvByte, tmpReal, tmpByte, 0, tmpRemin);
                RecvByte(tmpByte, tmpRemin);
            }
            else
            {
                //
                RecvOneMsgOver();
            }
        }
    }

#region rec call back

    public delegate void CallBackRecvOver(byte[] allData);

    CallBackRecvOver callBackRecvOver;
    /// <summary>
    /// 接收完成
    /// </summary>
    private void RecvOneMsgOver()
    {
        if (callBackRecvOver != null)
        {
            callBackRecvOver(allRecvData);
        }
        curRecvLength = 0;
        allDataLength = 0;
        allRecvData = null;
    }

#endregion

}
