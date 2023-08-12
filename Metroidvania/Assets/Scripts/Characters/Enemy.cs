using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Enemy : Character
{
    [SerializeField] private string enemyName;
    
    private Transform target;
    [SerializeField] private float chaseRange;
    [SerializeField] private Transform chase;

    [Header("Rigidbody, Animator")]
    private Rigidbody2D rb; 
    private Animator myAnimator; 
    
    #region Overrides


    public override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    public override void Update()
    {
        base.Update();

        if(currentHealth >= 0f) 
        {
            myAnimator.SetBool("Idle", true);
        }
        

    }

    public override void FixedUpdate()
    {
        base.Update();
        Move();
    }

    protected override void Attack()
    {
        



        // //detect enemies
        // Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        // //damage them
        // foreach(Collider2D enemy in hitEnemies)
        // {
        //     enemy.GetComponent<Player>().TakeDamage(attackDamage);
            
        // }
    }
    private void Move()
    {
        if(Vector2.Distance(rb.position, target.position) < chaseRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        //play hurt anim
        
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

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(chase.position, chaseRange);
    }

    #endregion
}
