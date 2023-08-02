using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Enemy : Character
{
    [Header("Rigidbody, Animator")]
    private Rigidbody2D rb; 
    private Animator myAnimator; 
    
    #region Overrides


    public override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        base.Start();
    }




    protected override void Attack()
    {
        //detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        //damage them
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Player>().TakeDamage(attackDamage);
            
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        //play hurt anim
        myAnimator.SetTrigger("Hurt");
    }

    public override IEnumerator DisableOnDeath()
    {
        //die animation
        myAnimator.SetBool("IsDead", true);
        yield return new WaitForSeconds(.8f);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
        yield return null;
    }

    #endregion
}
