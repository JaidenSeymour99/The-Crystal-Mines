using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : Player
{   
    
    public Rigidbody2D rb;

    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }
    public override void FixedUpdate() 
    {
        base.FixedUpdate();

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocityY);
    }

#region Actions
    public void Move(InputAction.CallbackContext context) 
    {
        if(context.performed)
        {
            direction = context.ReadValue<Vector2>().x;
        }    
        else if(context.canceled)
        {
            direction = context.ReadValue<Vector2>().x;
        } 
    }

    public void Jump(InputAction.CallbackContext context)
    {
        
    }

#endregion




}
