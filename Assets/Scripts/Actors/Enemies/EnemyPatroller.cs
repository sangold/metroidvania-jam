using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    [SerializeField] private Transform[] _patrolGizmos;
    private CollisionDetector _lineOfSight;
    private Vector3[] _patrolPoints;
    private Rigidbody2D _rb;
    private int _currentPoint = 0;

    private float _moveSpeed, _jumpForce;

    private void Awake()
    {
        _moveSpeed = 2f;
        _jumpForce = 3.5f;
        _rb = GetComponent<Rigidbody2D>();
        _lineOfSight = GetComponent<CollisionDetector>();
        _patrolPoints = new Vector3[_patrolGizmos.Length];
        for (int i = 0; i < _patrolGizmos.Length; i++)
        {
            _patrolPoints[i] = _patrolGizmos[i].position;
            _patrolGizmos[i].gameObject.SetActive(false);
        }
        
    }

    private void FixedUpdate()
    {
        Vector3 dir = _patrolPoints[_currentPoint] - transform.position;
        if (Mathf.Abs(dir.x) > .2f)
        {
            _rb.velocity = new Vector2(Mathf.Sign(dir.x) * _moveSpeed, _rb.velocity.y);
            if ((dir.x > 0 && _lineOfSight.OnRightWall) || (dir.x < 0 && _lineOfSight.OnLeftWall))
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            }
        }
        else
        {
            _currentPoint = (_currentPoint + 1) % _patrolPoints.Length;
        } 

    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        HealthComponent targetHealth = target.gameObject.GetComponent<HealthComponent>();
        if(target.gameObject.tag == "Player")
        {
            targetHealth.TakeDamage(1);
        }
    }
}
