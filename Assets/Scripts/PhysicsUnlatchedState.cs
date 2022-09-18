using UnityEngine;

public class PhysicsUnlatchedState : PhysicsState
{
    
    public override void EnterState(PhysicsHandler physics)
    {
        Debug.Log("Entering UNLATCHED STATE");
    }

    public override void UpdateState(PhysicsHandler physics)
    {
        Debug.Log("Testing state update");
        // Check to see if we have shot either of the grapples and latched
        if (physics.player.CheckLeftGrapple() || physics.player.CheckRightGrapple())
        {
            // Grapple shot transition
            // Since at least one of the grapples was shot and is latched, switch to latched state
            physics.ChangeState(physics.LatchedState);
        }
    }
}
