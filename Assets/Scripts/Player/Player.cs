using MV.Player;
using System;
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

    private PlayerInputs _playerInputs;
    
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

    [SerializeField]
    private float maxHealth = 100;
    private float health = 0;
    public float Health {
        get {
            return health;
        }
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
        }
    }
    private float _stunDuration = 0;

    public string _touchingARoom = null;
    public float VerticalSpeed { get => _rb.velocity.y; }

    public event EventHandler OnJump;

    private void Awake()
    {
        _playerInputs = new PlayerInputs();
    }
    // Start is called before the first frame update
    public override void Start()
    {
        health = maxHealth;
        base.Start();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }
    public override void FixedUpdate(){
        _playerInputs.GetInputs();
        _movementX = _playerInputs.MovementX;
        if (state == _finiteState.ghostDash)
        {
            _movementY = _playerInputs.MovementY;
        }
        else
        {
            _movementY = 0;
        }
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
            if (_playerInputs.JumpButtonPressed && _lastGroundTime > 0){
                Jump();
                OnJump?.Invoke(this, null);
            }
            else if (_playerInputs.JumpButtonPressed && _lastGroundTime <= 0 && canDoubleJump && hasDoubleJump)
            {
                DoubleJump();
                OnJump?.Invoke(this, null);
            }
            if (_playerInputs.SlideButtonPressed){
                Slide();
            }
            if (_playerInputs.AttackButtonPressed){
                Attack();
            }
            if (_playerInputs.GhostDashButtonPressed){
                GhostDash();
            }
            //if (state == _finiteState.stand){
            //    if (isGrounded){
            //        _animator.PlayInFixedTime("Stand",-1,Time.fixedDeltaTime);
            //    } else {
            //        _animator.PlayInFixedTime("Jump",-1,Time.fixedDeltaTime);
            //    }
            //}
            //if (state == _finiteState.walk){
            //    if (isGrounded){
            //        _animator.PlayInFixedTime("Walk",-1,Time.fixedDeltaTime);
            //    } else {
            //        _animator.PlayInFixedTime("Jump",-1,Time.fixedDeltaTime);
            //    }
            //}
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
            //_animator.PlayInFixedTime("Walling",-1,Time.fixedDeltaTime);
            if (_playerInputs.JumpButtonPressed && _lastGroundTime <= 0 && canWallJump){
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
            if (_playerInputs.SlideButtonPressed){
                Slide();
            }
            if (_playerInputs.GhostDashButtonPressed){
                GhostDash();
            }
        }
        if (state == _finiteState.wallJumping){
            //_animator.PlayInFixedTime("Jump",-1,Time.fixedDeltaTime);
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
            if (_playerInputs.SlideButtonPressed){
                Slide();
            }
        }
        base.FixedUpdate();
        if (state == _finiteState.ghostDash){
            _canMove = false;
            _rb.velocity = new Vector2(_movementX * _ghostDashSpeed,_movementY * _ghostDashSpeed);
            _rb.gravityScale = 0;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("FireTile"),true);

        } else {
            if (_snapTolastNoneGhostPosition && isGrounded && _isAgainstLeftWall && _isAgainstRightWall || _snapTolastNoneGhostPosition && _touchingARoom == null){
                transform.position = _lastNoneGhostPosition;
            }
            _snapTolastNoneGhostPosition = false;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("FireTile"), false);
        }
        if (state == _finiteState.hurt){
            _stunDuration -= Time.deltaTime;
            if (_stunDuration <= 0){
                state = _finiteState.stand;
                _stunDuration = 0;
            }
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
    private void ShortHop(){
        if (!_playerInputs.JumpButton && !isGrounded)
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
    public void TakeDamage(float damage, float stunDuration, Vector2 knockBack, float friction){
        if (health <= 0)
        {
            //already dead. Do not take any more damage.
            return;
        }
        _canMove = false;
        state = _finiteState.hurt;
        _stunDuration = stunDuration;
        _rb.velocity = knockBack;
        _friction = friction;
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health == 0){
            Debug.Log("dead");
        }
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        /* debug buttons */
        if (GameDataManager.hasDebugButtons){
            if (Input.GetKeyDown(KeyCode.Alpha1)){
                GameDataManager.saveData();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                GameDataManager.loadData();
            }
        }
    }
}
