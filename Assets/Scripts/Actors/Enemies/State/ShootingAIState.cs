using System;
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
        if(_target.transform.position.x > _owner.transform.position.x)
        {
            _owner.TurnRight();
        }
        else
        {
            _owner.TurnLeft();
        }
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
            _animator.SetTrigger("Shoot");
            ShootBullet();
            _shootTimer -= _fireDelay;
        }
        _shootTimer += Time.fixedDeltaTime;
    }

    private void ShootBullet()
    {
        // Pooling might be a good Idea
        GameObject bullet = GameObject.Instantiate(_bulletGO);
        bullet.transform.position = _animator.transform.position;
        bullet.GetComponent<Bullet>().Init(4f, 1, _animator.transform.position, _target.position);
    }
}

