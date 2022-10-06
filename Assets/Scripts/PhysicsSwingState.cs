using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSwingState : PhysicsState
{
    private SpringJoint leftSwingJoint;
    private SpringJoint rightSwingJoint;
    private Vector3 leftLatchPoint;
    private Vector3 rightLatchPoint;
    private float leftSwingDistance;
    private float rightSwingDistance;

    // Helper function to determine whether grapple latches are too far apart
    // for the player to swing from a single pivot point or not and to check that
    // they are grappling to the same object. (can't swing from two things at once)
    private bool VerifyDoubleLatch()
    {
        ///TODO Need to define logic to determine double latch okay or not
        // for now returning true regardless
        return true;
    }

    public override void EnterState(PhysicsHandler physics)
    {
        //Debug.Log("Entering the SWING STATE");

    }

    
    public override void UpdateState(PhysicsHandler physics)
    {
        // SWING FUNCTIONALITY
        // Use side grips to intiate swinging from an attached grappled while in the swing state
        // Using springjoints to swing player's rigid body about the latchpoint like a pendulum
        // It should feel intuitive since gripping onto a rope you want to swing from is a natural motion
        // If either swing input is active then process swinging and apply springjoints 
        if (physics.player.CheckLeftSwingInput() || physics.player.CheckRightSwingInput()) 
        {
            // left swing
            if (physics.player.CheckLeftGrapple() && physics.player.CheckLeftSwingInput())
            {
                // Utilizing SpringJoint to swing from latch point like a pendulum
                leftLatchPoint = physics.player.GetLeftLatchPoint();

                // Only attach a springJoint one is not already created and attached
                if (physics.gameObject.GetComponent<SpringJoint>() == null)
                {
                    // Maybe shouldnt set on xr rig object, testing for now
                    leftSwingJoint = physics.gameObject.AddComponent<SpringJoint>();
                    // have to specify when you are providing an anchor point yourself before doing so
                    leftSwingJoint.autoConfigureConnectedAnchor = false;
                    leftSwingJoint.connectedAnchor = leftLatchPoint;

                    // Want to save the distance when we begin swing as the joint's min max length is based on this
                    // Adjusting these set a tolerance for how much the two anchor objects can be apart before the spring
                    // force begins being applied.
                    leftSwingDistance = Vector3.Distance(physics.rb.position, leftLatchPoint);
                    //leftSwingJoint.minDistance = leftSwingDistance * physics.minSwingDistance;
                    //leftSwingJoint.maxDistance = leftSwingDistance * physics.minSwingDistance;
                    leftSwingJoint.minDistance = leftSwingDistance;
                    leftSwingJoint.maxDistance = leftSwingDistance;
                    // Spring float control the force of anchors pulling toward each other want a low spring to create an arc. 
                    // Entering swing with downward momentum then letting go on upward path to release is what results in a
                    // satisfying swing and release combo.
                    leftSwingJoint.spring = physics.swingSpring;
                    // Damper float will control the level that the objects at end of anchors oscilate back and forth on spring
                    // Want a high damper so player is swung around by latch point, want stable swinging with low oscilation
                    leftSwingJoint.damper = physics.swingDamper;
                }
                // Allow line to get shorter but not longer during a swing if they reel in while swinging
                leftSwingJoint.maxDistance = leftSwingDistance;
                // Lastly add a small force in the direction of the pivot to aid in keeping the player's motion on a smooth arc
                // This is not the same as the spring force necessarily as the origin of the force is on the player rigidbody
                physics.rb.AddForce(physics.player.GetLeftReelDirection().normalized * (physics.swingBoostForce * physics.swingBoostFactor));
            }
            // right swing
            if (physics.player.CheckRightGrapple() && physics.player.CheckRightSwingInput())
            {
                rightLatchPoint = physics.player.GetRightLatchPoint();

                // Only attach a springJoint one is not already created and attached
                if (physics.gameObject.GetComponent<SpringJoint>() == null)
                {
                    rightSwingJoint = physics.gameObject.AddComponent<SpringJoint>();
                    rightSwingJoint.autoConfigureConnectedAnchor = false;
                    rightSwingJoint.connectedAnchor = rightLatchPoint;
                    rightSwingDistance = Vector3.Distance(physics.rb.position, rightLatchPoint);
                    //rightSwingJoint.minDistance = rightSwingDistance * physics.minSwingDistance;
                    rightSwingJoint.minDistance = rightSwingDistance;
                    rightSwingJoint.maxDistance = rightSwingDistance;
                    rightSwingJoint.spring = physics.swingSpring;
                    rightSwingJoint.damper = physics.swingDamper;
                }
                
                rightSwingJoint.maxDistance = rightSwingDistance;
                physics.rb.AddForce(physics.player.GetLeftReelDirection().normalized * (physics.swingBoostForce * physics.swingBoostFactor));
            }

            // ALLOW REELING WHILE IN A SPRINGJOINT SWING
            // Unplanned functionality but I realized that if using reel along with swing, then an additional
            // aspect of control for increasing the swing speed and depth is achieved
            if ((physics.player.GetLeftGrappleLength() > physics.minReelLength) && physics.player.CheckLeftReelInput())
            {
                // Normalize the force, I don't know if magnitude affects this or not but normalizing incase
                physics.rb.AddForce(physics.player.GetLeftReelDirection().normalized * physics.swingBoostForce);
            }
            if ((physics.player.GetRightGrappleLength() > physics.minReelLength) && physics.player.CheckRightReelInput())
            {
                physics.rb.AddForce(physics.player.GetRightReelDirection().normalized * physics.swingBoostForce);
            }

        }

        // Make sure to disable the springjoint swinging effect if player stops gripping, only active while gripping
        if (!physics.player.CheckLeftSwingInput())
        {
            SpringJoint.Destroy(leftSwingJoint);
        }
        if (!physics.player.CheckRightSwingInput())
        {
            SpringJoint.Destroy(rightSwingJoint);
        }

        ///TODO SWING ADJUSTMENT FUNCTIONALITY
        // Add a force to manipulate the player's swing path while attached and swinging
        // using the same force application method as flying state to strafe slightly through
        // air according to headset's look direction.


        // STATE TRANSITIONS
        // Check if player contacts the ground while latched and go to latchedState if they do
        if ((physics.player.CheckLeftGrapple() || physics.player.CheckRightGrapple()) && physics.player.IsGrounded())
        {
            // Grounded transition to LatchedState
            physics.ChangeState(physics.LatchedState);
            SpringJoint.Destroy(leftSwingJoint);
            SpringJoint.Destroy(rightSwingJoint);
        }
        // Check if player is latched and reeling with either of the grapples and go to reelState if they are
        else if ((physics.player.CheckLeftGrapple() && physics.player.CheckLeftReelInput()) || (physics.player.CheckRightGrapple() && physics.player.CheckRightReelInput()))
        {
            // Only go to reel state if they cease swing input as well
            if(!physics.player.CheckLeftSwingInput() && !physics.player.CheckRightSwingInput())
            {
                // Reel input transition to reelState
                physics.ChangeState(physics.ReelState);
                SpringJoint.Destroy(leftSwingJoint);
                SpringJoint.Destroy(rightSwingJoint);
            }
        }
        // Check if player is unlatched and grounded and go to UnlatchedState if they are
        else if ((!physics.player.CheckLeftGrapple() && !physics.player.CheckRightGrapple()) && physics.player.IsGrounded())
        {
            // Cease grapple input & grounded transition to UnlatchedState
            physics.ChangeState(physics.UnlatchedState);
            SpringJoint.Destroy(leftSwingJoint);
            SpringJoint.Destroy(rightSwingJoint);
        }
        // Check if player is unlatched and not grounded and go to FlyingState if they are
        else if ((!physics.player.CheckLeftGrapple() && !physics.player.CheckRightGrapple()) && !physics.player.IsGrounded())
        {
            // Cease grapple input & grounded transition to FlyingState
            physics.ChangeState(physics.FlyingState);
            SpringJoint.Destroy(leftSwingJoint);
            SpringJoint.Destroy(rightSwingJoint);
        }
    }
}


