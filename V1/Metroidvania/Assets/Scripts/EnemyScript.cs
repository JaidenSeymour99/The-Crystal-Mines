using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyScript : MonoBehaviour
{

    private Rigidbody2D rb; 
    private Animator myAnimator; 

    public float maxHealth = 100;
    private float currentHealth; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        //play hurt anim
        myAnimator.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //die animation
        myAnimator.SetBool("IsDead", true);
        //disable enemy
        StartCoroutine(DisableOnDeath());
    }

    IEnumerator DisableOnDeath()
    {
        yield return new WaitForSeconds(.8f);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
        yield return null;
    }
}
