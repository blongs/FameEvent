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

    public byte[] GetNetBytes()
    {
        return buffer;
    }
	
}
