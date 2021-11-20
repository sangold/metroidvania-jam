using System.Collections;
using UnityEngine;
/*
for any human like figure will inherit this script.
*/
public class Humanoid : MonoBehaviour
{
    protected Rigidbody2D _rb;
    protected float _horizontalSpeed;
    protected float _maxHorizontalSpeed;
    protected float _maxVerticalSpeed;

    public PlayerCollision PlayerCollision { get; private set; }

    protected float _lastGroundTime;
    [SerializeField]
    private float _jumpCoyoteTime = .16f;

    protected float _movementX = 0;
    protected float _movementY = 0;
    protected float _jumpForce;

    protected bool _canMove = true;
    protected bool _disableMovement;
    protected bool _canGrab = true;

    protected bool canDoubleJump = false;// Can I doubleJump?
    
    public bool hasDoubleJump = true;// Do you have the ability?
    public bool hasWallJump = true;

    protected float _gravityScale;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        PlayerCollision = GetComponent<PlayerCollision>();
        _gravityScale = _rb.gravityScale;
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
        _rb.velocity = new Vector2(dir.x * _horizontalSpeed, _rb.velocity.y);
    }

    protected void WallSlide()
    {
        bool towardWall = (_rb.velocity.x > 0 && PlayerCollision.OnRightWall) || (_rb.velocity.x < 0 && PlayerCollision.OnLeftWall);
        _rb.velocity = new Vector2(towardWall ? 0 : _rb.velocity.x, -_maxVerticalSpeed);
    }

    protected void WallJump()
    {
        StopCoroutine(DisableMovementForWallJump(0));
        StartCoroutine(DisableMovementForWallJump(.35f));

        Jump((Vector2.up + new Vector2(PlayerCollision.WallSide, 0)).normalized);
    }

    private IEnumerator DisableMovementForWallJump(float time)
    {
        _disableMovement = true;
        _canGrab = false;
        yield return new WaitForSeconds(time);
        _canGrab = true;
        _disableMovement = false;
    }

    protected void Jump(Vector2 dir){
        _rb.velocity = new Vector2(_rb.velocity.x,0);
        _rb.velocity += dir * _jumpForce;
    }

    public virtual void Update(){}
}
