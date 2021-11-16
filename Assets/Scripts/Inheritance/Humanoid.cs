using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
for any human like figure will inherit this script.
*/
public class Humanoid : MonoBehaviour
{
    private float friction = 0.000083f;// 0.000001 to 1 
    [HideInInspector]
    public Rigidbody2D rb;
    public float HorizontalSpeed = 100;

    //ground collision
    [HideInInspector]
     public bool isGrounded = false;
     public Transform GroundCheck1;
     public LayerMask groundLayer;

    [HideInInspector]
    public float movementX = 0;
    [HideInInspector]
    public bool jumpButton = false;
     [HideInInspector]
    public bool jumpButtonPressed = false;

    [HideInInspector]
    public float JumpPower = 25;


    private bool canDoubleJump = false;// Can I doubleJump?
    public bool doubleJumpEnabled = true;// Do you have the ability?
    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public virtual void FixedUpdate()
    {
        rb.velocity += new Vector2(movementX * HorizontalSpeed * Time.fixedDeltaTime,0);
        rb.velocity = new Vector2(rb.velocity.x * Mathf.Pow(friction,Time.fixedDeltaTime),rb.velocity.y);
        isGrounded = Physics2D.OverlapCircle(GroundCheck1.position, 0.15f, groundLayer);
        if (jumpButton && jumpButtonPressed && isGrounded){
            Jump();
        }
        if (jumpButton && jumpButtonPressed && !isGrounded && doubleJumpEnabled && canDoubleJump){
            DoubleJump();
        }
        if (isGrounded){
            //allow the ability to double jump again.
            canDoubleJump = true;
        }
    }
    private void Jump(){
        rb.velocity = new Vector2(rb.velocity.x,JumpPower);
    }
    private void DoubleJump(){
        rb.velocity = new Vector2(rb.velocity.x,JumpPower);
        canDoubleJump = false;
    }
    // Update is called once per frame
    public virtual void Update()
    {
        
    }
}
