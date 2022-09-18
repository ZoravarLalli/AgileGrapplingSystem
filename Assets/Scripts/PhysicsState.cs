
// Blueprint for physics states
// derived classes will override methods and define physics constraints to be applied to the player's 
// rigidbody the specific constraints and desired effect varying with each different derived class
public abstract class PhysicsState
{
    public abstract void EnterState(PhysicsHandler physics);
    public abstract void UpdateState(PhysicsHandler physics);
    //public abstract void OnCollide(PhysicsHandler physics); // Might not need to define collisions behavior specific to each state.
}
