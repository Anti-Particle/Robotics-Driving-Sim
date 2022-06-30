using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Movement : MonoBehaviour
{
    
    private GameObject RedBallObject;
    private GameObject BlueBallObject;
    private Transform RedBallTransform;
    private Transform BlueBallTransform;
    private Rigidbody RedBall;
    private Rigidbody BlueBall;

    private Transform transform;
    private Rigidbody robot;
    private PlayerInput input;
    private InputActionMap actionMap;
    private InputAction intake;
    private InputAction RStick;
    private InputAction LStick;
    private InputAction FL;
    private InputAction FR;
    private InputAction BL;
    private InputAction BR;
    public float maxSpeed;
    public float maxSpin;
    public float forceMult;
    public float torqueMult;
    public float pullPower;

    // Start is called before the first frame update
    void Awake()
    {
        RedBallObject = GameObject.Find("Red Ball");
        BlueBallObject = GameObject.Find("Blue Ball");
        RedBallTransform = RedBallObject.GetComponent<Transform>();
        BlueBallTransform = BlueBallObject.GetComponent<Transform>();
        BlueBall = BlueBallObject.GetComponent<Rigidbody>();
        RedBall = RedBallObject.GetComponent<Rigidbody>();

        transform = GetComponent<Transform>();
        robot = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();

        actionMap = input.currentActionMap;

        FL = actionMap.FindAction("Left Foreward");
        FR = actionMap.FindAction("Right Foreward");
        BL = actionMap.FindAction("Left Backward");
        BR = actionMap.FindAction("Right Backward");

        LStick = actionMap.FindAction("Left Movement");
        RStick = actionMap.FindAction("Right Movement");

        intake = actionMap.FindAction("Pick Ball");

        intake.performed += index;

        input.onActionTriggered += PlayerInput_onActionTriggered;
    }

    private void Update() {
        float PID1 = (float) (maxSpeed - robot.velocity.magnitude)/maxSpeed;
        if (!(PID1 > 0)) PID1 = 0f;

        float PID2 = (float) (maxSpin - robot.angularVelocity.magnitude)/maxSpin;
        if (!(PID2 > 0)) PID1 = 0f;

        robot.AddForce(transform.forward * forceMult * PID1 * (float) root((FL.ReadValue<float>() - BL.ReadValue<float>()) + (FR.ReadValue<float>() - BR.ReadValue<float>())), ForceMode.Force);
        robot.AddTorque(transform.up * torqueMult * (float) root((FL.ReadValue<float>() + BR.ReadValue<float>()) - (BL.ReadValue<float>() + FR.ReadValue<float>())));

        robot.AddForce(transform.forward * forceMult * PID1 * (float) root(LStick.ReadValue<Vector2>().y + RStick.ReadValue<Vector2>().y), ForceMode.Force);
        robot.AddTorque(transform.up * torqueMult * (float) root(LStick.ReadValue<Vector2>().y - RStick.ReadValue<Vector2>().y));
    }

    private float root(float a) {
        bool neg = a < 0;
        if (neg) return -1f * (float) Math.Sqrt(-1f * a);
        return (float) Math.Sqrt(a);
    }

    private void PlayerInput_onActionTriggered(InputAction.CallbackContext context) {

        //Debug.Log(context.control.displayName);

    }

    private void index(InputAction.CallbackContext context) {
        
        Vector3 Rdir = RedBallTransform.position - transform.position;
        Vector3 Bdir = BlueBallTransform.position - transform.position;
        float Rang = Vector3.Angle(transform.forward, Rdir);
        float Bang = Vector3.Angle(transform.forward, Bdir);
        float Rdist = Rdir.magnitude;
        float Bdist = Bdir.magnitude;
        if ((Math.Abs(Rang) < 45) && (Rdist < 1)) {
            RedBall.AddForce(transform.forward * -pullPower, ForceMode.Impulse);
        }

        if ((Math.Abs(Bang) < 45) && (Bdist < 1)) {
            BlueBall.AddForce(transform.forward * -pullPower, ForceMode.Impulse);
        }

    }

}

