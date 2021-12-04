public class StateMachine
{
   
    protected IState _currentState;

    public IState GetCurrentState()
    {
        return _currentState;
    }
    public virtual void Tick()
    {
        _currentState?.Tick();
    }

    public virtual void SetState(IState toState)
    {
        if (_currentState != null && toState.GetType() == _currentState.GetType())
            return;

        _currentState?.OnExit();
        _currentState = toState;

        _currentState?.OnEnter();
    }
}
