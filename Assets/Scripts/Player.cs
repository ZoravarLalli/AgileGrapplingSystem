using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define an enum to represent different states for player in physics FSM
public enum PlayerState
{
    UNLATCHED,
    LATCHED,
    SWING,
    REEL,
    AIR
}

// Holds references to gameobjects relevant to player like grapples and rigidbodies

// Contains state machine for player state

// Contains player forces but these forces are controlled and updated by a helper class PhysicsHandler

// QuestRig was initially named VRInputHandler and going to be a helper class of Player but since
// input handling from quest hardware was more complex than expected I changed it be a separate script
// since it required its own helper classes to help process input handling.
public class Player : MonoBehaviour
{
    private GameObject leftGrapple;
    private GameObject rightGrapple;
    private Transform transform;
    private Rigidbody rb;
    private Camera playerCamera;
    private Vector3 forces; // maybe should be a matrix instead
    // Current active playerState, assign starting enum in start
    private PlayerState playerState;

    public void ChangeState(PlayerState state)
    {
        playerState = state;
        ///TODO Add logic for if states can be changed or not depending on curr state and requested state in param. Maybe the full FSM logic should go in here?
    }

    // Need to be able to identify angle of control inputs from questrig?
    // or from openXR adjusted code

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

[System.Serializable]
// Helper class for handling physics 
public class PhysicsHandler
{
    // Physics values to be tracked from player's rigid body
    private Vector3 position;
    private Vector3 momentum;
    private Vector3 velocity;
    private Vector3 acceleration;
    private float mass;

    // Returns the force values to update player's forces according to physics calculations done frame by frame.
    public Vector3 UpdateForces()
    {
        ///TODO Define force update based on physics calc here
        return Vector3.zero;
    }
}

