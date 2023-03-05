using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public abstract class Character : MonoBehaviour
{

    [Header ("Mvt variables")]

    [SerializeField] protected float speed = 1.0f;
    protected float direction;
    protected bool facingRight = true;

    [Header("Jump variables")]

    [SerializeField]protected float jumpForce;
    [SerializeField]protected float jumpTime;
    [SerializeField]protected float radOfCircle;
    [SerializeField]protected LayerMask groundMask;
    [SerializeField]protected bool grounded;
    [SerializeField]protected Transform groundCheck;
    protected float jumpTimeCounter;
    protected bool stoppedJumping;

    // [Header("Attack variables")]
    // [Header("Character stats")]

    protected Rigidbody2D rb;
    protected Animator myAnimator;

    //virtual becuase i need most of these in the childeren 
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        //setting player jump time, how long the player can jump for.
        jumpTimeCounter = jumpTime;
    }

    public virtual void Update()
    {
        //what it means to be grounded.
        grounded = Physics2D.OverlapCircle(groundCheck.position, radOfCircle, groundMask);

        //checking verticle velocity.
        if(rb.velocity.y < 0 )
        {
            myAnimator.SetBool("falling", true);

        }
    }

    public virtual void FixedUpdate()
    {
        //handle physics / mechanics
        HandleMovement();
    }

    protected void Jump()
    {
        //applying jump force to the rigidbody.
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    protected abstract void HandleJumping();

    //function for character movement adds a force in a direction to the rigidbody 
    protected void Move()
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }

    protected virtual void HandleMovement()
    {
        Move();
    }

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

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, radOfCircle);
    }
}
