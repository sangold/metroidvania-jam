using UnityEngine;

public class EnemyKnight: Enemy
{
    public ActorDetector ActorDetector;
    public MeleeAttackComponent MeleeAttackComponent;
    private StunAIState _stunState;

    protected override void Awake()
    {
        base.Awake();
        _moveSpeed = 2f;
        _jumpForce = 3.5f;

        PatrolAIState patrol = new PatrolAIState(this, _patrolPoints, _moveSpeed, _jumpForce, true, null);
        ChaseAIState chase = new ChaseAIState(Rb, Animator, ActorDetector, false, _moveSpeed, true);
        MeleeAIState attack = new MeleeAIState(this, 2f);
        _stunState = new StunAIState(this, .25f);

        _stateMachine.AddAnyTransition(attack, () => ActorDetector.IsInMeleeRange(1.35f) && CanMove);
        _stateMachine.AddAnyTransition(chase, () => ActorDetector.CanSee && CanMove);
        _stateMachine.AddAnyTransition(patrol, () => !ActorDetector.CanSee && CanMove);
        _stateMachine.AddTransition(attack, chase, () => !ActorDetector.IsInMeleeRange(1.35f) && CanMove);
        _stateMachine.SetState(patrol);
    }

    protected override void OnDamageTaken(int hp, Vector3 attackOrigin)
    {
        _previousState = _stateMachine.GetCurrentState();
        TurnTo(attackOrigin);
        _stateMachine.SetState(_stunState);
    }
}

