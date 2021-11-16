using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Humanoid
{
    public float JumpPower = 25;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }
    public override void FixedUpdate(){
        Inputs();
        if (Input.GetButtonDown("Jump") && isGrounded){
            Jump();
        }
        base.FixedUpdate();
    }
    private void Inputs(){
        //reads all the player inputs here
        movementX = Input.GetAxis("Horizontal");
        
    }
    private void Jump(){
        //jump code.
        Debug.Log("Jump");
        rb.velocity = new Vector2(rb.velocity.x,JumpPower);
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
