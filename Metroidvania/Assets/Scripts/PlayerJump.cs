using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{

    [SerializeField]private float jumpForce;
    [SerializeField]private bool grounded;
    [SerializeField]private Transform groundCheck;
    [SerializeField]private float radOfCircle;
    [SerializeField]private LayerMask groundMask;
    private Rigidbody2D rb; 


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, radOfCircle, groundMask);
        if(Input.GetButtonDown("Jump") && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, radOfCircle);
    }
        
    
}
