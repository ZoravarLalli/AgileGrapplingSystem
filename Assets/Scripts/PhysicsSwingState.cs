using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSwingState : PhysicsState
{
    public override void EnterState(PhysicsHandler physics)
    {
        Debug.Log("Entering the SWING STATE");
    }

    public override void UpdateState(PhysicsHandler physics)
    {
        // Check if player contacts the ground while latched and go to latchedState if they do
        if ((physics.player.CheckLeftGrapple() || physics.player.CheckRightGrapple()) && physics.player.IsGrounded())
        {
            // Grounded transition to LatchedState
            physics.ChangeState(physics.LatchedState);
        }
        // Check if player is latched and reeling with either of the grapples and go to reelState if they are
        else if ((physics.player.CheckLeftGrapple() && physics.player.CheckLeftReelInput()) || (physics.player.CheckRightGrapple() && physics.player.CheckRightReelInput()))
        {
            // Reel input transition to reelState
            physics.ChangeState(physics.ReelState);
        }
        // Check if player is unlatched and grounded and go to UnlatchedState if they are
        else if ((!physics.player.CheckLeftGrapple() && !physics.player.CheckRightGrapple()) && physics.player.IsGrounded())
        {
            // Cease grapple input & grounded transition to UnlatchedState
            physics.ChangeState(physics.UnlatchedState);
        }
        // Check if player is unlatched and not grounded and go to FlyingState if they are
        else if ((!physics.player.CheckLeftGrapple() && !physics.player.CheckRightGrapple()) && !physics.player.IsGrounded())
        {
            // Cease grapple input & grounded transition to FlyingState
            physics.ChangeState(physics.FlyingState);
        }
    }
}
