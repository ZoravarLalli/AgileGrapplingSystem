using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsLatchedState : PhysicsState
{
    public override void EnterState(PhysicsHandler physics)
    {
        Debug.Log("Entering LATCHED STATE");
    }

    public override void UpdateState(PhysicsHandler physics)
    {
        // Check to see if all grapples are released
        if(!physics.player.CheckLeftGrapple() && !physics.player.CheckRightGrapple())
        {
            // Grapple released transition to Unlatched State
            physics.ChangeState(physics.UnlatchedState);
        }
        // Check for reeling input
        /*else if ()
        {

        }*/
    }
}
