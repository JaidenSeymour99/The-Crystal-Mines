using UnityEditor.Tilemaps;
using UnityEngine;

public class Character : MonoBehaviour
{

    [SerializeField] protected private float maxSpeed = 8.0f;
    [SerializeField] protected private float speed;
    //public Vector2 currentMovement;
    public float currentMovement;
    public bool facingRight = true;

    public virtual void Start()
    {
        speed = maxSpeed;
    }


    public virtual void FixedUpdate()
    {
        if(currentMovement > 0 || currentMovement < 0) ChangeDirection();
    }

#region Direction Changes
protected private void ChangeDirection()
    {  
        if(!facingRight && currentMovement > 0)
        {
            Flip();
        }
        else if (facingRight && currentMovement < 0)
        {
            Flip();
        }
    }

    protected virtual void Flip()
    {
        if(facingRight)
        {
            Vector2 rotator = new(transform.rotation.x, 180f);
            transform.rotation = Quaternion.Euler(rotator);
            facingRight = !facingRight;
        }
        else
        {
            Vector2 rotator = new(transform.rotation.x, 0f);
            transform.rotation = Quaternion.Euler(rotator);
            facingRight = !facingRight;
        }

    }

#endregion



}
