using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Animator Animator;
    public Rigidbody2D Rb;
    [SerializeField] protected Transform[] _patrolGizmos;
    [SerializeField] protected Transform _spriteTransform;
    protected Vector3[] _patrolPoints;

    protected float _moveSpeed, _jumpForce;
    protected AIStateMachine _stateMachine;

    public void TurnRight()
    {
        if(GetFaceDirection() < 0f)
            _spriteTransform.transform.localScale = new Vector2(1, 1);
    }
    public void TurnLeft()
    {
        if (GetFaceDirection() > 0f)
            _spriteTransform.transform.localScale = new Vector2(-1, 1);
    }
    public float GetFaceDirection()
    {
        return _spriteTransform.transform.localScale.x;
    }

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
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
        if (Rb.velocity.x < 0)
        {
            TurnLeft();
        }
        else if (Rb.velocity.x > 0)
        {
            TurnRight();
        }
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
