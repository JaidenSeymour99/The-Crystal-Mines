using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{

    Player playerControls;
    BoxCollider2D box;

    private void Awake()
    {
        
        box = GetComponent<BoxCollider2D>();
    }

    //when an object collides with the platform check if it is the player using the compare tag function.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
            playerControls = collision.gameObject.GetComponent<Player>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(playerControls == null)
            return;
        if(playerControls.fallThrough)
        {
            StartCoroutine(DisableCollision());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerControls = null;
        

    }

    private IEnumerator DisableCollision()
    {

        box.enabled = false;
        yield return new WaitForSeconds(0.5f);
        box.enabled = true;
        playerControls = null;

    }
}
