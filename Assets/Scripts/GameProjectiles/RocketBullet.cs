using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBullet : GameProjectile
{
    bool boomed = false;
    bool bounce = false;



    public override void Colliding(Collision2D collision)
    {
        if (!bounce && !boomed)
        {
            boomed = true;
            GameObject explosion = GameObject.Instantiate(Resources.Load<GameObject>("ProjectilePrefabs/Explosion"), transform.position, Quaternion.identity);
            explosion.GetComponent<Explosion>().Init(damage, 4, 25);
            GetComponent<ParticleSystem>().Stop();
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<PolygonCollider2D>());
            Destroy(gameObject, GetComponent<ParticleSystem>().startLifetime);
        }
        else if(bounce)
        {
            GetComponent<Rigidbody2D>().velocity += -GetComponent<Rigidbody2D>().velocity * 5;
            bounce = false;
        }
        
    }

    public void Glitch()
    {
        GameManager.instance.Glitch(GlitchType.Player);
        bounce = true;
    }
}
