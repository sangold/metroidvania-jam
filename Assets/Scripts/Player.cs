using UnityEngine;

public class Player : Humanoid
{

    private bool _jumpButtonPressed;
    [SerializeField]
    private float _jumpCutGravityMultiplier;

    public override void Start()
    {
        base.Start();
    }
    public override void FixedUpdate(){
        base.FixedUpdate();
        if (_rb.velocity.y > 0 && !_jumpButtonPressed)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_jumpCutGravityMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
    private void Inputs(){
        //reads all the player inputs here
        /*
        Unity Input only updates in update not fixed update. 
        So we have to make our own button pressed so the button press can work.
        */
        movementX = Input.GetAxis("Horizontal");
        if(Input.GetButtonDown("Jump"))
        {
            _jumpButtonPressed = true;
            if (_lastGroundTime > 0f)
                Jump(Vector2.up);
            else if (_canDoubleJump)
            {
                Jump(Vector2.up);
                _canDoubleJump = false;
            }
            
        }
        if(Input.GetButtonUp("Jump"))
        {
            _jumpButtonPressed = false;
        }
        
    }
    // Update is called once per frame
    public override void Update()
    {
        Inputs();
        base.Update();
    }
}
