using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : Enemy
{
    private CollisionDetector _lineOfSight;

    protected override void Awake()
    {
        base.Awake();
        _moveSpeed = 2f;
        _jumpForce = 3.5f;
        _lineOfSight = GetComponent<CollisionDetector>();
        PatrolAIState patrol = new PatrolAIState(Rb, Animator, _patrolPoints, _moveSpeed, _jumpForce, true, _lineOfSight);
        _stateMachine.SetState(patrol);

    }
}
