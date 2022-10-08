using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBullet : GameProjectile
{
    bool boomed = false;
    public void Init(Weapon parent, Vector2 shootDirection)
    {
        base.Init(30, 0.05f, 2, parent, shootDirection);
    }


    public override void Colliding(Collision2D collision)
    {
        
            GameObject explosion = GameObject.Instantiate(Resources.Load<GameObject>("ProjectilePrefabs/Explosion"), transform.position, Quaternion.identity);
            explosion.GetComponent<Explosion>().Init(damage, 4, 10000);
            Destroy(gameObject);      
        
    }
}
