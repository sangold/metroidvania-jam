using UnityEngine;

public class IntroPrideState: IState
{
    private PrideBattle _owner;
    private PrideBoss _bossGo;
    private bool _isEnded;
    private Animator _animator;

    public IntroPrideState(PrideBattle owner, PrideBoss bossGo)
    {
        _owner = owner;
        _bossGo = bossGo;
        _animator = _bossGo.GetComponentInChildren<Animator>();
    }

    public bool IsDone()
    {
        return _isEnded;
    }

    public void OnEnter()
    {
        _bossGo.SetColliderActive(false);
        _isEnded = false;
        _animator.SetTrigger("Intro");
        _bossGo.AnimatorEventHandler.OnEnd += OnEnd;
    }

    private void OnEnd()
    {
        _isEnded = true;
    }

    public void OnExit()
    {
        _bossGo.AnimatorEventHandler.OnEnd -= OnEnd;
        _isEnded = false;
    }

    public void Tick()
    {
    }
}

