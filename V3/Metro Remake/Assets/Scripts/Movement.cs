using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : Player
{   
    //reference variable
    PlayerInput playerInput;

    //variables to store player input
    Vector2 currentMovementInput;
    //Vector2 currentMovement;
    bool isMovementPressed;


    public Rigidbody2D rb;
    Animator animator;

    int isRunningHash;
    int isIdleHash;
    int speedHash;



    void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        speedHash = Animator.StringToHash("speed");
        isRunningHash = Animator.StringToHash("isRunning");
        isIdleHash = Animator.StringToHash("isIdle");
        
        playerInput.Player.Move.started += OnMovementInput;
        playerInput.Player.Move.canceled += OnMovementInput;
        playerInput.Player.Move.performed += OnMovementInput;
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
        rb.linearVelocity = new Vector2(currentMovement * speed, rb.linearVelocityY);
    }

    void HandleAnimation()
    {
        //getting params from animator
        bool isRunning = animator.GetBool(isRunningHash);
        float speed = animator.GetFloat(speedHash);
        bool isIdle = animator.GetBool(isIdleHash);

        //start walking if movement is pressedd and not already walking.
        if(isMovementPressed && !isRunning)
        {
            animator.SetFloat(speedHash, Mathf.Abs(currentMovement));
            animator.SetBool(isRunningHash, true);
        }
        //stop walking if moovemetn is not pressed and not already walking.
        else if (!isMovementPressed && isRunning)
        {
            animator.SetFloat(speedHash, Mathf.Abs(currentMovement));
            animator.SetBool(isRunningHash, false);
        }
    }
    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement = currentMovementInput.x;
        isMovementPressed = currentMovementInput.x != 0;    
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
    public void Move(InputAction.CallbackContext context) 
    {
        if(context.performed)
        {
            currentMovement = context.ReadValue<Vector2>().x;
        }    
        else if(context.canceled)
        {
            currentMovement = context.ReadValue<Vector2>().x;
        } 
    }

    public void Jump(InputAction.CallbackContext context)
    {
        
    }

#endregion




}
