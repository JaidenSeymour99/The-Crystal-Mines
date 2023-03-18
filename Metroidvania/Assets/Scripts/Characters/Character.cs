using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//required components. won't run program without them being assigned.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

//dont want to implement a base character so abstract class, want other things to be able to use this freely.
public abstract class Character : MonoBehaviour
{
    //movement variables header
    [Header ("Mvt variables")]
    //variable for speed of a rigidbody
    [SerializeField] protected float speed = 1.0f;
    //variable for direction of a rigidbody
    protected float direction;
    //variable to check which direction the character is in to make animations face the correct direction.
    protected bool facingRight = true;

    
    

    //Jump variables header.
    [Header("Jump variables")]

    //variable for the power of a jump
    [SerializeField]protected float jumpForce;
    //variable to set how long a character can jump for.
    [SerializeField]protected float jumpTime;
    //variable used when drawing a gizmo to check if the character is on the ground.
    [SerializeField]protected float radOfCircle;
    //layer mask that will cover the ground, can check if a character touches the mask 
    [SerializeField]protected LayerMask groundMask;
    //variable to check if a character is on the ground.
    [SerializeField]protected bool grounded;
    //transform used to check if a character is touching the ground mask.
    [SerializeField]protected Transform groundCheck;

    protected float jumpTimeCounter;
    //used to let a character decide how high they can jump
    protected bool stoppedJumping;

    // [Header("Attack variables")]
    // [Header("Character stats")]

    //rigidbody
    protected Rigidbody2D rb;
    //animator
    protected Animator myAnimator;

    //getting the rigidbody and animator on the character, 
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        //setting player jump time, how long the player can jump for.
        jumpTimeCounter = jumpTime;
    }

    public virtual void Update()
    {
        //checking if grounded
        IsGrounded();

        //checking verticle velocity. and if falling play the animation.
        if(rb.velocity.y < 0 )
        {
            myAnimator.SetBool("falling", true);
        }
    }

    public virtual void FixedUpdate()
    {
        //handle physics / mechanics
        //Move();
    }

    protected bool IsGrounded()
    {
        return grounded = Physics2D.OverlapCircle(groundCheck.position, radOfCircle, groundMask);
    }

    protected void Jump()
    {
        //applying jump force to the rigidbody.
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    //abstract function because jump would be based off of when the character wants to jump. this will mainly be for the player, the enemies will be able to jump but will know when to jump.
    // protected abstract void HandleJumping();


    // public void Move(InputAction.CallbackContext context)
    // {
        
    // }



    //function to have the animation flip so that it turns the other direction.
    protected void TurnAround(float horizontal) 
    {
        if (horizontal < 0 && facingRight || horizontal > 0 && !facingRight)
        {
            //
            facingRight = !facingRight;
            
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
    //drawing a circle to be used to check for overlap between the ground and a character. 
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, radOfCircle);
    }
}
