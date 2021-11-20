using System.Collections;
using UnityEngine;
/*
for any human like figure will inherit this script.
*/
public class Humanoid : MonoBehaviour
{
    protected float _friction;// 0.000001 to 1 
    protected Rigidbody2D _rb;
    protected float _horizontalSpeed;
    protected float _maxHorizontalSpeed;
    protected float _maxVerticalSpeed;

    public PlayerCollision PlayerCollision { get; private set; }

    protected float _lastGroundTime;
    protected float _lastJumpTime;
    [SerializeField]
    private float _jumpCoyoteTime = .16f;

    protected float _movementX = 0;
    protected float _movementY = 0;
    protected float _jumpForce;

    protected bool _canMove = true;
    protected bool _disableMovement;
    protected bool _canGrab = true;


    protected bool canDoubleJump = false;// Can I doubleJump?
    protected bool canWallJump = false;
    
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
        if (_canMove && !_disableMovement)
        {
            if (PlayerCollision.OnGround)
            {
                _rb.velocity = new Vector2(_movementX * _horizontalSpeed, _rb.velocity.y);
            }
            else
            {
                _rb.velocity += new Vector2(_movementX * _horizontalSpeed, 0);
            }
        }
        if (Mathf.Abs(_rb.velocity.x) < .01f){
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
        _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x, -_maxHorizontalSpeed, _maxHorizontalSpeed), Mathf.Clamp(_rb.velocity.y, -_maxVerticalSpeed, _maxVerticalSpeed));

        #region CollisionDetection
        if (PlayerCollision.OnGround){
            _lastGroundTime = _jumpCoyoteTime;
            canDoubleJump = true;
        }
        #endregion

        #region Timers
        _lastGroundTime -= Time.fixedDeltaTime;
        _lastJumpTime -= Time.fixedDeltaTime;
        #endregion
    }
    protected void WallJump()
    {
        StopCoroutine(DisableMovementForWallJump(0));
        StartCoroutine(DisableMovementForWallJump(.35f));
        _rb.velocity = new Vector2(_horizontalSpeed * -PlayerCollision.WallSide, _jumpForce);
    }
    private IEnumerator DisableMovementForWallJump(float time)
    {
        _disableMovement = true;
        _canGrab = false;
        yield return new WaitForSeconds(time);
        _canGrab = true;
        _disableMovement = false;
    }
    protected void Jump(){
        _rb.velocity = new Vector2(_rb.velocity.x,_jumpForce);
        _lastJumpTime = 0;
    }
    protected void DoubleJump(){
        _rb.velocity = new Vector2(_rb.velocity.x,_jumpForce);
        _lastJumpTime = 0;
        canDoubleJump = false;
    }
    // Update is called once per frame
    public virtual void Update()
    {
        
    }
}
