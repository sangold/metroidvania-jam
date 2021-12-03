public class StateMachine
{
   
    protected IState _currentState;


    public virtual void Tick()
    {
        _currentState?.Tick();
    }

    public virtual void SetState(IState toState)
    {
        if (toState == _currentState)
            return;

        _currentState?.OnExit();
        _currentState = toState;

        _currentState?.OnEnter();
    }
}
