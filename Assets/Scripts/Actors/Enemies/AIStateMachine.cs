using System;
using System.Collections.Generic;

public class AIStateMachine : StateMachine
{
    public class Transition
    {
        public Func<bool> Condition { get; }
        public IState To { get; }
        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
    private List<Transition> _currentTransitions = new List<Transition>();
    private List<Transition> _anyTransitions = new List<Transition>();
    private static List<Transition> EmptyTransitions = new List<Transition>(0);

    public override void Tick()
    {
        Transition transition = GetTransition();
        if (transition != null)
            SetState(transition.To);

        base.Tick();
    }

    public override void SetState(IState toState)
    {

        base.SetState(toState);
        _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
        if (_currentTransitions == null)
            _currentTransitions = EmptyTransitions;
    }

    private Transition GetTransition()
    {
        
        foreach (Transition transition in _currentTransitions)
        {
            if (transition.Condition())
                return transition;
        }

        foreach (Transition transition in _anyTransitions)
        {
            if (transition.Condition())
                return transition;
        }

        return null;
    }

    public void AddTransition(IState from, IState to, Func<bool> condition)
    {
        if(_transitions.TryGetValue(from.GetType(), out List<Transition> transitions) == false)
        {
            transitions = new List<Transition>();
            _transitions[from.GetType()] = transitions;
        }

        transitions.Add(new Transition(to, condition));
    }

    public void AddAnyTransition(IState state, Func<bool> condition)
    {
        _anyTransitions.Add(new Transition(state, condition));
    }
}
