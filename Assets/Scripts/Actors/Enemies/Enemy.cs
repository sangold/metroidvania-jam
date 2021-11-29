using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected Transform[] _patrolGizmos;
    [SerializeField] protected Animator _animator;
    protected Vector3[] _patrolPoints;
    protected Rigidbody2D _rb;

    protected float _moveSpeed, _jumpForce;
    protected AIStateMachine _stateMachine;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _patrolPoints = new Vector3[_patrolGizmos.Length];
        for (int i = 0; i < _patrolGizmos.Length; i++)
        {
            _patrolPoints[i] = _patrolGizmos[i].position;
            _patrolGizmos[i].gameObject.SetActive(false);
        }

        _stateMachine = new AIStateMachine();
    }

    protected virtual void FixedUpdate()
    {
        _stateMachine.Tick();
    }

    protected virtual void OnCollisionEnter2D(Collision2D target)
    {
        HealthComponent targetHealth = target.gameObject.GetComponent<HealthComponent>();
        if (target.gameObject.tag == "Player")
        {
            targetHealth.TakeDamage(1);
        }
    }
}
