using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE:
// if not fully reeled in
// calculateReel function will determine the rate of reeling depending on 
// existing forces from the player (if we have forces in the direction of 
// reeling they will be added onto and accounted for)
// UPDATE:
// I realized that as long as we use the rigidbody correctly and add forces through unity's physics engine then adding onto
// prior physics will be handled implicitly, just don't deviate from adding forces through unity's addforce methods on their rigidbodies.

public class PhysicsReelState : PhysicsState
{
    /*LineRenderer testingLine;
    testingLine = physics.gameObject.AddComponent<LineRenderer>();
            testingLine.SetPosition(0, physics.player.position);
            testingLine.SetPosition(1, hit.point); // careful it will stay if not cleared.
            testingLine.SetColors(Color.green, Color.green);*/

    public override void EnterState(PhysicsHandler physics)
    {
        //Debug.Log("Entering the REEL STATE");
    }

    public override void UpdateState(PhysicsHandler physics)
    {
        // REELING FUNCTIONALITY
        // Check which grapples are reeling and if they are then reel the player towards
        // the grapples (add force in the direction of the grapple latch point from player
        // position) until we get to min reel length.
        if (physics.player.GetLeftGrappleLength() > physics.minReelLength) 
        {
            // Normalize the force, I don't know if magnitude affects this or not but normalizing incase
            physics.rb.AddForce(physics.player.GetLeftReelDirection().normalized * physics.reelRate);
        }
        if (physics.player.GetRightGrappleLength() > physics.minReelLength)
        {
            physics.rb.AddForce(physics.player.GetRightReelDirection().normalized * physics.reelRate);
        }

        // STATE TRANSITIONS
        // If grapples fully released and not grounded then go to flyingState
        if ((!physics.player.CheckLeftGrapple() && !physics.player.CheckRightGrapple()) && !physics.player.IsGrounded())
        {
            // Grapple released & !grounded transition to FlyingState
            physics.ChangeState(physics.FlyingState);
        }
        // If latches are attached, reeling is ceased and player is grounded then return to latchedState
        else if (((physics.player.CheckLeftGrapple() || physics.player.CheckRightGrapple()) && (!physics.player.CheckLeftReelInput() && !physics.player.CheckRightReelInput())) && physics.player.IsGrounded())
        {
            // Release reel input & grounded transition to latchedState
            physics.ChangeState(physics.LatchedState);
        }
        // If latches are attached, reeling is ceased and player is not grounded then go to swingState
        else if ((((physics.player.CheckLeftGrapple() || physics.player.CheckRightGrapple()) 
            && (!physics.player.CheckLeftReelInput() && !physics.player.CheckRightReelInput()))) && !physics.player.IsGrounded())
        {
            // Release reel input & !grounded transition to swingState
            physics.ChangeState(physics.SwingState);
        }
        // Check if player is unlatched and grounded and go to UnlatchedState if they are can probably just remove this state and let this get handled from latched transition
        else if ((!physics.player.CheckLeftGrapple() && !physics.player.CheckRightGrapple()) && physics.player.IsGrounded())
        {
            // Cease grapple input & grounded transition to UnlatchedState
            physics.ChangeState(physics.UnlatchedState);
        }
    }
}
