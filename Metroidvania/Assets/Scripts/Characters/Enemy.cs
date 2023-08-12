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
    Collider2D otherTag;

    #region Overrides


    public override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Collider2D otherTag = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
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


    protected void Attack(Collider2D other)
    {
        if(Time.time >= nextAttackTime)
        {
            if(other.gameObject.CompareTag("Player"));
            {

                other.GetComponent<Player>().TakeDamage(attackDamage);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
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
        StartCoroutine(FlashCoroutine());
        
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Attack(other);
    }
}
