using UnityEngine;

public class PhysicsUnlatchedState : PhysicsState
{
    
    public override void EnterState(PhysicsHandler physics)
    {
        Debug.Log("Entering UNLATCHED STATE");
    }

    public override void UpdateState(PhysicsHandler physics)
    {
        // Check to see if we have shot either of the grapples and latched
        if (physics.player.CheckLeftGrapple() || physics.player.CheckRightGrapple())
        {
            // Grapple shot transition
            // Since at least one of the grapples was shot and is latched, switch to latched state
            physics.ChangeState(physics.LatchedState);
        }
        // Check if not on the ground and unlatched, this means in air and no attachments so flying
        else if (!physics.player.IsGrounded()) 
        {
            // Not Grounded transition to FlyingState
            physics.ChangeState(physics.FlyingState);
        }
    }
}
