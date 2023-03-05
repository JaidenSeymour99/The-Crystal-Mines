using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private float runSpeed = 7.0f;

    public override void Start()
    {
        base.Start();
        speed = runSpeed;
    }

    //using pverrode to add things to update() that will take parts from character and add parts that only player can use 
    public override void Update()
    {
        base.Update();
        //making the inputs only controll the player not other characters.
        HandleJumping();
        direction = Input.GetAxisRaw("Horizontal");
        
    }

    protected override void HandleMovement()
    {
        
        base.HandleMovement();
        myAnimator.SetFloat("speed", Mathf.Abs(direction));
        TurnAround(direction);
    }

    protected override void HandleJumping()
    {
        //checking if the player is on the ground.
        if(grounded)
        {
            jumpTimeCounter = jumpTime;
            myAnimator.ResetTrigger("jump");
            myAnimator.SetBool("falling", false);
        }
        //press space to jump. if the player is grounded(touching the ground) the player will jump.
        if(Input.GetButtonDown("Jump") && grounded)
        {   

            //Making the player jump
            Jump();
            //sets bool for jumping or grounded
            stoppedJumping = false;
            //playing jump anim
            myAnimator.SetTrigger("jump");
        }
        //stay jumping longer while button is pressed. ( hold jump )
        if(Input.GetButton("Jump") && !stoppedJumping && (jumpTimeCounter > 0))
        {
            //Making the player jump 
            Jump();
            jumpTimeCounter -= Time.deltaTime;
            myAnimator.SetTrigger("jump");
            
        }
        //when let go of jump the player will fall.
        if(Input.GetButtonUp("Jump"))
        {
            jumpTimeCounter = 0;
            stoppedJumping = true;
            //playing falling anim
            myAnimator.SetBool("falling", true);
            myAnimator.ResetTrigger("jump");
        }
    }



}
