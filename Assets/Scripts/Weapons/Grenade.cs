using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Weapon
{
    public Grenade() : base(70, 0, 0, 1, Vector2.zero, Resources.Load<Sprite>("WeaponSprites/grenade"), Resources.Load<GameObject>("ProjectilePrefabs/GrenadeBullet"))
    {

    }

    override public void Shoot(Vector2 position, Vector2 shootDirection)
    {
        base.Shoot(position, shootDirection);
        lastProjectile.GetComponent<GameProjectile>().Init(this, shootDirection);

        //SFX SHOOT GRENADE OR SOMTEHING  IDK
    }
}
