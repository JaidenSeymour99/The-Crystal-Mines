using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class StateManager<Estate> : MonoBehaviour where Estate : Enum
{
    protected Dictionary<Estate, BaseState<Estate>> States = new Dictionary<Estate, BaseState<Estate>>();

    protected BaseState<Estate> CurrentState;
    protected bool IsTransitioningState = false;

    public Animator myAnimator; 
    public Rigidbody2D rb;
    public bool grounded { get; private set; }
    public bool attacking { get; private set; }
    


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        CurrentState.EnterState();
    }

    void Update()
    {
        Estate nextStateKey = CurrentState.GetNextState();
        
        if(!IsTransitioningState && nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        } else if (!IsTransitioningState) 
        {
            TransitionToState(nextStateKey);
        }


    }

    public void TransitionToState(Estate stateKey)
    {
        IsTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
        IsTransitioningState = false;
    }

    void OnTriggerEnter(Collider other){}

    void OnTriggerStay(Collider other){}

    void OnTriggerExit(Collider other){}




}
