using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsLatchedState : PhysicsState
{
    public override void EnterState(PhysicsHandler physics)
    {
        //Debug.Log("Entering LATCHED STATE");
    }

    public override void UpdateState(PhysicsHandler physics)
    {
        // Physics is implicitly handled via default rigidbody physics in this state
        // No additional constraints required while in this state

        // STATE TRANSITIONS
        // Check to see if all grapples are released
        if (!physics.player.CheckLeftGrapple() && !physics.player.CheckRightGrapple())
        {
            // Grapple released transition to Unlatched State
            physics.ChangeState(physics.UnlatchedState);
        }
        // Check for an attached grapple and corresponding reeling input -> ReelState
        // going to make it so reel inputs override swing inputs
        else if ((physics.player.CheckLeftGrapple() && physics.player.CheckLeftReelInput()) || (physics.player.CheckRightGrapple() && physics.player.CheckRightReelInput()))
        {
            // Reel input transition to ReelState
            physics.ChangeState(physics.ReelState);
        }
        // Go into a swinging state if no longer on ground and not reeling but still latched
        else if ((physics.player.CheckLeftGrapple() || physics.player.CheckRightGrapple()) && !physics.player.IsGrounded())
        {
            // Latched with no ground underneath transition to SwingState
            physics.ChangeState(physics.SwingState);
        }
    }
}
