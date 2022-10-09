using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : GameProjectile
{
    private bool boom;
    public override void Init(Weapon parent, Vector2 shootDirection)
    {
        base.Init(parent, shootDirection);
        boom = false;
        if (Random.Range(0, 10) == 5)
        {
            GameManager.instance.Glitch(GlitchType.Player);
            boom = true;
            GetComponent<SpriteRenderer>().sprite = Resources.Load<GameObject>("ProjectilePrefabs/RocketBullet").GetComponent<SpriteRenderer>().sprite;
        }
    }

    protected override void OnHit()
    {
        if (boom)
        {
            GameObject explosion = GameObject.Instantiate(Resources.Load<GameObject>("ProjectilePrefabs/Explosion"), transform.position, Quaternion.identity);
            explosion.GetComponent<Explosion>().Init(20, 2, 5);
        }
        Destroy(gameObject);
    }

    public override void Colliding(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerControls>().TakeDamage(damage);
        }
        if (collision.gameObject.tag != "Bullet")
            OnHit();
    }

}
