using UnityEditor.Tilemaps;
using UnityEngine;

public class Character : MonoBehaviour
{

    [SerializeField] protected private float maxSpeed = 8.0f;
    [SerializeField] protected private float speed;
    public float direction;
    public bool facingRight = true;

    public virtual void Start()
    {
        speed = maxSpeed;
    }

    public virtual void FixedUpdate()
    {
        if(direction > 0 || direction < 0) ChangeDirection();
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

    protected virtual void Flip()
    {
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
}
