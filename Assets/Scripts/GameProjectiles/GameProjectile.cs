using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProjectile : MonoBehaviour
{
    protected int damage;
    protected float gravityScale;
    protected float speed;
    protected Weapon parent;
    protected Vector2 shootDirection;
    protected Rigidbody2D rb;
    public void Init(int damage, float gravityScale, float speed, Weapon parent,Vector2 shootDirection)
    {
        this.damage = damage;
        this.gravityScale = gravityScale;
        this.speed = speed;
        this.parent = parent;
        this.shootDirection = shootDirection;

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale; 
        rb.velocity = shootDirection.normalized * speed;
    }

    private void Update()
    {
        transform.eulerAngles = new Vector3(0,0, Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg);
    }
    protected virtual void OnHit()
    {
        Destroy(gameObject);
    }

    public virtual void Colliding(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerControls>().TakeDamage(damage);           
        }
        OnHit();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Colliding(collision);
    }
}
