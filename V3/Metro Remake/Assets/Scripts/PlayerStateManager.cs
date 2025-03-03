using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{

    public PlayerBaseState currentState;
    public PlayerJumpState JumpState = new();
    public PlayerIdleState IdleState = new();
    public PlayerMoveState MoveState = new();

    [SerializeField] protected private float maxSpeed = 8.0f;
    [SerializeField] protected private float speed;
    public float direction;
    public bool facingRight = true;

    public Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        speed = maxSpeed;
        rb = GetComponent<Rigidbody2D>();

        currentState = IdleState;
        currentState.EnterState(this);

        
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);


    }

    public void Move(InputAction.CallbackContext context) 
    {
        if(context.performed)
        {
            direction = context.ReadValue<Vector2>().x;
            SwitchState(MoveState);
        }    
        else if(context.canceled)
        {
            direction = context.ReadValue<Vector2>().x;
            SwitchState(IdleState);
        } 
    }
    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public virtual void FixedUpdate()
    {
        //controling direction of the player.
        if(direction > 0 || direction < 0) ChangeDirection();
    }


    #region Direction Changes
    protected private void ChangeDirection()
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

    protected void Flip()
    {
        if(facingRight)
        {
            Vector2 rotator = new Vector2(transform.rotation.x, 180f);
            transform.rotation = Quaternion.Euler(rotator);
            facingRight = !facingRight;
        }
        else
        {
            Vector2 rotator = new Vector2(transform.rotation.x, 0f);
            transform.rotation = Quaternion.Euler(rotator);
            facingRight = !facingRight;
        }

    }

#endregion



}
