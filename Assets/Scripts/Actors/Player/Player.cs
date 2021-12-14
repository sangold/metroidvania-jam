using MV.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Humanoid
{
    [SerializeField]
    private PlayerStatsSO _playerStats;
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
    
    private HealthComponent _healthComponent;

    [SerializeField]
    private PlayerPostWiseEvent _postWiseEvent;
    private MeleeAttackComponent _meleeAttackComponent;
    public int Health {
        get {
            return _healthComponent.Health;
        }
        set
        {
            _healthComponent.Health = value;
            _playerStats.CurrentHealth = value;
        }
    }
    public int MaxHealth
    {
        get
        {
            return _healthComponent.MaxHealth;
        }
        set
        {
            _healthComponent.MaxHealth = value;
            _playerStats.MaxHealth = value;
        }
    }


    private float _stunDurationTimer = 0;
    public float VerticalSpeed { get => _rb.velocity.y; }
    public float HorizontalSpeed { get => _rb.velocity.x; }
    public event EventHandler OnJump;
    public event EventHandler OnAttack;
    public event EventHandler OnChargeSpinAttack;
    [SerializeField]
    private ModalEvent _onPowerUpPickup;
    [SerializeField]
    private GameEvent _onPortalTaken;
    public bool IsInInteractivePortalRange;

    private bool _canDoChargeAttack;
    private float _chargeAttackTimer = 0;

    public void LoadData(float x, float y, int currentHealth, int maxHealth, bool hasScythe, bool hasMirror, bool hasDoubleJump, bool hasWallJump, bool hasGhostDash, bool hasDash, bool hasChargeAttack)
    {
        transform.position = new Vector2(x, y);
        Health = currentHealth;
        MaxHealth = maxHealth;
        _playerStats.LoadData(currentHealth, maxHealth, hasScythe, hasMirror, hasDoubleJump, hasWallJump, hasGhostDash, hasDash, hasChargeAttack);
    }

    protected override void Awake()
    {
        base.Awake();
        _playerInputs = new PlayerInputs();
        _meleeAttackComponent = GetComponent<MeleeAttackComponent>();
        _healthComponent = GetComponent<HealthComponent>();
        SetState(PlayerState.STANDARD);
    }

    private void Start()
    {
        _healthComponent.OnDamageTaken += OnDamageTaken;
        _healthComponent.OnHealthIncreased += OnHealthIncreased;
        _healthComponent.OnHealthPieceCollected += OnHealthPieceCollected;
        _healthComponent.MaxHealth = _playerStats.MaxHealth;
        _healthComponent.Health = _playerStats.CurrentHealth;
    }

    public override void FixedUpdate(){

        base.FixedUpdate();

        if (!GameManager.Instance.PlayerCanMove)
        {
            _movementX = 0;
            _movementY = 0;
            return;
        }

        _playerInputs.GetInputs();
        _movementX = _playerInputs.MovementX;
        _movementY = _playerInputs.MovementY;

        if(IsInInteractivePortalRange && _playerInputs.AttackButtonPressed)
        {
            _onPortalTaken.Raise();
        }

        if (_currentState.CanDash && _playerInputs.SlideButtonPressed)
        {
            Dash();
        }
        if (_currentState.CanAttack && _playerInputs.AttackButtonPressed && _playerStats.HasScythe)
        {
            Attack();
        }
        if (_currentState.CanGhost && _playerInputs.GhostDashButtonPressed && _playerStats.HasGhostDash)
        {
            GhostDash();
        }
        if (_currentState.CanChargeAttack && _playerStats.HasScythe && _playerStats.HasChargeAttack)
        {
            ChargeSpinAttack();
        }
        if (_currentState.CanJump && _playerInputs.JumpButtonPressed)
        {
            SetState(PlayerState.INAIR);
            Jump(Vector2.up);
            OnJump?.Invoke(this, null);
            _postWiseEvent.Player_Jump_Event.Post(this.gameObject);
        }
        if (_playerInputs.MovementY < -.5f)
        {
            //go through platform.
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"),true);
        } else {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"),false);
        }
        if (_currentState.StateType == PlayerState.INAIR)
        {

            // Specific movement
            if(_playerInputs.JumpButtonPressed && _lastWallTime > 0 && _playerStats.HasWallJump)
            {
                WallJump();
            }
            else if (_playerInputs.JumpButtonPressed && _lastGroundTime <= 0 && canDoubleJump && _playerStats.HasDoubleJump && !PlayerCollision.OnGround)
            {
                Jump(Vector2.up);
                canDoubleJump = false;
                OnJump?.Invoke(this, null);
                _postWiseEvent.Player_Double_Jump_Event.Post(this.gameObject);
            }
            //sound
            if (PlayerCollision.OnGround && _rb.velocity.y <= -3f){
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
            //else if (PlayerCollision.IsPushingAgainstAWall(_movementX) && _lastGroundTime < 0)
            //{
            //    if((PlayerCollision.OnLeftWall && _rb.velocity.x <= 0) || (PlayerCollision.OnRightWall && _rb.velocity.x >= 0))
            //    SetState(PlayerState.WALL_SLIDE);
            //}
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
            _lastWallTime = _jumpCoyoteTime;
            WallSlide();
            // Specific movement
            if (_playerInputs.JumpButtonPressed && _lastGroundTime <= 0 && _playerStats.HasWallJump)
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

        //if (_currentState.StateType == PlayerState.ATTACK){
        //    if (!PlayerCollision.OnGround)
        //        Walk(new Vector2(_movementX * _horizontalSpeed, 0));
        //}

        if (_currentState.StateType == PlayerState.GHOSTDASH){
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("GhostDashable"),true);
        } else {
            if (_snapTolastNoneGhostPosition && PlayerCollision.OnGround && PlayerCollision.OnLeftWall && PlayerCollision.OnRightWall || _snapTolastNoneGhostPosition){
                transform.position = _lastNoneGhostPosition;
            }
            _snapTolastNoneGhostPosition = false;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("GhostDashable"), false);
        }
        if (_currentState.StateType == PlayerState.HURT){
            _stunDurationTimer -= Time.deltaTime;
            if (_stunDurationTimer <= 0){
                SetState(PlayerState.STANDARD);
                _stunDurationTimer = 0;
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

    public void GetItem(CollectableSO collectedItem)
    {
        if (collectedItem.CollectableType == CollectableEnum.HEALTH_UP)
        {
            _healthComponent.HealthPiecesCollected += 1;
        }
        else if (collectedItem.CollectableType == CollectableEnum.MANA_UP)
        {

        }
        else if (collectedItem is PowerUpCollectableSO puc)
        {
            _playerStats.UnlockAbility(puc.PowerUpType);
            _onPowerUpPickup.Raise(puc.Description);
        }
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
        _rb.velocity += new Vector2(_horizontalSpeed * (IsTurnToTheLeft() ? -1 : 1), 0);
        WaitStateDuration(.2f);
        _postWiseEvent.Player_Slide_Event.Post(this.gameObject);
    }
    private void ChargeSpinAttack(){
        if (_playerInputs.AttackButton){
            _chargeAttackTimer += Time.fixedDeltaTime;
            if (_chargeAttackTimer >= 1){
              _canDoChargeAttack = true;
            }  
        } else {
            if (_canDoChargeAttack){
                DoChargeAttack();
                _canDoChargeAttack = false;
            }
            _chargeAttackTimer = 0;
        }
    }
    private void DoChargeAttack(){
        OnChargeSpinAttack?.Invoke(this, null);
        SetState(PlayerState.SPIN_ATTACK);
    }
    private void Attack(){
        OnAttack?.Invoke(this, null);
        SetState(PlayerState.ATTACK);
        _meleeAttackComponent.Attack();
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

    public void OnDamageTaken(int newHealth, Vector3 attackOrigin)
    { 
        SetState(PlayerState.HURT);
        _rb.velocity = Vector2.zero;
        Vector2 knockbackDirection = transform.position - attackOrigin;
        knockbackDirection.y = 0;
        Vector2 knocbackVelocity = knockbackDirection.normalized * _horizontalSpeed;
        knocbackVelocity += new Vector2(0, _verticalSpeed);
        _rb.velocity = knocbackVelocity;
        _stunDurationTimer = _healthComponent.StunDuration;
        _playerStats.CurrentHealth = newHealth;
    }

    private void OnHealthIncreased(int maxHP)
    {
        _playerStats.MaxHealth = maxHP;
    }

    private void OnHealthPieceCollected(int newCount)
    {
        _playerStats.HealthCollectible = newCount;
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

    public bool CheckAbility(PowerUpType type)
    {
        return _playerStats.CheckAbility(type);
    }
}
