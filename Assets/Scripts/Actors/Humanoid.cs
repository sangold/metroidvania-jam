using System.Collections;
using UnityEngine;
/*
for any human like figure will inherit this script.
*/
public class Humanoid : Actor
{
    protected Rigidbody2D _rb;
    protected float _horizontalSpeed;
    protected float _verticalSpeed;

    public CollisionDetector PlayerCollision { get; private set; }

    protected float _lastGroundTime;
    protected float _lastWallTime;
    [SerializeField]
    protected float _jumpCoyoteTime = .16f;
    [SerializeField]
    protected float _wallJumpAcceleration = 5f;

    protected float _movementX = 0;
    public float MovementX {
        get{
            return _movementX;
        }
    }
    protected float _movementY = 0;
    protected float _jumpForce;

    protected bool _canMove = true;
    protected bool _isWallJumping;

    protected bool canDoubleJump = false;// Can I doubleJump?

    private IEnumerator _wallJumpCoroutine;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        PlayerCollision = GetComponent<CollisionDetector>();
    }
    public virtual void FixedUpdate()
    {
        Walk(new Vector2(_movementX, _movementY));

        _rb.velocity = new Vector2(_rb.velocity.x,
            Mathf.Clamp(_rb.velocity.y, -30f, 100f));

        #region CollisionDetection
        if (PlayerCollision.OnGround){
            _lastGroundTime = _jumpCoyoteTime;
            canDoubleJump = true;
        }
        #endregion

        #region Timers
        _lastGroundTime -= Time.fixedDeltaTime;
        _lastWallTime -= Time.fixedDeltaTime;
        #endregion
    }

    protected void Walk(Vector2 dir)
    {
        if (!_canMove)
            return;
        if(_isWallJumping)
        {
            _rb.velocity = Vector2.Lerp(_rb.velocity, new Vector2(PlayerCollision.WallSide * _horizontalSpeed, _rb.velocity.y), _wallJumpAcceleration * Time.fixedDeltaTime);
            return;
        }
        if(PlayerCollision.OnGround)
            _rb.velocity = new Vector2(dir.x * _horizontalSpeed, _rb.velocity.y);
        else
        {
            _rb.velocity += new Vector2(dir.x * _horizontalSpeed, 0);
            _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x, -_horizontalSpeed, _horizontalSpeed), _rb.velocity.y);
        }
    }

    protected void WallSlide()
    {
        if(_wallJumpCoroutine != null)
            StopCoroutine(_wallJumpCoroutine);
        _isWallJumping = false;
        bool towardWall = (_rb.velocity.x > 0 && PlayerCollision.OnRightWall) || (_rb.velocity.x < 0 && PlayerCollision.OnLeftWall);
        _rb.velocity = new Vector2(towardWall ? 0 : _rb.velocity.x, -_verticalSpeed);
    }

    protected void WallJump()
    {
        if(_wallJumpCoroutine != null)
            StopCoroutine(_wallJumpCoroutine);
        _wallJumpCoroutine = DisableMovementForWallJump(.2f);
        _isWallJumping = true;
        StartCoroutine(_wallJumpCoroutine);
        _rb.velocity = Vector2.zero;
        Jump((Vector2.up + new Vector2(PlayerCollision.WallSide/2f, 0)).normalized / 1.3f);
    }

    private IEnumerator DisableMovementForWallJump(float time)
    {   
        yield return new WaitForSeconds(time);
        _isWallJumping = false;
        _wallJumpCoroutine = null;
    }

    protected void Jump(Vector2 dir){
        _rb.velocity = new Vector2(_rb.velocity.x,0);
        _rb.velocity += dir * _jumpForce;
    }

    public virtual void Update(){}
}
