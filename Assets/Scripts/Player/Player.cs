using MV.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Humanoid
{

    [SerializeField]
    private float _fallMultiplier = 3f;
    [SerializeField]
    private float _fastFallMultiplier = 8f;
    private PlayerStateSO _currentState;
    public PlayerStateSO CurrentState => _currentState;
    [SerializeField]
    private List<PlayerStateSO> _states;
    
    private PlayerInputs _playerInputs;
    
    private Vector2 _lastNoneGhostPosition;
    private bool _snapTolastNoneGhostPosition = false;
    
    [SerializeField]
    private GameObject _spriteGameObject;

    [SerializeField]
    private float maxHealth = 100;
    private float health = 0;

    private PlayerPostWiseEvent _postWiseEvent;
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
    public float HorizontalSpeed { get => _rb.velocity.x; }
    public event EventHandler OnJump;
    public event EventHandler OnAttack;

    protected override void Awake()
    {
        base.Awake();
        _playerInputs = new PlayerInputs();
        SetState(PlayerState.STANDARD);
        health = maxHealth;
        _postWiseEvent = GetComponent<PlayerPostWiseEvent>();
    }
    public override void FixedUpdate(){
        base.FixedUpdate();

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
            Dash();
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
            SetState(PlayerState.INAIR);
            Jump(Vector2.up);
            OnJump?.Invoke(this, null);
            _postWiseEvent.Player_Jump_Event.Post(this.gameObject);
        }
        if (_currentState.StateType == PlayerState.INAIR)
        {

            // Specific movement
            if (_playerInputs.JumpButtonPressed && _lastGroundTime <= 0 && canDoubleJump && hasDoubleJump && !PlayerCollision.OnGround)
            {
                Jump(Vector2.up);
                canDoubleJump = false;
                OnJump?.Invoke(this, null);
                _postWiseEvent.Player_Double_Jump_Event.Post(this.gameObject);
            }
            //sound
            if (PlayerCollision.OnGround && _rb.velocity.y <= 0){
                Debug.Log("landed on ground");
                _postWiseEvent.Player_Landed_Event.Post(this.gameObject);
            }
        }
        
        if(_canMove)
            SnappyJump();

        if(_currentState.HasStandardTransition)
        {
            if (PlayerCollision.OnGround && _rb.velocity.y <= 0.1f)
            {
                SetState(PlayerState.STANDARD);
            }
            else if (PlayerCollision.IsPushingAgainstAWall(_movementX) && _lastGroundTime < 0)
            {
                if((PlayerCollision.OnLeftWall && _rb.velocity.x <= 0) || (PlayerCollision.OnRightWall && _rb.velocity.x >= 0))
                SetState(PlayerState.WALL_SLIDE);
            }
            else if (!PlayerCollision.OnGround)
            {
                SetState(PlayerState.INAIR);
            }
        }


        if (_currentState.StateType == PlayerState.STANDARD || _currentState.StateType == PlayerState.INAIR){
            
            // Visual Update
            if(_isWallJumping)
            {

            }
            else if (_movementX > 0){
                TurnRight();
            } else if (_movementX < 0){
                TurnLeft();
            } else {
                // if no movement
            }    
        }
        if (_currentState.StateType == PlayerState.DASH){
            // Transition
            if (Mathf.Abs(_rb.velocity.x) < 2f){
                SetState(PlayerCollision.OnGround ? PlayerState.STANDARD : PlayerState.INAIR);
            }
        }
        if (_currentState.StateType == PlayerState.WALL_SLIDE)
        {
            WallSlide();

            // Specific movement
            if (_playerInputs.JumpButtonPressed && _lastGroundTime <= 0 && hasWallJump)
            {
                SetState(PlayerState.INAIR);
                WallJump();
                _postWiseEvent.Player_Jump_Event.Post(this.gameObject);
            }

            // Visual Update
            if (PlayerCollision.OnLeftWall){
                TurnRight();
            } else if (PlayerCollision.OnRightWall)
            {
                TurnLeft();
            } else {
                SetState(PlayerState.INAIR);
            }
            if (PlayerCollision.OnGround){
                SetState(PlayerState.STANDARD);
            }
        }

        if (_currentState.StateType == PlayerState.ATTACK){
            if (!PlayerCollision.OnGround)
                Walk(new Vector2(_movementX * _horizontalSpeed, 0));
        }

        if (_currentState.StateType == PlayerState.GHOSTDASH){
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("FireTile"),true);
        } else {
            if (_snapTolastNoneGhostPosition && PlayerCollision.OnGround && PlayerCollision.OnLeftWall && PlayerCollision.OnRightWall || _snapTolastNoneGhostPosition && _touchingARoom == null){
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
    }

    public void SetState(PlayerState targetState)
    {
        if (_currentState != null && _currentState.StateType == targetState)
            return;
        
        _currentState = _states[(int)targetState];
        _rb.gravityScale = _currentState.GravityScale;
        _canMove = _currentState.CanMove;
        _horizontalSpeed = _currentState.HorizontalSpeed;
        _verticalSpeed = _currentState.VerticalSpeed;
        _jumpForce = _currentState.JumpForce;
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
    private void SnappyJump(){
        if(_rb.velocity.y < 0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (!_playerInputs.JumpButton && _rb.velocity.y > 0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fastFallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
    private void Dash(){
        SetState(PlayerState.DASH);
        _rb.velocity = Vector2.zero;
        _rb.velocity += new Vector2(_horizontalSpeed * GetFaceDirection(), 0);
        WaitStateDuration(.2f);
        _postWiseEvent.Player_Slide_Event.Post(this.gameObject);
    }
    private void Attack(){
        OnAttack?.Invoke(this, null);
        SetState(PlayerState.ATTACK);
        WaitStateDuration(.1f);
        _postWiseEvent.Player_Slash_Event.Post(this.gameObject);
    }
    private void GhostDash(){
        _rb.velocity = new Vector2(0,0);
        _lastNoneGhostPosition = new Vector2(transform.position.x,transform.position.y);
        SetState(PlayerState.GHOSTDASH);
        WaitStateDuration(.5f);
        _snapTolastNoneGhostPosition = true;
        _postWiseEvent.Player_Ghost_Dash_Event.Post(this.gameObject);
    }

    private void WaitStateDuration(float duration)
    {
        StopCoroutine(WaitAnim(0));
        StartCoroutine(WaitAnim(duration));
    }

    private IEnumerator WaitAnim(float time)
    {
        yield return new WaitForSeconds(time);
        SetState(PlayerCollision.OnGround ? PlayerState.STANDARD : PlayerState.INAIR);
    }

    public void TakeDamage(float damage, float stunDuration, Vector2 knockBack, float friction){
        if (health <= 0)
        {
            //already dead. Do not take any more damage.
            return;
        }
        SetState(PlayerState.HURT);
        _stunDuration = stunDuration;
        _rb.velocity = knockBack;
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
