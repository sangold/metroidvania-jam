using System;
using System.Collections.Generic;
using UnityEngine;

public class PrideBattle: MonoBehaviour
{
    [SerializeField]
    private List<PrideTPPoint> _tpPoints;
    [SerializeField]
    private List<Mirror> _mirrors;
    private AIStateMachine _stateMachine;
    [SerializeField]
    private PrideBoss _boss;
    [SerializeField]
    private float _delayBetweenAttacks;
    [SerializeField]
    private float _portalOpeningTimer;
    //private float _timer = 0f;
    private AttackPrideState _attackState;
    private TPPrideState _tpState;

    [HideInInspector]
    public PrideTPPoint SelectedPoint;

    private void Awake()
    {
        _stateMachine = new AIStateMachine();
    }


    private void Start()
    {
        var introState = new IntroPrideState(this, _boss);
        _tpState = new TPPrideState(this, _tpPoints, _boss, _delayBetweenAttacks, _portalOpeningTimer);
        _attackState = new AttackPrideState(this, _boss, "SideAttack");

        _stateMachine.AddTransition(introState, _tpState, () => introState.IsDone());
        _stateMachine.AddTransition(_tpState, _attackState, () => _tpState.IsDone());
        _stateMachine.AddTransition(_attackState, _tpState, () => _attackState.IsDone());

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

    public void StartFight()
    {
        _stateMachine.SetState(_tpState);
    }

    public void CloseCurrentMirror()
    {
        if (SelectedPoint != null)
            _mirrors[_tpPoints.IndexOf(SelectedPoint)].Close();
    }

    public void OpenCurrentMirror()
    {
        if (SelectedPoint != null)
            _mirrors[_tpPoints.IndexOf(SelectedPoint)].Open();
    }

    public void SetSelectedPoint(PrideTPPoint tp)
    {
        SelectedPoint = tp;
    }
}