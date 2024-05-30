public interface IState
{
    public void Enter()
    {
        // runs when first entering state
    }
    public void Update()
    {
        // per-frame logic, include condition to transition to a new state
    }
    public void Exit()
    {
        // code that runs when we exit state
    }
}
