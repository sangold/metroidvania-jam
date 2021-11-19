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
    }
    
    private void OnJump(object sender, EventArgs e)
    {
        _animator.SetTrigger("Jump");
    }

    private void FixedUpdate()
    {
        _animator.SetFloat("VerticalSpeed", _main.VerticalSpeed);
        _animator.SetBool("isGrounded", _main.isGrounded);
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
    }


}
