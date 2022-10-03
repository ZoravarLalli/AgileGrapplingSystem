using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSwingState : PhysicsState
{
    private SpringJoint leftSwingJoint;
    private SpringJoint rightSwingJoint;
    private Vector3 leftLatchPoint;
    private Vector3 rightLatchPoint;
    private float swingDistance;

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

    // SWING FUNCTIONALITY
    // Use side grips to intiate swinging from an attached grappled while in the swing state
    // Using springjoints to swing player rigid body about the latchpoint like a pendulum
    // I attempted to develop other methods however, this seems to be the best way to attempt swings in unity
    // It should feel intuitive since gripping onto a rope you want to swing from is a natural motion
    public override void UpdateState(PhysicsHandler physics)
    {
        // If either swing input is active
        if (physics.player.CheckLeftSwingInput() || physics.player.CheckRightSwingInput()) 
        {
            // Double swing case, need to consider bad latching and mitigate the scenario.
            // Determine whether one or both grapples are attempting swinging, and also verify that both are latched
            // Leaving as stretch goal for now, going to encourage use of one swinging grapple at a time
            if ((physics.player.CheckLeftGrapple() && physics.player.CheckRightGrapple()) && (physics.player.CheckLeftSwingInput() && physics.player.CheckRightSwingInput()))
            {
                // Check for bad latching (Latches too far apart to treat two latches near one another as a single pivot point for swing)
                if (VerifyDoubleLatch())
                {
                    // Find the midway point between the two latch points on the collided surface and use that as the pivot of the swing
                }
            }
            // left swing case
            else if (physics.player.CheckLeftGrapple() && physics.player.CheckLeftSwingInput())
            {
                // Utilizing SpringJoint to swing from latch point like a pendulum
                leftLatchPoint = physics.player.GetLeftLatchPoint();
                // Maybe shouldnt set on xr rig object, testing for now
                leftSwingJoint = physics.gameObject.AddComponent<SpringJoint>();
                // have to specify when you are providing an anchor point yourself before doing so
                leftSwingJoint.autoConfigureConnectedAnchor = false; 
                leftSwingJoint.connectedAnchor = leftLatchPoint;

                // Want to save the distance when we begin swing as the joint's min max length is based on this
                swingDistance = Vector3.Distance(physics.rb.position, leftLatchPoint);
                leftSwingJoint.maxDistance = swingDistance * physics.maxSwingDistance;
                leftSwingJoint.minDistance = swingDistance * physics.minSwingDistance;

                // Values to play with to achieve different feeling swings
                leftSwingJoint.spring = physics.swingSpring;
                leftSwingJoint.damper = physics.swingDamper;
                leftSwingJoint.massScale = physics.swingMassScale;
            }
            // right swing case
            else if (physics.player.CheckRightGrapple() && physics.player.CheckRightSwingInput())
            {
                rightLatchPoint = physics.player.GetRightLatchPoint();
                rightSwingJoint = physics.gameObject.AddComponent<SpringJoint>();
                rightSwingJoint.autoConfigureConnectedAnchor = false;
                rightSwingJoint.connectedAnchor = rightLatchPoint;
                swingDistance = Vector3.Distance(physics.rb.position, rightLatchPoint);
                rightSwingJoint.maxDistance = swingDistance * physics.maxSwingDistance;
                rightSwingJoint.minDistance = swingDistance * physics.minSwingDistance;

                // Values to 
                rightSwingJoint.spring = physics.swingSpring;
                rightSwingJoint.damper = physics.swingDamper;
                //rightSwingJoint.massScale = physics.swingMassScale;
            }
        }

        ///TODO SWING ADJUSTMENT FUNCTIONALITY
        // Add force to increase or decrease the player's swing angle while attached and swinging
        // OPTION 1 (Could result in unintuitive feeling control):
        // Force will be applied depending on the distance between headset and controller sensors
        // and push opposite of the direction the headset is closer to currently.
        // OPTION 2 (Simpler but more intuitive):
        // Force will be applied depending on the headset's tilt orientation (rotation about the Z-axis)



        // STATE TRANSITIONS
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