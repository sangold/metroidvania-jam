using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TPPrideState : IState
{
    private readonly PrideBattle _owner;
    private readonly List<PrideTPPoint> _tpPoints;
    private readonly PrideBoss _boss;
    private float _timer;
    private float _duration;

    public TPPrideState(PrideBattle owner, List<PrideTPPoint> tpPoints, PrideBoss boss, float duration)
    {
        _owner = owner;
        _tpPoints = tpPoints;
        _boss = boss;
        _duration = duration;
    }

    public bool IsDone()
    {
        return _timer >= _duration;
    }

    public void OnEnter()
    {
        _boss.SetColliderActive(false);
        _timer = 0f;
        PrideTPPoint selectedPoint = _owner.SelectedPoint;
        while(selectedPoint == _owner.SelectedPoint)
        {
            int random = Random.Range(0, _tpPoints.Count);
            selectedPoint = _tpPoints[random];
        }
        _owner.SelectedPoint = selectedPoint;
        _boss.transform.position = selectedPoint.transform.position;
        _boss.transform.rotation = selectedPoint.transform.rotation;
    }


    public void OnExit()
    {
        _timer = 0f;
    }

    public void Tick()
    {
        _timer += Time.deltaTime;
    }
}
