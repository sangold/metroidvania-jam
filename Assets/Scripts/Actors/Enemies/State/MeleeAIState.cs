using System;
using System.Collections;
using UnityEngine;

public class MeleeAIState: IState
{
    public bool IsAttacking;
    private Enemy _owner;
    private Animator _animator;
    private ActorDetector _actorDetector;
    private MeleeAttackComponent _meleeAttackComponent;
    private Rigidbody2D _rb;
    private Transform _target;
    private float _fireDelay;
    private float _shootTimer;
    private float _attackFrame = 11f;
    private float _sampleRate = 30f;
    public MeleeAIState(Enemy enemy, float fireDelay)
    {
        _owner = enemy;
        _rb = enemy.Rb;
        _animator = enemy.Animator;
        _meleeAttackComponent = _owner.GetComponent<MeleeAttackComponent>();
        _actorDetector = enemy.GetComponentInChildren<ActorDetector>();
        _fireDelay = fireDelay;
        _target = _actorDetector.Target;
    }

    public void OnEnter()
    {
        _target = _actorDetector.Target;
        if(_target.transform.position.x > _owner.transform.position.x)
        {
            _owner.TurnRight();
        }
        else
        {
            _owner.TurnLeft();
        }

        if(Time.time - _owner.lastAttackTimer >= _fireDelay)
            _shootTimer = _fireDelay;
        _rb.velocity = Vector2.zero;
    }

    public void OnExit()
    {
        _target = null;
    }

    public void Tick() 
    { 
        if(_shootTimer >= _fireDelay)
        {
            _owner.lastAttackTimer = Time.time;
            IsAttacking = true;
            GameManager.Instance.StartCoroutine(Attack());
            _shootTimer -= _fireDelay;
        }
        _shootTimer += Time.fixedDeltaTime;
    }

    private IEnumerator Attack()
    {
        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(_attackFrame / _sampleRate);
        _meleeAttackComponent.Attack();
        IsAttacking = false;
    }
}

