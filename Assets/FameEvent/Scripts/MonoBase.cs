using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class MonoBase : MonoBehaviour
{
    public abstract void ProcessEvent(MsgBase tmpMsg);
	
}
