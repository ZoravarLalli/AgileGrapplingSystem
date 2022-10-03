using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsFlyingState : PhysicsState
{
    public override void EnterState(PhysicsHandler physics)
    {
        //Debug.Log("Entering the FLYING STATE");
        physics.physicsLineMomentumAdjustment.enabled = true;
    }

    public override void UpdateState(PhysicsHandler physics)
    {
        // MOMENTUM DIRECTION ADJUSTMENT FUNCTIONALITY
        // Use the player's headset orientation (rotation about the Y-axis) to adjust the direction
        // of the rigidbody's current velocity to provide the ability to intuitively strafe and adjust 
        // path of travel while flying through the air

        // Want to add a small force in the direction of current velocity direction + adjusted y value from headset
        // Save the direction of the current velocity
        //Vector3 currentDir = physics.rb.velocity.normalized;
        // Now get the direction to change to according to the headset orientation
        //Vector3 headsetEuler = physics.player.GetHeadEulerAngle();

        /*        Vector3 currPos = currPlayerTrans.position;
                Vector3 currRot = currPlayerTrans.rotation.eulerAngles;
                Vector3 normalizeTest = currRot.normalized;*/
        // Only want to affect the Y-component of the current velocity vector according to headset values
        // so assemble new vector using x, z of original and y of headset
        // Vector3 desiredDirectionAdjustment = new Vector3(currentDir.x, headsetEuler.y, currentDir.z).normalized;
        //Vector3 desiredDirectionAdjustment = new Vector3(0, headsetEuler.y, 0);

        // COULD NOT USE RAW POSITION DATA IN CALCULATIONS, SOMETHING GOING WRONG CONVERTING EULER ANGLES TO DIRECTION VECTOR
        // Instead using the forward of a transform that is controlled by the headset's orientation... Should result in same effect
        //Transform currPlayerCamTrans = physics.player.GetPlayerCamTransform();
        // Magnitude of adjustment force factor is small compared to other forces from grappling, mean't to nudge existing trajectory
        //Debug.Log("Curr Forward Vector: " + currPlayerCamTrans.forward);
        
        //Vector3 camForwardOnY = new Vector3(0, 0, currPlayerCamTrans.forward.z);
        //Debug.Log("Curr Force: " + (camForwardOnY * physics.momentumAdjustmentForce));
        // Want to instantaneously change direction regardless of mass based on look direction
        //physics.rb.AddForce(camForwardOnY * physics.momentumAdjustmentForce, ForceMode.VelocityChange);

        // Debugging force
        //physics.physicsLineMomentumAdjustment.SetColors(Color.yellow, Color.yellow);
        //physics.physicsLineMomentumAdjustment.SetPosition(0, physics.player.transform.position);
        //physics.physicsLineMomentumAdjustment.SetPosition(1, physics.player.transform.position + (camForwardOnY * physics.momentumAdjustmentForce));

        // STATE TRANSITIONS
        // Check if the player is grounded and go to unlatchedState if they are
        if (physics.player.IsGrounded())
        {
            // Grounded transition to unlatchedState
            physics.ChangeState(physics.UnlatchedState);
            physics.physicsLineMomentumAdjustment.enabled = false;
        }
        // Check if the player has grapple latched while reeling and not grounded
        else if((physics.player.CheckLeftGrapple() && physics.player.CheckLeftReelInput()) || (physics.player.CheckRightGrapple() && physics.player.CheckRightReelInput()))
        {
            // Grapple shot & reel transition to ReelState
            physics.ChangeState(physics.ReelState);
            physics.physicsLineMomentumAdjustment.enabled = false;
        }
        // Check if the player has either grapple latched without reeling and not grounded -> flying state
        else if ((physics.player.CheckLeftGrapple() || physics.player.CheckRightGrapple()) && (!physics.player.CheckLeftReelInput() && !physics.player.CheckRightReelInput()))
        {
            // Grapple shot & no reel transition to SwingState
            physics.ChangeState(physics.SwingState);
            physics.physicsLineMomentumAdjustment.enabled = false;
        }
    }
}
