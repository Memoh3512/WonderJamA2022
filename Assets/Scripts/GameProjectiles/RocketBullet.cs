using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class RocketBullet : GameProjectile
{
    bool boomed = false;
    bool bounce = false;
    public void Init(Weapon parent, Vector2 shootDirection)
    {
        base.Init(30, 0.05f, 3, parent, shootDirection);
    }


    public override void Colliding(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
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
