using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {

        Debug.Log("idle");
        Animator animator = player.GetComponent<Animator>();
        animator.SetFloat("speed", Mathf.Abs(player.direction));
        animator.SetBool("idle", true);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.direction != 0)
        {
            player.SwitchState(player.MoveState);
        }
        else
        {

        }
    }

    public override void OnCollisionEnter(PlayerStateManager player)
    {

    }

}
