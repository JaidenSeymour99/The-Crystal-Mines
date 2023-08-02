using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//dont want to implement a base character so abstract class, want other things to be able to use this freely.
public class Character : MonoBehaviour
{

    [Header("Character Health")]
    [SerializeField] public float maxHealth;
    protected private float currentHealth; 

    [Header("Movement Details")]
    [SerializeField] protected private float maxSpeed = 8.0f;
    [SerializeField] protected private float speed;
    protected private float direction;
    public bool facingRight = true;

    [Header("Jump Details")]
    [SerializeField] protected private float jumpForce = 10.0f;
    protected private float jumps;
    [SerializeField] protected private float maxJumps = 2f;

    [Header("Attack Details")]
    [SerializeField] protected private float attackDamage = 40f;
    [SerializeField] protected private float attackRate = 2f;
    [SerializeField] protected private float attackRange = .8f;
    [SerializeField] protected private Transform attackPoint;
    [SerializeField] protected private LayerMask enemyLayers;
    protected private float nextAttackTime = 0f;
    protected private bool attacking;

    [Header("Ground Details")]
    [SerializeField] protected private float radOfCircle = 0.03f;
    [SerializeField] protected private bool grounded;
    [SerializeField] protected private LayerMask groundMask;
    [SerializeField] protected private Transform groundCheck;

    [Header("Wall Details")]
    [SerializeField] protected private Transform wallCheck;
    [SerializeField] protected private LayerMask wallLayer;



    protected Collider2D[] hitEnemies;

    public virtual void Start()
    {
        speed = maxSpeed;
        attacking = false;
        currentHealth = maxHealth;
    }


    public virtual void Update()
    {
        if(PauseScript.isPaused) return;


    }

    public virtual void FixedUpdate()
    {
        if(PauseScript.isPaused) return;
        
    }

    protected virtual void Attack()
    {
        
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        
        //disable enemy
        StartCoroutine(DisableOnDeath());
    }

    public virtual IEnumerator DisableOnDeath()
    {
        yield return new WaitForSeconds(.8f);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
        yield return null;
    }


    protected private void ChangeDirection()
    {  
        if(!facingRight && direction > 0)
        {
            Flip();
        }
        else if (facingRight && direction < 0)
        {
            Flip();
        }
    }
    //method used to change the direction a rigid body is facing 
    protected virtual void Flip()
    {
        //OLD VERSION OF CHANGING DIRECTION
        //this made the camera not work correctly.

        // facingRight = !facingRight;
        
        // Vector3 theScale = transform.localScale;
        // theScale.x *= -1;
        // transform.localScale = theScale;
        // cameraFollowObject.Turn();

        if(facingRight)
        {
            Vector2 rotator = new Vector2(transform.rotation.x, 180f);
            transform.rotation = Quaternion.Euler(rotator);
            facingRight = !facingRight;
        }
        else
        {
            Vector2 rotator = new Vector2(transform.rotation.x, 0f);
            transform.rotation = Quaternion.Euler(rotator);
            facingRight = !facingRight;
        }

    }
    //bool to check if the player is on the ground. returns true or false.
    protected private bool IsGrounded()
    {
        //drawing a small circle under the rigid body to check if its touching the ground mask. if it is return true. if not return false.
        return Physics2D.OverlapCircle(groundCheck.position, radOfCircle, groundMask);
    }

    //bool. checking for player / wall overlap. returns true or false.
    protected private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, radOfCircle, wallLayer);
    }
}
