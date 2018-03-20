using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zwalk : NPCBase
{
    Animator animator;
    private void Awake()
    {
        msgIds = new ushort[] {
            (ushort)NPCEvent.ZWalkAttack,
        };
        RegistSelf(this, msgIds);
    }
    // Use this for initialization
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)NPCEvent.ZWalkAttack:
                Debug.Log("NPCEvent.ZWalkAttack");
                animator.SetBool("attack", true);
                break;
        }
    }
}
