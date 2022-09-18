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
    public Player player;

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
        Debug.Log("1 state: " + UnlatchedState);
        currentState = UnlatchedState;
        Debug.Log("2 state: " + currentState);
        currentState.EnterState(this);
    }

    // Call the corresponding update function in the current state
    private void Update()
    {
        Debug.Log("curr state: " + currentState);
        currentState.UpdateState(this);
    }

    // Set the current active state in the FSM to the specified one
    public void ChangeState(PhysicsState state)
    {
        currentState = state;
        state.EnterState(this);
    }


    // Update the forces acting on the player's rigidbody
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
    }
}
