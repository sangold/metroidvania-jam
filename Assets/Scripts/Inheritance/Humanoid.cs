using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
for any human like figure will inherit this script.
*/
public class Humanoid : MonoBehaviour
{
    private float _friction = 0.000001f;// 0.000001 to 1 
    protected Rigidbody2D _rb;
    [SerializeField]
    private float _groundHorizontalSpeed = 100;
    [SerializeField]
    private float _airHorizontalSpeed = 50;
    [SerializeField]
    private float _maxHorizontalSpeed = 15;

    //ground collision
    protected bool isGrounded = false;
    [SerializeField]
    private Transform _groundCheck1;
    [SerializeField]
    private LayerMask _groundLayer;

    protected float _lastGroundTime;
    protected float _lastJumpTime;
    [SerializeField]
    private float _jumpCoyoteTime = .16f;

    protected float _movementX = 0;
    protected bool _jumpButton = false;
    protected bool _jumpButtonSwitch = false;
    protected bool _jumpButtonPressed = false;
    [SerializeField]
    private float _jumpForce = 50;


    protected bool canDoubleJump = false;// Can I doubleJump?
    
    public bool hasDoubleJump = true;// Do you have the ability?
    // Start is called before the first frame update
    public virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    public virtual void FixedUpdate()
    {
        if (isGrounded){
            _rb.velocity = new Vector2(_movementX * _groundHorizontalSpeed,_rb.velocity.y);
        } else {
            _rb.velocity += new Vector2(_movementX * _airHorizontalSpeed * Time.fixedDeltaTime,0);
        }
        if (_rb.velocity.x > _maxHorizontalSpeed){
            _rb.velocity = new Vector2(_maxHorizontalSpeed,_rb.velocity.y);
        }
        if (_rb.velocity.x < -_maxHorizontalSpeed){
            _rb.velocity = new Vector2(-_maxHorizontalSpeed,_rb.velocity.y);
        }
        if (Mathf.Abs(_rb.velocity.x) < .01f){
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
        _rb.velocity = new Vector2(_rb.velocity.x * Mathf.Pow(_friction,Time.fixedDeltaTime),_rb.velocity.y);
        isGrounded = Physics2D.OverlapCircle(_groundCheck1.position, 0.15f, _groundLayer);
        if (isGrounded){
            _lastGroundTime = _jumpCoyoteTime;
            canDoubleJump = true;
        }
        _lastGroundTime -= Time.fixedDeltaTime;
        _lastJumpTime -= Time.fixedDeltaTime;
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
