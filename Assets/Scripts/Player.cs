using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Humanoid
{
    [SerializeField]
    private float _jumpShortMultiplier;

    private enum _finiteState {stand,walk,attack,hurt,slide,walling,wallJumping,dead}
    private _finiteState state = _finiteState.stand;


    //button presses/release/held
    private bool _slideButton = false;
    private bool _slideButtonSwitch = false;
    private bool _slideButtonPressed = false;
    [SerializeField]
    private GameObject _spriteGameObject;

    [SerializeField]
    private float _maxHorizontalSlideSpeed = 25;
    [SerializeField]
    private float _slideFriction = .005f;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }
    public override void FixedUpdate(){
        Inputs();
        if (state == _finiteState.stand || state == _finiteState.walk){
            _canMove = true;
            _friction = 0.000001f;
            if (_movementX > 0){
                state = _finiteState.walk;
                TurnRight();
            } else if (_movementX < 0){
                state = _finiteState.walk;
                TurnLeft();
            } else {
                state = _finiteState.stand;
            }
            if (canWallJump){
                state = _finiteState.walling;
            }
        ShortHop();
        if (_jumpButtonPressed && _lastGroundTime > 0)
        {
            Jump();
        }
        else if (_jumpButtonPressed && _lastGroundTime <= 0 && canDoubleJump && hasDoubleJump)
        {
            DoubleJump();
        }
        if (_slideButtonPressed){
            Slide();
        }
        }
        if (state == _finiteState.slide){
            _movementX = 0;
            _canMove = false;
            _friction = _slideFriction;
            _rb.velocity = new Vector2(
                Mathf.Clamp(_rb.velocity.x,-_maxHorizontalSlideSpeed,_maxHorizontalSlideSpeed),
                _rb.velocity.y);
            if (Mathf.Abs(_rb.velocity.x) < 10){
                state = _finiteState.stand;
                _canMove = true;
            }
        }
        if (state == _finiteState.walling){
            if (_jumpButtonPressed && _lastGroundTime <= 0 && canWallJump){
                WallJump();
                state = _finiteState.wallJumping;
            }
            if (_isAgainstLeftWall){
                TurnRight();
            }
            if (_isAgainstRightWall){
                TurnLeft();
            }
            if (isGrounded){
                state = _finiteState.stand;
            }
            if (_slideButtonPressed){
                Slide();
            }
        }
        if (state == _finiteState.wallJumping){
            _friction = 1;
            if (_canMove == true){
                state = _finiteState.stand;
            }
        }
        base.FixedUpdate();
    }
    private void TurnRight(){
        _spriteGameObject.transform.localScale = new Vector2(1,1);
    }
    private void TurnLeft(){
        _spriteGameObject.transform.localScale = new Vector2(-1,1);
    }
    private float GetFaceDirection(){
        return _spriteGameObject.transform.localScale.x;
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
        } else {
            _jumpButtonSwitch = false;
        }
        _jumpButton = Input.GetButton("Jump");
        _jumpButtonPressed = _jumpButton && _jumpButtonSwitch;

        if (_slideButton != Input.GetButton("Slide")){
            _slideButtonSwitch = true;
        } else {
            _slideButtonSwitch = false;
        }
        _slideButton = Input.GetButton("Slide");
        _slideButtonPressed = _slideButton && _slideButtonSwitch;


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
    private void Slide(){
        _rb.velocity = new Vector2(100 * GetFaceDirection(),_rb.velocity.y);
        _friction = _slideFriction;
        state = _finiteState.slide;
        _canMove = false;
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
