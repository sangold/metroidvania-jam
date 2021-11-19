using MV.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Humanoid
{
    [SerializeField]
    private float _jumpShortMultiplier;
    private PlayerStateSO _currentState;
    public PlayerStateSO CurrentState => _currentState;
    [SerializeField]
    private List<PlayerStateSO> _states;
    [SerializeField]
    private Animator _animator;

    private PlayerInputs _playerInputs;
    
    private Vector2 _lastNoneGhostPosition;
    private bool _snapTolastNoneGhostPosition = false;
    
    [SerializeField]
    private GameObject _spriteGameObject;

    [SerializeField]
    private CapsuleCollider2D _capsuleCollider2D;

    [SerializeField]
    private float maxHealth = 100;
    private float health = 0;
    private float _stunDuration = 0;

    public string _touchingARoom = null;
    public float VerticalSpeed { get => _rb.velocity.y; }

    public event EventHandler OnJump;

    protected override void Awake()
    {
        base.Awake();
        _playerInputs = new PlayerInputs();
        SetState(PlayerState.STANDARD);
        health = maxHealth;
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    public override void FixedUpdate(){
        _playerInputs.GetInputs();
        _movementX = _playerInputs.MovementX;
        if (_currentState.StateType == PlayerState.GHOSTDASH)
        {
            _movementY = _playerInputs.MovementY;
        }
        else
        {
            _movementY = 0;
        }

        if (_currentState.CanDash && _playerInputs.SlideButtonPressed)
        {
            Slide();
        }
        if (_currentState.CanAttack && _playerInputs.AttackButtonPressed)
        {
            Attack();
        }
        if (_currentState.CanGhost && _playerInputs.GhostDashButtonPressed)
        {
            GhostDash();
        }
        if (_currentState.CanJump && _playerInputs.JumpButtonPressed)
        {
            Jump();
            OnJump?.Invoke(this, null);
        }

        ShortHop();

        if(_currentState.HasStandardTransition)
        {
            if (canWallJump)
            {
                SetState(PlayerState.WALLING);
            }
            if (isGrounded && _rb.velocity.y <= 0f)
            {
                SetState(PlayerState.STANDARD);
            }
        }


        if (_currentState.StateType == PlayerState.STANDARD){
            
            // Visual Update
            if (_movementX > 0){
                TurnRight();
            } else if (_movementX < 0){
                TurnLeft();
            } else {
                // if no movement
            }    
        }
        if (_currentState.StateType == PlayerState.SLIDE){
            // Transition
            if (Mathf.Abs(_rb.velocity.x) < 10){
                SetState(PlayerState.STANDARD);
            }
        }
        if (_currentState.StateType == PlayerState.WALLING)
        {
            // Specific movement
            if (_playerInputs.JumpButtonPressed && _lastGroundTime <= 0 && canWallJump){
                SetState(PlayerState.INAIR);
                WallJump();
            }

            // Visual Update
            if (_isAgainstLeftWall){
                TurnRight();
            } else if (_isAgainstRightWall){
                TurnLeft();
            }
        }

        if (_currentState.StateType == PlayerState.INAIR){
            
            // Specific movement
            if (_playerInputs.JumpButtonPressed && _lastGroundTime <= 0 && canDoubleJump && hasDoubleJump)
            {
                DoubleJump();
                OnJump?.Invoke(this, null);
            }
        }

        if (_currentState.StateType == PlayerState.ATTACK){
            if (!isGrounded){
                _rb.velocity += new Vector2(_movementX * _airHorizontalSpeed * Time.fixedDeltaTime,0);   
            }
        }

        if (_currentState.StateType == PlayerState.GHOSTDASH){
            _rb.velocity = new Vector2(_movementX * _groundHorizontalSpeed,_movementY * _groundHorizontalSpeed);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("FireTile"),true);
        } else {
            if (_snapTolastNoneGhostPosition && isGrounded && _isAgainstLeftWall && _isAgainstRightWall || _snapTolastNoneGhostPosition && _touchingARoom == null){
                transform.position = _lastNoneGhostPosition;
            }
            _snapTolastNoneGhostPosition = false;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("FireTile"), false);
        }
        if (_currentState.StateType == PlayerState.HURT){
            _stunDuration -= Time.deltaTime;
            if (_stunDuration <= 0){
                SetState(PlayerState.STANDARD);
                _stunDuration = 0;
            }
        }
        base.FixedUpdate();
    }

    private void SetState(PlayerState targetState)
    {
        if (_currentState != null && _currentState.StateType == targetState)
            return;

        _currentState = _states[(int)targetState];
        _friction = _currentState.Friction;
        _rb.gravityScale = _currentState.gravityScale;
        _canMove = _currentState.CanMove;
        _groundHorizontalSpeed = _currentState.HSpeed;
        _maxHorizontalSpeed = _currentState.MaxHSpeed;
        _maxVerticalSpeed = _currentState.MaxVSpeed;
        _jumpForce = _currentState.VSpeed;
        Debug.Log(_currentState);
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
        SetState(PlayerState.SLIDE);
        _rb.velocity = new Vector2(_groundHorizontalSpeed * GetFaceDirection(),_rb.velocity.y);
    }
    private void Attack(){
        _animator.PlayInFixedTime("Attacking",-1,Time.fixedDeltaTime);
        SetState(PlayerState.ATTACK);
        StartCoroutine(WaitAnim(.5f));
    }
    private void GhostDash(){
        _rb.velocity = new Vector2(0,0);
        _lastNoneGhostPosition = new Vector2(transform.position.x,transform.position.y);
        _animator.PlayInFixedTime("GhostDash",-1,Time.fixedDeltaTime);
        SetState(PlayerState.GHOSTDASH);
        StartCoroutine(WaitAnim(10f/60f));
        _snapTolastNoneGhostPosition = true;
    }

    private IEnumerator WaitAnim(float time)
    {
        yield return new WaitForSeconds(time);
        SetState(isGrounded ? PlayerState.STANDARD : PlayerState.INAIR);
    }

    public void TakeDamage(float damage, float stunDuration, Vector2 knockBack, float friction){
        if (health <= 0)
        {
            //already dead. Do not take any more damage.
            return;
        }
        _canMove = false;
        SetState(PlayerState.HURT);
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
    }
}
