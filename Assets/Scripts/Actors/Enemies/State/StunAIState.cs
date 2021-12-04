using System.Collections;
using UnityEngine;

public class StunAIState : IState
{
    private Enemy _owner;
    private Rigidbody2D _rb;
    private Animator _animator;
    private float _stunDelay;

    public StunAIState(Enemy enemy, float stunDelay)
    {
        _owner = enemy;
        _rb = enemy.Rb;
        _animator = enemy.Animator;
        _stunDelay = stunDelay;
    }
    public void OnEnter()
    {
        _rb.velocity = Vector2.zero;
        _owner.CanMove = false;
        _owner.StartCoroutine(Stun());
    }

    public void OnExit()
    {
        _owner.CanMove = true;
    }

    public void Tick()
    {
    }

    private IEnumerator Stun()
    {
        yield return new WaitForSeconds(_stunDelay);
        _owner.ReturnToPreviousState();
    }
}