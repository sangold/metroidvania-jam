using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
for any human like figure will inherit this script.
*/
public class Humanoid : MonoBehaviour
{
    protected float _friction = 0.000001f;// 0.000001 to 1 
    protected Rigidbody2D _rb;
    [SerializeField]
    private float _groundHorizontalSpeed = 100;
    [SerializeField]
    protected float _airHorizontalSpeed = 50;
    [SerializeField]
    protected float _maxHorizontalSpeed = 15;

    //ground collision
    public bool isGrounded = false;
    protected bool _isAgainstLeftWall = false;
    protected bool _isAgainstRightWall = false;
    [SerializeField]
    private Transform _leftWallCheck;
    [SerializeField]
    private Transform _righttWallCheck;
    [SerializeField]
    private Transform _groundCheck1;
    [SerializeField]
    private LayerMask _groundLayer;

    protected float _lastGroundTime;
    protected float _lastJumpTime;
    [SerializeField]
    private float _jumpCoyoteTime = .16f;

    protected float _movementX = 0;
    protected float _movementY = 0;
    protected bool _jumpButton = false;
    protected bool _jumpButtonSwitch = false;
    protected bool _jumpButtonPressed = false;
    [SerializeField]
    private float _jumpForce = 50;

    protected bool _canMove = true;


    protected bool canDoubleJump = false;// Can I doubleJump?
    protected bool canWallJump = false;
    
    public bool hasDoubleJump = true;// Do you have the ability?
    public bool hasWallJump = true;

    protected float _gravityScale;
    // Start is called before the first frame update
    public virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _gravityScale = _rb.gravityScale;
    }
    public virtual void FixedUpdate()
    {
        if (_canMove){
            if (isGrounded){
                _rb.velocity = new Vector2(_movementX * _groundHorizontalSpeed,_rb.velocity.y);
            } else {
                 _rb.velocity += new Vector2(_movementX * _airHorizontalSpeed * Time.fixedDeltaTime,0);
            }
            _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x,-_maxHorizontalSpeed,_maxHorizontalSpeed),_rb.velocity.y);
        }
        if (Mathf.Abs(_rb.velocity.x) < .01f){
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
        #region CollisionDetection
        isGrounded = Physics2D.OverlapCircle(_groundCheck1.position, 0.15f, _groundLayer);
        _isAgainstLeftWall = Physics2D.OverlapCircle(_leftWallCheck.position, 0.15f, _groundLayer);
        _isAgainstRightWall = Physics2D.OverlapCircle(_righttWallCheck.position, 0.15f, _groundLayer);
        if((_isAgainstLeftWall || _isAgainstRightWall) && !isGrounded)
        {
            canWallJump = true;
            if(_rb.velocity.y <= 0)
                _rb.gravityScale = 1;
        }
        else
        {
            canWallJump = false;
            _rb.gravityScale = _gravityScale;
        }
        _rb.velocity = new Vector2(_rb.velocity.x * Mathf.Pow(_friction,Time.fixedDeltaTime),_rb.velocity.y);
        if (isGrounded){
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
        StartCoroutine(DisableMovementForWallJump(.1f));

        Vector2 jumpDirection = _isAgainstRightWall ? Vector2.left : Vector2.right;
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.velocity += (Vector2.up + jumpDirection).normalized * _jumpForce;
    }
    private IEnumerator DisableMovementForWallJump(float time)
    {
        _canMove = false;
        yield return new WaitForSeconds(time);
        _canMove = true;
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
