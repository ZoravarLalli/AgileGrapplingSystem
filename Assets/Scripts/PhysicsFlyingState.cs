using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsFlyingState : PhysicsState
{
    Transform currPlayerCamTrans;
    Vector3 camForwardAdjusted;
    public override void EnterState(PhysicsHandler physics)
    {
        //Debug.Log("Entering the FLYING STATE");
        //physics.physicsLineMomentumAdjustment.enabled = true;
    }

    public override void UpdateState(PhysicsHandler physics)
    {
        // MOMENTUM DIRECTION ADJUSTMENT FUNCTIONALITY
        // Use the player's headset orientation (according to camera transform's forward vector) to adjust 
        // the direction of the rigidbody's current velocity to provide the ability to nudge player slightly
        // towards the direction they are looking in with a small force
        currPlayerCamTrans = physics.player.GetPlayerCamTransform();
        
        // Use the normalized directional components for the x and z for turning only, don't want to adjust y with this.
        camForwardAdjusted = new Vector3(currPlayerCamTrans.forward.x, 0, currPlayerCamTrans.forward.z);
        physics.rb.AddForce(camForwardAdjusted * physics.momentumAdjustmentForce);

        // Visual Debugging for force vector
        physics.physicsLineMomentumAdjustment.SetColors(Color.yellow, Color.yellow);
        physics.physicsLineMomentumAdjustment.SetPosition(0, physics.rb.transform.position);
        physics.physicsLineMomentumAdjustment.SetPosition(1, physics.rb.transform.position + (camForwardAdjusted * physics.momentumAdjustmentForce));

        // STATE TRANSITIONS
        // Check if the player is grounded and go to unlatchedState if they are
        if (physics.player.IsGrounded())
        {
            // Grounded transition to unlatchedState
            physics.ChangeState(physics.UnlatchedState);
            //physics.physicsLineMomentumAdjustment.enabled = false;
        }
        // Check if the player has grapple latched while reeling and not grounded
        else if((physics.player.CheckLeftGrapple() && physics.player.CheckLeftReelInput()) || (physics.player.CheckRightGrapple() && physics.player.CheckRightReelInput()))
        {
            // Grapple shot & reel transition to ReelState
            physics.ChangeState(physics.ReelState);
            //physics.physicsLineMomentumAdjustment.enabled = false;
        }
        // Check if the player has either grapple latched without reeling and not grounded -> flying state
        else if ((physics.player.CheckLeftGrapple() || physics.player.CheckRightGrapple()) && (!physics.player.CheckLeftReelInput() && !physics.player.CheckRightReelInput()))
        {
            // Grapple shot & no reel transition to SwingState
            physics.ChangeState(physics.SwingState);
            //physics.physicsLineMomentumAdjustment.enabled = false;
        }
    }
}
