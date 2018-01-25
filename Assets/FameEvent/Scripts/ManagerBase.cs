using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class EventNode
{
    public MonoBase data;
    public EventNode next;
    public EventNode(MonoBase tmpMono)
    {
        this.data = tmpMono;
        this.next = null;
    }
}
public class ManagerBase :MonoBase
{
    public Dictionary<ushort, EventNode> eventTree = new Dictionary<ushort, EventNode>();

    public void RegistMsg(MonoBase mono,params ushort[] msgs)
    {
        for (int i = 0; i < msgs.Length; i++)
        {
            EventNode tmpNode = new EventNode(mono);
            RegistMsg(msgs[i], tmpNode);
        }
    }

    public void RegistMsg(ushort id, EventNode eventNode)
    {
        if (!eventTree.ContainsKey(id))
        {
            eventTree.Add(id, eventNode);
        }
        else
        {
            EventNode tmp = eventTree[id];
            while (tmp.next != null)
            {
                tmp = tmp.next;
            }
            tmp.next = eventNode;
        }
    }

    public void UnRegistMsg(MonoBase mono, params ushort[] msgs)
    {
        for (int i = 0; i < msgs.Length; i++)
        {
            UnRegistMsg(msgs[i], mono);
        }
    }

    public void UnRegistMsg(ushort id, MonoBase mono)
    {
        if (!eventTree.ContainsKey(id))
        {
            return;
        }
        else
        {
            EventNode tmp = eventTree[id];
            if (tmp.data == mono)
            {
                EventNode header = tmp;
                if (header.next != null)
                {
                    header.data = tmp.next.data;
                    header.next = tmp.next.next;
                }
                else
                {
                    eventTree.Remove(id);
                }
            }
            else
            {
                while (tmp.next != null && tmp.next.data != null)
                {
                    tmp = tmp.next;
                } 
                if (tmp.next.next != null)
                {
                    tmp.next = tmp.next.next;
                }
                else
                {
                    tmp.next = null;
                }
            }
        }
    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        if (!eventTree.ContainsKey(tmpMsg.msgId))
        {
            Debug.LogError("此消息id没有注册："+tmpMsg.msgId);
        }
        else
        {
            EventNode tmp = eventTree[tmpMsg.msgId];
            do
            {
                tmp.data.ProcessEvent(tmpMsg);
                tmp = tmp.next;
            }
            while (tmp != null);
        }
    }
}
