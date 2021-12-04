using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Animator Animator;
    public Rigidbody2D Rb;
    [HideInInspector]
    public float lastAttackTimer;
    [HideInInspector]
    public Coroutine AttackCoroutine;
    [HideInInspector]
    public bool IsAttacking;

    [SerializeField] protected Transform[] _patrolGizmos;
    protected Vector3[] _patrolPoints;
    

    protected float _moveSpeed, _jumpForce;
    protected AIStateMachine _stateMachine;

    private void OnDestroy()
    {
        if (AttackCoroutine != null)
            StopCoroutine(AttackCoroutine);
    }

    public void TurnRight()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }
    public void TurnLeft()
    {
        transform.eulerAngles = new Vector3(180, 0, 180);
    }
    public bool IsTurnToTheLeft()
    {
        return transform.eulerAngles.x == 180;
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
        if (Rb.velocity.x < 0 && !IsAttacking)
        {
            TurnLeft();
        }
        else if (Rb.velocity.x > 0 && !IsAttacking)
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
            targetHealth.TakeDamage(1, transform.position);
        }
    }
}
