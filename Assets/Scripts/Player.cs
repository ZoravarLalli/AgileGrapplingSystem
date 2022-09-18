using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds references to gameobjects relevant to player like grapples and rigidbodies

// Contains state machine for player state

// Contains player forces but these forces are controlled and updated by a helper class PhysicsHandler

// QuestRig was initially named VRInputHandler and going to be a helper class of Player but since
// input handling from quest hardware was more complex than expected I changed it be a separate script
// since it required its own helper classes to help process input handling.
public class Player : MonoBehaviour
{
    [SerializeField]
    private GrappleGun leftGrapple;
    [SerializeField]
    private GrappleGun rightGrapple;
    [SerializeField]
    private Transform transform;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Camera playerCamera;
    private Vector3 forces; // maybe should be a matrix instead
    [SerializeField]
    // Instance of physics handler to use in conjunction with state machine
    private PhysicsHandler physics;

    // Need to be able to identify angle of control inputs from questrig?
    // or from openXR adjusted code

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckLeftGrapple()
    {
        return leftGrapple.latched;
    }

    public bool CheckRightGrapple()
    {
        return rightGrapple.latched;
    }
}

