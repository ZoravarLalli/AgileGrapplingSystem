using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsFlyingState : PhysicsState
{
    public override void EnterState(PhysicsHandler physics)
    {
        Debug.Log("Entering the FLYING STATE");
    }

    public override void UpdateState(PhysicsHandler physics)
    {
        // MOMENTUM DIRECTION ADJUSTMENT FUNCTIONALITY
        // Use the player's headset orientation (rotation about the Y-axis) to adjust the
        // momentum direction of the player to provide the ability to strafe and adjust path
        // of travel while flying through the air


        // STATE TRANSITIONS
        // Check if the player is grounded and go to unlatchedState if they are
        if (physics.player.IsGrounded())
        {
            // Grounded transition to unlatchedState
            physics.ChangeState(physics.UnlatchedState);
        }
        // Check if the player has grapple latched while reeling and not grounded
        else if((physics.player.CheckLeftGrapple() && physics.player.CheckLeftReelInput()) || (physics.player.CheckRightGrapple() && physics.player.CheckRightReelInput()))
        {
            // Grapple shot & reel transition to ReelState
            physics.ChangeState(physics.ReelState);
        }
        // Check if the player has either grapple latched without reeling and not grounded -> flying state
        else if ((physics.player.CheckLeftGrapple() || physics.player.CheckRightGrapple()) && (!physics.player.CheckLeftReelInput() && !physics.player.CheckRightReelInput()))
        {
            // Grapple shot & no reel transition to SwingState
            physics.ChangeState(physics.SwingState);
        }
    }
}
