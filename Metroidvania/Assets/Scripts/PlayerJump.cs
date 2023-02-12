using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump Details")]
    [SerializeField]private float jumpForce;
    [SerializeField]private float jumpTime;
    private float jumpTimeCounter;
    private bool stoppedJumping;

    
    [Header("Ground Details")]
    [SerializeField]private float radOfCircle;
    [SerializeField]private LayerMask groundMask;
    [SerializeField]private bool grounded;
    [SerializeField]private Transform groundCheck;
    
    [Header("Ground Details")]
    private Rigidbody2D rb; 
    private Animator myAnimator; 



    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        jumpTimeCounter = jumpTime;

    }

    // Update is called once per frame
    private void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, radOfCircle, groundMask);


        if(grounded)
        {
            jumpTimeCounter = jumpTime;
            myAnimator.ResetTrigger("jump");
            myAnimator.SetBool("falling", false);
        }
        //press space to jump. if the player is grounded(touching the ground) the player will jump. ( press jump )
        if(Input.GetButtonDown("Jump") && grounded)
        {   
            //applying a force to the player rigidbody.
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            //sets bool for jumping or grounded
            stoppedJumping = false;
            //playing jump anim
            myAnimator.SetTrigger("jump");
        }
        //stay jumping while butting is pressed. ( hold jump )
        if(Input.GetButton("Jump") && !stoppedJumping && (jumpTimeCounter > 0))
        {
            //applying a force to the player rigidbody.
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
            myAnimator.SetTrigger("jump");
            
        }
        if(Input.GetButtonUp("Jump"))
        {
            jumpTimeCounter = 0;
            stoppedJumping = true;
            //playing falling anim
            myAnimator.SetBool("falling", true);
            myAnimator.ResetTrigger("jump");
        }
        if(rb.velocity.y < 0 )
        {
            myAnimator.SetBool("falling", true);

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, radOfCircle);
    }
    private void FixedUpdate()
    {
        HandleLayers();
    }
    private void HandleLayers()
    {
        if(!grounded)
        {
            myAnimator.SetLayerWeight(1,1);
        } else 
        {
            myAnimator.SetLayerWeight(1,0);
        }
    }
    
}
