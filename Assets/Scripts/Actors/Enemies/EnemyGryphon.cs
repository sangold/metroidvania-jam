﻿using UnityEngine;

public class EnemyGryphon: Enemy
{
    [SerializeField] private ActorDetector _actorDetector;
    [SerializeField] private GameObject _bulletGO;

    protected override void Awake()
    {
        base.Awake();
        _moveSpeed = 2f;
        _jumpForce = 0f;

        PatrolAIState patrol = new PatrolAIState(Rb, Animator, _patrolPoints, _moveSpeed, _jumpForce, false, null);
        ShootingAIState shoot = new ShootingAIState(this, 2f, _bulletGO);

        _stateMachine.AddAnyTransition(shoot, () => _actorDetector.CanSee);
        _stateMachine.AddTransition(shoot, patrol, () => !_actorDetector.CanSee);
        _stateMachine.SetState(patrol);
    }
}

