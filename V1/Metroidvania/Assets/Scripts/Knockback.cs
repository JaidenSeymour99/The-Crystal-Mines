using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{

    [SerializeField] private float knockForce;
    [SerializeField] private float knockbackTime;
    [SerializeField] private string otherTag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //checking for the specified tag entering the trigger collider.
        if(other.gameObject.CompareTag(otherTag) && other.isTrigger)
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            //if the rigid body hit has a value.
            if(hit != null)
            {
                //Getting a value for how far the hit rigid body will be knocked.
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * knockForce;
                //adding a force to the hit object
                hit.AddForce(difference, ForceMode2D.Impulse);

                //if the thing entering the trigger collider has the enemy tag.
                if(other.gameObject.CompareTag("Enemy") )
                {
                    //using the knockback function.
                    other.GetComponent<Enemy>().Knockback(hit, knockbackTime);
                }
                //if the thing entering the trigger collider has the player tag.
                if(other.gameObject.CompareTag("Player"))
                {
                    //using the knockback function.
                    other.GetComponent<Player>().Knockback(hit, knockbackTime);
                }
            }
        }
    }



}
