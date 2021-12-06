using System;
using System.Collections.Generic;
using UnityEngine;

public class PrideBattle: MonoBehaviour
{
    [SerializeField]
    private List<PrideTPPoint> _tpPoints;
    private AIStateMachine _stateMachine;
    [SerializeField]
    private PrideBoss _boss;
    [SerializeField]
    private float _delayBetweenAttacks;
    private float _timer = 0f;
    private AttackPrideState _attackState;

    [HideInInspector]
    public PrideTPPoint SelectedPoint;

    private void Awake()
    {
        _stateMachine = new AIStateMachine();
    }


    private void Start()
    {
        var introState = new IntroPrideState(this, _boss);
        var tpState = new TPPrideState(this, _tpPoints, _boss, _delayBetweenAttacks);
        _attackState = new AttackPrideState(this, _boss, "SideAttack");

        _stateMachine.AddTransition(introState, tpState, () => introState.IsDone());
        _stateMachine.AddTransition(tpState, _attackState, () => tpState.IsDone());
        _stateMachine.AddTransition(_attackState, tpState, () => _attackState.IsDone());
        _stateMachine.SetState(introState);

        _boss.HealthComponent.OnDamageTaken += OnDamageTaken;
    }

    private void OnDestroy()
    {
        _boss.HealthComponent.OnDamageTaken -= OnDamageTaken;
    }

    private void OnDamageTaken(int hp, Vector3 attackOrigin)
    {
        
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    public void SetSelectedPoint(PrideTPPoint tp)
    {
        SelectedPoint = tp;
        _boss.SetToType(tp.MirrorType);
    }
}