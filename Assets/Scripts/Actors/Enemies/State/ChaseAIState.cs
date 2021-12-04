using UnityEngine;

public class ChaseAIState : IState
{
    private Rigidbody2D _rb;
    private bool _isDiving;
    private ActorDetector _actorDetector;
    private Transform _target;
    private Vector3 _lastScenePoint;
    private Animator _animator;
    private float _moveSpeed;
    public ChaseAIState(Rigidbody2D rb, Animator animator, ActorDetector actorDetector, bool isDiving, float moveSpeed)
    {
        _rb = rb;
        _isDiving = isDiving;
        _actorDetector = actorDetector;
        _animator = animator;
        _moveSpeed = moveSpeed;
    }
    public void OnEnter()
    {
        _animator.SetBool("Chase", true);
        _animator.SetBool("Dive", _isDiving);
        _target = _actorDetector.Target;
        _lastScenePoint = _target.position;
    }

    public void OnExit()
    {
        _animator.SetBool("Chase", false);
        _animator.SetBool("Dive", false);
        _target = null;

    }

    public bool HasReachedPosition()
    {
        Debug.Log(Vector3.Distance(_rb.transform.position, _lastScenePoint));
        return Vector3.Distance(_rb.transform.position, _lastScenePoint) < 1f;
    }

    public void Tick()
    {
        Vector3 dir = _lastScenePoint - _rb.transform.position;
        _rb.velocity = dir.normalized * _moveSpeed;
        if (!_isDiving)
            _lastScenePoint = _target.position;
    }
}
