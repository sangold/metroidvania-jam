using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

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
    [SerializeField]
    private CutScene _introDirector;
    [SerializeField]
    private CutScene _outroDirector;
    //private float _timer = 0f;
    private AttackPrideState _attackState;
    private TPPrideState _tpState;
    private bool _fightHasEnded;

    [SerializeField]
    private GameEvent _prideBossEnd;

    [HideInInspector]
    public PrideTPPoint SelectedPoint;

    private BoxCollider2D _bossTrigger;

    private void Awake()
    {
        _stateMachine = new AIStateMachine();
        _bossTrigger = GetComponent<BoxCollider2D>();
    }


    private void Start()
    {
        var outroState = new OutroPrideState(this, _boss, _tpPoints[2].transform);
        _tpState = new TPPrideState(this, _tpPoints, _boss, _delayBetweenAttacks, _portalOpeningTimer);
        _attackState = new AttackPrideState(this, _boss, "SideAttack");

        _stateMachine.AddTransition(_tpState, _attackState, () => _tpState.IsDone());
        _stateMachine.AddTransition(_attackState, _tpState, () => _attackState.IsDone());
        _stateMachine.AddAnyTransition(outroState, () => _fightHasEnded);

        _boss.HealthComponent.OnDamageTaken += OnDamageTaken;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null) return;

        _introDirector.StartCutscene();
        GameManager.Instance.GoToCutSceneState();
        _bossTrigger.enabled = false;
    }

    private void OnDestroy()
    {
        _boss.HealthComponent.OnDamageTaken -= OnDamageTaken;
    }

    private void OnDamageTaken(int hp, Vector3 attackOrigin)
    {
        if (hp <= 0)
        {
            _fightHasEnded = true;
            _outroDirector.StartCutscene();
            GameManager.Instance.AddBossKill("Pride");
            GameManager.Instance.GoToCutSceneState();
            _prideBossEnd.Raise();
        }
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    public void StartFight()
    {
        GameManager.Instance.GoToGameLoopState();
        _stateMachine.SetState(_tpState);
    }

    public void EndOutro()
    {
        GameManager.Instance.GoToGameLoopState();
    }

    public void CloseAllMirrors()
    {
        foreach (Mirror mirror in _mirrors)
            mirror.ForceClose();
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