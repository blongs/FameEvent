using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum ManagerId
{
    GameManager = 0,
    UIManader = FrameTools.MsgSpan * 1,
    AudioManager = FrameTools.MsgSpan * 2,
    NPCManager = FrameTools.MsgSpan * 3,
    CharacterManager = FrameTools.MsgSpan * 4,
    AssetManager = FrameTools.MsgSpan * 5,
    NetManager = FrameTools.MsgSpan * 6,
}
public class FrameTools
{
    public const int MsgSpan = 3000;

}
