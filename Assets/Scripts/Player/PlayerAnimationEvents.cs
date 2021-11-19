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

    public void ReturnToStand()
    {
        _main.state = Player._finiteState.stand;
        _animator.PlayInFixedTime("Stand", -1, Time.fixedDeltaTime);
    }

        private void OnJump(object sender, EventArgs e)
    {
        _animator.SetTrigger("Jump");
    }

    private void FixedUpdate()
    {
        _animator.SetFloat("VerticalSpeed", _main.VerticalSpeed);
        _animator.SetBool("isGrounded", _main.isGrounded);
    }


}
