using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cowboy : NPCBase
{
    private void Awake()
    {
        msgIds = new ushort[] {
            (ushort)NPCEvent.CowboyAttack,
        };
        RegistSelf(this,msgIds);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)NPCEvent.CowboyAttack:
                Debug.Log("NPCEvent.CowboyAttack");
                break;
        }
    }
}
