using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateManager<PlayerStateMachine.PlayerState>
{
    private Player1 player;

    public bool isDashing { get; private set; }
    public bool isJumping { get; private set; }
    public bool isFalling { get; private set; }
    public bool isWallSliding { get; private set; }
    public bool isWalking { get; private set; }
    public bool isWallJumping { get; private set; }
    

    //making the different possible states for the player to be in.
    public enum PlayerState
    {
        Idle,
        Run,
        Attack,
        Slide,
        Jump, 
        Fall,
        Dash,
        Hurt,
        Die
    }

    void Awake() 
    {
        CurrentState = States[PlayerState.Idle];
    }

    public void Start()
    {
        



    }
    public void Update()
    {

    }

}
