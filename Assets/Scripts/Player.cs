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
    private Camera playerCamera;
    private Vector3 forces; // maybe should be a matrix instead
    [SerializeField]
    // Instance of physics handler to use in conjunction with state machine
    private PhysicsHandler physics;
    // Using an inputmanager class to be able to access live input data easily
    // can be public as it is just a collection of getters 
    // and methods to provide the returns of the getters
    public InputManager inputs;
    private bool isGrounded;
    private int collisionCount; // use to track whether currently colliding with anything or not

    // Need to be able to identify angle of control inputs from questrig?
    // or from openXR adjusted code

    // Start is called before the first frame update
    void Awake()
    {
        collisionCount = 0;
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("rb vel: " + rb.velocity);
    }

    public bool CheckLeftGrapple()
    {
        return leftGrapple.latched;
    }

    public bool CheckRightGrapple()
    {
        return rightGrapple.latched;
    }
    public Vector3 GetLeftLatchPoint()
    {
        return leftGrapple.GetLatchPosition();
    }

    public Vector3 GetRightLatchPoint()
    {
        return rightGrapple.GetLatchPosition();
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionCount++;
        // Stop from sliding when hitting surface at speed
        physics.rb.velocity = Vector3.zero;
        physics.rb.angularVelocity = Vector3.zero;
        //Debug.Log("count up: " + collisionCount);
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionCount--;
        //Debug.Log("count up: " + collisionCount);
    }

    public bool IsGrounded()
    {
        // When not grounded the player is not colliding with any ground collider
        // right now defining based on any collider, later may constrain to ground colliders
        // Only ground and building to collide to anyways
        if(collisionCount == 0)
        {
            return false; // Not grounded, falling
        }
        else
        {
            return true; // Grounded, in contact with surface
        }
    }

    // Checks if reel input is being fired
    public bool CheckLeftReelInput()
    {
        return inputs.GetLeftReelInput();
    }
    public bool CheckRightReelInput()
    {
        return inputs.GetRightReelInput();
    }
    // Returns length of line from grapple gun shot origin to grapple latch point
    public float GetLeftGrappleLength()
    {
        return leftGrapple.distanceToGun();
    }
    public float GetRightGrappleLength()
    {
        return rightGrapple.distanceToGun();
    }
    // Returns the direction that a player should be reeled in according to left and right grapple/latch orientation
    public Vector3 GetLeftReelDirection()
    {
        return leftGrapple.GetLatchDirection();
    }
    public Vector3 GetRightReelDirection()
    {
        return rightGrapple.GetLatchDirection();
    }
    // Checks if swing input is being fired
    public bool CheckLeftSwingInput()
    {
        return inputs.GetLeftSwingInput();
    }
    public bool CheckRightSwingInput()
    {
        return inputs.GetRightSwingInput();
    }
    // Get the amount the swing input is pressed
    public float GetLeftSwingAmount()
    {
        return inputs.GetLeftSwingInputAmount();
    }
    public float GetRightSwingAmount()
    {
        return inputs.GetRightSwingInputAmount();
    }
    // Get the head rotation in Euler Angle vector
    public Transform GetPlayerCamTransform()
    {
        //return inputs.GetHeadEulerRotation();\
        // I'm having issues using the position headset of the data in calculations for momentum adjustment
        // however the headset is correctly manipulating the player mesh in game, so I am going to try and indirectly
        // capture headset orientation through the sensor controlled mesh's attributes.
        return playerCamera.transform;
    }

}

