using UnityEngine;

public class PatrolAIState : IState
{
    private Enemy _owner;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector3[] _patrolPoints;

    private int _currentPoint = 0;
    private float _moveSpeed;
    private float _jumpForce;
    private bool _horizontalOnly;
    private CollisionDetector _lineOfSight;

    public PatrolAIState(Enemy owner, Vector3[] patrolPoints, float moveSpeed, float jumpForce, bool horizontalOnly, CollisionDetector lineOfSight)
    {
        _owner = owner;
        _rb = owner.Rb;
        _animator = owner.Animator;
        _patrolPoints = patrolPoints;
        _moveSpeed = moveSpeed;
        _jumpForce = jumpForce;
        _horizontalOnly = horizontalOnly;
        _lineOfSight = lineOfSight;
    }
    public void OnEnter()
    {
        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < _patrolPoints.Length; i++)
        {
            if (i == _currentPoint) continue;
            float distance = _patrolPoints[i].x - _owner.transform.position.x;
            if (distance > 0 && _owner.IsTurnToTheLeft()) continue;
            if (distance < 0 && !_owner.IsTurnToTheLeft()) continue;
            if(distance < closestDistance)
            {
                closestDistance = distance;
                _currentPoint = i;
            }
        }
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        Vector3 dir = (_patrolPoints[_currentPoint] - _rb.transform.position);
        if (Mathf.Abs(dir.magnitude) > .5f)
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

            if (_lineOfSight.OnGround)
                _animator.SetBool("isJumping", false);
            if ((dir.x > 0 && _lineOfSight.OnRightWall) || (dir.x < 0 && _lineOfSight.OnLeftWall))
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
                _animator.SetBool("isJumping", true);
            }
        }
        else
        {
            _currentPoint = (_currentPoint + 1) % _patrolPoints.Length;
        }
    }
}
