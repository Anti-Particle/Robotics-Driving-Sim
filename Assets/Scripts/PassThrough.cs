using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PassThrough : MonoBehaviour
{
    GameObject robotObject;
    private Transform transform;
    private Transform botTransform;
    private Rigidbody robot;
    private Rigidbody wheel;
    private PlayerInput input;
    private InputActionMap actionMap;
    private InputAction rev;
    private Vector3 relDir;
    private float relAng;
    public float RevMult;

    // Start is called before the first frame update
    void Awake()
    {   
        robotObject = GameObject.Find("Robot");
        transform = GetComponent<Transform>();
        botTransform = robotObject.GetComponent<Transform>();
        wheel = GetComponent<Rigidbody>();
        robot = robotObject.GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        actionMap = input.currentActionMap;
        rev = actionMap.FindAction("Intake");

        relDir = transform.position - botTransform.position;
        relAng = Vector3.Angle(botTransform.forward, relDir);
    }

    // Update is called once per frame
    void Update()
    {
        wheel.AddTorque(transform.up * RevMult * rev.ReadValue<float>());
        Debug.Log(wheel.angularVelocity);
        transform.position = (botTransform.position + (botTransform.forward * relDir.z) + (botTransform.up * relDir.y) + (botTransform.right * relDir.x));
    }

    void LateUpdate()
    {
        
    }
}
