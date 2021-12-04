using UnityEngine;

public class EnemyKnight: Enemy
{
    public ActorDetector ActorDetector;
    public MeleeAttackComponent MeleeAttackComponent;

    protected override void Awake()
    {
        base.Awake();
        _moveSpeed = 2f;
        _jumpForce = 3.5f;

        PatrolAIState patrol = new PatrolAIState(Rb, Animator, _patrolPoints, _moveSpeed, _jumpForce, true, null);
        ChaseAIState chase = new ChaseAIState(Rb, Animator, ActorDetector, false, _moveSpeed, true);
        MeleeAIState attack = new MeleeAIState(this, 2f);

        _stateMachine.AddAnyTransition(attack, () => ActorDetector.IsInMeleeRange(1.35f));
        _stateMachine.AddAnyTransition(chase, () => ActorDetector.CanSee && !IsAttacking);
        _stateMachine.AddAnyTransition(patrol, () => !ActorDetector.CanSee && !IsAttacking);
        _stateMachine.AddTransition(attack, chase, () => !ActorDetector.IsInMeleeRange(1.35f));
        _stateMachine.SetState(patrol);
    }
}

