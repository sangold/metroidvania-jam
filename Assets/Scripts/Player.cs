using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Humanoid
{
    [SerializeField]
    private float _jumpShortMultiplier;
    // Start is called before the first frame update
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
        if(Input.GetButton("Jump"))
        {
            Debug.Log("Pressed");

            jumpButtonPressed = true;
            _lastJumpTime = .1f;
        }
        if(Input.GetButtonUp("Jump"))
        {
            Debug.Log("Release");
            jumpButtonPressed = false;
            if(_rb.velocity.y > 0 && _isJumping)
            {
                _rb.AddForce(Vector2.down * _rb.velocity.y * (1 - _jumpShortMultiplier), ForceMode2D.Impulse);
            }

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
