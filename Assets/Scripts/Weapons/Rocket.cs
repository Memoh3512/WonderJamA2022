using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Weapon
{
    public Rocket() : base(60, 0, 50, 1, Vector2.zero, Resources.Load<Sprite>("WeaponSprites/Rocket"), Resources.Load<GameObject>("ProjectilePrefabs/RocketBullet"),"Rocket Launcher")
    {

    }

    override public void Shoot(Vector2 position, Vector2 shootDirection)
    {
        base.Shoot(position, shootDirection);
        lastProjectile.GetComponent<RocketBullet>().Init(this, shootDirection);
    }

}
