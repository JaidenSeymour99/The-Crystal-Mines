using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateManager<PlayerStateMachine.PlayerState>
{

    private Animator myAnimator; 

    //making the different possible states for the player to be in.
    public enum PlayerState
    {
        Idle,
        Run,
        Attack,
        Slide,
        Jump, 
        Dash,
        Hurt,
        Die
    }



    // public void Start()
    // {

    // }
    // public void Update()
    // {

    // }


}
