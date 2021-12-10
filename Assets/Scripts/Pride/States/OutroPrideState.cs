using UnityEngine;

public class OutroPrideState: IState
{
    private PrideBattle _owner;
    private PrideBoss _bossGo;
    private Transform _targetPos;
    private Animator _animator;

    public OutroPrideState(PrideBattle owner, PrideBoss bossGo, Transform tpPos)
    {
        _owner = owner;
        _bossGo = bossGo;
        _targetPos = tpPos;
        _animator = _bossGo.GetComponentsInChildren<Animator>()[1];

    }

    public bool IsDone()
    {
        return false;
    }

    public void OnEnter()
    {
        _bossGo.gameObject.SetActive(false);
        _owner.CloseAllMirrors();
        _animator.SetTrigger("Outro");
        _bossGo.SetColliderActive(false);
        _bossGo.transform.position = _targetPos.position;
        _bossGo.transform.rotation = _targetPos.rotation;
        _bossGo.gameObject.SetActive(true);
    }

    private void OnEnd()
    {
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}

