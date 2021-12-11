using UnityEngine;

public class ChaseAIState : IState
{
    private Rigidbody2D _rb;
    private bool _isDiving;
    private bool _isGrounded;
    private ActorDetector _actorDetector;
    private Transform _target;
    private Vector3 _lastScenePoint;
    private Animator _animator;
    private float _moveSpeed;
    public ChaseAIState(Rigidbody2D rb, Animator animator, ActorDetector actorDetector, bool isDiving, float moveSpeed, bool isGrounded)
    {
        _rb = rb;
        _isDiving = isDiving;
        _isGrounded = isGrounded;
        _actorDetector = actorDetector;
        _animator = animator;
        _moveSpeed = moveSpeed;
    }
    public void OnEnter()
    {
        _animator.SetBool("Chase", true);
        if(_isDiving)
            _animator.SetBool("Dive", true);
        _target = _actorDetector.Target;
        if(_target != null)
            _lastScenePoint = _target.position;
    }

    public void OnExit()
    {
        _animator.SetBool("Chase", false);
        if(_isDiving)
            _animator.SetBool("Dive", false);
        _target = null;

    }

    public bool HasReachedPosition()
    {
        return Vector3.Distance(_rb.transform.position, _lastScenePoint) < 1f;
    }

    public void Tick()
    {
        Vector3 dir = _lastScenePoint - _rb.transform.position;
        if (_isGrounded)
        {
            dir.y = 0;
            _rb.velocity = new Vector2(dir.normalized.x * _moveSpeed, _rb.velocity.y);
        }
        else
        {
            _rb.velocity = dir.normalized * _moveSpeed;
        }
        if (!_isDiving && _target != null)
            _lastScenePoint = _target.position;
    }
}
