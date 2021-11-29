using UnityEngine;

public class PatrolAIState : IState
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector3[] _patrolPoints;

    private int _currentPoint = 0;
    private float _moveSpeed;
    private float _jumpForce;
    private bool _horizontalOnly;
    private CollisionDetector _lineOfSight;

    public PatrolAIState(Rigidbody2D rb, Animator animator, Vector3[] patrolPoints, float moveSpeed, float jumpForce, bool horizontalOnly, CollisionDetector lineOfSight)
    {
        _rb = rb;
        _animator = animator;
        _patrolPoints = patrolPoints;
        _moveSpeed = moveSpeed;
        _jumpForce = jumpForce;
        _horizontalOnly = horizontalOnly;
        _lineOfSight = lineOfSight;
    }
    public void OnEnter()
    {
        //_animator.SetBool("Patrol", true);
    }

    public void OnExit()
    {
        //_animator.SetBool("Patrol", false);
    }

    public void Tick()
    {
        Vector3 dir = (_patrolPoints[_currentPoint] - _rb.transform.position);
        if (Mathf.Abs(dir.magnitude) > .25f)
        {
            if (!_horizontalOnly)
            {
                _rb.velocity = dir.normalized * _moveSpeed;
            }
            else
            {
                _rb.velocity = new Vector2(Mathf.Sign(dir.x) * _moveSpeed, _rb.velocity.y);
            }
            
            if (_lineOfSight == null) return;

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
}
