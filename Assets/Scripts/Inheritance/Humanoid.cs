using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
for any human like figure will inherit this script.
*/
public class Humanoid : MonoBehaviour
{
    protected Rigidbody2D _rb;

    [SerializeField] 
    private float _moveSpeed;
    [SerializeField] 
    private float _acceleration;
    [SerializeField] 
    private float _deceleration;
    [SerializeField] 
    private float _airAcceleration;
    [SerializeField] 
    private float _airDeceleration;
    [SerializeField] 
    private float _velocityPower;

    [SerializeField] 
    private float _friction = .02f;

    private bool _canMove = true;
    protected bool _isJumping;
    
    [SerializeField] 
    private float _fallGravityMultiplier;

    [SerializeField] 
    private float _jumpForce;
    private float _gravityScale;
    private float _lastGroundTime;
    protected float _lastJumpTime;
    [SerializeField] 
    private float _jumpCoyoteTime;
    

    //ground collision
    [HideInInspector]
     public bool isGrounded = false;
     public Transform GroundCheck1;
     public LayerMask groundLayer;


    protected float movementX = 0;
    protected bool jumpButton = false;
    protected bool jumpButtonPressed = false;

    [HideInInspector]
    public float JumpPower = 25;


    private bool canDoubleJump = false;// Can I doubleJump?
    public bool doubleJumpEnabled = true;// Do you have the ability?
    // Start is called before the first frame update
    public virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _gravityScale = _rb.gravityScale;
    }

    public virtual void FixedUpdate()
    {
        if(_canMove)
        {
            float targetSpeed = movementX * _moveSpeed;
            float speedDiff = targetSpeed - _rb.velocity.x;
            float accelRate;
            if (_lastGroundTime > 0)
                accelRate = Mathf.Abs(targetSpeed) > .01f ? _acceleration : _deceleration;
            else
                accelRate = Mathf.Abs(targetSpeed) > .01f ? _acceleration * _airAcceleration : _deceleration * _airDeceleration;

            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, _velocityPower) * Mathf.Sign(speedDiff);
            _rb.AddForce(movement * Vector2.right);
        }

        if(_lastGroundTime > 0 && !_isJumping && Mathf.Abs(movementX) < .01f)
        {
            float fAmount = Mathf.Min(Mathf.Abs(_rb.velocity.x), Mathf.Abs(_friction));
            fAmount *= -Mathf.Sign(_rb.velocity.x);
            _rb.AddForce(Vector2.right * fAmount, ForceMode2D.Impulse);
        }

        if(_rb.velocity.y < 0 && _lastGroundTime <= 0)
        {
            _rb.gravityScale = _gravityScale * _fallGravityMultiplier;
        }
        else
        {
            _rb.gravityScale = _gravityScale;
        }
        
        //if (jumpButton && jumpButtonPressed && isGrounded){
        //    Jump();
        //}
        //if (jumpButton && jumpButtonPressed && !isGrounded && doubleJumpEnabled && canDoubleJump){
        //    DoubleJump();
        //}
        //if (isGrounded){
        //    //allow the ability to double jump again.
        //    canDoubleJump = true;
        //}
    }
    private void Jump(float jumpForce){
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        _lastJumpTime = 0;
        _isJumping = true;
        jumpButtonPressed = true;
    }
    private void DoubleJump(){
        _rb.velocity = new Vector2(_rb.velocity.x,JumpPower);
        canDoubleJump = false;
    }
    // Update is called once per frame
    public virtual void Update()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck1.position, 0.15f, groundLayer);

        if (isGrounded && !_isJumping)
            _lastGroundTime = _jumpCoyoteTime;

        if (_rb.velocity.y <= 0)
            _isJumping = false;

        if (_lastJumpTime > 0 && !_isJumping && jumpButtonPressed)
        {
            if (_lastGroundTime > 0)
            {
                _lastGroundTime = 0;
                Jump(_jumpForce);
            }
        }

        _lastGroundTime -= Time.deltaTime;
        _lastJumpTime -= Time.deltaTime;

    }
}
