using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateManager<EnemyStateMachine.EnemyState>
{
    //making the different possible states for the enemy to be in.
    public enum EnemyState
    {
        Idle,
        Run,
        Attack,
        Hurt,
        Die
    } 
}
