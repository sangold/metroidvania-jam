using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Humanoid
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }
    public override void FixedUpdate(){
        Inputs();
        base.FixedUpdate();
    }
    private void Inputs(){
        //reads all the player inputs here
        /*
        Unity Input only updates in update not fixed update. 
        So we have to make our own button pressed so the button press can work.
        */
        movementX = Input.GetAxis("Horizontal");
        if (jumpButton != Input.GetButton("Jump")){
            jumpButtonPressed = true;
            jumpButton = Input.GetButton("Jump");
        } else {
            jumpButtonPressed = false;
            jumpButton = Input.GetButton("Jump");
        }
        
    }
    // Update is called once per frame
    public override void Update()
    {
        
        base.Update();
    }
}
