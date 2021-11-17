using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Humanoid
{
    public override void Start()
    {
        base.Start();
    }
    public override void FixedUpdate(){
        base.FixedUpdate();
    }
    private void Inputs(){
        //reads all the player inputs here
        /*
        Unity Input only updates in update not fixed update. 
        So we have to make our own button pressed so the button press can work.
        */
        movementX = Input.GetAxis("Horizontal");
        Walk(new Vector2(movementX, 0));
        if(Input.GetButtonDown("Jump") && _lastGroundTime > 0f)
        {
            jumpButtonPressed = true;
            Jump(Vector2.up);
            _lastJumpTime = .1f;
        }
        if(Input.GetButtonUp("Jump"))
        {
            jumpButtonPressed = false;
            _lastJumpTime = 0;
        }
        
    }
    // Update is called once per frame
    public override void Update()
    {
        Inputs();
        base.Update();
    }
}
