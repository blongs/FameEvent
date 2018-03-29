using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorJoystickEvent : MonoBehaviour {

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
    }


    void On_JoystickMoveEnd(MovingJoystick move)
    {
        if (move.joystickName == "ActorMoveJoy")
        {
           // GetComponent<Animation>().CrossFade("idle");
        }
    }
    void On_JoystickMove(MovingJoystick move)
    {
        if (move.joystickName == "ActorMoveJoy")
        {

        }
    }
}
