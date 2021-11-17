using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Humanoid
{
    [SerializeField]
    private float _jumpShortMultiplier;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }
    public override void FixedUpdate(){
        Inputs();
        ShortHop();
        if (_jumpButtonPressed && _lastGroundTime > 0)
        {
            Jump();
        }
        else if (_jumpButtonPressed && _lastGroundTime <= 0 && canWallJump)
        {
            WallJump();
        }
        else if (_jumpButtonPressed && _lastGroundTime <= 0 && canDoubleJump && hasDoubleJump)
        {
            DoubleJump();
        }
        base.FixedUpdate();
    }
    private void Inputs(){
        //reads all the player inputs here
        /*
        Unity Input only updates in update not fixed update. 
        So we have to make our own button pressed so the button press can work.

        using a combination of _jumpButton && _jumpButtonPressed
        */
        _movementX = Input.GetAxis("Horizontal");

        // for jump pressed to work in one frame.
        if (_jumpButton != Input.GetButton("Jump")){
            _jumpButtonSwitch = true;
            _jumpButton = Input.GetButton("Jump");
        } else {
            _jumpButtonSwitch = false;
            _jumpButton = Input.GetButton("Jump");
        }
        _jumpButtonPressed = _jumpButton && _jumpButtonSwitch;


    }
    private void ShortHop(){
        if (_jumpButton == false && !isGrounded)
        {
            if (_rb.velocity.y > 0 && _lastJumpTime > -.5f)
            {
                _rb.AddForce(Vector2.down * _rb.velocity.y * (1 - _jumpShortMultiplier), ForceMode2D.Impulse);
            }
        }
    }
    // Update is called once per frame
    public override void Update()
    {
        
        base.Update();
    }
}
