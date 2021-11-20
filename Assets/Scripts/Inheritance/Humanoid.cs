using System.Collections;
using UnityEngine;
/*
for any human like figure will inherit this script.
*/
public class Humanoid : MonoBehaviour
{
    protected Rigidbody2D _rb;
    protected float _horizontalSpeed;
    protected float _verticalSpeed;

    public PlayerCollision PlayerCollision { get; private set; }

    protected float _lastGroundTime;
    [SerializeField]
    private float _jumpCoyoteTime = .16f;
    [SerializeField]
    protected float _wallJumpAcceleration = 5f;

    protected float _movementX = 0;
    protected float _movementY = 0;
    protected float _jumpForce;

    protected bool _canMove = true;
    protected bool _isWallJumping;

    protected bool canDoubleJump = false;// Can I doubleJump?
    
    public bool hasDoubleJump = true;// Do you have the ability?
    public bool hasWallJump = true;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        PlayerCollision = GetComponent<PlayerCollision>();
    }
    public virtual void FixedUpdate()
    {
        Walk(new Vector2(_movementX, _movementY));

        #region CollisionDetection
        if (PlayerCollision.OnGround){
            _lastGroundTime = _jumpCoyoteTime;
            canDoubleJump = true;
        }
        #endregion

        #region Timers
        _lastGroundTime -= Time.fixedDeltaTime;
        #endregion
    }

    protected void Walk(Vector2 dir)
    {
        if (!_canMove)
            return;
        if(_isWallJumping)
        {
            _rb.velocity = Vector2.Lerp(_rb.velocity, new Vector2( _horizontalSpeed, _rb.velocity.y), _wallJumpAcceleration * Time.fixedDeltaTime);
            Debug.Log(_rb.velocity);
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
        StopCoroutine(DisableMovementForWallJump(0));
        _isWallJumping = false;
        bool towardWall = (_rb.velocity.x > 0 && PlayerCollision.OnRightWall) || (_rb.velocity.x < 0 && PlayerCollision.OnLeftWall);
        _rb.velocity = new Vector2(towardWall ? 0 : _rb.velocity.x, -_verticalSpeed);
    }

    protected void WallJump()
    {
        StopCoroutine(DisableMovementForWallJump(0));
        StartCoroutine(DisableMovementForWallJump(.2f));

        Jump((Vector2.up + new Vector2(PlayerCollision.WallSide, 0)).normalized);
    }

    private IEnumerator DisableMovementForWallJump(float time)
    {
        _isWallJumping = true;
        yield return new WaitForSeconds(time);
        _isWallJumping = false;
    }

    protected void Jump(Vector2 dir){
        _rb.velocity = new Vector2(_rb.velocity.x,0);
        _rb.velocity += dir * _jumpForce;
    }

    public virtual void Update(){}
}
