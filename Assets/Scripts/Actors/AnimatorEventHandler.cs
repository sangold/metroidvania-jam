using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorEventHandler: MonoBehaviour
{
    public Action OnAttack;
    public Action OnEnd;
    public Action OnAnimTrigger;

    public void AnimTrigger()
    {
        OnAnimTrigger?.Invoke();
    }

    public void Attack()
    {
        OnAttack?.Invoke();
    }

    public void End()
    {
        OnEnd?.Invoke();
    }
}
