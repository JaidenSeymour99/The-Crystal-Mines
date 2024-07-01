using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateManager<PlayerStateMachine.PlayerState>
{

    //making the different possible states for the player to be in.
    public enum PlayerState
    {
        Idle,
        Run,
        Attack, 
        Slide,
        Jump
    } 
}
