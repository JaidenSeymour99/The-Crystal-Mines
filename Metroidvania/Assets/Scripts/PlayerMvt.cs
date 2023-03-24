using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMvt : MonoBehaviour
{
    [Header("Movement Details")]
    [SerializeField] private float speed = 2.0f;
    private float direction;
    private bool facingRight = true;

    [Header("Jump Details")]
    [SerializeField]private float jumpForce = 5.0f;
    private bool stoppedJumping;

    
    [Header("Ground Details")]
    [SerializeField]private float radOfCircle;
    [SerializeField]private LayerMask groundMask;
    [SerializeField]private bool grounded;
    [SerializeField]private Transform groundCheck;
    
    [Header("Rigidbody and Animator")]
    private Rigidbody2D rb; 
    private Animator myAnimator; 



    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        

    }

    // Update is called once per frame
    private void Update()
    {
        
        if (IsGrounded())
        {
            myAnimator.SetBool("falling", false); 
        }
        if (!facingRight && direction > 0f)
        {
            Flip();
        }
        else if(facingRight && direction < 0f)
        {
            Flip();
        }

        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }

    private void FixedUpdate()
    {

    }

    public void Jump(InputAction.CallbackContext context)
    {

        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            myAnimator.SetTrigger("jump");
            myAnimator.SetBool("falling", false);            
        } 
        else if (context.canceled && rb.velocity.y > 0f)
        {
           
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

    public void Move(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>().x;
        myAnimator.SetFloat("speed", Mathf.Abs(direction));
        
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, radOfCircle, groundMask);
    }
    private void Flip()
    {
            facingRight = !facingRight;
            
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, radOfCircle);
    }
    
}
