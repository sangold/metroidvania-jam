using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    [SerializeField] private Player _main;

    private void Start()
    {
        _main.OnJump += OnJump;
        _main.OnAttack += OnAttack;
    }
    
    private void OnJump(object sender, EventArgs e)
    {
        _animator.SetTrigger("Jump");
    }
    private void OnAttack(object sender, EventArgs e)
    {
        _animator.PlayInFixedTime("Attacking",-1,0);
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
        } else {
        _animator.SetBool("isAttacking", false);
        }
        if (Mathf.Abs(_main._playerInputs.MovementX) > .01){
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
    }


}
