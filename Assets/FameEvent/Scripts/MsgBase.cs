using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消息体类，有个最起码的id，其他可自定义添加
/// </summary>
public class MsgBase
{
    // 表示65535个消息
    public ushort msgId;

    public ManagerId GetManager()
    {
        int temId = msgId / FrameTools.MsgSpan;
        return (ManagerId)(temId * FrameTools.MsgSpan );
    }
    public MsgBase()
    {

    }
    public MsgBase(ushort tmpMsg)
    {
        msgId = tmpMsg;
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //以下是自定义的消息函数体，带参数的，自己去实现
    public class MsgTransform : MsgBase
    {
        public Transform value;
   
        public MsgTransform(ushort tmpId)
        {
            msgId = tmpId;
        }
    }

}
