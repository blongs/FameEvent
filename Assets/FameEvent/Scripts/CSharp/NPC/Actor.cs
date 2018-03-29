using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Actor : NPCBase
{
    public float speed = 0;
    Animator animator;
    NavMeshAgent navAgent;
    CharacterController characterController;
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
        characterController = gameObject.GetComponent<CharacterController>();
    }

    

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.W))
        {
            characterController.SimpleMove(transform.forward * 2.0f);
        }
        
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");


        if ((y != 0 || y != 0))
        {
            Vector3 direction = ComputeDirectionBaseOnCamera(x, y);
            Vector3 moveDestPos = transform.position + direction;

            //设置角色移动到的位置
            navAgent.ResetPath();
            navAgent.speed = speed;
            navAgent.SetDestination(moveDestPos);
            gameObject.transform.LookAt(moveDestPos);
        }
        */

    }


    public void StartMove(Vector3 DestPos, float moveSpeed = 5.0f)
    {
        navAgent.ResetPath();
        navAgent.speed = moveSpeed;
        navAgent.SetDestination(DestPos);
        gameObject.transform.LookAt(DestPos);
        animator.SetBool("run", true);
        animator.SetBool("idle", false);
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

    void OnEnable()
    {
        EasyJoystick.On_JoystickMove += On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
    }

    void OnDisable()
    {
        EasyJoystick.On_JoystickMove -= On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
    }

    void OnDestroy()
    {
        EasyJoystick.On_JoystickMove -= On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
        UnRegistSelf(this, msgIds);
    }

    void On_JoystickMoveEnd(MovingJoystick move)
    {
        if (move.joystickName == "ActorMoveJoy")
        {
            animator.SetBool("idle", true);
            animator.SetBool("run", false);
        }
    }
    void On_JoystickMove(MovingJoystick move)
    {
        if (move.joystickName == "ActorMoveJoy")
        {
            //获取摇杆中心偏移的坐标  
            float joyPositionX = move.joystickAxis.x;// > 0?2f:-2f;
            float joyPositionY = move.joystickAxis.y;// > 0?2f:-2f;
            if (joyPositionY != 0 || joyPositionX != 0)
            {
                if ((joyPositionY != 0 || joyPositionX != 0))
                {
                    Vector3 direction = ComputeDirectionBaseOnCamera(joyPositionX, joyPositionY);
                    Vector3 moveDestPos = transform.position + direction;
                    StartMove(moveDestPos,speed);
                }
            }
        }
    }
}
