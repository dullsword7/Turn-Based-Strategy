public interface IState
{
    // runs when first entering state
    public void Enter();

    // per-frame logic, include condition to transition to a new state
    public void Update();

    // code that runs when we exit state
    public void Exit();
}
