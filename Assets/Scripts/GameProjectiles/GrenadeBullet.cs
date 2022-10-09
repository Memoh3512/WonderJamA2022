using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBullet : GameProjectile
{
    public float secondsBeforeExplosion;
    public override void Init(Weapon parent, Vector2 shootDirection)
    {
        base.Init(parent, shootDirection);
        StartCoroutine(Die());
    }

    public override void Colliding(Collision2D collision)
    {
        //override pour pas que ça fasse le base.Colliding do not remove
    }


    IEnumerator Die()
    {
        yield return new WaitForSeconds(secondsBeforeExplosion);
        GameObject explosion = GameObject.Instantiate(Resources.Load<GameObject>("ProjectilePrefabs/Explosion"), transform.position, Quaternion.identity);
        explosion.GetComponent<Explosion>().Init(damage, 4, 10);
        Destroy(gameObject);
    }

 
}
