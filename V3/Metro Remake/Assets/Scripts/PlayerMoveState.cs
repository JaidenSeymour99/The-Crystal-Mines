using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("move");
        Animator animator = player.GetComponent<Animator>();
        animator.SetFloat("speed", Mathf.Abs(player.direction));
        animator.SetBool("idle", false);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.direction == 0)
        {
            player.SwitchState(player.IdleState);
        }
        else
        {

        }
    }

    public override void OnCollisionEnter(PlayerStateManager player)
    {

    }
}
