using UnityEngine;
/*
for any human like figure will inherit this script.
*/
public class Humanoid : MonoBehaviour
{
    protected Rigidbody2D _rb;

    [SerializeField] 
    private float _moveSpeed;

    private bool _canMove = true;
    protected bool _isJumping;

    [SerializeField]
    private float _gravityScale;
    [SerializeField] 
    private float _fallGravityMultiplier;

    [SerializeField] 
    private float _jumpForce;
    protected float _lastGroundTime;
    [SerializeField] 
    private float _jumpCoyoteTime;
    

    //ground collision
    protected bool isGrounded = false;
    public Transform GroundCheck1;
    public LayerMask groundLayer;


    protected float movementX = 0;


    protected bool _canDoubleJump = false;// Can I doubleJump?
    public bool doubleJumpEnabled = true;// Do you have the ability?
    // Start is called before the first frame update
    public virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = _gravityScale;
    }

    public virtual void Update()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck1.position, 0.15f, groundLayer);

        Walk(new Vector2(movementX, 0));

        if (isGrounded && !_isJumping)
            _lastGroundTime = _jumpCoyoteTime;

        if (isGrounded)
        {
            _isJumping = false;
            _canDoubleJump = true;
        }

        _lastGroundTime -= Time.deltaTime;
    }

    private void Walk(Vector2 dir)
    {
        if (!_canMove)
            return;

        _rb.velocity = new Vector2(dir.x * _moveSpeed, _rb.velocity.y);
    }

    public virtual void FixedUpdate()
    {
        if(_rb.velocity.y < 0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallGravityMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
    protected void Jump(Vector2 direction){
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.velocity += direction * _jumpForce;
        _isJumping = true;
    }
}
