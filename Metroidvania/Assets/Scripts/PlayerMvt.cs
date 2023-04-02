using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMvt : MonoBehaviour
{
    [Header("Movement Details")]
    [SerializeField] private float speed = 2.0f;
    private float direction;
    private bool facingRight = true;

    [Header("Jump Details")]
    [SerializeField]private float jumpForce = 5.0f;
    private bool stoppedJumping;

    [SerializeField]private float jumpsLeft;
    [SerializeField]private float maxJumps = 1;
    
    [Header("Ground Details")]
    [SerializeField]private float radOfCircle;
    [SerializeField]private LayerMask groundMask;
    [SerializeField]private bool grounded;
    [SerializeField]private Transform groundCheck;
    
    [Header("Rigidbody, Animator, CharacterController")]
    private Rigidbody2D rb; 
    private Animator myAnimator; 
    


    //getting the rigid body and animator components.
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        jumpsLeft = maxJumps;  

    }

    //method that repeats every frame used to check if the player is touching the ground and is facing the right direction.
    //handles the movement of the player.
    private void Update()
    {
        //if the player is on the ground the falling anim will be false. 
        if (IsGrounded())
        {
            myAnimator.SetBool("falling", false); 

            jumpsLeft = maxJumps;
            
        }
        //making sure the player is facing the correct direction.
        if (!facingRight && direction > 0f)
        {
            Flip();
        }
        //making sure the player is facing the correct direction.
        else if(facingRight && direction < 0f)
        {
            Flip();
        }
        //controlling the movement of the player.
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }

    private void FixedUpdate()
    {
        
    }

    //using the new input system to control the player jump.
    public void Jump(InputAction.CallbackContext context)
    {
        //when jump is pressed and jumps left is more than 0.
        if (context.performed && jumpsLeft > 0)
        {
            
            //when the player is grounded and jump is pressed add a virticle velocity to the rigid body (the player).
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            myAnimator.SetTrigger("jump");
            myAnimator.SetBool("falling", false);            

            //taking away 1 jump from jumps left.
            jumpsLeft -= 1;

        } 
        //when jump is canceled.
        else if (context.canceled && rb.velocity.y > 0f)
        {
            //allowing the player to jump higher by pressing jump for longer and lower by pressing it for a short time.
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            myAnimator.SetBool("falling", true);
            myAnimator.ResetTrigger("jump");
        }
        else if(rb.velocity.y < 0 )
        {
            myAnimator.SetBool("falling", true);
            myAnimator.ResetTrigger("jump");
        }

    }

    //finding the direction the player is trying to move
    public void Move(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>().x;
        myAnimator.SetFloat("speed", Mathf.Abs(direction));
        
    }
    //bool to check if the player is on the ground. returns true or false.
    private bool IsGrounded()
    {
        //drawing a small circle under the rigid body to check if its touching the ground mask. if it is return true. if not return false.
        return Physics2D.OverlapCircle(groundCheck.position, radOfCircle, groundMask);
        
        
    }

    //method used to change the direction a rigid body is facing 
    private void Flip()
    {
            facingRight = !facingRight;
            
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
    }

    //used to draw a gizmo that is visible to the editor but not in game.
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, radOfCircle);
    }
    
}
