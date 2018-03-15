using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class NetMsgBase : MsgBase
{
    public byte[] buffer;
    public NetMsgBase(byte [] arr)
    {
        buffer = arr;
        //表示具体消息内容的长度
        msgId = BitConverter.ToUInt16(arr,4);
    }

    /// <summary>
    /// 获得此消息体的长度
    /// </summary>
    /// <returns></returns>
    public int GetBodyLength()
    {
        return BitConverter.ToInt32(buffer, 0);
    }

    /// <summary>
    /// 获得消息体的字节流
    /// </summary>
    /// <returns></returns>
    public byte[] GetBodyBytes()
    {
        byte[] tmpByte = new byte[GetBodyLength()];
        //表示将剩下的字节 送入 tmpByte
        Buffer.BlockCopy(buffer, 6, tmpByte, 0, GetBodyLength());
        return tmpByte;
    }

    public byte[] GetNetBytes()
    {
        return buffer;
    }
	
}
