using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Humanoid
{
    [SerializeField]
    private float _jumpShortMultiplier;
    [HideInInspector]
    public enum _finiteState {stand,walk,attack,ghostDash,hurt,slide,walling,wallJumping,dead}
    [HideInInspector]
    public _finiteState state = _finiteState.stand;
    [SerializeField]
    private Animator _animator;


    //button presses/release/held
    private bool _slideButton = false;
    private bool _slideButtonSwitch = false;
    private bool _slideButtonPressed = false;

    private bool _attackButton = false;
    private bool _attackButtonSwitch = false;
    private bool _attackButtonPressed = false;

    private bool _ghostDashButton = false;
    private bool _ghostDashButtonSwitch = false;
    private bool _ghostDashButtonPressed = false;
    private Vector2 _lastNoneGhostPosition;
    private bool _snapTolastNoneGhostPosition = false;
    [SerializeField]
    private float _ghostDashSpeed = 25;
    
    [SerializeField]
    private GameObject _spriteGameObject;

    [SerializeField]
    private float _maxHorizontalSlideSpeed = 25;
    [SerializeField]
    private float _slideFriction = .005f;

    [SerializeField]
    private CapsuleCollider2D _capsuleCollider2D;

    public string _touchingARoom = null;
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
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
            if (_jumpButtonPressed && _lastGroundTime > 0){
            Jump();
            }
            else if (_jumpButtonPressed && _lastGroundTime <= 0 && canDoubleJump && hasDoubleJump)
            {
            DoubleJump();
            }
            if (_slideButtonPressed){
                Slide();
            }
            if (_attackButtonPressed){
                Attack();
            }
            if (_ghostDashButtonPressed){
                GhostDash();
            }
            if (state == _finiteState.stand){
                if (isGrounded){
                    _animator.PlayInFixedTime("Stand",-1,Time.fixedDeltaTime);
                } else {
                    _animator.PlayInFixedTime("Jump",-1,Time.fixedDeltaTime);
                }
            }
            if (state == _finiteState.walk){
                if (isGrounded){
                    _animator.PlayInFixedTime("Walk",-1,Time.fixedDeltaTime);
                } else {
                    _animator.PlayInFixedTime("Jump",-1,Time.fixedDeltaTime);
                }
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
            _animator.PlayInFixedTime("Walling",-1,Time.fixedDeltaTime);
            if (_jumpButtonPressed && _lastGroundTime <= 0 && canWallJump){
                WallJump();
                state = _finiteState.wallJumping;
            }
            if (_isAgainstLeftWall){
                TurnRight();
            } else if (_isAgainstRightWall){
                TurnLeft();
            } else {
                state = _finiteState.stand;
            }
            if (isGrounded){
                state = _finiteState.stand;
            }
            if (_slideButtonPressed){
                Slide();
            }
            if (_ghostDashButtonPressed){
                GhostDash();
            }
        }
        if (state == _finiteState.wallJumping){
            _animator.PlayInFixedTime("Jump",-1,Time.fixedDeltaTime);
            _friction = 1;
            if (_canMove == true){
                state = _finiteState.stand;
            }
        }
        if (state == _finiteState.attack){
            if (!isGrounded){
                _rb.velocity += new Vector2(_movementX * _airHorizontalSpeed * Time.fixedDeltaTime,0);
                _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x,-_maxHorizontalSpeed,_maxHorizontalSpeed),_rb.velocity.y);
            }
            if (_slideButtonPressed){
                Slide();
            }
        }
        base.FixedUpdate();
        if (state == _finiteState.ghostDash){
            _canMove = false;
            _rb.velocity = new Vector2(_movementX * _ghostDashSpeed,_movementY * _ghostDashSpeed);
            _rb.gravityScale = 0;
            _capsuleCollider2D.enabled = false;

        } else {
            if (_snapTolastNoneGhostPosition && isGrounded && _isAgainstLeftWall && _isAgainstRightWall || _snapTolastNoneGhostPosition && _touchingARoom == null){
                transform.position = _lastNoneGhostPosition;
            }
            _snapTolastNoneGhostPosition = false;
            _capsuleCollider2D.enabled = true;
        }
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
        if (state == _finiteState.ghostDash){
            _movementY = Input.GetAxis("Vertical");
        } else {
            _movementY = 0;
        }
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

        if (_attackButton != Input.GetButton("Attack")){
            _attackButtonSwitch = true;
        } else {
            _attackButtonSwitch = false;
        }
        _attackButton = Input.GetButton("Attack");
        _attackButtonPressed = _attackButton && _attackButtonSwitch;

        if (_ghostDashButton != Input.GetButton("GhostDash")){
            _ghostDashButtonSwitch = true;
        } else {
            _ghostDashButtonSwitch = false;
        }
        _ghostDashButton = Input.GetButton("GhostDash");
        _ghostDashButtonPressed = _ghostDashButton && _ghostDashButtonSwitch;


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
        _animator.PlayInFixedTime("Sliding",-1,Time.fixedDeltaTime);
        _rb.velocity = new Vector2(100 * GetFaceDirection(),_rb.velocity.y);
        _friction = _slideFriction;
        state = _finiteState.slide;
        _canMove = false;
    }
    private void Attack(){
        _animator.PlayInFixedTime("Attacking",-1,Time.fixedDeltaTime);
        state = _finiteState.attack;
        _canMove = false;
    }
    private void GhostDash(){
        _rb.velocity = new Vector2(0,0);
        _lastNoneGhostPosition = new Vector2(transform.position.x,transform.position.y);
        _animator.PlayInFixedTime("GhostDash",-1,Time.fixedDeltaTime);
        state = _finiteState.ghostDash;
        _canMove = false;
        _snapTolastNoneGhostPosition = true;
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
