using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// UI模块开发人员的通信消息声明。
/// </summary>
public enum UIEventAllen
{
    Register = ManagerId.UIManader,
    Login,
    Load,
    MaxValue = ManagerId.UIManader+200,
}