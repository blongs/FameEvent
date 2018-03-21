using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Actor : NPCBase
{
    Animator animator;
    NavMeshAgent navAgent;
    private void Awake()
    {
        msgIds = new ushort[] {
            (ushort)NPCEvent.Attack,
            (ushort)NPCEvent.Idle,
            (ushort)NPCEvent.Run,
        };
        RegistSelf(this, msgIds);
    }
    // Use this for initialization
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        navAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    

    // Update is called once per frame
    void Update()
    {


        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");


        if ((y != 0 || y != 0))
        {
            Vector3 direction = ComputeDirectionBaseOnCamera(x, y);
            Vector3 moveDestPos = transform.position + direction;

            //设置角色移动到的位置
            navAgent.ResetPath();
            navAgent.speed = 2.0f;
            navAgent.SetDestination(moveDestPos);
            gameObject.transform.LookAt(moveDestPos);
        }
    }

    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)NPCEvent.Attack:
                Debug.Log("NPCEvent.ZWalkAttack");
                animator.SetBool("attack", true);
                break;
            case (ushort)NPCEvent.Run:
                Debug.Log("NPCEvent.ZWalkAttack");
                animator.SetBool("run", true);
                animator.SetBool("idle", false);
                break;
            case (ushort)NPCEvent.Idle:
                Debug.Log("NPCEvent.ZWalkAttack");
                animator.SetBool("idle", true);
                animator.SetBool("run", false);
                animator.SetBool("attack", false);
                break;
        }
    }



    public Vector3 ComputeDirectionBaseOnCamera(float inputX, float inputY)
    {
        Vector3 camreaRight = Camera.main.transform.right;
        camreaRight.y = 0;
        Vector3 cameraGroundForward = Vector3.Cross(camreaRight, Vector3.up);
        Vector3 direction = inputX * camreaRight + inputY * cameraGroundForward;
        return direction;
    }





}
