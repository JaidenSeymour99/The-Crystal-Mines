using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerScript : MonoBehaviour
{
    [Header("Movement Details")]
    [SerializeField] private float maxSpeed = 8.0f;
    [SerializeField]private float speed;
    private float direction;
    private bool facingRight = true;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    [Header("Jump Details")]
    [SerializeField]private float jumpForce = 10.0f;
    

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private bool isJumping;
    private bool isFalling; 

    [SerializeField]private bool isWallJumping;
    [SerializeField]private float wallJumpingDirection;
    private float wallJumpingTimeMax = 0.2f;
    [SerializeField]private float wallJumpingTime;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 1f;
    [SerializeField]private Vector2 wallJumpingPower = new Vector2(3f,7f);
 
    [SerializeField]private float jumps;
    [SerializeField]private float maxJumps = 2f;
    
    [Header("Ground Details")]
    [SerializeField]private float radOfCircle;
    [SerializeField]private bool grounded;
    [SerializeField]private LayerMask groundMask;
    [SerializeField]private Transform groundCheck;

    [Header("Wall Details")]
    [SerializeField]private Transform wallCheck;
    [SerializeField]private LayerMask wallLayer;

    [Header("Dash Details")]
    [SerializeField]private TrailRenderer tr;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 14f;
    private float dashingTime = .2f;
    private float dashingCooldown = 1f;
    
    [Header("Attack Details")]
    [SerializeField]private float attackRange;
    [SerializeField]private Transform attackPoint;
    [SerializeField]private LayerMask enemyLayers;
    private float attackDamage = 40f;
    private bool attacking;
    private float chainAttackTime = 0.7f;
    private float attackRate = 2f;
    private float nextAttackTime = 0f;


    [Header("Rigidbody, Animator")]
    private Rigidbody2D rb; 
    private Animator myAnimator; 

    


    //getting the rigid body and animator components.
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        wallJumpingCounter = wallJumpingDuration;
        maxJumps = 2f;  
        speed = maxSpeed;
        attacking = false;
    }

    //method that repeats every frame used to check if the player is touching the ground and is facing the right direction.
    //handles the movement of the player.
    private void Update()
    {
        if(isDashing)
            return;

        if(rb.velocity.y > 0.1f)
        {
            isJumping = true;
        }
        else 
        {
            isJumping = false;
        }

        if(rb.velocity.y < -0.1f)
        {
            isFalling = true;

        }
        else 
        {
            isFalling = false;
        }

        
        //if the player is on the ground the falling anim will be false. 
        if (IsGrounded())
        {
            jumps = 0f;
            coyoteTimeCounter = coyoteTime;
            wallJumpingDirection = 0f;
        }
        else 
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (IsWalled())
        {
            wallJumpingTime = wallJumpingTimeMax;
            wallJumpingDirection = -direction;
        }
        else 
        {   
            wallJumpingTime -= Time.deltaTime;
            if (wallJumpingTime < 0f) isWallJumping = false;
        }
        //when the player is falling play anims
        
        //making sure the player is facing the correct direction.
        if(!isWallJumping)
        {
            IsMoving();
            WallSlide();
            IsFalling();
            IsWalling();
            IsJumping();
            ChangeDirection();
            
        }
        else if (isWallJumping)
        {
            IsJumping();
            IsMoving();
            IsFalling();

        }

        
    }

    private void FixedUpdate()
    {
        if(isDashing)
            return;

        if(!isWallJumping)
        {
            //controlling the movement of the player, changing the x velocity.
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
            
        }
        else if (wallJumpingTime <= 0f)
        {
            isWallJumping = false;
            //rb.velocity = new Vector2(rb.velocity.x * 0.5f, rb.velocity.y * 0.5f);
        }
    }

    //using the new input system to control the player jump.
    public void Jump(InputAction.CallbackContext context)
    {

        //Jump from the wall
        if (context.performed && wallJumpingTime > 0f && isWallSliding)
        {
            speed = 0f;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            StartCoroutine(WallBounce());
        }
        //when jump is pressed and jumps left is less than max jumps.
        //Normal jump without wall
        //using a jump after the player leaves the wall
        else if (context.performed && maxJumps > jumps && !isWallSliding)
        {
            
            isJumping = true;
            //adds a velocity (jump force) to the y value of the rigid body.
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
            //the coyote timer makes it so there is some leaniency with jumping if you have just left the ground and the jump button is pressed the player will still jump. 
            //taking away 1 jump from jumps left.   
            if(coyoteTimeCounter <= 0f)
            {
                jumps += 1f; 
            }
                        
        }
        
        //when jump is canceled.
        else if ((context.canceled && rb.velocity.y > 0f) && (wallJumpingTime < 0f))
        {
            isJumping = false;
            isWallJumping = false;
            wallJumpingDirection = 0f;
            //allowing the player to jump higher by pressing jump for longer and lower by pressing it for a short time.
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;

        }
        else if (context.canceled) 
        {
            wallJumpingDirection = 0f;
            isJumping = false;
            isWallJumping = false;
            coyoteTimeCounter = 0f;
            wallJumpingTime = 0f;
        }

    }

    public void Dash(InputAction.CallbackContext context)
    {
        if(context.performed && canDash)
        {
            StartCoroutine(Dash());
        }
    }
    
    //finding the direction the player is trying to move
    public void Move(InputAction.CallbackContext context)
    {
        
        if(!isWallJumping)
        {
            //when the player moves.
            if(context.performed && IsWalled())
            {
                direction = context.ReadValue<Vector2>().x;     
                isWallSliding = true;
            }
            if(context.performed)
            {
                direction = context.ReadValue<Vector2>().x;
                
            }
            //when the player cancels their movement.
            else if(context.canceled)
            {
                direction = context.ReadValue<Vector2>().x;
                
            }    

        }
        else if (context.performed && isWallJumping)
        {
            direction = context.ReadValue<Vector2>().x;
        }

    }
    public void Fire(InputAction.CallbackContext context)
    {
        if(Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    private void Attack()
    {
        //play attack anim
        myAnimator.SetTrigger("attack1");
        

        //detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        

        //damage them
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyScript>().TakeDamage(attackDamage);
            
        }

    }

    private void WallSlide()
    {
        if(IsWalled() && !IsGrounded() && (direction > 0f || direction < 0f)) 
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            
        }
        else
        {
            isWallSliding = false;

        }
    }



    IEnumerator WallBounce()
    {
        isWallJumping = true;       
        yield return new WaitForSeconds(.4f);
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        isWallJumping = false;
        yield return null;
        
    }
    private void IsMoving()
    {
        myAnimator.SetFloat("speed", Mathf.Abs(direction));
        
    }

    IEnumerator Dash()
    {
        //when dash() is run the original gravity of the player is stored, the players gravity is 0 so they are not affected by gravity while they dash
        //the players velocity is changed (dashing power), and the trail starts emitting
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        //waits until the end of the dash.
        yield return new WaitForSeconds(dashingTime);
        //turns off the trail, resets the gravity scale.
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        //wait for the cool down before being able to dash again.
        yield return new WaitForSeconds(dashingCooldown);
        
        canDash = true;


    }

    private void IsAttacking()
    {
        if(attacking)
        {
            myAnimator.SetTrigger("attack2");
        }
        else 
        {
            myAnimator.SetTrigger("attack1");
        }
    }

    private void IsWalling()
    {
        if (isWallSliding && isJumping)
        {
            myAnimator.SetTrigger("jump");
            myAnimator.SetBool("walled", false);
        }
        else if (isWallSliding)
        {
            myAnimator.SetBool("walled", true);

        }
        else
        {
            myAnimator.SetBool("walled", false);
        }
    }
    
    private void IsJumping()
    {
        if (isJumping || isWallJumping)
        {
            myAnimator.SetTrigger("jump");
            myAnimator.SetBool("walled", false);
        }
        else if (isJumping)
        {
            myAnimator.SetTrigger("jump");

        }
        else 
        {
            speed = maxSpeed;
            myAnimator.ResetTrigger("jump");
        }
    }
    private void IsFalling()
    {
        if (isFalling && isWallSliding)
        {
            myAnimator.SetBool("walled", true);
            myAnimator.SetBool("falling", true);
        }
        else if (isFalling)
        {
            myAnimator.SetBool("falling", true);
        }
        else
        {
            myAnimator.SetBool("falling", false);
        }
    }

    private void ChangeDirection()
    {
        if (!facingRight && direction > 0f)
        {
            Flip();
        }
        //making sure the player is facing the correct direction.
        else if(facingRight && direction < 0f)
        {
            Flip();
        }
    }
    //method used to change the direction a rigid body is facing 
    private void Flip()
    {
            facingRight = !facingRight;
            
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
    }
    //bool to check if the player is on the ground. returns true or false.
    private bool IsGrounded()
    {
        //drawing a small circle under the rigid body to check if its touching the ground mask. if it is return true. if not return false.
        return Physics2D.OverlapCircle(groundCheck.position, radOfCircle, groundMask);
    }

    //bool. checking for player / wall overlap. returns true or false.
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, radOfCircle, wallLayer);
    }

    //used to draw a gizmo that is visible to the editor but not in game.
    private void OnDrawGizmos()
    {
        
        Gizmos.DrawSphere(groundCheck.position, radOfCircle);
        Gizmos.DrawSphere(wallCheck.position, radOfCircle);
        if(attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    
    

}
