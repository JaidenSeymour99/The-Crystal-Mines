using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{

    PlayerBaseState currentState;
    PlayerJumpState JumpState = new();
    PlayerIdleState IdleState = new();
    PlayerMoveState MoveState = new();



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = IdleState;

        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
