using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBullet : GameProjectile
{
    bool boomed = false;
    bool bounce = false;


    public override void Init(Weapon parent, Vector2 shootDirection)
    {
        base.Init(parent, shootDirection);
        if (Random.Range(0, 10) == 5) Glitch();
    }

    public override void Colliding(Collision2D collision)
    {
        if (!bounce && !boomed && collision.gameObject.tag != "Bullet")
        {
            boomed = true;
            SoundPlayer.instance.PlaySFX(1, 
                Resources.Load<AudioClip>("Sound/SFX/Explosion_01_V01"),
                Resources.Load<AudioClip>("Sound/SFX/Explosion_02_V01"),
                Resources.Load<AudioClip>("Sound/SFX/Explosion_03_V01"));
            
            GameObject explosion = GameObject.Instantiate(Resources.Load<GameObject>("ProjectilePrefabs/Explosion"), transform.position, Quaternion.identity);
            explosion.GetComponent<Explosion>().Init(damage, 4, 25);
            GetComponent<ParticleSystem>().Stop();
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<PolygonCollider2D>());
            Destroy(gameObject, GetComponent<ParticleSystem>().startLifetime);
            OnHit();
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
