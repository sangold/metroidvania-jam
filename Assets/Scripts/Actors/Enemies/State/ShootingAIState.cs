using System;
using System.Collections;
using UnityEngine;

public class ShootingAIState: IState
{
    private Enemy _owner;
    private Animator _animator;
    private ActorDetector _actorDetector;
    private Rigidbody2D _rb;
    private Transform _target;
    private float _fireDelay;
    private float _shootTimer;
    private GameObject _bulletGO;
    private float _attackFrame = 5f;
    private float _sampleRate = 12f;
    private float _animationTotalFrame = 10f;

    public ShootingAIState(Enemy enemy, float fireDelay, GameObject go)
    {
        _owner = enemy;
        _rb = enemy.Rb;
        _animator = enemy.Animator;
        _actorDetector = enemy.GetComponentInChildren<ActorDetector>();
        _fireDelay = fireDelay;
        _target = _actorDetector.Target;
        _bulletGO = go;
    }

    public void OnEnter()
    {
        _target = _actorDetector.Target;
        _owner.TurnTo(_target.position);
        if (Time.time - _owner.lastAttackTimer >= _fireDelay)
            _shootTimer = _fireDelay;
        _rb.velocity = Vector2.zero;
    }

    public void OnExit()
    {
        _target = null;
    }

    public bool LoseTarget()
    {
        return !_actorDetector.CanSee;
    }

    public void Tick() 
    { 
        if(_shootTimer >= _fireDelay)
        {
            _owner.CanMove = false;
            _owner.TurnTo(_target.position);
            _owner.lastAttackTimer = Time.time;
            _owner.AttackCoroutine = _owner.StartCoroutine(Attack());
            _shootTimer -= _fireDelay;
        }
        _shootTimer += Time.fixedDeltaTime;
    }

    private IEnumerator Attack()
    {
        _animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(_attackFrame / _sampleRate);
        ShootBullet();
        yield return new WaitForSeconds((_animationTotalFrame - _attackFrame) / _sampleRate);
        _owner.CanMove = true;
    }

    private void ShootBullet()
    {
        // Pooling might be a good Idea
        GameObject bullet = GameObject.Instantiate(_bulletGO);
        bullet.transform.position = _animator.transform.position;
        bullet.GetComponent<Bullet>().Init(4f, 1, _animator.transform.position, _target.position);
    }
}

