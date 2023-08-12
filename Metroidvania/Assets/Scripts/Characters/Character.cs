using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//dont want to implement a base character so abstract class, want other things to be able to use this freely.
public class Character : MonoBehaviour
{
    private SpriteRenderer sprite;

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

    

    [Header("IFrame")]
    [SerializeField] private Color flashColour;
    [SerializeField] private Color normalColour;
    [SerializeField] private float flashDuration;
    [SerializeField] private int numberOfFlashes;
    [SerializeField] private Collider2D triggerCollider2D;

    

    public virtual void Start()
    {
        speed = maxSpeed;
        attacking = false;
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
    }


    public virtual void Update()
    {
        if(PauseScript.isPaused) return;
        if(currentHealth <= 0)
        {
            Die();
        }

    }

    public virtual void FixedUpdate()
    {
        if(PauseScript.isPaused) return;
        if(direction > 0 || direction < 0) ChangeDirection();
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

    public virtual void Knockback(Rigidbody2D myRigidbody, float knockbackTime)
    {
        
        StartCoroutine(KnockbackCoroutine(myRigidbody, knockbackTime));
        
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

    protected private IEnumerator FlashCoroutine()
    {
        int flashes = 0;
        triggerCollider2D.enabled = false;
        while(flashes < numberOfFlashes)
        {
            sprite.color = flashColour;
            yield return new WaitForSeconds(flashDuration);
            sprite.color = normalColour;
            yield return new WaitForSeconds(flashDuration);
            flashes++;
        }
        triggerCollider2D.enabled = true;
    }


    protected virtual IEnumerator KnockbackCoroutine(Rigidbody2D myRigidbody, float knockbackTime) 
    {
       if(myRigidbody != null)
        { 
            yield return new WaitForSeconds(knockbackTime);
            myRigidbody.velocity = Vector2.zero;

            //Vector2.zero is short hand for writing Vector2(0,0);
        }
    }


    //used to draw a gizmo that is visible to the editor but not in game.
    protected virtual void OnDrawGizmos()
    {
        if(attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


    

}
