using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : Player
{   
    //reference variable
    PlayerInput playerInput;
    public Rigidbody2D rb;
    Animator animator;

    // variables to store optimized setter/getter peram ID's for example with animations.
    int isRunningHash;
    int isIdleHash;
    int speedHash;
    int isJumpingHash;



    //gravity variables
    float gravity = -9.8f;
    float groundedGravity = -.05f;



    //variables to store player input
    Vector2 currentDirectionInput;
    bool isMovementPressed;



    //Jump variables
    bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 10.0f;
    float maxJumpTime = 1f;
    bool isJumping = false;
    bool isJumpAnimating = false;



    //Grounded variables
    private float radOfCircle = 0.03f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;



    void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        speedHash = Animator.StringToHash("speed");
        isRunningHash = Animator.StringToHash("isRunning");
        isIdleHash = Animator.StringToHash("isIdle");
        isJumpingHash = Animator.StringToHash("isJumping");
        
        playerInput.Player.Move.started += OnMovementInput;
        playerInput.Player.Move.canceled += OnMovementInput;
        playerInput.Player.Move.performed += OnMovementInput;
        playerInput.Player.Jump.started += OnJump;
        playerInput.Player.Jump.canceled += OnJump;

        SetupJumpVariables();
    }


    void HandleJump()
    {
        if(!isJumping && IsGrounded() && isJumpPressed)
        {
            
            isJumpAnimating = true;
            isJumping = true;
            rb.linearVelocity = new(rb.linearVelocityX, initialJumpVelocity * .5f);
        } else if(!isJumpPressed && isJumping && IsGrounded())
        {
            isJumping = false;        
        }
    }

    void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    public override void Start()
    {
        base.Start();
        
    }


    public override void FixedUpdate() 
    {
        base.FixedUpdate();
        HandleAnimation();

        //if dashpresseed do dash else move
        rb.linearVelocity = new Vector2(currentDirection.x * speed, rb.linearVelocityY);
        HandleGravity();
        HandleJump();
    }



    void OnEnable()
    {
        playerInput.Player.Enable();
    }
    void OnDisable()
    {
        playerInput.Player.Disable();
    }
#region Actions

    void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        //Debug.Log(isJumpPressed);
    }
    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentDirectionInput = context.ReadValue<Vector2>();
        currentDirection.x = currentDirectionInput.x;
        // currentDirection.y = currentDirectionInput.y;

        isMovementPressed = currentDirectionInput.x != 0;    
    }

#endregion

    void HandleGravity()
    {
        bool isFalling = rb.linearVelocityY <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2.0f ;
        //apply proper gravity if the player is grounded or not 
        if(IsGrounded())
        {
            if(isJumpAnimating)
            {
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
            }
            rb.linearVelocityY = groundedGravity;
        } else if (isFalling)
        {
            float previousYVelocity = rb.linearVelocityY;
            float newYVelocity = rb.linearVelocityY + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * .5f, -20.0f );
            rb.linearVelocityY = nextYVelocity;
        } else
        {
            float previousYVelocity = rb.linearVelocityY;
            float newYVelocity = rb.linearVelocityY + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f ;
            rb.linearVelocityY = nextYVelocity;
        }
    }

    

    void HandleAnimation()
    {
        //getting params from animator
        bool isRunning = animator.GetBool(isRunningHash);
        float speed = animator.GetFloat(speedHash);
        bool isIdle = animator.GetBool(isIdleHash);
        bool isJump = animator.GetBool(isJumpingHash);
        

        if (!IsGrounded())
        {

            if (isJumping && !isJump)
            {
                animator.SetBool(isJumpingHash, true);
            } else 
            {
                animator.SetBool(isJumpingHash, false);
            }
        } else
        {
            animator.SetBool(isJumpingHash, false);
            //start walking if movement is pressedd and not already walking.
            if(isMovementPressed && isJumping && isRunning)
            {
                animator.SetBool(isJumpingHash, true);
            }
            else if(isMovementPressed && !isRunning)
            {
                //animator.SetFloat(speedHash, Mathf.Abs(currentDirection.x));
                animator.SetBool(isRunningHash, true);
            }
            //stop walking if moovemetn is not pressed and not already walking.
            else if (!isMovementPressed && isRunning)
            {
                //animator.SetFloat(speedHash, Mathf.Abs(currentDirection.x));
                
                animator.SetBool(isRunningHash, false);
            }

        }
        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, radOfCircle);
    }
    private bool IsGrounded()
    {
        //drawing a small circle under the rigid body to check if its touching the ground mask. if it is return true. if not return false.
        return Physics2D.OverlapCircle(groundCheck.position, radOfCircle, groundMask);
    }

}
