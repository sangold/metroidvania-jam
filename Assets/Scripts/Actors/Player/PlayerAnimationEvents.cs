using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private AnimationClip _standNoScythe;
    [SerializeField] private AnimationClip _standScythe;

    [SerializeField] private AnimationClip _runNoScythe;
    [SerializeField] private AnimationClip _runScythe;

    [SerializeField] private AnimationClip _jumpNoScythe;
    [SerializeField] private AnimationClip _jumpScythe;

    [SerializeField] private AnimationClip _fallingNoScythe;
    [SerializeField] private AnimationClip _fallingScythe;

    [SerializeField] private AnimationClip _landingNoScythe;
    [SerializeField] private AnimationClip _landingScythe;

    protected AnimatorOverrideController _animatorOverrideController;

    [SerializeField] private Animator _animator;
    [SerializeField] private Player _main;

    private void Start()
    {
        _main.OnJump += OnJump;
        _main.OnAttack += OnAttack;
        _main.OnChargeSpinAttack += OnChargeSpinAttack;
        _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = _animatorOverrideController;
    }
    
    private void OnJump(object sender, EventArgs e)
    {
        _animator.SetTrigger("Jump");
    }
    private void OnAttack(object sender, EventArgs e)
    {
        _animator.PlayInFixedTime("Attacking",-1,0);
    }
    private void OnChargeSpinAttack(object sender, EventArgs e)
    {
        _animator.PlayInFixedTime("ScytheSpinAttack",-1,0);
    }
    private void ReturnToIdle(){
        //this method is call in the ScytheSpinAttack animation.
        _main.SetState(PlayerState.STANDARD);
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("isGhostDashing", false);
    }

    private void FixedUpdate()
    {
        _animator.SetFloat("VerticalSpeed", _main.VerticalSpeed);
        _animator.SetBool("isGrounded", _main.PlayerCollision.OnGround);
        if (_main.CurrentState.StateType == PlayerState.GHOSTDASH){
        _animator.SetBool("isGhostDashing", true);
        } else {
        _animator.SetBool("isGhostDashing", false);
        }
        if (_main.CurrentState.StateType == PlayerState.ATTACK){
        _animator.SetBool("isAttacking", true);
        _animator.SetBool("Walking", false);
        } else if (_main.CurrentState.StateType == PlayerState.SPIN_ATTACK){
        _animator.SetBool("isAttacking", true);
        _animator.SetBool("Walking", false);
        } else {
        _animator.SetBool("isAttacking", false);
        }
        if (Mathf.Abs(_main.HorizontalSpeed) > .01){
            _animator.SetBool("Walking", true);
        } else {
            _animator.SetBool("Walking", false);
        }
        if (_main.CurrentState.StateType == PlayerState.DASH){
        _animator.SetBool("isDashing", true);
        _animator.SetBool("Walking", false);
        } else {
        _animator.SetBool("isDashing", false);
        }
        if (_main.CheckAbility(PowerUpType.SCYTHE)){
            _animatorOverrideController["StandNoScythe"] = _standScythe;
            _animatorOverrideController["RunNoScythe"] = _runScythe;
            _animatorOverrideController["JumpNoScythe"] = _jumpScythe;
            _animatorOverrideController["FallingNoScythe"] = _fallingScythe;
            _animatorOverrideController["LandingNoScythe"] = _landingScythe;
        } else {
            _animatorOverrideController["StandNoScythe"] = _standNoScythe;
            _animatorOverrideController["RunNoScythe"] = _runNoScythe;
            _animatorOverrideController["JumpNoScythe"] = _jumpNoScythe;
            _animatorOverrideController["FallingNoScythe"] = _fallingNoScythe;
            _animatorOverrideController["LandingNoScythe"] = _landingNoScythe;
        }
    }


}
