using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TPPrideState : IState
{
    private readonly PrideBattle _owner;
    private readonly List<PrideTPPoint> _tpPoints;
    private readonly PrideBoss _boss;
    private float _timer;
    private float _delayBetweenAttacks;
    private float _totalDuration;
    private bool _hasOpenPortal;

    public TPPrideState(PrideBattle owner, List<PrideTPPoint> tpPoints, PrideBoss boss, float delayBetweenAttacks, float animationDuration)
    {
        _owner = owner;
        _tpPoints = tpPoints;
        _boss = boss;
        _delayBetweenAttacks = delayBetweenAttacks;
        _totalDuration = delayBetweenAttacks + animationDuration;
    }

    public bool IsDone()
    {
        return _timer >= _totalDuration;
    }

    public void OnEnter()
    {
        _hasOpenPortal = false;
        _boss.SetColliderActive(false);
        _timer = 0f;
        PrideTPPoint selectedPoint = _owner.SelectedPoint;
        while (selectedPoint == _owner.SelectedPoint)
        {
            int random = Random.Range(0, _tpPoints.Count);
            selectedPoint = _tpPoints[random];
        }
        _owner.SetSelectedPoint(selectedPoint);
        _boss.transform.position = selectedPoint.transform.position;
        _boss.transform.rotation = selectedPoint.transform.rotation;
    }


    public void OnExit()
    {
        _hasOpenPortal = true;
        _timer = 0f;
    }

    public void Tick()
    {
        if(_timer >= _delayBetweenAttacks && !_hasOpenPortal)
        {
            _owner.OpenCurrentMirror();
            _hasOpenPortal = true;
        }
        _timer += Time.deltaTime;
    }
}
