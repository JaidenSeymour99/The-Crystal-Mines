using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    //variables needed for animations and physics
    private Rigidbody2D rb2D;
    private Animator myAnimator;

    private bool facingRight = true;



    //variables that control the players movement.
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float horizontalMovement;

    // Start is called before the first frame update
    void Start()
    {
        //defining the gameobjects that the player has
        rb2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    //Update handles the input for the physics
    void Update()
    {
        //check direction inputed 
        horizontalMovement = Input.GetAxis("Horizontal");
    }

    //fixed updated used for running the physics
    private void FixedUpdate()
    {
        //move the character.
        rb2D.velocity = new Vector2(horizontalMovement * speed, rb2D.velocity.y);
        Flip(horizontalMovement);
    }

    //function for flipping the player animation
    //if the player is facing right and moving left this function will flip the sprite so it is looking the direction it is moving
    private void Flip(float horizontal)
    {
        if (horizontal < 0 && facingRight || horizontal > 0 && !facingRight)
        {
            //
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
