using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBullet : GameProjectile
{
    public float secondsBeforeExplosion;
    public float knockback;
    public override void Init(Weapon parent, Vector2 shootDirection)
    {
        base.Init(parent, shootDirection);
        StartCoroutine(Die());
    }

    public override void Colliding(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity += (direction * knockback);
        }
    }


    IEnumerator Die()
    {
        yield return new WaitForSeconds(secondsBeforeExplosion);
        GameObject explosion = GameObject.Instantiate(Resources.Load<GameObject>("ProjectilePrefabs/Explosion"), transform.position, Quaternion.identity);
        explosion.GetComponent<Explosion>().Init(damage, 4, 10);
        Destroy(gameObject);
    }

 
}
