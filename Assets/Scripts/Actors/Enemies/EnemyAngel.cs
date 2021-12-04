using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAngel : Enemy
{
    [SerializeField] private ActorDetector _actorDetector;

    protected override void Awake()
    {
        base.Awake();
        _moveSpeed = 2f;
        _jumpForce = 0f;

        PatrolAIState patrol = new PatrolAIState(Rb, Animator, _patrolPoints, _moveSpeed, _jumpForce, false, null);
        ChaseAIState chase = new ChaseAIState(Rb, Animator, _actorDetector, true, 5f, false);

        _stateMachine.AddAnyTransition(chase, () => _actorDetector.CanSee);
        _stateMachine.AddTransition(chase, patrol, () => chase.HasReachedPosition());
        _stateMachine.SetState(patrol);
    }
}
