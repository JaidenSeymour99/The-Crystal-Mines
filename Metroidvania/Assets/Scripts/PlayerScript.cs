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
    public bool facingRight = true;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    [Header("Jump Details")]
    [SerializeField]private float jumpForce = 10.0f;
    

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private bool isJumping;
    private bool isFalling; 

 
    [SerializeField]private float jumps;
    [SerializeField]private float maxJumps = 2f;

    private float originalGravity;
    private float newGravity;

    [Header("One Way Platform")]
    public bool fallThrough;
    
    [Header("Ground Details")]
    [SerializeField]private float radOfCircle;
    [SerializeField]private bool grounded;
    [SerializeField]private LayerMask groundMask;
    [SerializeField]private Transform groundCheck;

    [Header("Wall Details")]
    [SerializeField]private Transform wallCheck;
    [SerializeField]private LayerMask wallLayer;
    
    [Header("Wall Jump Details")]
    [SerializeField]private float wallJumpingDirection;
    [SerializeField]private Vector2 wallJumpingPower = new Vector2(9f,9f);
    [SerializeField]private float wallJumpingTime;
    private float maxWallJumpingTime = .1f;
    [SerializeField]private bool canWallJump = true;
    [SerializeField]private bool isWallJumping;
    [SerializeField]private float wallJumpingCooldown = .1f;

    [Header("Dash Details")]
    [SerializeField]private TrailRenderer tr;
    [SerializeField]private float dashingPower = 40f;
    [SerializeField]private float dashingTime = .1f;
    private bool canDash = true;
    
    private bool isDashing;
    private float dashingCooldown = 1f;
    
    [Header("Attack Details")]
    [SerializeField]private float attackRange = .8f;
    [SerializeField]private Transform attackPoint;
    [SerializeField]private LayerMask enemyLayers;
    private float attackDamage = 40f;
    private bool attacking;
    //private float chainAttackTime = 0.7f;
    private float attackRate = 2f;
    private float nextAttackTime = 0f;


    [Header("Rigidbody, Animator")]
    private Rigidbody2D rb; 
    private Animator myAnimator; 

    [Header("Interactables")]
    [SerializeField]float interactRange = 3f;
    [SerializeField] private LayerMask NPCLayers;
    private NPC NPCdeets;

    [Header("Camera")]
    [SerializeField] private GameObject cameraFollow;
    private CameraFollowObject cameraFollowObject;

    //getting the rigid body and animator components.
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        NPCdeets = GetComponent<NPC>();

        originalGravity = rb.gravityScale;
        
        wallJumpingTime = maxWallJumpingTime;
        maxJumps = 2f;
        speed = maxSpeed;
        attacking = false;

        cameraFollowObject = cameraFollow.GetComponent<CameraFollowObject>();
    }

    //method that repeats every frame used to check if the player is touching the ground and is facing the right direction.
    //handles the movement of the player.
    private void Update()
    {
        if(PauseScript.isPaused) return;
        if(isDashing || isWallJumping)
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

        //dealing with platforms that you can stand on but also choose to jump down from.
        //when the down arrow key is pressed the bool fall through is set to true and lets the player fall. 
        if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
        {
            
            fallThrough = true;
        }
        else
        {
            fallThrough = false;
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
            
            
            wallJumpingDirection = -direction;
            wallJumpingTime = maxWallJumpingTime;
        
        }
        else 
        {
            wallJumpingTime -= Time.deltaTime;
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
        if(PauseScript.isPaused) return;
        if(isDashing || isWallJumping) return;
        //controlling the movement of the player, changing the x velocity.
        if(IsWalled()) StartCoroutine(WallJumpCooldown());
        if(direction > 0 || direction < 0) ChangeDirection();
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

    }

    //using the new input system to control the player jump.
    public void Jump(InputAction.CallbackContext context)
    {
        if(PauseScript.isPaused) return;
        //Jump from the wall
        if (context.performed && canWallJump)
        {
            
            StartCoroutine(WallBounce());
        }
        //when jump is pressed and jumps left is less than max jumps.
        //Normal jump without wall
        //using a jump after the player leaves the wall
        else if (context.performed && maxJumps > jumps && !isWallSliding)
        {
            AudioManager.instance.JumpSFX();
            isJumping = true;
            //adds a velocity (jump force) to the y value of the rigid body.
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
            //the coyote timer makes it so there is some leaniency with jumping if you have just left the ground and the jump button is pressed the player will still jump. 
            if(coyoteTimeCounter <= 0f)
            {
            //addingg a jump to the jumps used.   
                jumps += 1f; 
            }
                        
        }
        
        //when jump is canceled.
        else if ((context.canceled && rb.velocity.y > 0f) && (!canWallJump))
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
            
        }

    }

    public void Interact(InputAction.CallbackContext context)
    {
        
        if(context.performed)
        {
            CheckInteractions();
        }

         
    }

    public void Dash(InputAction.CallbackContext context)
    {
        
        if(context.performed && canDash && !PauseScript.isPaused)
        {
            StartCoroutine(Dash());
        }
    }
    
    //finding the direction the player is trying to move
    public void Move(InputAction.CallbackContext context)
    {

        if(IsWalled() && !isWallJumping)
        {
            //when the player moves.
            if(context.performed)
            {
                direction = context.ReadValue<Vector2>().x;     
            }
            else if(context.canceled)
            {
                direction = context.ReadValue<Vector2>().x;
            }    

        }
        else if (context.performed)
        {
            direction = context.ReadValue<Vector2>().x;
        }
        else if (context.canceled)
        {
            direction = context.ReadValue<Vector2>().x;
        }

    }
    public void Fire(InputAction.CallbackContext context)
    {
        if(Time.time >= nextAttackTime && context.performed)
        {
            Attack();
            AudioManager.instance.AttackSFX();
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

    private void CheckInteractions()
    {
            
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, interactRange, NPCLayers);
        foreach (Collider2D collider in colliderArray) 
        {
            if(collider.TryGetComponent(out NPCInteractable npcInteractable))
            {
                npcInteractable.Interact();
            }

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



    private void IsMoving()
    {
        myAnimator.SetFloat("speed", Mathf.Abs(direction));
        
    }
    
    IEnumerator WallJumpCooldown()
    {
        if (!canWallJump)
        {
            yield return new WaitForSeconds(wallJumpingCooldown);
            canWallJump = true;
        }
        
    }

    IEnumerator WallBounce()
    {
        
        canWallJump = false;
        isWallJumping = true;
        myAnimator.SetTrigger("jump");
        myAnimator.SetBool("walled", false);
        newGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);   
        //tr.emitting = true;

        yield return new WaitForSeconds(wallJumpingTime);
        
        //tr.emitting = false;
        rb.gravityScale = originalGravity;
        isWallJumping = false;
        myAnimator.ResetTrigger("jump");


    }


    IEnumerator Dash()
    {
        //when dash() is run the original gravity of the player is stored, the players gravity is 0 so they are not affected by gravity while they dash
        //the players velocity is changed (dashing power), and the trail starts emitting
        canDash = false;
        isDashing = true;
        myAnimator.SetTrigger("dashing");
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(dashingPower, 0f);
        tr.emitting = true;

        //waits until the end of the dash.
        yield return new WaitForSeconds(dashingTime);

        //turns off the trail, resets the gravity scale.
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        myAnimator.ResetTrigger("dashing");

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
        if(!facingRight && direction > 0)
        {
            Flip();
        }
        else if (facingRight && direction < 0)
        {
            Flip();
        }
    }
    //method used to change the direction a rigid body is facing 
    private void Flip()
    {
            //OLD VERSION OF CHANGING DIRECTION
            //this made the camera not work correctly.

            // facingRight = !facingRight;
            
            // Vector3 theScale = transform.localScale;
            // theScale.x *= -1;
            // transform.localScale = theScale;
            // cameraFollowObject.Turn();

            if(facingRight)
            {
                Vector2 rotator = new Vector2(transform.rotation.x, 180f);
                transform.rotation = Quaternion.Euler(rotator);
                facingRight = !facingRight;
                dashingPower *= -1f;
            }
            else
            {
                Vector2 rotator = new Vector2(transform.rotation.x, 0f);
                transform.rotation = Quaternion.Euler(rotator);
                facingRight = !facingRight;
                dashingPower *= -1f;
            }

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
        Gizmos.DrawWireSphere(transform.position, interactRange);
        Gizmos.DrawSphere(wallCheck.position, radOfCircle);
        if(attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    
    

}
