using System;
using UnityEngine;

public class AttackPrideState : IState
{
    private PrideBattle _owner;
    private PrideBoss _bossGo;
    private string _triggerName;
    private Animator _animator;
    private bool _isEnded;

    public AttackPrideState(PrideBattle owner, PrideBoss bossGo, string triggerName)
    {
        _owner = owner;
        _bossGo = bossGo;
        _triggerName = triggerName;
        _animator = _bossGo.GetComponentsInChildren<Animator>()[1];
    }

    public bool IsDone()
    {
        return _isEnded;
    }

    public void OnEnter()
    {
        _bossGo.SetColliderActive(true);
        _isEnded = false;
        _triggerName = _owner.SelectedPoint.MirrorType == Mirror.MirrorType.SIDE ? "SideAttack" : "SmashAttack";
        _animator.SetTrigger(_triggerName);
        _bossGo.AnimatorEventHandler.OnAttack += OnAttack;
        _bossGo.AnimatorEventHandler.OnAnimTrigger += OnAnimTrigger;
        _bossGo.AnimatorEventHandler.OnEnd += OnEnd;
    }

    private void OnEnd()
    {
        _isEnded = true;
    }

    private void OnAnimTrigger()
    {
        _owner.CloseCurrentMirror();
    }

    private void OnAttack()
    {
        _bossGo.Attack();
    }

    public void OnExit()
    {
        _bossGo.AnimatorEventHandler.OnAnimTrigger -= OnAnimTrigger;
        _bossGo.AnimatorEventHandler.OnAttack -= OnAttack;
        _bossGo.AnimatorEventHandler.OnEnd -= OnEnd;
    }

    public void Tick()
    {
    }
}

