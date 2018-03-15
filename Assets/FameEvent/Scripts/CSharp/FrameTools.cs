using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



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
    public static string  ResoucesParentPath = "Art/Scences";
    public const int MsgSpan = 3000;




    public static byte[] CombomBinaryArray(byte[] srcArray1, byte[] srcArray2)
    {
        //根据要合并的两个数组元素总数新建一个数组
        byte[] newArray = new byte[srcArray1.Length + srcArray2.Length];

        //把第一个数组复制到新建数组
        Array.Copy(srcArray1, 0, newArray, 0, srcArray1.Length);

        //把第二个数组复制到新建数组
        Array.Copy(srcArray2, 0, newArray, srcArray1.Length, srcArray2.Length);

        return newArray;
    }
}