// ATTEMPTED SWING FUNCTIONALITY
// Has a default swing force once in this state
// Use side grip input to increase the gravity of physics
// this was an idea I had to give an additional aspect of swing
// control by increasing gravity force to speed up swinging, ideally a player will use
// this to gain downward momentum and then release at the bottom of a swing to maximize
// upward velocity when leaving bottom and going up again
/*public override void UpdateState(PhysicsHandler physics)
{
    // SWING FUNCTIONALITY
    // Has a default swing force once in this state
    // Use side grip input to increase the gravity of physics
    // this was an idea I had to give an additional aspect of swing
    // control by increasing gravity force to speed up swinging, ideally a player will use
    // this to gain downward momentum and then release at the bottom of a swing to maximize
    // upward velocity when leaving bottom and going up again

    // It should feel intuitive since gripping and leaning into a rope you want to swing faster from is a natural motion

    // if in this state should apply base swing force regardless as we are falling while latch is attached
    ///TODO consider double aswell: Double swing case
    *//*if (physics.player.CheckLeftGrapple())
    {
        physics.rb.AddForce(physics.player.GetLeftReelDirection().normalized * physics.swingRate);
    }
    if (physics.player.CheckRightGrapple())
    {
        physics.rb.AddForce(physics.player.GetRightReelDirection().normalized * physics.swingRate);
    }*//*

    if (physics.player.CheckLeftSwingInput() || physics.player.CheckRightSwingInput()) // If either swing input is active
    {
        // Double swing case, need to consider bad latching and mitigate the scenario.
        // Determine whether one or both grapples are attempting swinging, and also verify that both are latched
        if ((physics.player.CheckLeftGrapple() && physics.player.CheckRightGrapple()) && (physics.player.CheckLeftSwingInput() && physics.player.CheckRightSwingInput()))
        {
            // Check for bad latching (Latches too far apart to treat two latches near one another as a single pivot point for swing)
            if (VerifyDoubleLatch())
            {
                // Find the midway point between the two latch points on the collided surface and use that as the pivot of the swing
            }
        }
        // DISCRETE INPUT VERSION
        // left swing case
        else if (physics.player.CheckLeftGrapple() && physics.player.CheckLeftSwingInput())
        {

            // Only ever applying artificial gravity increase when the input is held
            physics.rb.AddForce(Physics.gravity * physics.swingGravityIncreaseAmount, ForceMode.Acceleration);
            physics.rb.AddForce(physics.player.GetLeftReelDirection().normalized * physics.swingRate);
            // Apply an artificial force towards the grapple line to combine with the increasing gravity
            // The resulting combined movement should be a parabola or curve downward
            // Done constantly according to latch now
            // May need code to make swing go back up

            // if that approach don't work will need to lock grapple length during swing and calculate the angular motion 
            // and treat player as mass on end of pivoting 
        }
        // right swing case
        else if (physics.player.CheckRightGrapple() && physics.player.CheckRightSwingInput())
        {
            physics.rb.AddForce(Physics.gravity * physics.swingGravityIncreaseAmount, ForceMode.Acceleration);
            physics.rb.AddForce(physics.player.GetRightReelDirection().normalized * physics.swingRate);
        }
        // CONTINUOUS INPUT VERSION
        // left swing case
        *//*else if (physics.player.CheckLeftGrapple() && physics.player.CheckLeftSwingInput())
        {
            // Only ever applying artificial gravity increase when the input is held
            physics.rb.AddForce(Physics.gravity * physics.swingGravityIncreaseAmount * physics.player.GetLeftSwingAmount(), ForceMode.Acceleration);
        }
        // right swing case
        else if (physics.player.CheckRightGrapple() && physics.player.CheckRightSwingInput())
        {
            physics.rb.AddForce(Physics.gravity * physics.swingGravityIncreaseAmount * physics.player.GetLeftSwingAmount(), ForceMode.Acceleration);
        }*//*
    }*/