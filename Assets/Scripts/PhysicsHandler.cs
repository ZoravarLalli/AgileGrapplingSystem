using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all the physics related to grappling and adjusts the player's rigidbody parameters according
/// to the current state in a Finite State Machine for the current Physics state (Reeling/swinging/flying etc).
/// </summary>
public class PhysicsHandler : MonoBehaviour
{
    private Vector3 position;
    private Vector3 momentum;
    private Vector3 velocity;
    private Vector3 acceleration;
    private float mass;
    public Rigidbody rb;
    public Player player;
    [SerializeField]
    public float reelRate;
    [SerializeField]
    public float momentumGainRate;
    [SerializeField]
    public float terminalVelocity;
    [SerializeField]
    public float minReelLength;
    [SerializeField]
    public float swingGravityIncreaseAmount;
    [SerializeField]
    public float maxSwingDistance;
    [SerializeField]
    public float minSwingDistance;
    [SerializeField]
    public float swingSpring;
    [SerializeField]
    public float swingDamper;
    [SerializeField]
    public float swingBoostForce;
    [SerializeField]
    public float swingBoostFactor;
    [SerializeField]
    public float swingDownwardForce;
    [SerializeField]
    public float momentumAdjustmentForce;
    [SerializeField]
    public LineRenderer physicsLineMomentumAdjustment;
    [SerializeField]
    public float joystickForceValue;
    [SerializeField]
    public float joystickDeadzone;
    private Vector2 joystickVal;
    private Transform currPlayerCamTrans;
    private Vector3 camForwardAdjusted;
    private Vector3 camRightAdjusted;



    // Current state to start from and drive Finite State Machine
    PhysicsState currentState;
    // Different PhysicsStates that can be transitioned to within the FSM and set as currentState
    public PhysicsUnlatchedState UnlatchedState = new PhysicsUnlatchedState();
    public PhysicsLatchedState LatchedState = new PhysicsLatchedState();
    public PhysicsReelState ReelState = new PhysicsReelState();
    public PhysicsSwingState SwingState = new PhysicsSwingState();
    public PhysicsFlyingState FlyingState = new PhysicsFlyingState();

    // Set the first state on start
    private void Start()
    {
        currentState = UnlatchedState;
        currentState.EnterState(this);
    }

    // Call the corresponding update function in the current state
    private void FixedUpdate()
    {
        Debug.Log("VELOCITY: " + rb.velocity);
        Debug.Log("curr state: " + currentState);

        // PHYSICS UPDATE LOOP DRIVES STATE MACHINE
        currentState.UpdateState(this);

        // Setting a terminal velocity so player cannot gain an excessive velocity
        // around 100 m/s is when it becomes to fast to react to incoming obstacles
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, terminalVelocity);

        // Base Movement and Force Adjustment control
        // Can use for base movement on ground or to manipulate path of motion while grappling
        joystickVal = player.inputs.GetJoystickValue();
        currPlayerCamTrans = player.GetPlayerCamTransform();
        camForwardAdjusted = new Vector3(currPlayerCamTrans.forward.x, 0, currPlayerCamTrans.forward.z);
        camRightAdjusted = new Vector3(currPlayerCamTrans.right.x, 0, currPlayerCamTrans.right.z);

        // Backward
        if (joystickVal.y < 0 - joystickDeadzone)
        {
            Debug.Log("BACK: " + player.inputs.GetJoystickValue());
            rb.AddForce(-camForwardAdjusted * joystickForceValue);
        }
        if (joystickVal.y > 0 + joystickDeadzone) // Forward
        {
            Debug.Log("FORWARD: " + player.inputs.GetJoystickValue());
            rb.AddForce(camForwardAdjusted * joystickForceValue);
        }
        if (joystickVal.x < 0 - joystickDeadzone) // Left
        {
            Debug.Log("LEFT: " + player.inputs.GetJoystickValue());
            rb.AddForce(-camRightAdjusted * joystickForceValue);
        }
        if (joystickVal.x > 0 + joystickDeadzone) // Right
        {
            Debug.Log("Right: " + player.inputs.GetJoystickValue());
            rb.AddForce(camRightAdjusted * joystickForceValue);
        }




        /*        Debug.Log("Spring: " + swingSpring);
        Debug.Log("Damper: " + swingDamper);
        Debug.Log("Mass: " + rb.mass);
        Debug.Log("maxSwingDistance: " + maxSwingDistance);
        Debug.Log("minSwingDistance: " + minSwingDistance);
        Debug.Log("swingBoost: " + swingBoostForce);*/
        // Testing physc values for swinging live
        /*        if (Input.GetKey(KeyCode.Q))
                {
                    swingSpring++;
                } else if (Input.GetKey(KeyCode.A))
                {
                    swingSpring--;
                }
                if (Input.GetKey(KeyCode.W))
                {
                    swingDamper++;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    swingDamper--;
                }
                if (Input.GetKey(KeyCode.E))
                {
                    rb.mass++;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    rb.mass--;
                }
                if (Input.GetKey(KeyCode.R))
                {
                    maxSwingDistance += 0.01f;
                }
                else if (Input.GetKey(KeyCode.F))
                {
                    maxSwingDistance -= 0.01f;
                }
                if (Input.GetKey(KeyCode.T))
                {
                    minSwingDistance += 0.01f;
                }
                else if (Input.GetKey(KeyCode.G))
                {
                    minSwingDistance -= 0.01f;
                }
                if (Input.GetKey(KeyCode.Y))
                {
                    swingBoostForce++;
                }
                else if (Input.GetKey(KeyCode.H))
                {
                    swingBoostForce--; 
                }*/
    }

    // Set the current active state in the FSM to the specified one
    public void ChangeState(PhysicsState state)
    {
        currentState = state;
        state.EnterState(this);
    }

/*    // Update the forces acting on the player's rigidbody
    // according to the currently active physics state
    public Vector3 UpdateForces()
    {
        ///TODO Update forces function
        return Vector3.zero;
    }

    // Set the momentum direction of the player's rigidbody
    // according to the orientation of the quest's head sensor
    private void SetMomenetumDirection(Vector3 headValues)
    {
        ///TODO Use head orientation values to adjust momentum direction of rigidbody
    }

    private void ApplyReelRate()
    {
        ///TODO Reel rate application done based on button press
    }

    private void ApplySwingForce()
    {
        ///TODO Swing force application done based on button press
    }*/
}
