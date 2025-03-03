using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    
    //public Animator myAnimator;

    public override void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void FixedUpdate()
    {
        base.FixedUpdate();


    }

    private void IsMoving()
    {
        //myAnimator.SetFloat("speed", Mathf.Abs(direction));
        
    }
}
